using System;
using System.Diagnostics;
using System.IO;
//using System.Text.RegularExpressions;

namespace angeldnd.dap {
    public class AspectHelper {
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

        public static int CompareAspectAccessor(AspectAccessor accessorA, AspectAccessor accessorB) {
            if (accessorA == null && accessorB == null) return 0;
            if (accessorA == null) return -1;
            if (accessorB == null) return 1;
            return CompareAspect(accessorA.Aspect, accessorB.Aspect);
        }

        public static int CompareAccessor<T>(T aspectA, T aspectB) where T : AspectAccessor {
            return CompareAspectAccessor(aspectA, aspectB);
        }
    }
}
