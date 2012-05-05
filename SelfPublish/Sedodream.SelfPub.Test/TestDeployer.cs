namespace Sedodream.SelfPub.Test {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using log4net.Config;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MongoDB.Driver;
    using Sedodream.SelfPub.Common;
    using Sedodream.SelfPub.Common.Deploy;
    using Sedodream.SelfPub.Common.Exceptions;
    using Sedodream.SelfPub.Test.Helpers;

    [TestClass]
    public class TestDeployer {
        private static Process mongoDbProcess;

        [ClassInitialize]
        public static void Initalize(TestContext testContext) {
            XmlConfigurator.Configure();
            // we have to start mongo DB
            DirectoryInfo mongoDbDir = GetMongoDbDir(testContext);
            var mongodbexe = mongoDbDir.GetFiles("mongod.exe").Single();

            DirectoryInfo testCtxDirectory = new DirectoryInfo(testContext.TestDir);
            DirectoryInfo dataDbDir = testCtxDirectory.CreateSubdirectory(new Config().GetAppSetting<string>(CommonTestStrings.MongodbDir));

            string connectionString = new Config().GetConnectionString(CommonStrings.Database.ConnectionStringName).ConnectionString;
            MongoUrlBuilder mub = new MongoUrlBuilder(connectionString);
            string args = string.Format(@"--dbpath ""{0}"" --port {1}", dataDbDir.FullName, mub.Server.Port);

            var psi = new ProcessStartInfo {
                FileName = mongodbexe.FullName,
                WorkingDirectory = testCtxDirectory.FullName,
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            mongoDbProcess = Process.Start(psi);
        }

        [ClassCleanup]
        public static void Cleanup() {
            // we have to stop mongo DB
            mongoDbProcess.CloseMainWindow();
            mongoDbProcess.WaitForExit(5 * 1000);
            if (!mongoDbProcess.HasExited) {
                mongoDbProcess.Kill();
            }
        }

        private static DirectoryInfo GetMongoDbDir(TestContext testContext) {
            if (testContext == null) { throw new ArgumentNullException("testContext"); }

            var mongoDbBindir =
                new DirectoryInfo(testContext.TestDir)
                    .Parent.Parent.Parent
                    .GetDirectories("lib").Single()
                    .GetDirectories("mongodb*").Single()
                    .GetDirectories("bin").Single();

            return mongoDbBindir;
        }

        [TestMethod]
        public void Test_GetHandler_MSDeploy() {
            Deployer deployer = new Deployer();

            Package package = RandomDataHelper.Instance.CreateRandomePackage();
            package.PackageType = KnownPackageTypes.msdeploy.ToString();

            IDeployHandler result = deployer.GetDeployHandlerFor(package);

            Assert.IsNotNull(result);
            Assert.IsTrue(result is MSDeployHandler);
        }

        [TestMethod]
        [Microsoft.VisualStudio.TestTools.UnitTesting.ExpectedException(typeof(UnknownPackageHandlerException))]
        public void Test_GetHandler_Unknown() {
            Deployer deployer = new Deployer();
            Package package = RandomDataHelper.Instance.CreateRandomePackage();
            package.PackageType = RandomDataHelper.Instance.Primitives.GetRandomString(20);

            IDeployHandler result = deployer.GetDeployHandlerFor(package);
        }
    }
}
