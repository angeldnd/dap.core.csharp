using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class ContextExtension {
        public static T GetAncestor<T>(this IContext context) where T : class, IContext {
            return TreeHelper.GetAncestor<T>(context);
        }

        public static IContext GetAncestor(this IContext context) {
            return GetAncestor<IContext>(context);
        }

        public static T GetDescendant<T>(this IDictContext context, string relPath, bool logError)
                                            where T : class, IContext {
            return TreeHelper.GetDescendant<T>(context, relPath, logError);
        }

        public static IContext GetDescendant(this IDictContext context, string relPath, bool logError) {
            return GetDescendant<IContext>(context, relPath, logError);
        }

        public static T GetDescendant<T>(this IDictContext context, string relPath) where T : class, IContext {
            return GetDescendant<T>(context, relPath, true);
        }

        public static IContext GetDescendant(this IDictContext context, string relPath) {
            return GetDescendant<IContext>(context, relPath);
        }

        public static T GetDescendantManner<T>(this IDictContext context, string relPath, string mannerKey, bool logError)
                                                    where T : Manner {
            IContext descendant = GetDescendant<IContext>(context, relPath, logError);
            if (descendant == null) {
                T manner = descendant.Manners.Get<T>(mannerKey, logError);
                return manner;
            }
            return null;
        }

        public static T GetDescendantManner<T>(this IDictContext context, string relPath, string mannerKey)
                                                    where T : Manner {
            return GetDescendantManner<T>(context, relPath, mannerKey, true);
        }

        public static string GetRelativePath(this IDictContext context, IContext descendant) {
            return PathHelper.GetRelativePath(context.Path, descendant.Path);
        }

        public static void ForEachDescendants<T>(this IDictContext context, Action<T> callback)
                                                    where T : class, IContext {
            TreeHelper.ForEachDescendants<T>(context, callback);
        }

        public static List<T> GetDescendants<T>(this IDictContext context)
                                                    where T : class, IContext {
            return TreeHelper.GetDescendants<T>(context);
        }

        public static void ForEachDescendantsWithManner<T>(this IDictContext context, string mannerKey, Action<T> callback)
                                                    where T : Manner {
            TreeHelper.ForEachDescendants<IContext>(context, (IContext element) => {
                T manner = element.Manners.Get<T>(mannerKey, false);
                if (manner != null) {
                    callback(manner);
                }
            });
        }

        public static List<T> GetDescendantsWithManner<T>(this IDictContext context, string mannerKey)
                                                    where T : Manner {
            List<T> result = null;
            ForEachDescendantsWithManner<T>(context, mannerKey, (T manner) => {
                if (result == null) {
                    result = new List<T>();
                }
                result.Add(manner);
            });
            return result;
        }

        public static T AddDescendant<TO, T>(this IDictContext context, string relPath)
                                                    where TO : class, IDictContext
                                                    where T : class, IContext {
            return TreeHelper.AddDescendant<TO, T>(context, relPath);
        }

        public static T AddDescendant<T>(this IDictContext context, string relPath)
                                                    where T : class, IContext {
            return TreeHelper.AddDescendant<Items, T>(context, relPath);
        }

        public static Item AddDescendant(this IDictContext context, string relPath) {
            return TreeHelper.AddDescendant<Items, Item>(context, relPath);
        }

        public static T NewDescendant<TO, T>(this IDictContext context, string type, string relPath)
                                                    where TO : class, IDictContext
                                                    where T : class, IContext {
            return TreeHelper.NewDescendant<TO, T>(context, type, relPath);
        }

        public static T NewDescendant<T>(this IDictContext context, string type, string relPath)
                                                    where T : class, IContext {
            return TreeHelper.NewDescendant<Items, T>(context, type, relPath);
        }

        public static IContext NewDescendant(this IDictContext context, string type, string relPath) {
            return TreeHelper.NewDescendant<Items, IContext>(context, type, relPath);
        }

        public static T NewDescendantWithManner<T>(this IDictContext context,
                                    string type, string relPath, string mannerKey)
                                                    where T : Manner {
            IContext descendant = NewDescendant(context, type, relPath);
            if (descendant != null) {
                return descendant.Manners.Add<T>(mannerKey);
            }
            return null;
        }
    }
}
