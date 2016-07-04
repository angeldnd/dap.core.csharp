using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public static class EnumHelper {
        public static T FromInt<T>(int val) where T : struct, IConvertible {
            Type valueType = typeof(T);
            if (valueType.IsEnum) {
                if (Enum.IsDefined(valueType, val)) {
                    return (T)Enum.ToObject(valueType, val);
                } else {
                    Log.Error("Invalid Enum Value: {0} -> {1}", valueType, val);
                }
            } else {
                Log.Error("Invalid Enum Type: {0}", valueType);
            }
            return default(T);
        }

        public static int ToInt<T>(T val) where T : struct, IConvertible {
            Type valueType = typeof(T);
            if (valueType.IsEnum) {
                return System.Convert.ToInt32(val);
            } else {
                Log.Error("Invalid Enum Type: {0}", valueType);
            }
            return -1;
        }
    }
}
