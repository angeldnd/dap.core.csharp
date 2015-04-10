using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public class Spec : DataChecker {
        public readonly Data Defaults = new Data();

        public virtual bool IsValid(Data data) {
            return Defaults.CheckEachValueType((string key, DataType type) => {
                return data.GetValueType(key) == type;
            });
        }
    }
}
