using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Table<T> {
        public T1 Remove<T1>(Pass pass, int index) where T1 : class, T {
            if (!CheckAdminPass(pass)) return null;

            if (index >= 0 && index < _Elements.Count) {
                T1 element = As<T1>(_Elements[index]);
                if (element != null) {
                    _Elements.RemoveAt(index);
                    UpdateIndexes(index);

                    OnElementRemoved(element);
                    element.OnRemoved();

                    return element;
                }
            }
            return null;
        }

        public T1 Remove<T1>(int index) where T1 : class, T {
            return Remove<T1>(null, index);
        }

        public T Remove(Pass pass, int index) {
            return Remove<T>(pass, index);
        }

        public T Remove(int index) {
            return Remove<T>(index);
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
            /* Must use the list version since need to remove some of them */
            List<T> all = All();
            if (all != null) {
                foreach (T element in all) {
                    if (checker(element)) {
                        _Elements.Remove(element);
                        if (removed == null) {
                            removed = new List<T>();
                        }
                        removed.Add(element);
                    }
                };
            }
            if (removed != null) {
                UpdateIndexes(removed[0].Index);
                foreach (T element in removed) {
                    OnElementRemoved(element);
                    element.OnRemoved();
                }
            }
            return removed;
        }

        public List<T> RemoveByChecker(Func<T, bool> checker) {
            return RemoveByChecker(null, checker);
        }
    }
}
