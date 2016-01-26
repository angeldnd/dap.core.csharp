using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Table<T> {
        private bool CheckAdd<T1>() where T1 : class, IInTableElement {
            Type t1 = typeof(T1);
            if (t1 != _ElementType && !IsValidElementType(t1)) {
                Log.Error("Type Mismatched: <{0}> -> {1}",
                            _ElementType.FullName, t1.FullName);
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
                    OnElementAdded(element);
                    _Elements.Add(element);

                    AdvanceRevision();
                }
            }
            return _element;
        }

        public T1 Add<T1>() where T1 : class, IInTableElement {
            if (!CheckAdd<T1>()) return null;

            object element = null;
            try {
                element = Activator.CreateInstance(typeof(T1), this, _Elements.Count);
            } catch (Exception e) {
                Error("CreateInstance Failed: <{0}> {1} -> {2}",
                            typeof(T1).FullName, _Elements.Count, e);
            }

            return AddElement<T1>(element);
        }

        public T Add() {
            return Add<T>();
        }
    }
}
