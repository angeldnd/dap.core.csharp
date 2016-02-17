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
        protected override bool DoEncode(Data data) {                                  //__SILP__
            return data.SetLong(PropertiesConsts.KeyValue, Value);                     //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        protected override bool DoDecode(Data data) {                                  //__SILP__
            return SetValue(data.GetLong(PropertiesConsts.KeyValue));                  //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        protected override bool NeedUpdate(long newVal) {                              //__SILP__
            return base.NeedUpdate(newVal) || (Value != newVal);                       //__SILP__
        }                                                                              //__SILP__
    }                                                                                  //__SILP__
}
