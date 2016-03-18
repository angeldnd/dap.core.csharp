using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Table<T> {
        public IInTableElement GetElement(int index) {
            return Get(index);
        }

        public T1 Get<T1>(int index, bool isDebug = false) where T1 : class, IInTableElement {
            if (index >= 0 && index < _Elements.Count) {
                return As<T1>(_Elements[index], isDebug);
            } else {
                ErrorOrDebug(isDebug, "Get<{0}>({1}): Not Found", typeof(T1).FullName, index);
            }
            return null;
        }

        public T1 GetByKey<T1>(string key, bool isDebug = false) where T1 : class, IInTableElement {
            for (int i = 0; i < _Elements.Count; i++) {
                T element = _Elements[i];
                if (element.Key == key) {
                    return As<T1>(element, isDebug);
                }
            }
            ErrorOrDebug(isDebug, "Get({0}): Not Found", key);
            return null;
        }

        public T1 GetByKey<T1>(string key) where T1 : class, IInTableElement {
            return GetByKey<T1>(key, false);
        }

        public T Get(int index, bool isDebug = false) {
            return Get<T>(index, isDebug);
        }

        public T GetByKey(string key, bool isDebug = false) {
            return GetByKey<T>(key, isDebug);
        }
    }
}
