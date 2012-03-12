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

        public MongoPackageRepository(string baseAddress, string databaseName = "self-publish") {
            if (string.IsNullOrEmpty(baseAddress)) { throw new ArgumentNullException("baseAddress"); }

            this.Server = MongoServer.Create(baseAddress);
            this.Server.Connect();
            if (!Server.DatabaseExists(CommonStrings.Database.DatabaseName)) {
                // get the database to create it
                var db = this.Server.GetDatabase(CommonStrings.Database.DatabaseName);
                // create the collection
                db.CreateCollection(CommonStrings.Database.CollectionName);
            }

            this.Database = this.Server.GetDatabase(databaseName);
        }
        /// <summary>
        /// In this case the BaseAddress is the MongoDB connection string.
        /// Which is in the format: <code>mongodb://[username:password@]hostname[:port][/[database][?options]]</code>
        /// </summary>
        public string BaseAddress { get; private set; }

        public string RepositoryConfig { get; private set; }

        public IPackage AddPackage(IPackage package) {
            if (package == null) { throw new ArgumentNullException("package"); }

            var result = this.PackagesCollection.Insert<IPackage>(package);

            return package;
        }

        public IQueryable<IPackage> GetPackages() {
            // for now this just returns all packages but this should not be the case
            return this.PackagesCollection.FindAllAs<IPackage>().AsQueryable();
        }


        protected MongoServer GetServer() {
            return this.Server;
        }
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
            this.PackagesCollection.Drop();
        }
    }
}
