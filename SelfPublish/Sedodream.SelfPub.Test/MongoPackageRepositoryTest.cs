namespace Sedodream.SelfPub.Test {
    using System;
    using System.Configuration;
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Sedodream.SelfPub.Common;
    using System.Linq;
    using MongoDB.Driver;

    [TestClass]
    public class MongoPackageRepositoryTest {
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
    }
}
