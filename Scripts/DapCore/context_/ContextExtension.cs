using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class ContextExtension {
        public static string[] GetKeys(this IContext context) {
            List<string> keys = new List<string>();
            IContext c = context;
            while (c != null) {
                if (!string.IsNullOrEmpty(c.Key)) {
                    keys.Add(c.Key);
                }
                c = c.GetOwner() as IContext;
            }
            keys.Reverse();
            return keys.ToArray();
        }

        public static T GetAncestor<T>(this IContext context) where T : class, IContext {
            IContext c = context;
            while (c != null) {
                c = c.GetOwner() as IContext;
                if (c == null) {
                    context.Error("Ancestor Not Found: <{0}>", typeof(T).FullName);
                    return null;
                }
                T ancestor = c as T;
                if (ancestor != null) {
                    return ancestor;
                }
            }
            return null;
        }

        public static IContext GetAncestor(this IContext context) {
            return GetAncestor<IContext>(context);
        }

        public static T GetDescendant<T>(this IDictContext context, string relPath, bool logError)
                                            where T : class, IContext {
            string[] keys = relPath.Split(ContextConsts.ContextSeparator);
            IDictContext dict = context;
            for (int i = 0; i < keys.Length; i++) {
                string key = keys[i];
                IContext element = dict.Get<IContext>(key);
                if (element == null) {
                    if (logError) {
                        dict.Error("Not Found: {0} -> {1}: {2}", relPath, i, key);
                    }
                    return null;
                }
                if (i == keys.Length - 1) {
                    T result = element as T;
                    if (result == null) {
                        if (logError) {
                            element.Error("Type Mismatched: <{0}> {1} -> {2}: {3}",
                                            typeof(T).FullName, relPath, i, key);
                        }
                    }
                    return result;
                } else {
                    dict = element as IDictContext;
                    if (dict == null) {
                        if (logError) {
                            element.Error("Not IDictContext: {0} -> {1}: {2}",
                                            relPath, i, key);
                        }
                        return null;
                    }
                }
            }
            //In case of empty relPath, return itself.
            return context as T;
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
                T manner;
                if (logError) {
                    manner = descendant.Manners.Get<T>(mannerKey);
                } else {
                    descendant.Manners.TryGet<T>(mannerKey, out manner);
                }
                return manner;
            }
            return null;
        }

        public static T GetDescendantManner<T>(this IDictContext context, string relPath, string mannerKey)
                                                    where T : Manner {
            return GetDescendantManner<T>(context, relPath, mannerKey, true);
        }

        public static string GetRelativePath(this IDictContext context, IContext descendant) {
            string prefix = context.Path + ContextConsts.ContextSeparator;
            if (descendant.Path.StartsWith(prefix)) {
                return descendant.Path.Replace(prefix, "");
            } else {
                context.Error("Is Not Desecendant: {0}", descendant);
            }
            return null;
        }

        public static void ForEachDescendantsWithManner<T>(this IDictContext context, string mannerKey, Action<T> callback)
                                                    where T : Manner {
            context.ForEach((IContext element) => {
                T manner;
                if (element.Manners.TryGet<T>(mannerKey, out manner)) {
                    callback(manner);
                }
                IDictContext dict = element as IDictContext;
                if (dict != null) {
                    ForEachDescendantsWithManner<T>(dict, mannerKey, callback);
                }
            });
        }
    }
}
