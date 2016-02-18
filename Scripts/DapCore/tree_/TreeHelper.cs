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

        public static T GetDescendant<T>(IOwner owner, string relPath, bool logError)
                                            where T : class, IElement {
            string[] keys = relPath.Split(PathConsts.PathSeparator);
            IObject current = owner;
            for (int i = 0; i < keys.Length; i++) {
                current = GetChild<IElement>(current as IOwner, keys[i], logError);
                if (current == null) {
                    return null;
                }
            }
            T result = current as T;
            if (result == null) {
                if (logError) {
                    owner.Error("Type Mismatched: <{0}> {1} -> {2}",
                                    typeof(T).FullName, relPath, current);
                }
            }
            return result;
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

        public static T AddDescendant<TO, T>(IDict owner, string relPath)
                                                where TO : class, IDict, IInDictElement
                                                where T : class, IInDictElement {
            string[] keys = relPath.Split(PathConsts.PathSeparator);
            IDict current = owner;
            for (int i = 0; i < keys.Length; i++) {
                if (i < keys.Length - 1) {
                    current = current.GetOrAdd<TO>(keys[i]);
                } else {
                    return current.GetOrAdd<T>(keys[i]);
                }
                if (current == null) {
                    return null;
                }
            }
            return null;
        }

        public static T NewDescendant<TO, T>(IDict owner, string type, string relPath)
                                                where TO : class, IDict, IInDictElement
                                                where T : class, IInDictElement {
            string[] keys = relPath.Split(PathConsts.PathSeparator);
            IDict current = owner;
            for (int i = 0; i < keys.Length; i++) {
                if (i < keys.Length - 1) {
                    current = current.GetOrAdd<TO>(keys[i]);
                } else {
                    return current.GetOrNew<T>(type, keys[i]);
                }
                if (current == null) {
                    return null;
                }
            }
            return null;
        }
    }
}
