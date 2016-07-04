using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public static class EnumHelper {
        public static T FromInt<T>(int val) where T : struct, IConvertible {
            Type valueType = typeof(T);
            if (valueType.IsEnum) {
                return Enum.ToObject(Type, val);
            } else {
                Log.Error("Invalid Enum Type: {0}", valueType);
            }
            return default(T);
        }

        public static int ToInt<T>(T val) where T : struct, IConvertible {
            Type valueType = typeof(T);
            if (valueType.IsEnum) {
                return (int)val;
            } else {
                Log.Error("Invalid Enum Type: {0}", valueType);
            }
            return -1;
        }
    }
}
