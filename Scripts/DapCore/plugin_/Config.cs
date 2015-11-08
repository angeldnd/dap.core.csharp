using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace angeldnd.dap {
    [DapOrder(-1)] //Make config plugin init before other
    public abstract class Config : Plugin {
        protected override bool OnInit() {
            //TODO
            return true;
        }
    }
}
