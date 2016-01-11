using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Tree<TO, T> : Element<TO>, ITree<TO, T>
                                                where TO : IOwner
                                                where T : class, IElement {
        private bool CheckAdd(Pass pass, string path) {
            if (!CheckWritePass(pass)) return false;

            T oldElement = null;
            if (_Elements.TryGetValue(path, out oldElement)) {
                Error("Already Exist: {0}, {1} -> {2}", path, oldElement);
                return false;
            }
            return true;
        }

        private T1 AddElement<T1>(object _element) where T1 : class, T {
            if (_element == null) return null;

            T1 element = As<T1>(_element);
            if (element != null) {
                element.OnAdded();
                OnElementAdded(element);
                _Elements[element.Path] = element;

                AdvanceRevision();
            }
            return element;
        }

        public T1 Add<T1>(Pass pass, string path, Pass elementPass) where T1 : class, T {
            if (!CheckAdd(pass, path)) return null;

            object element = null;
            try {
                element = Activator.CreateInstance(typeof(T), this, path, elementPass);
            } catch (Exception e) {
                Error("CreateInstance Failed: <{0}> {1} -> {2}", typeof(T).FullName, path, e);
            }

            return AddElement<T1>(element);
        }

        public T1 Add<T1>(Pass pass, string path) where T1 : class, T {
            return Add<T1>(pass, path, null);
        }

        public T1 Add<T1>(string path, Pass elementPass) where T1 : class, T {
            return Add<T1>(null, path, elementPass);
        }

        public T1 Add<T1>(string path) where T1 : class, T {
            return Add<T1>(null, path, null);
        }

        public T Add(Pass pass, string path, Pass elementPass) {
            return Add<T>(pass, path, elementPass);
        }

        public T Add(Pass pass, string path) {
            return Add<T>(pass, path);
        }

        public T Add(string path, Pass elementPass) {
            return Add<T>(path, elementPass);
        }

        public T Add(string path) {
            return Add<T>(path);
        }
    }
}
