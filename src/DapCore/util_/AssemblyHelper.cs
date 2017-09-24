using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;


namespace angeldnd.dap {
    public static class AssemblyHelper {
        public enum CheckMode {SubClass, Interface, Assignable};

        public static bool IsValidType(CheckMode mode, Type baseType, Type type) {
            if (type == null || baseType == null) return false;
            switch (mode) {
                case CheckMode.SubClass:
                    return type._IsSubclassOf(baseType);
                case CheckMode.Interface:
                    return type.GetInterfaces().Contains(baseType);
                case CheckMode.Assignable:
                    return baseType._IsAssignableFrom(type);
            }
            return false;
        }

        public static void ForEachAssembly(Action<Assembly> callback) {
            Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly asm in asms) {
                callback(asm);
            }
        }

        public static string _Debugging = null;

        public static void ForEachType(Action<Type> callback) {
            ForEachAssembly((Assembly asm) => {
                try {
                    Type[] types = asm.GetTypes();

                    foreach (Type type in types) {
                        callback(type);
                    }
                } catch (Exception e) {
                    if (_Debugging != null && Log.Provider != null) {
                        Log.Info("ForEachType Failed: [{0}] {1} -> {2}: {3}",
                                _Debugging, asm.GetName().Name, e.GetType().Name, e.Message);
                    }
                }
            });
        }

        public static void ForEachType(CheckMode mode, Type baseType, Action<Type> callback) {
            ForEachType((Type type) => {
                /*
                if (_Debugging != null && Log.Provider != null) {
                    Log.Info("ForEachType: {0} <{1}> {2} {3} -> {4} -> {5}",
                             _Debugging, baseType.FullName,
                             type.Assembly.GetName().Name, type.FullName,
                             type._IsAbstract(), IsValidType(mode, baseType, type));
                }
                */
                if (type._IsAbstract()) return;
                if (!IsValidType(mode, baseType, type)) return;

                callback(type);
            });
        }

        public static void ForEachSubClass<T>(Action<Type> callback) where T : class {
            ForEachType(CheckMode.SubClass, typeof(T), callback);
        }

        /*
         * There is no generic constraint for interface.
         *
         * https://msdn.microsoft.com/en-us/library/d5x73970.aspx
         */
        public static void ForEachInterface<T>(Action<Type> callback) where T : class {
            ForEachType(CheckMode.Interface, typeof(T), callback);
        }

        public static void ForEachAssignable<T>(Action<Type> callback) where T : class {
            ForEachType(CheckMode.Assignable, typeof(T), callback);
        }

        private static List<T> CreateInstances<T>(CheckMode mode, bool sortByOrder) {
            var result = new List<T>();
            ForEachType(mode, typeof(T), (Type type) => {
                T instance = Factory.Create<T>(type);
                if (instance != null) {
                    result.Add(instance);
                }
            });
            if (sortByOrder) {
                DapOrder.SortByOrder(result);
            }
            return result;
        }

        public static List<T> CreateInstancesOfSubClass<T>(bool sortByOrder = false) {
            return CreateInstances<T>(CheckMode.SubClass, sortByOrder);
        }

        public static List<T> CreateInstancesOfInterface<T>(bool sortByOrder = false) {
            return CreateInstances<T>(CheckMode.Interface, sortByOrder);
        }

        public static Type GetType(string fullName) {
            Type result = null;
            ForEachAssembly((Assembly asm) => {
                Type t = asm.GetType(fullName);
                if (t != null){
                    if (result == null) {
                        result = t;
                    } else {
                        Log.Critical("Type Conflicted: {0}: {1} -> {2}", fullName, result, t);
                    }
                }
            });
            return result;
        }
    }
}
