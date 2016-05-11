using System;
using System.Collections.Generic;

using System.Diagnostics;
using System.IO;
using System.Reflection;
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

        public static void SortByOrder<T>(List<T> objs) {
            Dictionary<T, int> orders = new Dictionary<T, int>();

            foreach (T obj in objs) {
                orders[obj] = GetOrder(obj.GetType());
            }

            objs.Sort((T a, T b) => {
                int orderA = orders[a];
                int orderB = orders[b];
                if (orderA == orderB) {
                    return a.GetType().FullName.CompareTo(b.GetType().FullName);
                } else {
                    return orderA.CompareTo(orderB);
                }
            });
        }

        public readonly int Order;
        public DapOrder(int order) {
            Order = order;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class DapParam : System.Attribute {
        public const string DefaultSuffix = "_Default";
        public static string GetDefaultFieldName(string fieldName) {
            return string.Format("{0}{1}", fieldName, DefaultSuffix);
        }

        public static DapParam GetDapParam(FieldInfo field) {
            object[] attribs = field.GetCustomAttributes(false);
            foreach (var attr in attribs) {
                if (attr is DapParam) {
                    return (DapParam)attr;
                }
            }
            return null;
        }

        public readonly System.Type ParamType;
        public readonly bool Optional;

        public DapParam(System.Type t, bool optional) {
            ParamType = t;
            Optional = optional;
        }

        public DapParam(System.Type t) : this(t, false) {}
    }

    public static class DataDapParamExtension {
        public static Data AddParamHint(this Data data, Type type, string fieldName) {
            FieldInfo field = type.GetField(fieldName,
                                    BindingFlags.Public | BindingFlags.Static);
            if (field == null) {
                Log.Error("public static field not found: {0}.{1}", type.FullName, fieldName);
                return data;
            }

            DapParam dapParam = DapParam.GetDapParam(field);
            if (dapParam == null) {
                Log.Error("DapParam attribute not found: {0}.{1}", type.FullName, fieldName);
                return data;
            }

            string hint = string.Format("{0}", dapParam.ParamType.FullName);
            if (dapParam.Optional) {
                FieldInfo defaultField = type.GetField(DapParam.GetDefaultFieldName(fieldName),
                                        BindingFlags.Public | BindingFlags.Static);
                if (field == null) {
                    Log.Error("public static default field not found: {0}.{1}",
                                type.FullName, DapParam.GetDefaultFieldName(fieldName));
                    hint = string.Format("{0} = N/A", hint);
                } else {
                    hint = string.Format("{0} = {1}", hint, defaultField.GetValue(null));
                }
            }
            data.SetString(field.GetValue(null).ToString(), hint);

            return data;
        }
    }
}
