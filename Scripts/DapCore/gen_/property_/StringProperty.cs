using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    //SILP: PROPERTY_CLASS(String, string)
    public sealed class StringProperty : Property<string> {                                          //__SILP__
        public override string Type {                                                                //__SILP__
            get { return PropertiesConsts.TypeStringProperty; }                                      //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        public StringProperty(Properties owner, string path, Pass pass) : base(owner, path, pass) {  //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        protected override bool DoEncode(Data data) {                                                //__SILP__
            return data.SetString(PropertiesConsts.KeyValue, Value);                                 //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        protected override bool DoDecode(Pass pass, Data data) {                                     //__SILP__
            return SetValue(pass, data.GetString(PropertiesConsts.KeyValue));                        //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        protected override bool NeedUpdate(string newVal) {                                          //__SILP__
            return base.NeedUpdate(newVal) || (Value != newVal);                                     //__SILP__
        }                                                                                            //__SILP__
    }                                                                                                //__SILP__
}
