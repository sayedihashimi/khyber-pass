﻿namespace Sedodream.SelfPub.Test {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Sedodream.SelfPub.Common;
    using Sedodream.SelfPub.Test.Helpers;
    using Sedodream.SelfPub.Common.Extensions;
    
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
                var allPackages = packageRepo.GetPackages();
                int count = allPackages.Count();

                Assert.IsNotNull(allPackages);
                Assert.AreEqual(numPackagesToAdd, allPackages.Count());
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsIfPackageIsNull(){
                RavenDbPackageRepository packageRepo = RavenDbPackageRepositoryTests.GetRavenDbRepostiory(TestContext);
                
                packageRepo.AddPackage(null);
            }
        }

        [TestClass]
        public class TheGetPackageByNameMethod{
            public TestContext TestContext { get; set; }

            [TestMethod]
            public void ReturnsThePackageWithTheGivenName() {
                RavenDbPackageRepository repo = RavenDbPackageRepositoryTests.GetRavenDbRepostiory(TestContext);

                // add a few random packages
                int numPackages = RandomDataHelper.Instance.Primitives.GetRandomInt(10);
                numPackages.Times(() => repo.AddPackage(RandomDataHelper.Instance.CreateRandomePackage()));

                // add the package we want to find
                Package package = RandomDataHelper.Instance.CreateRandomePackage();
                repo.AddPackage(package);

                // add a few more random packages
                numPackages = RandomDataHelper.Instance.Primitives.GetRandomInt(10);
                numPackages.Times(() => repo.AddPackage(RandomDataHelper.Instance.CreateRandomePackage()));

                Package foundPackage = repo.GetPackagesByName(package.Name).SingleOrDefault();

                Assert.IsNotNull(foundPackage);
                CustomAssert.AreEqual(package, foundPackage);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsIfNameIsNull() {
                RavenDbPackageRepository repo = RavenDbPackageRepositoryTests.GetRavenDbRepostiory(TestContext);

                repo.GetPackagesByName(null);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsIfNameIsEmpty() {
                RavenDbPackageRepository repo = RavenDbPackageRepositoryTests.GetRavenDbRepostiory(TestContext);

                repo.GetPackagesByName(string.Empty);
            }
        }

        [TestClass]
        public class TheGetPackageByIdMethod {
            public TestContext TestContext { get; set; }

            [TestMethod]
            public void ReturnsThePackageWithTheGivenId() {               
                RavenDbPackageRepository repo = RavenDbPackageRepositoryTests.GetRavenDbRepostiory(TestContext);

                // add a few random packages
                int numPackages = RandomDataHelper.Instance.Primitives.GetRandomInt(10);
                numPackages.Times(() => repo.AddPackage(RandomDataHelper.Instance.CreateRandomePackage()));

                repo.AddPackage(RandomDataHelper.Instance.CreateRandomePackage());

                // add the package we want to find
                Package package = RandomDataHelper.Instance.CreateRandomePackage();
                repo.AddPackage(package);
                // add a few more random packages
                numPackages = RandomDataHelper.Instance.Primitives.GetRandomInt(10);
                numPackages.Times(() => repo.AddPackage(RandomDataHelper.Instance.CreateRandomePackage()));

                // find the package
                Package foundPackage = repo.GetPackage(package.Id);
                Assert.IsNotNull(foundPackage);
                CustomAssert.AreEqual(package, foundPackage);
            }

            [TestMethod]
            public void ReturnsNullIfThePackageDoesntExist() {
                RavenDbPackageRepository repo = RavenDbPackageRepositoryTests.GetRavenDbRepostiory(TestContext);

                Package foundPackage = repo.GetPackage(Guid.NewGuid());

                Assert.IsNull(foundPackage);
            }

        }

        [TestClass]
        public class TheGetRavenDbRepoForMethod{
            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsIfBaseDirectoryIsNull(){
                RavenDbPackageRepository.GetRavenDbRepoFor(null);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsIfBaseDirectoryIsEmpty(){
                RavenDbPackageRepository.GetRavenDbRepoFor(string.Empty);
            }
        }

        [TestClass]
        public class TheGetPackagesByTagMethod{
            public TestContext TestContext { get; set; }

            // The GetPackagesByTag method is not yet supported in RavenDbPackageRepository
            [Ignore]
            [TestMethod]
            public void ReturnsPackagesWithTheGivenTag(){
                RavenDbPackageRepository repo = RavenDbPackageRepositoryTests.GetRavenDbRepostiory(TestContext);
                // add a few packages
                int numPackages = RandomDataHelper.Instance.Primitives.GetRandomInt(10);
                numPackages.Times(()=>repo.AddPackage(RandomDataHelper.Instance.CreateRandomePackage()));

                // add the package we are looking for with a specific tag
                string tag = RandomDataHelper.Instance.Primitives.GetRandomString(10);
                Package package = RandomDataHelper.Instance.CreateRandomePackage();
                package.Tags.Add(tag);

                // add a few more packages
                numPackages = RandomDataHelper.Instance.Primitives.GetRandomInt(10);
                numPackages.Times(()=>repo.AddPackage(RandomDataHelper.Instance.CreateRandomePackage()));

                IQueryable<Package>foundPackages = repo.GetPackagesByTag(tag);

                Assert.IsNotNull(foundPackages);
                Assert.AreEqual(1,foundPackages.Count());

                Package foundPackage = foundPackages.Single();
                CustomAssert.AreEqual(package, foundPackage);           
            }
            

        }

        [TestClass]
        public class TheGetLatestPackageByNameMethod{
            public TestContext TestContext { get; set; }
            [TestMethod]
            public void ReturnsTheLastestPackageWithTheGivenName_Only1PkgInRepo(){
                RavenDbPackageRepository repo = RavenDbPackageRepositoryTests.GetRavenDbRepostiory(TestContext);

                Package package = RandomDataHelper.Instance.CreateRandomePackage();

                repo.AddPackage(package);

                Package foundPackage = repo.GetLatestPackageByName(package.Name);

                Assert.IsNotNull(foundPackage);
                CustomAssert.AreEqual(package,foundPackage);
            }

            public void ReturnsTheLastestPackageWithTheGivenName_MultiplePackagesInRepo() {
                RavenDbPackageRepository repo = RavenDbPackageRepositoryTests.GetRavenDbRepostiory(TestContext);

                string pkgName = RandomDataHelper.Instance.Primitives.GetRandomString(10);
                
                // # add a few packages with a specfic name
                int numPackagesToAdd = RandomDataHelper.Instance.Primitives.GetRandomInt(10);
                numPackagesToAdd.Times(() => {
                    Package pkg = RandomDataHelper.Instance.CreateRandomePackage();
                    pkg.Name = pkgName;

                    repo.AddPackage(pkg);
                });

                // now add the package we are actually looking to find

                Package package = RandomDataHelper.Instance.CreateRandomePackage();
                package.Name = pkgName;

                repo.AddPackage(package);

                Package foundPackage = repo.GetLatestPackageByName(package.Name);

                Assert.IsNotNull(foundPackage);
                CustomAssert.AreEqual(package, foundPackage);
            }

            [TestMethod]
            public void ReturnsNullIfNoSuchPackageExists() {
                RavenDbPackageRepository repo = RavenDbPackageRepositoryTests.GetRavenDbRepostiory(TestContext);
                Package result = repo.GetLatestPackageByName(Guid.NewGuid().ToString());

                Assert.IsNull(result);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsIfNameIsNull() {
                RavenDbPackageRepository repo = RavenDbPackageRepositoryTests.GetRavenDbRepostiory(TestContext);

                repo.GetLatestPackageByName(null);
            }
        }

        protected static RavenDbPackageRepository GetRavenDbRepostiory(TestContext testContext) {
            if (testContext == null) { throw new ArgumentNullException("testContext"); }

            DirectoryInfo testCtxDirectory = new DirectoryInfo(testContext.TestDir);
            DirectoryInfo dataDbDir = testCtxDirectory.CreateSubdirectory(new Config().GetAppSetting<string>(CommonTestStrings.RavenDir));

            RavenDbPackageRepository packageRepo = RavenDbPackageRepository.GetRavenDbRepoFor(dataDbDir.FullName);
            packageRepo.Reset();

            return packageRepo;
        }
    }
}



