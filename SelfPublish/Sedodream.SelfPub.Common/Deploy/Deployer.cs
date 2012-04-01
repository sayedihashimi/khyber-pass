namespace Sedodream.SelfPub.Common.Deploy {
    using System;
    using System.Collections.Generic;
    using System.Json;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Sedodream.SelfPub.Common.Exceptions;

    /// <summary>
    /// This is the object which will execute the deployment<br/>
    /// Every deployer can handle a single deployment artifact. If you need to deploy more than
    /// one thing on the same machine then you need to configure 1 deployer for each app.
    /// </summary>
    public interface IDeployer {
        void Start();
    }

    public class Deployer : IDeployer {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Deployer));
        private DeployerConfig Config { get; set; }
        private IJsonSearlizer Searlizer { get; set; }
        private IDeployRecorder DeployRecorder { get; set; }

        public Deployer()
            : this(new JsonNetSearlizer(),MongoDbDeployRecorder.Instance) {
        }

        public Deployer(IJsonSearlizer searlizer,IDeployRecorder recorder) {
            if (searlizer == null) { throw new ArgumentNullException("searlizer"); }

            this.Searlizer = searlizer;
            this.DeployRecorder = recorder;
        }

        public void Start() {
            try {
                // get machine config
                // TODO: We can fetch this form a service somewhere
                this.Config = DeployerConfig.BuildFromAppConfig();
                log.Debug("Getting package to deploy");
                
                // get latest package
                Package packageToDeploy = this.GetLatestPackage();
                if (packageToDeploy == null) {
                    log.WarnFormat("Did not find a package to deploy, skipping deployment");
                }
                else if (!this.DeployRecorder.HasPackageBeenPreviouslyDeployed(packageToDeploy.Id)) {
                    log.Debug("Getting deploy handler");

                    // get package handler
                    IDeployHandler deployHandler = this.GetDeployHandlerFor(packageToDeploy);

                    log.Debug("Starting deployment");
                    deployHandler.HandleDeployment(packageToDeploy, this.Config.DeploymentParameters);

                    // now record the deployment so that we don't do it again
                    this.DeployRecorder.RecordDeployedPackage(packageToDeploy);
                }
                else {
                    log.InfoFormat("Skipping package deployment becausse the package has already been deployed, package id [{0}]{1}", packageToDeploy.Id, Environment.NewLine);
                }
            }
            catch (Exception ex) {
                log.Fatal(ex);
            }
        }

        /// <summary>
        /// For the most part this will inspect the <c>package.PackageType</c>, but maybe
        /// later we will want to use some other values for inspection as well
        /// </summary>
        public IDeployHandler GetDeployHandlerFor(Package package) {

            if (string.Compare(KnownPackageTypes.msdeploy.ToString(), package.PackageType) == 0) {
                return new MSDeployHandler();
            }
            else {
                string message = string.Format(@"Unable to determine the deployment handler for package type: {0}", package.PackageType);
                log.Error(message);
                throw new UnknownPackageHandlerException(message);
            }
        }

        public virtual Package GetLatestPackage() {
            Package result = null;
            string url = string.Format(@"{0}config/packages/latest/{1}", this.Config.GetConfigServiceBaseUrl, this.Config.PackageNameToDeploy);
            log.Debug(string.Format("Getting latest package using URL [{0}]", url));

            HttpClient client = new HttpClient();
            // TODO: Get timeout from config
            client.Timeout = new TimeSpan(0, 0, 15);

            Task continueTask = client.GetAsync(url).ContinueWith(resp => {
                HttpResponseMessage response = resp.Result;
                response.EnsureSuccessStatusCode();

                Task<Package> taskPackage = response.Content.ReadAsAsync<Package>();
                taskPackage.Wait();

                result = taskPackage.Result;                
            });

            continueTask.Wait();
            
            return result;
        }
    }
}
