using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public struct SpecConsts {
        public const string KeyValue = "v";
    }

    public class SpecChecker : DataChecker {
        public readonly Data Spec = new Data();

        public virtual bool IsValid(Data data) {
            return Spec.CheckEachValueType((string key, DataType type) => {
                return data.GetValueType(key) == type;
            });
        }
    }
}
