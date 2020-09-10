using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _VX_ACS_Administration
{
    public class Logging
    {
        private const string _tabKey = ","; // "\t";

        public static int iRetention;
        public static string LogFilePath;
        public static string ProductName;

        public static bool ErrorLog { get; set; }
        public static bool OperationLog { get; set; }
        public static bool DebugLog { get; set; }
        public static bool TraceLog { get; set; }

        public static void LogFile_Trace(string operationtype, string message)
        {
            if (TraceLog)
            {
                System.Diagnostics.Trace.WriteLine(operationtype + " " + message);
            }
        }

        public static bool LogFile_Error(string errortype, string message)
        {
            LogFile_Trace(errortype, message);
            if (ErrorLog)
            {
                string csvFile = LogFilePath + @"\VX_PlugIn_Administration_Errlog.csv";
                try
                {
                    //                string path = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf('\\'));

                    // Error log file
                    if (!File.Exists(csvFile))
                        File.WriteAllText(csvFile, Properties.Resources.Timestamp + _tabKey + Properties.Resources.Description + Environment.NewLine);

                    message = message.Replace(",", " -");
                    File.AppendAllText(csvFile, DateTime.Now + _tabKey + message + Environment.NewLine);
                    /*                
                                    if (!File.Exists(csvFile))
                                        File.WriteAllText(csvFile, Properties.Resources.Timestamp + _tabKey + Properties.Resources.Module + _tabKey + Properties.Resources.Description + Environment.NewLine);

                                    File.AppendAllText(csvFile, DateTime.Now + _tabKey + errortype + _tabKey + message + Environment.NewLine);
                    */
                }
                catch
                {
//                    MessageBox.Show(ex.Message, Properties.Resources.AccessGroup, MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    File.AppendAllText(csvFile, DateTime.Now + _tabKey + message + Environment.NewLine);
                }
            }

            return true;
        }

        public static bool LogFile_Operation(string operationtype, string message)
        {
            LogFile_Trace(operationtype, message);
            if (OperationLog)
            {
                string csvFile = LogFilePath + @"\VX_PlugIn_Administration_Log.csv";

                try
                {
                    // Error log file
                    if (!File.Exists(csvFile))
                        File.WriteAllText(csvFile, Properties.Resources.Timestamp + _tabKey + Properties.Resources.Description + Environment.NewLine);

                    message = message.Replace(",", " -");
                    File.AppendAllText(csvFile, DateTime.Now + _tabKey + message + Environment.NewLine);
                }
                catch
                {
//                   MessageBox.Show(ex.Message, Properties.Resources.AccessGroup, MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    File.AppendAllText(csvFile, DateTime.Now + _tabKey + message + Environment.NewLine);
                }
            }

            return true;
        }

        public static bool LogFile_Debug(string operationtype, string message)
        {
            LogFile_Trace(operationtype, message);
            if (DebugLog)
            {
                string csvFile = LogFilePath + @"\VX_PlugIn_Administration_DebugLog.csv";

                try
                {
                    // Error log file
                    if (!File.Exists(csvFile))
                        File.WriteAllText(csvFile, Properties.Resources.Timestamp + _tabKey + Properties.Resources.Description + Environment.NewLine);

                    message = message.Replace(",", " -");
                    File.AppendAllText(csvFile, DateTime.Now.ToLongTimeString() + _tabKey + message + Environment.NewLine);
                }
                catch
                {
//                    MessageBox.Show(ex.Message, Properties.Resources.AccessGroup, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //                    File.AppendAllText(csvFile, DateTime.Now + _tabKey + message + Environment.NewLine);
                }
            }

            return true;
        }

/*        /// <summary>
        /// Truncate Log File
        /// </summary>
        /// <param name="error"></param>
        /// <param name="latestDate"></param>
        /// <returns></returns>
        public static bool TruncateLogFile1(bool error)
        {
            try
            {
//                string path = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf('\\'));
                string logFile = LogFilePath + @"\AccessGroup_" + (error ? "Errlog" : "Log") + ".csv";
                DateTime latestDate = DateTime.Now - new TimeSpan(iRetention, 0, 0, 0);

                string[] lines = File.ReadAllLines(logFile);
                int k = 1;
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] columns = lines[i].Split(',', '\"');
                    DateTime dateTime = DateTime.Parse(columns[0]);

                    if (latestDate <= dateTime)
                    {
                        k = i;
                        break;
                    }
                }

                if (k > 1)
                {
                    File.Delete(logFile);

                    File.WriteAllText(logFile, Properties.Resources.Timestamp + _tabKey + Properties.Resources.Description + Environment.NewLine);
                    File.AppendAllLines(logFile, lines.Skip<string>(k));
                }

                return true;
            }
            catch (Exception ex)
            {
                Logging.LogFile_Error(Application.ProductName.ToString(), System.Reflection.MethodInfo.GetCurrentMethod().ToString() + " " + ex.Message);
                return false;
            }
        }
*/
        /// <summary>
        /// Truncate Log File
        /// </summary>
        /// <param name="error"></param>
        /// <param name="latestDate"></param>
        /// <returns></returns>
        public static bool TruncateLogFile(bool error, ProgressBar progressBar)
        {
            try
            {
                string logFile = LogFilePath + @"\VX_PlugIn_Administration_" + (error ? "Errlog" : "Log") + ".csv";
                string logFileTemp = LogFilePath + @"\VX_PlugIn_Administration_" + (error ? "Errlog" : "Log") + ".tmp";

                if (!File.Exists(logFile))
                    return true;

                if (File.Exists(logFileTemp))
                    File.Delete(logFileTemp);

                File.WriteAllText(logFileTemp, Properties.Resources.Timestamp + _tabKey + Properties.Resources.Description + Environment.NewLine);

                DateTime latestDate = DateTime.Now - new TimeSpan(iRetention, 0, 0, 0);

                string[] lines = File.ReadAllLines(logFile);

                for (int i = 1; i < lines.Length; i++)
                {
                    int percent = i * 100 / lines.Length;
                    progressBar.Value = percent;
                    progressBar.Refresh();

                    Application.DoEvents();
                    string[] columns = lines[i].Split(',', '\"');
                    try
                    {
                        DateTime dateTime = DateTime.Parse(columns[0]);

                        if (latestDate <= dateTime)
                            File.AppendAllText(logFileTemp, lines[i] + Environment.NewLine);
                    }
                    catch (Exception)
                    {
                    }

                }

                try
                {
                    File.Copy(logFileTemp, logFile, true);
                    File.Delete(logFileTemp);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return true;
            }
            catch (Exception ex)
            {
                Logging.LogFile_Error(Application.ProductName.ToString(), System.Reflection.MethodInfo.GetCurrentMethod().ToString() + " " + ex.Message);
                return false;
            }
        }
    }
}
