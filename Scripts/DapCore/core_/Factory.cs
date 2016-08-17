using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class Factory {
        private static readonly Dictionary<string, Type> _Types = new Dictionary<string, Type>();
        private static readonly Dictionary<string, Type> _VarTypes = new Dictionary<string, Type>();

        public static bool Register<T>(string type) where T : class, IObject {
            return Register(type, typeof(T));
        }

        public static bool Register(string type, Type newType) {
            Type oldType = GetDapType(type, true);
            if (oldType != null) {
                if (oldType == newType) {
                    return true;
                } else {
                    Log.Critical("Factory.Register: <{0}> Already Registered: {1} -> {2}",
                                type, oldType.FullName, newType.FullName);
                    return false;
                }
            }
            _Types[type] = newType;
            Type varType = DapVarType.GetDapVarType(newType);
            if (varType != null) {
                _VarTypes[type] = varType;
            }
            return true;
        }

        public static Type GetDapType(string type, bool isDebug = false) {
            Type oldType;
            if (_Types.TryGetValue(type, out oldType)) {
                return oldType;
            } else {
                Log.ErrorOrDebug(isDebug, "DapType Not Found: {0}", type);
            }
            return null;
        }

        public static Type GetDapVarType(string type, bool isDebug = false) {
            Type oldVarType;
            if (_VarTypes.TryGetValue(type, out oldVarType)) {
                return oldVarType;
            } else {
                Log.ErrorOrDebug(isDebug, "DapVarType Not Found: {0}", type);
            }
            return null;
        }

        private static T Create<T>(string caller, Type type, params object[] values) {
            if (type != null) {
                try {
                    object obj = Activator.CreateInstance(type, values);
                    if (obj is T) {
                        return (T)obj;
                    } else {
                        Log.Error("Factory.{0}: <{1}> Type Mismatched: {2} -> {3}",
                                caller, typeof(T).FullName, type, obj.GetType().FullName);
                    }
                } catch (Exception e) {
                    Log.Error("Factory.{0}: <{1}> {2} -> {3}",
                                caller, typeof(T).FullName, type, e);
                }
            } else {
                Log.Error("Factory.{0}: <{1}> {2} -> Unknown Type",
                                caller, typeof(T).FullName, type);
            }
            return default(T);
        }

        public static T New<T>(string type, params object[] values) where T : IObject {
            Type dapType = GetDapType(type);
            if (dapType != null) {
                return Create<T>("New", dapType, values);
            }
            return default(T);
        }

        public static T Create<T>(Type type, params object[] values) {
            return Create<T>("Create", type, values);
        }

        public static T Create<T>(params object[] values) {
            return Create<T>(typeof(T), values);
        }

        public static object[] InsertParams(object[] originalValues, params object[] toInsertValues) {
            if (originalValues == null || originalValues.Length == 0) return toInsertValues;
            if (toInsertValues == null || toInsertValues.Length == 0) return originalValues;

            object[] values = new object[toInsertValues.Length + originalValues.Length];
            for (int i = 0; i < toInsertValues.Length; i++) {
                values[i] = toInsertValues[i];
            }
            for (int i = 0; i < originalValues.Length; i++) {
                values[i + toInsertValues.Length] = originalValues[i];
            }
            return values;
        }

        public static object[] AppendParams(object[] originalValues, params object[] toAppendValues) {
            if (originalValues == null || originalValues.Length == 0) return toAppendValues;
            if (toAppendValues == null || toAppendValues.Length == 0) return originalValues;

            object[] values = new object[originalValues.Length + toAppendValues.Length];
            for (int i = 0; i < originalValues.Length; i++) {
                values[i] = originalValues[i];
            }
            for (int i = 0; i < toAppendValues.Length; i++) {
                values[i + originalValues.Length] = toAppendValues[i];
            }
            return values;
        }
    }
}
