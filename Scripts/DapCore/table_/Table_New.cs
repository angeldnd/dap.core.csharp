using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Table<T> {
        public T1 New<T1>(string type, Pass pass) where T1 : class, IInTableElement {
            if (!CheckAdd(pass)) return null;

            object element = Factory.New<T1>(type, this, _Elements.Count, Pass);

            return AddElement<T1>(element);
        }

        public T1 New<T1>(string type) where T1 : class, IInTableElement {
            return New<T1>(null);
        }

        public T New(string type, Pass pass) {
            return New<T>(type, pass);
        }

        public T New(string type) {
            return New<T>(type);
        }
    }
}
