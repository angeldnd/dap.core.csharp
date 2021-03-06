using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Dict<T> {
        public int Count {
            get { return _Elements.Count; }
        }

        public T this[string index] {
            get {
                return _Elements[index];
            }
        }

        public ICollection<string> Keys {
            get {
                return _Elements.Keys;
            }
        }

        public ICollection<T> Values {
            get {
                return _Elements.Values;
            }
        }

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator() {
            return _Elements.GetEnumerator();
        }

        public bool Contains(string key) {
            return _Elements.ContainsKey(key);
        }
    }
}
