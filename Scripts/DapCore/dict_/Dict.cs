using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IInDictElement : IElement {
        IDict OwnerAsDict { get; }
        /*
         * Constructor(TO owner, string key)
         */
    }

    public interface IDict : IOwner {
        Type ElementType { get; }
        bool IsValidElementType(Type type);

        //Watcher
        bool AddDictWatcher<T1>(IDictWatcher<T1> watcher) where T1 : class, IInDictElement;
        bool RemoveDictWatcher<T1>(IDictWatcher<T1> watcher) where T1 : class, IInDictElement;

        //Partial IDict
        int Count { get; }
        ICollection<string> Keys { get; }

        //Has
        bool Has(string key);

        //Generic Add
        T1 Add<T1>(string key) where T1 : class, IInDictElement;

        //Generic New With Factory
        T1 New<T1>(string type, string key) where T1 : class, IInDictElement;

        //Generic Remove
        T1 Remove<T1>(string key) where T1 : class, IInDictElement;

        //Generic Get
        T1 Get<T1>(string key, bool logError) where T1 : class, IInDictElement;
        T1 Get<T1>(string key) where T1 : class, IInDictElement;
        T1 GetOrAdd<T1>(string key) where T1 : class, IInDictElement;
        T1 GetOrNew<T1>(string type, string key) where T1 : class, IInDictElement;

        //Is
        bool Is<T1>(string key) where T1 : class, IInDictElement;

        //Generic Filter
        void ForEach<T1>(PatternMatcher matcher, Action<T1> callback) where T1 : class, IInDictElement;
        void ForEach<T1>(Action<T1> callback) where T1 : class, IInDictElement;
        bool UntilTrue<T1>(PatternMatcher matcher, Func<T1, bool> callback) where T1 : class, IInDictElement;
        bool UntilFalse<T1>(PatternMatcher matcher, Func<T1, bool> callback) where T1 : class, IInDictElement;
        bool UntilTrue<T1>(Func<T1, bool> callback) where T1 : class, IInDictElement;
        bool UntilFalse<T1>(Func<T1, bool> callback) where T1 : class, IInDictElement;
        List<T1> Matched<T1>(PatternMatcher matcher) where T1 : class, IInDictElement;
        List<T1> All<T1>() where T1 : class, IInDictElement;
    }

    public interface IDict<T> : IDict
                                    where T : class, IInDictElement {
        //Watcher
        bool AddDictWatcher(IDictWatcher<T> watcher);
        bool RemoveDictWatcher(IDictWatcher<T> watcher);

        //Partial IDict<string, T>
        T this[string index] { get; }
        ICollection<T> Values { get; }
        IEnumerator<KeyValuePair<string, T>> GetEnumerator();

        //Add
        T Add(string key);

        //New With Factory
        T New(string type, string key);

        //Remove
        T Remove(string key);

        //Remove By Checker
        List<T> RemoveByChecker(Func<T, bool> checker);

        //Clear
        List<T> Clear();

        //Get
        T Get(string key, bool logError);
        T Get(string key);
        T GetOrAdd(string key);
        T GetOrNew(string type, string key);

        //Filter
        void ForEach(PatternMatcher matcher, Action<T> callback);
        void ForEach(Action<T> callback);
        bool UntilTrue(PatternMatcher matcher, Func<T, bool> callback);
        bool UntilFalse(PatternMatcher matcher, Func<T, bool> callback);
        bool UntilTrue(Func<T, bool> callback);
        bool UntilFalse(Func<T, bool> callback);
        List<T> Matched(PatternMatcher matcher);
        List<T> All();
    }

    public abstract partial class Dict<T> : Object, IDict<T>
                                                where T : class, IInDictElement {
        private readonly Type _ElementType;
        public Type ElementType {
            get { return _ElementType; }
        }

        public bool IsValidElementType(Type type) {
            return type != null && _ElementType.IsAssignableFrom(type);
        }

        private readonly Dictionary<string, T> _Elements = new Dictionary<string, T>();

        protected Dict() {
            _ElementType = typeof(T);
        }

        public bool Has(string key) {
            return _Elements.ContainsKey(key);
        }

        public bool Is<T1>(string key) where T1 : class, IInDictElement {
            T element = Get(key);
            return Object.Is<T1>(element);
        }

        private void OnElementAdded(T element) {
            if (element.LogDebug) element.Debug("Added");

            WeakListHelper.Notify(_Watchers, (IDictWatcher<T> watcher) => {
                watcher.OnElementAdded(element);
            });
            WeakListHelper.Notify(_GenericWatchers, (IDictWatcher<T> watcher) => {
                watcher.OnElementAdded(element);
            });
        }

        private void OnElementRemoved(T element) {
            if (element.LogDebug) element.Debug("Removed");

            WeakListHelper.Notify(_Watchers, (IDictWatcher<T> watcher) => {
                watcher.OnElementRemoved(element);
            });
            WeakListHelper.Notify(_GenericWatchers, (IDictWatcher<T> watcher) => {
                watcher.OnElementRemoved(element);
            });
        }


        protected virtual void OnElementsRemoved(List<T> elements) {
            for (int i = 0; i < elements.Count; i++) {
                OnElementRemoved(elements[i]);
            }
        }
    }
}
