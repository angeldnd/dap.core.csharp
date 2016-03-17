using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Table<T> {
        public IInTableElement GetElement(int index) {
            return Get(index);
        }

        public T1 Get<T1>(int index, bool logError) where T1 : class, IInTableElement {
            if (index >= 0 && index < _Elements.Count) {
                return As<T1>(_Elements[index], logError);
            } else {
                if (logError) {
                    Error("Get<{0}>({1}): Not Found", typeof(T1).FullName, index);
                } else if (LogDebug) {
                    Debug("Get<{0}>({1}): Not Found", typeof(T1).FullName, index);
                }
            }
            return null;
        }

        public T1 Get<T1>(int index) where T1 : class, IInTableElement {
            return Get<T1>(index, true);
        }

        public T1 GetByKey<T1>(string key, bool logError) where T1 : class, IInTableElement {
            for (int i = 0; i < _Elements.Count; i++) {
                T element = _Elements[i];
                if (element.Key == key) {
                    return As<T1>(element, logError);
                }
            }
            if (logError) {
                Error("Get({0}): Not Found", key);
            } else if (LogDebug) {
                Debug("Get({0}): Not Found", key);
            }
            return null;
        }

        public T1 GetByKey<T1>(string key) where T1 : class, IInTableElement {
            return GetByKey<T1>(key, true);
        }

        public T Get(int index, bool logError) {
            return Get<T>(index, logError);
        }

        public T Get(int index) {
            return Get(index, true);
        }

        public T GetByKey(string key, bool logError) {
            return GetByKey<T>(key, logError);
        }

        public T GetByKey(string key) {
            return GetByKey(key, true);
        }
    }
}
