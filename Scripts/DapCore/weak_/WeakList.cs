using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    /*
     * There are some special logic for WeakBlock, since don't want to force all items
     * to implement OnAdded() and OnRemoved().
     */
    public sealed class WeakList<T> : IList<T> where T : class {
        private readonly List<WeakReference> _Elements = new List<WeakReference>();

        #region IList<T>
        public int Count {
            get {
                return _Elements.Count;
            }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public T this[int index] {
            get {
                throw new System.NotSupportedException("WeakList<T>.indexer:get");
            }
            set {
                throw new System.NotSupportedException("WeakList<T>.indexer:set");
            }
        }

        public IEnumerator<T> GetEnumerator() {
            throw new System.NotSupportedException("WeakList<T>.GetEnumerator<T>");
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            throw new System.NotSupportedException("WeakList<T>.GetEnumerator");
        }

        public void Add(T element) {
            AddElement(element);
        }

        public void Clear() {
            _Elements.Clear();
        }

        public bool Contains(T element) {
            return IndexOf(element) >= 0;
        }

        public void CopyTo(T[] array, int arrayIndex) {
            throw new System.NotSupportedException("WeakList<T>.CopyTo()");
        }

        public int IndexOf(T element) {
            for (int i = 0; i < _Elements.Count; i++) {
                if (_Elements[i].IsAlive && _Elements[i].Target == element) {
                    return i;
                }
            }
            return -1;
        }

        public void Insert(int index, T element) {
            throw new System.NotSupportedException("WeakList<T>.Insert()");
        }

        public bool Remove(T element) {
            int index = IndexOf(element);
            if (index >= 0) {
                WeakBlock block = element as WeakBlock;
                if (block != null) {
                    block.OnRemoved();
                }
                _Elements.RemoveAt(index);
                return true;
            }
            return false;
        }

        public void RemoveAt(int index) {
            throw new System.NotSupportedException("WeakList<T>.RemoveAt()");
        }
        #endregion

        public bool AddElement(T element) {
            if (!Contains(element)) {
                _Elements.Add(new WeakReference(element));
                WeakBlock block = element as WeakBlock;
                if (block != null) {
                    block.OnAdded();
                }
                return true;
            }
            return false;
        }

        public void ForEach(Action<T> callback) {
            UntilTrue((T element) => {
                callback(element);
                return false;
            });
        }

        private int CollectAllGarbage() {
            int count = 0;
            while (CollectOneGarbage()) {
                count++;
            }
            return count;
        }

        private bool CollectOneGarbage() {
            int garbageIndex = -1;

            for (int i = 0; i < _Elements.Count; i++) {
                WeakReference element = _Elements[i];
                if (!element.IsAlive) {
                    garbageIndex = i;
                    break;
                }
            }

            if (garbageIndex >= 0) {
                _Elements.RemoveAt(garbageIndex);
                return true;
            }
            return false;
        }

        private bool Until(Func<T, bool> callback, bool breakCondition) {
            bool result = !breakCondition;

            //The trick here is to reclaim at most one garbage in each publish, so don't need
            //to maintain any List, for better performance and cleaness.
            int garbageIndex = -1;

            for (int i = 0; i < _Elements.Count; i++) {
                WeakReference element = _Elements[i];
                if (element.IsAlive) {
                    if (callback((T)element.Target) == breakCondition) {
                        result = breakCondition;
                        break;
                    }
                } else if (garbageIndex < 0) {
                    Log.Debug("Garbage Found: {0}", element.Target);
                    garbageIndex = i;
                }
            }

            if (garbageIndex >= 0) {
                _Elements.RemoveAt(garbageIndex);
            }

            return result;
        }

        public bool UntilTrue(Func<T, bool> callback) {
            return Until(callback, true);
        }

        public bool UntilFalse(Func<T, bool> callback) {
            return Until(callback, false);
        }
    }
}
