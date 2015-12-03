using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface ItemAccessor : Accessor {
        Item GetItem();
    }

    public interface ItemAccessor<T> : Accessor<T>, ItemAccessor, Logger where T : class, Item {
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
}
