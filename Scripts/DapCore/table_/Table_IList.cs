using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Table<T> {
        public int Count {
            get { return _Elements.Count; }
        }

        public T this[int index] {
            get {
                return Get(index);
            }
        }

        public IEnumerator<T> GetEnumerator() {
            return _Elements.GetEnumerator();
        }

        public bool Contains(T element) {
            return _Elements.Contains(element);
        }
    }
}
