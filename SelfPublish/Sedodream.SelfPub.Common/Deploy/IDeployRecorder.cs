namespace Sedodream.SelfPub.Common.Deploy {
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using MongoDB.Driver;

    /// <summary>
    /// This will be used to keep track of what packages have been published on a particular
    /// runner.
    /// </summary>
    public interface IDeployRecorder {
        bool HasPackageBeenPreviouslyDeployed(Package package);
        
        bool HasPackageBeenPreviouslyDeployed(Guid id);
        
        void RecordDeployedPackage(Package package);
    }

    /// <summary>
    /// This will record all the packages which have been previously deployed into a
    /// mongodb in the specified folder in appSettings "dataFolder".<br />
    /// This is a singleton class.
    /// </summary>
    public class MongoDbDeployRecorder :IDeployRecorder {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(MongoDbDeployRecorder));
        private static MongoDbDeployRecorder instance = new MongoDbDeployRecorder();
        
        private FileInfo MongoDbExePath { get; set; }
        private DirectoryInfo MongoDbDataDirPath { get; set; }
        private Process MongoDbProcess { get; set; }

        protected internal MongoServer Server { get; set; }
        protected internal MongoDatabase Database { get; set; }
        protected MongoCollection RecordedPackagesCollection {
            get {
                return this.Database.GetCollection(CommonStrings.Database.CollectionName);
            }
        }

        private MongoDbDeployRecorder() {
            this.StartMongoDb();
        }

        ~MongoDbDeployRecorder() {
            // tear down MongoDB here
            if (this.MongoDbProcess != null) {
                this.MongoDbProcess.CloseMainWindow();
                this.MongoDbProcess.WaitForExit(5 * 1000);
                if (!this.MongoDbProcess.HasExited) {
                    this.MongoDbProcess.Kill();
                }
                this.MongoDbProcess = null;
            }
        }

        public static MongoDbDeployRecorder Instance {
            get { return MongoDbDeployRecorder.instance; }
        }

        #region IDeployRecord implementation
        public bool HasPackageBeenPreviouslyDeployed(Package package) {
            return this.HasPackageBeenPreviouslyDeployed(package.Id);
        }

        public bool HasPackageBeenPreviouslyDeployed(Guid id) {
            throw new NotImplementedException();
        }

        public void RecordDeployedPackage(Package package) {
            this.RecordedPackagesCollection.Insert<Package>(package);
        }
        #endregion

        internal void StartMongoDb() {
            this.PrepareMongoDirectory();
            // now both MongoDbExePath and MongoDbDataDirPath should be non-null and on disk
            if (this.MongoDbExePath == null || !this.MongoDbExePath.Exists) {
                string message = string.Format("Expected mongo.exe to be not-null and on disk, but it isn't. Value: [{0}] ",this.MongoDbExePath);
                log.Error(message);
                throw new UnknownStateException(message);
            }
            if (this.MongoDbDataDirPath == null || !this.MongoDbDataDirPath.Exists) {
                string message = string.Format("Expected the mongo data dir to be not-null and on disk, but it isn't. Value: [{0}] ", this.MongoDbExePath);
                log.Error(message);
                throw new UnknownStateException(message);
            }

            string mongoConnectionString = new Config().GetConnectionString(CommonStrings.Deployer.MongoDbRunnerConnectionString, required: true).ConnectionString;
            MongoUrlBuilder mub = new MongoUrlBuilder(mongoConnectionString);

            string args = string.Format(@"--dbpath ""{0}"" --port {1}", this.MongoDbDataDirPath.FullName, mub.Server.Port);

            log.InfoFormat(
                "Starting mongodb: {0} {1}{2}",
                this.MongoDbExePath.FullName,
                args,
                Environment.NewLine);

            var psi = new ProcessStartInfo {
                FileName = this.MongoDbExePath.FullName,
                WorkingDirectory = this.MongoDbExePath.Directory.FullName,
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            MongoDbProcess = Process.Start(psi);

            log.Info("Started mongodb");

            this.Server = MongoServer.Create(mub.ToMongoUrl());
            this.Server.Connect();
            log.Info("Now connected to mongodb server");

            if (!Server.DatabaseExists(mub.DatabaseName)) {
                var db = this.Server.GetDatabase(mub.DatabaseName);
                // create the collection
                db.CreateCollection(CommonStrings.Deployer.RecordedPackagesCollectionName);
            }

            this.Database = this.Server.GetDatabase(mub.DatabaseName);
        }

        protected internal void PrepareMongoDirectory() {
            // initalize MongoDB here
            string mongoDbDir = new Config().GetAppSetting<string>(CommonStrings.Deployer.DataFolder, required: true);

            // see if it is a relative or full path
            string fullPathToMonboDbDir = mongoDbDir;
            if (!Path.IsPathRooted(mongoDbDir)) {
                fullPathToMonboDbDir = Path.Combine(this.GetPathToService(), mongoDbDir);
            }

            if (!Directory.Exists(fullPathToMonboDbDir)) {
                string message = string.Format("mongodb directory not found at [{0}]", fullPathToMonboDbDir);
                throw new ConfigurationErrorsException(message);
            }

            this.MongoDbExePath = new FileInfo(Path.Combine(fullPathToMonboDbDir, "mongo.exe"));
            if (!this.MongoDbExePath.Exists) {
                string message = string.Format("mongo.exe not found at [{0}]", this.MongoDbExePath.FullName);
                throw new ConfigurationErrorsException(message);
            }

            this.MongoDbDataDirPath = new DirectoryInfo(Path.Combine(fullPathToMonboDbDir, @"data\"));
            if (!MongoDbDataDirPath.Exists) {
                log.InfoFormat("Creating mongodb data directory: [{0}]{1}", this.MongoDbDataDirPath.FullName, Environment.NewLine);
                this.MongoDbDataDirPath.Create();
            }
        }
            
        protected internal string GetPathToService() {
            return new FileInfo(this.GetType().Assembly.Location).Directory.FullName;
        }

        /// <summary>
        /// Should only be used by unit tests
        /// </summary>
        internal void Reset() {
            if (this.RecordedPackagesCollection.Exists()) {
                this.RecordedPackagesCollection.Drop();
            }
        }
    }
}
