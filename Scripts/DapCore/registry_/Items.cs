using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;

namespace angeldnd.dap {
    public static class ItemsConsts {
        public const string TypeItems = "Items";
    }

    [DapType(ItemsConsts.TypeItems)]
    [DapOrder(-9)]
    public sealed class Items : DictContext<IDictContext, IContext> {
        public Items(IDictContext owner, string key) : base(owner, key) {
        }
    }
}
