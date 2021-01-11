using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Reflection;
using VxEvents;
using XMLUtilities;

using System.Xml;
using System.Xml.Serialization;

namespace VxEventServer
{
    public class VxEventServerManager : IDisposable
    {
        private static string acsCertificateFileName = "ACSRoot.pfx";

        private CustomSituations _customSituations = null;
        private VideoXpertSettings _VxSettings = null;
        private ACSSettings _AxSettings = null;
        private EventConfiguration _eventConfiguration = null;
        private AccessControlServerSettings _acServerSettings = null;
        private CameraAssociations _cameraAssociations = null;
        private Scripts _scripts = null;
        private XMLWrapper _xmlWrapper = new XMLWrapper();

        public bool Initialized { get; private set; }
        private VxEventHandler _vxEventHandler = null;

        public static VxEventServerManager Instance
        {
            get
            {
                return InstanceClass.Instance;
            }
        }

        private class InstanceClass
        {
            public static readonly VxEventServerManager Instance = new VxEventServerManager();

            static InstanceClass()
            {
            }
        }

        /// <summary>
        /// Init the VxEventServerManager
        /// </summary>
        public VxEventServerManager Init()
        {
            if (this.Initialized)
                return this;

            Reload();

            _vxEventHandler = new VxEventHandler(_VxSettings, 
                                                _AxSettings, 
                                                _acServerSettings, 
                                                _customSituations, 
                                                _eventConfiguration,
                                                _cameraAssociations,
                                                _scripts);

            this.Initialized = true;

            return this;
        }

        private void Reload()
        {
            _customSituations = _xmlWrapper.GetCustomSituations();
            _VxSettings = _xmlWrapper.GetVideoXpertSettings();
            if ((_VxSettings != null) && (_VxSettings.IsChanged()))
            {
                _xmlWrapper.SaveVideoXpertSettings(_VxSettings);
            }
            _AxSettings = _xmlWrapper.GetACSSettings();
            if ((_AxSettings != null) && (_AxSettings.IsChanged()))
            {
                _xmlWrapper.SaveACSSettings(_AxSettings);
            }
            _eventConfiguration = _xmlWrapper.GetEventConfiguration();
            _acServerSettings = _xmlWrapper.GetAccessControlServerSettings();
            _cameraAssociations = _xmlWrapper.GetCameraAssociations();
            _scripts = _xmlWrapper.GetScripts();
        }

        public void Dispose()
        {
            if (_vxEventHandler != null)
            {
                _vxEventHandler.Dispose();
                _vxEventHandler = null;
            }
        }
        
        /// <summary>
        /// Print Status of service
        /// </summary>
        public void PrintStatus()
        {
            Console.WriteLine("\nSTATUS:\n");

            if (_vxEventHandler != null)
            {
                string status = _vxEventHandler.GetStatus();
                Console.WriteLine(status);
            }
            else Console.WriteLine("VxEventHandler is NULL");
        }

        /// <summary>
        /// SetDebugLevel
        /// </summary>
        /// <param name="level">Level to set debug at</param>
        public void SetDebugLevel(int level)
        {
            if (_vxEventHandler != null)
            {
                _vxEventHandler.SetDebug(level);
            }
        }

        /// <summary>
        /// List Data Sources
        /// </summary>
        /// <param name="partialCameraName">Filter for returned datasources.  May be empty.</param>
        /// <returns>First datasource matching containing partial camera name or all datasources</returns>
        public string ListDataSources(string partialCameraName)
        {
            string response = string.Empty;
            if (_vxEventHandler != null)
            {
                partialCameraName = partialCameraName.Replace("\n", "");
                partialCameraName = partialCameraName.Replace("\r", "");
                response = _vxEventHandler.GetDataSourceInfo(partialCameraName);
            }
            return response;
        }

        /// <summary>
        /// List Monitors
        /// </summary>
        /// <param name="partialMonitorName">Filter for returned monitors.  May be empty.</param>
        /// <returns>First monitor matching containing partial monitor name or all monitors</returns>
        public string ListMonitors(string partialMonitorName)
        {
            string response = string.Empty;
            if (_vxEventHandler != null)
            {
                partialMonitorName = partialMonitorName.Replace("\n", "");
                partialMonitorName = partialMonitorName.Replace("\r", "");
                response = _vxEventHandler.GetMonitorInfo(partialMonitorName);
            }
            return response;
        }

        /// <summary>
        /// List Situations
        /// </summary>
        /// <param name="partialSituation">Filter for returned situations.  May be empty.</param>
        /// <returns>First situaiton matching containing partial situation name or all situations</returns>
        public string ListSituations(string partialSituation)
        {
            string response = string.Empty;
            if (_vxEventHandler != null)
            {
                string[] situationTypes = _vxEventHandler.GetSituationTypes();
                if (situationTypes != null)
                {
                    foreach (string situationType in situationTypes)
                    {
                        if ((String.IsNullOrEmpty(partialSituation)) || (situationType.Contains(partialSituation)))
                        {
                            response += situationType + "\n";
                        }
                    }
                }
            }
            return response;
        }

        /// <summary>
        /// List ACSEventInformation
        /// </summary>
        /// <param name="partialAppFilter">Filter for returning events belonging to an app</param>
        /// <returns>First situaiton matching containing partial situation name or all situations</returns>
        public string ListACSEventInformation(string partialAppFilter)
        {
            return _vxEventHandler.GetACSEventInformation(partialAppFilter);
        }

        /// <summary>
        /// List Users
        /// </summary>
        /// <param name="partialUserName">filter for user name</param>
        /// <returns>String containing user information.</returns>
        public string ListUsers(string partialUserName)
        {
            return _vxEventHandler.ListUsers(partialUserName);
        }

        /// <summary>
        /// List Alarms
        /// </summary>
        /// <param name="partialAlarmName">filter for alarm name</param>
        /// <returns>String containing alarm information.</returns>
        public string ListAlarms(string partialAlarmName)
        {
            return _vxEventHandler.ListAlarms(partialAlarmName);
        }


        /// <summary>
        /// List Doors
        /// </summary>
        /// <param name="partialDoorName">filter for door name</param>
        /// <returns>String containing door information.</returns>
        public string ListDoors(string partialDoorName)
        {
            return _vxEventHandler.ListDoors(partialDoorName);
        }

        /// <summary>
        /// Set Alarm Status
        /// </summary>
        /// <param name="partialAlarmName">filter for alarm name</param>
        /// <param name="status">alarm status</param>
        public string SetAlarmStatus(string partialAlarmName, string status)
        {
            return _vxEventHandler.SetAlarmStatus(partialAlarmName, status);
        }

        /// <summary>
        /// Set Door Status
        /// </summary>
        /// <param name="partialDoorName">filter for door name</param>
        /// <param name="status">door status</param>
        public string SetDoorStatus(string partialDoorName, string status)
        {
            return _vxEventHandler.SetDoorStatus(partialDoorName, status);
        }

        #region FakeACS
        /// <summary>
        /// Create a fake ACS Event for testing
        /// </summary>
        public void CreateFakeACSEvent()
        {
            _vxEventHandler.CreateFakeACSEvent();
        }
        #endregion
    }
}
