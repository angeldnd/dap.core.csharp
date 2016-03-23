using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class DictConsts {
        public const char KeySeparator = '.';

        public const string KeySeparatorAsString = ".";

        public static string Encode(string key, string fragment) {
            if (string.IsNullOrEmpty(key)) {
                return fragment;
            } else {
                return string.Format("{0}{1}{2}", key, KeySeparator, fragment);
            }
        }
    }
}
