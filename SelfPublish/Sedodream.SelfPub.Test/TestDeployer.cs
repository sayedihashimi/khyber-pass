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
        [ClassInitialize]
        public static void Initalize(TestContext testContext) {
            MongoUnitTestBase.Initalize(testContext);
        }

        [ClassCleanup]
        public static void Cleanup() {
            MongoUnitTestBase.Cleanup();
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
