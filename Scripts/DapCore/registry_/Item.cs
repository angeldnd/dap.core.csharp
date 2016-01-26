using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;

namespace angeldnd.dap {
    public static class ItemConsts {
        public const string TypeItem = "Item";
    }

    public class Item : Context<IDictContext> {
        public override string Type {
            get { return ItemConsts.TypeItem; }
        }

        public Item(IDictContext owner, string key) : base(owner, key) {
        }
    }
}
