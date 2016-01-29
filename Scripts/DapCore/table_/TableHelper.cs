using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public static class TableHelper {
        public static int CompareElement(IInTableElement elementA, IInTableElement elementB) {
            if (elementA == null && elementB == null) return 0;
            if (elementA == null) return -1;
            if (elementB == null) return 1;
            return elementA.Index.CompareTo(elementB.Index);
        }

        /*
         * IMPORTANT: the generic name can NOT be the same as the non-generic one
         * otherwise will case loop back call (at least in unity)
         */
        public static int Compare<T>(T elementA, T elementB) where T : IInTableElement {
            return CompareElement(elementA, elementB);
        }

        public static int CompareElementAccessor(IAccessor<IInTableElement> accessorA, IAccessor<IInTableElement> accessorB) {
            if (accessorA == null && accessorB == null) return 0;
            if (accessorA == null) return -1;
            if (accessorB == null) return 1;
            return CompareElement(accessorA.Obj, accessorB.Obj);
        }

        public static int CompareAccessor<T>(T accessorA, T accessorB) where T : IAccessor<IInTableElement> {
            return CompareElementAccessor(accessorA, accessorB);
        }
    }
}
