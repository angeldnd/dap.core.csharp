using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class TreeHelper {
        public static string[] GetKeys<T>(IOwner root, T element) where T : class, IElement {
            List<string> keys = new List<string>();
            T current = element;
            while (current != null) {
                keys.Add(current.Key);
                IOwner owner = current.GetOwner();
                if (owner == root) {
                    break;
                }
                current = owner as T;
            }
            keys.Reverse();
            return keys.ToArray();
        }

        public static string GetPath<T>(IOwner root, T element) where T : class, IElement {
            return string.Join(PathConsts.PathSeparatorAsString, GetKeys<T>(root, element));
        }

        public static T GetAncestor<T>(IElement element) where T : class, IOwner {
            IObject current = element.GetOwner();
            while (current != null) {
                T ancestor = current as T;
                if (ancestor != null) {
                    return ancestor;
                }
                IElement currentAsElement = current as IElement;
                if (currentAsElement == null) {
                    element.Error("Ancestor Not Found: <{0}>", typeof(T).FullName);
                    return null;
                }
                current = currentAsElement.GetOwner();
            }
            return null;
        }

        public static T GetChild<T>(IOwner owner, string key, bool logError) where T : class, IElement {
            if (owner == null) return null;

            T child = null;
            IDict ownerAsDict = owner as IDict;
            if (ownerAsDict != null) {
                child = ownerAsDict.Get<IInDictElement>(key, logError) as T;
            } else {
                ITable ownerAsTable = owner as ITable;
                if (ownerAsTable != null) {
                    child = ownerAsTable.GetByKey<IInTableElement>(key, logError) as T;
                }
            }
            if (child == null) {
                if (logError) {
                    owner.Error("Not Found: {0}", key);
                }
            }
            return child;
        }

        public static T GetDescendant<T>(IOwner owner, string[] keys, int startIndex, bool logError)
                                            where T : class, IElement {
            IObject current = owner;
            for (int i = startIndex; i < keys.Length; i++) {
                current = GetChild<IElement>(current as IOwner, keys[i], logError);
                if (current == null) {
                    return null;
                }
            }
            T result = current as T;
            if (result == null) {
                if (logError) {
                    owner.Error("Type Mismatched: <{0}> {1} {2} -> {3}",
                                    typeof(T).FullName,
                                    string.Join(PathConsts.PathSeparatorAsString, keys),
                                    startIndex, current);
                }
            }
            return result;
        }

        public static T GetDescendant<T>(IOwner owner, string relPath, bool logError)
                                            where T : class, IElement {
            string[] keys = relPath.Split(PathConsts.PathSeparator);
            return GetDescendant<T>(owner, keys, 0, logError);
        }

        public static void ForEachDescendants<T>(IDict owner, Action<T> callback)
                                                    where T : class, IElement {
            owner.ForEach((IInDictElement element) => {
                T elementAsT = element as T;
                if (elementAsT != null) {
                    callback(elementAsT);
                }

                IDict elementAsDict = element as IDict;
                if (elementAsDict != null) {
                    ForEachDescendants<T>(elementAsDict, callback);
                } else {
                    ITable elementAsTable = element as ITable;
                    if (elementAsTable != null) {
                        ForEachDescendants<T>(elementAsTable, callback);
                    }
                }
            });
        }

        public static void ForEachDescendants<T>(ITable owner, Action<T> callback)
                                                    where T : class, IElement {
            owner.ForEach((IInTableElement element) => {
                T elementAsT = element as T;
                if (elementAsT != null) {
                    callback(elementAsT);
                }

                IDict elementAsDict = element as IDict;
                if (elementAsDict != null) {
                    ForEachDescendants<T>(elementAsDict, callback);
                } else {
                    ITable elementAsTable = element as ITable;
                    if (elementAsTable != null) {
                        ForEachDescendants<T>(elementAsTable, callback);
                    }
                }
            });
        }

        public static List<T> GetDescendants<T>(IDict owner)
                                                    where T : class, IElement {
            List<T> result = null;
            ForEachDescendants<T>(owner, (T element) => {
                if (result == null) {
                    result = new List<T>();
                }
                result.Add(element);
            });
            return result;
        }

        public static List<T> GetDescendants<T>(ITable owner)
                                                    where T : class, IElement {
            List<T> result = null;
            ForEachDescendants<T>(owner, (T element) => {
                if (result == null) {
                    result = new List<T>();
                }
                result.Add(element);
            });
            return result;
        }

        private static T GetOrCreateDescendant<TO, T>(IDict owner, string relPath, Func<IDict, string, T> func)
                                                where TO : class, IDict, IInDictElement
                                                where T : class, IInDictElement {
            string[] keys = relPath.Split(PathConsts.PathSeparator);
            IDict current = owner;
            for (int i = 0; i < keys.Length; i++) {
                if (i < keys.Length - 1) {
                    if (current.Has(keys[i])) {
                        current = Object.As<IDict>(current.Get<IInDictElement>(keys[i]));
                    } else {
                        current = current.Add<TO>(keys[i]);
                    }
                } else {
                    return func(current, keys[i]);
                }
                if (current == null) {
                    return null;
                }
            }
            return null;
        }

        public static T AddDescendant<TO, T>(IDict owner, string relPath)
                                                where TO : class, IDict, IInDictElement
                                                where T : class, IInDictElement {
            return GetOrCreateDescendant<TO, T>(owner, relPath,
                    (IDict parent, String key) => parent.Add<T>(key));
        }

        public static T GetOrAddDescendant<TO, T>(IDict owner, string relPath)
                                                where TO : class, IDict, IInDictElement
                                                where T : class, IInDictElement {
            return GetOrCreateDescendant<TO, T>(owner, relPath,
                    (IDict parent, String key) => parent.GetOrAdd<T>(key));
        }

        public static T NewDescendant<TO, T>(IDict owner, string type, string relPath)
                                                where TO : class, IDict, IInDictElement
                                                where T : class, IInDictElement {
            return GetOrCreateDescendant<TO, T>(owner, relPath,
                    (IDict parent, String key) => parent.New<T>(type, key));
        }

        public static T GetOrNewDescendant<TO, T>(IDict owner, string type, string relPath)
                                                where TO : class, IDict, IInDictElement
                                                where T : class, IInDictElement {
            return GetOrCreateDescendant<TO, T>(owner, relPath,
                    (IDict parent, String key) => parent.GetOrNew<T>(type, key));
        }
    }
}
