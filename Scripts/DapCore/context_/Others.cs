using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public sealed class Others : TreeSection<IContext, IInTreeAspect> {
        public Others(IContext owner, Pass pass) : base(owner, pass) {
        }
    }
}
