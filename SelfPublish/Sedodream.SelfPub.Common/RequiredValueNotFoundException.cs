namespace Sedodream.SelfPub.Common {
    using System;
    
    [Serializable]
    public class RequiredValueNotFoundException : Exception {
        public RequiredValueNotFoundException() { }
        public RequiredValueNotFoundException(string message) : base(message) { }
        public RequiredValueNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected RequiredValueNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
