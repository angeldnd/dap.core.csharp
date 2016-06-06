using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class BaseProperty<T>: Var<IProperties, T>, IProperty<T> {
        public BaseProperty(IProperties owner, string key) : base(owner, key) {
        }

        public BaseProperty(IProperties owner, int index) : base(owner, index) {
        }

        public Data Encode() {
            if (!string.IsNullOrEmpty(DapType)) {
                Data data = EncodeValue();
                if (data.SetString(ObjectConsts.KeyDapType, DapType)) {
                    return data;
                }
            }
            if (LogDebug) Debug("Not Encodable!");
            return null;
        }

        public virtual bool Decode(Data data) {
            if (data == null) return false;
            string dapType = data.GetString(ObjectConsts.KeyDapType);
            if (dapType == DapType) {
                return DoDecode(data);
            } else {
                Error("Dap Type Mismatched: {0}, {1}", DapType, dapType);
            }
            return false;
        }

        public Data EncodeValue() {
            Data data = new Data();
            if (DoEncode(data)) {
                return data;
            }
            return null;
        }

        public virtual bool DecodeValue(Data data) {
            if (data == null) return false;
            return DoDecode(data);
        }

        protected override void AddSummaryFields(Data summary) {
            base.AddSummaryFields(summary);
            summary.A(ContextConsts.SummaryData, Encode());
        }

        protected abstract bool DoEncode(Data data);
        protected abstract bool DoDecode(Data data);
    }
}
