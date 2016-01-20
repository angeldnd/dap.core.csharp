using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public static class BuiltInSpecFactory {
        public static void RegistrySpecValueCheckers() {
            //SILP:REGISTER_SPEC_HELPER(Int, int);
            Spec.RegisterSpecValueChecker(PropertiesConsts.TypeIntProperty, SpecConsts.KindBigger,               //__SILP__
                    (IProperty _prop, Pass pass, Data spec, string specKey) => {                                 //__SILP__
                if (spec == null) return false;                                                                  //__SILP__
                IntProperty prop = _prop as IntProperty;                                                         //__SILP__
                if (prop == null) return false;                                                                  //__SILP__
                return prop.AddValueChecker(pass, new IntSpecValueCheckerBigger(spec.GetInt(specKey)));          //__SILP__
            });                                                                                                  //__SILP__
                                                                                                                 //__SILP__
            Spec.RegisterSpecValueChecker(PropertiesConsts.TypeIntProperty, SpecConsts.KindBiggerOrEqual,        //__SILP__
                    (IProperty _prop, Pass pass, Data spec, string specKey) => {                                 //__SILP__
                if (spec == null) return false;                                                                  //__SILP__
                IntProperty prop = _prop as IntProperty;                                                         //__SILP__
                if (prop == null) return false;                                                                  //__SILP__
                return prop.AddValueChecker(pass, new IntSpecValueCheckerBiggerOrEqual(spec.GetInt(specKey)));   //__SILP__
            });                                                                                                  //__SILP__
                                                                                                                 //__SILP__
            Spec.RegisterSpecValueChecker(PropertiesConsts.TypeIntProperty, SpecConsts.KindSmaller,              //__SILP__
                    (IProperty _prop, Pass pass, Data spec, string specKey) => {                                 //__SILP__
                if (spec == null) return false;                                                                  //__SILP__
                IntProperty prop = _prop as IntProperty;                                                         //__SILP__
                if (prop == null) return false;                                                                  //__SILP__
                return prop.AddValueChecker(pass, new IntSpecValueCheckerSmaller(spec.GetInt(specKey)));         //__SILP__
            });                                                                                                  //__SILP__
                                                                                                                 //__SILP__
            Spec.RegisterSpecValueChecker(PropertiesConsts.TypeIntProperty, SpecConsts.KindSmallerOrEqual,       //__SILP__
                    (IProperty _prop, Pass pass, Data spec, string specKey) => {                                 //__SILP__
                if (spec == null) return false;                                                                  //__SILP__
                IntProperty prop = _prop as IntProperty;                                                         //__SILP__
                if (prop == null) return false;                                                                  //__SILP__
                return prop.AddValueChecker(pass, new IntSpecValueCheckerSmallerOrEqual(spec.GetInt(specKey)));  //__SILP__
            });                                                                                                  //__SILP__
                                                                                                                 //__SILP__
            //SILP:REGISTER_SPEC_IN_HELPER(Int, int);
            Spec.RegisterSpecValueChecker(PropertiesConsts.TypeIntProperty, SpecConsts.KindIn,      //__SILP__
                    (IProperty _prop, Pass pass, Data spec, string specKey) => {                    //__SILP__
                if (spec == null) return false;                                                     //__SILP__
                IntProperty prop = _prop as IntProperty;                                            //__SILP__
                if (prop == null) return false;                                                     //__SILP__
                Data _values = spec.GetData(specKey, null);                                         //__SILP__
                if (_values == null) return false;                                                  //__SILP__
                List<int> values = new List<int>();                                                 //__SILP__
                for (int i = 0; i < _values.Count; i++) {                                           //__SILP__
                    string index = i.ToString();                                                    //__SILP__
                    if (_values.IsInt(index)) {                                                     //__SILP__
                        values.Add(_values.GetInt(index));                                          //__SILP__
                    }                                                                               //__SILP__
                }                                                                                   //__SILP__
                return prop.AddValueChecker(pass, new IntSpecValueCheckerIn(values.ToArray()));     //__SILP__
            });                                                                                     //__SILP__
                                                                                                    //__SILP__
            Spec.RegisterSpecValueChecker(PropertiesConsts.TypeIntProperty, SpecConsts.KindNotIn,   //__SILP__
                    (IProperty _prop, Pass pass, Data spec, string specKey) => {                    //__SILP__
                if (spec == null) return false;                                                     //__SILP__
                IntProperty prop = _prop as IntProperty;                                            //__SILP__
                if (prop == null) return false;                                                     //__SILP__
                Data _values = spec.GetData(specKey, null);                                         //__SILP__
                if (_values == null) return false;                                                  //__SILP__
                List<int> values = new List<int>();                                                 //__SILP__
                for (int i = 0; i < _values.Count; i++) {                                           //__SILP__
                    string index = i.ToString();                                                    //__SILP__
                    if (_values.IsInt(index)) {                                                     //__SILP__
                        values.Add(_values.GetInt(index));                                          //__SILP__
                    }                                                                               //__SILP__
                }                                                                                   //__SILP__
                return prop.AddValueChecker(pass, new IntSpecValueCheckerNotIn(values.ToArray()));  //__SILP__
            });                                                                                     //__SILP__
                                                                                                    //__SILP__
            //SILP:REGISTER_SPEC_HELPER(Long, long);
            Spec.RegisterSpecValueChecker(PropertiesConsts.TypeLongProperty, SpecConsts.KindBigger,                //__SILP__
                    (IProperty _prop, Pass pass, Data spec, string specKey) => {                                   //__SILP__
                if (spec == null) return false;                                                                    //__SILP__
                LongProperty prop = _prop as LongProperty;                                                         //__SILP__
                if (prop == null) return false;                                                                    //__SILP__
                return prop.AddValueChecker(pass, new LongSpecValueCheckerBigger(spec.GetLong(specKey)));          //__SILP__
            });                                                                                                    //__SILP__
                                                                                                                   //__SILP__
            Spec.RegisterSpecValueChecker(PropertiesConsts.TypeLongProperty, SpecConsts.KindBiggerOrEqual,         //__SILP__
                    (IProperty _prop, Pass pass, Data spec, string specKey) => {                                   //__SILP__
                if (spec == null) return false;                                                                    //__SILP__
                LongProperty prop = _prop as LongProperty;                                                         //__SILP__
                if (prop == null) return false;                                                                    //__SILP__
                return prop.AddValueChecker(pass, new LongSpecValueCheckerBiggerOrEqual(spec.GetLong(specKey)));   //__SILP__
            });                                                                                                    //__SILP__
                                                                                                                   //__SILP__
            Spec.RegisterSpecValueChecker(PropertiesConsts.TypeLongProperty, SpecConsts.KindSmaller,               //__SILP__
                    (IProperty _prop, Pass pass, Data spec, string specKey) => {                                   //__SILP__
                if (spec == null) return false;                                                                    //__SILP__
                LongProperty prop = _prop as LongProperty;                                                         //__SILP__
                if (prop == null) return false;                                                                    //__SILP__
                return prop.AddValueChecker(pass, new LongSpecValueCheckerSmaller(spec.GetLong(specKey)));         //__SILP__
            });                                                                                                    //__SILP__
                                                                                                                   //__SILP__
            Spec.RegisterSpecValueChecker(PropertiesConsts.TypeLongProperty, SpecConsts.KindSmallerOrEqual,        //__SILP__
                    (IProperty _prop, Pass pass, Data spec, string specKey) => {                                   //__SILP__
                if (spec == null) return false;                                                                    //__SILP__
                LongProperty prop = _prop as LongProperty;                                                         //__SILP__
                if (prop == null) return false;                                                                    //__SILP__
                return prop.AddValueChecker(pass, new LongSpecValueCheckerSmallerOrEqual(spec.GetLong(specKey)));  //__SILP__
            });                                                                                                    //__SILP__
                                                                                                                   //__SILP__
            //SILP:REGISTER_SPEC_IN_HELPER(Long, long);
            Spec.RegisterSpecValueChecker(PropertiesConsts.TypeLongProperty, SpecConsts.KindIn,      //__SILP__
                    (IProperty _prop, Pass pass, Data spec, string specKey) => {                     //__SILP__
                if (spec == null) return false;                                                      //__SILP__
                LongProperty prop = _prop as LongProperty;                                           //__SILP__
                if (prop == null) return false;                                                      //__SILP__
                Data _values = spec.GetData(specKey, null);                                          //__SILP__
                if (_values == null) return false;                                                   //__SILP__
                List<long> values = new List<long>();                                                //__SILP__
                for (int i = 0; i < _values.Count; i++) {                                            //__SILP__
                    string index = i.ToString();                                                     //__SILP__
                    if (_values.IsLong(index)) {                                                     //__SILP__
                        values.Add(_values.GetLong(index));                                          //__SILP__
                    }                                                                                //__SILP__
                }                                                                                    //__SILP__
                return prop.AddValueChecker(pass, new LongSpecValueCheckerIn(values.ToArray()));     //__SILP__
            });                                                                                      //__SILP__
                                                                                                     //__SILP__
            Spec.RegisterSpecValueChecker(PropertiesConsts.TypeLongProperty, SpecConsts.KindNotIn,   //__SILP__
                    (IProperty _prop, Pass pass, Data spec, string specKey) => {                     //__SILP__
                if (spec == null) return false;                                                      //__SILP__
                LongProperty prop = _prop as LongProperty;                                           //__SILP__
                if (prop == null) return false;                                                      //__SILP__
                Data _values = spec.GetData(specKey, null);                                          //__SILP__
                if (_values == null) return false;                                                   //__SILP__
                List<long> values = new List<long>();                                                //__SILP__
                for (int i = 0; i < _values.Count; i++) {                                            //__SILP__
                    string index = i.ToString();                                                     //__SILP__
                    if (_values.IsLong(index)) {                                                     //__SILP__
                        values.Add(_values.GetLong(index));                                          //__SILP__
                    }                                                                                //__SILP__
                }                                                                                    //__SILP__
                return prop.AddValueChecker(pass, new LongSpecValueCheckerNotIn(values.ToArray()));  //__SILP__
            });                                                                                      //__SILP__
                                                                                                     //__SILP__
            //SILP:REGISTER_SPEC_HELPER(Float, float);
            Spec.RegisterSpecValueChecker(PropertiesConsts.TypeFloatProperty, SpecConsts.KindBigger,                 //__SILP__
                    (IProperty _prop, Pass pass, Data spec, string specKey) => {                                     //__SILP__
                if (spec == null) return false;                                                                      //__SILP__
                FloatProperty prop = _prop as FloatProperty;                                                         //__SILP__
                if (prop == null) return false;                                                                      //__SILP__
                return prop.AddValueChecker(pass, new FloatSpecValueCheckerBigger(spec.GetFloat(specKey)));          //__SILP__
            });                                                                                                      //__SILP__
                                                                                                                     //__SILP__
            Spec.RegisterSpecValueChecker(PropertiesConsts.TypeFloatProperty, SpecConsts.KindBiggerOrEqual,          //__SILP__
                    (IProperty _prop, Pass pass, Data spec, string specKey) => {                                     //__SILP__
                if (spec == null) return false;                                                                      //__SILP__
                FloatProperty prop = _prop as FloatProperty;                                                         //__SILP__
                if (prop == null) return false;                                                                      //__SILP__
                return prop.AddValueChecker(pass, new FloatSpecValueCheckerBiggerOrEqual(spec.GetFloat(specKey)));   //__SILP__
            });                                                                                                      //__SILP__
                                                                                                                     //__SILP__
            Spec.RegisterSpecValueChecker(PropertiesConsts.TypeFloatProperty, SpecConsts.KindSmaller,                //__SILP__
                    (IProperty _prop, Pass pass, Data spec, string specKey) => {                                     //__SILP__
                if (spec == null) return false;                                                                      //__SILP__
                FloatProperty prop = _prop as FloatProperty;                                                         //__SILP__
                if (prop == null) return false;                                                                      //__SILP__
                return prop.AddValueChecker(pass, new FloatSpecValueCheckerSmaller(spec.GetFloat(specKey)));         //__SILP__
            });                                                                                                      //__SILP__
                                                                                                                     //__SILP__
            Spec.RegisterSpecValueChecker(PropertiesConsts.TypeFloatProperty, SpecConsts.KindSmallerOrEqual,         //__SILP__
                    (IProperty _prop, Pass pass, Data spec, string specKey) => {                                     //__SILP__
                if (spec == null) return false;                                                                      //__SILP__
                FloatProperty prop = _prop as FloatProperty;                                                         //__SILP__
                if (prop == null) return false;                                                                      //__SILP__
                return prop.AddValueChecker(pass, new FloatSpecValueCheckerSmallerOrEqual(spec.GetFloat(specKey)));  //__SILP__
            });                                                                                                      //__SILP__
                                                                                                                     //__SILP__
            //SILP:REGISTER_SPEC_HELPER(Double, double);
            Spec.RegisterSpecValueChecker(PropertiesConsts.TypeDoubleProperty, SpecConsts.KindBigger,                  //__SILP__
                    (IProperty _prop, Pass pass, Data spec, string specKey) => {                                       //__SILP__
                if (spec == null) return false;                                                                        //__SILP__
                DoubleProperty prop = _prop as DoubleProperty;                                                         //__SILP__
                if (prop == null) return false;                                                                        //__SILP__
                return prop.AddValueChecker(pass, new DoubleSpecValueCheckerBigger(spec.GetDouble(specKey)));          //__SILP__
            });                                                                                                        //__SILP__
                                                                                                                       //__SILP__
            Spec.RegisterSpecValueChecker(PropertiesConsts.TypeDoubleProperty, SpecConsts.KindBiggerOrEqual,           //__SILP__
                    (IProperty _prop, Pass pass, Data spec, string specKey) => {                                       //__SILP__
                if (spec == null) return false;                                                                        //__SILP__
                DoubleProperty prop = _prop as DoubleProperty;                                                         //__SILP__
                if (prop == null) return false;                                                                        //__SILP__
                return prop.AddValueChecker(pass, new DoubleSpecValueCheckerBiggerOrEqual(spec.GetDouble(specKey)));   //__SILP__
            });                                                                                                        //__SILP__
                                                                                                                       //__SILP__
            Spec.RegisterSpecValueChecker(PropertiesConsts.TypeDoubleProperty, SpecConsts.KindSmaller,                 //__SILP__
                    (IProperty _prop, Pass pass, Data spec, string specKey) => {                                       //__SILP__
                if (spec == null) return false;                                                                        //__SILP__
                DoubleProperty prop = _prop as DoubleProperty;                                                         //__SILP__
                if (prop == null) return false;                                                                        //__SILP__
                return prop.AddValueChecker(pass, new DoubleSpecValueCheckerSmaller(spec.GetDouble(specKey)));         //__SILP__
            });                                                                                                        //__SILP__
                                                                                                                       //__SILP__
            Spec.RegisterSpecValueChecker(PropertiesConsts.TypeDoubleProperty, SpecConsts.KindSmallerOrEqual,          //__SILP__
                    (IProperty _prop, Pass pass, Data spec, string specKey) => {                                       //__SILP__
                if (spec == null) return false;                                                                        //__SILP__
                DoubleProperty prop = _prop as DoubleProperty;                                                         //__SILP__
                if (prop == null) return false;                                                                        //__SILP__
                return prop.AddValueChecker(pass, new DoubleSpecValueCheckerSmallerOrEqual(spec.GetDouble(specKey)));  //__SILP__
            });                                                                                                        //__SILP__
                                                                                                                       //__SILP__
            //SILP:REGISTER_SPEC_IN_HELPER(String, string);
            Spec.RegisterSpecValueChecker(PropertiesConsts.TypeStringProperty, SpecConsts.KindIn,      //__SILP__
                    (IProperty _prop, Pass pass, Data spec, string specKey) => {                       //__SILP__
                if (spec == null) return false;                                                        //__SILP__
                StringProperty prop = _prop as StringProperty;                                         //__SILP__
                if (prop == null) return false;                                                        //__SILP__
                Data _values = spec.GetData(specKey, null);                                            //__SILP__
                if (_values == null) return false;                                                     //__SILP__
                List<string> values = new List<string>();                                              //__SILP__
                for (int i = 0; i < _values.Count; i++) {                                              //__SILP__
                    string index = i.ToString();                                                       //__SILP__
                    if (_values.IsString(index)) {                                                     //__SILP__
                        values.Add(_values.GetString(index));                                          //__SILP__
                    }                                                                                  //__SILP__
                }                                                                                      //__SILP__
                return prop.AddValueChecker(pass, new StringSpecValueCheckerIn(values.ToArray()));     //__SILP__
            });                                                                                        //__SILP__
                                                                                                       //__SILP__
            Spec.RegisterSpecValueChecker(PropertiesConsts.TypeStringProperty, SpecConsts.KindNotIn,   //__SILP__
                    (IProperty _prop, Pass pass, Data spec, string specKey) => {                       //__SILP__
                if (spec == null) return false;                                                        //__SILP__
                StringProperty prop = _prop as StringProperty;                                         //__SILP__
                if (prop == null) return false;                                                        //__SILP__
                Data _values = spec.GetData(specKey, null);                                            //__SILP__
                if (_values == null) return false;                                                     //__SILP__
                List<string> values = new List<string>();                                              //__SILP__
                for (int i = 0; i < _values.Count; i++) {                                              //__SILP__
                    string index = i.ToString();                                                       //__SILP__
                    if (_values.IsString(index)) {                                                     //__SILP__
                        values.Add(_values.GetString(index));                                          //__SILP__
                    }                                                                                  //__SILP__
                }                                                                                      //__SILP__
                return prop.AddValueChecker(pass, new StringSpecValueCheckerNotIn(values.ToArray()));  //__SILP__
            });                                                                                        //__SILP__
                                                                                                       //__SILP__
            //SILP:REGISTER_SPEC_DATA_HELPER();
            Spec.RegisterSpecValueChecker(PropertiesConsts.TypeDataProperty, SpecConsts.KindBigger,                              //__SILP__
                    (IProperty _prop, Pass pass, Data spec, string specKey) => {                                                 //__SILP__
                if (spec == null) return false;                                                                                  //__SILP__
                DataProperty prop = _prop as DataProperty;                                                                       //__SILP__
                if (prop == null) return false;                                                                                  //__SILP__
                string subKey = SpecConsts.GetSubKey(specKey);                                                                   //__SILP__
                if (subKey == null) return false;                                                                                //__SILP__
                DataType valueType = spec.GetValueType(specKey);                                                                 //__SILP__
                switch (valueType) {                                                                                             //__SILP__
                    case DataType.Int:                                                                                           //__SILP__
                        return prop.AddValueChecker(pass,                                                                        //__SILP__
                                new DataIntSpecValueCheckerBigger(subKey, spec.GetInt(specKey)));                                //__SILP__
                    case DataType.Long:                                                                                          //__SILP__
                        return prop.AddValueChecker(pass,                                                                        //__SILP__
                                new DataLongSpecValueCheckerBigger(subKey, spec.GetLong(specKey)));                              //__SILP__
                    case DataType.Float:                                                                                         //__SILP__
                        return prop.AddValueChecker(pass,                                                                        //__SILP__
                                new DataFloatSpecValueCheckerBigger(subKey, spec.GetFloat(specKey)));                            //__SILP__
                    case DataType.Double:                                                                                        //__SILP__
                        return prop.AddValueChecker(pass,                                                                        //__SILP__
                                new DataDoubleSpecValueCheckerBigger(subKey, spec.GetDouble(specKey)));                          //__SILP__
                }                                                                                                                //__SILP__
                return false;                                                                                                    //__SILP__
            });                                                                                                                  //__SILP__
                                                                                                                                 //__SILP__
            Spec.RegisterSpecValueChecker(PropertiesConsts.TypeDataProperty, SpecConsts.KindBiggerOrEqual,                       //__SILP__
                    (IProperty _prop, Pass pass, Data spec, string specKey) => {                                                 //__SILP__
                if (spec == null) return false;                                                                                  //__SILP__
                DataProperty prop = _prop as DataProperty;                                                                       //__SILP__
                if (prop == null) return false;                                                                                  //__SILP__
                string subKey = SpecConsts.GetSubKey(specKey);                                                                   //__SILP__
                if (subKey == null) return false;                                                                                //__SILP__
                DataType valueType = spec.GetValueType(specKey);                                                                 //__SILP__
                switch (valueType) {                                                                                             //__SILP__
                    case DataType.Int:                                                                                           //__SILP__
                        return prop.AddValueChecker(pass,                                                                        //__SILP__
                                new DataIntSpecValueCheckerBiggerOrEqual(subKey, spec.GetInt(specKey)));                         //__SILP__
                    case DataType.Long:                                                                                          //__SILP__
                        return prop.AddValueChecker(pass,                                                                        //__SILP__
                                new DataLongSpecValueCheckerBiggerOrEqual(subKey, spec.GetLong(specKey)));                       //__SILP__
                    case DataType.Float:                                                                                         //__SILP__
                        return prop.AddValueChecker(pass,                                                                        //__SILP__
                                new DataFloatSpecValueCheckerBiggerOrEqual(subKey, spec.GetFloat(specKey)));                     //__SILP__
                    case DataType.Double:                                                                                        //__SILP__
                        return prop.AddValueChecker(pass,                                                                        //__SILP__
                                new DataDoubleSpecValueCheckerBiggerOrEqual(subKey, spec.GetDouble(specKey)));                   //__SILP__
                }                                                                                                                //__SILP__
                return false;                                                                                                    //__SILP__
            });                                                                                                                  //__SILP__
                                                                                                                                 //__SILP__
            Spec.RegisterSpecValueChecker(PropertiesConsts.TypeDataProperty, SpecConsts.KindSmaller,                             //__SILP__
                    (IProperty _prop, Pass pass, Data spec, string specKey) => {                                                 //__SILP__
                if (spec == null) return false;                                                                                  //__SILP__
                DataProperty prop = _prop as DataProperty;                                                                       //__SILP__
                if (prop == null) return false;                                                                                  //__SILP__
                string subKey = SpecConsts.GetSubKey(specKey);                                                                   //__SILP__
                if (subKey == null) return false;                                                                                //__SILP__
                DataType valueType = spec.GetValueType(specKey);                                                                 //__SILP__
                switch (valueType) {                                                                                             //__SILP__
                    case DataType.Int:                                                                                           //__SILP__
                        return prop.AddValueChecker(pass,                                                                        //__SILP__
                                new DataIntSpecValueCheckerSmaller(subKey, spec.GetInt(specKey)));                               //__SILP__
                    case DataType.Long:                                                                                          //__SILP__
                        return prop.AddValueChecker(pass,                                                                        //__SILP__
                                new DataLongSpecValueCheckerSmaller(subKey, spec.GetLong(specKey)));                             //__SILP__
                    case DataType.Float:                                                                                         //__SILP__
                        return prop.AddValueChecker(pass,                                                                        //__SILP__
                                new DataFloatSpecValueCheckerSmaller(subKey, spec.GetFloat(specKey)));                           //__SILP__
                    case DataType.Double:                                                                                        //__SILP__
                        return prop.AddValueChecker(pass,                                                                        //__SILP__
                                new DataDoubleSpecValueCheckerSmaller(subKey, spec.GetDouble(specKey)));                         //__SILP__
                }                                                                                                                //__SILP__
                return false;                                                                                                    //__SILP__
            });                                                                                                                  //__SILP__
                                                                                                                                 //__SILP__
            Spec.RegisterSpecValueChecker(PropertiesConsts.TypeDataProperty, SpecConsts.KindSmallerOrEqual,                      //__SILP__
                    (IProperty _prop, Pass pass, Data spec, string specKey) => {                                                 //__SILP__
                if (spec == null) return false;                                                                                  //__SILP__
                DataProperty prop = _prop as DataProperty;                                                                       //__SILP__
                if (prop == null) return false;                                                                                  //__SILP__
                string subKey = SpecConsts.GetSubKey(specKey);                                                                   //__SILP__
                if (subKey == null) return false;                                                                                //__SILP__
                DataType valueType = spec.GetValueType(specKey);                                                                 //__SILP__
                switch (valueType) {                                                                                             //__SILP__
                    case DataType.Int:                                                                                           //__SILP__
                        return prop.AddValueChecker(pass,                                                                        //__SILP__
                                new DataIntSpecValueCheckerSmallerOrEqual(subKey, spec.GetInt(specKey)));                        //__SILP__
                    case DataType.Long:                                                                                          //__SILP__
                        return prop.AddValueChecker(pass,                                                                        //__SILP__
                                new DataLongSpecValueCheckerSmallerOrEqual(subKey, spec.GetLong(specKey)));                      //__SILP__
                    case DataType.Float:                                                                                         //__SILP__
                        return prop.AddValueChecker(pass,                                                                        //__SILP__
                                new DataFloatSpecValueCheckerSmallerOrEqual(subKey, spec.GetFloat(specKey)));                    //__SILP__
                    case DataType.Double:                                                                                        //__SILP__
                        return prop.AddValueChecker(pass,                                                                        //__SILP__
                                new DataDoubleSpecValueCheckerSmallerOrEqual(subKey, spec.GetDouble(specKey)));                  //__SILP__
                }                                                                                                                //__SILP__
                return false;                                                                                                    //__SILP__
            });                                                                                                                  //__SILP__
                                                                                                                                 //__SILP__
            Spec.RegisterSpecValueChecker(PropertiesConsts.TypeDataProperty, SpecConsts.KindIn,                                  //__SILP__
                    (IProperty _prop, Pass pass, Data spec, string specKey) => {                                                 //__SILP__
                if (spec == null) return false;                                                                                  //__SILP__
                DataProperty prop = _prop as DataProperty;                                                                       //__SILP__
                if (prop == null) return false;                                                                                  //__SILP__
                string subKey = SpecConsts.GetSubKey(specKey);                                                                   //__SILP__
                if (subKey == null) return false;                                                                                //__SILP__
                Data _values = spec.GetData(specKey, null);                                                                      //__SILP__
                if (_values == null) return false;                                                                               //__SILP__
                DataType valueType = _values.GetValueType(0.ToString());                                                         //__SILP__
                switch (valueType) {                                                                                             //__SILP__
                    case DataType.Int:                                                                                           //__SILP__
                        List<int> intValues = new List<int>();                                                                   //__SILP__
                        for (int i = 0; i < _values.Count; i++) {                                                                //__SILP__
                            string index = i.ToString();                                                                         //__SILP__
                            if (_values.IsInt(index)) {                                                                          //__SILP__
                                intValues.Add(_values.GetInt(index));                                                            //__SILP__
                            }                                                                                                    //__SILP__
                        }                                                                                                        //__SILP__
                        return prop.AddValueChecker(pass, new DataIntSpecValueCheckerIn(subKey, intValues.ToArray()));           //__SILP__
                    case DataType.Long:                                                                                          //__SILP__
                        List<long> longValues = new List<long>();                                                                //__SILP__
                        for (int i = 0; i < _values.Count; i++) {                                                                //__SILP__
                            string index = i.ToString();                                                                         //__SILP__
                            if (_values.IsLong(index)) {                                                                         //__SILP__
                                longValues.Add(_values.GetLong(index));                                                          //__SILP__
                            }                                                                                                    //__SILP__
                        }                                                                                                        //__SILP__
                        return prop.AddValueChecker(pass, new DataLongSpecValueCheckerIn(subKey, longValues.ToArray()));         //__SILP__
                    case DataType.String:                                                                                        //__SILP__
                        List<string> stringValues = new List<string>();                                                          //__SILP__
                        for (int i = 0; i < _values.Count; i++) {                                                                //__SILP__
                            string index = i.ToString();                                                                         //__SILP__
                            if (_values.IsString(index)) {                                                                       //__SILP__
                                stringValues.Add(_values.GetString(index));                                                      //__SILP__
                            }                                                                                                    //__SILP__
                        }                                                                                                        //__SILP__
                        return prop.AddValueChecker(pass, new DataStringSpecValueCheckerIn(subKey, stringValues.ToArray()));     //__SILP__
                }                                                                                                                //__SILP__
                return false;                                                                                                    //__SILP__
            });                                                                                                                  //__SILP__
                                                                                                                                 //__SILP__
            Spec.RegisterSpecValueChecker(PropertiesConsts.TypeDataProperty, SpecConsts.KindNotIn,                               //__SILP__
                    (IProperty _prop, Pass pass, Data spec, string specKey) => {                                                 //__SILP__
                if (spec == null) return false;                                                                                  //__SILP__
                DataProperty prop = _prop as DataProperty;                                                                       //__SILP__
                if (prop == null) return false;                                                                                  //__SILP__
                string subKey = SpecConsts.GetSubKey(specKey);                                                                   //__SILP__
                if (subKey == null) return false;                                                                                //__SILP__
                Data _values = spec.GetData(specKey, null);                                                                      //__SILP__
                if (_values == null) return false;                                                                               //__SILP__
                DataType valueType = _values.GetValueType(0.ToString());                                                         //__SILP__
                switch (valueType) {                                                                                             //__SILP__
                    case DataType.Int:                                                                                           //__SILP__
                        List<int> intValues = new List<int>();                                                                   //__SILP__
                        for (int i = 0; i < _values.Count; i++) {                                                                //__SILP__
                            string index = i.ToString();                                                                         //__SILP__
                            if (_values.IsInt(index)) {                                                                          //__SILP__
                                intValues.Add(_values.GetInt(index));                                                            //__SILP__
                            }                                                                                                    //__SILP__
                        }                                                                                                        //__SILP__
                        return prop.AddValueChecker(pass, new DataIntSpecValueCheckerNotIn(subKey, intValues.ToArray()));        //__SILP__
                    case DataType.Long:                                                                                          //__SILP__
                        List<long> longValues = new List<long>();                                                                //__SILP__
                        for (int i = 0; i < _values.Count; i++) {                                                                //__SILP__
                            string index = i.ToString();                                                                         //__SILP__
                            if (_values.IsLong(index)) {                                                                         //__SILP__
                                longValues.Add(_values.GetLong(index));                                                          //__SILP__
                            }                                                                                                    //__SILP__
                        }                                                                                                        //__SILP__
                        return prop.AddValueChecker(pass, new DataLongSpecValueCheckerNotIn(subKey, longValues.ToArray()));      //__SILP__
                    case DataType.String:                                                                                        //__SILP__
                        List<string> stringValues = new List<string>();                                                          //__SILP__
                        for (int i = 0; i < _values.Count; i++) {                                                                //__SILP__
                            string index = i.ToString();                                                                         //__SILP__
                            if (_values.IsString(index)) {                                                                       //__SILP__
                                stringValues.Add(_values.GetString(index));                                                      //__SILP__
                            }                                                                                                    //__SILP__
                        }                                                                                                        //__SILP__
                        return prop.AddValueChecker(pass, new DataStringSpecValueCheckerNotIn(subKey, stringValues.ToArray()));  //__SILP__
                }                                                                                                                //__SILP__
                return false;                                                                                                    //__SILP__
            });                                                                                                                  //__SILP__
                                                                                                                                 //__SILP__
        }
    }
}
