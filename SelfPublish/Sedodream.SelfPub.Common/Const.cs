namespace Sedodream.SelfPub.Common {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public enum KnownPackageTypes {
        msdeploy,
        tfs,
        git,
        fileSystem
    }

    public enum KnownRepositorTypes {
        MonogDb
    }

    public enum KnownUriSchemeTypes {
        file,
        http,
        https
    }
}
