namespace Sedodream.SelfPub.Common {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Raven.Client.Embedded;

    public class RavenDbPackageRepository : IPackageRepository {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(RavenDbPackageRepository));
        private static IDictionary<string, RavenDbPackageRepository> RepoMap = new Dictionary<string, RavenDbPackageRepository>();
        private static object RepoMapLock = new object();

        

        public static RavenDbPackageRepository GetRavenDbRepoFor(string baseDirectory) {
            if (baseDirectory == null) { throw new ArgumentNullException("baseDirectory"); }

            string baseDirLower = baseDirectory.ToLower();
            RavenDbPackageRepository repo = null;
            lock (RepoMapLock) {
                RepoMap.TryGetValue(baseDirLower, out repo);
                if (repo == null) {
                    repo = new RavenDbPackageRepository(baseDirLower);
                    RepoMap.Add(baseDirLower, repo);
                }
            }

            return repo;
        }

        EmbeddableDocumentStore DocStore;
        private object docStoreLock = new object();

        private RavenDbPackageRepository(string baseDirectory) {
            if (string.IsNullOrEmpty(baseDirectory)) { throw new ArgumentNullException("baseDirectory"); }

            this.BaseAddress = baseDirectory;

            if (!Directory.Exists(this.BaseAddress)) {
                log.DebugFormat("Creating Raven Data directory becuase it doesn't exist [{0}]", this.BaseAddress);
                Directory.CreateDirectory(this.BaseAddress);
            }

            this.DocStore = new EmbeddableDocumentStore {
                DataDirectory = this.BaseAddress
            };
            this.DocStore.Initialize();
        }

        ~RavenDbPackageRepository() {
            if (this.DocStore != null) {
                lock (this.docStoreLock) {
                    if (this.DocStore != null) {
                        this.DocStore.Dispose();
                        this.DocStore = null;
                    }
                }
            }
        }

        public string BaseAddress {
            get;
            private set;
        }

        public string RepositoryConfig {
            get;
            private set;
        }

        public Package AddPackage(Package package) {
            if (package == null) { throw new ArgumentNullException("package"); }
            
            using (var session = this.DocStore.OpenSession()) {
                session.Store(package);
                session.SaveChanges();
            }

            return package;
        }

        public IQueryable<Package> GetPackages() {

            IQueryable<Package> result = null;
            using (var session = this.DocStore.OpenSession()) {
                result = from p in session.Query<Package>()
                         // http://ravendb.net/docs/client-api/querying/stale-indexes
                         .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                         select p;
            }

            return result;
        }

        public IQueryable<Package> GetPackagesByTag(string tag) {
            if (string.IsNullOrEmpty(tag)) { throw new ArgumentNullException("tag"); }

            IQueryable<Package> result = null;
            using (var session = this.DocStore.OpenSession()) {
                result = from pkg in session.Query<Package>()
                         .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                         where pkg.Tags.Contains(tag)
                         select pkg;
            }

            return result;
        }

        public IQueryable<Package> GetPackagesByName(string name) {
            if (string.IsNullOrEmpty(name)) { throw new ArgumentNullException("name"); }

            IQueryable<Package> result = null;
            using (var session = this.DocStore.OpenSession()) {
                result = from p in session.Query<Package>()
                         .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                         where string.Compare(name, p.Name, StringComparison.OrdinalIgnoreCase) == 0
                         select p;
            }

            return result;
        }

        public Package GetPackage(Guid id) {
            Package result = null;

            using (var session = this.DocStore.OpenSession()) {
                result = (from p in session.Query<Package>()
                          .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                          where p.Id == id
                          select p).SingleOrDefault();
            }

            return result;
        }

        public Package GetLatestPackageByName(string name) {
            throw new NotImplementedException();
        }

        #region protected/private items
        /// <summary>
        /// Should only be used by unit tests
        /// </summary>
        internal void Reset() {
            // get all packages and delete them
            using (var session = this.DocStore.OpenSession()) {
                var allPackages = from p in session.Query<Package>()
                                  .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                                  select p;

                List<Package> pkgList = allPackages.ToList();
                if (pkgList.Count > 0) {
                    // TODO: is there a better way to delete a batch of objects?
                    pkgList.ForEach(p => {
                        session.Delete(p);                        
                    });

                    session.SaveChanges();
                }
            }
        }
        #endregion
    }
}
