using System;
using System.Collections.Generic;
using System.Linq;

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

        private T1 AddElement<T1>(T1 _element) where T1 : class, IInTableElement {
            if (_element != null) {
                T element = _element.As<T>();
                if (element != null) {
                    _Elements.Add(element);

                    AdvanceRevision();
                    OnElementAdded(element);
                    element._OnAdded(this);
                }
            }
            return _element;
        }

        public T1 Add<T1>() where T1 : class, IInTableElement {
            Type t1 = typeof(T1);
            if (t1._IsInterface()) {
                if (t1._IsAssignableFrom(_ElementType)) {
                    return Add() as T1;
                } else {
                    Error("Invalid Type: <{0}>, -> {1}",
                                _ElementType.FullName, t1.FullName);
                    return null;
                }
            }

            if (!CheckAdd(t1)) return null;

            return AddElement<T1>(Factory.Create<T1>(this, _Elements.Count));
        }

        public T Add() {
            if (!CheckAdd(_ElementType)) return null;

            return AddElement<T>(Factory.Create<T>(this, _Elements.Count));
        }
    }
}
