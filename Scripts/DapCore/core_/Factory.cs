using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class Factory {
        private static readonly Dictionary<string, Type> _Types = new Dictionary<string, Type>();

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
    }
}
