using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Tree<T> {
        public T1 Remove<T1>(Pass pass, string path, Pass elementPass) where T1 : class, IInTreeElement {
            if (!CheckWritePass(pass)) return null;

            T element;
            if (_Elements.TryGetValue(path, out element)) {
                if (CheckAdminPass(pass, false) || element.CheckAdminPass(elementPass)) {
                    T1 _element = As<T1>(element);
                    if (_element != null) {
                        _Elements.Remove(path);
                        AdvanceRevision();

                        OnElementRemoved(element);
                        element.OnRemoved();

                        return _element;
                    }
                }
            } else {
                Error("Not Exist: {0}", path);
            }
            return null;
        }

        public T1 Remove<T1>(Pass pass, string path) where T1 : class, IInTreeElement {
            return Remove<T1>(pass, path, null);
        }

        public T1 Remove<T1>(string path, Pass elementPass) where T1 : class, IInTreeElement {
            return Remove<T1>(null, path, elementPass);
        }

        public T1 Remove<T1>(string path) where T1 : class, IInTreeElement {
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

        private void NotifyRemoves(List<T> removed) {
            if (removed != null) {
                AdvanceRevision();
                foreach (T element in removed) {
                    element.OnRemoved();
                }
                OnElementsRemoved(removed);
            }
        }

        public List<T> Clear(Pass pass) {
            if (!CheckAdminPass(pass)) return null;

            List<T> removed = All();
            _Elements.Clear();

            NotifyRemoves(removed);
            return removed;
        }

        public List<T> Clear() {
            return Clear(null);
        }

        public List<T> RemoveByChecker(Pass pass, Func<T, bool> checker) {
            if (!CheckAdminPass(pass)) return null;

            List<T> removed = null;
            /* Must use the list version since need to remove some of them */
            List<T> all = All();
            if (all != null) {
                foreach (T element in all) {
                    if (checker(element)) {
                        _Elements.Remove(element.Path);
                        if (removed == null) {
                            removed = new List<T>();
                        }
                        removed.Add(element);
                    }
                };
            }

            NotifyRemoves(removed);
            return removed;
        }

        public List<T> RemoveByChecker(Func<T, bool> checker) {
            return RemoveByChecker(null, checker);
        }
    }
}
