using System;
using System.Diagnostics;
using System.IO;
//using System.Text.RegularExpressions;

namespace angeldnd.dap {
    [System.AttributeUsage(System.AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class DapType: System.Attribute {
        public static string GetDapType(Type type) {
            object[] attribs = type.GetCustomAttributes(false);
            foreach (var attr in attribs) {
                if (attr is DapType) {
                    return ((DapType)attr).Type;
                }
            }
            return null;
        }

        public readonly string Type;
        public DapType(string type) {
            Type = type;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class DapPriority: System.Attribute {
        public static int GetPriority(Type type) {
            object[] attribs = type.GetCustomAttributes(false);
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

    [System.AttributeUsage(System.AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class DapOrder: System.Attribute {
        public static int GetOrder(Type type) {
            object[] attribs = type.GetCustomAttributes(false);
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
