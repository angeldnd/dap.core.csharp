using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Tree<T> {
        public IInTreeElement GetElement(string path) {
            return Get(path);
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
            T element = null;
            if (_Elements.TryGetValue(path, out element)) {
                return element;
            } else {
                Debug("Get({0}): Not Found", path);
            }
            return null;
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
