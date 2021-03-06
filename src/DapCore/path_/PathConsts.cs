using System;
using System.Collections.Generic;
using System.Linq;

namespace angeldnd.dap {
    public static class PathConsts {
        public const char SegmentSeparator = '/';
        public const string SegmentSeparatorAsString = "/";

        public static List<string> Split(string path) {
            if (path == null) return null;
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
            if (segments == null) return null;
            return Join(segments.ToArray());
        }

        public static string Join(params string[] segments) {
            return string.Join(SegmentSeparatorAsString, segments);
        }

        public static string PathToKey(string path) {
            if (path == null) return null;
            return path.Replace(SegmentSeparator, DictConsts.KeySeparator);
        }

        public static string KeyToPath(string key) {
            if (key == null) return null;
            return key.Replace(DictConsts.KeySeparator, SegmentSeparator);
        }

        public static string Encode(string path, string relPath) {
            if (string.IsNullOrEmpty(path)) {
                return relPath;
            } else {
                return string.Format("{0}{1}{2}", path, SegmentSeparatorAsString, relPath);
            }
        }
    }
}
