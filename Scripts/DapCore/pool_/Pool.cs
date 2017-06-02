using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public class Pool<T> {
        private Queue<T> _Items = null;

        public int Count {
            get { return _Items.Count; }
        }

        public Pool() {
            _Items = new Queue<T>();
        }

        public Pool(int capacity) {
            _Items = new Queue<T>(capacity);
            EnsureCapacity(capacity);
        }

        public void Clear() {
            _Items.Clear();
        }

        public void Pack() {
            _Items.TrimExcess();
        }

        public void EnsureCapacity(int capacity) {
            while (_Items.Count < capacity) {
                _Items.Enqueue(NewItem());
            }
        }

        public T Take(bool createNew = false) {
            while (_Items.Count > 0) {
                T item = _Items.Dequeue();
                if (CheckTake(item)) {
                    return item;
                }
            }
            return createNew ? NewItem() : default(T);
        }

        public void Add(T item) {
            if (CheckAdd(item)) {
                _Items.Enqueue(item);
            }
        }

        //Dequeue without calling CheckTake(), mainly for
        //subclass to do cleanup
        protected T Dequeue() {
            if (_Items.Count <= 0) {
                return default(T);
            }
            return _Items.Dequeue();
        }

        protected virtual T NewItem() {
            throw new NotImplementedException(string.Format("{0}.NewItem()", GetType().FullName));
        }

        //return true means can be used
        protected virtual bool CheckTake(T item) {
            return true;
        }

        //return true means can be added into pool
        protected virtual bool CheckAdd(T item) {
            return true;
        }
    }
}
