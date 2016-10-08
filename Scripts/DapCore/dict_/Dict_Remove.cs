using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Dict<T> {
        public T1 Remove<T1>(string key) where T1 : class, IInDictElement {
            T element;
            if (_Elements.TryGetValue(key, out element)) {
                T1 _element = As<T1>(element);
                if (_element != null) {
                    _Elements.Remove(key);
                    AdvanceRevision();

                    OnElementRemoved(element);
                    element._OnRemoved(this);

                    return _element;
                }
            } else {
                Error("Not Exist: {0}", key);
            }
            return null;
        }

        public void Clear() {
            RemoveAll();
        }

        public T Remove(string key) {
            return Remove<T>(key);
        }

        private void NotifyRemoves(List<T> removed) {
            if (removed != null) {
                AdvanceRevision();
                OnElementsRemoved(removed);
                foreach (T element in removed) {
                    element._OnRemoved(this);
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
                        _Elements.Remove(element.Key);
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

        public List<T> RemoveAll() {
            List<T> removed = All();
            _Elements.Clear();

            NotifyRemoves(removed);
            return removed;
        }
    }
}
