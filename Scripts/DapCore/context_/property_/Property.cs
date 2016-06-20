using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class Property<T>: BaseProperty<T> {
        public Property(IProperties owner, string key) : base(owner, key) {
        }

        public Property(IProperties owner, int index) : base(owner, index) {
        }

        protected override bool DoEncode(Data data) {
            return data.SetData(PropertiesConsts.KeyValue, DoEncodeValue());
        }

        protected override bool DoDecode(Data data) {
            Data v = data.GetData(PropertiesConsts.KeyValue);
            if (v == null) return false;

            return DoDecodeValue(v);
        }

        protected abstract Data DoEncodeValue();
        protected abstract bool DoDecodeValue(Data v);
    }
}
