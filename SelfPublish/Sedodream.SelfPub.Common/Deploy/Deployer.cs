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


    public class Deployer {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Deployer));
        private DeployerConfig Config { get; set; }
        private IJsonSearlizer Searlizer { get; set; }

        public Deployer()
            : this(new JsonNetSearlizer()) {
        }

        public Deployer(IJsonSearlizer searlizer) {
            if (searlizer == null) { throw new ArgumentNullException("searlizer"); }

            this.Searlizer = searlizer;
        }

        public async void Start() {
            log.Info("Deployer started");
            try {
                // get machine config
                // TODO: We can fetch this form a service somewhere
                this.Config = DeployerConfig.BuildFromAppConfig();

                log.Debug("Getting package to deploy");
                // get latest package
                Package packageToDeploy = await this.GetLatestPackage();

                log.Debug("Getting deploy handler");
                // get package handler
                IDeployHandler deployHandler = this.GetDeployHandlerFor(packageToDeploy);

                log.Debug("Starting deployment");
                deployHandler.HandleDeployment(packageToDeploy, this.Config.DeploymentParameters);
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


        public virtual async Task<Package> GetLatestPackage() {
            Package result = null;
            // ex. http://localhost:12914/api/config/packages/latest/SayedHaPackage
            string url = string.Format(@"{0}config/packages/latest/{1}", this.Config.GetConfigServiceBaseUrl, this.Config.PackageNameToDeploy);

            log.Debug(string.Format("Getting latest package using URL [{0}]", url));

            HttpClient client = new HttpClient();
            client.Timeout = new TimeSpan(0, 0, 10);           
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            result = await response.Content.ReadAsAsync<Package>();
            return result;
        }

        public void TestGetLatestPackage() {
            bool hasCompleted = false;

            int waitTimeSec = 1;
            int numWaits = 0;
            int maxNumWaits = 1000;

            string url = @"http://localhost:12914/api/config/packages/latest/SayedHaPackage";

            log.Info("sending request");
            HttpClient client = new HttpClient();
            client.GetAsync(url).ContinueWith(response => {
                hasCompleted = true;
                response.Result.EnsureSuccessStatusCode();
            });

            while (!hasCompleted) {
                if (numWaits++ > maxNumWaits) { hasCompleted = true; }
                log.Info("sleeping");
                System.Threading.Thread.Sleep(waitTimeSec * 1000);
            }

            string debug = "finished";
        }


    }
}
