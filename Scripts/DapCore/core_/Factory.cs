using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class Factory {
        private static readonly Dictionary<string, Type> _Types = new Dictionary<string, Type>();

        public static bool Register<T>(string type) where T : class, IObject {
            return Register(type, typeof(T));
        }

        public static bool Register(string type, Type newType) {
            Type oldType = GetDapType(type);
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

        public static Type GetDapType(string type) {
            Type oldType;
            if (_Types.TryGetValue(type, out oldType)) {
                return oldType;
            }
            return null;
        }

        public static T New<T>(string type, params object[] values) where T : class, IObject {
            Type oldType = GetDapType(type);
            if (oldType != null) {
                try {
                    object obj = Activator.CreateInstance(oldType, values);
                    if (obj is T) {
                        return (T)obj;
                    } else {
                        Log.Error("Factory.New: <{0}> Type Mismatched: {1} -> {2}",
                                type, typeof(T).FullName, obj.GetType().FullName);
                    }
                } catch (Exception e) {
                    Log.Error("Factory.New: <{0}> {1} -> {2}", type, typeof(T).FullName, e);
                }
            } else {
                Log.Error("Factory.New: <{0}> Unknown Type", type);
            }
            return null;
        }
    }
}
