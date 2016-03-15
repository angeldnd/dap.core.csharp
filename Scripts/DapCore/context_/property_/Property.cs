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
                if (data.SetString(ObjectConsts.KeyDapType, DapType)) {
                    if (DoEncode(data)) {
                        return data;
                    }
                }
            }
            if (LogDebug) Debug("Not Encodable!");
            return null;
        }

        public virtual bool Decode(Data data) {
            string dapType = data.GetString(ObjectConsts.KeyDapType);
            if (dapType == DapType) {
                return DoDecode(data);
            } else {
                Error("Dap Type Mismatched: {0}, {1}", DapType, dapType);
            }
            return false;
        }

        protected override void AddSummaryFields(Data summary) {
            base.AddSummaryFields(summary);
            summary.A(ContextConsts.SummaryData, Encode());
        }

        protected abstract bool DoEncode(Data data);
        protected abstract bool DoDecode(Data data);
    }
}
