using System;
using System.Diagnostics;
using System.IO;
using System.Text;
//using System.Text.RegularExpressions;

namespace angeldnd.dap {
    public static class AspectHelper {
        public static string GetParentPath(string path) {
            return GetParentPath(path, EntityConsts.Separator);
        }

        public static string GetParentPath(string path, char separator) {
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

        public static Entity GetRootEntity(Aspect aspect) {
            if (aspect == null) return null;
            Entity result = aspect.Entity;
            while (result != null && result is Aspect) {
                Entity upper = (result as Aspect).Entity;
                if (upper == null) {
                    break;
                } else {
                    result = upper;
                }
            }
            return result;
        }

        public static int CompareAspect(Aspect aspectA, Aspect aspectB) {
            if (aspectA == null && aspectB == null) return 0;
            if (aspectA == null) return -1;
            if (aspectB == null) return 1;
            if (aspectA.Path == null) return -1;
            return aspectA.Path.CompareTo(aspectB.Path);
        }

        /*
         * IMPORTANT: the generic name can NOT be the same as the non-generic one
         * otherwise will case loop back call (at least in unity)
         */
        public static int Compare<T>(T aspectA, T aspectB) where T : Aspect {
            return CompareAspect(aspectA, aspectB);
        }

        public static int CompareAspectAccessor(Accessor accessorA, Accessor accessorB) {
            if (accessorA == null && accessorB == null) return 0;
            if (accessorA == null) return -1;
            if (accessorB == null) return 1;
            return CompareAspect(accessorA.GetObject() as Aspect, accessorB.GetObject() as Aspect);
        }

        public static int CompareAccessor<T>(T aspectA, T aspectB) where T : Accessor {
            return CompareAspectAccessor(aspectA, aspectB);
        }
    }
}
