using System;
using System.Collections.Generic;
using System.Linq;

namespace angeldnd.dap {
    public struct ListPropertyConsts {
        public const string KeyIndex = "_i";
    }

    public abstract class ListProperty<T> : GroupProperty, IList<T> where T : class, Property {
        private List<T> _Elements = new List<T>();

        #region IList<T>
        public int Count {
            get { return _Elements.Count; }
        }

        public bool IsReadOnly {
            get { return !Sealed; }
        }

        public T this[int index] {
            get {
                if (index < 0 || index >= _Elements.Count) return null;
                return _Elements[index];
            }
            set {
                throw new System.NotSupportedException("ListProperty<T>.indexer:set");
            }
        }

        public IEnumerator<T> GetEnumerator() {
            return _Elements.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void Add(T element) {
            throw new System.NotSupportedException("ListProperty<T>.Add(T), use Add() or Add(Pass)");
        }

        public void Clear() {
            if (Sealed) {
                Error("Clear Failed: Sealed");
                return;
            }
            Clear(null);
        }

        public bool Contains(T element) {
            return _Elements.Contains(element);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            throw new System.NotSupportedException("ListProperty<T>.CopyTo()");
        }

        public int IndexOf(T element) {
            return _Elements.IndexOf(element);
        }

        public void Insert(int index, T element) {
            throw new System.NotSupportedException("ListProperty<T>.InsertAt()");
        }

        public bool Remove(T element) {
            if (Sealed) return false;

            return Remove(element, null) != null;
        }

        public void RemoveAt(int index) {
            if (index < 0 || index >= _Elements.Count) return;

            Remove(_Elements[index]);
        }

        #endregion

        #region APIs
        public T Add() {
            string path = Guid.NewGuid().ToString();
            return Add<T>(path, null);
        }

        public T Add(Pass pass) {
            string path = Guid.NewGuid().ToString();
            return Add<T>(path, pass);
        }

        public T Remove(T element, Pass pass) {
            return Remove<T>(element.Path, pass);
        }

        public void Clear(Pass pass) {
            All<T>((T element) => {
                Remove(element, pass);
            });
        }

        public bool MoveToHead(T element) {
            int index = _Elements.IndexOf(element);
            if (index < 0) return false;
            if (index == 0) return true;

            _Elements.RemoveAt(index);
            _Elements.Insert(0, element);
            return true;
        }

        public bool MoveToTail(T element) {
            int index = _Elements.IndexOf(element);
            if (index < 0) return false;
            if (index == _Elements.Count - 1) return true;

            _Elements.RemoveAt(index);
            _Elements.Add(element);
            return true;
        }

        public bool Swap(T elementA, T elementB) {
            if (elementA == elementB) return true;
            int indexA = _Elements.IndexOf(elementA);
            if (indexA < 0) return false;
            int indexB = _Elements.IndexOf(elementB);
            if (indexB < 0) return false;

            T tmp = _Elements[indexA];
            _Elements[indexA] = _Elements[indexB];
            _Elements[indexB] = tmp;
            return true;
        }

        private bool MoveBy(T elementA, T elementB, int offset) {
            int indexA = _Elements.IndexOf(elementA);
            if (indexA < 0) return false;

            _Elements.RemoveAt(indexA);

            int indexB = _Elements.IndexOf(elementB);
            if (indexB < 0) {
                _Elements.Insert(indexA + offset, elementA);
                return false;
            }

            _Elements.Insert(indexB, elementA);
            return true;
        }

        public bool MoveBefore(T elementA, T elementB) {
            return MoveBy(elementA, elementB, 0);
        }

        public bool MoveAfter(T elementA, T elementB) {
            return MoveBy(elementA, elementB, 1);
        }

        public override void OnAspectAdded(Entity entity, Aspect aspect) {
            base.OnAspectAdded(entity, aspect);
            if (entity == this) {
                if (aspect is T) {
                    T element = (T)aspect;
                    _Elements.Add(element);
                } else {
                    aspect.Error("Type Mismatched: <{0}>", typeof(T).FullName);
                }
            }
        }
        #endregion

        public override void OnAspectRemoved(Entity entity, Aspect aspect) {
            base.OnAspectRemoved(entity, aspect);
            if (entity == this) {
                if (aspect is T) {
                    T element = (T)aspect;
                    _Elements.Remove(element);
                } else {
                    aspect.Error("Type Mismatched: <{0}>", typeof(T).FullName);
                }
            }
        }

        protected override bool DoEncode(Data data) {
            for (int i = 0; i < _Elements.Count; i++) {
                T element = _Elements[i];
                Data subData = element.Encode();
                if (subData != null) {
                    if (!subData.SetInt(ListPropertyConsts.KeyIndex, i) || !data.SetData(element.Path, subData)) {
                        return false;
                    }
                } else {
                    return false;
                }
            }
            return true;
        }

        protected override bool DoDecode(Pass pass, Data data) {
            RemoveByChecker<T>(Pass, (T element) => true);
            if (_Elements.Count > 0) {
                Error("Orghan Elements Found: {0}", _Elements.Count);
            }
            Dictionary<int, string> keyByIndex = new Dictionary<int, string>();
            Dictionary<int, Data> dataByIndex = new Dictionary<int, Data>();
            foreach (var key in data.Keys) {
                Data subData = data.GetData(key);
                if (subData == null) {
                    Log.Error("Invalid Elements Data: {0} -> {1}", key, data.GetValue(key));
                    return false;
                }
                int index = subData.GetInt(ListPropertyConsts.KeyIndex, -1);
                if (index < 0) {
                    Log.Error("Invalid Elements Index: {0} -> {1}", key, data.GetValue(ListPropertyConsts.KeyIndex));
                    return false;
                }
                if (keyByIndex.ContainsKey(index)) {
                    Log.Error("Duplicated Elements Index: {0} -> {1} -> {2}, {3}",
                                key, index, keyByIndex[index], dataByIndex[index]);
                    return false;
                }
                keyByIndex[index] = key;
                dataByIndex[index] = subData;
            }

            List<int> indexes = keyByIndex.Keys.ToList();
            indexes.Sort();

            foreach (int index in indexes) {
                string key = keyByIndex[index];
                Data subData = dataByIndex[index];
                Property prop = SpecHelper.AddWithSpec(this, key, Pass, false, subData);
                if (!(prop is T)) {
                    Log.Error("Type Mismatched: {0}: {1} -> {2}", key, typeof(T).Name, prop.GetType().FullName);
                    return false;
                }
            }
            return true;
        }
    }
}
