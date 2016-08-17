using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    //SILP: PROPERTY_CLASS(Data, Data)
    [DapVarType(PropertiesConsts.TypeDataProperty, typeof(Data))]                      //__SILP__
    [DapOrder(DapOrders.Property)]                                                     //__SILP__
    public sealed class DataProperty : Property<Data> {                                //__SILP__
        public DataProperty(IDictProperties owner, string key) : base(owner, key) {    //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        public DataProperty(ITableProperties owner, int index) : base(owner, index) {  //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        protected override Encoder<Data> GetEncoder() {                                //__SILP__
            return Encoder.DataEncoder;                                                //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        protected override bool NeedUpdate(Data newVal) {                              //__SILP__
            return base.NeedSetup() || (Value != newVal);                              //__SILP__
        }                                                                              //__SILP__
    }                                                                                  //__SILP__
}
