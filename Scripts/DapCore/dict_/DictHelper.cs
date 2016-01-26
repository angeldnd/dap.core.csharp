using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public static class DictHelper {
        public static int GetDepth(char separator, string key) {
            if (string.IsNullOrEmpty(key)) return 0;
            int depth = 1;
            foreach (char ch in key) {
                if (ch == separator) {
                    depth++;
                }
            }
            return depth;
        }

        public static string GetName(char separator, string key) {
            if (string.IsNullOrEmpty(key)) return null;
            int pos = key.LastIndexOf(separator);
            if (pos >= 0) {
                return key.Substring(pos + 1);
            }
            return key;
        }

        public static string GetParentKey(char separator, string key) {
            if (string.IsNullOrEmpty(key)) return null;

            string[] segments = key.Split(separator);
            if (segments.Length <= 1) return null;

            StringBuilder parentKey = new StringBuilder();
            for (int i = 0; i < segments.Length - 1; i++) {
                parentKey.Append(segments[i]);
                if (i < segments.Length - 2) {
                    parentKey.Append(separator);
                }
            }
            return parentKey.ToString();
        }

        public static string GetDescendantKey(char separator, string key, string relativeKey) {
            if (string.IsNullOrEmpty(key)) {
                return relativeKey;
            } else if (string.IsNullOrEmpty(relativeKey)) {
                return string.Empty;
            } else {
                return string.Format("{0}{1}{2}", key, separator, relativeKey);
            }
        }

        public static string GetDescendantsPattern(char separator, string key) {
            if (string.IsNullOrEmpty(key)) {
                return PatternMatcherConsts.WildcastSegments;
            } else {
                return key + separator + PatternMatcherConsts.WildcastSegments;
            }
        }

        public static string GetChildrenPattern(char separator, string key) {
            if (string.IsNullOrEmpty(key)) {
                return PatternMatcherConsts.WildcastSegment;
            } else {
                return key + separator + PatternMatcherConsts.WildcastSegment;
            }
        }

        public static string GetRelativeKey(char separator, string ancestorKey, string descendantKey) {
            string prefix = ancestorKey + separator;
            if (descendantKey.StartsWith(prefix)) {
                return descendantKey.Replace(prefix, "");
            } else {
                Log.Error("Is Not Desecendant: {0}, {1}", ancestorKey, descendantKey);
            }
            return null;
        }

        public static int CompareElement(IInDictElement elementA, IInDictElement elementB) {
            if (elementA == null && elementB == null) return 0;
            if (elementA == null) return -1;
            if (elementB == null) return 1;
            if (elementA.Key == null) return -1;
            return elementA.Key.CompareTo(elementB.Key);
        }

        /*
         * IMPORTANT: the generic name can NOT be the same as the non-generic one
         * otherwise will case loop back call (at least in unity)
         */
        public static int Compare<T>(T elementA, T elementB) where T : IInDictElement {
            return CompareElement(elementA, elementB);
        }

        public static int CompareElementAccessor(IAccessor<IInDictElement> accessorA, IAccessor<IInDictElement> accessorB) {
            if (accessorA == null && accessorB == null) return 0;
            if (accessorA == null) return -1;
            if (accessorB == null) return 1;
            return CompareElement(accessorA.Obj, accessorB.Obj);
        }

        public static int CompareAccessor<T>(T aspectA, T aspectB) where T : IAccessor<IInDictElement> {
            return CompareElementAccessor(aspectA, aspectB);
        }
    }
}
