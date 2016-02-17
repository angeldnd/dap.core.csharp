using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public static class SpecHelper {
        public static Data EncodeWithSpec(this IProperty prop) {
            Data data = prop.Encode();
            if (data != null) {
                Data spec = null;
                prop.AllValueCheckers<ISpecValueChecker>((ISpecValueChecker checker) => {
                    if (spec == null) spec = new Data();
                    checker.DoEncode(spec);
                });
                if (spec != null && spec.Count > 0) {
                    data.SetData(SpecConsts.KeySpec, spec);
                }
            }
            return data;
        }

        public static void SetPropertyWithSpec(this IProperty prop, Data data) {
            if (prop != null) {
                Data spec = data.GetData(SpecConsts.KeySpec, null);
                if (spec != null) {
                    foreach (string key in spec.Keys) {
                        Spec.FactorySpecValueChecker(prop, spec, key);
                    }
                }
            }
        }

        public static IProperty AddPropertyWithSpec(this IDictProperties properties, string key, Data data) {
            IProperty prop = properties.AddProperty(key, data);
            if (prop != null) {
                SetPropertyWithSpec(prop, data);
            }
            return prop;
        }

        public static IProperty AddPropertyWithSpec(this ITableProperties properties, Data data) {
            IProperty prop = properties.AddProperty(data);
            if (prop != null) {
                SetPropertyWithSpec(prop, data);
            }
            return prop;
        }

        public static IProperty AddPropertyWithSpec(this IContext context, string key, Data data) {
            return AddPropertyWithSpec(context.Properties, key, data);
        }
    }
}
