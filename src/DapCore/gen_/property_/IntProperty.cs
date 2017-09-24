using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    //SILP: PROPERTY_CLASS(Int, int)
    [DapVarType(PropertiesConsts.TypeIntProperty, typeof(int))]                            //__SILP__
    [DapOrder(DapOrders.Property)]                                                         //__SILP__
    public sealed class IntProperty : Property<int> {                                      //__SILP__
        public IntProperty(IDictProperties owner, string key) : base(owner, key) {         //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
        public IntProperty(ITableProperties owner, int index) : base(owner, index) {       //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
        protected override Encoder<int> GetEncoder() {                                     //__SILP__
            return Encoder.IntEncoder;                                                     //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
        protected override bool NeedUpdate(int newVal) {                                   //__SILP__
            return base.NeedSetup() || (Value != newVal);                                  //__SILP__
        }                                                                                  //__SILP__
    }                                                                                      //__SILP__
                                                                                           //__SILP__
    [DapType(PropertiesConsts.TypeIntTableProperty)]                                       //__SILP__
    [DapOrder(DapOrders.Property)]                                                         //__SILP__
    public sealed class IntTableProperty : TableProperty<IntProperty> {                    //__SILP__
        public IntTableProperty(IDictProperties owner, string key) : base(owner, key) {    //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
        public IntTableProperty(ITableProperties owner, int index) : base(owner, index) {  //__SILP__
        }                                                                                  //__SILP__
    }                                                                                      //__SILP__
                                                                                           //__SILP__
    [DapType(PropertiesConsts.TypeIntDictProperty)]                                        //__SILP__
    [DapOrder(DapOrders.Property)]                                                         //__SILP__
    public sealed class IntDictProperty : DictProperty<IntProperty> {                      //__SILP__
        public IntDictProperty(IDictProperties owner, string key) : base(owner, key) {     //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
        public IntDictProperty(ITableProperties owner, int index) : base(owner, index) {   //__SILP__
        }                                                                                  //__SILP__
    }                                                                                      //__SILP__
}
