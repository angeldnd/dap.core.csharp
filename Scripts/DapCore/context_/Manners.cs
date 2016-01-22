using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public sealed class Manners : TreeAspect<IContext, Manner> {
        public Manners(IContext owner, Pass pass) : base(owner, pass) {
        }
    }
}
