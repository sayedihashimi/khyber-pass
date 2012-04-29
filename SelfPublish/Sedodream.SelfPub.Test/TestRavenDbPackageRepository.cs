namespace Sedodream.SelfPub.Test {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Sedodream.SelfPub.Common;
    using Sedodream.SelfPub.Test.Helpers;

    [TestClass]
    public class RavenDbPackageRepositoryTests {
        [TestClass]
        public class TheAddPackageMethod {
            public TestContext TestContext { get; set; }
            
            [TestMethod]
            public void AddsOnePackageWhenCalledOnce() {
                Package package = RandomDataHelper.Instance.CreateRandomePackage();

                RavenDbPackageRepository packageRepo = RavenDbPackageRepositoryTests.GetRavenDbRepostiory(TestContext);

                packageRepo.AddPackage(package);

                var allPackages = packageRepo.GetPackages();

                Assert.IsNotNull(allPackages);
                Assert.AreEqual(1, allPackages.Count());               
            }

            [TestMethod]
            public void AddsAPackageEachTimeCalled() {
                RavenDbPackageRepository packageRepo = RavenDbPackageRepositoryTests.GetRavenDbRepostiory(TestContext);

                int numPackagesToAdd = RandomDataHelper.Instance.Primitives.GetRandomInt(10);
                for (int i = 0; i < numPackagesToAdd; i++) {
                    Package package = RandomDataHelper.Instance.CreateRandomePackage();
                    packageRepo.AddPackage(package);
                }

                // give the add operations time to complete 
                // TODO: Is there a better way to do this?
                Thread.Sleep(1000);
                var allPackages = packageRepo.GetPackages();
                int count = allPackages.Count();

                Assert.IsNotNull(allPackages);
                Assert.AreEqual(numPackagesToAdd, allPackages.Count());
            }


        }

        protected static RavenDbPackageRepository GetRavenDbRepostiory(TestContext testContext) {
            if (testContext == null) { throw new ArgumentNullException("testContext"); }

            DirectoryInfo testCtxDirectory = new DirectoryInfo(testContext.TestDir);
            DirectoryInfo dataDbDir = testCtxDirectory.CreateSubdirectory(new Config().GetAppSetting<string>(CommonTestStrings.RavenDbDir));

            RavenDbPackageRepository packageRepo = RavenDbPackageRepository.GetRavenDbRepoFor(dataDbDir.FullName);
            // RavenDbPackageRepository packageRepo = new RavenDbPackageRepository(dataDbDir.FullName);
            packageRepo.Reset();
            // we have to give it some time to compelet
            Thread.Sleep(1000);

            return packageRepo;
        }
    }
}
