using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class TablePropertiesExtension {
        public static T GetProperty<T>(this ITableProperties properties,
                                            string propertyPath, bool isDebug = false)
                                                where T : class, IProperty {
            return TreeHelper.GetDescendant<T>(properties, propertyPath, isDebug);
        }

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
