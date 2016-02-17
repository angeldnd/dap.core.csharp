using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class Property<T>: Var<IProperties, T>, IProperty<T> {
        public Property(IProperties owner, string key) : base(owner, key) {
        }

        public Property(IProperties owner, int index) : base(owner, index) {
        }

        public Data Encode() {
            if (!string.IsNullOrEmpty(DapType)) {
                Data data = new Data();
                if (data.SetString(ObjectConsts.KeyType, DapType)) {
                    if (DoEncode(data)) {
                        return data;
                    }
                }
            }
            if (LogDebug) Debug("Not Encodable!");
            return null;
        }

        public virtual bool Decode(Data data) {
            string type = data.GetString(ObjectConsts.KeyType);
            if (type == DapType) {
                return DoDecode(data);
            } else {
                Error("Type Mismatched: {0}, {1}", DapType, type);
            }
            return false;
        }

        protected abstract bool DoEncode(Data data);
        protected abstract bool DoDecode(Data data);
    }
}
