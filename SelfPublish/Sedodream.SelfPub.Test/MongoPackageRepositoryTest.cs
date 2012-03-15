namespace Sedodream.SelfPub.Test {
    using System;
    using System.Configuration;
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Sedodream.SelfPub.Common;
    using System.Linq;
    using MongoDB.Driver;
    using System.Diagnostics;

    [TestClass]
    public class MongoPackageRepositoryTest {
        private static Process mongoDbProjcess;

        [ClassInitialize]
        public static void Initalize(TestContext testContext) {
            // we have to start mongo DB
            DirectoryInfo mongoDbDir = GetMongoDbDir(testContext);
            var mongodbexe = mongoDbDir.GetFiles("mongod.exe").Single();

            DirectoryInfo testCtxDirectory = new DirectoryInfo(testContext.TestDir);
            DirectoryInfo dataDbDir = testCtxDirectory.CreateSubdirectory(ConfigurationManager.AppSettings[CommonTestStrings.MongodbDir]);

            string args = string.Format(@"--dbpath ""{0}""",dataDbDir.FullName);
            var psi = new ProcessStartInfo {
                FileName = mongodbexe.FullName,
                WorkingDirectory = testCtxDirectory.FullName,
                Arguments = args
            };

            mongoDbProjcess = Process.Start(psi);
        }

        [ClassCleanup]
        public static void Cleanup() {
            // we have to stop mongo DB
            mongoDbProjcess.CloseMainWindow();
            mongoDbProjcess.WaitForExit(5 * 1000);
            if (!mongoDbProjcess.HasExited) {
                mongoDbProjcess.Kill();
            }
        }

        [TestMethod]
        public void TestInsert() {            
            Guid guid = Guid.NewGuid();
            Package package = new Package {
                Name = guid.ToString(),
                PackageLocation = new Uri(Path.Combine(@"C:\temp\", guid.ToString())),
                PackageManifest = "some string"
            };

            string connectionString = ConfigurationManager.ConnectionStrings[CommonStrings.Database.ConnectionStringName].ConnectionString;

            MongoUrlBuilder mub = new MongoUrlBuilder(connectionString);

            MongoPackageRepository repo = new MongoPackageRepository(connectionString);

            repo.Reset();

            repo.AddPackage(package);

            var r = from p in repo.GetPackages()
                    where string.Compare(p.Name, package.Name, StringComparison.OrdinalIgnoreCase) == 0
                    select p;

            Assert.IsNotNull(r);
        }

        private static DirectoryInfo GetMongoDbDir(TestContext testContext) {
            if (testContext == null) { throw new ArgumentNullException("testContext"); }

            var mongoDbBindir =
                new DirectoryInfo(testContext.TestDir)
                    .Parent.Parent.Parent
                    .GetDirectories("lib").Single()
                    .GetDirectories("mongodb*").Single()
                    .GetDirectories("bin").Single();

            return mongoDbBindir;
        }
    }
}
