using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;

namespace angeldnd.dap {
    public static class ItemConsts {
        public const string TypeItem = "Item";
    }

    [DapType(ItemConsts.TypeItem)]
    [DapOrder(-9)]
    public class Item : Context<IDictContext> {
        public Item(IDictContext owner, string key) : base(owner, key) {
        }
    }
}
