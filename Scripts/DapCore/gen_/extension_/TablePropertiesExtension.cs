using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class TablePropertiesExtension {
        public static IProperty AddProperty(this ITableProperties properties,
                                            Data data) {
            if (data == null) return null;
            string type = data.GetString(ObjectConsts.KeyType);
            if (string.IsNullOrEmpty(type)) {
                properties.Error("Invalid Property data: {0}", data);
                return null;
            }
            IProperty prop = properties.New<IProperty>(type);
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
