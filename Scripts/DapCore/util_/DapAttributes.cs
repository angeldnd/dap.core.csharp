using System;
using System.Diagnostics;
using System.IO;
//using System.Text.RegularExpressions;

namespace angeldnd.dap {
    [System.AttributeUsage(System.AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class DapPriority: System.Attribute {
        public static int GetPriority(Type type) {
            Object[] attribs = type.GetCustomAttributes(false);
            foreach (var attr in attribs) {
                if (attr is DapPriority) {
                    return ((DapPriority)attr).Priority;
                }
            }
            return 0;
        }

        public readonly int Priority;
        public DapPriority(int priority) {
            Priority = priority;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class DapOrder: System.Attribute {
        public static int GetOrder(Type type) {
            Object[] attribs = type.GetCustomAttributes(false);
            foreach (var attr in attribs) {
                if (attr is DapOrder) {
                    return ((DapOrder)attr).Order;
                }
            }
            return 0;
        }

        public readonly int Order;
        public DapOrder(int order) {
            Order = order;
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
