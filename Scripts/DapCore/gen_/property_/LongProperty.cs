using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    //SILP: PROPERTY_CLASS(Long, long)
    public sealed class LongProperty : Property<long> {                                            //__SILP__
        public override string Type {                                                              //__SILP__
            get { return PropertiesConsts.TypeLongProperty; }                                      //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        public LongProperty(Properties owner, string path, Pass pass) : base(owner, path, pass) {  //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        protected override bool DoEncode(Data data) {                                              //__SILP__
            return data.SetLong(PropertiesConsts.KeyValue, Value);                                 //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        protected override bool DoDecode(Pass pass, Data data) {                                   //__SILP__
            return SetValue(pass, data.GetLong(PropertiesConsts.KeyValue));                        //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        protected override bool NeedUpdate(long newVal) {                                          //__SILP__
            return base.NeedUpdate(newVal) || (Value != newVal);                                   //__SILP__
        }                                                                                          //__SILP__
    }                                                                                              //__SILP__
}
