using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    //SILP: PROPERTY_CLASS(Bool, bool)
    public sealed class BoolProperty : Property<bool> {                                //__SILP__
        public override string Type {                                                  //__SILP__
            get { return PropertiesConsts.TypeBoolProperty; }                          //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        public BoolProperty(IDictProperties owner, string key) : base(owner, key) {    //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        public BoolProperty(ITableProperties owner, int index) : base(owner, index) {  //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        protected override bool DoEncode(Data data) {                                  //__SILP__
            return data.SetBool(PropertiesConsts.KeyValue, Value);                     //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        protected override bool DoDecode(Data data) {                                  //__SILP__
            return SetValue(data.GetBool(PropertiesConsts.KeyValue));                  //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        protected override bool NeedUpdate(bool newVal) {                              //__SILP__
            return base.NeedUpdate(newVal) || (Value != newVal);                       //__SILP__
        }                                                                              //__SILP__
    }                                                                                  //__SILP__
}
