using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class ElementConsts {
        [DapParam(typeof(string))]
        public const string SummaryKey = "key";
        [DapParam(typeof(int))]
        public const string SummaryIndex = "index";
    }
}
