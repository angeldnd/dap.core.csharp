using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class EnumExtension {
        public static T GetValueAsEnum<T>(this IVar<int> v) where T : struct, IConvertible {
            return EnumHelper.FromInt<T>(v.Value);
        }

        public static bool SetValueAsEnum<T>(this IVar<int> v, T newValue) where T : struct, IConvertible {
            Type valueType = typeof(T);
            if (valueType._IsEnum()) {
                return v.SetValue(System.Convert.ToInt32(newValue));
            } else {
                Log.Error("Invalid Enum Type: {0}", valueType);
            }
            return false;
        }

        public static bool SetupAsEnum<T>(this IVar<int> v, T defaultValue) where T : struct, IConvertible {
            Type valueType = typeof(T);
            if (valueType._IsEnum()) {
                return v.Setup(System.Convert.ToInt32(defaultValue));
            } else {
                Log.Error("Invalid Enum Type: {0}", valueType);
            }
            return false;
        }
    }
}
