using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public class DataTypeConvertor : Convertor<DataType> {
        public const string Bool = "B";
        public const string Int = "I";
        public const string Long = "L";
        public const string Float = "F";
        public const string Double = "D";
        public const string String = "S";
        public const string Data = "A";

        public override string Convert(DataType valueType) {
            switch (valueType) {
                case DataType.Bool:
                    return Bool;
                case DataType.Int:
                    return Int;
                case DataType.Long:
                    return Long;
                case DataType.Float:
                    return Float;
                case DataType.Double:
                    return Double;
                case DataType.String:
                    return String;
                case DataType.Data:
                    return Data;
            }
            return "?";
        }

        public override DataType Parse(string str) {
            if (str == Bool) {
                return DataType.Bool;
            } else if (str == Int) {
                return DataType.Int;
            } else if (str == Long) {
                return DataType.Long;
            } else if (str == Float) {
                return DataType.Float;
            } else if (str == Double) {
                return DataType.Double;
            } else if (str == String) {
                return DataType.String;
            } else if (str == Data) {
                return DataType.Data;
            }
            throw new ArgumentException(string.Format(
                        "Invalid DataType: {0}", str));
        }
    }
}
