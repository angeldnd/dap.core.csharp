using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Dict<T> {
        public T1 New<T1>(string type, string key) where T1 : class, IInDictElement {
            if (!CheckAdd<T1>(key)) return null;

            object element = Factory.New<T1>(type, this, key);

            return AddElement<T1>(element);
        }

        public T New(string type, string key) {
            return New<T>(type, key);
        }
    }
}
