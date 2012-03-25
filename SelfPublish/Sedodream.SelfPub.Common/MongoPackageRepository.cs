namespace Sedodream.SelfPub.Common {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using MongoDB.Bson;
    using MongoDB.Driver;

    public class MongoPackageRepository : IPackageRepository{
        private MongoServer Server { get; set; }
        private MongoDatabase Database { get; set; }

        public MongoPackageRepository(string baseAddress) {
            if (string.IsNullOrEmpty(baseAddress)) { throw new ArgumentNullException("baseAddress"); }

            MongoUrl mongoUrl = new MongoUrl(baseAddress);
            this.Server = MongoServer.Create(mongoUrl);
            this.Server.Connect();

            if (!Server.DatabaseExists(mongoUrl.DatabaseName)) {
                // get the database to create it
                var db = this.Server.GetDatabase(CommonStrings.Database.DatabaseName);
                // create the collection
                db.CreateCollection(CommonStrings.Database.CollectionName);
            }

            this.Database = this.Server.GetDatabase(mongoUrl.DatabaseName);
        }
        /// <summary>
        /// In this case the BaseAddress is the MongoDB connection string.
        /// Which is in the format: <code>mongodb://[username:password@]hostname[:port][/[database][?options]]</code>
        /// </summary>
        public string BaseAddress { get; private set; }

        public string RepositoryConfig { get; private set; }

        public Package AddPackage(Package package) {
            if (package == null) { throw new ArgumentNullException("package"); }

            package.DateCreated = DateTime.Now;

            var result = this.PackagesCollection.Insert<Package>(package);

            return package;
        }

        public IQueryable<Package> GetPackages() {
            // for now this just returns all packages but this should not be the case
            return this.PackagesCollection.FindAllAs<Package>().AsQueryable();
        }

        public IQueryable<Package> GetPackagesByTag(string tag) {
            if (string.IsNullOrEmpty(tag)) { throw new ArgumentNullException("tag"); }

            var result = from p in this.PackagesCollection.FindAllAs<Package>()
                         where p.Tags.Contains(tag)
                         select p;

            return result.AsQueryable();
        }

        public IQueryable<Package> GetPackagesByName(string name) {
            if (string.IsNullOrEmpty(name)) { throw new ArgumentNullException("name"); }

            var result = from p in this.PackagesCollection.FindAllAs<Package>()
                         where string.Compare(name, p.Name, StringComparison.OrdinalIgnoreCase) == 0
                         select p;

            return result.AsQueryable();
        }

        public Package GetLatestPackageByName(string name) {
            if (string.IsNullOrEmpty(name)) { throw new ArgumentNullException("name"); }

            var result = from p in this.GetPackagesByName(name)
                         orderby p.DateCreated descending
                         select p;

            return result.FirstOrDefault();
        }

        #region protected/private items
        protected MongoDatabase GetDatabase() {
            return this.Database;
        }

        protected MongoCollection PackagesCollection {
            get {
                return this.GetDatabase().GetCollection(CommonStrings.Database.CollectionName);
            }
        }
        
        /// <summary>
        /// Should only be used by unit tests
        /// </summary>
        internal void Reset() {
            if (this.PackagesCollection.Exists()) {
                this.PackagesCollection.Drop();
            }
        }
        #endregion
    }
}
