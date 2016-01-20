using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    //SILP: PROPERTY_CLASS(Float, float)
    public class FloatProperty : Property<float> {                                                  //__SILP__
        public override string Type {                                                               //__SILP__
            get { return PropertiesConsts.TypeFloatProperty; }                                      //__SILP__
        }                                                                                           //__SILP__
                                                                                                    //__SILP__
        public FloatProperty(Properties owner, string path, Pass pass) : base(owner, path, pass) {  //__SILP__
        }                                                                                           //__SILP__
                                                                                                    //__SILP__
        protected override bool DoEncode(Data data) {                                               //__SILP__
            return data.SetFloat(PropertiesConsts.KeyValue, Value);                                 //__SILP__
        }                                                                                           //__SILP__
                                                                                                    //__SILP__
        protected override bool DoDecode(Pass pass, Data data) {                                    //__SILP__
            return SetValue(pass, data.GetFloat(PropertiesConsts.KeyValue));                        //__SILP__
        }                                                                                           //__SILP__
                                                                                                    //__SILP__
        protected override bool NeedUpdate(float newVal) {                                          //__SILP__
            return base.NeedUpdate(newVal) || (Value != newVal);                                    //__SILP__
        }                                                                                           //__SILP__
    }                                                                                               //__SILP__
}
