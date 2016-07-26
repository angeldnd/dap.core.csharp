using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

#if DOTNET_CORE
using Microsoft.Extensions.DependencyModel;
using Microsoft.DotNet.InternalAbstractions;
#endif

namespace angeldnd.dap {
    public static class AssemblyHelper {
        private enum CheckMode {SubClass, Interface, Assignable};

        private static bool IsValidType(CheckMode mode, Type baseType, Type type) {
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

        private static void ForEachAssembly(Action<Assembly> callback) {
#if DOTNET_CORE
            var libs = DependencyContext.Default.CompileLibraries;
            foreach (var lib in libs) {
                Assembly asm = Assembly.Load(new AssemblyName(lib.Name));
#else
            Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly asm in asms) {
#endif
                callback(asm);
            }
        }

        private static void ForEachType(Action<Type> callback) {
            ForEachAssembly((Assembly asm) => {
                Type[] types = asm.GetTypes();

                foreach (Type type in types) {
                    callback(type);
                }
            });
        }

        private static void ForEachType(CheckMode mode, Type baseType, Action<Type> callback) {
            ForEachType((Type type) => {
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
