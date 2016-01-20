using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IInTableElement : IElement {
        /*
         * Constructor(TO owner, int index, Pass pass)
         */
        int Index { get; }
        bool SetIndex(Pass pass, int index);
    }

    public interface IInTableElement<TO> : IElement<TO>, IInTableElement
                                            where TO : ITable {
    }

    public interface ITable : IOwner {
        //Partial IList
        int Count { get; }
    }

    /*
     * Note that all elements in Table are sharing same pass, which is only
     * used for index update, not controlling the remove permission.
     *
     * So the APIs won't have elementPass related params.
     */
    public interface ITable<T> : ITable
                                    where T : class, IInTableElement {
        //Partial IList<T>
        T this[int index] { get; }
        IEnumerator<T> GetEnumerator();

        bool Contains(T element);

        //Generic Add
        T1 Add<T1>(Pass pass) where T1 : class, T;
        T1 Add<T1>() where T1 : class, T;

        //Add
        T Add(Pass pass);
        T Add();

        //Generic New With factory
        T1 New<T1>(string type, Pass pass) where T1 : class, T;
        T1 New<T1>(string type) where T1 : class, T;

        //New With factory
        T New(string type, Pass pass);
        T New(string type);

        //Generic Remove
        T1 Remove<T1>(Pass pass, int index) where T1 : class, T;
        T1 Remove<T1>(int index) where T1 : class, T;

        //Remove
        T Remove(Pass pass, int index);
        T Remove(int index);

        //Remove By Checker
        List<T> RemoveByChecker(Pass pass, Func<T, bool> checker);
        List<T> RemoveByChecker(Func<T, bool> checker);

        //Clear
        List<T> Clear(Pass pass);
        List<T> Clear();

        //Get
        T1 Get<T1>(int index) where T1 : class, T;
        T Get(int index);

        //Is
        bool Is<T1>(int index) where T1 : class, T;

        //Generic Filter
        void All<T1>(Action<T1> callback) where T1 : class, T;
        List<T1> All<T1>() where T1 : class, T;

        //Filter
        void All(Action<T> callback);
        List<T> All();

        //Move
        bool MoveToHead(Pass pass, T element);
        bool MoveToHead(T element);
        bool MoveToTail(Pass pass, T element);
        bool MoveToTail(T element);
        bool Swap(Pass pass, T elementA, T elementB);
        bool Swap(T elementA, T elementB);
        bool MoveBefore(Pass pass, T element, T anchor);
        bool MoveBefore(T element, T anchor);
        bool MoveAfter(Pass pass, T element, T anchor);
        bool MoveAfter(T element, T anchor);
    }

    public abstract partial class Table<T> : Object, ITable<T>
                                                where T : class, IInTableElement {
        private readonly List<T> _Elements = new List<T>();

        protected Table(Pass pass) : base(pass) {
        }

        public bool Is<T1>(int index) where T1 : class, T {
            T element = Get(index);
            return Object.Is<T1>(element);
        }

        private void UpdateIndexes(int startIndex) {
            UpdateIndexes(startIndex, _Elements.Count - 1);
        }

        private void UpdateIndexes(int startIndex, int endIndex) {
            if (startIndex <= endIndex) {
                for (int i = startIndex; i <= endIndex; i++) {
                    _Elements[i].SetIndex(Pass, i);
                }
                AdvanceRevision();
            }
        }

        protected virtual void OnElementAdded(T element) {}
        protected virtual void OnElementRemoved(T element) {}

        protected virtual void OnElementsRemoved(List<T> elements) {
            for (int i = 0; i < elements.Count; i++) {
                OnElementRemoved(elements[i]);
            }
        }
    }
}
