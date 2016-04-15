using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace angeldnd.dap {
    public static class AssemblyHelper {
        private enum CheckMode {SubClass, Interface, Assignable};

        private static bool IsValidType(CheckMode mode, Type baseType, Type type) {
            if (type == null || baseType == null) return false;
            switch (mode) {
                case CheckMode.SubClass:
                    return type.IsSubclassOf(baseType);
                case CheckMode.Interface:
                    return type.GetInterfaces().Contains(baseType);
                case CheckMode.Assignable:
                    return baseType.IsAssignableFrom(type);
            }
            return false;
        }

        private static void ForEachType(CheckMode mode, Type baseType, Action<Type> callback) {
            Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly asm in asms) {
                Type[] types = asm.GetTypes();

                foreach (Type type in types) {
                    if (type.IsAbstract) continue;
                    if (!IsValidType(mode, baseType, type)) continue;

                    callback(type);
                }
            }
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

        public static Type GetType(string fullName) {
            Type result = null;
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies()) {
                Type t = a.GetType(fullName);
                if (t != null){
                    if (result == null) {
                        result = t;
                    } else {
                        Log.Critical("Type Conflicted: {0}: {1} -> {2}", fullName, result, t);
                    }
                }
            }
            return result;
        }
    }
}
