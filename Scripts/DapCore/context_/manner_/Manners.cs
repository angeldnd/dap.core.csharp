using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public sealed class Manners : DictAspect<IContext, IManner> {
        public Manners(IContext owner, string key) : base(owner, key) {
        }
    }
}
