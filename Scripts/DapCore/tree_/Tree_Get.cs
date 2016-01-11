using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Tree<TO, T> : Element<TO>, ITree<TO, T>
                                                where TO : IOwner
                                                where T : class, IElement {
        public bool Has(string path) {
            return _Elements.ContainsKey(path);
        }

        public T1 Get<T1>(string path) where T1 : class, T {
            T element = null;
            if (_Elements.TryGetValue(path, out element)) {
                return As<T1>(element);
            } else {
                Debug("Get<{0}>({1}): Not Found", typeof(T1).FullName, path);
            }
            return null;
        }

        public T Get(string path) {
            return Get<T>(path);
        }

        public T1 GetOrAdd<T1>(string path) where T1 : class, T {
            T element = null;
            if (_Elements.TryGetValue(path, out element)) {
                return As<T1>(element);
            } else {
                return Add<T1>(path);
            }
        }

        public T GetOrAdd(string path) {
            return GetOrAdd<T>(path);
        }
    }
}
