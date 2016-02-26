using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IInTableElement : IElement {
        ITable OwnerAsTable { get; }
        /*
         * Constructor(TO owner, int index)
         */
        int Index { get; }
        bool SetIndex(IOwner owner, int index);
    }

    public interface ITable : IOwner {
        Type ElementType { get; }
        bool IsValidElementType(Type type);

        //Watcher
        bool AddTableWatcher<T1>(ITableWatcher<T1> watcher) where T1 : class, IInTableElement;
        bool RemoveTableWatcher<T1>(ITableWatcher<T1> watcher) where T1 : class, IInTableElement;

        //Partial IList
        int Count { get; }

        //Generic Add
        T1 Add<T1>() where T1 : class, IInTableElement;

        //Generic New With factory
        T1 New<T1>(string type) where T1 : class, IInTableElement;

        //Generic Remove
        T1 Remove<T1>(int index) where T1 : class, IInTableElement;

        //Clear
        void Clear();

        //Generic Get
        T1 Get<T1>(int index, bool logError) where T1 : class, IInTableElement;
        T1 Get<T1>(int index) where T1 : class, IInTableElement;

        T1 GetByKey<T1>(string key, bool logError) where T1 : class, IInTableElement;
        T1 GetByKey<T1>(string key) where T1 : class, IInTableElement;

        //Is
        bool Is<T1>(int index) where T1 : class, IInTableElement;

        //Generic Filter
        void ForEach<T1>(Action<T1> callback) where T1 : class, IInTableElement;
        bool UntilTrue<T1>(Func<T1, bool> callback) where T1 : class, IInTableElement;
        bool UntilFalse<T1>(Func<T1, bool> callback) where T1 : class, IInTableElement;
        List<T1> All<T1>() where T1 : class, IInTableElement;

        //Move
        bool MoveToHead(int index);
        bool MoveToTail(int index);
        bool Swap(int indexA, int indexB);
        bool MoveBefore(int index, int anchorIndex);
        bool MoveAfter(int index, int anchorIndex);
    }

    public interface ITable<T> : ITable
                                    where T : class, IInTableElement {
        //Watcher
        bool AddTableWatcher(ITableWatcher<T> watcher);
        bool RemoveTableWatcher(ITableWatcher<T> watcher);

        //Partial IList<T>
        T this[int index] { get; }
        IEnumerator<T> GetEnumerator();

        bool Contains(T element);

        //Add
        T Add();

        //New With factory
        T New(string type);

        //Remove
        T Remove(int index);

        //Remove By Checker
        List<T> RemoveByChecker(Func<T, bool> checker);

        //Remove All
        List<T> RemoveAll();

        //Get
        T Get(int index, bool logError);
        T Get(int index);

        T GetByKey(string key, bool logError);
        T GetByKey(string key);

        //Filter
        void ForEach(Action<T> callback);
        bool UntilTrue(Func<T, bool> callback);
        bool UntilFalse(Func<T, bool> callback);
        List<T> All();

        //Move
        bool MoveToHead(T element);
        bool MoveToTail(T element);
        bool Swap(T elementA, T elementB);
        bool MoveBefore(T element, T anchor);
        bool MoveAfter(T element, T anchor);
    }

    public abstract partial class Table<T> : Object, ITable<T>
                                                where T : class, IInTableElement {
        private readonly Type _ElementType;
        public Type ElementType {
            get { return _ElementType; }
        }

        public bool IsValidElementType(Type type) {
            return type != null && _ElementType.IsAssignableFrom(type);
        }

        private readonly List<T> _Elements = new List<T>();

        protected Table() {
            _ElementType = typeof(T);
        }

        public bool Is<T1>(int index) where T1 : class, IInTableElement {
            T element = Get(index);
            return Object.Is<T1>(element);
        }

        private void UpdateIndexes(int startIndex) {
            UpdateIndexes(startIndex, _Elements.Count - 1);
        }

        private void UpdateIndexes(int startIndex, int endIndex) {
            if (startIndex <= endIndex) {
                for (int i = startIndex; i <= endIndex; i++) {
                    T element = _Elements[i];
                    element.SetIndex(this, i);
                    OnElementIndexChanged(element);
                }
                AdvanceRevision();
            }
        }

        private void OnElementIndexChanged(T element) {
            WeakListHelper.Notify(_Watchers, (ITableWatcher<T> watcher) => {
                watcher.OnElementIndexChanged(element);
            });
            WeakListHelper.Notify(_GenericWatchers, (ITableWatcher<T> watcher) => {
                watcher.OnElementIndexChanged(element);
            });
        }

        private void OnElementAdded(T element) {
            if (element.DebugMode) element.Debug("Added");

            WeakListHelper.Notify(_Watchers, (ITableWatcher<T> watcher) => {
                watcher.OnElementAdded(element);
            });
            WeakListHelper.Notify(_GenericWatchers, (ITableWatcher<T> watcher) => {
                watcher.OnElementAdded(element);
            });
        }

        private void OnElementRemoved(T element) {
            if (element.DebugMode) element.Debug("Removed");

            WeakListHelper.Notify(_Watchers, (ITableWatcher<T> watcher) => {
                watcher.OnElementRemoved(element);
            });
            WeakListHelper.Notify(_GenericWatchers, (ITableWatcher<T> watcher) => {
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
