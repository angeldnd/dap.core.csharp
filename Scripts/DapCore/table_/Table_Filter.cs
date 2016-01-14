using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Table<T> {
        public void All<T1>(Action<T1> callback) where T1 : class, T {
            var en = _Elements.GetEnumerator();
            while (en.MoveNext()) {
                if (en.Current is T1) {
                    callback((T1)en.Current);
                }
            }
        }

        public List<T1> All<T1>() where T1 : class, T {
            List<T1> result = null;
            All<T1>((T1 element) => {
                if (result == null) result = new List<T1>();
                result.Add(element);
            });
            return result;
        }

        public void All(Action<T> callback) {
            All<T>(callback);
        }

        public List<T> All() {
            return All<T>();
        }
    }
}
