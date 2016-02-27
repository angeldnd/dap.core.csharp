using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace angeldnd.dap {
    public abstract class Plugin : Logger {
        public bool Init() {
            return OnInit();
        }

        protected virtual bool OnInit() {
            return true;
        }
    }
}
