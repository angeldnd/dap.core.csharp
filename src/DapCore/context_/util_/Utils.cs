using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public partial class Utils : Aspect<IContext> {
        public Utils(IContext owner, string key) : base(owner, key) {
        }
    }
}
