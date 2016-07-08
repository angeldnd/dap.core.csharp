using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    //SILP: PROPERTY_CLASS(String, string)
    [DapType(PropertiesConsts.TypeStringProperty)]                                       //__SILP__
    [DapOrder(-10)]                                                                      //__SILP__
    public sealed class StringProperty : BaseProperty<string> {                          //__SILP__
        public StringProperty(IDictProperties owner, string key) : base(owner, key) {    //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        public StringProperty(ITableProperties owner, int index) : base(owner, index) {  //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        protected override bool DoEncode(Data data) {                                    //__SILP__
            return data.SetString(PropertiesConsts.KeyValue, Value);                     //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        protected override bool DoDecode(Data data) {                                    //__SILP__
            return SetValue(data.GetString(PropertiesConsts.KeyValue));                  //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        protected override bool NeedUpdate(string newVal) {                              //__SILP__
            return base.NeedSetup() || (Value != newVal);                                //__SILP__
        }                                                                                //__SILP__
    }                                                                                    //__SILP__
}
