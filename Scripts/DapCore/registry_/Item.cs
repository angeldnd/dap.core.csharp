using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;

namespace angeldnd.dap {
    public interface IItem : IInTreeElement<Registry>, IContext {
    }

    public static class ItemConsts {
        public const string TypeItem = "Item";
    }

    public class Item : InTreeContext<Registry>, IItem {
        public override string Type {
            get { return ItemConsts.TypeItem; }
        }

        public Item(Registry owner, string path, Pass pass) : base(owner, path, pass) {
        }
    }
}
