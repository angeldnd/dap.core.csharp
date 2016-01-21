using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Table<T> {
        private bool CheckAdd(Pass pass) {
            if (!CheckAdminPass(pass)) return false;

            return true;
        }

        private T1 AddElement<T1>(object obj) where T1 : class, IInTableElement {
            if (obj == null) return null;

            T1 _element = As<T1>(obj);
            if (_element != null) {
                T element = As<T>(obj);
                if (element != null) {
                    element.OnAdded();
                    OnElementAdded(element);
                    _Elements.Add(element);

                    AdvanceRevision();
                }
            }
            return _element;
        }

        public T1 Add<T1>(Pass pass) where T1 : class, IInTableElement {
            if (!CheckAdd(pass)) return null;

            object element = null;
            try {
                element = Activator.CreateInstance(typeof(T1), this, _Elements.Count, Pass);
            } catch (Exception e) {
                Error("CreateInstance Failed: <{0}> {1} -> {2}",
                            typeof(T1).FullName, _Elements.Count, e);
            }

            return AddElement<T1>(element);
        }

        public T1 Add<T1>() where T1 : class, IInTableElement {
            return Add<T1>(null);
        }

        public T Add(Pass pass) {
            return Add<T>(pass);
        }

        public T Add() {
            return Add<T>();
        }
    }
}
