using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class ExtraConsts {
        [DapParam(typeof(string))]
        public const string SummaryTop = "top";
        [DapParam(typeof(string))]
        public const string SummaryFragment = "fragment";
        [DapParam(typeof(string))]
        public const string SummarySubKey = "sub_key";
        [DapParam(typeof(string))]
        public const string SummaryOriginalSubKey = "_sub_key";
    }
}
