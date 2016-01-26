using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Dict<T> {
        public void ForEach<T1>(PatternMatcher matcher, Action<T1> callback) where T1 : class, IInDictElement {
            var en = _Elements.GetEnumerator();
            while (en.MoveNext()) {
                if (en.Current.Value is T1) {
                    bool matched = (matcher == null) ||matcher.IsMatched(en.Current.Key);
                    if (matched) {
                        callback(en.Current.Value as T1);
                    }
                }
            }
        }

        public void ForEach<T1>(Action<T1> callback) where T1 : class, IInDictElement {
            ForEach<T1>(null, callback);
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

        public List<T> Matched(PatternMatcher matcher) {
            return Matched<T>(matcher);
        }

        public List<T> All() {
            return All<T>();
        }
    }
}
