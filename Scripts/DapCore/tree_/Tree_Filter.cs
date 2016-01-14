using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Tree<T> {
        public void Filter<T1>(string pattern, Action<T1> callback) where T1 : class, T {
            var matcher = new PatternMatcher(Separator, pattern);
            var en = _Elements.GetEnumerator();
            while (en.MoveNext()) {
                if (en.Current.Value is T1 && matcher.IsMatched(en.Current.Key)) {
                    callback((T1)en.Current.Value);
                }
            }
        }

        public void All<T1>(Action<T1> callback) where T1 : class, T {
            Filter<T1>(PatternMatcherConsts.WildcastSegments, callback);
        }

        public List<T1> Filter<T1>(string pattern) where T1 : class, T {
            List<T1> result = null;
            Filter<T1>(pattern, (T1 element) => {
                if (result == null) result = new List<T1>();
                result.Add(element);
            });
            return result;
        }

        public List<T1> All<T1>() where T1 : class, T {
            return Filter<T1>(PatternMatcherConsts.WildcastSegments);
        }

        public void Filter(string pattern, Action<T> callback) {
            Filter<T>(pattern, callback);
        }

        public void All(Action<T> callback) {
            All<T>(callback);
        }

        public List<T> Filter(string pattern) {
            return Filter<T>(pattern);
        }

        public List<T> All() {
            return All<T>();
        }
    }
}
