using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public static class SpecHelper {
        public static Data EncodeWithSpec<T>(IProperty<T> prop) {
            Data data = prop.Encode();
            if (data != null) {
                Data spec = null;
                prop.AllValueCheckers<SpecValueChecker<T>>((SpecValueChecker<T> checker) => {
                    if (spec == null) spec = new Data();
                    checker.DoEncode(spec);
                });
                if (spec != null && spec.Count > 0) {
                    data.SetData(SpecConsts.KeySpec, spec);
                }
            }
            return data;
        }

        public static void SetPropertyWithSpec(IProperty prop, Pass pass, Data data) {
            if (prop != null) {
                Data spec = data.GetData(SpecConsts.KeySpec, null);
                if (spec != null) {
                    foreach (string key in spec.Keys) {
                        Spec.FactorySpecValueChecker(prop, pass, spec, key);
                    }
                }
            }
        }

        public static IProperty AddPropertyWithSpec(ITreeProperties properties, string path, Pass pass, bool open, Data data) {
            IProperty prop = properties.AddProperty(path, pass, open, data);
            if (prop != null) {
                SetPropertyWithSpec(prop, pass, data);
            }
            return prop;
        }

        public static IProperty AddPropertyWithSpec(IContext context, string path, Pass pass, bool open, Data data) {
            return AddPropertyWithSpec(context.Properties, path, pass, open, data);
        }
    }
}
