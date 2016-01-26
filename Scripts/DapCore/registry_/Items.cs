using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;

namespace angeldnd.dap {
    public static class ItemsConsts {
        public const string TypeItems = "Items";
    }

    public class Items : DictContext<IDictContext, IContext> {
        public override string Type {
            get { return ItemsConsts.TypeItems; }
        }

        public Items(IDictContext owner, string key) : base(owner, key) {
        }
    }
}
