using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class TreeHelper {
        public static List<string> GetSegments<T>(IOwner root, T element) where T : class, IElement {
            List<string> segments = new List<string>();
            T current = element;
            while (current != null) {
                segments.Add(current.Key);
                IOwner owner = current.GetOwner();
                if (owner == root) {
                    break;
                }
                current = owner as T;
            }
            segments.Reverse();
            return segments;
        }

        public static string GetPath<T>(IOwner root, T element) where T : class, IElement {
            return PathConsts.Join(GetSegments<T>(root, element));
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

        public static T GetChild<T>(IOwner owner, string key, bool isDebug = false) where T : class, IElement {
            if (owner == null) return null;

            T child = null;
            IDict ownerAsDict = owner as IDict;
            if (ownerAsDict != null) {
                child = ownerAsDict.Get<IInDictElement>(key, isDebug) as T;
            } else {
                ITable ownerAsTable = owner as ITable;
                if (ownerAsTable != null) {
                    child = ownerAsTable.GetByKey<IInTableElement>(key, isDebug) as T;
                }
            }
            if (child == null) {
                owner.ErrorOrDebug(isDebug, "Not Found: {0}", key);
            }
            return child;
        }

        public static T GetDescendant<T>(IOwner owner, List<string> segments, int startIndex, bool isDebug = false)
                                            where T : class, IElement {
            IObject current = owner;
            for (int i = startIndex; i < segments.Count; i++) {
                current = GetChild<IElement>(current as IOwner, segments[i], isDebug);
                if (current == null) {
                    return null;
                }
            }
            T result = current as T;
            if (result == null) {
                owner.ErrorOrDebug(isDebug, "Type Mismatched: <{0}> {1} {2} -> {3}",
                                typeof(T).FullName,
                                PathConsts.Join(segments),
                                startIndex, current);
            }
            return result;
        }

        public static T GetDescendant<T>(IOwner owner, string relPath, bool isDebug = false)
                                            where T : class, IElement {
            List<string> segments = PathConsts.Split(relPath);
            return GetDescendant<T>(owner, segments, 0, isDebug);
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
            List<string> segments = PathConsts.Split(relPath);
            IDict current = owner;
            for (int i = 0; i < segments.Count; i++) {
                if (i < segments.Count - 1) {
                    if (current.Has(segments[i])) {
                        current = Object.As<IDict>(current.Get<IInDictElement>(segments[i]));
                    } else {
                        current = current.Add<TO>(segments[i]);
                    }
                } else {
                    return func(current, segments[i]);
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
