using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    //SILP: PROPERTY_CLASS(Float, float)
    [DapType(PropertiesConsts.TypeFloatProperty)]                                       //__SILP__
    [DapOrder(-10)]                                                                     //__SILP__
    public sealed class FloatProperty : Property<float> {                               //__SILP__
        public FloatProperty(IDictProperties owner, string key) : base(owner, key) {    //__SILP__
        }                                                                               //__SILP__
                                                                                        //__SILP__
        public FloatProperty(ITableProperties owner, int index) : base(owner, index) {  //__SILP__
        }                                                                               //__SILP__
                                                                                        //__SILP__
        protected override Encoder<float> GetEncoder() {                                //__SILP__
            return Encoder.FloatEncoder;                                                //__SILP__
        }                                                                               //__SILP__
                                                                                        //__SILP__
        protected override bool NeedUpdate(float newVal) {                              //__SILP__
            return base.NeedSetup() || (Value != newVal);                               //__SILP__
        }                                                                               //__SILP__
    }                                                                                   //__SILP__
}
