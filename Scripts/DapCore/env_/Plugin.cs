using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace angeldnd.dap {
    public abstract class Plugin {
        private bool _Inited = false;
        public bool Inited {
            get { return _Inited; }
        }

        public bool Init() {
            if (_Inited) {
                return false;
            }
            _Inited = true;
            return OnInit();
        }

        protected virtual bool OnInit() {
            return true;
        }

        public virtual bool SetupRegistry(Registry registry) {
            return true;
        }
    }
}
