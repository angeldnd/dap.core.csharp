using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class ContextExtension {
        public static bool RemoveFromOwner(this IContext context) {
            IDict owner = context.OwnerAsDict;
            if (owner == null) return false;

            return owner.Remove<IContext>(context.Key) != null;
        }

        public static T GetAncestor<T>(this IContext context) where T : class, IContext {
            return TreeHelper.GetAncestor<T>(context);
        }

        public static IContext GetAncestor(this IContext context) {
            return GetAncestor<IContext>(context);
        }

        public static T GetContext<T>(this IDictContext context, string relPath, bool logError)
                                            where T : class, IContext {
            return TreeHelper.GetDescendant<T>(context, relPath, logError);
        }

        public static IContext GetContext(this IDictContext context, string relPath, bool logError) {
            return GetContext<IContext>(context, relPath, logError);
        }

        public static T GetContext<T>(this IDictContext context, string relPath) where T : class, IContext {
            return GetContext<T>(context, relPath, true);
        }

        public static IContext GetContext(this IDictContext context, string relPath) {
            return GetContext<IContext>(context, relPath);
        }

        public static T GetContextManner<T>(this IDictContext context, string relPath, string mannerKey, bool logError)
                                                    where T : Manner {
            IContext descendant = GetContext<IContext>(context, relPath, logError);
            if (descendant == null) {
                T manner = descendant.Manners.Get<T>(mannerKey, logError);
                return manner;
            }
            return null;
        }

        public static T GetContextManner<T>(this IDictContext context, string relPath, string mannerKey)
                                                    where T : Manner {
            return GetContextManner<T>(context, relPath, mannerKey, true);
        }

        public static string GetRelativePath(this IDictContext context, IContext descendant) {
            return PathHelper.GetRelativePath(context.Path, descendant.Path);
        }

        public static void ForEachContexts<T>(this IDictContext context, Action<T> callback)
                                                    where T : class, IContext {
            TreeHelper.ForEachDescendants<T>(context, callback);
        }

        public static List<T> GetContexts<T>(this IDictContext context)
                                                    where T : class, IContext {
            return TreeHelper.GetDescendants<T>(context);
        }

        public static void ForEachContextsWithManner<T>(this IDictContext context, string mannerKey, Action<T> callback)
                                                    where T : Manner {
            TreeHelper.ForEachDescendants<IContext>(context, (IContext element) => {
                T manner = element.Manners.Get<T>(mannerKey, false);
                if (manner != null) {
                    callback(manner);
                }
            });
        }

        public static List<T> GetContextsWithManner<T>(this IDictContext context, string mannerKey)
                                                    where T : Manner {
            List<T> result = null;
            ForEachContextsWithManner<T>(context, mannerKey, (T manner) => {
                if (result == null) {
                    result = new List<T>();
                }
                result.Add(manner);
            });
            return result;
        }

        public static T AddContext<TO, T>(this IDictContext context, string relPath)
                                                    where TO : class, IDictContext
                                                    where T : class, IContext {
            return TreeHelper.AddDescendant<TO, T>(context, relPath);
        }

        public static T AddContext<T>(this IDictContext context, string relPath)
                                                    where T : class, IContext {
            return TreeHelper.AddDescendant<Items, T>(context, relPath);
        }

        public static T GetOrAddContext<TO, T>(this IDictContext context, string relPath)
                                                    where TO : class, IDictContext
                                                    where T : class, IContext {
            return TreeHelper.GetOrAddDescendant<TO, T>(context, relPath);
        }

        public static T GetOrAddContext<T>(this IDictContext context, string relPath)
                                                    where T : class, IContext {
            return TreeHelper.GetOrAddDescendant<Items, T>(context, relPath);
        }

        public static T NewContext<TO, T>(this IDictContext context, string type, string relPath)
                                                    where TO : class, IDictContext
                                                    where T : class, IContext {
            return TreeHelper.NewDescendant<TO, T>(context, type, relPath);
        }

        public static T NewContext<T>(this IDictContext context, string type, string relPath)
                                                    where T : class, IContext {
            return TreeHelper.NewDescendant<Items, T>(context, type, relPath);
        }

        public static IContext NewContext(this IDictContext context, string type, string relPath) {
            return TreeHelper.NewDescendant<Items, IContext>(context, type, relPath);
        }

        public static T NewContextWithManner<T>(this IDictContext context,
                                    string type, string relPath, string mannerKey)
                                                    where T : Manner {
            IContext descendant = NewContext(context, type, relPath);
            if (descendant != null) {
                return descendant.Manners.Add<T>(mannerKey);
            }
            return null;
        }

        public static T GetOrNewContext<T>(this IDictContext context, string type, string relPath)
                                                    where T : class, IContext {
            return TreeHelper.GetOrNewDescendant<Items, T>(context, type, relPath);
        }

        public static IContext GetOrNewContext(this IDictContext context, string type, string relPath) {
            return TreeHelper.GetOrNewDescendant<Items, IContext>(context, type, relPath);
        }

        public static T GetOrNewContextWithManner<T>(this IDictContext context,
                                    string type, string relPath, string mannerKey)
                                                    where T : Manner {
            IContext descendant = GetOrNewContext(context, type, relPath);
            if (descendant != null) {
                return descendant.Manners.Add<T>(mannerKey);
            }
            return null;
        }
    }
}
