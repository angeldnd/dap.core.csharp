using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    /* TODO: find a better way for this old logic to fit into the current system

    public class Spec : ItemAspect<Item>, IDataChecker {
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
    */
}
