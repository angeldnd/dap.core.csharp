using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class PathHelper {
        public static int GetDepth(string path) {
            return SegmentHelper.GetDepth(PathConsts.PathSeparator, path);
        }

        public static string GetSegment(string path) {
            return SegmentHelper.GetSegment(PathConsts.PathSeparator, path);
        }

        public static string GetParentPath(string path) {
            return SegmentHelper.GetParentStr(PathConsts.PathSeparator, path);
        }

        public static string GetDescendantPath(string path, string relativePath) {
            return SegmentHelper.GetDescendantStr(PathConsts.PathSeparator, path, relativePath);
        }

        public static string GetDescendantsPattern(string path) {
            return SegmentHelper.GetDescendantsPattern(PathConsts.PathSeparator, path);
        }

        public static string GetChildrenPattern(string path) {
            return SegmentHelper.GetChildrenPattern(PathConsts.PathSeparator, path);
        }

        public static string GetRelativePath(string ancestorPath, string descendantPath) {
            return SegmentHelper.GetRelativeStr(PathConsts.PathSeparator, ancestorPath, descendantPath);
        }
    }
}
