namespace Sedodream.SelfPub.Test {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Sedodream.SelfPub.Common;
    using Sedodream.SelfPub.Common.Deploy;
    using Sedodream.SelfPub.Test.Helpers;

    [TestClass]
    public class TestMongoDbDeployRecorder {

        [TestMethod]
        public void Test_Recorder_AddPackage() {
            try {
                Package package = RandomDataHelper.Instance.CreateRandomePackage();

                MongoDbDeployRecorder recorder = MongoDbDeployRecorder.Instance;
                recorder.Reset();

                recorder.RecordDeployedPackage(package);

                string debug = "foo";
            }
            catch (Exception ex) {
                string message = ex.Message;
                Assert.Fail(message);
            }

        }
    }
}
