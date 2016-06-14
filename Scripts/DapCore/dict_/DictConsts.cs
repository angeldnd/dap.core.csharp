using System;
using System.Collections.Generic;
using System.Linq;

namespace angeldnd.dap {
    public static class DictConsts {
        public const char KeySeparator = '.';

        public const string KeySeparatorAsString = ".";

        public static string Join(List<string> segments) {
            return Join(segments.ToArray());
        }

        public static string Join(params string[] segments) {
            return string.Join(KeySeparatorAsString, segments);
        }

        public static string Encode(string key, string fragment) {
            if (string.IsNullOrEmpty(key)) {
                return fragment;
            } else if (string.IsNullOrEmpty(fragment)) {
                return key;
            } else {
                return string.Format("{0}{1}{2}", key, KeySeparator, fragment);
            }
        }
    }
}
