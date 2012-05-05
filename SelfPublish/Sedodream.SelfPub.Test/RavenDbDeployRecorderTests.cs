namespace Sedodream.SelfPub.Test {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Sedodream.SelfPub.Common;
    using Sedodream.SelfPub.Common.Deploy;
    using Sedodream.SelfPub.Test.Helpers;

    // having some issues with mstest executing these test cases for some reason
    [TestClass]
    public class RavenDbDeployRecorderTests {
        [TestClass]
        public class TheRecordDeployedPackage {
            [TestMethod]
            public void RecordsThePackage() {
                Package package = RandomDataHelper.Instance.CreateRandomePackage();

                RavenDbDeployRecorder recorder = RavenDbDeployRecorder.Instance;
                recorder.Reset();

                bool beforeAddHasBeenRecorded = recorder.HasPackageBeenPreviouslyDeployed(package.Id);

                recorder.RecordDeployedPackage(package);

                bool afterAddHasBeenRecorded = recorder.HasPackageBeenPreviouslyDeployed(package.Id);

                Assert.IsFalse(beforeAddHasBeenRecorded);
                Assert.IsTrue(afterAddHasBeenRecorded);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsIfNull() {
                RavenDbDeployRecorder recorder = RavenDbDeployRecorder.Instance;
                recorder.Reset();

                recorder.RecordDeployedPackage(null);
            }
        }
    }
}
