namespace Sedodream.SelfPub.Runner {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.ServiceProcess;
    using System.Text;
    using System.Threading;
    using System.Timers;
    using Sedodream.SelfPub.Common;

    //public partial class DeploymentService: ServiceBase{
    //    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(DeploymentService));

    //    public DeploymentService() {
    //        log.Info("DeploymentService started");

    //        InitializeComponent();
    //    }

    //    protected override void OnStart(string[] args) {
    //        log.Info("OnStart called");
    //        base.OnStart(args);
    //    }


    //    protected override void OnStop() {
    //        log.Info("OnStop called");
    //        base.OnStop();
    //    }
    //}

    internal partial class DeploymentService : ServiceBase {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(DeploymentService));

        protected IServiceRunner ServiceRunner { get; set; }
        protected System.Timers.Timer Timer { get; set; }
        protected Config Config { get; set; }
        protected int ErrorCount { get; set; }


        public DeploymentService()
            : this(new ServiceRunner(), new Config()) {
        }

        public DeploymentService(IServiceRunner serviceRunner, Config config) {
            if (serviceRunner == null) { throw new ArgumentNullException("serviceRunner"); }
            if (config == null) { throw new ArgumentNullException("config"); }

            this.ServiceRunner = serviceRunner;
            this.Config = config;

            InitializeComponent();
        }

        void TimerElapsed(object sender, ElapsedEventArgs e) {
            log.Info("DeploymentService timer elapsed");

            if (this.ErrorCount > this.Config.GetAppSetting<int>(CommonStrings.Service.MaxErrorCount, 10)) {
                log.Error("Stopping service because too many error have occurred");
                this.Stop();
            }
            else {
                this.Timer.Stop();
                try {
                    this.ServiceRunner.Run();
                }
                catch (Exception ex) {
                    log.Error(ex);
                    this.ErrorCount++;
                }
                finally {
                    this.Timer.Start();
                }
            }

            this.ErrorCount = 0;
        }

        protected override void OnStart(string[] args) {
            log.Info(string.Format("OnStart called - [{0}]", this.GetType().FullName));

            /*if (args != null && args.Length > 0 && args[0].Contains("pauseOnStart")) */{
                // this will give a chance to attach a debugger to the service
                Thread.Sleep(1000 * 60);
            }

            this.Timer = new System.Timers.Timer();
            this.Timer.Interval = this.Config.GetAppSetting<int>(CommonStrings.Service.ServiceSleepIntervalSeconds, 60 * 5);
            this.Timer.Elapsed += TimerElapsed;
            
            this.Timer.Stop();
            this.ServiceRunner.Start();
            this.ServiceRunner.Run();
            log.Info("Starting DeploymentService timer");
            this.Timer.Start();
        }

        protected override void OnStop() {
            log.Info(string.Format("OnStop called - [{0}]", this.GetType().FullName));
            this.ServiceRunner.Stop(StopReason.ServiceStopRequested);
            this.Timer.Stop();
        }
    }
    


}
