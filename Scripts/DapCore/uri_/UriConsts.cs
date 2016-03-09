using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class UriConsts {
        public const char PathSeparator = ':';

        public const string PathSeparatorAsString = ":";

        public static string Encode(string pathA, string pathB) {
            return string.Format("{0}{1}{2}", pathA, PathSeparator, pathB);
        }
    }
}
