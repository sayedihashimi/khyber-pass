namespace Sedodream.SelfPub.Test {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using log4net.Config;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Sedodream.SelfPub.Common;
    using Sedodream.SelfPub.Common.Deploy;
    using Sedodream.SelfPub.Test.Helpers;

    [TestClass]
    public class TestMongoDbDeployRecorder {
        [ClassInitialize]
        public static void Initalize(TestContext testContext) {
            XmlConfigurator.Configure();
        }

        [TestMethod]
        public void Test_Recorder_AddPackage() {
            try {
                Package package = RandomDataHelper.Instance.CreateRandomePackage();

                MongoDbDeployRecorder recorder = MongoDbDeployRecorder.Instance;
                recorder.Reset();

                recorder.RecordDeployedPackage(package);

                Assert.Inconclusive("Not finished yet");
            }
            catch (Exception ex) {
                string message = ex.Message;
                Assert.Fail(message);
            }

        }
    }
}
