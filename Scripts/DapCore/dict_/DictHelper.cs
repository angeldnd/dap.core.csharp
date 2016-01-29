using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public static class DictHelper {
        public static int GetDepth(string key) {
            return SegmentHelper.GetDepth(DictConsts.KeySeparator, key);
        }

        public static string GetSegment(string key) {
            return SegmentHelper.GetSegment(DictConsts.KeySeparator, key);
        }

        public static string GetParentKey(string key) {
            return SegmentHelper.GetParentStr(DictConsts.KeySeparator, key);
        }

        public static string GetDescendantKey(string key, string relativeKey) {
            return SegmentHelper.GetDescendantStr(DictConsts.KeySeparator, key, relativeKey);
        }

        public static string GetDescendantsPattern(string key) {
            return SegmentHelper.GetDescendantsPattern(DictConsts.KeySeparator, key);
        }

        public static string GetChildrenPattern(string key) {
            return SegmentHelper.GetChildrenPattern(DictConsts.KeySeparator, key);
        }

        public static string GetRelativeKey(string ancestorKey, string descendantKey) {
            return SegmentHelper.GetRelativeStr(DictConsts.KeySeparator, ancestorKey, descendantKey);
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

        public static int CompareAccessor<T>(T accessorA, T accessorB) where T : IAccessor<IInDictElement> {
            return CompareElementAccessor(accessorA, accessorB);
        }
    }
}
