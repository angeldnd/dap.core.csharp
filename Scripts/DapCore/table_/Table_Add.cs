using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Table<T> {
        private bool CheckAdd(Type type) {
            if (type != _ElementType && !IsValidElementType(type)) {
                Error("Type Mismatched: <{0}> -> {1}",
                            _ElementType.FullName, type.FullName);
                return false;
            }

            return true;
        }

        private T1 AddElement<T1>(object obj) where T1 : class, IInTableElement {
            if (obj == null) return null;

            T1 _element = As<T1>(obj);
            if (_element != null) {
                T element = As<T>(obj);
                if (element != null) {
                    _Elements.Add(element);

                    AdvanceRevision();
                    OnElementAdded(element);
                    element.OnAdded();
                }
            }
            return _element;
        }

        public object CreateElement(Type type, int index) {
            object element = null;
            try {
                element = Activator.CreateInstance(type, this, index);
            } catch (Exception e) {
                Error("CreateInstance Failed: <{0}> {1} -> {2}", type.FullName, index, e);
            }

            return element;
        }

        public T1 Add<T1>() where T1 : class, IInTableElement {
            Type t1 = typeof(T1);
            if (t1.IsInterface) {
                if (t1 == typeof(IInTableElement)) {
                    return Add() as T1;
                } else {
                    Error("Invalid Type: <{0}>, -> {1}",
                                _ElementType.FullName, t1.FullName);
                    return null;
                }
            }

            if (!CheckAdd(t1)) return null;

            object element = CreateElement(t1, _Elements.Count);
            return AddElement<T1>(element);
        }

        public T Add() {
            if (!CheckAdd(_ElementType)) return null;

            object element = CreateElement(_ElementType, _Elements.Count);
            return AddElement<T>(element);
        }
    }
}
