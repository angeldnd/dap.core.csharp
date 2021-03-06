using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class ContextExtension {
        public static bool RemoveFromOwner(this IContext context) {
            IDict owner = context.OwnerAsDict;
            if (owner == null) return false;

            return owner.Remove<IContext>(context.Key) != null;
        }

        public static T GetAncestor<T>(this IContext context, Func<T, bool> checker = null) where T : class, IContext {
            return TreeHelper.GetAncestor<T>(context, checker);
        }

        public static T GetAncestorManner<T>(this IContext context, string mannerKey)
                                                    where T : class, IManner {
            T result = null;
            GetAncestor<IContext>(context, (IContext ancestor) => {
                result = ancestor.Manners.Get<T>(mannerKey, true);
                return result != null;
            });
            return result;
        }

        public static T GetOwnOrAncestorManner<T>(this IContext context, string mannerKey)
                                                    where T : class, IManner {
            T manner = context.Manners.Get<T>(mannerKey, true);
            if (manner != null) return manner;

            return GetAncestorManner<T>(context, mannerKey);
        }

        public static T GetContext<T>(this IDictContext context, string relPath,
                                        bool isDebug = false)
                                            where T : class, IContext {
            return TreeHelper.GetDescendant<T>(context, relPath, isDebug);
        }

        public static IContext GetContext(this IDictContext context, string relPath,
                                            bool isDebug = false) {
            return GetContext<IContext>(context, relPath, isDebug);
        }

        public static bool HasContext(this IDictContext context, string relPath) {
            return GetContext<IContext>(context, relPath, true) != null;
        }

        public static T GetContextManner<T>(this IDictContext context, string relPath,
                                                string mannerKey, bool isDebug = false)
                                                    where T : class, IManner {
            IContext descendant = GetContext<IContext>(context, relPath, isDebug);
            if (descendant != null) {
                T manner = descendant.Manners.Get<T>(mannerKey, isDebug);
                return manner;
            }
            return null;
        }

        //Note: Use IContext here so no need to cast to IDictContext
        public static string GetRelativePath(this IContext context, IContext descendant) {
            return PathHelper.GetRelativePath(context.Path, descendant.Path);
        }

        public static void ForEachContexts<T>(this IDictContext context, Action<T> callback)
                                                    where T : class, IContext {
            TreeHelper.ForEachDescendants<T>(context, callback);
        }

        public static void ForEachContexts(this IDictContext context, Action<IContext> callback) {
            ForEachContexts<IContext>(context, callback);
        }

        public static List<T> GetContexts<T>(this IDictContext context)
                                                    where T : class, IContext {
            return TreeHelper.GetDescendants<T>(context);
        }

        public static void ForEachContextsWithManner<T>(this IDictContext context, string mannerKey, Action<T> callback)
                                                    where T : class, IManner {
            TreeHelper.ForEachDescendants<IContext>(context, (IContext element) => {
                T manner = element.Manners.Get<T>(mannerKey, true);
                if (manner != null) {
                    callback(manner);
                }
            });
        }

        public static List<T> GetContextsWithManner<T>(this IDictContext context, string mannerKey)
                                                    where T : class, IManner {
            List<T> result = null;
            ForEachContextsWithManner<T>(context, mannerKey, (T manner) => {
                if (result == null) {
                    result = new List<T>();
                }
                result.Add(manner);
            });
            return result;
        }

        public static T FirstContext<T>(this IDictContext context, Func<T, bool> callback, bool isDebug = false)
                                                    where T : class, IContext {
            T result = null;
            TreeHelper.ForEachDescendants<T>(context, (T element) => {
                if (result == null && callback(element)) {
                    result = element;
                }
            });
            if (result == null) {
                context.ErrorOrDebug(isDebug, "First<{0}>({1}): Not Found", typeof(T).FullName, callback);
            }
            return result;
        }

        public static T FirstContext<T>(this IDictContext context, bool isDebug = false)
                                                    where T : class, IContext {
            return FirstContext<T>(context, (T element) => { return true; }, isDebug);
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

        public static T NewContext<T>(this IDictContext context, string relPath)
                                                    where T : class, IContext {
            string dapType = DapType.GetDapType(typeof(T));
            if (dapType == null) {
                context.Error("NewContext Failed, DapType Not Defined: " + typeof(T));
                return null;
            }
            return NewContext<T>(context, dapType, relPath);
        }

        public static IContext NewContext(this IDictContext context, string type, string relPath) {
            return TreeHelper.NewDescendant<Items, IContext>(context, type, relPath);
        }

        public static T NewContextWithManner<T>(this IDictContext context,
                                    string type, string relPath, string mannerKey)
                                                    where T : class, IManner {
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

        public static T GetOrNewContext<T>(this IDictContext context, string relPath)
                                                    where T : class, IContext {
            string dapType = DapType.GetDapType(typeof(T));
            if (dapType == null) {
                context.Error("GetOrNewContext Failed, DapType Not Defined: " + typeof(T));
                return null;
            }
            return TreeHelper.GetOrNewDescendant<Items, T>(context, dapType, relPath);
        }

        public static IContext GetOrNewContext(this IDictContext context, string type, string relPath) {
            return TreeHelper.GetOrNewDescendant<Items, IContext>(context, type, relPath);
        }

        public static T GetOrNewContextWithManner<T>(this IDictContext context,
                                    string type, string relPath, string mannerKey)
                                                    where T : class, IManner {
            IContext descendant = GetOrNewContext(context, type, relPath);
            if (descendant != null) {
                return descendant.Manners.Add<T>(mannerKey);
            }
            return null;
        }
    }
}
