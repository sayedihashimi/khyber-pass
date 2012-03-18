namespace Sedodream.SelfPub.Test {
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sedodream.SelfPub.Common;
using Sedodream.SelfPub.Common.Deploy;
using Sedodream.SelfPub.Common.Exceptions;
using Sedodream.SelfPub.Test.Helpers;

    [TestClass]
    public class TestDeployer {
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
