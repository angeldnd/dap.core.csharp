using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    //SILP: PROPERTY_CLASS(Float, float)
    public sealed class FloatProperty : Property<float> {                               //__SILP__
        public override string Type {                                                   //__SILP__
            get { return PropertiesConsts.TypeFloatProperty; }                          //__SILP__
        }                                                                               //__SILP__
                                                                                        //__SILP__
        public FloatProperty(IDictProperties owner, string key) : base(owner, key) {    //__SILP__
        }                                                                               //__SILP__
                                                                                        //__SILP__
        public FloatProperty(ITableProperties owner, int index) : base(owner, index) {  //__SILP__
        }                                                                               //__SILP__
                                                                                        //__SILP__
        protected override bool DoEncode(Data data) {                                   //__SILP__
            return data.SetFloat(PropertiesConsts.KeyValue, Value);                     //__SILP__
        }                                                                               //__SILP__
                                                                                        //__SILP__
        protected override bool DoDecode(Data data) {                                   //__SILP__
            return SetValue(data.GetFloat(PropertiesConsts.KeyValue));                  //__SILP__
        }                                                                               //__SILP__
                                                                                        //__SILP__
        protected override bool NeedUpdate(float newVal) {                              //__SILP__
            return base.NeedUpdate(newVal) || (Value != newVal);                        //__SILP__
        }                                                                               //__SILP__
    }                                                                                   //__SILP__
}
