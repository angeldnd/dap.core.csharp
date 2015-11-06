using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace angeldnd.dap {
    public abstract class Plugin {
        public bool Inited { get; private set; }

        public bool Init() {
            if (Inited) {
                return false;
            }
            Inited = true;
            return OnInit();
        }

        protected abstract bool OnInit();
    }
}
