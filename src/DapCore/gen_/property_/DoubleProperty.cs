using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    //SILP: PROPERTY_CLASS(Double, double)
    [DapVarType(PropertiesConsts.TypeDoubleProperty, typeof(double))]                         //__SILP__
    [DapOrder(DapOrders.Property)]                                                            //__SILP__
    public sealed class DoubleProperty : Property<double> {                                   //__SILP__
        public DoubleProperty(IDictProperties owner, string key) : base(owner, key) {         //__SILP__
        }                                                                                     //__SILP__
                                                                                              //__SILP__
        public DoubleProperty(ITableProperties owner, int index) : base(owner, index) {       //__SILP__
        }                                                                                     //__SILP__
                                                                                              //__SILP__
        protected override Encoder<double> GetEncoder() {                                     //__SILP__
            return Encoder.DoubleEncoder;                                                     //__SILP__
        }                                                                                     //__SILP__
                                                                                              //__SILP__
        protected override bool NeedUpdate(double newVal) {                                   //__SILP__
            return base.NeedSetup() || (Value != newVal);                                     //__SILP__
        }                                                                                     //__SILP__
    }                                                                                         //__SILP__
                                                                                              //__SILP__
    [DapType(PropertiesConsts.TypeDoubleTableProperty)]                                       //__SILP__
    [DapOrder(DapOrders.Property)]                                                            //__SILP__
    public sealed class DoubleTableProperty : TableProperty<DoubleProperty> {                 //__SILP__
        public DoubleTableProperty(IDictProperties owner, string key) : base(owner, key) {    //__SILP__
        }                                                                                     //__SILP__
                                                                                              //__SILP__
        public DoubleTableProperty(ITableProperties owner, int index) : base(owner, index) {  //__SILP__
        }                                                                                     //__SILP__
    }                                                                                         //__SILP__
                                                                                              //__SILP__
    [DapType(PropertiesConsts.TypeDoubleDictProperty)]                                        //__SILP__
    [DapOrder(DapOrders.Property)]                                                            //__SILP__
    public sealed class DoubleDictProperty : DictProperty<DoubleProperty> {                   //__SILP__
        public DoubleDictProperty(IDictProperties owner, string key) : base(owner, key) {     //__SILP__
        }                                                                                     //__SILP__
                                                                                              //__SILP__
        public DoubleDictProperty(ITableProperties owner, int index) : base(owner, index) {   //__SILP__
        }                                                                                     //__SILP__
    }                                                                                         //__SILP__
}
