using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    //SILP: PROPERTY_CLASS(Double, double)
    [DapType(PropertiesConsts.TypeDoubleProperty)]                                       //__SILP__
    [DapOrder(-10)]                                                                      //__SILP__
    public sealed class DoubleProperty : BaseProperty<double> {                          //__SILP__
        public DoubleProperty(IDictProperties owner, string key) : base(owner, key) {    //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        public DoubleProperty(ITableProperties owner, int index) : base(owner, index) {  //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        protected override bool DoEncode(Data data) {                                    //__SILP__
            return data.SetDouble(PropertiesConsts.KeyValue, Value);                     //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        protected override bool DoDecode(Data data) {                                    //__SILP__
            return SetValue(data.GetDouble(PropertiesConsts.KeyValue));                  //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        protected override bool NeedUpdate(double newVal) {                              //__SILP__
            return base.NeedUpdate(newVal) || (Value != newVal);                         //__SILP__
        }                                                                                //__SILP__
    }                                                                                    //__SILP__
}
