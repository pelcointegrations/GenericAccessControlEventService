using System;
using System.Collections;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text.RegularExpressions;
using VxEventServer;

namespace GenericAccessControlEventService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += (domain, e) => OnUnhandledException((Exception)e.ExceptionObject);
            
            try
            {
                if (args.Any(arg => Regex.IsMatch(arg, "[/-]i(nstall){0,1}")))
                {
                    InstallService();
                    return;
                }

                if (args.Any(arg => Regex.IsMatch(arg, "[/-]u(ninstall){0,1}")))
                {
                    UninstallService();
                    return;
                }


                if (Environment.UserInteractive)
                {
                    Start(RunInteractive);
                }
                else
                {
                    Start(() => ServiceBase.Run(new GenericAccessControlEventService()));
                }
            }
            catch (Exception e)
            {
                OnUnhandledException(e);
            }
        }

        private static void InstallService()
        {
            var ti = GetInstaller();
            ti.Install(new Hashtable());
        }

        private static void UninstallService()
        {
            var ti = GetInstaller();
            ti.Uninstall(null);
        }

        private static TransactedInstaller GetInstaller()
        {
            var ti = new TransactedInstaller();
            ti.Installers.Add(new GenericAccessControlEventServiceInstaller());
            var path = String.Format("/assemblypath={0}", System.Reflection.Assembly.GetExecutingAssembly().Location);
            ti.Context = new InstallContext("", new string[] {path});
            return ti;
        }

        private static void Start(Action run)
        {
            run();
        }

        private static void RunInteractive()
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            using (VxEventServer.VxEventServerManager.Instance.Init())
            {
                if (!VxEventServer.VxEventServerManager.Instance.Initialized)
                    return;

                string command = string.Empty;
                CommandHelp();
                do
                {
                    try
                    {
                        command = Console.ReadLine();
                        if (command.ToUpper() == "STATUS")
                        {
                            VxEventServer.VxEventServerManager.Instance.PrintStatus();
                        }
                        else if ((command.ToUpper() == "H") || (command.ToUpper() == "?"))
                        {
                            CommandHelp();
                        }
                        else if (command.ToUpper().Contains("DEBUG"))
                        {
                            var commands = command.Split(' ');
                            string param = string.Empty;
                            if (commands.Length > 1)
                                param = commands[1];
                            try
                            {
                                int level = Convert.ToInt32(param);
                                VxEventServer.VxEventServerManager.Instance.SetDebugLevel(level);
                                Console.WriteLine("Debug level set to " + level);
                            }
                            catch
                            {
                                Console.WriteLine("Failed to set Debug level.  Make sure parameter is an integer.");
                            }
                        }
                        else if (command.ToUpper().Contains("DATASOURCE"))
                        {
                            var commands = command.Split(' ');
                            string param = string.Empty;
                            if (commands.Length > 1)
                                param = commands[1];
                            string response = VxEventServer.VxEventServerManager.Instance.ListDataSources(param);
                            Console.WriteLine(response);
                        }
                        else if (command.ToUpper().Contains("MONITOR"))
                        {
                            var commands = command.Split(' ');
                            string param = string.Empty;
                            if (commands.Length > 1)
                                param = commands[1];
                            string response = VxEventServer.VxEventServerManager.Instance.ListMonitors(param);
                            Console.WriteLine(response);
                        }
                        else if (command.ToUpper().Contains("SITUATIONS"))
                        {
                            var commands = command.Split(' ');
                            string param = string.Empty;
                            if (commands.Length > 1)
                                param = commands[1];
                            string response = VxEventServer.VxEventServerManager.Instance.ListSituations(param);
                            Console.WriteLine(response);
                        }
                        else if (command.ToUpper().Contains("EVENTINFO"))
                        {
                            var commands = command.Split(' ');
                            string param = string.Empty;
                            if (commands.Length > 1)
                            {
                                for (int i = 1; i < commands.Length; i++)
                                {
                                    param = param + " " + commands[i];
                                }
                                param = param.TrimStart();
                            }
                            string response = VxEventServer.VxEventServerManager.Instance.ListACSEventInformation(param);
                            Console.WriteLine(response);
                        }
                        else if (command.ToUpper().Contains("USER"))
                        {
                            var commands = command.Split(' ');
                            string param = string.Empty;
                            if (commands.Length > 1)
                            {
                                for (int i = 1; i < commands.Length; i++)
                                {
                                    param = param + " " + commands[i];
                                }
                                param = param.TrimStart();
                            }
                            string response = VxEventServer.VxEventServerManager.Instance.ListUsers(param);
                            Console.WriteLine(response);
                        }
                        else if (command.ToUpper().Contains("SETDOOR"))
                        {
                            var commands = command.Split(' ');
                            string param1 = string.Empty;
                            string param2 = string.Empty;
                            if (commands.Length > 1)
                            {
                                param1 = commands[1];
                                param1 = param1.TrimStart();
                            }
                            if (commands.Length > 2)
                            {
                                param2 = commands[2];
                                param2 = param2.TrimStart();
                            }
                            string response = VxEventServer.VxEventServerManager.Instance.SetDoorStatus(param1, param2);
                            Console.WriteLine(response);
                        }
                        else if (command.ToUpper().Contains("DOOR"))
                        {
                            var commands = command.Split(' ');
                            string param = string.Empty;
                            if (commands.Length > 1)
                            {
                                for (int i = 1; i < commands.Length; i++)
                                {
                                    param = param + " " + commands[i];
                                }
                                param = param.TrimStart();
                            }
                            string response = VxEventServer.VxEventServerManager.Instance.ListDoors(param);
                            Console.WriteLine(response);
                        }
                        else if (command.ToUpper().Contains("FAKE"))
                        {
                            VxEventServer.VxEventServerManager.Instance.CreateFakeACSEvent();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception in console command processing: " + e.Message);
                    }
                } while (command.ToUpper() != "QUIT");
            }
        }

        private static void OnUnhandledException(Exception exception)
        {
            Console.WriteLine(exception.Message);
        }

        private static void CommandHelp()
        {
            Console.WriteLine("Type  \"quit\" to close" );
            Console.WriteLine("Type \"DEBUG\" followed by an integer for the level.  Higher numbers output more verbose debug." );
            Console.WriteLine("Type \"STATUS\" to print current status" );
            Console.WriteLine("Type \"DATASOURCE\" optionally followed by part of a camera name to list datasources");
            Console.WriteLine("Type \"MONITOR\" optionally followed by part of a monitor name to list monitors");
            Console.WriteLine("Type \"SITUATIONS\" followed by part of a situation name to list situations\n" +
                                "                  containing that substring.\n" +
                                "                  (if blank, all situations will be listed)\n");
            Console.WriteLine("Type \"EVENTINFO\" followed by part of an application name to list events\n" +
                                "                  containing that substring.\n" +
                                "                  (if blank, all events will be listed)\n");
            Console.WriteLine("Type \"USERS\" followed by a partial user name to filter on if desired\n");
            Console.WriteLine("Type \"DOORS\" followed by a partial door name to filter on if desired\n");
            Console.WriteLine("Type \"SETDOOR\" followed by a partial door name and status\n");
            Console.WriteLine("Type \"FAKEACSEVENT\"\n");
            Console.WriteLine("Press \"H\" for help");
        }

    }
}
