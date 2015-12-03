using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface ItemAccessor : Accessor {
        Item GetItem();
    }

    public interface ItemAccessor<T> : Accessor<T>, ItemAccessor where T : Item {
        T Item { get; }
    }

    public class BaseItemAccessor : BaseAccessor<Item>, ItemAccessor<Item> {
        public Item GetItem() {
            return Object;
        }

        public Item Item {
            get { return Object; }
        }
    }

    public class BaseItemAccessor<T> : BaseAccessor<T>, ItemAccessor<T> where T : Item {
        public Item GetItem() {
            return Object;
        }

        public T Item {
            get { return Object; }
        }
    }
}
