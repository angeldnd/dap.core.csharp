using System;
using System.Reflection;
using System.Linq;

namespace angeldnd.dap {
    //https://blogs.msdn.microsoft.com/dotnet/2016/02/10/porting-to-net-core/
    public static class TypeExtension {
        public static bool _IsEnum(this Type type) {
#if DOTNET_CORE
            return type.GetTypeInfo().IsEnum;
#else
            return type.IsEnum;
#endif
        }

        public static bool _IsInterface(this Type type) {
#if DOTNET_CORE
            return type.GetTypeInfo().IsInterface;
#else
            return type.IsInterface;
#endif
        }

        public static bool _IsAbstract(this Type type) {
#if DOTNET_CORE
            return type.GetTypeInfo().IsAbstract;
#else
            return type.IsAbstract;
#endif
        }

        public static bool _IsAssignableFrom(this Type type, Type anotherType) {
#if DOTNET_CORE
            return type.GetTypeInfo().IsAssignableFrom(anotherType);
#else
            return type.IsAssignableFrom(anotherType);
#endif
        }

        public static bool _IsSubclassOf(this Type type, Type anotherType) {
#if DOTNET_CORE
            return type.GetTypeInfo().IsSubclassOf(anotherType);
#else
            return type.IsSubclassOf(anotherType);
#endif
        }

        public static object[] _GetCustomAttributes(this Type type, bool inherit) {
#if DOTNET_CORE
            return type.GetTypeInfo().GetCustomAttributes(inherit).ToArray();
#else
            return type.GetCustomAttributes(inherit);
#endif
        }

        public static object[] _GetCustomAttributes(this FieldInfo field, bool inherit) {
#if DOTNET_CORE
            return field.GetCustomAttributes(inherit).ToArray();
#else
            return field.GetCustomAttributes(inherit);
#endif
        }
    }
}
