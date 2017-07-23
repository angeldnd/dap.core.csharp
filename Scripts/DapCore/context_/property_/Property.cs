using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class Property<T>: Var<IProperties, T>, IProperty<T> {
        public Property(IProperties owner, string key) : base(owner, key) {
        }

        public Property(IProperties owner, int index) : base(owner, index) {
        }

        public override string GetValueStr() {
            Convertor<T> convertor = Convertor.GetConvertor<T>(true);
            string val = convertor != null ? convertor.Convert(Value) : base.GetValueString();
        }

        public Data Encode() {
            if (!string.IsNullOrEmpty(DapType)) {
                Data data = DataCache.Take(this);
                if (data.SetString(ObjectConsts.KeyDapType, DapType)) {
                    if (GetEncoder().Encode(data, PropertiesConsts.KeyValue, Value)) {
                        return data;
                    } else {
                        Error("Encode Failed: {0}", Value);
                    }
                }
            } else {
                Error("Not Encodable!");
            }
            return null;
        }

        public bool Decode(Data data) {
            if (data == null) return false;
            string dapType = data.GetString(ObjectConsts.KeyDapType);
            if (dapType == DapType) {
                return DecodeValue(data);
            } else {
                Error("Dap Type Mismatched: {0}, {1}", DapType, data.ToFullString());
            }
            return false;
        }

        public Data EncodeValue() {
            IProfiler profiler = Log.BeginSample("EncodeValue");
            Data result = null;
            Data data = DataCache.Take(this);
            if (GetEncoder().Encode(data, PropertiesConsts.KeyValue, Value)) {
                result = data;
            }
            if (profiler != null) profiler.EndSample();
            return result;
        }

        public bool DecodeValue(Data data) {
            T val;
            if (GetEncoder().TryDecode(data, PropertiesConsts.KeyValue, out val)) {
                return SetValue(val);
            }
            return false;
        }

        protected override void AddSummaryFields(Data summary) {
            base.AddSummaryFields(summary);
            summary.A(ContextConsts.SummaryData, Encode());
        }

        protected abstract Encoder<T> GetEncoder();
    }
}
