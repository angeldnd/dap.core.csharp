using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public class DataTypeConvertor : Convertor<DataType> {
        public override string Convert(DataType valueType) {
            switch (valueType) {
                case DataType.Bool:
                    return "B";
                case DataType.Int:
                    return "I";
                case DataType.Long:
                    return "L";
                case DataType.Float:
                    return "F";
                case DataType.Double:
                    return "D";
                case DataType.String:
                    return "S";
                case DataType.Data:
                    return "A";
            }
            return "?";
        }

        public override DataType Parse(string str) {
            if (str == "B") {
                return DataType.Bool;
            } else if (str == "I") {
                return DataType.Int;
            } else if (str == "L") {
                return DataType.Long;
            } else if (str == "F") {
                return DataType.Float;
            } else if (str == "D") {
                return DataType.Double;
            } else if (str == "S") {
                return DataType.String;
            } else if (str == "A") {
                return DataType.Data;
            }
            throw new ArgumentException(string.Format(
                        "Invalid DataType: {0}", str));
        }
    }
}
