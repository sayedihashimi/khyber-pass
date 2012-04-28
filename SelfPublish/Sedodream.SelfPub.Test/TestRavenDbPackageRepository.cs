namespace Sedodream.SelfPub.Test {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Sedodream.SelfPub.Common;
    using Sedodream.SelfPub.Test.Helpers;

    [TestClass]
    public class TestRavenDbPackageRepository  {

        [TestMethod]
        public void TestAddPackage_1Package() {
            Package fooPackage = RandomDataHelper.Instance.CreateRandomePackage();
            
            RavenDbPackageRepository packageRepo = new RavenDbPackageRepository(@"C:\temp\_Net\ravendb");
            packageRepo.Reset();

            packageRepo.AddPackage(fooPackage);

            var allPackages = packageRepo.GetPackages();

            Assert.IsNotNull(allPackages);
            Assert.AreEqual(1, allPackages.Count());
            CustomAssert.AreEqual(fooPackage, allPackages.ElementAt(0));

            string debug = "foo";
        }

        public void TestAddPackage_2Packages() {
        }

    }
}
