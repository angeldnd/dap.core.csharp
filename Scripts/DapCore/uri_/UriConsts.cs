using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class UriConsts {
        public const char PathSeparator = ':';

        public const string PathSeparatorAsString = ":";

        public static bool IsRelUri(string uri) {
            return uri.StartsWith(PathSeparatorAsString);
        }

        public static string Encode(string pathA, string pathB) {
            return string.Format("{0}{1}{2}", pathA, PathSeparator, pathB);
        }

        public static bool Decode(string uri, out string pathA, out string pathB) {
            string[] segments = uri.Split(UriConsts.PathSeparator);
            pathA = segments.Length > 0 ? segments[0] : null;
            pathB = segments.Length > 1 ? segments[1] : null;
            if (segments.Length < 1 || segments.Length > 2) {
                Log.Error("Invalid Uri: {0} -> {1}", uri, segments.Length);
                return false;
            }
            return true;
        }

        public static string GetPathFromRelUri(string relUri) {
            if (string.IsNullOrEmpty(relUri)) return relUri;
            return relUri.Replace(PathSeparatorAsString, "");
        }

        public static string EncodeRelUri(string path) {
            return string.Format("{0}{1}", PathSeparator, path);
        }
    }
}
