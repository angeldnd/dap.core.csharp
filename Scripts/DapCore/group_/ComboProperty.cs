using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class ComboProperty : GroupProperty {
        protected override bool DoEncode(Data data) {
            bool result = true;
            All<Property>((Property prop) => {
                Data subData = prop.Encode();
                if (subData != null) {
                    if (!data.SetData(prop.Path, subData)) {
                        result = false;
                    }
                } else {
                    result = false;
                }
            });
            return result;
        }

        protected override bool DoDecode(Pass pass, Data data) {
            bool result = true;
            All<Property>((Property prop) => {
                Data subData = data.GetData(prop.Path, null);
                if (subData != null) {
                    if (!prop.Decode(pass, subData)) {
                        result = false;
                    }
                } else {
                    result = false;
                }
            });
            return result;
        }
    }
}
