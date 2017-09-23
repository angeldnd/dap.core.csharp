using System;
using System.Reflection;
using System.Linq;

namespace angeldnd.dap {
    public static class TypeExtension {
        public static bool _IsEnum(this Type type) {
            return type.IsEnum;
        }

        public static bool _IsInterface(this Type type) {
            return type.IsInterface;
        }

        public static bool _IsAbstract(this Type type) {
            return type.IsAbstract;
        }

        public static bool _IsAssignableFrom(this Type type, Type anotherType) {
            return type.IsAssignableFrom(anotherType);
        }

        public static bool _IsSubclassOf(this Type type, Type anotherType) {
            return type.IsSubclassOf(anotherType);
        }

        public static object[] _GetCustomAttributes(this Type type, bool inherit) {
            return type.GetCustomAttributes(inherit);
        }

        public static object[] _GetCustomAttributes(this FieldInfo field, bool inherit) {
            return field.GetCustomAttributes(inherit);
        }
    }
}
