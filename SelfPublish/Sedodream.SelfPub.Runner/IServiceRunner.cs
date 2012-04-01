namespace Sedodream.SelfPub.Runner {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Sedodream.SelfPub.Common;
    using Sedodream.SelfPub.Common.Deploy;

    public interface IServiceRunner {
        void Run();
        void Start();
        void Stop(StopReason reason);
    }

    public enum StopReason {
        StopFileFound,
        ServiceStopRequested
    }

    public class DeployServiceRunner : IServiceRunner {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(DeployServiceRunner));
        private IDeployer Deployer { get; set; }

        public DeployServiceRunner()
            : this(new Deployer()) {
        }

        public DeployServiceRunner(IDeployer deployer) {
            this.Deployer = deployer;
        }

        public void Run() {
            this.Deployer.Start();
        }

        public void Start() {
            log.Info("Starting ServiceRunner");
            this.Run();
        }

        public void Stop(StopReason reason) {
            log.InfoFormat("ServiceRunner stopping, reason: [{0}]{1}", reason.ToString(), Environment.NewLine);
        }
    }
}
