using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface ITree : IObject, IElement {
        char Separator { get; }

        void OnElementAdded();
        void OnElementRemoved();
    }

    public interface ITree<TO, T> : IElement<TO>, ITree
                                                where TO : ITree
                                                where T : IElement {
        //Partial IDict<string, T>
        int Count { get; }
        T this[string index] { get; }
        ICollection<string> Keys { get; }
        ICollection<T> Values { get; }
        IEnumerator<KeyValuePair<string, T>> GetEnumerator();
        bool Contains(KeyValuePair<string, T> kv);
        bool ContainsKey(string key);
        bool TryGetValue(string path, out T element);

        //Generic Add
        T1 Add<T1>(Pass pass, string path, Pass elementPass) where T1 : T;
        T1 Add<T1>(Pass pass, string path) where T1 : T;
        T1 Add<T1>(string path, Pass elementPass) where T1 : T;
        T1 Add<T1>(string path) where T1 : T;

        //Add
        T Add(Pass pass, string path, Pass elementPass);
        T Add(Pass pass, string path);
        T Add(string path, Pass elementPass);
        T Add(string path);

        //Add With Factory
        T Add(string type, Pass pass, string path, Pass elementPass);
        T Add(string type, Pass pass, string path);
        T Add(string type, string path, Pass elementPass);
        T Add(string type, string path);

        //Remove
        T Remove(Pass pass, string path, Pass elementPass);
        T Remove(Pass pass, string path);
        T Remove(string path, Pass elementPass);
        T Remove(string path);

        //Clear
        void Clear(Pass pass);
        void Clear();

        //Has and Get
        bool Has(string path);
        T1 Get<T1>(string path);
        T Get(string path);

        //Filter and All
        void Filter(string pattern, Action<T> callback);
        void All(Action<T> callback);
        List<T> Filter(string pattern);
        List<T> All();

        //Remove By Checker
        List<T> RemoveByChecker(Pass pass, Func<T, bool> checker);
        List<T> RemoveByChecker(Func<T, bool> checker);
    }

    public static class TreeConsts {
        public const char Separator = '.';
    }

    public abstract class Tree<TO, T> : Element<TO>, ITree<TO, T>
                                                where TO : ITree
                                                where T  : IElement {
        private readonly Dictionary<string, T> _Elements = new Dictionary<string, T>();

        protected Tree(TO owner, string path, Pass pass) : base(owner, path, pass) {
        }

        public virtual char Separator {
            get { return TreeConsts.Separator; }
        }

#region Partial IDict<string, T>
        public int Count {
            get { return _Elements.Count; }
        }

        public T this[string index] {
            get {
                return _Elements[index];
            }
        }

        public ICollection<string> Keys {
            get {
                return _Elements.Keys;
            }
        }

        public ICollection<T> Values {
            get {
                return _Elements.Values;
            }
        }

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator() {
            return _Elements.GetEnumerator();
        }

        public bool Contains(KeyValuePair<string, T> kv) {
            return _Elements.Contains(kv);
        }

        public bool ContainsKey(string key) {
            return _Elements.ContainsKey(key);
        }

        public bool TryGetValue(string path, out T element) {
            return _Elements.TryGetValue(path, out element);
        }
#endregion

        private T CheckAdd(Pass pass, string path) {
            if (!CheckWritePass(pass)) return false;

            T oldElement = null;
            if (_Elements.TryGetValue(path, out oldElement)) {
                Error("Already Exist: {0}, {1} -> {2}", path, oldElement);
                return false;
            }
            return true;
        }

        private T1 As<T1>(object element) where T1 : T {
            if (element == null) return null;

            if (!element is T1) {
                Error("Type Mismatched: <{0}> -> {1}", typeof(T1).FullName, element.GetType().FullName);
                return null;
            }

            return (T1)element;
        }

        private T1 AddElement<T1>(string path, object _element) where T1 : T {
            T1 element = As<T1>(_element);
            if (element != null) {
                element.OnAdded();
                OnElementAdded(element);
                _Elements[path] = element;

                AdvanceRevision();
            }
            return element;
        }

        public T1 Add<T1>(Pass pass, string path, Pass elementPass) where T1 : T {
            if (!CheckAdd(pass, path)) return null;

            object element = null;
            try {
                element = Activator.CreateInstance(typeof(T), this, path, elementPass);
            } catch (Exception e) {
                Error("CreateInstance Failed: <{0}> {1} -> {2}", typeof(T).FullName, path, e);
            }

            return AddElement<T1>(element);
        }

        public T1 Add<T1>(Pass pass, string path) where T1 : T {
            return Add<T1>(pass, path, null);
        }

        public T1 Add<T1>(string path, Pass elementPass) where T1 : T {
            return Add<T1>(null, path, elementPass);
        }

        public T1 Add<T1>(string path) where T1 : T {
            return Add<T1>(null, path, null);
        }

        public T Add(Pass pass, string path, Pass elementPass) {
            return Add<T>(pass, path, elementPass);
        }

        public T Add(Pass pass, string path) {
            return Add<T>(pass, path, null);
        }

        public T Add(string path, Pass elementPass) {
            return Add<T>(null, path, elementPass);
        }

        public T Add(string path) {
            return Add<T>(path);
        }

        public T Add(string type, Pass pass, string path, Pass elementPass) {
            if (!CheckAdd(pass, path)) return null;

            object element = null;
            try {
                element = Factory.New<T>(type, this, path, elementPass);
            } catch (Exception e) {
                Error("FactoryInstance Failed: <{0}> {1} -> {2}", typeof(T).FullName, path, e);
            }

            return AddElement<T>(element);
        }

        public T Add(string type, string path, Pass elementPass) {
            return Add(type, null, path, elementPass);
        }

        public T Add(string type, Pass pass, string path) {
            return Add(type, pass, path, null);
        }

        public T Add(string type, string path) {
            return Add(type, null, path, null);
        }

        public T Remove(Pass pass, string path, Pass elementPass) {
            if (!CheckWritePass(pass)) return false;

            T element;
            if (_Elements.TryGetValue(element.Path, out element)) {
                if (CheckAdminPass(pass, false) || element.CheckAdminPass(elementPass)) {
                    _Elements.Remove(path);
                    OnElementRemoved(element);
                    element.OnRemoved();

                    AdvanceRevision();
                    return element;
                }
            } else {
                Error("Not Exist: {0}", path);
            }
            return null;
        }

        public T Remove(Pass pass, string path) {
            return Remove(pass, path, null);
        }

        public T Remove(string path, Pass elementPass) {
            return Remove(null, path, elementPass);
        }

        public T Remove(string path) {
            return Remove<T>(null, path, null);
        }

        public void Clear(Pass pass) {
            if (!CheckAdminPass(pass)) return null;

            _Elements.Clear();

            AdvanceRevision();
        }

        public void Clear() {
            Clear(null);
        }

        public bool Has(string path) {
            return ContainsKey(key);
        }

        public T1 Get<T1>(string path) where T1 : T {
            T element = null;
            if (_Elements.TryGetValue(path, out element)) {
                return As<T1>(element);
            } else {
                Debug("Get<{0}>({1}): Not Found", typeof(T1).FullName, path);
            }
            return null;
        }

        public T Get(string path) {
            return Get<T>(path);
        }

        public void Filter(string pattern, Action<T> callback) {
            var matcher = new PatternMatcher(Separator, pattern);
            var en = _Elements.GetEnumerator();
            while (en.MoveNext()) {
                /*
                if (LogDebug) {
                    Debug("Check: {0}, {1} -> {2}, {3} -> {4}, {5}",
                        pattern, typeof(T),
                        pair.Key, pair.Value.GetType(),
                        (pair.Value is T), matcher.IsMatched(pair.Key));
                }
                */
                if (en.Current.Value is T && matcher.IsMatched(en.Current.Key)) {
                    callback((T)en.Current.Value);
                }
            }
        }

        public void All(Action<T> callback) {
            Filter(PatternMatcherConsts.WildcastSegments, callback);
        }

        public List<T> Filter(string pattern) {
            List<T> result = null;
            Filter<T>(pattern, (T aspect) => {
                if (result == null) result = new List<T>();
                result.Add(aspect);
            });
            return result;
        }

        public List<T> All() {
            return Filter(PatternMatcherConsts.WildcastSegments);
        }

        public List<T> RemoveByChecker(Pass pass, Func<T, bool> checker) {
            if (!CheckAdminPass(pass)) return null;

            List<T> removed = null;
            List<T> matched = All<T>();
            if (matched != null) {
                foreach (T aspect in matched) {
                    if (checker(aspect)) {
                        T _aspect = Remove(aspect.Path, pass);
                        if (_aspect != null) {
                            if (removed == null) {
                                removed = new List<T>();
                            }
                            removed.Add(_aspect);
                        }
                    }
                }
            }
            return removed;
        }

        public List<T> RemoveByChecker(Func<T, bool> checker) {
            return RemoveByChecker(null, checker);
        }

        protected virtual void OnElementAdded(T element) {}
        protected virtual void OnElementRemoved(T element) {}
    }
}
