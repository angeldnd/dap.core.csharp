using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Dict<T> {
        public IInDictElement GetElement(string key) {
            return Get(key);
        }

        public bool TryGet<T1>(string key, out T1 element) where T1 : class, IInDictElement {
            T _element = null;
            if (_Elements.TryGetValue(key, out _element)) {
                return TryAs<T1>(_element, out element);
            } else {
                Debug("TryGet<{0}>({1}): Not Found", typeof(T1).FullName, key);
            }
            element = null;
            return false;
        }

        public T1 Get<T1>(string key) where T1 : class, IInDictElement {
            T element = null;
            if (_Elements.TryGetValue(key, out element)) {
                return As<T1>(element);
            } else {
                Debug("Get<{0}>({1}): Not Found", typeof(T1).FullName, key);
            }
            return null;
        }

        public bool TryGet(string key, out T element) {
            return _Elements.TryGetValue(key, out element);
        }

        public T Get(string key) {
            T element = null;
            if (_Elements.TryGetValue(key, out element)) {
                return element;
            } else {
                Debug("Get({0}): Not Found", key);
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

        public T GetOrAdd(string key) {
            return GetOrAdd<T>(key);
        }
    }
}
