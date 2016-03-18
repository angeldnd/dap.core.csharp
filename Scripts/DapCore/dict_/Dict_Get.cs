using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Dict<T> {
        public IInDictElement GetElement(string key) {
            return Get(key);
        }

        public T1 Get<T1>(string key, bool isDebug = false) where T1 : class, IInDictElement {
            T element = null;
            if (_Elements.TryGetValue(key, out element)) {
                return As<T1>(element, isDebug);
            } else {
                ErrorOrDebug(isDebug, "Get<{0}>({1}): Not Found", typeof(T1).FullName, key);
            }
            return null;
        }

        public T Get(string key, bool isDebug = false) {
            T element = null;
            if (_Elements.TryGetValue(key, out element)) {
                return element;
            } else {
                ErrorOrDebug(isDebug, "Get({0}): Not Found", key);
            }
            return null;
        }

        public T1 GetOrAdd<T1>(string key) where T1 : class, IInDictElement {
            T element = null;
            if (_Elements.TryGetValue(key, out element)) {
                return As<T1>(element);
            } else {
                return Add<T1>(key);
            }
        }

        public T1 GetOrNew<T1>(string type, string key) where T1 : class, IInDictElement {
            T element = null;
            if (_Elements.TryGetValue(key, out element)) {
                return As<T1>(element);
            } else {
                return New<T1>(type, key);
            }
        }

        public T GetOrAdd(string key) {
            return GetOrAdd<T>(key);
        }

        public T GetOrNew(string type, string key) {
            return GetOrNew<T>(type, key);
        }
    }
}
