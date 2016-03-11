using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class TablePropertiesExtension {
        public static IProperty AddProperty(this ITableProperties properties,
                                            Data data) {
            if (data == null) return null;
            string dapType = data.GetString(ObjectConsts.KeyDapType);
            if (string.IsNullOrEmpty(dapType)) {
                properties.Error("Invalid Property data: {0}", data);
                return null;
            }
            IProperty prop = properties.New<IProperty>(dapType);
            if (prop == null) {
                properties.Error("Failed to Add Property: {0}", data);
                return null;
            }
            if (!prop.Decode(data)) {
                properties.Error("Failed to Decode Property: {0}, {1} -> {2}", prop.Index, data, prop);
            }

            return prop;
        }
    }
}
