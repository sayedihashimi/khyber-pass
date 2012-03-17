namespace Sedodream.SelfPub.Common.Deploy {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
using System.Threading.Tasks;

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
        public void Start() {
            // get machine config
            // TODO: We can fetch this form a service somewhere
            this.Config = DeployerConfig.BuildFromAppConfig();
            
            // get latest package
            


            // get uri halder

            // get package handler

            // install package
        }
        
        public virtual void GetLatestPackage(Task<Package> continuationTask) {
            Package result = null;

            // ex. http://localhost:12914/api/config/packages/latest/SayedHaPackage
            string url = string.Format(@"{0}config/packages/latest/{1}", this.Config.GetConfigServiceBaseUrl, this.Config.PackageNameToDeploy);

            throw new NotImplementedException();
        }
    }
}
