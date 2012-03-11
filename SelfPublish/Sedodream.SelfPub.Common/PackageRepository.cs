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
        string BaseAddress { get; set; }
        /// <summary>
        /// Represents which type of repository this is.<br/>
        /// For example:
        ///     NetworkShare
        ///     http
        ///     git
        ///     TFS
        /// </summary>
        string RepositoryType { get; set; }
        /// <summary>
        /// An optional JSON string containng any additional info that clients would need to interact with this repository.
        /// </summary>
        string RepositoryConfig { get; set; }
    }

}
