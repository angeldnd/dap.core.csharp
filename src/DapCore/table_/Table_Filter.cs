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

        public T1 First<T1>(Func<T1, bool> callback, bool isDebug = false)
                                    where T1 : class, IInTableElement {
            T1 result = null;
            UntilTrue<T1>((T1 element) => {
                if (callback(element)) {
                    result = element;
                    return true;
                }
                return false;
            });
            if (result == null) {
                ErrorOrDebug(isDebug, "First<{0}>({1}): Not Found", typeof(T1).FullName, callback);
            }
            return result;
        }

        public T1 First<T1>(bool isDebug = false) where T1 : class, IInTableElement {
            return First<T1>((T1 element) => { return true; });
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

        public T First(Func<T, bool> callback, bool isDebug = false) {
            return First<T>(callback);
        }

        public List<T> All() {
            return All<T>();
        }
    }
}
