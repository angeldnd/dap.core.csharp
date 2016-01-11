using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;

namespace angeldnd.dap {
    /*
    public static class RegistryHelper {
        public static Registry GetRegistry(Aspect aspect) {
            if (aspect == null) {
                return null;
            } else if (aspect is ItemAspect) {
                return ((ItemAspect)aspect).Registry;
            } else if (aspect is Item) {
                return ((Item)aspect).Registry;
            } else if (aspect.Entity is Aspect) {
                return GetRegistry((Aspect)aspect.Entity);
            }
            return null;
        }

        public static int GetDepth(string path) {
            if (string.IsNullOrEmpty(path)) return 0;
            int depth = 1;
            foreach (char ch in path) {
                if (ch == RegistryConsts.Separator) {
                    depth++;
                }
            }
            return depth;
        }

        public static string GetName(string path) {
            if (string.IsNullOrEmpty(path)) return null;
            int pos = path.LastIndexOf(RegistryConsts.Separator);
            if (pos >= 0) {
                return path.Substring(pos + 1);
            }
            return path;
        }

        public static string GetDescendantPath(string path, string relativePath) {
            return GetAbsolutePath(path, relativePath);
        }

        public static string GetDescendantsPattern(string path) {
            if (string.IsNullOrEmpty(path)) {
                return PatternMatcherConsts.WildcastSegments;
            } else {
                return path + RegistryConsts.Separator + PatternMatcherConsts.WildcastSegments;
            }
        }

        public static string GetChildrenPattern(string path) {
            if (string.IsNullOrEmpty(path)) {
                return PatternMatcherConsts.WildcastSegment;
            } else {
                return path + RegistryConsts.Separator + PatternMatcherConsts.WildcastSegment;
            }
        }

        public static string GetParentPath(string path) {
            return AspectHelper.GetParentPath(path, RegistryConsts.Separator);
        }

        public static string GetAbsolutePath(string ancestorPath, string relativePath) {
            if (string.IsNullOrEmpty(ancestorPath)) {
                return relativePath;
            } else if (string.IsNullOrEmpty(relativePath)) {
                return string.Empty;
            } else {
                return string.Format("{0}{1}{2}", ancestorPath, RegistryConsts.Separator, relativePath);
            }
        }

        public static string GetAbsolutePath(Item item, string relativePath) {
            return GetAbsolutePath(item.Path, relativePath);
        }

        public static string GetAbsolutePath(ItemAspect ancestorAspect, string relativePath) {
            return GetAbsolutePath(ancestorAspect.ItemPath, relativePath);
        }

        public static string GetRelativePath(string ancestorPath, string descendantPath) {
            string prefix = ancestorPath + RegistryConsts.Separator;
            if (descendantPath.StartsWith(prefix)) {
                return descendantPath.Replace(prefix, "");
            } else {
                Log.Error("Is Not Desecendant: {0}, {1}", ancestorPath, descendantPath);
            }
            return null;
        }

        public static string GetRelativePath(Item item, string descendantPath) {
            return GetRelativePath(item.Path, descendantPath);
        }

        public static string GetRelativePath(ItemAspect ancestorAspect, string descendantPath) {
            return GetRelativePath(ancestorAspect.ItemPath, descendantPath);
        }

        public static T GetDescendantAspect<T>(this Registry registry, string path, string relativePath, string aspectPath) where T : class, ItemAspect {
            Item item = registry.GetDescendant<Item>(path, relativePath);
            if (item != null) {
                return item.GetItemAspect<T>(aspectPath);
            }
            return null;
        }

        public static T GetDescendantAspect<T>(this Registry registry, Item item, string relativePath, string aspectPath) where T : class, ItemAspect {
            return GetDescendantAspect<T>(registry, item.Path, relativePath, aspectPath);
        }

        public static T GetDescendantAspect<T>(this Registry registry, ItemAspect itemAspect, string relativePath, string aspectPath) where T : class, ItemAspect {
            return GetDescendantAspect<T>(registry, itemAspect.ItemPath, relativePath, aspectPath);
        }
    }
    */
}
