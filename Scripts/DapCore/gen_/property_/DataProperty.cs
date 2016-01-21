using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    //SILP: PROPERTY_CLASS(Data, Data)
    public sealed class DataProperty : Property<Data> {                                            //__SILP__
        public override string Type {                                                              //__SILP__
            get { return PropertiesConsts.TypeDataProperty; }                                      //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        public DataProperty(Properties owner, string path, Pass pass) : base(owner, path, pass) {  //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        protected override bool DoEncode(Data data) {                                              //__SILP__
            return data.SetData(PropertiesConsts.KeyValue, Value);                                 //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        protected override bool DoDecode(Pass pass, Data data) {                                   //__SILP__
            return SetValue(pass, data.GetData(PropertiesConsts.KeyValue));                        //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        protected override bool NeedUpdate(Data newVal) {                                          //__SILP__
            return base.NeedUpdate(newVal) || (Value != newVal);                                   //__SILP__
        }                                                                                          //__SILP__
    }                                                                                              //__SILP__
}
