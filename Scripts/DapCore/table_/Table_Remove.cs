using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Table<T> {
        public T1 Remove<T1>(int index) where T1 : class, IInTableElement {
            if (index >= 0 && index < _Elements.Count) {
                T element = _Elements[index];
                T1 _element = As<T1>(element);
                if (_element != null) {
                    _Elements.RemoveAt(index);
                    UpdateIndexes(index);

                    OnElementRemoved(element);
                    element.OnRemoved();

                    return _element;
                }
            }
            return null;
        }

        public void Clear() {
            RemoveAll();
        }

        public T Remove(int index) {
            return Remove<T>(index);
        }

        private void NotifyRemoves(List<T> removed, bool updateIndexes) {
            if (removed != null) {
                if (updateIndexes) {
                    UpdateIndexes(removed[0].Index);
                } else {
                    AdvanceRevision();
                }
                OnElementsRemoved(removed);
                foreach (T element in removed) {
                    element.OnRemoved();
                }
            }
        }

        public List<T> RemoveByChecker(Func<T, bool> checker) {
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
            NotifyRemoves(removed, true);
            return removed;
        }

        public List<T> RemoveAll() {
            List<T> removed = All();
            _Elements.Clear();

            NotifyRemoves(removed, false);
            return removed;
        }
    }
}
