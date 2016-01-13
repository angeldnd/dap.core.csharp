using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Tree<T> {
        public T1 New<T1>(string type, Pass pass, string path, Pass elementPass) where T1 : class, T {
            if (!CheckAdd(pass, path)) return null;

            object element = Factory.New<T1>(type, this, path, elementPass);

            return AddElement<T1>(element);
        }

        public T1 New<T1>(string type, Pass pass, string path) where T1 : class, T {
            return New<T1>(type, pass, path, null);
        }

        public T1 New<T1>(string type, string path, Pass elementPass) where T1 : class, T {
            return New<T1>(type, null, path, elementPass);
        }

        public T1 New<T1>(string type, string path) where T1 : class, T {
            return New<T1>(type, null, path, null);
        }

        public T New(string type, Pass pass, string path, Pass elementPass) {
            return New<T>(type, pass, path, elementPass);
        }

        public T New(string type, Pass pass, string path) {
            return New<T>(type, pass, path);
        }

        public T New(string type, string path, Pass elementPass) {
            return New<T>(type, path, elementPass);
        }

        public T New(string type, string path) {
            return New<T>(type, path);
        }
    }
}
