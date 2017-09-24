using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    //SILP: PROPERTY_CLASS(Long, long)
    [DapVarType(PropertiesConsts.TypeLongProperty, typeof(long))]                           //__SILP__
    [DapOrder(DapOrders.Property)]                                                          //__SILP__
    public sealed class LongProperty : Property<long> {                                     //__SILP__
        public LongProperty(IDictProperties owner, string key) : base(owner, key) {         //__SILP__
        }                                                                                   //__SILP__
                                                                                            //__SILP__
        public LongProperty(ITableProperties owner, int index) : base(owner, index) {       //__SILP__
        }                                                                                   //__SILP__
                                                                                            //__SILP__
        protected override Encoder<long> GetEncoder() {                                     //__SILP__
            return Encoder.LongEncoder;                                                     //__SILP__
        }                                                                                   //__SILP__
                                                                                            //__SILP__
        protected override bool NeedUpdate(long newVal) {                                   //__SILP__
            return base.NeedSetup() || (Value != newVal);                                   //__SILP__
        }                                                                                   //__SILP__
    }                                                                                       //__SILP__
                                                                                            //__SILP__
    [DapType(PropertiesConsts.TypeLongTableProperty)]                                       //__SILP__
    [DapOrder(DapOrders.Property)]                                                          //__SILP__
    public sealed class LongTableProperty : TableProperty<LongProperty> {                   //__SILP__
        public LongTableProperty(IDictProperties owner, string key) : base(owner, key) {    //__SILP__
        }                                                                                   //__SILP__
                                                                                            //__SILP__
        public LongTableProperty(ITableProperties owner, int index) : base(owner, index) {  //__SILP__
        }                                                                                   //__SILP__
    }                                                                                       //__SILP__
                                                                                            //__SILP__
    [DapType(PropertiesConsts.TypeLongDictProperty)]                                        //__SILP__
    [DapOrder(DapOrders.Property)]                                                          //__SILP__
    public sealed class LongDictProperty : DictProperty<LongProperty> {                     //__SILP__
        public LongDictProperty(IDictProperties owner, string key) : base(owner, key) {     //__SILP__
        }                                                                                   //__SILP__
                                                                                            //__SILP__
        public LongDictProperty(ITableProperties owner, int index) : base(owner, index) {   //__SILP__
        }                                                                                   //__SILP__
    }                                                                                       //__SILP__
}
