using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace angeldnd.dap {
    public abstract class Config : DataEntry {
        protected override bool OnSetup() {
            foreach (string key in Data.Keys) {
                DataType valueType = Data.GetValueType(key);
                if (valueType != DataType.Data) {
                    Error("Invalid Value Type: {0} : {1} -> {2}", key, valueType, Data.GetValue(key));
                    return false;
                }
                Property prop = Item.Properties.Get<Property>(key);
                if (prop == null) {
                    Error("Property Not Exist: {0}", key);
                    return false;
                }
                if (!prop.Decode(Pass, Data.GetData(key))) {
                    return false;
                }
            }
            return true;
        }
    }
}
