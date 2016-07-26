using System;
using System.Reflection;
using System.Linq;

namespace angeldnd.dap {
    //https://blogs.msdn.microsoft.com/dotnet/2016/02/10/porting-to-net-core/
    public static class TypeExtension {
        public static bool IsEnum(this Type type) {
#if DOTNET_CORE
            return type.GetTypeInfo().IsEnum;
#else
            return type.IsEnum;
#endif
        }

        public static bool IsInterface(this Type type) {
#if DOTNET_CORE
            return type.GetTypeInfo().IsInterface;
#else
            return type.IsInterface;
#endif
        }

        public static bool IsAssignableFrom(this Type type, Type anotherType) {
#if DOTNET_CORE
            return type.GetTypeInfo().IsAssignableFrom(anotherType);
#else
            return type.IsAssignableFrom(anotherType);
#endif
        }

        public static bool IsSubclassOf(this Type type, Type anotherType) {
#if DOTNET_CORE
            return type.GetTypeInfo().IsSubclassOf(anotherType);
#else
            return type.IsSubclassOf(anotherType);
#endif
        }

        public static object[] GetCustomAttributes(this Type type, bool inherit) {
#if DOTNET_CORE
            return type.GetTypeInfo().GetCustomAttributes(inherit).ToArray();
#else
            return type.GetCustomAttributes(inherit);
#endif
        }
    }
}
