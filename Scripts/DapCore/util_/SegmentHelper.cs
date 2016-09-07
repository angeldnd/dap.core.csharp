using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public static class SegmentHelper {
        public static int GetDepth(char separator, string str) {
            if (string.IsNullOrEmpty(str)) return 0;
            int depth = 1;
            foreach (char ch in str) {
                if (ch == separator) {
                    depth++;
                }
            }
            return depth;
        }

        public static string GetSegment(char separator, string str) {
            if (string.IsNullOrEmpty(str)) return null;
            int pos = str.LastIndexOf(separator);
            if (pos >= 0) {
                return str.Substring(pos + 1);
            }
            return str;
        }

        public static string GetParentStr(char separator, string str) {
            if (string.IsNullOrEmpty(str)) return str == null ? null : "";

            string[] segments = str.Split(separator);
            if (segments.Length <= 1) return null;

            StringBuilder parentStr = new StringBuilder();
            for (int i = 0; i < segments.Length - 1; i++) {
                parentStr.Append(segments[i]);
                if (i < segments.Length - 2) {
                    parentStr.Append(separator);
                }
            }
            return parentStr.ToString();
        }

        public static string GetDescendantStr(string separator, string str, string relativeStr) {
            if (string.IsNullOrEmpty(str)) {
                return relativeStr;
            } else if (string.IsNullOrEmpty(relativeStr)) {
                return str;
            } else {
                return string.Format("{0}{1}{2}", str, separator, relativeStr);
            }
        }

        public static string GetDescendantsPattern(string separator, string str) {
            if (string.IsNullOrEmpty(str)) {
                return PatternMatcherConsts.WildcastSegments;
            } else {
                return string.Format("{0}{1}{2}", str, separator, PatternMatcherConsts.WildcastSegments);
            }
        }

        public static string GetChildrenPattern(string separator, string str) {
            if (string.IsNullOrEmpty(str)) {
                return PatternMatcherConsts.WildcastSegment;
            } else {
                return string.Format("{0}{1}{2}", str, separator, PatternMatcherConsts.WildcastSegment);
            }
        }

        public static string GetRelativeStr(char separator, string ancestorStr, string descendantStr) {
            if (string.IsNullOrEmpty(ancestorStr)) {
                return descendantStr;
            }

            string result = null;
            if (descendantStr.StartsWith(ancestorStr)) {
                result = descendantStr.Remove(0, ancestorStr.Length);
            }
            if (result != null) {
                if (result.Length > 0 && result[0] == separator) {
                    result = result.Remove(0, 1);
                } else {
                    result = null;
                }
            }
            if (result == null) {
                Log.Error("Is Not Desecendant: {0}, {1}", ancestorStr, descendantStr);
            }
            return result;
        }
    }
}
