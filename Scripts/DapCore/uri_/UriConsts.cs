using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class UriConsts {
        public const char UriSeparator = ':';

        public const string UriSeparatorAsString = ":";

        public static string Encode(string path, string param) {
            return string.Format("{0}{1}{2}", path, UriSeparator, param);
        }

        public static void Decode(string uri, out string path, out string param) {
            if (!string.IsNullOrEmpty(uri)) {
                int pos = uri.IndexOf(UriSeparator);
                if (pos < 0) {
                    path = uri;
                    param = null;
                } else {
                    path = uri.Remove(pos);
                    param = uri.Substring(pos + 1);
                }
            } else {
                path = null;
                param = null;
            }
        }
    }
}
