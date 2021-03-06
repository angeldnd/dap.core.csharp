using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Table<T> {
        public T1 Remove<T1>(int index) where T1 : class, IInTableElement {
            if (index >= 0 && index < _Elements.Count) {
                T element = _Elements[index];
                T1 _element = element.As<T1>();
                if (_element != null) {
                    _Elements.RemoveAt(index);
                    UpdateIndexes(index);

                    OnElementRemoved(element);
                    element._OnRemoved(this);

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
                    element._OnRemoved(this);
                }
            }
        }

        public List<T> RemoveByChecker(Func<T, bool> checker) {
            List<T> removed = null;
            IProfiler profiler = Log.BeginSample("RemoveByChecker: Check");
            foreach (T element in _Elements) {
                if (checker(element)) {
                    if (removed == null) {
                        removed = new List<T>();
                    }
                    removed.Add(element);
                }
            }
            if (profiler != null) profiler.EndSample();
            if (removed != null) {
                if (profiler != null) profiler.BeginSample("RemoveByChecker: Remove");
                for (int i = removed.Count - 1; i >= 0; i--) {
                    _Elements.RemoveAt(removed[i].Index);
                }
                if (profiler != null) profiler.EndSample();
            }
            if (profiler != null) profiler.BeginSample("RemoveByChecker: Notify");
            NotifyRemoves(removed, true);
            if (profiler != null) profiler.EndSample();
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
