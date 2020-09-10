using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Net.Sockets;
using System.Net.NetworkInformation;

using Pelco.AccessControlIPC;
using System.Collections.Concurrent;
using XMLUtilities;
using Pelco.AccessControlIPC.Interfaces;
using Pelco.AccessControlIPC.ACSTypes;
using System.Net;
using System.IO;
using System.Drawing;
using System.Reflection;
using Newtonsoft.Json;

namespace VxEvents
{
    class VxEventHandler: IACSServer, IDisposable
    {
        private VxSdkNet.VXSystem _vxSystem = null;
        private object _vxSystemLock = new object();

        private string _systemID = string.Empty;
        private List<VxSdkNet.Monitor> _monitors = null;
        private List<VxSdkNet.DataSource> _datasources = null;
        private List<VxSdkNet.Situation> _situations = null; //total situation list read from Vx
        private List<CustomSituation> _customSituations = null;
        private VideoXpertSettings _vxSettings = null;
        private ACSSettings _acsSettings = null;
        private AccessControlServerSettings _acServerSettings = null;
        private CameraAssociations _cameraAssociations = null;
        private List<Script> _scripts = null;
        private List<EventMap> _eventMap = null;
        private List<VxSdkNet.Situation> _subscriptions = null; // situations we have subscribed to
        private object _subscriptionsLock = new object();
        private bool _needToResubscribe = false;
        private DateTime _nextSubscriptionTime = DateTime.Now + new TimeSpan(0,0,5);  // 5 seconds from now
        List<VxSdkNet.Situation> _subscribeSituations = null;

        private ConcurrentQueue<VxSdkNet.Event> _matchedVxEvents = new ConcurrentQueue<VxSdkNet.Event>();
        private ConcurrentQueue<string> _scriptsToExecute = new ConcurrentQueue<string>();
        #region FakeACS
        private ConcurrentQueue<FakeACSEvent> _fakeACSEvents = new ConcurrentQueue<FakeACSEvent>();
        private bool _createRandomACSEvent = false;
        #endregion
        private int _debugLevel;

        private Thread _refreshDataThread = null;
        private Thread _pollEventsThread = null;
        private Thread _processEventsThread = null;
        private Thread _executeScriptsThread = null;
        private volatile bool _stopping = false;

        private AccessControlIPCServer _ipcServer = null;

        private ACSWrapper _acsWrapper = null;

#region CONSTRUCTOR DESTRUCTOR
        /// <summary>
        /// Constructor
        /// </summary>
        public VxEventHandler(VideoXpertSettings vxSettings,
            ACSSettings acsSettings,
            AccessControlServerSettings acServerSettings,
            CustomSituations customSits, 
            EventConfiguration eventConfiguration,
            CameraAssociations cameraAssociations,
            Scripts scripts)
        {
            try
            {
                _vxSettings = vxSettings;
                _acsSettings = acsSettings;
                _acServerSettings = acServerSettings;
                _cameraAssociations = cameraAssociations;
                if ((scripts != null) && (scripts.scriptArray != null))
                {
                    _scripts = scripts.scriptArray.ToList();
                }

                if (_vxSettings == null)
                {
                    Trace.WriteLine("Configuration Error: VideoXpertSettings");
                }
                if (_acsSettings == null)
                {
                    Trace.WriteLine("Configuration Error: ACSSettings");
                }
                if (_acServerSettings == null)
                {
                    Trace.WriteLine("Configuration Error: AccessControlServerSettings");
                }

                _debugLevel = _vxSettings.DebugLevel;

                if ((customSits != null)&&(customSits.customSituations != null))
                    _customSituations = customSits.customSituations.ToList();
                
                if ((eventConfiguration != null)&&(eventConfiguration.EventMap != null))
                    _eventMap = eventConfiguration.EventMap.ToList();

                // initialize _vxSystem
                InitializeVx();

                _stopping = false;                

                Trace.WriteLineIf(_debugLevel > 0, "Integration host time: " + DateTime.UtcNow.ToString() + " UTC");

                // initialize Access Control System
                InitializeACS();

                // initialize Access Control Server
                InitializeACServer();

                this._refreshDataThread = new Thread(this.RefreshDataThread);
                this._refreshDataThread.Start();

                this._pollEventsThread = new Thread(this.PollEventsThread);
                this._pollEventsThread.Start();

                this._processEventsThread = new Thread(this.ProcessEventsThread);
                this._processEventsThread.Start();

                if (_scripts != null)
                {
                    this._executeScriptsThread = new Thread(this.ExecuteScriptsThread);
                    this._executeScriptsThread.Start();
                }
            }
            catch (Exception exception)
            {
                Trace.WriteLine(string.Format("Error Initializing VxEventHandler {0}\n{1}", exception.Message, exception.StackTrace));
            }
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~VxEventHandler()
        {
            Dispose();
        }
#endregion // CONSTRUCTOR DESTRUCTOR

#region IDisposable        
        public void Dispose()
        {
            // stop monitor thread
            _stopping = true;

            // join it to wait for it to exit
            if (_refreshDataThread != null)
                _refreshDataThread.Join();

            _refreshDataThread = null;

            if (_processEventsThread != null)
                _processEventsThread.Join();

            _processEventsThread = null;

            if (_pollEventsThread != null)
                _pollEventsThread.Join();

            _pollEventsThread = null;

            if (_executeScriptsThread != null)
                _executeScriptsThread.Join();

            _executeScriptsThread = null;

            if (_ipcServer != null)
            {
                _ipcServer.Dispose();
                _ipcServer = null;
            }
        }
#endregion

#region INITIALIZATION OF VXSDK
/// <summary>
/// Get connection to VideoXpert system
/// </summary>
private VxSdkNet.VXSystem GetVxSystem()
        {
            // re-initialize connection if needed
            if ((_vxSystem == null))// || (!_vxCore.IsConnected()))
            {
                ForceReconnect();
            }
            return _vxSystem;
        }

        /// <summary>
        /// Force re-Connect to VideoXpert
        /// </summary>
        private void ForceReconnect()
        {
            lock (_vxSystemLock)
            {
                if (_vxSystem != null)
                {
                    _vxSystem.Dispose();
                    _vxSystem = null;
                }
                _monitors = null;
                _situations = null;
                _datasources = null;
                if (ConnectVxSystem(ref _vxSystem, _vxSettings.GetUsername(), _vxSettings.GetPassword(), _vxSettings.VxCoreAddress, _vxSettings.VxCorePort, true))
                {
                    // any time we reconnect, we need to resubscribe to Vx Events we are interested in
                    _needToResubscribe = true;
                    _nextSubscriptionTime = DateTime.Now.AddSeconds(3);
                }
            }
        }

        /// <summary>
        /// Initialize Connection to VideoXpert
        /// </summary>
        private void InitializeVx()
        {
            lock(_vxSystemLock)
            {
                VxSdkNet.VXSystem system = GetVxSystem();
                if (system == null)
                {
                    Trace.WriteLine("Failed to connect to VideoXpert system at " + _vxSettings.VxCoreAddress);
                }
                else
                {
                    _monitors = system.Monitors;
                    _datasources = system.DataSources;
                    _situations = system.Situations;

                    RegisterAccessControlDevice("GenericAccessControlEventService");

                    LoadCustomSituations();
                }
            }
        }


        /// <summary>
        /// Initialize the local Access Control Server that supplies information to
        /// VideoXpert about the Access Control System we are connected to
        /// </summary>
        private void InitializeACServer()
        {
            if (_ipcServer != null)
            {
                _ipcServer.Dispose();
                _ipcServer = null;
            }

            string ipAddress;
            if (! string.IsNullOrEmpty(_acServerSettings.Address))
            {
                ipAddress = _acServerSettings.Address;
            }
            else ipAddress = GetLocalIPv4();

            if (_acServerSettings.UseSSL)
            {
                Trace.WriteLineIf(_debugLevel > 0, "Initializing AccessControlServer " + ipAddress + " " + _acServerSettings.Port + " " + _acServerSettings.Certificate);
                _ipcServer = new AccessControlIPCServer(this, ipAddress, _acServerSettings.Port, useSSL:true);
            }
            else
            {
                Trace.WriteLineIf(_debugLevel > 0, "Initializing AccessControlServer " + ipAddress + " " + _acServerSettings.Port);
                _ipcServer = new AccessControlIPCServer(this, ipAddress, _acServerSettings.Port, useSSL: false);
            }
        }

        /// <summary>
        /// Initialize the Access Control System
        /// </summary>
        private void InitializeACS()
        {
            _acsWrapper = new ACSWrapper(_acsSettings.HostUrl);
            string acsSysInfo = _acsWrapper.GetSysInfo();
            System.Diagnostics.Trace.WriteLineIf(_debugLevel > 0, "ACSInfo: " + "\r\nHostUrl: " + _acsSettings.HostUrl + "\r\n" + acsSysInfo);
            if (_acsWrapper.Login(_acsSettings.Username, _acsSettings.Password))
            {
                System.Diagnostics.Trace.WriteLineIf(_debugLevel > 0, "Logged into " + _acsSettings.HostUrl);
            }
            else 
            {
                System.Diagnostics.Trace.WriteLineIf(_debugLevel > 0, "Failed to Log into " + _acsSettings.HostUrl);
            }
        }

        /// <summary>
        /// Register this device with VideoXpert as an Access Control device
        /// </summary>
        /// <param name="deviceName">name of device</param>
        private void RegisterAccessControlDevice(string deviceName)
        {
            string localIp = GetLocalIPv4();
            int port = 0;
            if (_acServerSettings != null)
            {
                port = _acServerSettings.Port;
                if (! string.IsNullOrEmpty(_acServerSettings.Address))
                {
                    localIp = _acServerSettings.Address;
                }
            }
            lock (_vxSystemLock)
            {
                VxSdkNet.VXSystem system = GetVxSystem();
                if (system != null)
                {
                    VxSdkNet.Device thisDevice = null;
                    try
                    {
                        List<VxSdkNet.Device> devices = system.Devices;
                        if (devices != null)
                        {
                            thisDevice = devices.Find(x => (x.Ip == localIp && x.Type == VxSdkNet.Device.Types.AccessController && x.Name == deviceName && x.Port == port));
                            if (thisDevice == null)
                            {
                                thisDevice = devices.Find(x => (x.Ip == localIp && x.Type == VxSdkNet.Device.Types.AccessController && x.Name == deviceName && x.Port == 0));
                                if (thisDevice != null)
                                {
                                    Trace.WriteLineIf(_debugLevel > 0, DateTime.Now.ToString() + " Integration device found, Changing integration port to: " + port);
                                    if (port != 0)
                                    {
                                        thisDevice.Port = port;
                                    }
                                }
                            }
                        }
                    }
                    catch { };
                    // if we are not registered, then call AddDevice to register us with the system
                    if (thisDevice == null)
                    {
                        VxSdkNet.Results.Value ret = AddAccessControlDevice(deviceName, localIp);
                        if (ret == VxSdkNet.Results.Value.OK)
                        {
                            try
                            {
                                List<VxSdkNet.Device> devices = system.Devices;
                                if (devices != null)
                                {
                                    thisDevice = devices.Find(x => (x.Ip == localIp && x.Type == VxSdkNet.Device.Types.AccessController && x.Name == deviceName && x.Port == port));
                                }
                            }
                            catch(Exception e)
                            { 
                                Trace.WriteLineIf(_debugLevel > 0, DateTime.Now.ToString() + " Exception in GetDevices: " + e.Message);
                            };
                            if (thisDevice != null)
                            {
                                _vxSettings.IntegrationId = thisDevice.Id;
                                Trace.WriteLineIf(_debugLevel > 0, DateTime.Now.ToString() + " Integration registered with Vx: " + thisDevice.Name + " : " + thisDevice.Id);
                            }
                            else
                            {
                                Trace.WriteLineIf(_debugLevel > 0, DateTime.Now.ToString() + " ERROR: unable to retrieve integrationId from core after registration.");
                            }
                        }
                        else
                        {
                            Trace.WriteLineIf(_debugLevel > 0, DateTime.Now.ToString() + " Failed to register Integration with Vx, return from AddDevice: " + ret);
                        }
                    }
                    else
                    {
                        _vxSettings.IntegrationId = thisDevice.Id;
                        string currentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                        if (thisDevice.Version != currentVersion)
                        {
                            thisDevice.Version = currentVersion;
                        }
                        Trace.WriteLineIf(_debugLevel > 0, DateTime.Now.ToString() + " Integration already registered with Vx: " + thisDevice.Name + " : " + thisDevice.Id);
                    }
                }
            }
        }

        /// <summary>
        /// Add this device as an Access Control Device to VideoXpert
        /// </summary>
        /// <param name="deviceName">name of device</param>
        /// <param name="localIp">localIp of device</param>
        /// <returns>results of add operation</returns>
        private VxSdkNet.Results.Value AddAccessControlDevice(string deviceName, string localIp)
        {
            VxSdkNet.Results.Value ret = VxSdkNet.Results.Value.OperationFailed;

            VxSdkNet.NewDevice extDevice = new VxSdkNet.NewDevice();
            extDevice.Name = deviceName;
            extDevice.Model = "VX-SKU-GENERIC";
            extDevice.Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            // Id cannot be set.  It is assigned by Core
            //extDevice.Id = _settings.IntegrationId;
            if (_acServerSettings != null)
            {
                extDevice.Port = _acServerSettings.Port;
            }
            extDevice.Ip = localIp;
            extDevice.ShouldAutoCommission = true;
            extDevice.Type = VxSdkNet.Device.Types.AccessController;
            lock (_vxSystemLock)
            {
                VxSdkNet.VXSystem system = GetVxSystem();
                if (system != null)
                {
                    ret = system.AddDevice(extDevice);
                }
            }
            return ret;
        }

        /// <summary>
        /// Gets the local Ipv4.
        /// </summary>
        /// <param name="networkInterfaceType">Network interface type.</param>
        /// <returns>String containing the local Ip Address.</returns>
        private string GetLocalIPv4()
        {
            string address = string.Empty;
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                .Where(i => i.OperationalStatus == OperationalStatus.Up);

            foreach (NetworkInterface networkInterface in networkInterfaces)
            {
                var adapterProperties = networkInterface.GetIPProperties();

                if (adapterProperties.GatewayAddresses.FirstOrDefault() != null)
                {
                    foreach (UnicastIPAddressInformation ip in networkInterface.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            address = ip.Address.ToString();
                            break;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(address))
                    break;
            }

            return address;
        }

        /// <summary>
        /// Load custom (external) situations from configuration.  Compare to existing and update if needed
        /// </summary>
        private void LoadCustomSituations()
        {
            if (_customSituations == null)
                return;
            lock (_vxSystemLock)
            {
                VxSdkNet.VXSystem system = GetVxSystem();
                if (system == null)
                    return;

                bool situationsAddedOrModified = false;
                foreach (CustomSituation custSit in _customSituations)
                {
                    VxSdkNet.Situation vxSit = null;
                    try
                    {
                        vxSit = _situations.Find(x => x.Type == custSit.SituationType);
                    }
                    catch { };

                    if (vxSit == null)
                    {
                        if (AddSituation(custSit))
                        {
                            Trace.WriteLineIf(_debugLevel > 0, "Added custom situation: " + custSit.SituationType);
                            situationsAddedOrModified = true; //at least one was added.
                        }
                    }
                    else
                    {
                        bool modified = false;
                        //Custom Sit is already in the system... 
                        // see if our xml version differs and patch each difference
                        if (custSit.AckNeeded != vxSit.IsAckNeeded)
                        {
                            vxSit.IsAckNeeded = custSit.AckNeeded;
                            modified = true;
                        }
                        if (custSit.AutoAcknowledge != vxSit.AutoAcknowledge)
                        {
                            vxSit.AutoAcknowledge = custSit.AutoAcknowledge;
                            modified = true;
                        }
                        if (custSit.Audible != vxSit.ShouldAudiblyNotify)
                        {
                            vxSit.ShouldAudiblyNotify = custSit.Audible;
                            modified = true;
                        }
                        if (custSit.DisplayBanner != vxSit.ShouldPopupBanner)
                        {
                            vxSit.ShouldPopupBanner = custSit.DisplayBanner;
                            modified = true;
                        }
                        if (custSit.ExpandBanner != vxSit.ShouldExpandBanner)
                        {
                            vxSit.ShouldExpandBanner = custSit.ExpandBanner;
                            modified = true;
                        }
                        if (custSit.Log != vxSit.ShouldLog)
                        {
                            vxSit.ShouldLog = custSit.Log;
                            modified = true;
                        }
                        if (custSit.Name != vxSit.Name)
                        {
                            vxSit.Name = custSit.Name;
                            modified = true;
                        }
                        if (custSit.Notify != vxSit.ShouldNotify)
                        {
                            vxSit.ShouldNotify = custSit.Notify;
                            modified = true;
                        }
                        if (custSit.Severity != vxSit.Severity)
                        {
                            vxSit.Severity = custSit.Severity;
                            modified = true;
                        }
                        // VXINT-1123, SourceDeviceId - must delete and re-add if this
                        // changes since VxSituation SourceDeviceId is read only
                        if (! string.IsNullOrEmpty(custSit.SourceDeviceId))
                        {
                            if (custSit.SourceDeviceId == "USE_INTEGRATION_ID")
                            {
                                custSit.SourceDeviceId = _vxSettings.IntegrationId;
                            }
                            if (custSit.SourceDeviceId != vxSit.SourceDeviceId)
                            {
                                system.DeleteSituation(vxSit);
                                AddSituation(custSit);
                                situationsAddedOrModified = true;
                            }
                        }
                        // VXINT-1140, add SnoozeIntervals
                        if ((! string.IsNullOrEmpty(custSit.SnoozeIntervals)) && (! custSit.CompareSnoozeIntervals(vxSit.SnoozeIntervals)))
                        {
                            vxSit.SnoozeIntervals = custSit.GetSnoozeIntervals();
                            modified = true;
                        }

                        if (modified)
                        {
                            Trace.WriteLineIf(_debugLevel > 0, "Modified custom situation: " + vxSit.Type);
                            situationsAddedOrModified = true;
                        }
                    }
                }
                if (situationsAddedOrModified)
                {
                    // refresh situations now that one or more have been added
                    _situations = _vxSystem.Situations;
                }
            }
        }

        /// <summary>
        /// Add a custom (external) situation to VideoXpert
        /// </summary>
        /// <param name="custSit">custom Situation to add</param>
        private bool AddSituation(CustomSituation custSit)
        {
            bool success = false;
            lock (_vxSystemLock)
            {
                VxSdkNet.VXSystem system = GetVxSystem();
                if (system == null)
                    return false;

                VxSdkNet.NewSituation newSit = new VxSdkNet.NewSituation();
                newSit.IsAckNeeded = custSit.AckNeeded;
                //newSit.AudibleLoopDelay = 2;
                newSit.ShouldAudiblyNotify = custSit.Audible;
                //newSit.AudiblePlayCount = 1; //not in custom xml
                newSit.AutoAcknowledge = custSit.AutoAcknowledge;
                newSit.ShouldPopupBanner = custSit.DisplayBanner;
                newSit.ShouldExpandBanner = custSit.ExpandBanner;
                newSit.ShouldLog = custSit.Log;
                newSit.Name = custSit.Name;
                newSit.ShouldNotify = custSit.Notify;
                newSit.Severity = custSit.Severity;
                //newSit.SnoozeIntervals = null;
                // VXINT-1123, add SourceDeviceId
                if (! string.IsNullOrEmpty(custSit.SourceDeviceId))
                {
                    if (custSit.SourceDeviceId == "USE_INTEGRATION_ID")
                        newSit.SourceDeviceId = _vxSettings.IntegrationId;
                    else
                        newSit.SourceDeviceId = custSit.SourceDeviceId;
                }
                // VXINT-1140, add SnoozeIntervals
                if (! string.IsNullOrEmpty(custSit.SnoozeIntervals))
                {
                    newSit.SnoozeIntervals = custSit.GetSnoozeIntervals();
                }
                newSit.Type = custSit.SituationType;
                VxSdkNet.Results.Value addRes = system.AddSituation(newSit);
                if (addRes == VxSdkNet.Results.Value.OK)
                {
                    success = true;
                }
            }
            return success;
        }

        #endregion

        #region THREADS
        /// <summary>
        /// Thread that refreshes data from VideoXpert and reconnects to Vx if necessary
        /// </summary>
        private void RefreshDataThread()
        {
            int sleepInterval = 100;
            int intervalMonitorUpdate = 3 * 60 ; // 3 minutes intervals
            int intervalDataSourceUpdate = 4 * 60; // 4 minutes intervals
            int intervalSituationUpdate = 30 * 60; // 30 minutes interval (should not need to do this)
            int intervalSystemCheck = 2 * 60; // 2 minutes intervals

            TimeSpan monitorTimeSpan = new TimeSpan(0, 0, intervalMonitorUpdate);
            TimeSpan dataSourceTimeSpan = new TimeSpan(0, 0, intervalDataSourceUpdate);
            TimeSpan situationTimeSpan = new TimeSpan(0, 0, intervalSituationUpdate);
            TimeSpan systemCheckTimeSpan = new TimeSpan(0, 0, intervalSystemCheck);

            DateTime nextMonitorUpdate = DateTime.Now + monitorTimeSpan;
            DateTime nextDataSourceUpdate = DateTime.Now + dataSourceTimeSpan;
            DateTime nextSituationUpdate = DateTime.Now + situationTimeSpan;
            DateTime nextSystemCheck = DateTime.Now + systemCheckTimeSpan;

            while (!_stopping)
            {
                Thread.Sleep(sleepInterval);
                DateTime now = DateTime.Now;
                if (now > nextMonitorUpdate)
                {
                    DateTime time = DateTime.Now;
                    Trace.WriteLineIf((_debugLevel > 0), time.ToString() + " Refreshing Monitor Data");
                    List<VxSdkNet.Monitor> monitors = null;
                    try
                    {
                        lock (_vxSystemLock)
                        {
                            VxSdkNet.VXSystem system = GetVxSystem();
                            if (system != null)
                            {
                                monitors = system.Monitors;
                            }
                        }
                    }
                    catch { };

                    if (monitors != null)
                    {
                        DateTime timeUpdate = DateTime.Now;
                        Trace.WriteLineIf((_debugLevel > 0), timeUpdate.ToString() + " Update Monitors " + monitors.Count);
                        lock (_vxSystemLock)
                        {
                            _monitors = monitors;
                        }
                    }
                    nextMonitorUpdate = DateTime.Now + monitorTimeSpan;
                }

                if (now > nextDataSourceUpdate)
                {
                    DateTime time = DateTime.Now;
                    Trace.WriteLineIf((_debugLevel > 0), time.ToString() + " Refreshing DataSources");
                    List<VxSdkNet.DataSource> datasources = null;
                    try
                    {
                        lock (_vxSystemLock)
                        {
                            VxSdkNet.VXSystem system = GetVxSystem();
                            lock (_vxSystemLock)
                            {
                                datasources = system.DataSources;
                            }
                        }
                    }
                    catch { };

                    if (datasources != null)
                    {
                        DateTime timeUpdate = DateTime.Now;
                        Trace.WriteLineIf((_debugLevel > 0), timeUpdate.ToString() + " Update DataSources " + datasources.Count);
                        lock (_vxSystemLock)
                        {
                            _datasources = datasources;
                        }
                    }
                    nextDataSourceUpdate = DateTime.Now + dataSourceTimeSpan;
                }

                if (now > nextSituationUpdate)
                {
                    DateTime time = DateTime.Now;
                    Trace.WriteLineIf((_debugLevel > 0), time.ToString() + " Refreshing Situation Data");
                    List<VxSdkNet.Situation> situations = null;
                    try
                    {
                        lock (_vxSystemLock)
                        {
                            VxSdkNet.VXSystem system = GetVxSystem();
                            if (system != null)
                            {
                                situations = system.Situations;
                            }
                        }
                    }
                    catch { };

                    if (situations != null)
                    {
                        DateTime timeUpdate = DateTime.Now;
                        Trace.WriteLineIf((_debugLevel > 0), timeUpdate.ToString() + " Update Situations " + situations.Count);
                        lock (_vxSystemLock)
                        {
                            _situations = situations;
                        }
                    }
                    nextSituationUpdate = DateTime.Now + situationTimeSpan;
                }

                // Force reconnection?
                if (now > nextSystemCheck)
                {
                    lock (_vxSystemLock)
                    {
                        if (((_datasources == null) || (_datasources.Count == 0)) ||
                         ((_situations == null) || (_situations.Count == 0)))
                        {
                            Trace.WriteLineIf((_debugLevel > 0), "Forcing reconnect to VideoXpert");
                            ForceReconnect();
                            nextMonitorUpdate = DateTime.Now;
                            nextDataSourceUpdate = DateTime.Now;
                            nextSituationUpdate = DateTime.Now;
                        }
                    }

                    nextSystemCheck = DateTime.Now + systemCheckTimeSpan;
                }
            }
        }

        /// <summary>
        /// Thread that polls for events from the ACS system
        /// </summary>
        private void PollEventsThread()
        {
            // todo: determine appropriate interval for polling events or checking event queue from Access Control System
            int sleepInterval = 1000;

            while (!_stopping)
            {
                Thread.Sleep(sleepInterval);
                DateTime now = DateTime.Now;

                // if we need to re-subscribe to VideoXpert events, this is a convenient place to do it
                if (_needToResubscribe)
                {
                    if (DateTime.Now > _nextSubscriptionTime)
                        this.SubscribeToVxEvents();
                }

                // todo: 
                // 1.  Poll or check event queue from Access Control System
                // 2.  Determine if event is mapped to be injected into VideoXpert
                // 3.  Queue mapped events for processing in ProcessEventsThread

                #region FakeACS
                // for test purposes, create fake ACS Event if flagged
                if (_createRandomACSEvent)
                {
                    _createRandomACSEvent = false;
                    FakeACSEvent fakeEvent = _acsWrapper.CreateFakeEvent();
                    _fakeACSEvents.Enqueue(fakeEvent);
                }
                #endregion
            }
        }

        /// <summary>
        /// Thread that processes queued events from both the ACS system and VideoXpert
        /// </summary>
        private void ProcessEventsThread()
        {
            int sleepInterval = 50;

            while (!_stopping)
            {
                // todo: Process Events from Access Control System that have been mapped to VideoXpert

                // 1.  Find EventMap associated to Event

                // 2.  Find Situation to inject into VideoXpert
                // events may be mapped to Custom Situations or to the pre-defined Access Control Situations
                // pre-defined Access Control Situations include:
                // "system/access_door_closed"
                // "system/access_door_faulted"
                // "system/access_door_forced"
                // "system/access_door_locked"
                // "system/access_door_opened"
                // "system/access_door_propped"
                // "system/access_door_unknown"
                // "system/access_door_unlocked"
                // "system/access_denied"
                // "system/access_granted"

                // 3.  Create NewEvent from Mapping with desired properties

                // 4.  Call ForwardVxEvent(NewEvent)

                // 5.  Queue up any local Scripts that have been associated with the Event to be ran by ExecuteScriptsThread
                //     by calling QueueScriptsForExecution or QueueScriptsForAcknowledgement

                #region FakeACS
                FakeACSEvent fakeEvent;
                if (_fakeACSEvents.TryDequeue(out fakeEvent))
                {
                    ProcessACSEvent(fakeEvent);
                }
                #endregion

                // Process Events from VideoXpert
                if (!_matchedVxEvents.IsEmpty)
                {
                    VxSdkNet.Event eventFromVx;
                    if (_matchedVxEvents.TryDequeue(out eventFromVx))
                    {
                        ProcessVideoXpertEvent(eventFromVx);
                    }
                }

                // todo: determine an appropriate interval or other method of waking
                Thread.Sleep(sleepInterval);
            }
        }

        /// <summary>
        /// Thread that executes scripts that have been queued for execution
        /// </summary>
        private void ExecuteScriptsThread()
        {
            int sleepInterval = 50;

            while (!_stopping)
            {
                if (! _scriptsToExecute.IsEmpty)
                {
                    string scriptNumber;
                    if (_scriptsToExecute.TryDequeue(out scriptNumber))
                    {
                        ExecuteScript(scriptNumber);
                    }
                }
                else
                {
                    Thread.Sleep(sleepInterval);
                }
            }
        }

        #endregion // THREADS

        #region THREAD SUPPORT

        /// <summary>
        /// Queues scripts from the EventMap for execution
        /// </summary>
        /// <param name="eventMap">eventMap containing scripts</param>
        private void QueueScriptsForExecution(EventMap eventMap)
        {
            if (! string.IsNullOrEmpty(eventMap.RunScripts))
            {
                string[] scriptsToRun = eventMap.RunScripts.Split(',');
                foreach(string scriptNumberString in scriptsToRun)
                {
                    try
                    {
                        _scriptsToExecute.Enqueue(scriptNumberString);
                    }
                    catch {};
                }
            }
        }

        /// <summary>
        /// Queues acknowledge scripts from the EventMap for execution
        /// </summary>
        /// <param name="eventMap">eventMap containing scripts</param>
        private void QueueScriptsForAcknowledgement(EventMap eventMap)
        {
            if (! string.IsNullOrEmpty(eventMap.AckScripts))
            {
                string[] ackScriptsToRun = eventMap.AckScripts.Split(',');
                foreach(string scriptNumberString in ackScriptsToRun)
                {
                    try
                    {
                        _scriptsToExecute.Enqueue(scriptNumberString);
                    }
                    catch {};
                }
            }
        }

        /// <summary>
        /// Processes an event from the ACS system that has been determined to match configuration
        /// </summary>
        /// <param name="acsEvent">event from ACS</param>
        private void ProcessACSEvent(FakeACSEvent acsEvent)
        {
            try
            {
                foreach (var eventMap in _eventMap)
                {
                    if (eventMap.ACSEvent == acsEvent.EventType)
                    {
                        if (eventMap.Direction.Contains("Video")) // to VideoXpert
                        {
                            VxSdkNet.NewEvent newEvent = new VxSdkNet.NewEvent();
                            newEvent.SituationType = eventMap.VxSituation.Type;
                            newEvent.GeneratorDeviceId = _vxSettings.IntegrationId;
                            newEvent.SourceDeviceId = _vxSettings.IntegrationId;
                            newEvent.Time = DateTime.UtcNow;
                            List<KeyValuePair<string, string>> properties = new List<KeyValuePair<string, string>>();
                            Dictionary<string, string> dataProperties = new Dictionary<string, string>();
                            if (newEvent.SituationType.Contains("system/access_"))
                            {
                                // all these situations need properties "access_point_id" and "access_point_name"
                                KeyValuePair<string, string> kvpId = new KeyValuePair<string, string>("access_point_id", acsEvent.Door.DoorId);
                                properties.Add(kvpId);
                                KeyValuePair<string, string> kvpName = new KeyValuePair<string, string>("access_point_name", acsEvent.Door.Name);
                                properties.Add(kvpName);
                            }
                            // all other properties go under _data as JSON object of kvp values
                            else
                            {
                                dataProperties.Add("door_name", acsEvent.Door.Name);
                                dataProperties.Add("door_id", acsEvent.Door.DoorId);
                            }
                            if (acsEvent.User != null)
                            {
                                dataProperties.Add("card_id", acsEvent.User.CardNumber.ToString());
                                dataProperties.Add("user_id", acsEvent.User.UserId);
                                dataProperties.Add("user_name", acsEvent.User.Name);
                            }
                            List<string> cameraAssociations = GetCameraAssociations(acsEvent.EventType, acsEvent.Door.DoorId);
                            int i = 0;
                            foreach (var cameraId in cameraAssociations)
                            {
                                string key = "data_source_id";
                                if (i > 0)
                                {
                                    key = key + "_" + i;
                                }
                                dataProperties.Add(key, cameraId);
                            }
                            // encode dataProperties to JSON string and place in properties with key _data
                            if (dataProperties.Count > 0)
                            {
                                string dataProps = JsonConvert.SerializeObject(dataProperties);
                                KeyValuePair<string, string> kvpDataProps = new KeyValuePair<string, string>("_data", dataProps);
                                properties.Add(kvpDataProps);
                            }

                            if (properties.Count > 0)
                                newEvent.Properties = properties;

                            ForwardEventToVx(newEvent);
                        }
                        else // to ACS
                        {
                            // todo:
                        }
                        // Queue any scripts associated to the event for execution
                        // todo: determine if ack or not
                        // if (this is ack)
                        //      QueueScriptsForAcknowledgement(eventMap);
                        // else
                        QueueScriptsForExecution(eventMap);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLineIf(_debugLevel > 0, "Exception in ProcessACSEvent: " + ex.Message);
            }
        }

        /// <summary>
        /// Processes an event from VideoXpert that has been determined to match configuration
        /// </summary>
        /// <param name="eventFromVx">event from VideoXpert</param>
        private void ProcessVideoXpertEvent(VxSdkNet.Event eventFromVx)
        {
            try
            {
                List<EventMap> matchingEvents = _eventMap.FindAll(x => (x.VxSituation.Type == eventFromVx.SituationType));
                if (matchingEvents != null)
                {
                    // set up properties to inject with event (only do it once for each)
                    string k1 = string.Empty;
                    string v1 = string.Empty;
                    string k2 = string.Empty;
                    string v2 = string.Empty;
                    string k3 = string.Empty;
                    string v3 = string.Empty;
                    // allow first 3 properties to be sent
                    if (eventFromVx.Properties.Count > 0)
                    {
                        var prop = eventFromVx.Properties[0];
                        k1 = prop.Key;
                        v1 = prop.Value;
                    }
                    if (eventFromVx.Properties.Count > 1)
                    {
                        var prop = eventFromVx.Properties[1];
                        k2 = prop.Key;
                        v2 = prop.Value;
                    }
                    if (eventFromVx.Properties.Count > 2)
                    {
                        var prop = eventFromVx.Properties[2];
                        k2 = prop.Key;
                        v2 = prop.Value;
                    }

                    foreach(EventMap eventMap in matchingEvents)
                    {
                        if (_acsWrapper != null)
                        {
                            try
                            {
                                if (eventFromVx.IsInitial)
                                {
                                    if (eventMap.Direction.Contains("ACS")) // "To ACS"
                                    {
                                        // inject event into ACS
                                        Trace.WriteLineIf(_debugLevel > 0, "Initial event detected, injecting " + eventFromVx.SituationType + " into ACS");
                                        // todo: inject into ACS

                                        // Queue any scripts associated to the event for execution
                                        QueueScriptsForExecution(eventMap);
                                    }
                                }
                                else if ((eventFromVx.AckState == VxSdkNet.Event.AckStates.Acked)||(eventFromVx.AckState == VxSdkNet.Event.AckStates.AutoAcked))
                                {
                                    // this is an acknowledgement from VideoXpert, check if this was an event we may have injected
                                    // and if so, forward the acknowledgement to ACS
                                    List<string> forKeys = new List<string>();
                                    if (eventMap.Direction.Contains("Video")) // if event originated in ACS and is "To VideoXpert"
                                    {
                                        // todo: need to find originating event from ACS
                                    }
                                    else
                                    {
                                        // todo: if originating event came from VideXpert, search for the a matching event
                                        // in ACS to Ack
                                    }

                                    //if (eventMap.Direction.Contains("ACS")) // "To ACS"
                                    {
                                        // todo: search for it in ACS
                                        DateTime? startOnTime = eventFromVx.Time - new TimeSpan(0,0,10);
                                        // look up event from eventmap and search for the originating event to ack
                                        Trace.WriteLineIf(_debugLevel > 1, "Searching for matching event in ACS");
                                        Trace.WriteLineIf(_debugLevel > 1, "   Situation time: " + eventFromVx.Time.ToString());
                                        Trace.WriteLineIf(_debugLevel > 1, "   Search start time: " + startOnTime.ToString());

                                        // todo: if found and no acknowledgements have been made, ack it

                                    }
                                }
                                else
                                {
                                    if (eventMap.Direction.Contains("ACS")) // "To ACS"
                                    {
                                        // todo: inject event into ACS
                                        Trace.WriteLineIf(_debugLevel > 0, "Injecting " + eventFromVx.SituationType + " into ACS");

                                        QueueScriptsForExecution(eventMap);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Trace.WriteLineIf(_debugLevel > 0, "Exception in ProcessVideoXpertEvent for " + eventFromVx.SituationType + " : " + ex.Message);
                            };
                            break; // only insert 1 event per situation
                        }
                    }
                }
                else
                {
                    Trace.WriteLineIf(_debugLevel >0, "ProcessEventsThread discarding non-matching event " + eventFromVx.SituationType);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLineIf(_debugLevel >0, "Exception in ProcessEventsThread: " + ex.Message);
            }
        }

        /// <summary>
        /// Gets the list of camera Ids associated with an ACS peripheral (door) and event
        /// </summary>
        /// <param name="eventKey">Id of event</param>
        /// <param name="peripheralKey">Id of peripheral (door)</param>
        /// <returns>List of associated camera ids.</returns>
        private List<string> GetCameraAssociations(string eventKey, string peripheralKey)
        {
            List<string> cameraIds = new List<string>();
            if ((_cameraAssociations != null)&&(_cameraAssociations.CameraAssociation != null))
            {
                foreach(CameraAssociation association in _cameraAssociations.CameraAssociation)
                {
                    if (association.ACSEvent == eventKey)
                    {
                        if (! string.IsNullOrEmpty(association.ACSPeripheral))
                        {
                            if (peripheralKey == association.ACSPeripheral)
                            {
                                cameraIds.Add(association.CameraId);
                            }
                        }
                        else // no peripheral so association is to all events of the same type
                        {
                            cameraIds.Add(association.CameraId);
                        }
                    }
                }
            }
            return cameraIds;
        }

        /// <summary>
        /// Subscribe to configured Events from VideoXpert
        /// </summary>
        private void SubscribeToVxEvents()
        {
            lock (_vxSystemLock)
            {
                if (_vxSystem == null)
                    return;

                // Build the list of situation types to subscribe to
                List<VxSdkNet.Situation> subSituations = GetSituationListForSubscription();

                if ((subSituations != null) && (subSituations.Count > 0))
                {
                    // Subscribe to system events using the situation types as a filter.
                    var result = _vxSystem.SubscribeToEventsByType(this.OnVxSystemEvent, subSituations);

                    Trace.WriteLineIf(_debugLevel > 0, "\nSubscription result:");
                    Trace.WriteLineIf(_debugLevel > 0, "   result: " + result.ToString());
                    if (_debugLevel > 0)
                    {
                        foreach (VxSdkNet.Situation sit in subSituations)
                            Trace.WriteLine("   situation: " + sit.Type);
                    }

                    if (result.ToString().ToUpper().Contains("OK"))
                    {
                        lock (_subscriptionsLock)
                        {
                            _needToResubscribe = false;
                            if (_subscriptions != null)
                                _subscriptions.Clear(); // remove all subscription information
                            _subscriptions = subSituations;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the list of situations we need to subscribe to
        /// </summary>
        /// <returns>List of situations to subscribe to</returns>
        private List<VxSdkNet.Situation> GetSituationListForSubscription()
        {
            // Build the list of situation types to subscribe to
            if (_subscribeSituations != null)
                return _subscribeSituations;
            
            _subscribeSituations = new List<VxSdkNet.Situation>();

            if (_eventMap != null)
            {
                foreach (EventMap eventMap in _eventMap)
                {
                    try
                    {
                        VxSdkNet.Situation sit = _situations.Find(x => x.Type == eventMap.VxSituation.Type);
                        if (!_subscribeSituations.Contains(sit))
                        {
                            Trace.WriteLineIf(_debugLevel > 0, "Adding situation to subscriptions: " + sit.Type);
                            _subscribeSituations.Add(sit);
                        }
                        else Trace.WriteLineIf(_debugLevel > 2, "Subscription situation already exists: " + sit.Type);
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLineIf(_debugLevel > 1, "GetSituationListForSubscription - Can't find situation in Vx: " + e.Message);
                    };
                }
            }

            return _subscribeSituations;
        }

        /// <summary>
        /// The OnSystemEvent method.  Callback for Vx Events we are subscribed to.
        /// </summary>
        /// <param name="systemEvent">The <paramref name="systemEvent"/> parameter.</param>
        public void OnVxSystemEvent(VxSdkNet.Event systemEvent)
        {
            Trace.WriteLineIf(_debugLevel > 0, "\nVx Event Received");
            Trace.WriteLineIf(_debugLevel > 0, "   situation : " + systemEvent.SituationType);
            Trace.WriteLineIf(_debugLevel > 1, "   sourceDevice : " + systemEvent.SourceDeviceId);
            Trace.WriteLineIf(_debugLevel > 1, "   AckState : " + systemEvent.AckState);
            Trace.WriteLineIf(_debugLevel > 1, "   AckTime : " + systemEvent.AckTime);
            Trace.WriteLineIf(_debugLevel > 1, "   Time : " + systemEvent.Time.ToString() + " UTC");

            if (_eventMap != null)
            {
                foreach (EventMap eventMap in _eventMap)
                {
                    bool isMatchingAck = false;

                    try
                    {
                        if (eventMap.VxSituation.Type == systemEvent.SituationType)
                        {
                            // if situation properties not defined or matched, we found situation
                            if (SituationMatch(eventMap.VxSituation, systemEvent, ref isMatchingAck))
                            {
                                // queue VideoXpert situation to forward to AccessControlSystem
                                _matchedVxEvents.Enqueue(systemEvent);
                                break; // only need to add once, processing takes care of multiple mappings
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLineIf(_debugLevel > 0, "Exception in OnVxSystemEvent: " + e.Message);
                    };
                }
            }
        }

        /// <summary>
        /// Check whether or not a situation from the eventMap matches an event received from Vx
        /// </summary>
        /// <param name="situation">situation from eventMap</param>
        /// <param name="systemEvent">Event from VideoXpert</param>
        /// <param name="isMatchingAcknowledgement">reference set when the match is an Ack</param>
        /// <returns>true if situation matches event</returns>
        private bool SituationMatch(VxSituation situation, VxSdkNet.Event systemEvent, ref bool isMatchingAcknowledgement)
        {
            bool match = true;
            if ((situation.properties != null) && (situation.properties.Count() > 0))
            {
                // each property defined must be found in event from Vx
                foreach (VxSituationProperty property in situation.properties)
                {
                    match = false; // must find each property
                    KeyValuePair<string, string> kvpToMatch = new KeyValuePair<string, string>(property.Key, property.Value);
                    foreach(KeyValuePair<string, string> kvp in systemEvent.Properties)
                    {
                        // key must match exactly
                        if (kvp.Key == kvpToMatch.Key)
                        {
                            // value must be at least contained within event (not exact match)
                            // since this is usually a uuid of a camera and the end is :video
                            if (kvp.Value.Contains(kvpToMatch.Value))
                            {
                                match = true;
                                break; // out of inner foreach
                            }
                        }
                    }
                }
            }
            // if still a match, check for ackstate
            if (match)
            {
                if (! string.IsNullOrEmpty(situation.AckState))
                {
                    string vxAckState = systemEvent.AckState.ToString();
                    if (situation.AckState.ToUpper() != vxAckState.ToUpper())
                        match = false;
                }
            }

            // if still a match, set whether or not it is an Ack
            if (match)
            {
                // if AckState of AckNeeded or NoAckNeeded, everything else is Acknowledgement
                if ((systemEvent.AckState == VxSdkNet.Event.AckStates.Acked)||(systemEvent.AckState == VxSdkNet.Event.AckStates.AutoAcked))
                {
                    isMatchingAcknowledgement = true;
                }
            }

            return match;
        }
        #endregion

        #region VxSDK communications
        private bool ConnectVxSystem(ref VxSdkNet.VXSystem vxSystem, string userName, string password, string ipAddress, int port, bool useSSL)
        {
            bool status = false;
            vxSystem = new VxSdkNet.VXSystem(ipAddress, port, useSSL, "DCovPywTKiY5LgolLiYsKCI/MywlBRUTdxAAD24MHw0OCAoI");

            var result = vxSystem.Login(userName, password);
            if (result == VxSdkNet.Results.Value.OK)
            {
                Trace.WriteLineIf(_debugLevel > 0, DateTime.Now.ToString() + " Logged into VideoXpert at " + ipAddress);
                status = true;
            }
            else if (result == VxSdkNet.Results.Value.SdkLicenseGracePeriodActive)
            {
                Trace.WriteLineIf(_debugLevel > 0, DateTime.Now.ToString() + " Logged into VideoXpert at " + ipAddress + " Warning: License Grace period active");
                if (vxSystem != null)
                {
                    DateTime expirationTime = vxSystem.GraceLicenseExpirationTime;
                    var message = string.Format("This system has not been licensed to run this integration.  The grace period for this license is active but will"
                        + " expire on {0} at {1}.  This integration will cease to function if the system remains unlicensed when the grace period expires.",
                        expirationTime.ToLocalTime().ToShortDateString(),
                        expirationTime.ToLocalTime().ToShortTimeString());
                    Trace.WriteLine(message);
                    status = true;
                }
                else
                {
                    Trace.WriteLineIf(_debugLevel > 0, DateTime.Now.ToString() + " Unable to log user " + userName + " into VideoXpert at " + ipAddress);
                }
            }
            else
            {
                Trace.WriteLineIf(_debugLevel > 0, DateTime.Now.ToString() + " Unable to log user " + userName + " into VideoXpert at " + ipAddress);
                Trace.WriteLineIf(_debugLevel > 2, "   Password " + password);
                Trace.WriteLineIf(_debugLevel > 2, "   result " + result);
                if ((result == VxSdkNet.Results.Value.LicenseRequired) ||
                    (result == VxSdkNet.Results.Value.NoLicense) ||
                    (result == VxSdkNet.Results.Value.SdkLicenseKeyEmpty) ||
                    (result == VxSdkNet.Results.Value.SdkLicenseKeyInvalid) ||
                    (result == VxSdkNet.Results.Value.SdkLicenseVersionInvalid) ||
                    (result == VxSdkNet.Results.Value.SdkLicenseExpired) ||
                    (result == VxSdkNet.Results.Value.SdkLicenseGracePeriodExpired))
                {
                    Trace.WriteLine(DateTime.Now.ToString() + " License is required or expired.  Code: " + result.ToString() + " Please verify the system contains a valid license for the integration");
                }

                status = false;
                vxSystem = null;
            }

            return status;
        }

        private bool ForwardEventToVx(VxSdkNet.NewEvent newEvent)
        {
            bool retVal = false;
            VxSdkNet.Situation situation = null;
            lock (_vxSystemLock)
            {
                try
                {
                    situation = _situations.Find(x => x.Type == newEvent.SituationType);
                }
                catch
                {
                    Trace.WriteLineIf((_debugLevel > 0), "Situation " + newEvent.SituationType + " not found.");
                };

                if (situation != null)
                {
                    Trace.WriteLineIf((_debugLevel > 0), "\nInjecting Vx Event");
                    Trace.WriteLineIf((_debugLevel > 0), "   GeneratorDeviceId: " + newEvent.GeneratorDeviceId);
                    Trace.WriteLineIf((_debugLevel > 0), "   SourceDeviceId   : " + newEvent.SourceDeviceId);
                    Trace.WriteLineIf((_debugLevel > 0), "   Situation        : " + newEvent.SituationType);
                    if (_debugLevel > 0)
                    {
                        foreach (var prop in newEvent.Properties)
                        {
                            Trace.WriteLine("   Property         : " + prop.Key + " , " + prop.Value);
                        }
                    }

                    VxSdkNet.Results.Value result = VxSdkNet.Results.Value.UnknownError;
                    lock (_vxSystemLock)
                    {
                        VxSdkNet.VXSystem system = GetVxSystem();
                        if (system != null)
                            result = system.InjectEvent(newEvent);
                    }
                    if (result != VxSdkNet.Results.Value.OK)
                    {
                        Trace.WriteLineIf((_debugLevel > 0), "ForwardEventToVx failed to inject event into Vx: " + result);
                    }
                    else retVal = true;
                }
            }

            return retVal;
        }
        #endregion

        #region SCRIPTING SUPPORT
        void ExecuteScript(string scriptNumber)
        {
            try
            {
                Script script = _scripts.Find(x => x.Number == scriptNumber);
                if (script != null)
                {
                    foreach (XMLUtilities.Action action in script.Actions)
                    {
                        switch (action.Name.ToLower())
                        {
                            case "setlayout":
                                {
                                    int mon = Convert.ToInt32(action.Monitor);
                                    VxSdkNet.Monitor.Layouts layout = StringToMonitorLayout(action.Layout);
                                    Trace.WriteLineIf((_debugLevel > 0), "Script: SetLayout " + action.Layout + " On Monitor " + mon);
                                    SetLayout(mon, layout);
                                    Trace.WriteLineIf((_debugLevel > 0), "Script: SetLayout Complete");
                                    break;
                                }
                            case "displaycamera":
                                {
                                    int mon = Convert.ToInt32(action.Monitor);
                                    int cell = Convert.ToInt32(action.Cell);
                                    if (cell > 0) // cell is 0 based, so subtract 1
                                        cell--;
                                    int camera = Convert.ToInt32(action.Camera);
                                    int previousSeconds = 0;
                                    if (!string.IsNullOrEmpty(action.PreviousSeconds))
                                        previousSeconds = Convert.ToInt32(action.PreviousSeconds);
                                    Trace.WriteLineIf((_debugLevel > 0), "Script: DisplayCamera " + camera + " On Monitor " + mon + " in Cell " + (cell + 1));
                                    DisplayCameraOnMonitor(camera, mon, cell, previousSeconds);
                                    Trace.WriteLineIf((_debugLevel > 0), "Script: DisplayCamera Complete");
                                    break;
                                }
                            case "disconnectcamera":
                                {
                                    int mon = Convert.ToInt32(action.Monitor);
                                    int cell = Convert.ToInt32(action.Cell);
                                    if (cell > 0) // cell is 0 based, so subtract 1
                                        cell--;
                                    Trace.WriteLineIf((_debugLevel > 0), "Script: Disconnect Camera from Monitor " + mon + " in Cell " + (cell + 1));
                                    Disconnect(mon, cell);
                                    Trace.WriteLineIf((_debugLevel > 0), "Script: Disconnect Complete");
                                    break;
                                }
                            case "gotopreset":
                                {
                                    int camera = Convert.ToInt32(action.Camera);
                                    Trace.WriteLineIf((_debugLevel > 0), "Script: Camera " + camera + " GotoPreset " + action.Preset);
                                    SendGotoPreset(camera, action.Preset);
                                    Trace.WriteLineIf((_debugLevel > 0), "Script: GotoPreset Complete");
                                    break;
                                }
                            case "runpattern":
                                {
                                    int camera = Convert.ToInt32(action.Camera);
                                    Trace.WriteLineIf((_debugLevel > 0), "Script: Camera " + camera + " RunPattern " + action.Pattern);
                                    SendGotoPattern(camera, action.Pattern);
                                    Trace.WriteLineIf((_debugLevel > 0), "Script: RunPattern Complete");
                                    break;
                                }
                            case "bookmark":
                                {
                                    int camera = Convert.ToInt32(action.Camera);
                                    string description = action.Description;
                                    if (string.IsNullOrEmpty(description))
                                    {
                                        description = "Script " + scriptNumber + " BookMark";
                                    }
                                    Trace.WriteLineIf((_debugLevel > 0), "Script: BookMark " + camera + " Description " + description);
                                    CreateBookmark(camera, description);
                                    Trace.WriteLineIf((_debugLevel > 0), "Script: BookMark Complete");
                                    break;
                                }
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine("Exception in ExecuteScript " + scriptNumber + ":" + e.Message);
            }
        }

        private bool SetLayout(int monitorNumber, VxSdkNet.Monitor.Layouts layout)
        {
            bool retVal = false;
            lock (_vxSystemLock)
            {
                int cell = 0;
                VxSdkNet.Monitor monitor = GetMonitor(monitorNumber, ref cell);
                if (monitor != null)
                {
                    monitor.Layout = layout;
                    retVal = true;
                }
            }
            return retVal;
        }

        private void GoToLive(int cameraNumber, int monitorNumber, int cell)
        {
            // can't set DateTime to null, so set it equal to DateTime.MinValue
            Seek(cameraNumber, monitorNumber, cell, new DateTime());
        }

        private void Disconnect(int monitorNumber, int cellNumber)
        {
            lock (_vxSystemLock)
            {
                int cell = cellNumber;
                // possibly overwrites cell if MonitorToCellMap in use
                VxSdkNet.Monitor monitor = GetMonitor(monitorNumber, ref cell);
                if (monitor != null)
                {
                    if (monitor.MonitorCells.Count > cell)
                    {
                        VxSdkNet.MonitorCell monCell = monitor.MonitorCells[cell];
                        monCell.Disconnect();
                    }
                }
            }
        }

        private void Seek(int cameraNumber, int monitorNumber, int cellNumber, DateTime time)
        {
            lock (_vxSystemLock)
            {
                VxSdkNet.DataSource camera = GetCamera(cameraNumber);
                int cell = cellNumber;
                // possibly overwrites cell if MonitorToCellMap in use
                VxSdkNet.Monitor monitor = GetMonitor(monitorNumber, ref cell);
                if (camera != null && monitor != null)
                {
                    if (monitor.MonitorCells.Count > cell)
                    {
                        VxSdkNet.MonitorCell monCell = monitor.MonitorCells[cell];
                        // DateTime.MinValue used to signal going to live
                        if (time == DateTime.MinValue)
                        {
                            monCell.GoToLive();
                            Trace.WriteLineIf(_debugLevel > 0, "Camera " + cameraNumber + " LIVE");
                        }
                        else
                        {
                            DateTime temptime = monCell.Time;
                            monCell.Time = time;
                            Trace.WriteLineIf(_debugLevel > 0, "Camera " + cameraNumber + " SEEK TO " + time.ToString());
                            Trace.WriteLineIf(_debugLevel > 0, "   Previous time: " + temptime.ToString());
                        }
                    }
                    else
                    {
                        Trace.WriteLineIf(_debugLevel > 0, "Unable to Seek Camera " + cameraNumber + " to " + time.ToString());
                    }
                }
                else
                {
                    Trace.WriteLineIf(_debugLevel > 0, "Unable to Seek Camera " + cameraNumber + " on Monitor " + monitorNumber);
                }
            }
        }

        private void ChangePlaySpeed(int cameraNumber, int monitorNumber, int cellNumber, int speed)
        {
            lock (_vxSystemLock)
            {
                VxSdkNet.DataSource camera = GetCamera(cameraNumber);
                int cell = cellNumber;
                // possibly overwrites cell if MonitorToCellMap in use
                VxSdkNet.Monitor monitor = GetMonitor(monitorNumber, ref cell);
                if (camera != null && monitor != null)
                {
                    if (monitor.MonitorCells.Count > cell)
                    {
                        VxSdkNet.MonitorCell monCell = monitor.MonitorCells[cell];
                        monCell.Speed = speed;
                    }
                    else
                    {
                        Trace.WriteLineIf(_debugLevel > 0, "Unable to change Camera " + cameraNumber + " speed to " + speed);
                    }
                }
                else
                {
                    Trace.WriteLineIf(_debugLevel > 0, "Unable to change Camera " + cameraNumber + " speed to " + speed + " on monitor " + monitorNumber);
                }
            }
        }

        private void DisplayCameraOnMonitor(int cameraNumber, int monitorNumber, int cellNumber = 0, int previousSeconds = 0)
        {
            if (cameraNumber == 0)
            {
                Disconnect(monitorNumber, cellNumber);
                return;
            }
            lock (_vxSystemLock)
            {
                int cell = cellNumber;
                VxSdkNet.DataSource camera = GetCamera(cameraNumber);
                // possibly overwrites cell if MonitorToCellMap in use
                VxSdkNet.Monitor monitor = GetMonitor(monitorNumber, ref cell);
                Trace.WriteLineIf(_debugLevel > 0, "Display Camera " + cameraNumber + " on Monitor " + monitorNumber + " in cell " + (cell + 1));

                if (camera != null && monitor != null)
                {
                    if (monitor.MonitorCells.Count > cell)
                    {
                        VxSdkNet.MonitorCell monCell = monitor.MonitorCells[cell];
                        if (monCell.DataSourceId != camera.Id)
                            monCell.DataSourceId = camera.Id;
                        if (previousSeconds != 0)
                        {
                            DateTime utcTime = DateTime.UtcNow;
                            TimeSpan span = new TimeSpan(0, 0, previousSeconds);
                            Trace.WriteIf(_debugLevel > 0, "   UTC Time: " + utcTime.ToString());
                            utcTime = utcTime - span;
                            Trace.WriteLineIf(_debugLevel > 0, ", Setting time to " + utcTime.ToString());
                            monCell.Time = utcTime;
                            //utcTime = utcTime - span;
                            //Trace.WriteLineIf(_debugLevel > 0, ", Setting time to " + utcTime.ToString());
                            //monCell.Time = utcTime;
                            Trace.WriteLineIf(_debugLevel > 0, ", Setting time Complete");
                        }
                        else monCell.GoToLive();
                    }
                    else
                    {
                        Trace.WriteLineIf(_debugLevel > 0, "Unable to display Camera " + cameraNumber + " on Monitor " + monitorNumber + " in cell " + cell);
                    }
                }
                else
                {
                    Trace.WriteLineIf(_debugLevel > 0, "Unable to display Camera " + cameraNumber + " on Monitor " + monitorNumber);
                }
            }
        }

        private VxSdkNet.DataSource GetCamera(int cameraNumber)
        {
            VxSdkNet.DataSource datasource = null;
            lock (_vxSystemLock)
            {
                if (_datasources != null)
                {
                    try
                    {
                        datasource = _datasources.Find(x => (x.Number == cameraNumber));
                    }
                    catch
                    {
                        Trace.WriteLineIf(_debugLevel > 0, "Camera " + cameraNumber + " not found");
                    };
                }
            }
            return datasource;
        }

        private VxSdkNet.DataSource GetCamera(string id)
        {
            VxSdkNet.DataSource datasource = null;
            lock (_vxSystemLock)
            {
                if (_datasources != null)
                {
                    try
                    {
                        datasource = _datasources.Find(x => (x.Id == id));
                    }
                    catch
                    {
                        Trace.WriteLineIf(_debugLevel > 0, "Camera " + id + " not found");
                    };
                }
            }
            return datasource;
        }

        private string GetCameraUUID(int cameraNumber)
        {
            string uuid = string.Empty;
            lock (_vxSystemLock)
            {
                if (_datasources != null)
                {
                    try
                    {
                        VxSdkNet.DataSource dataSource = _datasources.Find(x => (x.Number == cameraNumber));
                        uuid = dataSource.Id;
                        Trace.WriteLineIf(_debugLevel > 0, "Camera " + cameraNumber + " found: " + uuid);
                    }
                    catch
                    {
                        Trace.WriteLineIf(_debugLevel > 0, "Camera " + cameraNumber + " not found");
                    };
                }
            }
            return uuid;
        }

        private VxSdkNet.Monitor GetMonitor(int monitorNumber, ref int cell)
        {
            VxSdkNet.Monitor monitor = null;
            lock (_vxSystemLock)
            {
                if (_monitors != null)
                {
                    try
                    {
                        monitor = _monitors.Find(x => x.Number == monitorNumber);
                    }
                    catch
                    {
                        Trace.WriteLineIf(_debugLevel > 0, "Monitor " + monitorNumber + " not found");
                    };
                }
            }
            return monitor;
        }

        private string GetMonitorUUID(int monitorNumber)
        {
            string uuid = string.Empty;
            lock (_vxSystemLock)
            {
                if (_monitors != null)
                {
                    try
                    {
                        VxSdkNet.Monitor monitor = _monitors.Find(x => x.Number == monitorNumber);
                        uuid = monitor.Id;
                        Trace.WriteLineIf(_debugLevel > 0, "Monitor " + monitorNumber + " found: " + uuid);
                    }
                    catch
                    {
                        Trace.WriteLineIf(_debugLevel > 0, "Monitor " + monitorNumber + " not found");
                    };
                }
            }
            return uuid;
        }

        private int GetCameraInCell(int monitorNumber, int cellNumber)
        {
            int cameraNumber = 0;
            lock (_vxSystemLock)
            {
                int cell = cellNumber;
                // possibly overwrites cell if MonitorToCellMap in use
                VxSdkNet.Monitor monitor = GetMonitor(monitorNumber, ref cell);
                if (monitor != null)
                {
                    if (monitor.MonitorCells.Count > cell)
                    {
                        VxSdkNet.MonitorCell monCell = monitor.MonitorCells[cell];
                        VxSdkNet.DataSource dataSource = GetCamera(monCell.DataSourceId);
                        if (dataSource != null)
                            cameraNumber = dataSource.Number;
                    }
                    else
                    {
                        Trace.WriteLineIf(_debugLevel > 0, "GetCameraInCell has fewer cells than " + cell);
                    }
                }
                else
                {
                    Trace.WriteLineIf(_debugLevel > 0, "GetCameraInCell cannot find monitor " + monitorNumber);
                }
            }

            return cameraNumber;
        }

        private void CreateBookmark(int camera, string description)
        {
            try
            {
                lock (_vxSystemLock)
                {
                    VxSdkNet.VXSystem system = GetVxSystem();
                    if (system != null)
                    {
                        VxSdkNet.DataSource dataSource = GetCamera(camera);
                        DateTime time = DateTime.Now;
                        var newBookmark = new VxSdkNet.NewBookmark
                        {
                            Description = description,
                            Time = time.ToUniversalTime(),
                            DataSourceId = dataSource.Id
                        };

                        var result = system.AddBookmark(newBookmark);
                        if (result != VxSdkNet.Results.Value.OK)
                        {
                            Trace.WriteLineIf(_debugLevel > 0, "Unable to create BookMark for camera " + camera + " Result: " + result);
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Trace.WriteLineIf(_debugLevel > 0, "Unable to create BookMark for camera " + camera + " Exception: " + e.Message);
            }
        }

        private VxSdkNet.Monitor.Layouts StringToMonitorLayout(string layoutStr)
        {
            switch (layoutStr.ToLower())
            {
                case "1x1":
                    return VxSdkNet.Monitor.Layouts.CellLayout1x1;
                case "1x2":
                    return VxSdkNet.Monitor.Layouts.CellLayout1x2;
                case "2x1":
                    return VxSdkNet.Monitor.Layouts.CellLayout2x1;
                case "2x2":
                    return VxSdkNet.Monitor.Layouts.CellLayout2x2;
                case "2x3":
                    return VxSdkNet.Monitor.Layouts.CellLayout2x3;
                case "3x2":
                    return VxSdkNet.Monitor.Layouts.CellLayout3x2;
                case "3x3":
                    return VxSdkNet.Monitor.Layouts.CellLayout3x3;
                case "4x3":
                    return VxSdkNet.Monitor.Layouts.CellLayout4x3;
                case "4x4":
                    return VxSdkNet.Monitor.Layouts.CellLayout4x4;
                case "5x5":
                    return VxSdkNet.Monitor.Layouts.CellLayout5x5;
                case "1+12":
                    return VxSdkNet.Monitor.Layouts.CellLayout1plus12;
                case "2+8":
                    return VxSdkNet.Monitor.Layouts.CellLayout2plus8;
                case "3+4":
                    return VxSdkNet.Monitor.Layouts.CellLayout3plus4;
                case "1+5":
                    return VxSdkNet.Monitor.Layouts.CellLayout1plus5;
                case "1+7":
                    return VxSdkNet.Monitor.Layouts.CellLayout1plus7;
                case "12+1":
                    return VxSdkNet.Monitor.Layouts.CellLayout12plus1;
                case "8+2":
                    return VxSdkNet.Monitor.Layouts.CellLayout8plus2;
                case "1+1+4":
                    return VxSdkNet.Monitor.Layouts.CellLayout1plus1plus4;
                case "1+4 (tall)":
                    return VxSdkNet.Monitor.Layouts.CellLayout1plus4tall;
                case "1+4 (wide)":
                    return VxSdkNet.Monitor.Layouts.CellLayout1plus4wide;
                case "MonitorWall":
                    return VxSdkNet.Monitor.Layouts.MonitorWall;
                default:
                    return VxSdkNet.Monitor.Layouts.CellLayout2x2;
            }
        }
        #endregion //SCRIPTING SUPPORT

        #region PTZ Methods

        private VxSdkNet.PtzController GetPTZController(int cameraNumber)
        {
            VxSdkNet.PtzController ptzController = null;
            lock (_vxSystemLock)
            {
                try
                {
                    VxSdkNet.DataSource camera = GetCamera(cameraNumber);
                    ptzController = camera.PTZController;
                }
                catch
                {
                    Trace.WriteLineIf(_debugLevel > 0, "Unable to get PTZController for " + cameraNumber);
                }
            }
            return ptzController;
        }

        private void SendGotoPreset(int camera, string presetStr)
        {
            try
            {
                lock (_vxSystemLock)
                {
                    VxSdkNet.PtzController ptzController = GetPTZController(camera);
                    if (ptzController != null)
                    {
                        List<VxSdkNet.Preset> presets = ptzController.Presets;
                        VxSdkNet.Preset preset = presets.Find(x => x.Name.ToUpper() == presetStr.ToUpper());
                        if (preset != null)
                            ptzController.TriggerPreset(preset);
                        else
                        {
                            Trace.WriteLineIf((_debugLevel > 0), "Unable to find preset " + presetStr);
                            Trace.WriteLineIf((_debugLevel > 0), "Available:");
                            foreach (VxSdkNet.Preset prst in presets)
                            {
                                Trace.WriteLineIf((_debugLevel > 0), "   " + prst.Name);
                            }
                        }
                    }
                    else
                    {
                        Trace.WriteLineIf(_debugLevel > 0, "No PTZController for camera " + camera + " Unable to call Preset " + presetStr);
                    }
                }
            }
            catch
            {
                Trace.WriteLineIf(_debugLevel > 0, "Exception, Unable to call Preset " + presetStr + " on camera " + camera);
            }
        }

        private void SendGotoPattern(int camera, string patternStr)
        {
            try
            {
                lock (_vxSystemLock)
                {
                    VxSdkNet.PtzController ptzController = GetPTZController(camera);
                    if (ptzController != null)
                    {
                        List<VxSdkNet.Pattern> patterns = ptzController.Patterns;
                        VxSdkNet.Pattern pattern = patterns.Find(x => x.Name == patternStr);
                        if (pattern != null)
                            ptzController.TriggerPattern(pattern);
                    }
                    else
                    {
                        Trace.WriteLineIf(_debugLevel > 0, "No PTZController, Unable to call Pattern " + patternStr);
                    }
                }
            }
            catch
            {
                Trace.WriteLineIf(_debugLevel > 0, "Exception, Unable to call Pattern " + patternStr);
            }
        }
        #endregion

        #region STATUS_AND_DEBUG
        /// <summary>
        /// Get status of Event Handler
        /// </summary>
        /// <returns>String containing the status.</returns>
        public string GetStatus()
        {
            string status = string.Empty;
            status += "   Debug level set to " + _debugLevel;
            if (_vxSettings != null)
            {
                VxSdkNet.VXSystem system = GetVxSystem();
                if (system != null)
                {
                    status += "\n   Connected to VideoXpert " + _vxSettings.VxCoreAddress + ":" + _vxSettings.VxCorePort;
                }
                else status += "Invalid VideoXpert settings";
            }
            else status += "   NOT connected to VideoXpert";

            if (_ipcServer != null)
            {
                if (_acServerSettings != null)
                {
                    string sslString = string.Empty;
                    if ((_acServerSettings.UseSSL))
                        sslString += " ssl certificate: " + _acServerSettings.Certificate;
                    status += "\n   IPC Server running " + _acServerSettings.Address + ":" + _acServerSettings.Port + sslString;
                }
                else status += "Invalid AccessContolServer settings";
            }
            else status += "\n   IPC Server not running";

            if ((_acsWrapper != null)&&(_acsWrapper.LoggedIn))
            {
                status += "\n   Connected to ACS " + _acsSettings.HostUrl;
            }
            else status += "\n   Not Connected to ACS";

            status += "\n\nVideoXpert Subscriptions:";
            if (_subscribeSituations != null)
            {
                foreach (var subscription in _subscribeSituations)
                {
                    status += "\n   " + subscription.Type;
                }
            }

            return status;
        }

        /// <summary>
        /// Set Debug
        /// </summary>
        /// <param name="level">level 0 = off</param>
        public void SetDebug(int level)
        {
            _debugLevel = level;
        }

        /// <summary>
        /// Get datasource information
        /// </summary>
        /// <param name="partialCameraName">filter for a particular resource if desired</param>
        /// <returns>CR LF delimited String containing datasource information.</returns>
        public string GetDataSourceInfo(string partialCameraName)
        {
            string data = string.Empty;
            try
            {
                lock(_vxSystemLock)
                {
                    if (string.IsNullOrEmpty(partialCameraName))
                    {
                        foreach (VxSdkNet.DataSource dataSource in _datasources)
                        {
                            data += "Camera " + dataSource.Number + " : " + dataSource.Name + "\r\n";
                            data += "Id   : " + dataSource.Id + "\r\n";
                        }
                    }
                    else
                    {
                        VxSdkNet.DataSource dataSource = _datasources.Find(x => x.Name.Contains(partialCameraName));
                        data += "Camera " + dataSource.Number + " : " + dataSource.Name + "\r\n";
                        data += "Id   : " + dataSource.Id + "\r\n";
                    }
                }
            }
            catch
            {
                data = "Camera " + partialCameraName + "  Not Found";
            }

            return data;
        }

        /// <summary>
        /// Get monitor information
        /// </summary>
        /// <param name="partialMonitorName">filter for a particular monitor if desired</param>
        /// <returns>CR LF delimited String containing monitor information.</returns>
        public string GetMonitorInfo(string partialMonitorName)
        {
            string data = string.Empty;
            try
            {
                lock (_vxSystemLock)
                {
                    if (string.IsNullOrEmpty(partialMonitorName))
                    {
                        foreach (VxSdkNet.Monitor monitor in _monitors)
                        {
                            data += "Monitor " + monitor.Number + " : " + monitor.Name + "\r\n";
                            data += "Id   : " + monitor.Id + "\r\n";
                        }
                    }
                    else
                    {
                        VxSdkNet.Monitor monitor = _monitors.Find(x => x.Name.Contains(partialMonitorName));
                        data += "Monitor " + monitor.Number + " : " + monitor.Name + "\r\n";
                        data += "Id   : " + monitor.Id + "\r\n";
                    }
                }
            }
            catch
            {
                data = "Monitor " + partialMonitorName + "  Not Found";
            }

            return data;
        }

        /// <summary>
        /// Get situation type information
        /// </summary>
        /// <returns>String array containing all situation types.</returns>
        public string[] GetSituationTypes()
        {
            string[] situations = null;

            lock (_vxSystemLock)
            {
                if (_situations != null)
                {
                    int count = _situations.Count;
                    situations = new string[count];
                    int i = 0;
                    foreach (VxSdkNet.Situation sit in _situations)
                    {
                        situations[i] = sit.Type;
                        i++;
                    }
                }
            }
            return situations;
        }

        /// <summary>
        /// Get ACSEventInformation
        /// </summary>
        /// <param name="appFilter">filter for a particular app if desired</param>
        /// <returns>String containing event information.</returns>
        public string GetACSEventInformation(string appFilter = "", bool appOnly = false)
        {
            string eventInformation = string.Empty;
            if (_acsWrapper != null)
            {
                List<string> acsEvents = _acsWrapper.GetEventList();
                foreach(string acsEvent in acsEvents)
                {
                    eventInformation += acsEvent + "\n";
                }
            }
            return eventInformation;
        }

        /// <summary>
        /// List Users
        /// </summary>
        /// <param name="partialUserName">filter for user name</param>
        /// <returns>String containing user information.</returns>
        public string ListUsers(string partialUserName)
        {
            string response = string.Empty;
            List<FakeUser> users = _acsWrapper.GetUsers();
            if (users != null)
            {
                response = "Users found " + users.Count();
                foreach (var user in users)
                {
                    if (string.IsNullOrEmpty(partialUserName) || (user.Name.ToUpper().Contains(partialUserName.ToUpper())))
                    {
                        response += "\r\n    " + user.Name + "     UserId: " + user.UserId + "     Card: " + user.CardNumber;
                        response += "\r\n";
                    }
                }
            }
            else
            {
                response = "No Users found ";
            }

            return response;
        }

        /// <summary>
        /// List Doors
        /// </summary>
        /// <param name="partialDoorName">filter for door name</param>
        /// <returns>String containing door information.</returns>
        public string ListDoors(string partialDoorName)
        {
            string response = string.Empty;
            var doors = _acsWrapper.GetDoors();
            if (doors != null)
            {
                response = "Doors found " + doors.Count();
                foreach (var door in doors)
                {
                    if ((string.IsNullOrEmpty(partialDoorName) || (door.Name.ToUpper().Contains(partialDoorName.ToUpper()))))
                    {
                        response += "\r\n    " + door.Name + "    DoorId: " + door.DoorId + "     Status: " + door.Status;
                        response += "\r\n";
                    }
                }
            }
            else
            {
                response = "No Doors found ";
            }

            return response;
        }

        /// <summary>
        /// List Doors
        /// </summary>
        /// <param name="partialDoorName">filter for door name</param>
        /// <param name="status">door status</param>
        /// <returns>String containing door information.</returns>
        public string SetDoorStatus(string partialDoorName, string status)
        {
            string response = string.Empty;
            List<FakeDoor> doors = _acsWrapper.GetDoors();
            if (doors != null)
            {
                FakeDoor door = doors.FirstOrDefault(x => x.Name.ToUpper().Contains(partialDoorName.ToUpper()));
                if (door != null)
                {
                    _acsWrapper.SetDoorStatus(door.Name, status);
                    response = "Door " + door + " Status: " + status;
                    FakeACSEvent fakeEvent = _acsWrapper.CreateFakeDoorEvent(door.DoorId, status);
                    _fakeACSEvents.Enqueue(fakeEvent);
                }
                else
                {
                    response = "Door " + partialDoorName + " Not Found";
                }
            }
            else
            {
                response = "No Doors found ";
            }

            return response;
        }

        #region FakeACS
        /// <summary>
        /// Create a fake ACS Event for testing
        /// </summary>
        public void CreateFakeACSEvent()
        {
            _createRandomACSEvent = true;
        }
        #endregion
        #endregion

        #region IACSServer interface
        public ACSSystem GetSystem()
        {
            if ((_acsWrapper == null)||(! _acsWrapper.LoggedIn))
                throw new System.IO.IOException();    

            Trace.WriteLineIf(_debugLevel > 1, "IACS GetSystem called");
            ACSSystem system = new ACSSystem();
            system.Description = "Generic ACS";
            system.Manufacturer = "Generic";
            system.ProductName = "Generic";
            return system;
        }

        public List<ACSAlarm> GetAlarmList()
        {
            if ((_acsWrapper == null)||(! _acsWrapper.LoggedIn))
                throw new System.IO.IOException();    

            Trace.WriteLineIf(_debugLevel > 1, "IACS GetAlarmList called");
            throw new NotImplementedException();
        }

        public ACSAlarm GetAlarm(string Id)
        {
            if ((_acsWrapper == null)||(! _acsWrapper.LoggedIn))
                throw new System.IO.IOException();    

            Trace.WriteLineIf(_debugLevel > 1, "IACS GetAlarm " + Id);
            throw new NotImplementedException();
        }

        public bool AckAlarm(string alarmId)
        {
            if ((_acsWrapper == null)||(! _acsWrapper.LoggedIn))
                throw new System.IO.IOException();    
            // this call is not needed.  Acknowledgement is read from event in OnSystemEvent
            Trace.WriteLineIf(_debugLevel > 1, "IACS AckAlarm " + alarmId);
            throw new NotImplementedException();
        }

        public bool SetAlarmState(string alarmId, ACSAlarmState state)
        {
            if ((_acsWrapper == null)||(! _acsWrapper.LoggedIn))
                throw new System.IO.IOException();    

            Trace.WriteLineIf(_debugLevel > 1, "IACS SetAlarmState " + state.ToString());
            throw new NotImplementedException();
        }

        public List<ACSUser> GetUserList()
        {
            if ((_acsWrapper == null)||(! _acsWrapper.LoggedIn))
                throw new System.IO.IOException();    
                        
            Trace.WriteLineIf(_debugLevel > 1, "IACS GetUserList called");
            List<FakeUser> userList = _acsWrapper.GetUsers();
            int userId = 1;
            List<ACSUser> acsUsers = new List<ACSUser>();
            foreach(FakeUser user in userList)
            {
                ACSUser acsUser = new ACSUser();
                acsUser.Name = user.Name;
                acsUser.Id = user.UserId;
                userId++;
                acsUsers.Add(acsUser);
            }
            return acsUsers;
        }

        public ACSUser GetUser(string Id)
        {
            if ((_acsWrapper == null)||(! _acsWrapper.LoggedIn))
                throw new System.IO.IOException();    

            Trace.WriteLineIf(_debugLevel > 1, "IACS Getuser " + Id);
            List<ACSUser> userList = GetUserList();
            ACSUser user = userList.Find(x => x.Id == Id);
            return user;
        }

        public byte[] GetUserImage(string imageId)
        {
            if ((_acsWrapper == null)||(! _acsWrapper.LoggedIn))
                throw new System.IO.IOException();

            Trace.WriteLineIf(_debugLevel > 1, "IACS GetUserImage " + imageId);
            Image image = _acsWrapper.GetUserImage(imageId);

            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

        public List<ACSIcon> GetIconList()
        {
            if ((_acsWrapper == null)||(! _acsWrapper.LoggedIn))
                throw new System.IO.IOException();    

            Trace.WriteLineIf(_debugLevel > 1, "IACS GetIconList called");
            throw new NotImplementedException();
        }

        public ACSIcon GetIcon(string Id)
        {
            if ((_acsWrapper == null)||(! _acsWrapper.LoggedIn))
                throw new System.IO.IOException();    

            Trace.WriteLineIf(_debugLevel > 1, "IACS GetIcon " + Id);
            throw new NotImplementedException();
        }

        public List<ACSAccessPoint> GetAccessPointList()
        {
            if ((_acsWrapper == null)||(! _acsWrapper.LoggedIn))
                throw new System.IO.IOException();    

            Trace.WriteLineIf(_debugLevel > 2, "IACS GetAccessPointList called");
            List<ACSAccessPoint> accessPointList = new List<ACSAccessPoint>();
            var doorList = _acsWrapper.GetDoors();
            foreach(var door in doorList)
            {
                ACSAccessPoint accessPoint = new ACSAccessPoint();
                accessPoint.DeviceType = AccessPointDevice.DoorDevice;
                accessPoint.Id = door.DoorId;
                accessPoint.Name = door.Name;
                accessPoint.Status = ConvertToACSStatus(door.Status);
                accessPointList.Add(accessPoint);
            }
            return accessPointList;
        }

        private ACSAccessPointStatus ConvertToACSStatus(string status)
        {
            ACSAccessPointStatus acsStatus = ACSAccessPointStatus.UnKnown;
            if (status.ToUpper().Contains("UNL"))
                acsStatus = ACSAccessPointStatus.Unlocked;
            else if (status.ToUpper().Contains("LOCK"))
                acsStatus = ACSAccessPointStatus.Locked;
            else if (status.ToUpper().Contains("OPE"))
                acsStatus = ACSAccessPointStatus.Opened;
            else if (status.ToUpper().Contains("CL"))
                acsStatus = ACSAccessPointStatus.Closed;
            else if (status.ToUpper().Contains("FA"))
                acsStatus = ACSAccessPointStatus.Faulted;
            else if (status.ToUpper().Contains("FO"))
                acsStatus = ACSAccessPointStatus.Forced;
            else if (status.ToUpper().Contains("PR"))
                acsStatus = ACSAccessPointStatus.Propped;
            return acsStatus;
        }

        public bool SetAccessPointState(ACSAccessPoint condition)
        {
            if ((_acsWrapper == null)||(! _acsWrapper.LoggedIn))
                throw new System.IO.IOException();    

            Trace.WriteLineIf(_debugLevel > 0, "IACS SetAccessPoint " + condition.Status.ToString());
            _acsWrapper.SetDoorStatus(condition.Name, condition.Status.ToString());
            FakeACSEvent fakeEvent = _acsWrapper.CreateFakeDoorEvent(condition.Id, condition.Status.ToString());
            _fakeACSEvents.Enqueue(fakeEvent);
            return true;
        }

        public bool SetSystemStatus(ACSSystemStatus status)
        {
            if ((_acsWrapper == null) || (!_acsWrapper.LoggedIn))
                throw new System.IO.IOException();

            Trace.WriteLineIf(_debugLevel > 1, "IACS SetSystemStatus " + status);
            throw new NotImplementedException();
        }
        #endregion
    }
}
