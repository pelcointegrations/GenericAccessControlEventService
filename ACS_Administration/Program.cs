using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _VX_ACS_Administration
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // if mediacontroller is linked in we need to set the path to GStreamer
            //SetGStreamerPath();

            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(FormAdmin.UIThreadExceptionHandler);

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormAdmin());
        }

        //private static void SetGStreamerPath()
        //{
        //    //var gstreamerPath = GetGStreamerPath();
        //    var oldPath = Environment.GetEnvironmentVariable("PATH");
        //    var newPath = @"C:\ProgramData\Feenics\Plugins\Pelco;C:\ProgramData\Feenics\Plugins\Pelco\gstreamer;" + oldPath;
        //    // Add the gstreamer location to the path.
        //    Environment.SetEnvironmentVariable("PATH", newPath);
        //}

        public static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = (Exception)e.ExceptionObject;
                string errorMsg = Properties.Resources.ContactAdministrator;

                if (!EventLog.SourceExists("ThreadException"))
                {
                    EventLog.CreateEventSource("ThreadException", "Application");
                }

                // Create an EventLog instance and assign its source.
                EventLog myLog = new EventLog();
                myLog.Source = "ThreadException";
                myLog.WriteEntry(errorMsg + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace);
            }
            catch(Exception ex)
            {
                try
                {
                    MessageBox.Show(Properties.Resources.FatalNonUIError + ex.Message, Properties.Resources.Continuum, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Application.Exit();
                }
            }
        }
    }
}
