using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class PathHelper {
        public static int GetDepth(string path) {
            return SegmentHelper.GetDepth(PathConsts.SegmentSeparator, path);
        }

        public static string GetSegment(string path) {
            return SegmentHelper.GetSegment(PathConsts.SegmentSeparator, path);
        }

        public static string GetParentPath(string path) {
            return SegmentHelper.GetParentStr(PathConsts.SegmentSeparator, path);
        }

        public static string GetDescendantPath(string path, string relativePath) {
            return SegmentHelper.GetDescendantStr(PathConsts.SegmentSeparatorAsString, path, relativePath);
        }

        public static string GetDescendantsPattern(string path) {
            return SegmentHelper.GetDescendantsPattern(PathConsts.SegmentSeparatorAsString, path);
        }

        public static string GetChildrenPattern(string path) {
            return SegmentHelper.GetChildrenPattern(PathConsts.SegmentSeparatorAsString, path);
        }

        public static string GetRelativePath(string ancestorPath, string descendantPath) {
            return SegmentHelper.GetRelativeStr(PathConsts.SegmentSeparator, ancestorPath, descendantPath);
        }
    }
}
