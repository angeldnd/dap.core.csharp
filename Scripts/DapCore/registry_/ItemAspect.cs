using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface ItemAspect : ContextAspect {
        Registry Registry { get; }
        string ItemPath { get; }
        Item GetItem();
    }

    public abstract class ItemAspect<T> : ContextAspect<T>, ItemAccessor<T>, ItemAspect where T : Item {
        public T Item {
            get { return Context; }
        }

        public string ItemPath {
            get {
                return Context == null ? null : Context.Path;
            }
        }

        public Item GetItem() {
            return Context;
        }

        public Registry Registry {
            get { return Context == null ? null : Context.Registry; }
        }
    }
}
