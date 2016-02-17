using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Table<T> {
        private bool Until<T1>(Func<T1, bool> callback, bool breakCondition) where T1 : class, IInTableElement {
            bool result = !breakCondition;

            var en = _Elements.GetEnumerator();
            while (en.MoveNext()) {
                if (en.Current is T1) {
                    if (callback(en.Current as T1) == breakCondition) {
                        result = breakCondition;
                        break;
                    }
                }
            }
            return result;
        }

        public void ForEach<T1>(Action<T1> callback) where T1 : class, IInTableElement {
            UntilTrue<T1>((T1 element) => {
                callback(element);
                return false;
            });
        }

        public bool UntilTrue<T1>(Func<T1, bool> callback) where T1 : class, IInTableElement {
            return Until(callback, true);
        }

        public bool UntilFalse<T1>(Func<T1, bool> callback) where T1 : class, IInTableElement {
            return Until(callback, false);
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

        public bool UntilTrue(Func<T, bool> callback) {
            return Until<T>(callback, true);
        }

        public bool UntilFalse(Func<T, bool> callback) {
            return Until<T>(callback, false);
        }

        public List<T> All() {
            return All<T>();
        }
    }
}
