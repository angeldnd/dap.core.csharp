using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Dict<T> {
        private bool CheckAdd(Type type, string key) {
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

        public object CreateElement(Type type, string key) {
            object element = null;
            try {
                element = Activator.CreateInstance(type, this, key);
            } catch (Exception e) {
                Error("CreateInstance Failed: <{0}> {1} -> {2}", type.FullName, key, e);
            }

            return element;
        }

        public T1 Add<T1>(string key) where T1 : class, IInDictElement {
            Type t1 = typeof(T1);
            if (t1.IsInterface) {
                if (t1 == typeof(IInDictElement)) {
                    return Add(key) as T1;
                } else {
                    Error("Invalid Type: <{0}>, {1} -> {2}",
                                _ElementType.FullName, key, t1.FullName);
                    return null;
                }
            }

            if (!CheckAdd(t1, key)) return null;

            object element = CreateElement(t1, key);
            return AddElement<T1>(element);
        }

        public T Add(string key) {
            if (!CheckAdd(_ElementType, key)) return null;

            object element = CreateElement(_ElementType, key);
            return AddElement<T>(element);
        }
    }
}
