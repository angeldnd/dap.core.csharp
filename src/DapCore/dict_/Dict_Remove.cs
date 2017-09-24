using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Dict<T> {
        public T1 Remove<T1>(string key, bool isDebug = false) where T1 : class, IInDictElement {
            T element;
            if (_Elements.TryGetValue(key, out element)) {
                T1 _element = element.As<T1>();
                if (_element != null) {
                    _Elements.Remove(key);
                    AdvanceRevision();

                    OnElementRemoved(element);
                    element._OnRemoved(this);

                    return _element;
                }
            } else {
                ErrorOrDebug(isDebug, "Not Exist: {0}", key);
            }
            return null;
        }

        public void Clear() {
            RemoveAll();
        }

        public T Remove(string key, bool isDebug = false) {
            return Remove<T>(key, isDebug);
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
            var en = _Elements.GetEnumerator();
            while (en.MoveNext()) {
                T element = en.Current.Value;
                if (checker(element)) {
                    if (removed == null) {
                        removed = new List<T>();
                    }
                    removed.Add(element);
                }
            }
            if (removed != null) {
                foreach (T element  in removed) {
                    _Elements.Remove(element.Key);
                }
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
