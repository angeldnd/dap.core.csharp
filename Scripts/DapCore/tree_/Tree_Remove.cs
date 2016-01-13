using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Tree<T> {
        public T1 Remove<T1>(Pass pass, string path, Pass elementPass) where T1 : class, T {
            if (!CheckWritePass(pass)) return null;

            T _element;
            if (_Elements.TryGetValue(path, out _element)) {
                if (CheckAdminPass(pass, false) || _element.CheckAdminPass(elementPass)) {
                    T1 element = As<T1>(_element);
                    if (element != null) {
                        _Elements.Remove(path);
                        OnElementRemoved(element);
                        element.OnRemoved();

                        AdvanceRevision();
                        return element;
                    }
                }
            } else {
                Error("Not Exist: {0}", path);
            }
            return null;
        }

        public T1 Remove<T1>(Pass pass, string path) where T1 : class, T {
            return Remove<T1>(pass, path, null);
        }

        public T1 Remove<T1>(string path, Pass elementPass) where T1 : class, T {
            return Remove<T1>(null, path, elementPass);
        }

        public T1 Remove<T1>(string path) where T1 : class, T {
            return Remove<T1>(null, path, null);
        }

        public T Remove(Pass pass, string path, Pass elementPass) {
            return Remove<T>(pass, path, elementPass);
        }

        public T Remove(Pass pass, string path) {
            return Remove(pass, path, null);
        }

        public T Remove(string path, Pass elementPass) {
            return Remove(null, path, elementPass);
        }

        public T Remove(string path) {
            return Remove<T>(null, path, null);
        }

        public void Clear(Pass pass) {
            if (!CheckAdminPass(pass)) return;

            _Elements.Clear();

            AdvanceRevision();
        }

        public void Clear() {
            Clear(null);
        }

        public List<T> RemoveByChecker(Pass pass, Func<T, bool> checker) {
            if (!CheckAdminPass(pass)) return null;

            List<T> removed = null;
            List<T> matched = All<T>();
            if (matched != null) {
                foreach (T aspect in matched) {
                    if (checker(aspect)) {
                        T _aspect = Remove(aspect.Path, pass);
                        if (_aspect != null) {
                            if (removed == null) {
                                removed = new List<T>();
                            }
                            removed.Add(_aspect);
                        }
                    }
                }
            }
            return removed;
        }

        public List<T> RemoveByChecker(Func<T, bool> checker) {
            return RemoveByChecker(null, checker);
        }
    }
}
