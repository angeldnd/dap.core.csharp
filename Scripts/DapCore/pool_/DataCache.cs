using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public static class DataCache {
        private static Dictionary<string, Pool<Data>> _Pools = new Dictionary<string, Pool<Data>>();

        public static Data Take(string kind) {
            //TODO
            return new RealData();
        }

        public static Data Take(ILogger caller) {
            //TODO
            string kind = caller.GetType().FullName;
            return Take(kind);
        }
    }
}
