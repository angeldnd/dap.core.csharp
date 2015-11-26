using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;

namespace angeldnd.dap {
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

        public static string GetParentPath(string path) {
            return AspectHelper.GetParentPath(path, RegistryConsts.Separator);
        }

        public static string GetAbsolutePath(string ancestorPath, string relativePath) {
            return string.Format("{0}{1}{2}", ancestorPath, RegistryConsts.Separator, relativePath);
        }

        public static string GetAbsolutePath(ItemAspect ancestorAspect, string relativePath) {
            return GetAbsolutePath(ancestorAspect.Item.Path, relativePath);
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

        public static string GetRelativePath(ItemAspect ancestorAspect, string descendantPath) {
            return GetRelativePath(ancestorAspect.Item.Path, descendantPath);
        }
    }
}
