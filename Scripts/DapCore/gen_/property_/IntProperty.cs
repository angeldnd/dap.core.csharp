using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    //SILP: PROPERTY_CLASS(Int, int)
    public sealed class IntProperty : Property<int> {                           //__SILP__
        public override string Type {                                           //__SILP__
            get { return PropertiesConsts.TypeIntProperty; }                    //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        public IntProperty(Properties owner, string key) : base(owner, key) {   //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        public IntProperty(Properties owner, int index) : base(owner, index) {  //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        protected override bool DoEncode(Data data) {                           //__SILP__
            return data.SetInt(PropertiesConsts.KeyValue, Value);               //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        protected override bool DoDecode(Data data) {                           //__SILP__
            return SetValue(data.GetInt(PropertiesConsts.KeyValue));            //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        protected override bool NeedUpdate(int newVal) {                        //__SILP__
            return base.NeedUpdate(newVal) || (Value != newVal);                //__SILP__
        }                                                                       //__SILP__
    }                                                                           //__SILP__
}
