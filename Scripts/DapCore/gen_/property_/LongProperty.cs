using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    //SILP: PROPERTY_CLASS(Long, long)
    [DapType(PropertiesConsts.TypeLongProperty)]                                       //__SILP__
    [DapOrder(-10)]                                                                    //__SILP__
    public sealed class LongProperty : Property<long> {                                //__SILP__
        public LongProperty(IDictProperties owner, string key) : base(owner, key) {    //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        public LongProperty(ITableProperties owner, int index) : base(owner, index) {  //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        protected override Encoder<long> GetEncoder() {                                //__SILP__
            return Encoder.LongEncoder;                                                //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        protected override bool NeedUpdate(long newVal) {                              //__SILP__
            return base.NeedSetup() || (Value != newVal);                              //__SILP__
        }                                                                              //__SILP__
    }                                                                                  //__SILP__
}
