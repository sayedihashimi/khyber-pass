namespace Sedodream.SelfPub.Test {
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Sedodream.SelfPub.Common;
    using Sedodream.SelfPub.Common.Deploy;

    public class DeployerConfigTests {

        [TestClass]
        public class TheBuildFromAppConfigMethod {

            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsOnNullConfig() {
                DeployerConfig.BuildFromAppConfig(null);
            }

            [TestMethod]
            public void ReturnsDeployerConfig() {
                var dc = DeployerConfig.BuildFromAppConfig(new MockConfig());

                Assert.IsNotNull(dc);
                Assert.AreEqual(new Uri(Consts.ConfigServiceBaseUrlValue), dc.GetConfigServiceBaseUrl);
                Assert.AreEqual(Consts.PackageNameToDeployValue, dc.PackageNameToDeploy);
                Assert.AreEqual(Consts.DeployParametersValue, dc.DeploymentParameters);
                Assert.AreEqual(Consts.GetLatestPkgTimeoutValue, dc.GetLatestPkgTimeout);
            }
        }
    }

    // can't use moq at this time due to http://code.google.com/p/moq/issues/detail?id=284
    class MockConfig : Config {
        public override T GetAppSetting<T>(string name, T defaultValue = default(T), bool required = false) {
            if (string.IsNullOrEmpty(name)) { throw new ArgumentNullException("name"); }

            object result = default(T);

            if (string.Compare(name, CommonStrings.Deployer.ConfigServiceBaseUrl, StringComparison.OrdinalIgnoreCase) == 0) {
                result = Consts.ConfigServiceBaseUrlValue;
            }
            else if (string.Compare(name, CommonStrings.Deployer.PackageNameToDeploy, StringComparison.OrdinalIgnoreCase) == 0) {
                result = Consts.PackageNameToDeployValue;
            }
            else if (string.Compare(name, CommonStrings.Deployer.DeployParameters, StringComparison.OrdinalIgnoreCase) == 0) {
                result = Consts.DeployParametersValue;
            }
            else {
                result = base.GetAppSetting<T>(name, defaultValue, required);
            }

            return (T)result;
        }
    }

    static class Consts {
        public static string ConfigServiceBaseUrlValue = @"http://localhost:44444/foo";
        public static string PackageNameToDeployValue = @"test-package-name";
        public static string DeployParametersValue = @"{&quot;MsdeployTimeout&quot;:&quot;10000&quot;, &quot;Parameters&quot; : {&quot;IIS Web Application Name&quot; : &quot;Default Web Site/FooBar&quot; }}";
        public static TimeSpan GetLatestPkgTimeoutValue = TimeSpan.FromMilliseconds(10000);
    }
}
