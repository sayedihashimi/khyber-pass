namespace Sedodream.SelfPub.Common {
    using System;

    [Serializable]
    public class UnknownStateException : Exception {
        public UnknownStateException() { }
        public UnknownStateException(string message) : base(message) { }
        public UnknownStateException(string message, Exception inner) : base(message, inner) { }
        protected UnknownStateException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
