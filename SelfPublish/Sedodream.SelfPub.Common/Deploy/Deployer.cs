namespace Sedodream.SelfPub.Common.Deploy {
    using System;
    using System.Collections.Generic;
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
        private DeployerConfig Config { get; set; }
        public async void Start() {
            // get machine config
            // TODO: We can fetch this form a service somewhere
            this.Config = DeployerConfig.BuildFromAppConfig();
            
            // get latest package
            Package packageToDeploy = await this.GetLatestPackage();

            // get package handler
            IDeployHandler deployHandler = this.GetDeployHandlerFor(packageToDeploy);
            deployHandler.HandleDeployment(packageToDeploy, this.Config.DeploymentParameters);

            

            // install package
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
                string message = string.Format(@"Unable to determine the deployment handler for package type: {0}",package.PackageType);
                throw new UnknownPackageHandlerException(message);
            }
        }

        
        public virtual async Task<Package> GetLatestPackage() {
            Package result = null;

            // ex. http://localhost:12914/api/config/packages/latest/SayedHaPackage
            string url = string.Format(@"{0}config/packages/latest/{1}", this.Config.GetConfigServiceBaseUrl, this.Config.PackageNameToDeploy);

            using (HttpClient client = new HttpClient()) {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                result = await response.Content.ReadAsAsync<Package>();
            }

            return result;
        }
    }
}
