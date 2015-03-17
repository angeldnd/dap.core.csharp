using System;
using System.Diagnostics;
using System.IO;
//using System.Text.RegularExpressions;

namespace angeldnd.dap {
    [System.AttributeUsage(System.AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class DapParam : System.Attribute {
        public System.Type ParamType;
        public bool Optional;

        public DapParam(System.Type t, bool optional) {
            ParamType = t;
            Optional = optional;
        }

        public DapParam(System.Type t) : this(t, false) {}
    }
}