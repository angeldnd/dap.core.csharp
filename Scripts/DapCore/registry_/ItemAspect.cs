using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface ItemAspect : Aspect {
        Registry Registry { get; }
        string ItemPath { get; }
        Item GetItem();
    }

    public abstract class ItemAspect<T> : BaseSecurableAspect, ItemAccessor<T>, ItemAspect where T : Item {
        private T _Item = null;
        public T Item {
            get { return _Item; }
        }

        public string ItemPath {
            get {
                return _Item != null ? _Item.Path : null;
            }
        }

        public Item GetItem() {
            return _Item;
        }

        public T Object {
            get { return _Item; }
        }

        public T GetObject {
            return _Item;
        }

        public Registry Registry {
            get { return _Item == null ? null : _Item.Registry; }
        }

        public override bool Init(Entity entity, string path, Pass pass) {
            if (!base.Init(entity, path, pass)) {
                return false;
            }
            if (entity is T) {
                _Item = (T)entity;
                return true;
            } else {
                Error("Invalid Entity: {0} -> {1}", typeof(T).FullName, entity.GetType().FullName);
                return false;
            }
        }
    }
}
