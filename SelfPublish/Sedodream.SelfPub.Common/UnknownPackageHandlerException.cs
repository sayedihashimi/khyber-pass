namespace Sedodream.SelfPub.Common.Exceptions {
    using System;

    [Serializable]
    public class UnknownPackageHandlerException : Exception {
        public UnknownPackageHandlerException() { }
        public UnknownPackageHandlerException(string message) : base(message) { }
        public UnknownPackageHandlerException(string message, Exception inner) : base(message, inner) { }
        protected UnknownPackageHandlerException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
