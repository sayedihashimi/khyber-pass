namespace Sedodream.SelfPub.Common.Deploy {
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;    

    /// <summary>
    /// This is the object which has all the config which the deployer needs to start running.   
    /// </summary>
    public class DeployerConfig {
        public static DeployerConfig BuildFromAppConfig() {
            DeployerConfig result = new DeployerConfig {
                GetConfigServiceBaseUrl = new Uri(ConfigurationManager.AppSettings[CommonStrings.Deployer.ConfigServiceBaseUrl]),
                PackageNameToDeploy = ConfigurationManager.AppSettings[CommonStrings.Deployer.PackageNameToDeploy],
                DeploymentParameters = ConfigurationManager.AppSettings[CommonStrings.Deployer.DeployParameters],
            };
            
            return result;
        }

        /// <summary>
        /// This is the base address to the config service which should be used.
        /// </summary>
        public Uri GetConfigServiceBaseUrl { get; set; }

        /// <summary>
        /// The name of the pacage which this deployer will be responsible for deploying.<br/>
        /// Each deployer can only deploy a single package, to deploy more than 1 then you must configre
        /// more than 1 deployer.
        /// </summary>
        public string PackageNameToDeploy { get; set; }

        /// <summary>
        /// This is a JSON string which is passed to <c>IDeployHandler</c> to process the deploy.
        /// This will contain any environment specific options. For example in the MSDeploy case it
        /// will have all the MSDeploy parameter values.
        /// </summary>
        public string DeploymentParameters { get; set; }
    }
}
