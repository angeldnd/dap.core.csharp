using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class Items<TO, T> : Tree<TO, T>
                                            where TO : Section
                                            where T : Item {
        public override char Separator {
            get { return RegistryConsts.Separator; }
        }

        protected Items(TO owner, string path, Pass pass) : base(owner, path, pass) {
        }
    }

    public sealed class RegistryItems : Items<Registry, Item> {
    }
}
