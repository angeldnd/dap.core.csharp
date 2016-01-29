using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Dict<T> {
        private bool CheckAdd<T1>(string key) where T1 : class, IInDictElement {
            Type t1 = typeof(T1);
            if (t1 != _ElementType && !IsValidElementType(t1)) {
                Error("Type Mismatched: <{0}>, {1} -> {2}",
                            _ElementType.FullName, key, t1.FullName);
                return false;
            }

            T oldElement = null;
            if (_Elements.TryGetValue(key, out oldElement)) {
                Error("Already Exist: {0}, {1}", key, oldElement);
                return false;
            }
            return true;
        }

        private T1 AddElement<T1>(object obj) where T1 : class, IInDictElement {
            if (obj == null) return null;

            T1 _element = As<T1>(obj);
            if (_element != null) {
                T element = As<T>(obj);
                if (element != null) {
                    _Elements[element.Key] = element;

                    AdvanceRevision();
                    OnElementAdded(element);
                    element.OnAdded();
                }
            }
            return _element;
        }

        public T1 Add<T1>(string key) where T1 : class, IInDictElement {
            if (!CheckAdd<T1>(key)) return null;

            object element = null;
            try {
                element = Activator.CreateInstance(typeof(T1), this, key);
            } catch (Exception e) {
                Error("CreateInstance Failed: <{0}> {1} -> {2}", typeof(T1).FullName, key, e);
            }

            return AddElement<T1>(element);
        }

        public T Add(string key) {
            return Add<T>(key);
        }
    }
}
