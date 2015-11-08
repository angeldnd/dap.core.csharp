using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public struct SpecConsts {
        public const string KeySpec = "spec";

        public const string KindBigger = ">";
        public const string KindBiggerOrEqual = ">=";
        public const string KindSmaller = "<";
        public const string KindSmallerOrEqual = "<=";
        //Note that since the == and != check is not reliable for
        //float and double, not adding these kinds for them.
        public const string KindIn = "~";
        public const string KindNotIn = "!~";

        public const string Separator = ":";
        public const string SubPrefix = ".";

        public static string GetSubSpecKey(string subKey, string SpecKind) {
            return string.Format("{0}{1}{2}{3}", SubPrefix, subKey, Separator, SpecKind);
        }

        public static string GetSubKey(string specKey) {
            if (specKey.StartsWith(SubPrefix)) {
                int index = specKey.IndexOf(Separator);
                if (index > 0) {
                    return specKey.Substring(0, index).Substring(SubPrefix.Length - 1);
                }
            }
            return null;
        }

        public static string GetSpecKind(string specKey) {
            if (specKey.StartsWith(SubPrefix)) {
                int index = specKey.IndexOf(Separator);
                if (index > 0) {
                    return specKey.Substring(index + Separator.Length);
                }
            }
            return specKey;
        }
    }

    public class Spec : ItemAspect, DataChecker {
        protected virtual bool StrictMode {
            get { return false; }
        }

        public bool IsValid(Data data) {
            foreach (string key in data.Keys) {
                Property prop = Item.Properties.Get<Property>(key);
                if (prop == null ) {
                    if (StrictMode) {
                        return false;
                    }
                } else {
                    if (!IsValidValue(prop, data, key)) {
                        return false;
                    }
                }
            }
            return true;
        }

        protected virtual bool IsValidValue(Property prop, Data data, string key) {
            DataType valueType = data.GetValueType(key);

            switch (valueType) {
                case DataType.Bool:
                    return (prop is BoolProperty) && ((BoolProperty)prop).CheckNewValue(data.GetBool(key));
                case DataType.Int:
                    return (prop is IntProperty) && ((IntProperty)prop).CheckNewValue(data.GetInt(key));
                case DataType.Long:
                    return (prop is LongProperty) && ((LongProperty)prop).CheckNewValue(data.GetLong(key));
                case DataType.Float:
                    return (prop is FloatProperty) && ((FloatProperty)prop).CheckNewValue(data.GetFloat(key));
                case DataType.Double:
                    return (prop is DoubleProperty) && ((DoubleProperty)prop).CheckNewValue(data.GetDouble(key));
                case DataType.String:
                    return (prop is StringProperty) && ((StringProperty)prop).CheckNewValue(data.GetString(key));
                case DataType.Data:
                    return (prop is DataProperty) && ((DataProperty)prop).CheckNewValue(data.GetData(key));
            }
            return false;
        }
    }

    public class StrictSpec : Spec {
        protected override bool StrictMode {
            get { return true; }
        }
    }
}
