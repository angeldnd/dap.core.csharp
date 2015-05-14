using System;
using System.Diagnostics;
using System.IO;
//using System.Text.RegularExpressions;

namespace angeldnd.dap {
    [System.AttributeUsage(System.AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class DapPriority: System.Attribute {
        public readonly int Priority;
        public DapPriority(int priority) {
            Priority = priority;
        }
    }

    /*
     * DapParam is only used as more structured comments ATM
     */
    [System.AttributeUsage(System.AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class DapParam : System.Attribute {
        public readonly System.Type ParamType;
        public readonly bool Optional;

        public DapParam(System.Type t, bool optional) {
            ParamType = t;
            Optional = optional;
        }

        public DapParam(System.Type t) : this(t, false) {}
    }
}
