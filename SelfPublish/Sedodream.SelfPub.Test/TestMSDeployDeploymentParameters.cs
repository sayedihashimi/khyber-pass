namespace Sedodream.SelfPub.Test {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Sedodream.SelfPub.Common;
    using Sedodream.SelfPub.Common.Deploy;
    using Sedodream.SelfPub.Test.Helpers;

    [TestClass]
    public class TestMSDeployDeploymentParameters {
        [TestMethod]
        public void Test_DesearlizeFromString() {
            IJsonSearlizer searlizer = new JsonNetSearlizer();

            string iisAppParamName = @"IIS Web Application Name";
            string iisAppParamValue = @"Default Web Site/FooBar";

            string msdeployDeployParametersString =
string.Format(@"{{
	""Parameters"" : {{
		""{0}"" : ""{1}""
	}}
}}", iisAppParamName, iisAppParamValue);

            MSDeployDeploymentParameters result = searlizer.Desearlize<MSDeployDeploymentParameters>(msdeployDeployParametersString);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Parameters);
            Assert.AreEqual(1, result.Parameters.Count);
        }

        [TestMethod]
        public void Test_SearlizeToString() {
            IJsonSearlizer searlizer = new JsonNetSearlizer();

            MSDeployDeploymentParameters msdParams = RandomDataHelper.Instance.CreateRandomMSDeployDeploymentParameters();
            string result = searlizer.Searlize(msdParams);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 1);

        }      
    }
}
