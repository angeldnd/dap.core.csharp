using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Dict<T> {
        private bool CheckAdd(Type type, string key) {
            if (string.IsNullOrEmpty(key)) {
                Error("Invalid Key: {0}", key);
                return false;
            }
            if (type != _ElementType && !IsValidElementType(type)) {
                Error("Type Mismatched: <{0}>, {1} -> {2}",
                            _ElementType.FullName, key, type.FullName);
                return false;
            }

            T oldElement = null;
            if (_Elements.TryGetValue(key, out oldElement)) {
                Error("Already Exist: {0}, {1}", key, oldElement);
                return false;
            }
            return true;
        }

        private T1 AddElement<T1>(T1 _element) where T1 : class, IInDictElement {
            if (_element != null) {
                T element = As<T>(_element);
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
            Type t1 = typeof(T1);
            if (t1._IsInterface()) {
                if (t1 == typeof(IInDictElement)) {
                    return Add(key) as T1;
                } else {
                    Error("Invalid Type: <{0}>, {1} -> {2}",
                                _ElementType.FullName, key, t1.FullName);
                    return null;
                }
            }

            if (!CheckAdd(t1, key)) return null;

            return AddElement<T1>(Factory.Create<T1>(this, key));
        }

        public T Add(string key) {
            if (!CheckAdd(_ElementType, key)) return null;

            return AddElement<T>(Factory.Create<T>(this, key));
        }
    }
}
