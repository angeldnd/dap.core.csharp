using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    /*
     * Any Item can have only one ItemType aspect, which is used to
     * identify it's main type, though it may have multiple ItemAspect
     */
    public abstract class ItemAspect : BaseSecurableAspect {
        private Item _Item = null;
        public Item Item {
            get { return _Item; }
        }

        public Registry Registry {
            get { return _Item == null ? null : _Item.Registry; }
        }

        public override bool Init(Entity entity, string path, Pass pass) {
            if (!base.Init(entity, path, pass)) {
                return false;
            }
            if (!(entity is Item)) {
                Error("Invalid Entity: {0}", entity.GetType());
                return false;
            }
            _Item = entity as Item;
            return true;
        }

        public string GetDescendantPath(string relativePath) {
            return Item.GetDescendantPath(relativePath);
        }
    }

    public abstract class ItemType : ItemAspect {
    }
}
