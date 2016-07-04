using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class EnumExtension {
        public static T GetValueAsEnum<T>(this IVar<int> v) where T : struct, IConvertible {
            return EnumHelper.FromInt<T>(v.Value);
        }

        public static bool SetValueAsEnum<T>(this IVar<int> v, T newValue) where T : struct, IConvertible {
            Type valueType = typeof(T);
            if (valueType.IsEnum) {
                return v.SetValue((int)val);
            } else {
                Log.Error("Invalid Enum Type: {0}", valueType);
            }
            return false;
        }

        public static bool SetupAsEnum<T>(this IVar<int> v, T defaultValue) where T : struct, IConvertible {
            Type valueType = typeof(T);
            if (valueType.IsEnum) {
                return v.Setup((int)defaultValue);
            } else {
                Log.Error("Invalid Enum Type: {0}", valueType);
            }
            return false;
        }
    }
}
