using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XMLUtilities
{
    public class XMLWrapper
    {
        private static string customSituationsFileName = "CustomSituations.xml";
        private static string VideoXpertSettingsFileName = "VideoXpertSettings.xml";
        private static string EventConfigurationFileName = "EventConfiguration.xml";
        private static string AccessControlServerSettingsFileName = "AccessControlServerSettings.xml";
        private static string ACSSettingsFileName = "ACSSettings.xml";
        private static string CameraAssociationsFileName = "CameraAssociations.xml";
        private static string ScriptsFileName = "Scripts.xml";

        /// <summary>
        /// GetCustomSituations from XML File
        /// </summary>
        public CustomSituations GetCustomSituations()
        {
            string path = GetFullPath(customSituationsFileName);
            return GetCustomSituationSettings(path);
        }

        private CustomSituations GetCustomSituationSettings(string path)
        {
            CustomSituations situations = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(CustomSituations));
                Stream fs = File.OpenRead(path);

                situations = (CustomSituations)serializer.Deserialize(fs);
                fs.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Failed to load CustomSituations: " + ex.Message);
            }
            return situations;
        }

        /// <summary>
        /// SaveCustomSituations to XML File
        /// </summary>
        /// <param name="setting">Custom Situation Settings to write to disk.</param>
        public void SaveCustomSituations(CustomSituations setting)
        {
            string path = GetFullPath(customSituationsFileName);
            SaveXML<CustomSituations>(setting, path);
        }

        /// <summary>
        /// GetVideoXpertSettings from XML File
        /// </summary>
        public VideoXpertSettings GetVideoXpertSettings()
        {
            string path = GetFullPath(VideoXpertSettingsFileName);
            return GetVideoXpertSettings(path);
        }

        private VideoXpertSettings GetVideoXpertSettings(string path)
        {
            VideoXpertSettings settings = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(VideoXpertSettings));
                Stream fs = File.OpenRead(path);

                settings = (VideoXpertSettings)serializer.Deserialize(fs);
                fs.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Failed to load VideoXpertSettings: " + ex.Message);
            }
            return settings;
        }

        /// <summary>
        /// SaveVideoXpertSettings to XML File
        /// </summary>
        /// <param name="setting">VideoXpert Settings to write to disk.</param>
        public void SaveVideoXpertSettings(VideoXpertSettings setting)
        {
            string path = GetFullPath(VideoXpertSettingsFileName);
            SaveXML<VideoXpertSettings>(setting, path);
        }

        /// <summary>
        /// GetEventConfiguration from XML File
        /// </summary>
        public EventConfiguration GetEventConfiguration()
        {
            string path = GetFullPath(EventConfigurationFileName);
            return GetEventConfiguration(path);
        }

        private EventConfiguration GetEventConfiguration(string path)
        {
            EventConfiguration settings = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(EventConfiguration));
                Stream fs = File.OpenRead(path);

                settings = (EventConfiguration)serializer.Deserialize(fs);
                fs.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Failed to load EventConfiguration: " + ex.Message);
            }
            return settings;
        }

        /// <summary>
        /// SaveEventConfiguration to XML File
        /// </summary>
        /// <param name="setting">EventConfiguration Settings to write to disk.</param>
        public void SaveEventConfiguration(EventConfiguration setting)
        {
            string path = GetFullPath(EventConfigurationFileName);
            SaveXML<EventConfiguration>(setting, path);
        }

        /// <summary>
        /// GetACSSettings from XML File
        /// </summary>
        public ACSSettings GetACSSettings()
        {
            string path = GetFullPath(ACSSettingsFileName);
            return GetACSSettings(path);
        }

        private ACSSettings GetACSSettings(string path)
        {
            ACSSettings settings = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ACSSettings));
                Stream fs = File.OpenRead(path);

                settings = (ACSSettings)serializer.Deserialize(fs);
                fs.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Failed to load ACSSettings: " + ex.Message);
            }
            return settings;
        }

        /// <summary>
        /// SaveACSSettings to XML File
        /// </summary>
        /// <param name="setting">ACS Settings to write to disk.</param>
        public void SaveACSSettings(ACSSettings setting)
        {
            string path = GetFullPath(ACSSettingsFileName);
            SaveXML<ACSSettings>(setting, path);
        }

        /// <summary>
        /// GetScripts from XML File
        /// </summary>
        public Scripts GetScripts()
        {
            string path = GetFullPath(ScriptsFileName);
            return GetScripts(path);
        }

        private Scripts GetScripts(string path)
        {
            Scripts settings = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Scripts));
                Stream fs = File.OpenRead(path);

                settings = (Scripts)serializer.Deserialize(fs);
                fs.Close();
            }
            catch (Exception ex)
            {
                //Trace.WriteLine("Failed to load Scripts: " + ex.Message);
            }
            return settings;
        }

        /// <summary>
        /// SaveScripts to XML File
        /// </summary>
        /// <param name="setting">Script settings to write to disk.</param>
        public void SaveScripts(Scripts setting)
        {
            string path = GetFullPath(ScriptsFileName);
            SaveXML<Scripts>(setting, path);
        }

        /// <summary>
        /// GetAccessControlServerSettings from XML File
        /// </summary>
        public AccessControlServerSettings GetAccessControlServerSettings()
        {
            string path = GetFullPath(AccessControlServerSettingsFileName);
            return GetAccessControlServerSettings(path);
        }

        private AccessControlServerSettings GetAccessControlServerSettings(string path)
        {
            AccessControlServerSettings settings = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AccessControlServerSettings));
                Stream fs = File.OpenRead(path);

                settings = (AccessControlServerSettings)serializer.Deserialize(fs);
                fs.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Failed to load AccessControlServerSettings: " + ex.Message);
            }
            return settings;
        }

        /// <summary>
        /// SaveAccessControlServerSettings to XML File
        /// </summary>
        /// <param name="setting">AccessControlServer Settings to write to disk.</param>
        public void SaveAccessControlServerSettings(AccessControlServerSettings setting)
        {
            string path = GetFullPath(AccessControlServerSettingsFileName);
            SaveXML<AccessControlServerSettings>(setting, path);
        }

        /// <summary>
        /// GetCameraAssociations from XML File
        /// </summary>
        public CameraAssociations GetCameraAssociations()
        {
            string path = GetFullPath(CameraAssociationsFileName);
            return GetCameraAssociations(path);
        }

        private CameraAssociations GetCameraAssociations(string path)
        {
            CameraAssociations settings = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(CameraAssociations));
                Stream fs = File.OpenRead(path);

                settings = (CameraAssociations)serializer.Deserialize(fs);
                fs.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Failed to load CameraAssociations: " + ex.Message);
            }
            return settings;
        }

        /// <summary>
        /// SaveCameraAssociations to XML File
        /// </summary>
        /// <param name="setting">CameraAssociations Settings to write to disk.</param>
        public void SaveCameraAssociations(CameraAssociations setting)
        {
            string path = GetFullPath(CameraAssociationsFileName);
            SaveXML<CameraAssociations>(setting, path);
        }

        private string GetFullPath(string filename)
        {
            string path = GetCurrentPath();
            path += "\\" + filename;
            return path;
        }

        private string GetCurrentPath()
        {
            string path = string.Empty;
            var currentPath = new System.IO.DirectoryInfo(Assembly.GetExecutingAssembly().Location);
            path = System.IO.Path.GetDirectoryName(currentPath.FullName);
            return path;
        }

        private void SaveXML<T>(T xmlObject, string path)
        {
            XmlSerializer mySerializer = new XmlSerializer(typeof(T));
            StreamWriter myWriter = new StreamWriter(path);
            mySerializer.Serialize(myWriter, xmlObject);
            myWriter.Close();
        }
    }
}
