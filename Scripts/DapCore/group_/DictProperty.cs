using System;
using System.Collections.Generic;
using System.Linq;

namespace angeldnd.dap {
    public abstract class DictProperty<T> : GroupProperty, IDictionary<string, T> where T : class, Property {
        private Dictionary<string, T> _Elements = new Dictionary<string, T>();

        private bool _MuteOnChanged = false;

        #region IDictionary<string, T>
        public int Count {
            get { return _Elements.Count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public T this[string index] {
            get {
                return _Elements[index];
            }
            set {
                throw new System.NotSupportedException("DictProperty<T>.indexer:set");
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

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<string, T> kv) {
            throw new System.NotSupportedException("DictProperty<T>.Add(KeyValuePair<string, T>), use Add(string) or Add(string, Pass)");
        }

        public void Add(string key, T element) {
            throw new System.NotSupportedException("DictProperty<T>.Add(T), use Add(string) or Add(string, Pass)");
        }

        public void Clear() {
            Clear(null);
        }

        public bool Contains(KeyValuePair<string, T> kv) {
            return _Elements.Contains(kv);
        }

        public bool ContainsKey(string key) {
            return _Elements.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, T>[] array, int arrayIndex) {
            throw new System.NotSupportedException("DictProperty<T>.CopyTo()");
        }

        public bool Remove(KeyValuePair<string, T> kv) {
            throw new System.NotSupportedException("DictProperty<T>.Remove(KeyValuePair<string, T>)");
        }

        public bool Remove(string path) {
            return Remove<T>(path, null) != null;
        }

        public bool TryGetValue(string path, out T element) {
            return _Elements.TryGetValue(path, out element);
        }

        #endregion

        #region APIs
        public T Add(string path) {
            return Add<T>(path, null);
        }

        public T Add(string path, Pass pass) {
            return Add<T>(path, pass);
        }

        public T Remove(T element, Pass pass) {
            return Remove<T>(element.Path, pass);
        }

        public void Clear(Pass pass) {
            _MuteOnChanged = true;
            All<T>((T element) => {
                Remove(element, pass);
            });
            FireOnChanged();
            _MuteOnChanged = false;
        }

        public override void OnAspectAdded(Entity entity, Aspect aspect) {
            base.OnAspectAdded(entity, aspect);
            if (entity == this) {
                if (aspect is T) {
                    T element = (T)aspect;
                    _Elements[element.Path] = element;
                    if (!_MuteOnChanged) FireOnChanged();
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
                    _Elements.Remove(aspect.Path);
                    if (!_MuteOnChanged) FireOnChanged();
                } else {
                    aspect.Error("Type Mismatched: <{0}>", typeof(T).FullName);
                }
            }
        }

        protected override bool DoEncode(Data data) {
            foreach (T element in _Elements.Values) {
                Data subData = element.Encode();
                if (subData != null) {
                    if (!data.SetData(element.Path, subData)) {
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
            foreach (var key in data.Keys) {
                Data subData = data.GetData(key);
                if (subData == null) {
                    Log.Error("Invalid Elements Data: {0} -> {1}", key, data.GetValue(key));
                    return false;
                }
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
