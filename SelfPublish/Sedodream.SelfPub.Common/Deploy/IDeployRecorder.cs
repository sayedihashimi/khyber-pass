namespace Sedodream.SelfPub.Common.Deploy {
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;
    using MongoDB.Driver;

    /// <summary>
    /// This will be used to keep track of what packages have been published on a particular
    /// runner.
    /// </summary>
    public interface IDeployRecorder {        
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

        protected internal IPackageRepository PackageRepository;
        
        private MongoDbDeployRecorder() {
            
        }

        ~MongoDbDeployRecorder() {
            log.Info("Mongodb Recorder closing down");
            // tear down MongoDB here
            if (this.MongoDbProcess != null) {

                try {
                    if (!this.MongoDbProcess.HasExited) {
                        this.MongoDbProcess.CloseMainWindow();
                        this.MongoDbProcess.WaitForExit(5 * 1000);
                    }
                }
                catch (Exception ex) {
                    string message = string.Format("Unable to stop the mongodb process, message: {0}{1}", ex.Message, Environment.NewLine);
                    log.Error(message, ex);
                }

                if (!this.MongoDbProcess.HasExited) {
                    log.Info("Killing mongodb process because it hasn't exited in time");
                    this.MongoDbProcess.Kill();
                }

                this.MongoDbProcess = null;

            }
        }

        private bool HasBeenInitalized { get; set; }

        private void Intialize() {
            this.StartMongoDb();

            this.PackageRepository = new MongoPackageRepository(new Config().GetConnectionString(CommonStrings.Deployer.MongoDbRunnerConnectionString).ConnectionString);
        }
        
        public static MongoDbDeployRecorder Instance {
            get {
                if (!MongoDbDeployRecorder.instance.HasBeenInitalized) {
                    MongoDbDeployRecorder.instance.Intialize();
                    MongoDbDeployRecorder.instance.HasBeenInitalized = true;
                }

                return MongoDbDeployRecorder.instance; 
            }
        }

        #region IDeployRecord implementation
        public bool HasPackageBeenPreviouslyDeployed(Guid id) {
            return this.PackageRepository.GetPackage(id) == null ? false : true;
        }

        public void RecordDeployedPackage(Package package) {

            this.PackageRepository.AddPackage(package);


            // this.RecordedPackagesCollection.Insert<Package>(package);
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
            log.Info("Starting mongodb");

            string mongoConnectionString = new Config().GetConnectionString(CommonStrings.Deployer.MongoDbRunnerConnectionString, required: true).ConnectionString;
            MongoUrlBuilder mub = new MongoUrlBuilder(mongoConnectionString);

            string dataPath = this.MongoDbDataDirPath.FullName.TrimEnd('\\');
            string args = string.Format(@"--dbpath ""{0}"" --port {1}", dataPath, mub.Server.Port);

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
                RedirectStandardError = true,

                CreateNoWindow = true,
                LoadUserProfile=true,
            };

            this.StartMongoProcess(psi);
            // give it a chance to start up
            
            log.InfoFormat("Started mongodb with pid: {0}{1}",this.MongoDbProcess.Id,Environment.NewLine);
            Thread.Sleep(2000);
        }

        // this will start the process and attach event handlers to it for logging
        private void StartMongoProcess(ProcessStartInfo psi) {
            if (psi == null) { throw new ArgumentNullException("psi"); }

            this.MongoDbProcess = Process.Start(psi);
            
            this.MongoDbProcess.EnableRaisingEvents = true;

            this.MongoDbProcess.ErrorDataReceived += (object sender, DataReceivedEventArgs e) => {
                log.ErrorFormat("Error from mongo db process: [{0}]{1}", e.Data, Environment.NewLine);
            };

            this.MongoDbProcess.OutputDataReceived += (object sender, DataReceivedEventArgs e) => {
                log.InfoFormat("Message from mongo.exe: [{0}]{1}",e.Data,Environment.NewLine);
            };

            this.MongoDbProcess.Exited += (object sender, EventArgs e) => {
                if (this.MongoDbProcess.ExitCode != 0) {
                    string message = string.Format("mongodb process has exited with a non-zero exit code: [{0}]",this.MongoDbProcess.ExitCode);
                    
                    log.Error(message);
                    log.Error(MongoDbProcess.StandardError.ReadToEnd());
                    log.Info(MongoDbProcess.StandardOutput.ReadToEnd());
                    throw new UnknownStateException(message);
                }
            };
        }

        void MongoDbProcess_OutputDataReceived(object sender, DataReceivedEventArgs e) {
            throw new NotImplementedException();
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

            this.MongoDbExePath = new FileInfo(Path.Combine(fullPathToMonboDbDir, "mongod.exe"));
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
            MongoPackageRepository mpr = this.PackageRepository as MongoPackageRepository;
            if (mpr != null) {
                mpr.Reset();
            }
        }
    }
}