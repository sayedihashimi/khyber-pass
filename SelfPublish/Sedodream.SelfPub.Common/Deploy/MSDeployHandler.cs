namespace Sedodream.SelfPub.Common.Deploy {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Sedodream.SelfPub.Common.Extensions;

    public class MSDeployHandler : IDeployHandler {
        private IJsonSearlizer Searlizer { get; set; }
        
        public MSDeployHandler()
            : this(new JsonNetSearlizer()) {
        }

        public MSDeployHandler(IJsonSearlizer searlizer) {
            this.Searlizer = searlizer;
        }

        public void HandleDeployment(Package package, string deployParameters) {
            if (package == null) { throw new ArgumentNullException("package"); }

            MSDeployDeploymentParameters param = new MSDeployDeploymentParameters();
            if (!string.IsNullOrWhiteSpace(deployParameters)) {
                param = this.Searlizer.Desearlize<MSDeployDeploymentParameters>(deployParameters);
            }

            // for now this only hanldes file://
            if (string.Compare(package.PackageLocation.Scheme, KnownUriSchemeTypes.file.ToString(), StringComparison.OrdinalIgnoreCase) != 0) {
                string message = string.Format("Unknown URI scheme: [{0}]", package.PackageLocation.Scheme);
                throw new UnknownPackageUriSchemeException(message);
            }
            
            // we have to create the setparameters.xml file

            // we need to call out to MSDeploy now
            // msdeploy.exe -verb:sync -source:{package-path} -dest:auto -setParamFile={path-to-file}

            throw new NotImplementedException();
        }

        internal string CreateSetParametersXml(MSDeployDeploymentParameters parmeters) {
            if (parmeters == null) { throw new ArgumentNullException("parmeters"); }
            //<?xml version="1.0" encoding="utf-8"?>
            //<parameters>
            //  <setParameter name="IIS Web Application Name" 
            //                value="Default Web Site/WebApplication6_deploy" />
            //  <setParameter name="ApplicationServices-Web.config Connection String" 
            //                value="data source=.\SQLEXPRESS;Integrated Security=SSPI;AttachDBFilename=|DataDirectory|\aspnetdb.mdf;User Instance=true" />
            //</parameters>

            XDocument doc = new XDocument(
                new XElement("parameters",
                    from p in parmeters.Parameters
                    select new XElement("setParameter",
                        new XAttribute("name", p.Key),
                        new XAttribute("value", p.Value))));

            string fileToCreate = PathExtensions.GetTempFileWithExtension(".xml");
            doc.Save(fileToCreate);

            return fileToCreate;
        }

    }

    public class MSDeployDeploymentParameters {
        public MSDeployDeploymentParameters() {
            this.Parameters = new Dictionary<string, string>();
        }

        /// <summary>
        /// The MSDeploy Parameters used for publishing
        /// </summary>
        public Dictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// This accepts a JSON string, it will convert ito to an instance of MSDeployDeploymentParameters
        /// </summary>       
        public static MSDeployDeploymentParameters BuildFromString(string deploymentParameters,IJsonSearlizer searlizer) {
            if (string.IsNullOrEmpty(deploymentParameters)) { throw new ArgumentNullException("deploymentParameters"); }
            if (searlizer == null) { throw new ArgumentNullException("searlizer"); }

            return searlizer.Desearlize<MSDeployDeploymentParameters>(deploymentParameters);
        }
    }
}
