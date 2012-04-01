namespace Sedodream.SelfPub.Runner {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IServiceRunner {
        void Run();
        void Start();
        void Stop(StopReason reason);
    }
    public enum StopReason {
        StopFileFound,
        ServiceStopRequested
    }

    public class ServiceRunner : IServiceRunner {
        public void Run() {
            throw new NotImplementedException();
        }

        public void Start() {
            throw new NotImplementedException();
        }

        public void Stop(StopReason reason) {
            throw new NotImplementedException();
        }
    }
}
