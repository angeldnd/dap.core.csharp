using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Table<T> {
        public void ForEach<T1>(Action<T1> callback) where T1 : class, IInTableElement {
            var en = _Elements.GetEnumerator();
            while (en.MoveNext()) {
                if (en.Current is T1) {
                    callback(en.Current as T1);
                }
            }
        }

        public List<T1> All<T1>() where T1 : class, IInTableElement {
            List<T1> result = null;
            ForEach<T1>((T1 element) => {
                if (result == null) result = new List<T1>();
                result.Add(element);
            });
            return result;
        }

        public void ForEach(Action<T> callback) {
            ForEach<T>(callback);
        }

        public List<T> All() {
            return All<T>();
        }
    }
}
