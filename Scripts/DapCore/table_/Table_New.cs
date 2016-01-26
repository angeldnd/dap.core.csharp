using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Table<T> {
        public T1 New<T1>(string type) where T1 : class, IInTableElement {
            object element = Factory.New<T1>(type, this, _Elements.Count);

            return AddElement<T1>(element);
        }

        public T New(string type) {
            return New<T>(type);
        }
    }
}
