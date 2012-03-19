namespace Sedodream.SelfPub.Runner {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.ServiceProcess;
    using System.Text;

    public partial class DeploymentService : ServiceBase {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(DeploymentService));

        public DeploymentService() {
            InitializeComponent();
        }

        protected override void OnStart(string[] args) {
            log.Info(string.Format("OnStart called - [{0}]",this.GetType().FullName));

            throw new NotImplementedException();
        }

        protected override void OnStop() {
            log.Info(string.Format("OnStop called - [{0}]", this.GetType().FullName));    
        }
    }
}
