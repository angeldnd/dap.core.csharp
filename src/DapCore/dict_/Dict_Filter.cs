using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Dict<T> {
        private bool Until<T1>(PatternMatcher matcher, Func<T1, bool> callback, bool breakCondition) where T1 : class, IInDictElement {
            bool result = !breakCondition;

            var en = _Elements.GetEnumerator();
            while (en.MoveNext()) {
                if (en.Current.Value is T1) {
                    bool matched = (matcher == null) ||matcher.IsMatched(en.Current.Key);
                    if (matched) {
                        if (callback(en.Current.Value as T1) == breakCondition) {
                            result = breakCondition;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        public void ForEach<T1>(PatternMatcher matcher, Action<T1> callback) where T1 : class, IInDictElement {
            UntilTrue<T1>(matcher, (T1 element) => {
                callback(element);
                return false;
            });
        }

        public void ForEach<T1>(Action<T1> callback) where T1 : class, IInDictElement {
            ForEach<T1>(null, callback);
        }

        public bool UntilTrue<T1>(PatternMatcher matcher, Func<T1, bool> callback) where T1 : class, IInDictElement {
            return Until(matcher, callback, true);
        }

        public bool UntilFalse<T1>(PatternMatcher matcher, Func<T1, bool> callback) where T1 : class, IInDictElement {
            return Until(matcher, callback, false);
        }

        public bool UntilTrue<T1>(Func<T1, bool> callback) where T1 : class, IInDictElement {
            return Until(null, callback, true);
        }

        public bool UntilFalse<T1>(Func<T1, bool> callback) where T1 : class, IInDictElement {
            return Until(null, callback, false);
        }

        public List<T1> Matched<T1>(PatternMatcher matcher) where T1 : class, IInDictElement {
            List<T1> result = null;
            ForEach<T1>(matcher, (T1 element) => {
                if (result == null) result = new List<T1>();
                result.Add(element);
            });
            return result;
        }

        public List<T1> All<T1>() where T1 : class, IInDictElement {
            return Matched<T1>(null);
        }

        public void ForEach(PatternMatcher matcher, Action<T> callback) {
            ForEach<T>(matcher, callback);
        }

        public void ForEach(Action<T> callback) {
            ForEach<T>(callback);
        }

        public bool UntilTrue(PatternMatcher matcher, Func<T, bool> callback) {
            return Until<T>(matcher, callback, true);
        }

        public bool UntilFalse(PatternMatcher matcher, Func<T, bool> callback) {
            return Until<T>(matcher, callback, false);
        }

        public bool UntilTrue(Func<T, bool> callback) {
            return Until<T>(null, callback, true);
        }

        public bool UntilFalse(Func<T, bool> callback) {
            return Until<T>(null, callback, false);
        }

        public List<T> Matched(PatternMatcher matcher) {
            return Matched<T>(matcher);
        }

        public List<T> All() {
            return All<T>();
        }
    }
}
