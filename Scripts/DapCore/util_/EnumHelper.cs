using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public static class EnumHelper {
        public static T FromInt<T>(int val) where T : struct, IConvertible {
            Type valueType = typeof(T);
            if (valueType._IsEnum()) {
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
            if (valueType._IsEnum()) {
                return System.Convert.ToInt32(val);
            } else {
                Log.Error("Invalid Enum Type: {0}", valueType);
            }
            return -1;
        }

        public static T FromString<T>(string val) where T : struct, IConvertible {
            Type valueType = typeof(T);
            if (valueType._IsEnum()) {
                try {
                    T result = (T)Enum.Parse(valueType, val, true); //Case Insensitive here.
                    if (Enum.IsDefined(valueType, val)) {
                        return result;
                    } else {
                        Log.Error("Invalid Enum Value: {0} -> {1}", valueType, val);
                    }
                } catch (Exception e) {
                    Log.Error("Invalid Enum Value: {0} -> {1} -> {2}", valueType, val, e);
                }
            } else {
                Log.Error("Invalid Enum Type: {0}", valueType);
            }
            return default(T);
        }

        public static string ToString<T>(T val) where T : struct, IConvertible {
            Type valueType = typeof(T);
            if (valueType._IsEnum()) {
                return val.ToString();
            } else {
                Log.Error("Invalid Enum Type: {0}", valueType);
            }
            return "";
        }
    }
}
