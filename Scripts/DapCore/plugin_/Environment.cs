using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace angeldnd.dap {
    public sealed class Environment {
        public readonly LogProvider LogProvider;
        public readonly Plugin[] Plugins;

        private string _Bootstrapper;
        public string Bootstrapper {
            get {
                return _Bootstrapper;
            }
            set {
                if (_Bootstrapper == null) {
                    _Bootstrapper = value;
                }
            }
        }

        public Environment(LogProvider logProvider, Plugin[] plugins) {
            LogProvider = logProvider;
            Plugins = plugins;
        }
    }
}
