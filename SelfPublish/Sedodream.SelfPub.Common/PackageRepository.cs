namespace Sedodream.SelfPub.Common {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IPackageRepository {
        /// <summary>
        /// Represents the base location where the repository is located.<br/>
        /// This is typically a URI
        /// </summary>
        string BaseAddress { get; }
        
        /// <summary>
        /// Represents which type of repository this is.<br/>
        /// For example:
        ///     NetworkShare
        ///     http
        ///     git
        ///     TFS
        /// </summary>
        // string RepositoryType { get; }

        /// <summary>
        /// An optional JSON string containng any additional info that clients would need to interact with this repository.
        /// </summary>
        string RepositoryConfig { get; }
        
        /// <summary>
        /// Adds the given package to the repository
        /// </summary>
        /// <param name="package"></param>
        Package AddPackage(Package package);
        
        /// <summary>
        /// Gets an IQueryable for all packages
        /// </summary>
        /// <returns></returns>
        IQueryable<Package> GetPackages();

        IQueryable<Package> GetPackagesByTag(string tag);

        IQueryable<Package> GetPackagesByName(string name);

        /// <summary>
        /// This will return the package which has the given ID, or <c>null</c>
        /// if it is not found.
        /// </summary>
        /// <param name="id">The ID of the package to locate</param>
        /// <returns></returns>
        Package GetPackage(Guid id);

        Package GetLatestPackageByName(string name);
    }
}
