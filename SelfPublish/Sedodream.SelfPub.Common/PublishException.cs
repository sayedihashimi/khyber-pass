namespace Sedodream.SelfPub.Common {
    using System;
    
    [Serializable]
    public class PublishException : Exception {
        public PublishException() { }
        public PublishException(string message) : base(message) { }
        public PublishException(string message, Exception inner) : base(message, inner) { }
        protected PublishException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
