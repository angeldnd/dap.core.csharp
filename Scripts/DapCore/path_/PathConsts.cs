using System;
using System.Collections.Generic;
using System.Linq;

namespace angeldnd.dap {
    public static class PathConsts {
        public const char SegmentSeparator = '/';
        public const string SegmentSeparatorAsString = "/";

        public static List<string> Split(string path) {
            string[] segments = path.Split(SegmentSeparator);

            List<string> result = new List<string>();
            for (int i = 0; i < segments.Length; i++) {
                string segment = segments[i];
                if (string.IsNullOrEmpty(segment)) {
                    continue;
                }
                result.Add(segment);
            }
            return result;
        }

        public static string Join(List<string> segments) {
            return string.Join(SegmentSeparatorAsString, segments.ToArray());
        }

        public static string PathToKey(string path) {
            if (path == null) return string.Empty;

            return path.Replace(SegmentSeparator, DictConsts.KeySeparator);
        }

        public static string KeyToPath(string key) {
            if (key == null) return string.Empty;

            return key.Replace(DictConsts.KeySeparator, SegmentSeparator);
        }

        public static string Encode(string path, string relPath) {
            if (string.IsNullOrEmpty(path)) {
                if (relPath == null) return string.Empty;

                return relPath;
            } else {
                return string.Format("{0}{1}{2}", path, SegmentSeparator, relPath);
            }
        }
    }
}
