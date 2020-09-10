using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceProcess;
using System.Management;
using XMLUtilities;
using VxEvents;

namespace _VX_ACS_Administration
{
    public partial class FormAdmin : Form
    {
        private readonly SynchronizationContext _synchronizationContext;
        private bool _dirty = false;
        private bool _vxSettingsChanged = false;
        //private bool _cameraAssociationsChanged = false;
        private bool _customSituationsDirty = false;
        private bool _cancelProcess = false;
        private System.Windows.Forms.Timer _progressTimer = new System.Windows.Forms.Timer();
        private int _progressCount = 0;
        private VxSdkNet.VXSystem _vxSystem = null;
        private List<VxSdkNet.DataSource> _datasources = null;
        private List<VxSdkNet.Situation> _situations = null;
        private List<string> _situationList = new List<string>();
        private List<string> _cameraList = new List<string>();
        private List<string> _acsEvents = new List<string>();
        private List<string> _acsPeripherals = new List<string>();
        private List<PeripheralData> _acsPeripheralList = new List<PeripheralData>();
        private List<EventData> _acsEventList = new List<EventData>();
        private List<Script> _scripts = new List<Script>();
        private List<string> _actionNames = new List<string>();
        private List<string> _layoutNames = new List<string>();

        private XMLWrapper _xmlWrapper = new XMLWrapper();
        private ACSWrapper _acsWrapper = null;
        private bool _connectionChanged = false;
        private bool _connectedVx = false;
        private bool _connectedACS = false;
        private bool _eventDataLoaded = false;

        public FormAdmin()
        {
            InitializeComponent();
            _synchronizationContext = SynchronizationContext.Current;
            this.Load += FormAdmin_Load;
            this.FormClosing += FormAdmin_FormClosing;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
        }

        void FormAdmin_FormClosing(object sender, FormClosingEventArgs e)
        {
            _progressTimer.Tick -= new EventHandler(_progressTimer_Tick);
            _progressTimer.Dispose();
        }

        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FormAdmin_Load(object sender, EventArgs e)
        {
            try
            {
                situationBindingSource.DataSource = _situationList;
                cameraBindingSource.DataSource = _cameraList;
                feenicsEventsBindingSource.DataSource = _acsEvents;
                feenicsPeripheralsBindingSource.DataSource = _acsPeripherals;

                cameraAssociationGridView.DefaultCellStyle.SelectionForeColor = Color.Black;
                cameraAssociationGridView.DefaultCellStyle.ForeColor = Color.Black;
                eventMapDataGrid.DefaultCellStyle.SelectionForeColor = Color.Black;
                eventMapDataGrid.DefaultCellStyle.ForeColor = Color.Black;
                customSituationDataGrid.DefaultCellStyle.SelectionForeColor = Color.Black;
                customSituationDataGrid.DefaultCellStyle.ForeColor = Color.Black;
                scriptsDataGrid.DefaultCellStyle.SelectionForeColor = Color.Black;
                scriptsDataGrid.DefaultCellStyle.ForeColor = Color.Black;

                // Load XML data to page if it exists
                LoadACSSettings();
                LoadVideoXpertSettings();
                LoadAccessControlServerSettings();
                TryConnectACSAsync();
                TryConnectVideoXpertAsync();
                // clear dirty flag (loading to screen will cause dirty to be called)
                buttonApply.Enabled = false;
                _dirty = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logging.LogFile_Error(Application.ProductName.ToString(), System.Reflection.MethodInfo.GetCurrentMethod().ToString() + " " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="t"></param>
        public static void UIThreadExceptionHandler(object sender, System.Threading.ThreadExceptionEventArgs t)
        {
            MessageBox.Show(Properties.Resources.UnknownError + t.Exception.Message, Properties.Resources.Continuum, MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
        }

        private void LoadACSSettings()
        {
            try
            {
                ACSSettings settings = _xmlWrapper.GetACSSettings();
                if (settings != null)
                {
                    textBoxACSAddress.Text = settings.HostUrl;
                    textBoxACSUserName.Text = settings.GetUsername();
                    textBoxACSPassword.Text = settings.GetPassword();
                }
            }
            catch(Exception ex)
            {
                Logging.LogFile_Error(Application.ProductName.ToString(), System.Reflection.MethodInfo.GetCurrentMethod().ToString() + " " + ex.Message);
            }
        }

        private bool SaveACSSettings()
        {
            bool success = false;
            try
            {
                ACSSettings settings = new ACSSettings();
                if (settings != null)
                {
                    settings.HostUrl = textBoxACSAddress.Text;
                    settings.EncodeAndSaveUserNamePassword(textBoxACSUserName.Text, textBoxACSPassword.Text);
                    _xmlWrapper.SaveACSSettings(settings);
                    success = true;
                }
            }
            catch(Exception ex)
            {
                Logging.LogFile_Error(Application.ProductName.ToString(), System.Reflection.MethodInfo.GetCurrentMethod().ToString() + " " + ex.Message);
                success = false;
            }
            return success;
        }

        private void LoadVideoXpertSettings()
        {
            try
            {
                VideoXpertSettings settings = _xmlWrapper.GetVideoXpertSettings();
                if (settings != null)
                {
                    textBoxVxIpAddress.Text = settings.VxCoreAddress;
                    textBoxVxPort.Text = Convert.ToString(settings.VxCorePort);
                    textBoxVxUserName.Text = settings.GetUsername();
                    textBoxVxPassword.Text = settings.GetPassword();
                }
            }
            catch(Exception ex)
            {
                Logging.LogFile_Error(Application.ProductName.ToString(), System.Reflection.MethodInfo.GetCurrentMethod().ToString() + " " + ex.Message);
            }
        }

        private bool SaveVideoXpertSettings()
        {
            bool success = false;
            try
            {
                VideoXpertSettings settings = new VideoXpertSettings();
                if (settings != null)
                {
                    settings.VxCoreAddress = textBoxVxIpAddress.Text;
                    settings.VxCorePort = Convert.ToInt32(textBoxVxPort.Text);
                    settings.EncodeAndSaveUserNamePassword(textBoxVxUserName.Text, textBoxVxPassword.Text);
                    _xmlWrapper.SaveVideoXpertSettings(settings);
                    success = true;
                }
            }
            catch(Exception ex)
            {
                Logging.LogFile_Error(Application.ProductName.ToString(), System.Reflection.MethodInfo.GetCurrentMethod().ToString() + " " + ex.Message);
                success = false;
            }
            return success;
        }

        private void LoadAccessControlServerSettings()
        {
            try
            {
                AccessControlServerSettings settings = _xmlWrapper.GetAccessControlServerSettings();
                if (settings != null)
                {
                    textBoxACSIpAddress.Text = settings.Address;
                    textBoxACSPort.Text = Convert.ToString(settings.Port);
                    ACSuseSSLCheckBox.Checked = settings.UseSSL;
                }
            }
            catch(Exception ex)
            {
                Logging.LogFile_Error(Application.ProductName.ToString(), System.Reflection.MethodInfo.GetCurrentMethod().ToString() + " " + ex.Message);
            }
        }

        private bool SaveAccessControlServerSettings()
        {
            bool success = false;
            try
            {
                AccessControlServerSettings settings = new AccessControlServerSettings();
                if (settings != null)
                {
                    settings.Address = textBoxACSIpAddress.Text;
                    settings.Port = Convert.ToInt32(textBoxACSPort.Text);
                    settings.UseSSL = ACSuseSSLCheckBox.Checked;
                    _xmlWrapper.SaveAccessControlServerSettings(settings);
                    success = true;
                }
            }
            catch(Exception ex)
            {
                Logging.LogFile_Error(Application.ProductName.ToString(), System.Reflection.MethodInfo.GetCurrentMethod().ToString() + " " + ex.Message);
                success = false;
            }
            return success;
        }

        private void LoadCameraAssociations()
        {
            try
            {
                CameraAssociations cameraAssociations = _xmlWrapper.GetCameraAssociations();
                if ((cameraAssociations != null)&&(cameraAssociations.CameraAssociation != null))
                {
                    cameraAssociationGridView.Rows.Clear();
                    foreach(CameraAssociation cameraAssociation in cameraAssociations.CameraAssociation)
                    {
                        string cameraName = GetCameraName(cameraAssociation.CameraId);
                        string peripheralName = cameraAssociation.ACSPeripheral;
                        string eventName =cameraAssociation.ACSEvent;

                        // todo: figure out why empty string is a problem here - maybe add empty string to datasources
                        if ((!string.IsNullOrEmpty(cameraName))&&(! string.IsNullOrEmpty(peripheralName))&&(! string.IsNullOrEmpty(eventName)))
                        {
                            cameraAssociationGridView.Rows.Add(
                                cameraName,
                                peripheralName,
                                eventName
                                );
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Logging.LogFile_Error(Application.ProductName.ToString(), System.Reflection.MethodInfo.GetCurrentMethod().ToString() + " " + ex.Message);
            }
        }

        private bool SaveCameraAssociations()
        {
            bool success = false;
            try
            {
                List<CameraAssociation> associationList = new List<CameraAssociation>();
                foreach(DataGridViewRow row in cameraAssociationGridView.Rows)
                {
                    if (row.Cells["CameraNameColumn"].Value != null)
                    {
                        CameraAssociation association = new CameraAssociation();
                        string cameraName = row.Cells["CameraNameColumn"].Value.ToString();
                        association.CameraId = GetCameraId(cameraName);

                        string peripheralName = row.Cells["PeripheralColumn"].Value.ToString();
                        association.ACSPeripheral = peripheralName;

                        string eventName = row.Cells["EventNameColumn"].Value.ToString();
                        association.ACSEvent = eventName;

                        if ((! string.IsNullOrEmpty(association.CameraId))&&
                            ((! string.IsNullOrEmpty(association.ACSPeripheral))||
                            (! string.IsNullOrEmpty(association.ACSEvent))))
                            associationList.Add(association);
                    }
                }
                CameraAssociations cameraAssociations = new CameraAssociations();
                cameraAssociations.CameraAssociation = associationList.ToArray();
                _xmlWrapper.SaveCameraAssociations(cameraAssociations);

                success = true;
            }
            catch(Exception ex)
            {
                Logging.LogFile_Error(Application.ProductName.ToString(), System.Reflection.MethodInfo.GetCurrentMethod().ToString() + " " + ex.Message);
                success = false;
            }
            return success;
        }

        private string GetCameraName(string cameraId)
        {
            string cameraName = string.Empty;
            try
            {
                if (_datasources != null)
                {
                    var datasource = _datasources.Find(x => x.Id == cameraId);
                    if (datasource != null)
                    {
                        cameraName = datasource.Name;
                    }
                }
            }
            catch {};
            return cameraName;
        }

        private string GetCameraId(string cameraName)
        {
            string cameraId = string.Empty;
            try
            {
                if (_datasources != null)
                {
                    var datasource = _datasources.Find(x => x.Name == cameraName);
                    if (datasource != null)
                    {
                        cameraId = datasource.Id;
                    }
                }
            }
            catch {};
            return cameraId;
        }

        private void LoadEventMap()
        {
            try
            {
                EventConfiguration eventConfiguration = _xmlWrapper.GetEventConfiguration();
                if ((eventConfiguration != null)&&(eventConfiguration.EventMap != null))
                {
                    eventMapDataGrid.Rows.Clear();
                    foreach(EventMap eventMap in eventConfiguration.EventMap)
                    {
                        string runScripts = string.Empty;
                        string ackScripts = string.Empty;
                        if (!string.IsNullOrEmpty(eventMap.RunScripts))
                        {
                            runScripts = eventMap.RunScripts;
                        }
                        if (!string.IsNullOrEmpty(eventMap.AckScripts))
                        {
                            ackScripts = eventMap.AckScripts;
                        }
                        eventMapDataGrid.Rows.Add(
                            eventMap.Direction,
                            eventMap.VxSituation.Type,
                            eventMap.ACSEvent,
                            runScripts,
                            ackScripts
                            );
                    }
                }
            }
            catch(Exception ex)
            {
                Logging.LogFile_Error(Application.ProductName.ToString(), System.Reflection.MethodInfo.GetCurrentMethod().ToString() + " " + ex.Message);
            }
        }

        private bool SaveEventMap()
        {
            bool success = false;
            try
            {
                List<EventMap> eventMapList = new List<EventMap>();
                foreach(DataGridViewRow row in eventMapDataGrid.Rows)
                {
                    if ((row.Cells["DirectionColumn"].Value != null)&&
                        (row.Cells["SituationColumn"].Value != null)&&
                        (row.Cells["ACSEventColumn"].Value != null))
                    {
                        EventMap eventMap = new EventMap();
                        eventMap.VxSituation = new VxSituation();
                        eventMap.VxSituation.Type = row.Cells["SituationColumn"].Value.ToString();
                        if (row.Cells["DirectionColumn"].Value != null)
                            eventMap.Direction = row.Cells["DirectionColumn"].Value.ToString();
                        string eventName = string.Empty;
                        if (row.Cells["ACSEventColumn"].Value != null)
                            eventMap.ACSEvent = row.Cells["ACSEventColumn"].Value.ToString();
                        if (row.Cells["RunScriptsColumn"].Value != null)
                            eventMap.RunScripts = row.Cells["RunScriptsColumn"].Value.ToString();
                        if (row.Cells["AckScriptsColumn"].Value != null)
                            eventMap.AckScripts = row.Cells["AckScriptsColumn"].Value.ToString();

                        eventMapList.Add(eventMap);
                    }
                }
                EventConfiguration eventConfiguration = new EventConfiguration();
                eventConfiguration.EventMap = eventMapList.ToArray();
                _xmlWrapper.SaveEventConfiguration(eventConfiguration);

                success = true;
            }
            catch(Exception ex)
            {
                Logging.LogFile_Error(Application.ProductName.ToString(), System.Reflection.MethodInfo.GetCurrentMethod().ToString() + " " + ex.Message);
                success = false;
            }
            return success;
        }

        private void LoadCustomSituations()
        {
            try
            {
                CustomSituations customSituations = _xmlWrapper.GetCustomSituations();
                if ((customSituations != null)&&(customSituations.customSituations != null))
                {
                    customSituationDataGrid.Rows.Clear();
                    foreach(CustomSituation customSituation in customSituations.customSituations)
                    {
                        string AutoAck = string.Empty;
                        try
                        {
                            if (customSituation.AutoAcknowledge >= 0)
                                AutoAck = Convert.ToString(customSituation.AutoAcknowledge);
                        }
                        catch { };
                        customSituationDataGrid.Rows.Add(
                            customSituation.Name,
                            Convert.ToString(customSituation.Severity),
                            customSituation.Log,
                            customSituation.Notify,
                            customSituation.DisplayBanner,
                            customSituation.ExpandBanner,
                            customSituation.Audible,
                            customSituation.AckNeeded,
                            AutoAck,
                            customSituation.SnoozeIntervals
                            );
                    }
                }
            }
            catch(Exception ex)
            {
                Logging.LogFile_Error(Application.ProductName.ToString(), System.Reflection.MethodInfo.GetCurrentMethod().ToString() + " " + ex.Message);
            }
        }

        private bool SaveCustomSituations()
        {
            bool success = false;
            try
            {
                List<CustomSituation> customSituationList = new List<CustomSituation>();
                foreach(DataGridViewRow row in customSituationDataGrid.Rows)
                {
                    if (row.Cells["TypeColumn"].Value != null)
                    {
                        CustomSituation customSituation = new CustomSituation();
                        customSituation.Name = row.Cells["TypeColumn"].Value.ToString();
                        customSituation.SituationType = "external/" + companyTextBox.Text + "/" + customSituation.Name;
                        customSituation.Log = Convert.ToBoolean(row.Cells["LogColumn"].Value);
                        customSituation.AckNeeded = Convert.ToBoolean(row.Cells["NeedAckColumn"].Value);
                        customSituation.Notify = Convert.ToBoolean(row.Cells["NotifyColumn"].Value);
                        customSituation.DisplayBanner = Convert.ToBoolean(row.Cells["BannerColumn"].Value);
                        customSituation.ExpandBanner = Convert.ToBoolean(row.Cells["ExpandBannerColumn"].Value);
                        customSituation.Audible = Convert.ToBoolean(row.Cells["AudibleColumn"].Value);
                        string autoAck = string.Empty;
                        if (row.Cells["AutoAckColumn"].Value != null)
                        {
                            autoAck = row.Cells["AutoAckColumn"].Value.ToString();
                        }
                        if (string.IsNullOrEmpty(autoAck))
                            customSituation.AutoAcknowledge = -1;
                        else
                            customSituation.AutoAcknowledge = Convert.ToInt32(autoAck);
                        customSituation.Severity = Convert.ToInt32(row.Cells["SeverityColumn"].Value);
                        if (row.Cells["SnoozeColumn"].Value != null)
                            customSituation.SnoozeIntervals = row.Cells["SnoozeColumn"].Value.ToString();
                        customSituationList.Add(customSituation);
                    }
                }
                CustomSituations customSituations = new CustomSituations();
                customSituations.customSituations = customSituationList.ToArray();
                _xmlWrapper.SaveCustomSituations(customSituations);
                success = true;
            }
            catch(Exception ex)
            {
                Logging.LogFile_Error(Application.ProductName.ToString(), System.Reflection.MethodInfo.GetCurrentMethod().ToString() + " " + ex.Message);
                success = false;
            }
            return success;
        }

        private void LoadScripts()
        {
            try
            {
                Scripts scripts = _xmlWrapper.GetScripts();
                if ((scripts != null)&&(scripts.scriptArray != null))
                {
                    scriptsDataGrid.Rows.Clear();
                    _scripts = scripts.scriptArray.ToList();
                    foreach(var script in _scripts)
                    {
                        // script number is repeated to determine when the script number is changed by user
                        scriptsDataGrid.Rows.Add(script.Number, script.Number, script.Name, "Action");
                    }
                }
            }
            catch(Exception ex)
            {
                Logging.LogFile_Error(Application.ProductName.ToString(), System.Reflection.MethodInfo.GetCurrentMethod().ToString() + " " + ex.Message);
            }
        }

        private bool SaveScripts()
        {
            bool success = false;
            try
            {
                Scripts scripts = new Scripts();
                scripts.scriptArray = _scripts.ToArray();
                _xmlWrapper.SaveScripts(scripts);
                success = true;
            }
            catch(Exception ex)
            {
                Logging.LogFile_Error(Application.ProductName.ToString(), System.Reflection.MethodInfo.GetCurrentMethod().ToString() + " " + ex.Message);
                success = false;
            }
            return success;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (_dirty)
            {
                if (!DoApply(true))
                    return;
            }
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            // Confirm closing
            if (_dirty)
            {
                DialogResult result = MessageBox.Show(Properties.Resources.AreYouSure, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result == System.Windows.Forms.DialogResult.No)
                    return;
            }

            this.Close();
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            DoApply(false);
        }

        /// <summary>
        /// Truncate Log Files
        /// </summary>
        private void TruncateLogFiles()
        {
            // Truncate Log files
            try
            {
                Logging.TruncateLogFile(true, progressBar1);
                Logging.TruncateLogFile(false, progressBar1);
            }
            catch (Exception ex)
            {
                Logging.LogFile_Error(Application.ProductName.ToString(), System.Reflection.MethodInfo.GetCurrentMethod().ToString() + " " + ex.Message);
            }
        }

        /// <summary>
        /// Progress Timer Tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _progressTimer_Tick(object sender, EventArgs e)
        {

            if (_cancelProcess)
                StopProgress();

            progressBar1.Value = _progressCount % 100;
            progressBar1.Refresh();
            System.Diagnostics.Trace.WriteLine(progressBar1.Value.ToString());
            _progressCount += 9;
        }

        /// <summary>
        /// Start Progress
        /// </summary>
        private void StartProgress()
        {
            panelProgress.Visible = true;
            _progressCount = 0;

            // %%%%%            _progressTimer.Start();
            progressBar1.Value = 0;
            progressBar1.Refresh();

            EnableButtons(false);
        }

        /// <summary>
        /// Stop Progress
        /// </summary>
        private void StopProgress()
        {
            _cancelProcess = false;
            // %%%%%            _progressTimer.Stop();
            panelProgress.Visible = false;
            EnableButtons(true);
        }

        /// <summary>
        /// Enable / Disable Buttons
        /// </summary>
        /// <param name="enable"></param>
        private void EnableButtons(bool enable)
        {
            buttonApply.Enabled = buttonCancel.Enabled = buttonOK.Enabled = buttonReload.Enabled = enable;
        }


        /// <summary>
        /// Do Apply
        /// </summary>
        /// <param name="exit"></param>
        /// <returns></returns>
        private bool DoApply(bool exit)
        {
            if (! VerifyData())
            {
                var result = MessageBox.Show("Errors have been detected, please fix any items indicated by red exclamations.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (! SaveACSSettings())
            {
                var result = MessageBox.Show("Save ACS Settings failed. Continue saving remainder of data?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                    return false;
            }
            if (! SaveVideoXpertSettings())
            {
                var result = MessageBox.Show("Save VideoXpertSettings failed. Continue saving remainder of data?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                    return false;
            }
            if (! SaveAccessControlServerSettings())
            {
                var result = MessageBox.Show("Save AccessControlServerSettings failed. Continue saving remainder of data?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                    return false;
            }
            if (! SaveCameraAssociations())
            {
                var result = MessageBox.Show("Save CameraAssociations failed. Continue saving remainder of data?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                    return false;
            }
            if (! SaveCustomSituations())
            {
                var result = MessageBox.Show("Save CustomSituations failed. Continue saving remainder of data?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                    return false;
            }
            if (! SaveEventMap())
            {
                var result = MessageBox.Show("Save EventMap failed.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.None);
                if (result == DialogResult.No)
                    return false;
            }
            if (! SaveScripts())
            {
                var result = MessageBox.Show("Save Scripts failed.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.None);
                if (result == DialogResult.No)
                    return false;
            }
            // start/restart service
            {
                string serviceName = "GenericAccessControlEventService";
                try
                {
                    ServiceController serviceController = ServiceController.GetServices().First(x => x.ServiceName.Equals(serviceName));
                    if (serviceController == null)
                    {
                        MessageBox.Show(Properties.Resources.SettingsHaveBeenSaved + "\r\n\r\nThe service " + serviceName + " must be installed for Event Injection to work.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Question);
                    }
                    else
                    {
                        DialogResult result;
                        if ((serviceController.Status == ServiceControllerStatus.Stopped)||(serviceController.Status == ServiceControllerStatus.StopPending))
                        {
                            result = MessageBox.Show(Properties.Resources.SettingsHaveBeenSaved + "\r\n\r\nDo you wish to start the " + serviceName + "?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        }
                        else // consider it to need restarted
                        {
                            result = MessageBox.Show(Properties.Resources.SettingsHaveBeenSaved + "\r\n\r\nDo you wish to restart the " + serviceName + "?  Any changes to configuration will require a restart of the service to take effect.", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        }
                        if (result == DialogResult.Yes)
                        {
                            // todo: start progress bar or place in a thread
                            if ((serviceController != null)&&((serviceController.Status != ServiceControllerStatus.Stopped)&&(serviceController.Status != ServiceControllerStatus.StopPending)))
                            {
                                serviceController.Stop();
                            }

                            // wait (up to 5 seconds) until service is stopped before trying to start
                            int msWait = 5000;
                            while(serviceController.Status != ServiceControllerStatus.Stopped)
                            {
                                // todo: update progress bar
                                Thread.Sleep(5);
                                msWait = msWait - 5;
                                if (msWait <= 0)
                                    break;  // out of while
                            }

                            // try to start service
                            serviceController.Start();
                            if (serviceController.StartType != ServiceStartMode.Automatic)
                                ChangeServiceStartMode(serviceName, "Automatic");
                        }
                    }
                }
                catch (Exception e)
                {
                    var result = MessageBox.Show("Unable to change state of the service.  " + serviceName + " may not be installed properly on this machine.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            _dirty = false;
            buttonApply.Enabled = false;
            if (!exit)
                MessageBox.Show(Properties.Resources.SettingsHaveBeenSaved, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

            return true;
        }

        private void ChangeServiceStartMode(string serviceName, string startMode)
        {
            try
            {
                ManagementObject classInstance =
                            new ManagementObject("Win32_Service.Name='" + serviceName + "'", null);

                // Obtain in-parameters for the method
                ManagementBaseObject inParams = classInstance.GetMethodParameters("ChangeStartMode");

                // Add the input parameters.
                inParams["StartMode"] = startMode;

                // Execute the method and obtain the return values.
                ManagementBaseObject outParams = classInstance.InvokeMethod("ChangeStartMode", inParams, null);
            }
            catch (ManagementException e)
            {
                Logging.LogFile_Error(Properties.Resources.Continuum, System.Reflection.MethodInfo.GetCurrentMethod().ToString() + " " + e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool VerifyData()
        {
            try
            {
                if (!VerifyCustomSituationData())
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                Logging.LogFile_Error(Application.ProductName.ToString(), System.Reflection.MethodInfo.GetCurrentMethod().ToString() + " " + ex.Message);
                return false;
            }
        }

        private bool VerifyCustomSituationData()
        {
            try
            {
                foreach(DataGridViewRow row in customSituationDataGrid.Rows)
                {
                    if (row.Cells["TypeColumn"].Value != null)
                    {
                        string situationType = row.Cells["TypeColumn"].Value.ToString();
                        if (! VerifySituationString(situationType))
                        {
                            row.Cells["TypeColumn"].ErrorText ="Custom Situation Types must be less than 64 characters in length and may not contain a forward slash or space.";
                            return false;
                        }
                        else
                        {
                            row.Cells["TypeColumn"].ErrorText = string.Empty;
                        }
                    }
                    if (row.Cells["SeverityColumn"].Value == null || Convert.ToInt32(row.Cells["SeverityColumn"].Value) < 1 || Convert.ToInt32(row.Cells["SeverityColumn"].Value) > 10)
                    {
                        // verify this row is populated and not last row in table
                        if (row.Cells["TypeColumn"].Value != null)
                        {
                            row.Cells["SeverityColumn"].ErrorText = "Severity must be between 1 and 10";
                            return false;
                        }
                    }
                    else
                    {
                        row.Cells["SeverityColumn"].ErrorText = string.Empty;
                    }
                    if (row.Cells["SnoozeColumn"].Value != null)
                    {
                        string snoozeIntervals = row.Cells["SnoozeColumn"].Value.ToString();
                        if (! string.IsNullOrEmpty(snoozeIntervals))
                        {
                            string[] snoozeArray = snoozeIntervals.Split(',');
                            foreach (string snoozeStr in snoozeArray)
                            {
                                bool err = false;
                                int snoozeInt = 0;
                                try
                                {
                                    snoozeInt = Convert.ToInt32(snoozeStr);
                                }
                                catch 
                                {
                                    err = true;
                                };
                                if ((err)||(snoozeInt < 0))
                                {
                                    row.Cells["SnoozeColumn"].ErrorText = "Snooze Intervals must be a comma delimited list of integers";
                                    return false;
                                }
                                else
                                {
                                    row.Cells["SnoozeColumn"].ErrorText = string.Empty;
                                }
                            }
                        }
                    }
                    if (row.Cells["AutoAckColumn"].Value != null)
                    {
                        string autoAckStr = row.Cells["AutoAckColumn"].Value.ToString();
                        if (! string.IsNullOrEmpty(autoAckStr))
                        {
                            bool err = false;
                            int autoAckInt = 0;
                            try
                            {
                                autoAckInt = Convert.ToInt32(autoAckStr);
                            }
                            catch 
                            {
                                err = true;
                            };
                            if ((err)||(autoAckInt < 0))
                            {
                                 row.Cells["AutoAckColumn"].ErrorText = "AutoAck must be a positive number of seconds";
                                return false;
                            }
                            else
                            {
                                row.Cells["AutoAckColumn"].ErrorText = string.Empty;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private bool VerifySituationString(string situationString)
        {
            if (string.IsNullOrEmpty(situationString) ||
                (situationString.Length > 1024) || 
                InCorrectNumberOfForwardSlashes(situationString) ||
                situationString.Contains(" "))
            {
                return false;
            }

            return true;
        }

        private bool InCorrectNumberOfForwardSlashes(string situationString)
        {
            var situationParts = situationString.Split('/');
            if (situationParts.Count() < 1 || situationParts.Count() > 3)
            {
                return true;
            }
            return false;
        }

        private bool VerifySnoozeString(string snoozeString)
        {
            if (string.IsNullOrEmpty(snoozeString) ||
                (snoozeString.Length > 64) )
            {
                return false;
            }

            string[] snoozeArray = snoozeString.Split(',');
            foreach (string snooze in snoozeArray)
            {
                int number = 0;
                bool isNum = int.TryParse(snooze, out number);
                if (! isNum)
                    return false;
            }            

            return true;
        }

        
        /// <summary>
        /// Button Reload clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonReload_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = DialogResult.OK;
                if (_dirty)
                {
                    result = MessageBox.Show("Refresh will lose all changes. Are you sure?", "Refresh Data", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                }

                if (result == DialogResult.OK)
                {
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logging.LogFile_Error(Application.ProductName.ToString(), System.Reflection.MethodInfo.GetCurrentMethod().ToString() + " " + ex.Message);
            }
        }

        private void textBoxUserName_TextChanged(object sender, EventArgs e)
        {
            SetDirty();
            errorProvider1.SetError(textBoxACSUserName, string.Empty);
        }

        void textBoxOdbcDSN_LostFocus(object sender, System.EventArgs e)
        {
        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
            SetDirty();
        }

        private void checkBoxOperation_CheckedChanged(object sender, EventArgs e)
        {
            SetDirty();
        }

        private void checkBoxError_CheckedChanged(object sender, EventArgs e)
        {
            SetDirty();
        }

        private void numericUpDownRetention_ValueChanged(object sender, EventArgs e)
        {
            SetDirty();
        }

        private void SetDirty()
        {
            buttonApply.Enabled = true;
            _dirty = true;
        }

        /// <summary>
        /// Cancel progress button clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancelProgress_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(Properties.Resources.ConfirmCancel, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                _cancelProcess = true;
            }
        }

        void DeleteCameraAssociation_Click(object sender, EventArgs e) 
        {
            MenuItem clickedItem = sender as MenuItem;
            string[] temp = clickedItem.Text.Split(' ');
            string rowNumberString = temp[2].TrimEnd(); // remove CR LF
            int rowNumber = (Convert.ToInt32(rowNumberString) - 1); // back to 0 based
            cameraAssociationGridView.Rows.RemoveAt(rowNumber);
            cameraAssociationGridView.Update();
            SetDirty();
        }

        private bool ConnectVxSystem(ref VxSdkNet.VXSystem vxSystem, string userName, string password, string ipAddress, int port, bool useSSL)
        {
            bool status = false;
            vxSystem = new VxSdkNet.VXSystem(ipAddress, port, useSSL, "DCovPywTKiY5LgolLiYsKCI/MywlBRUTdxAAD24MHw0OCAoI");

            var result = vxSystem.Login(userName, password);
            if (result == VxSdkNet.Results.Value.OK)
            {
                Trace.WriteLine(DateTime.Now.ToString() + " Logged into VideoXpert at " + ipAddress);
                status = true;
            }
            else if (result == VxSdkNet.Results.Value.SdkLicenseGracePeriodActive)
            {
                Trace.WriteLine(DateTime.Now.ToString() + " Logged into VideoXpert at " + ipAddress + " Warning: License Grace period active");
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
                    Trace.WriteLine(DateTime.Now.ToString() + " Unable to log user " + userName + " into VideoXpert at " + ipAddress);
                }
            }
            else
            {
                Trace.WriteLine(DateTime.Now.ToString() + " Unable to log user " + userName + " into VideoXpert at " + ipAddress);
                Trace.WriteLine("   Password " + password);
                Trace.WriteLine("   result " + result);
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

        private void TryConnectVideoXpertAsync()
        {
            if (ValidateVxSettings())
            {
                var connectVxTask = Task.Run(
                    () =>
                    {
                        int port = Convert.ToInt32(textBoxVxPort.Text);
                        ForceReconnect(textBoxVxIpAddress.Text, port, textBoxVxUserName.Text, textBoxVxPassword.Text);
                    });
            }
        }

        private void ForceReconnect(string ip, int port, string userName, string password)
        {
            if (_vxSystem != null)
            {
                _vxSystem.Dispose();
                _vxSystem = null;
            }
            _datasources = null;
            if (ConnectVxSystem(ref _vxSystem, userName, password, ip, port, true))
            {
                //appsconfig.VxSystemName = _vxSystem.Name;
                _datasources = _vxSystem.DataSources;
                _situations = _vxSystem.Situations;                
                _situationList.Clear();
                foreach(var situation in _situations)
                {
                    _situationList.Add(situation.Type);
                }
                _cameraList.Clear();
                foreach(var datasource in _datasources)
                {
                    if ((datasource != null)&&(datasource.Id != null))
                    {
                        if (datasource.Type == VxSdkNet.DataSource.Types.Video)
                        {
                            _cameraList.Add(datasource.Name);
                        }
                    }
                    else
                    {
                        Trace.WriteLine("Invalid DataSource found");
                    }
                }
                _situationList.Sort();
                _cameraList.Sort();

                VxConnectionPictureBox.Image = Properties.Resources.Green_Dot;
                _connectedVx = true;
                _connectionChanged = true;
            }
            else
            {
                VxConnectionPictureBox.Image = Properties.Resources.Red_Dot;                
                _connectedVx = false;
                _connectionChanged = true;
            }
        }

        private void TryConnectACSAsync()
        {
            if (ValidateACSSettings())
            {
                        List<string> acsEvents = new List<string>();
                        List<string> acsPeripherals = new List<string>();
                        _acsWrapper = new ACSWrapper(textBoxACSAddress.Text);
                        string ACSSysInfo = _acsWrapper.GetSysInfo();
                        if (_acsWrapper.Login(textBoxACSUserName.Text, textBoxACSPassword.Text))
                        {
                            _acsEventList.Clear();
                            acsEvents = _acsWrapper.GetEventList();
                            acsEvents.Sort();
                            _synchronizationContext.Post(new SendOrPostCallback(o =>
                            {
                                UpdateACSEventList(acsEvents);
                                AxConnectionPictureBox.Image = Properties.Resources.Green_Dot;
                                _connectedACS = true;
                                _connectionChanged = true;
                            }), null);

                            acsPeripherals = _acsWrapper.GetDoorNames();
                            acsPeripherals.Sort();
                            _synchronizationContext.Post(new SendOrPostCallback(o =>
                            {
                                UpdateACSPeripheralList(acsPeripherals);
                            }), null);
                        }
                        else
                        {
                            AxConnectionPictureBox.Image = Properties.Resources.Red_Dot;
                            _connectedACS = false;
                            _connectionChanged = true;
                        }                    
            }
        }

        private void UpdateACSEventList(List<string> acsEventList)
        {
            _acsEvents.Clear();
            foreach (string eventString in acsEventList)
            {
                _acsEvents.Add(eventString);
            }
            eventMapDataGrid.Update();
        }

        private void UpdateACSPeripheralList(List<string> acsPeripheralList)
        {
            _acsPeripherals.Clear();
            foreach (string eventString in acsPeripheralList)
            {
                _acsPeripherals.Add(eventString);
            }
            cameraAssociationGridView.Update();
        }

        private void ForceReconnectFeenics(string url, string userName, string password)
        {
            if (_acsWrapper != null)
            {
                _acsWrapper = null;
            }

            _acsWrapper = new ACSWrapper(url);
            string acsSysInfo = _acsWrapper.GetSysInfo();
            System.Diagnostics.Trace.WriteLine("ACSInfo: " + "\r\nHostUrl: " + url + "\r\n" + acsSysInfo);
            if (_acsWrapper.Login(userName, password))
            {
                AxConnectionPictureBox.Image = Properties.Resources.Green_Dot;
            }
            else
            {
                AxConnectionPictureBox.Image = Properties.Resources.Red_Dot;
            }
        }

        private void textBoxVxIpAddress_TextChanged(object sender, EventArgs e)
        {
            _vxSettingsChanged = true;
            SetDirty();
        }

        private void textBoxVxPort_TextChanged(object sender, EventArgs e)
        {
            _vxSettingsChanged = true;
            SetDirty();
        }

        private void textBoxVxUserName_TextChanged(object sender, EventArgs e)
        {
            _vxSettingsChanged = true;
            SetDirty();
        }

        private void textBoxVxPassword_TextChanged(object sender, EventArgs e)
        {
            _vxSettingsChanged = true;
            SetDirty();
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
        }

        private void SwitchVxSystem()
        {
        }

        private void companyNameTextBox_TextChanged(object sender, EventArgs e)
        {
            SetDirty();
        }

        private void situationDataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            errorProvider1.SetError(eventMapDataGrid, string.Empty);
            SetDirty();
        }

        private void customSituationDataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            errorProvider1.SetError(customSituationDataGrid, string.Empty);
            SetDirty();
            _customSituationsDirty = true;
        }

        private void eventMapDataGrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void UpdateVideoXpertSituations(int rowIndex = -1)
        {
            //if (_customSituationsDirty)
            {
                List<string> customTypesList = GetCustomSituationTypesFromScreen();
                foreach (string customType in customTypesList)
                {
                    if (! _situationList.Contains(customType))
                    {
                        if (VerifySituationString(customType))
                        {
                            _situationList.Add(customType);
                        }
                    }
                }
                _situationList.Sort();
                //_customSituationsDirty = false;
            }
        }

        private List<string> GetCustomSituationTypesFromScreen()
        {
            List<string> customTypesList = new List<string>();
            foreach(DataGridViewRow row in customSituationDataGrid.Rows)
            {
                if (row.Cells["TypeColumn"].Value != null)
                {
                    string customSituationType = string.Format("external/{0}/{1}",companyTextBox.Text, row.Cells["TypeColumn"].Value.ToString());
                    customTypesList.Add(customSituationType);
                }
            }
            return customTypesList;
        }

        private void customSituationDataGrid_Leave(object sender, EventArgs e)
        {
            VerifyCustomSituationData();
        }

        private void eventMapDataGrid_Leave(object sender, EventArgs e)
        {
        }

        private void eventMapDataGrid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            SetDirty();
        }

        private void customSituationDataGrid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            SetDirty();
            _customSituationsDirty = true;
        }

        private void eventMapDataGrid_MouseLeave(object sender, EventArgs e)
        {
        }

        private void customSituationDataGrid_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            VerifyCustomSituationData();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonReload.Visible = tabControl1.SelectedIndex == 0;

            if ((_connectionChanged) && (_connectedVx) && (_connectedACS))
            {
                LoadCustomSituations();
                LoadCameraAssociations();
                LoadEventMap();
                LoadScripts();
                _connectionChanged = false;
                _eventDataLoaded = true;
            }

            if ((_eventDataLoaded) && (tabControl1.SelectedIndex == 2)) // Events tab
            {
                //if (_customSituationsDirty)
                {
                    UpdateVideoXpertSituations();
                    _customSituationsDirty = false;
                }
            }
        }

        private void cameraAssociationGridView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int currentMouseOverRow = cameraAssociationGridView.HitTest(e.X,e.Y).RowIndex;
                if ((currentMouseOverRow >= 0)&&(cameraAssociationGridView.Rows.Count > 1))
                {
                    cameraAssociationGridView.ClearSelection();
                        
                    cameraAssociationGridView.Rows[currentMouseOverRow].Selected = true;

                    ContextMenu m = new ContextMenu();

                    MenuItem deleteItem = new MenuItem("Delete Row " + (currentMouseOverRow + 1)); // 1 based for display
                    m.MenuItems.Add(deleteItem);
                    deleteItem.Click += new EventHandler(DeleteCameraAssociation_Click);

                    m.Show(cameraAssociationGridView, new Point(e.X, e.Y));
                }
            }
        }

        private void buttonConnectACS_Click(object sender, EventArgs e)
        {
            AxConnectionPictureBox.Image = Properties.Resources.Red_Dot;
            TryConnectACSAsync();
        }

        private void buttonConnectVx_Click(object sender, EventArgs e)
        {
            VxConnectionPictureBox.Image = Properties.Resources.Red_Dot;
            TryConnectVideoXpertAsync();
        }

        private bool ValidateVxSettings()
        {
            bool valid = true;
            int port;
            if ((string.IsNullOrEmpty(textBoxVxIpAddress.Text))||(textBoxVxIpAddress.Text.Count() > 64))
            {
                valid = false;
            }
            else if ((string.IsNullOrEmpty(textBoxVxPort.Text)) || (! Int32.TryParse(textBoxVxPort.Text, out port)))
            {
                valid = false;
            }
            else if ((string.IsNullOrEmpty(textBoxVxUserName.Text))||(textBoxVxUserName.Text.Count() > 64))
            {
                valid = false;
            }
            else if ((string.IsNullOrEmpty(textBoxVxPassword.Text))||(textBoxVxPassword.Text.Count() > 64))
            {
                valid = false;
            }
            return valid;
        }

        private bool ValidateACSSettings()
        {
            bool valid = true;
            if (string.IsNullOrEmpty(textBoxACSAddress.Text))
            {
                valid = false;
            }
            else if (string.IsNullOrEmpty(textBoxACSUserName.Text))
            {
                valid = false;
            }
            else if (string.IsNullOrEmpty(textBoxACSPassword.Text))
            {
                valid = false;
            }
            return valid;
        }

        private void FormAdmin_FormClosing_1(object sender, FormClosingEventArgs e)
        {
        }

        private void eventMapDataGrid_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int currentMouseOverRow = eventMapDataGrid.HitTest(e.X,e.Y).RowIndex;
                if ((currentMouseOverRow >= 0)&&(eventMapDataGrid.Rows.Count > 1))
                {
                    eventMapDataGrid.ClearSelection();
                        
                    eventMapDataGrid.Rows[currentMouseOverRow].Selected = true;

                    ContextMenu m = new ContextMenu();

                    MenuItem deleteItem = new MenuItem("Delete Row " + (currentMouseOverRow + 1)); // 1 based for display
                    m.MenuItems.Add(deleteItem);
                    deleteItem.Click += new EventHandler(DeleteEventMap_Click);

                    m.Show(eventMapDataGrid, new Point(e.X, e.Y));
                }
            }
        }

        void DeleteEventMap_Click(object sender, EventArgs e) 
        {
            MenuItem clickedItem = sender as MenuItem;
            string[] temp = clickedItem.Text.Split(' ');
            string rowNumberString = temp[2].TrimEnd(); // remove CR LF
            int rowNumber = (Convert.ToInt32(rowNumberString) - 1); // back to 0 based
            eventMapDataGrid.Rows.RemoveAt(rowNumber);
            eventMapDataGrid.Update();
            SetDirty();
        }

        private void customSituationDataGrid_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int currentMouseOverRow = customSituationDataGrid.HitTest(e.X,e.Y).RowIndex;
                if ((currentMouseOverRow >= 0)&&(customSituationDataGrid.Rows.Count > 1))
                {
                    customSituationDataGrid.ClearSelection();
                        
                    customSituationDataGrid.Rows[currentMouseOverRow].Selected = true;

                    ContextMenu m = new ContextMenu();

                    MenuItem deleteItem = new MenuItem("Delete Row " + (currentMouseOverRow + 1)); // 1 based for display
                    m.MenuItems.Add(deleteItem);
                    deleteItem.Click += new EventHandler(DeleteCustomSituation_Click);

                    m.Show(customSituationDataGrid, new Point(e.X, e.Y));
                }
            }
        }

        void DeleteCustomSituation_Click(object sender, EventArgs e) 
        {
            MenuItem clickedItem = sender as MenuItem;
            string[] temp = clickedItem.Text.Split(' ');
            string rowNumberString = temp[2].TrimEnd(); // remove CR LF
            int rowNumber = (Convert.ToInt32(rowNumberString) - 1); // back to 0 based
            customSituationDataGrid.Rows.RemoveAt(rowNumber);
            customSituationDataGrid.Update();
            SetDirty();
        }

        private void eventMapDataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            SetDirty();
        }

        private void eventMapDataGrid_RowsRemoved_1(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            SetDirty();
        }

        private void cameraAssociationGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            SetDirty();
        }

        private void cameraAssociationGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            SetDirty();
        }

        private void customSituationDataGrid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            UpdateVideoXpertSituations(e.RowIndex);
        }

        private void textBoxACSIpAddress_TextChanged(object sender, EventArgs e)
        {
            SetDirty();
        }

        private void textBoxACSPort_TextChanged(object sender, EventArgs e)
        {
            SetDirty();
        }

        private void ACSuseSSLCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SetDirty();
        }

        private void ScriptsDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if ((senderGrid != null) && (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0))
            {
                try
                {
                    var row = scriptsDataGrid.Rows[e.RowIndex];
                    if (CheckForRepeatScriptNumber(row))
                    {
                        MessageBox.Show("Script Numbers must be unique and non-zero before editing Actions.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);                        
                    }
                    else // not repeat
                    {
                        if (row.Cells["ScriptNumberColumn"].Value != null)
                        {
                            string scriptNumberStr = row.Cells["ScriptNumberColumn"].Value.ToString();
                            Script script = _scripts.Find(x => x.Number == scriptNumberStr);
                            bool newScript = false;
                            if (script == null)
                            {
                                script = new Script();
                                script.Number = scriptNumberStr;
                                if (row.Cells["ScriptNameColumn"].Value != null)
                                    script.Name = row.Cells["ScriptNameColumn"].Value.ToString();
                                newScript = true;
                            }
                            if (script != null)
                            {
                                string titleString = "Actions for " + script.Name;
                                List<XMLUtilities.Action> actions = new List<XMLUtilities.Action>();
                                if (script.Actions != null)
                                    actions = script.Actions.ToList();
                                string[] ActionNames = {"SetLayout", "DisplayCamera", "DisconnectCamera", "GotoPreset", "RunPattern", "BookMark"};
                                string[] Layouts = {"1x1","1x2","2x1","2x2","2x3","3x2","3x3","4x3","4x4","1plus12","2plus8","3plus4","1plus5","1plus7","12plus1","8plus2","1plus4tall","1plus4wide"};                            
                                FormActions actionsForm = new FormActions(titleString, ref actions, ActionNames.ToList(), Layouts.ToList());
                                if (actionsForm.ShowDialog(this) == DialogResult.OK)
                                {
                                    script.Actions = actionsForm.Actions.ToArray();
                                    if (newScript)
                                    {
                                        _scripts.Add(script);
                                    }
                                    SetDirty();
                                }
                                actionsForm.Dispose();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logging.LogFile_Error(Application.ProductName.ToString(), System.Reflection.MethodInfo.GetCurrentMethod().ToString() + " " + ex.Message);
                }
            }
        }

        bool CheckForRepeatScriptNumber(DataGridViewRow rowToCheck)
        {
            bool repeat = false;
            string scriptNumberToCheck = string.Empty;
            if (rowToCheck.Cells["ScriptNumberColumn"].Value != null)
            {
                scriptNumberToCheck = rowToCheck.Cells["ScriptNumberColumn"].Value.ToString();
                if (scriptNumberToCheck == "0")
                {
                    repeat = true; // fail if 0
                }
                else
                {
                    foreach(DataGridViewRow row in scriptsDataGrid.Rows)
                    {
                        if (row != rowToCheck)
                        {
                            if (row.Cells["ScriptNumberColumn"].Value != null)
                            {
                                string scriptNumber = row.Cells["ScriptNumberColumn"].Value.ToString();
                                if (scriptNumber == scriptNumberToCheck)
                                {
                                    repeat = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return repeat;
        }

        private void scriptsDataGrid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            var row = scriptsDataGrid.Rows[e.RowIndex];
            if (row.Cells["DBNumberColumn"].Value == null)
            {
                row.Cells["DBNumberColumn"].Value = "0";
                row.Cells["ScriptNumberColumn"].Value = "0";
            }
        }

        private void scriptsDataGrid_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            //DataGridViewRow row = scriptsDataGrid.Rows[e.RowIndex];
            //CheckScriptChanged(row);
        }

        void CheckScriptChanged(DataGridViewRow row)
        {
            Script script = null;
            if (row.Cells["DBNumberColumn"].Value == null)  // new row, never been checked
            {
                row.Cells["DBNumberColumn"].Value = "0";
                if (row.Cells["ScriptNumberColumn"].Value == null)
                {
                    row.Cells["ScriptNumberColumn"].Value = "0";
                }
            }
            else
            {
                string dbNumber = row.Cells["DBNumberColumn"].Value.ToString();
                string Number = row.Cells["ScriptNumberColumn"].Value.ToString();
                if (dbNumber != Number)
                {
                    if (dbNumber == "0") // new row, add to scripts
                    {
                        script = _scripts.Find(x => x.Number == Number);
                        if (script != null) // a script with same number already exists
                        {
                            row.Cells["ScriptNumberColumn"].ErrorText = "Script number " + Number + " already exists.";
                        }
                        else
                        {
                            script = new Script();
                            script.Number = Number;
                            if (row.Cells["ScriptNameColumn"].Value != null)
                                script.Name = row.Cells["ScriptNameColumn"].Value.ToString();
                            _scripts.Add(script);
                            row.Cells["DBNumberColumn"].Value = Number; // data in synch with screen
                            row.Cells["ScriptNumberColumn"].ErrorText = string.Empty;
                        }
                    }
                    else // script number was changed, change in scripts
                    {
                        script = _scripts.Find(x => x.Number == dbNumber);
                        if (script != null) // found script to update
                        {
                            script.Number = Number;
                            row.Cells["DBNumberColumn"].Value = Number; // data in synch with screen
                        }
                        else // is this possible?
                        {
                        }
                    }
                }
                string Name = string.Empty;
                if (row.Cells["ScriptNameColumn"].Value != null)
                {
                    Name = row.Cells["ScriptNameColumn"].Value.ToString();
                    if (script == null)
                    {
                        script = _scripts.Find(x => x.Number == Number);
                    }
                    if (script != null) // found script to update
                    {
                        script.Name = Name;
                    }
                }

            }
        }

        private void scriptsDataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            SetDirty();        
        }

        private void scriptsDataGrid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            // remove same row from script list
            if ((_scripts != null) && (_scripts.Count() > e.RowIndex))
            {
                _scripts.RemoveAt(e.RowIndex);
                SetDirty();
            }
        }

        private void scriptsDataGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = scriptsDataGrid.Rows[e.RowIndex];
            CheckScriptChanged(row);
        }

        private void scriptsDataGrid_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int currentMouseOverRow = scriptsDataGrid.HitTest(e.X,e.Y).RowIndex;
                if ((currentMouseOverRow >= 0)&&(scriptsDataGrid.Rows.Count > 1))
                {
                    scriptsDataGrid.ClearSelection();
                        
                    scriptsDataGrid.Rows[currentMouseOverRow].Selected = true;

                    ContextMenu m = new ContextMenu();

                    MenuItem deleteItem = new MenuItem("Delete Row " + (currentMouseOverRow + 1)); // 1 based for display
                    m.MenuItems.Add(deleteItem);
                    deleteItem.Click += new EventHandler(DeleteScriptFromGrid_Click);

                    m.Show(scriptsDataGrid, new Point(e.X, e.Y));
                }
            }
        }

        void DeleteScriptFromGrid_Click(object sender, EventArgs e) 
        {
            MenuItem clickedItem = sender as MenuItem;
            string[] temp = clickedItem.Text.Split(' ');
            string rowNumberString = temp[2].TrimEnd(); // remove CR LF
            int rowNumber = (Convert.ToInt32(rowNumberString) - 1); // back to 0 based
            scriptsDataGrid.Rows.RemoveAt(rowNumber);
            scriptsDataGrid.Update();
            SetDirty();
        }

        private void eventMapDataGrid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            // make sure there is a selection for direction or it will crash
            var row = eventMapDataGrid.Rows[e.RowIndex];
            if (row.Cells["DirectionColumn"].Value == null)
            {
                row.Cells["DirectionColumn"].Value = "To VideoXpert";
            }
            SetDirty();
        }
    }
}
