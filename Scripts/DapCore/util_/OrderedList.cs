using System;
using System.Collections;
using System.Collections.Generic;

using angeldnd.dap;

namespace angeldnd.dap {
    public abstract class OrderedList<T, TOrder> {
        private List<T> _Values = new List<T>();
        private Dictionary<T, TOrder> _Orders = new Dictionary<T, TOrder>();

        public int Add(T v, TOrder order) {
            if (Contains(v)) return -1;
            _Orders[v] = order;

            for (int i = 0; i < _Values.Count; i++) {
                int compareResult = Compare(order, _Orders[_Values[i]]);
                if (compareResult < 0) {
                    _Values.Insert(i, v);
                    return i;
                }
            }
            _Values.Add(v);
            return _Values.Count - 1;
        }

        public int Count {
            get { return _Values.Count; }
        }

        public T this[int index] {
            get {
                return _Values[index];
            }
        }

        public bool Contains(T v) {
            return _Orders.ContainsKey(v);
        }

        public IEnumerator<T> GetEnumerator() {
            return _Values.GetEnumerator();
        }

        public void Clear() {
            _Values.Clear();
            _Orders.Clear();
        }

        protected abstract int Compare(TOrder a, TOrder b);
    }

    public class IntOrderedList<T> : OrderedList<T, int> {
        protected override int Compare(int a, int b) {
            return a.CompareTo(b);
        }
    }

    public class LongOrderedList<T> : OrderedList<T, long> {
        protected override int Compare(long a, long b) {
            return a.CompareTo(b);
        }
    }

    public class FloatOrderedList<T> : OrderedList<T, float> {
        protected override int Compare(float a, float b) {
            return a.CompareTo(b);
        }
    }

    public class DoubleOrderedList<T> : OrderedList<T, double> {
        protected override int Compare(double a, double b) {
            return a.CompareTo(b);
        }
    }
}
