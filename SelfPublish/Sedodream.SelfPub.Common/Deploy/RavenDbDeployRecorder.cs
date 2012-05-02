namespace Sedodream.SelfPub.Common.Deploy {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class RavenDbDeployRecorder : IDeployRecorder {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(RavenDbDeployRecorder));
        private static RavenDbDeployRecorder instance = new RavenDbDeployRecorder();

        private RavenDbPackageRepository RavenRepo;

        private RavenDbDeployRecorder() {
            this.InitalizeRavenDb();            
        }

        private void InitalizeRavenDb() {
            // get path to the RavenDB directory
            string dataDirPath = new Config().GetAppSetting<string>(CommonStrings.Deployer.RavenDataDir, required: true);

            // see if it is a relative or full path
            if (!Path.IsPathRooted(dataDirPath)) {
                dataDirPath = Path.Combine(this.GetPathToService(), dataDirPath);
            }

            if (!Directory.Exists(dataDirPath)) {
                string message = string.Format("RavenDB data directory doesn't exist, creating it [{0}]",dataDirPath);
                log.Debug(message);
                Directory.CreateDirectory(dataDirPath);
            }

            this.RavenRepo = RavenDbPackageRepository.GetRavenDbRepoFor(dataDirPath);
        }

        protected internal string GetPathToService() {
            return new FileInfo(this.GetType().Assembly.Location).Directory.FullName;
        }

        public static RavenDbDeployRecorder Instance {
            get {
                return RavenDbDeployRecorder.instance;
            }
        }

        public bool HasPackageBeenPreviouslyDeployed(Guid id) {
            Package foundPackage = this.RavenRepo.GetPackage(id);

            return foundPackage != null;
        }

        public void RecordDeployedPackage(Package package) {
            if (package == null) { throw new ArgumentNullException("package"); }

            this.RavenRepo.AddPackage(package);
        }

        // Should only be called by the unit-tests
        internal void Reset() {
            RavenDbDeployRecorder.Instance.Reset();
        }
    }
}
