using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    //SILP: PROPERTY_CLASS(Bool, bool)
    [DapVarType(PropertiesConsts.TypeBoolProperty, typeof(bool))]                      //__SILP__
    [DapOrder(DapOrders.Property)]                                                     //__SILP__
    public sealed class BoolProperty : Property<bool> {                                //__SILP__
        public BoolProperty(IDictProperties owner, string key) : base(owner, key) {    //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        public BoolProperty(ITableProperties owner, int index) : base(owner, index) {  //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        protected override Encoder<bool> GetEncoder() {                                //__SILP__
            return Encoder.BoolEncoder;                                                //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        protected override bool NeedUpdate(bool newVal) {                              //__SILP__
            return base.NeedSetup() || (Value != newVal);                              //__SILP__
        }                                                                              //__SILP__
    }                                                                                  //__SILP__
}
