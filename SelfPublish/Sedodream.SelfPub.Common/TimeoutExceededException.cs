namespace Sedodream.SelfPub.Common {
    using System;
    
    [Serializable]
    public class TimeoutExceededException : Exception {
        public TimeoutExceededException() { }
        public TimeoutExceededException(string message) : base(message) { }
        public TimeoutExceededException(string message, Exception inner) : base(message, inner) { }
        protected TimeoutExceededException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
