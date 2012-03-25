namespace Sedodream.SelfPub.Common.Deploy {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Security;
    using System.Security.Principal;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Microsoft.Win32;
    using Sedodream.SelfPub.Common.Extensions;

    public class MSDeployHandler : IDeployHandler {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(MSDeployHandler));

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

            string pathToFile = null;
            // for now this only hanldes file://
            string pkgScheme = package.PackageLocation.Scheme;
            if (string.Compare(pkgScheme, KnownUriSchemeTypes.file.ToString(), StringComparison.OrdinalIgnoreCase) == 0) {
                pathToFile = package.PackageLocation.AbsolutePath;
            }
            else if (string.Compare(pkgScheme, KnownUriSchemeTypes.http.ToString(), StringComparison.OrdinalIgnoreCase) == 0 ||
                    string.Compare(pkgScheme, KnownUriSchemeTypes.https.ToString(), StringComparison.OrdinalIgnoreCase) == 0) {
                // pathToFile = await this.DownloadFileToLocalFile(package.PackageLocation);
                pathToFile = this.DownloadFileToLocalFile(package.PackageLocation);
            }
            else {
                string message = string.Format("Unknown URI scheme: [{0}]", package.PackageLocation.Scheme);
                throw new UnknownPackageUriSchemeException(message);
            }


            // we have to create the setparameters.xml file
            string parametersFile = this.CreateSetParametersXml(param);

            // now let's construct the command which needs to be executed
            string pathToMsdeployExe = this.GetPathToMsdeploy();
            if (string.IsNullOrWhiteSpace(pathToMsdeployExe)) {
                string message = string.Format("Unable to locate msdeploy.exe");
                throw new RequiredValueNotFoundException(message);
            }

            // msdeploy.exe -verb:sync -source:package={path-to-package} -dest:auto -setParamFile={path-to-setParam.xml}
            string args = string.Format(
                @"-verb:sync -source:package=""{0}"" -dest:auto -setParamFile=""{1}"" ",
                pathToFile,
                parametersFile
            );

            log.DebugFormat("Calling msdeploy.exe with: [{0}] {1}", args, Environment.NewLine);

            ProcessStartInfo psi = new ProcessStartInfo {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardInput = true,

                Arguments = args,
                FileName = pathToMsdeployExe,
                LoadUserProfile = true,
            };

            Process process = Process.Start(psi);
            bool processHasExited = process.WaitForExit(param.MsdeployTimeout);
            if (!processHasExited) {
                string message = string.Format("msdeploy process has not exited in the given timeout [{0}]. Kiling the process", param.MsdeployTimeout);
                process.Kill();
                throw new TimeoutExceededException(message);
            }

            if (process.ExitCode != 0) {
                string message = string.Format("There was an error inovking msdeploy.exe. ExitCode = [{0}]",process.ExitCode);
                log.Error(message);
                throw new PublishException(message);
            }


            log.Debug("msdeploy.exe has exited");
        }

        /// <summary>
        /// This will check the registry to see where msdeploy.exe is at
        /// </summary>
        protected internal string GetPathToMsdeploy() {
            // HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\IIS Extensions\MSDeploy\3@InstallPath
            // HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\IIS Extensions\MSDeploy\2@InstallPath
            // HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\IIS Extensions\MSDeploy\1@InstallPath

            log.Debug("Looking for msdeploy.exe");
            string path = null;
            List<string> versionsToLookfor = new List<string> { "3", "2", "1" };
            foreach (string ver in versionsToLookfor) {
                string key = string.Format(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\IIS Extensions\MSDeploy\{0}", ver);
                string regPathResult = Registry.GetValue(key, "InstallPath", null) as string;
                if (regPathResult != null) {
                    path = Path.Combine(regPathResult, "msdeploy.exe");
                    break;
                }
            }

            log.Debug(string.Format("path to msdeploy.exe [{0}]", path));

            return path;
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
            log.Debug(string.Format("Creating setparameters.xml at [{0}]", fileToCreate));
            doc.Save(fileToCreate);

            return fileToCreate;
        }

        public string DownloadFileToLocalFile(Uri fileUri) {
            string tempFile = PathExtensions.GetTempFileWithExtension(".zip");

            using (System.Net.WebClient webClient = new System.Net.WebClient()) {
                webClient.DownloadFile(fileUri, tempFile);
            }
            return tempFile;
        }

    }


    public class MSDeployDeploymentParameters {
        public MSDeployDeploymentParameters() {
            // 20 seconds by default
            this.MsdeployTimeout = 20 * 1000;
            this.Parameters = new Dictionary<string, string>();
        }

        /// <summary>
        /// The MSDeploy Parameters used for publishing
        /// </summary>
        public Dictionary<string, string> Parameters { get; set; }

        public int MsdeployTimeout { get; set; }

        /// <summary>
        /// This accepts a JSON string, it will convert ito to an instance of MSDeployDeploymentParameters
        /// </summary>       
        public static MSDeployDeploymentParameters BuildFromString(string deploymentParameters, IJsonSearlizer searlizer) {
            if (string.IsNullOrEmpty(deploymentParameters)) { throw new ArgumentNullException("deploymentParameters"); }
            if (searlizer == null) { throw new ArgumentNullException("searlizer"); }

            return searlizer.Desearlize<MSDeployDeploymentParameters>(deploymentParameters);
        }
    }
}
