namespace Sedodream.SelfPub.Runner {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.ServiceProcess;
    using System.Text;
    using log4net.Config;
    using Sedodream.SelfPub.Common.Deploy;

    static class Program {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Program));
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args) {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Console.WriteLine("Deployment runner starting");
            XmlConfigurator.Configure();

            StartService();

            return;


            try {
                log.Info("Deployment runner starting");

                bool startedFromCmdLine = false;
                if (args != null && args.Length > 0 && !string.IsNullOrEmpty(args[0])) {
                    // see if it is __debug
                    if (string.Compare("__debug", args[0], StringComparison.Ordinal) == 0) {
                        startedFromCmdLine = true;
                    }
                }

                if (startedFromCmdLine) {
                    StartCommandLine();
                }
                else {
                    StartService();
                }
            }
            catch (Exception ex) {
                log.Fatal(ex);
                throw;
            }
        }

        private static void StartService() {
            log.Info("Deployment runner service starting");
            
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new DeploymentService() 
                //new DummyService()
            };
            ServiceBase.Run(ServicesToRun);

            log.Info("Deployment runner service closing");
        }

        private static void StartCommandLine() {
            log.Info("Deployment runner command line starting");

            IDeployer deployer = new Deployer();
            deployer.Start();

            Console.WriteLine("Press ENTER to continue");
            Console.ReadLine();

            log.Info("Deployment runner command line closing");
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
            Exception ex = sender as Exception;
            if (ex != null) {
                log.Error("Unhandled exception", ex);
            }
            else {
                log.Error("Unhandled exception, type unknown");
            }
        }
    }
}
