using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public struct PropertiesSpecConsts {
        public const string KeySpec = "spec";

        public const string KeyBigger = ">";
        public const string KeyBiggerOrEqual = ">=";
        public const string KeySmaller = "<";
        public const string KeySmallerOrEqual = "<=";
        public const string KeyIn = "~";
        public const string KeyNotIn = "!~";
    }

    public delegate bool SpecValueCheckerFactory(Property prop, Pass pass, Data spec, string key);

    public static class PropertiesSpec {
        public static string GetFactoryKey(string propertyType, string specKey) {
            return string.Format("{0}:{1}", propertyType, specKey);
        }

        private static Dictionary<string, SpecValueCheckerFactory> _SpecValueCheckersFactories =
                            new Dictionary<string, SpecValueCheckerFactory>();

        public static Data EncodeWithSpec(this Property prop) {
            Data data = prop.Encode();
            if (data != null) {
                Data spec = null;
                prop.AllValueCheckers<SpecValueChecker>((SpecValueChecker checker) => {
                    if (spec == null) spec = new Data();
                    checker.DoEncode(spec);
                });
                if (spec != null && spec.Count > 0) {
                    data.SetData(PropertiesSpecConsts.KeySpec, spec);
                }
            }
            return data;
        }

        public static Property AddPropertyWithSpec(this Context context, string path, Pass pass, bool open, Data data) {
            Property prop = context.AddProperty(path, pass, open, data);
            if (prop != null) {
                Data spec = data.GetData(PropertiesSpecConsts.KeySpec, null);
                if (spec != null) {
                    foreach (string specKey in spec.Keys) {
                        string factoryKey = GetFactoryKey(prop.Type, specKey);
                        SpecValueCheckerFactory factory = null;
                        if (_SpecValueCheckersFactories.TryGetValue(factoryKey, out factory)) {
                            if (!factory(prop, pass, spec, specKey)) {
                                context.Error("SpecValueCheckerFactory Failed: {0}, Value: {2}",
                                    factoryKey, spec.GetValue(specKey));
                            }
                        } else {
                            context.Error("SpecValueCheckerFactory Not Found: {0}", factoryKey);
                        }
                    }
                }
            }
            return prop;
        }

        public static bool RegistrySpecValueCheckerFactories(string propertyType, string specKey,
                                                                SpecValueCheckerFactory factory) {
            string factoryKey = GetFactoryKey(propertyType, specKey);
            if (_SpecValueCheckersFactories.ContainsKey(factoryKey)) {
                Log.Error("SpecValueCheckerFactory Already Exist: {0}, {1}", factoryKey, factory);
                return false;
            }
            _SpecValueCheckersFactories[factoryKey] = factory;
            return true;
        }

        static PropertiesSpec() {
            //SILP:PROPERTIES_SPEC_HELPER(Int, int);
            _SpecValueCheckersFactories[GetFactoryKey(PropertiesConsts.TypeIntProperty, PropertiesSpecConsts.KeyBigger)] =          //__SILP__
                    (Property _prop, Pass pass, Data spec, string key) => {                                                         //__SILP__
                if (spec == null) return false;                                                                                     //__SILP__
                IntProperty prop = _prop as IntProperty;                                                                            //__SILP__
                if (prop == null) return false;                                                                                     //__SILP__
                return prop.AddValueChecker(new IntSpecValueCheckerBigger(spec.GetInt(key)));                                       //__SILP__
            };                                                                                                                      //__SILP__
                                                                                                                                    //__SILP__
            _SpecValueCheckersFactories[GetFactoryKey(PropertiesConsts.TypeIntProperty, PropertiesSpecConsts.KeyBiggerOrEqual)] =   //__SILP__
                    (Property _prop, Pass pass, Data spec, string key) => {                                                         //__SILP__
                if (spec == null) return false;                                                                                     //__SILP__
                IntProperty prop = _prop as IntProperty;                                                                            //__SILP__
                if (prop == null) return false;                                                                                     //__SILP__
                return prop.AddValueChecker(new IntSpecValueCheckerBiggerOrEqual(spec.GetInt(key)));                                //__SILP__
            };                                                                                                                      //__SILP__
                                                                                                                                    //__SILP__
            _SpecValueCheckersFactories[GetFactoryKey(PropertiesConsts.TypeIntProperty, PropertiesSpecConsts.KeySmaller)] =         //__SILP__
                    (Property _prop, Pass pass, Data spec, string key) => {                                                         //__SILP__
                if (spec == null) return false;                                                                                     //__SILP__
                IntProperty prop = _prop as IntProperty;                                                                            //__SILP__
                if (prop == null) return false;                                                                                     //__SILP__
                return prop.AddValueChecker(new IntSpecValueCheckerSmaller(spec.GetInt(key)));                                      //__SILP__
            };                                                                                                                      //__SILP__
                                                                                                                                    //__SILP__
            _SpecValueCheckersFactories[GetFactoryKey(PropertiesConsts.TypeIntProperty, PropertiesSpecConsts.KeySmallerOrEqual)] =  //__SILP__
                    (Property _prop, Pass pass, Data spec, string key) => {                                                         //__SILP__
                if (spec == null) return false;                                                                                     //__SILP__
                IntProperty prop = _prop as IntProperty;                                                                            //__SILP__
                if (prop == null) return false;                                                                                     //__SILP__
                return prop.AddValueChecker(new IntSpecValueCheckerSmallerOrEqual(spec.GetInt(key)));                               //__SILP__
            };                                                                                                                      //__SILP__
                                                                                                                                    //__SILP__
            //SILP:PROPERTIES_SPEC_HELPER(Long, long);
            _SpecValueCheckersFactories[GetFactoryKey(PropertiesConsts.TypeLongProperty, PropertiesSpecConsts.KeyBigger)] =          //__SILP__
                    (Property _prop, Pass pass, Data spec, string key) => {                                                          //__SILP__
                if (spec == null) return false;                                                                                      //__SILP__
                LongProperty prop = _prop as LongProperty;                                                                           //__SILP__
                if (prop == null) return false;                                                                                      //__SILP__
                return prop.AddValueChecker(new LongSpecValueCheckerBigger(spec.GetLong(key)));                                      //__SILP__
            };                                                                                                                       //__SILP__
                                                                                                                                     //__SILP__
            _SpecValueCheckersFactories[GetFactoryKey(PropertiesConsts.TypeLongProperty, PropertiesSpecConsts.KeyBiggerOrEqual)] =   //__SILP__
                    (Property _prop, Pass pass, Data spec, string key) => {                                                          //__SILP__
                if (spec == null) return false;                                                                                      //__SILP__
                LongProperty prop = _prop as LongProperty;                                                                           //__SILP__
                if (prop == null) return false;                                                                                      //__SILP__
                return prop.AddValueChecker(new LongSpecValueCheckerBiggerOrEqual(spec.GetLong(key)));                               //__SILP__
            };                                                                                                                       //__SILP__
                                                                                                                                     //__SILP__
            _SpecValueCheckersFactories[GetFactoryKey(PropertiesConsts.TypeLongProperty, PropertiesSpecConsts.KeySmaller)] =         //__SILP__
                    (Property _prop, Pass pass, Data spec, string key) => {                                                          //__SILP__
                if (spec == null) return false;                                                                                      //__SILP__
                LongProperty prop = _prop as LongProperty;                                                                           //__SILP__
                if (prop == null) return false;                                                                                      //__SILP__
                return prop.AddValueChecker(new LongSpecValueCheckerSmaller(spec.GetLong(key)));                                     //__SILP__
            };                                                                                                                       //__SILP__
                                                                                                                                     //__SILP__
            _SpecValueCheckersFactories[GetFactoryKey(PropertiesConsts.TypeLongProperty, PropertiesSpecConsts.KeySmallerOrEqual)] =  //__SILP__
                    (Property _prop, Pass pass, Data spec, string key) => {                                                          //__SILP__
                if (spec == null) return false;                                                                                      //__SILP__
                LongProperty prop = _prop as LongProperty;                                                                           //__SILP__
                if (prop == null) return false;                                                                                      //__SILP__
                return prop.AddValueChecker(new LongSpecValueCheckerSmallerOrEqual(spec.GetLong(key)));                              //__SILP__
            };                                                                                                                       //__SILP__
                                                                                                                                     //__SILP__
            //SILP:PROPERTIES_SPEC_HELPER(Float, float);
            _SpecValueCheckersFactories[GetFactoryKey(PropertiesConsts.TypeFloatProperty, PropertiesSpecConsts.KeyBigger)] =          //__SILP__
                    (Property _prop, Pass pass, Data spec, string key) => {                                                           //__SILP__
                if (spec == null) return false;                                                                                       //__SILP__
                FloatProperty prop = _prop as FloatProperty;                                                                          //__SILP__
                if (prop == null) return false;                                                                                       //__SILP__
                return prop.AddValueChecker(new FloatSpecValueCheckerBigger(spec.GetFloat(key)));                                     //__SILP__
            };                                                                                                                        //__SILP__
                                                                                                                                      //__SILP__
            _SpecValueCheckersFactories[GetFactoryKey(PropertiesConsts.TypeFloatProperty, PropertiesSpecConsts.KeyBiggerOrEqual)] =   //__SILP__
                    (Property _prop, Pass pass, Data spec, string key) => {                                                           //__SILP__
                if (spec == null) return false;                                                                                       //__SILP__
                FloatProperty prop = _prop as FloatProperty;                                                                          //__SILP__
                if (prop == null) return false;                                                                                       //__SILP__
                return prop.AddValueChecker(new FloatSpecValueCheckerBiggerOrEqual(spec.GetFloat(key)));                              //__SILP__
            };                                                                                                                        //__SILP__
                                                                                                                                      //__SILP__
            _SpecValueCheckersFactories[GetFactoryKey(PropertiesConsts.TypeFloatProperty, PropertiesSpecConsts.KeySmaller)] =         //__SILP__
                    (Property _prop, Pass pass, Data spec, string key) => {                                                           //__SILP__
                if (spec == null) return false;                                                                                       //__SILP__
                FloatProperty prop = _prop as FloatProperty;                                                                          //__SILP__
                if (prop == null) return false;                                                                                       //__SILP__
                return prop.AddValueChecker(new FloatSpecValueCheckerSmaller(spec.GetFloat(key)));                                    //__SILP__
            };                                                                                                                        //__SILP__
                                                                                                                                      //__SILP__
            _SpecValueCheckersFactories[GetFactoryKey(PropertiesConsts.TypeFloatProperty, PropertiesSpecConsts.KeySmallerOrEqual)] =  //__SILP__
                    (Property _prop, Pass pass, Data spec, string key) => {                                                           //__SILP__
                if (spec == null) return false;                                                                                       //__SILP__
                FloatProperty prop = _prop as FloatProperty;                                                                          //__SILP__
                if (prop == null) return false;                                                                                       //__SILP__
                return prop.AddValueChecker(new FloatSpecValueCheckerSmallerOrEqual(spec.GetFloat(key)));                             //__SILP__
            };                                                                                                                        //__SILP__
                                                                                                                                      //__SILP__
            //SILP:PROPERTIES_SPEC_HELPER(Double, double);
            _SpecValueCheckersFactories[GetFactoryKey(PropertiesConsts.TypeDoubleProperty, PropertiesSpecConsts.KeyBigger)] =          //__SILP__
                    (Property _prop, Pass pass, Data spec, string key) => {                                                            //__SILP__
                if (spec == null) return false;                                                                                        //__SILP__
                DoubleProperty prop = _prop as DoubleProperty;                                                                         //__SILP__
                if (prop == null) return false;                                                                                        //__SILP__
                return prop.AddValueChecker(new DoubleSpecValueCheckerBigger(spec.GetDouble(key)));                                    //__SILP__
            };                                                                                                                         //__SILP__
                                                                                                                                       //__SILP__
            _SpecValueCheckersFactories[GetFactoryKey(PropertiesConsts.TypeDoubleProperty, PropertiesSpecConsts.KeyBiggerOrEqual)] =   //__SILP__
                    (Property _prop, Pass pass, Data spec, string key) => {                                                            //__SILP__
                if (spec == null) return false;                                                                                        //__SILP__
                DoubleProperty prop = _prop as DoubleProperty;                                                                         //__SILP__
                if (prop == null) return false;                                                                                        //__SILP__
                return prop.AddValueChecker(new DoubleSpecValueCheckerBiggerOrEqual(spec.GetDouble(key)));                             //__SILP__
            };                                                                                                                         //__SILP__
                                                                                                                                       //__SILP__
            _SpecValueCheckersFactories[GetFactoryKey(PropertiesConsts.TypeDoubleProperty, PropertiesSpecConsts.KeySmaller)] =         //__SILP__
                    (Property _prop, Pass pass, Data spec, string key) => {                                                            //__SILP__
                if (spec == null) return false;                                                                                        //__SILP__
                DoubleProperty prop = _prop as DoubleProperty;                                                                         //__SILP__
                if (prop == null) return false;                                                                                        //__SILP__
                return prop.AddValueChecker(new DoubleSpecValueCheckerSmaller(spec.GetDouble(key)));                                   //__SILP__
            };                                                                                                                         //__SILP__
                                                                                                                                       //__SILP__
            _SpecValueCheckersFactories[GetFactoryKey(PropertiesConsts.TypeDoubleProperty, PropertiesSpecConsts.KeySmallerOrEqual)] =  //__SILP__
                    (Property _prop, Pass pass, Data spec, string key) => {                                                            //__SILP__
                if (spec == null) return false;                                                                                        //__SILP__
                DoubleProperty prop = _prop as DoubleProperty;                                                                         //__SILP__
                if (prop == null) return false;                                                                                        //__SILP__
                return prop.AddValueChecker(new DoubleSpecValueCheckerSmallerOrEqual(spec.GetDouble(key)));                            //__SILP__
            };                                                                                                                         //__SILP__
                                                                                                                                       //__SILP__
            //SILP:PROPERTIES_SPEC_IN_HELPER(String, string);
            RegistrySpecValueCheckerFactories(PropertiesConsts.TypeStringProperty, PropertiesSpecConsts.KeyIn,     //__SILP__
                    (Property _prop, Pass pass, Data spec, string key) => {                                        //__SILP__
                if (spec == null) return false;                                                                    //__SILP__
                StringProperty prop = _prop as StringProperty;                                                     //__SILP__
                if (prop == null) return false;                                                                    //__SILP__
                Data _values = spec.GetData(key, null);                                                            //__SILP__
                if (_values == null) return false;                                                                 //__SILP__
                List<string> values = new List<string>();                                                          //__SILP__
                for (int i = 0; i < _values.Count; i++) {                                                          //__SILP__
                    string index = i.ToString();                                                                   //__SILP__
                    if (_values.IsString(index)) {                                                                 //__SILP__
                        values.Add(_values.GetString(index));                                                      //__SILP__
                    }                                                                                              //__SILP__
                }                                                                                                  //__SILP__
                return prop.AddValueChecker(new StringSpecValueCheckerIn(values.ToArray()));                       //__SILP__
            });                                                                                                    //__SILP__
                                                                                                                   //__SILP__
            RegistrySpecValueCheckerFactories(PropertiesConsts.TypeStringProperty, PropertiesSpecConsts.KeyNotIn,  //__SILP__
                    (Property _prop, Pass pass, Data spec, string key) => {                                        //__SILP__
                if (spec == null) return false;                                                                    //__SILP__
                StringProperty prop = _prop as StringProperty;                                                     //__SILP__
                if (prop == null) return false;                                                                    //__SILP__
                Data _values = spec.GetData(key, null);                                                            //__SILP__
                if (_values == null) return false;                                                                 //__SILP__
                List<string> values = new List<string>();                                                          //__SILP__
                for (int i = 0; i < _values.Count; i++) {                                                          //__SILP__
                    string index = i.ToString();                                                                   //__SILP__
                    if (_values.IsString(index)) {                                                                 //__SILP__
                        values.Add(_values.GetString(index));                                                      //__SILP__
                    }                                                                                              //__SILP__
                }                                                                                                  //__SILP__
                return prop.AddValueChecker(new StringSpecValueCheckerNotIn(values.ToArray()));                    //__SILP__
            });                                                                                                    //__SILP__
                                                                                                                   //__SILP__
        }
    }
}
