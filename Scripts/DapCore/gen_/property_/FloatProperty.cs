using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    //SILP: PROPERTY_CLASS(Float, float)
    [DapVarType(PropertiesConsts.TypeFloatProperty, typeof(float))]                          //__SILP__
    [DapOrder(DapOrders.Property)]                                                           //__SILP__
    public sealed class FloatProperty : Property<float> {                                    //__SILP__
        public FloatProperty(IDictProperties owner, string key) : base(owner, key) {         //__SILP__
        }                                                                                    //__SILP__
                                                                                             //__SILP__
        public FloatProperty(ITableProperties owner, int index) : base(owner, index) {       //__SILP__
        }                                                                                    //__SILP__
                                                                                             //__SILP__
        protected override Encoder<float> GetEncoder() {                                     //__SILP__
            return Encoder.FloatEncoder;                                                     //__SILP__
        }                                                                                    //__SILP__
                                                                                             //__SILP__
        protected override bool NeedUpdate(float newVal) {                                   //__SILP__
            return base.NeedSetup() || (Value != newVal);                                    //__SILP__
        }                                                                                    //__SILP__
    }                                                                                        //__SILP__
                                                                                             //__SILP__
    [DapType(PropertiesConsts.TypeFloatTableProperty)]                                       //__SILP__
    [DapOrder(DapOrders.Property)]                                                           //__SILP__
    public sealed class FloatTableProperty : TableProperty<FloatProperty> {                  //__SILP__
        public FloatTableProperty(IDictProperties owner, string key) : base(owner, key) {    //__SILP__
        }                                                                                    //__SILP__
                                                                                             //__SILP__
        public FloatTableProperty(ITableProperties owner, int index) : base(owner, index) {  //__SILP__
        }                                                                                    //__SILP__
    }                                                                                        //__SILP__
                                                                                             //__SILP__
    [DapType(PropertiesConsts.TypeFloatDictProperty)]                                        //__SILP__
    [DapOrder(DapOrders.Property)]                                                           //__SILP__
    public sealed class FloatDictProperty : DictProperty<FloatProperty> {                    //__SILP__
        public FloatDictProperty(IDictProperties owner, string key) : base(owner, key) {     //__SILP__
        }                                                                                    //__SILP__
                                                                                             //__SILP__
        public FloatDictProperty(ITableProperties owner, int index) : base(owner, index) {   //__SILP__
        }                                                                                    //__SILP__
    }                                                                                        //__SILP__
}
