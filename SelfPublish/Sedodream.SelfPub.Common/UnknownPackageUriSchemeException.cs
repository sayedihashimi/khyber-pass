namespace Sedodream.SelfPub.Common {
    using System;
    
    [Serializable]
    public class UnknownPackageUriSchemeException : Exception {
        public UnknownPackageUriSchemeException() { }
        public UnknownPackageUriSchemeException(string message) : base(message) { }
        public UnknownPackageUriSchemeException(string message, Exception inner) : base(message, inner) { }
        protected UnknownPackageUriSchemeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
