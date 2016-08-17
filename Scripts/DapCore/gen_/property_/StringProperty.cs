using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    //SILP: PROPERTY_CLASS(String, string)
    [DapVarType(PropertiesConsts.TypeStringProperty, typeof(string))]                    //__SILP__
    [DapOrder(DapOrders.Property)]                                                       //__SILP__
    public sealed class StringProperty : Property<string> {                              //__SILP__
        public StringProperty(IDictProperties owner, string key) : base(owner, key) {    //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        public StringProperty(ITableProperties owner, int index) : base(owner, index) {  //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        protected override Encoder<string> GetEncoder() {                                //__SILP__
            return Encoder.StringEncoder;                                                //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        protected override bool NeedUpdate(string newVal) {                              //__SILP__
            return base.NeedSetup() || (Value != newVal);                                //__SILP__
        }                                                                                //__SILP__
    }                                                                                    //__SILP__
}
