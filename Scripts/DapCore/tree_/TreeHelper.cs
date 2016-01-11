using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public static class TreeHelper {
        public static int GetDepth(char separator, string path) {
            if (string.IsNullOrEmpty(path)) return 0;
            int depth = 1;
            foreach (char ch in path) {
                if (ch == separator) {
                    depth++;
                }
            }
            return depth;
        }

        public static string GetName(char separator, string path) {
            if (string.IsNullOrEmpty(path)) return null;
            int pos = path.LastIndexOf(separator);
            if (pos >= 0) {
                return path.Substring(pos + 1);
            }
            return path;
        }

        public static string GetParentPath(char separator, string path) {
            if (string.IsNullOrEmpty(path)) return null;

            string[] segments = path.Split(separator);
            if (segments.Length <= 1) return null;

            StringBuilder parentPath = new StringBuilder();
            for (int i = 0; i < segments.Length - 1; i++) {
                parentPath.Append(segments[i]);
                if (i < segments.Length - 2) {
                    parentPath.Append(separator);
                }
            }
            return parentPath.ToString();
        }

        public static string GetDescendantPath(char separator, string path, string relativePath) {
            if (string.IsNullOrEmpty(path)) {
                return relativePath;
            } else if (string.IsNullOrEmpty(relativePath)) {
                return string.Empty;
            } else {
                return string.Format("{0}{1}{2}", path, separator, relativePath);
            }
        }

        public static string GetDescendantsPattern(char separator, string path) {
            if (string.IsNullOrEmpty(path)) {
                return PatternMatcherConsts.WildcastSegments;
            } else {
                return path + separator + PatternMatcherConsts.WildcastSegments;
            }
        }

        public static string GetChildrenPattern(char separator, string path) {
            if (string.IsNullOrEmpty(path)) {
                return PatternMatcherConsts.WildcastSegment;
            } else {
                return path + separator + PatternMatcherConsts.WildcastSegment;
            }
        }

        public static string GetRelativePath(char separator, string ancestorPath, string descendantPath) {
            string prefix = ancestorPath + separator;
            if (descendantPath.StartsWith(prefix)) {
                return descendantPath.Replace(prefix, "");
            } else {
                Log.Error("Is Not Desecendant: {0}, {1}", ancestorPath, descendantPath);
            }
            return null;
        }

        public static int CompareElement(IElement elementA, IElement elementB) {
            if (elementA == null && elementB == null) return 0;
            if (elementA == null) return -1;
            if (elementB == null) return 1;
            if (elementA.Path == null) return -1;
            return elementA.Path.CompareTo(elementB.Path);
        }

        /*
         * IMPORTANT: the generic name can NOT be the same as the non-generic one
         * otherwise will case loop back call (at least in unity)
         */
        public static int Compare<T>(T elementA, T elementB) where T : IElement {
            return CompareElement(elementA, elementB);
        }

        public static int CompareElementAccessor(IAccessor<IElement> accessorA, IAccessor<IElement> accessorB) {
            if (accessorA == null && accessorB == null) return 0;
            if (accessorA == null) return -1;
            if (accessorB == null) return 1;
            return CompareElement(accessorA.Obj, accessorB.Obj);
        }

        public static int CompareAccessor<T>(T aspectA, T aspectB) where T : IAccessor<IElement> {
            return CompareElementAccessor(aspectA, aspectB);
        }
    }
}
