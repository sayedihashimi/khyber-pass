namespace Sedodream.SelfPub.Common.Extensions {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class IntExtensions {
        public static void Times(this int number, Action action) {
            for (int i = 0; i < number; i++) {
                action();
            }
        }
    }
}
