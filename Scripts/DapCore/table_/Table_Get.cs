using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Table<T> {
        public T1 Get<T1>(int index) where T1 : class, T {
            if (index >= 0 && index < _Elements.Count) {
                return As<T1>(_Elements[index]);
            } else {
                Debug("Get<{0}>({1}): Not Found", typeof(T1).FullName, index);
            }
            return null;
        }

        public T Get(int index) {
            if (index >= 0 && index < _Elements.Count) {
                return _Elements[index];
            }
            return null;
        }
    }
}
