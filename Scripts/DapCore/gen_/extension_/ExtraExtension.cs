using System;
using System.Collections.Generic;

using angeldnd.dap;

namespace angeldnd.dap {
    public static class ExtraExtension {
        //SILP: EXTRA_SETUP_PROPERTY(Bool, bool)
        public static BoolProperty SetupBoolProperty(this Extra ext,                         //__SILP__
                string fragment, bool val) {                                                 //__SILP__
            BoolProperty prop = ext.SetupProperty<BoolProperty>(                             //__SILP__
                                        PropertiesConsts.TypeBoolProperty, fragment);        //__SILP__
            if (prop != null) {                                                              //__SILP__
                prop.Setup(val);                                                             //__SILP__
            }                                                                                //__SILP__
            return prop;                                                                     //__SILP__
        }                                                                                    //__SILP__
                                                                                             //__SILP__
        public static BoolProperty SetupBoolProperty(this Extra ext,                         //__SILP__
                string fragment, Func<bool> getter,                                          //__SILP__
                Func<IVar<bool>, bool, bool> checker,                                        //__SILP__
                Action<IVar<bool>, bool> watcher) {                                          //__SILP__
            return ext.SetupProperty<BoolProperty, bool>(PropertiesConsts.TypeBoolProperty,  //__SILP__
                fragment, getter,                                                            //__SILP__
                checker == null ? null : new BlockValueChecker<bool>(ext, checker),          //__SILP__
                watcher == null ? null : new BlockValueWatcher<bool>(ext, watcher)           //__SILP__
            );                                                                               //__SILP__
        }                                                                                    //__SILP__
                                                                                             //__SILP__
        public static BoolProperty SetupBoolProperty(this Extra ext,                         //__SILP__
                string fragment,                                                             //__SILP__
                Func<bool> getter,                                                           //__SILP__
                Action<IVar<bool>, bool> watcher) {                                          //__SILP__
            return SetupBoolProperty(ext, fragment, getter, null, watcher);                  //__SILP__
        }                                                                                    //__SILP__
                                                                                             //__SILP__
        public static BoolProperty SetupBoolProperty(this Extra ext,                         //__SILP__
                string fragment, Func<bool> getter) {                                        //__SILP__
            return SetupBoolProperty(ext, fragment, getter, null, null);                     //__SILP__
        }                                                                                    //__SILP__
                                                                                             //__SILP__
        //SILP: EXTRA_SETUP_PROPERTY(Int, int)
        public static IntProperty SetupIntProperty(this Extra ext,                        //__SILP__
                string fragment, int val) {                                               //__SILP__
            IntProperty prop = ext.SetupProperty<IntProperty>(                            //__SILP__
                                        PropertiesConsts.TypeIntProperty, fragment);      //__SILP__
            if (prop != null) {                                                           //__SILP__
                prop.Setup(val);                                                          //__SILP__
            }                                                                             //__SILP__
            return prop;                                                                  //__SILP__
        }                                                                                 //__SILP__
                                                                                          //__SILP__
        public static IntProperty SetupIntProperty(this Extra ext,                        //__SILP__
                string fragment, Func<int> getter,                                        //__SILP__
                Func<IVar<int>, int, bool> checker,                                       //__SILP__
                Action<IVar<int>, int> watcher) {                                         //__SILP__
            return ext.SetupProperty<IntProperty, int>(PropertiesConsts.TypeIntProperty,  //__SILP__
                fragment, getter,                                                         //__SILP__
                checker == null ? null : new BlockValueChecker<int>(ext, checker),        //__SILP__
                watcher == null ? null : new BlockValueWatcher<int>(ext, watcher)         //__SILP__
            );                                                                            //__SILP__
        }                                                                                 //__SILP__
                                                                                          //__SILP__
        public static IntProperty SetupIntProperty(this Extra ext,                        //__SILP__
                string fragment,                                                          //__SILP__
                Func<int> getter,                                                         //__SILP__
                Action<IVar<int>, int> watcher) {                                         //__SILP__
            return SetupIntProperty(ext, fragment, getter, null, watcher);                //__SILP__
        }                                                                                 //__SILP__
                                                                                          //__SILP__
        public static IntProperty SetupIntProperty(this Extra ext,                        //__SILP__
                string fragment, Func<int> getter) {                                      //__SILP__
            return SetupIntProperty(ext, fragment, getter, null, null);                   //__SILP__
        }                                                                                 //__SILP__
                                                                                          //__SILP__
        //SILP: EXTRA_SETUP_PROPERTY(Long, long)
        public static LongProperty SetupLongProperty(this Extra ext,                         //__SILP__
                string fragment, long val) {                                                 //__SILP__
            LongProperty prop = ext.SetupProperty<LongProperty>(                             //__SILP__
                                        PropertiesConsts.TypeLongProperty, fragment);        //__SILP__
            if (prop != null) {                                                              //__SILP__
                prop.Setup(val);                                                             //__SILP__
            }                                                                                //__SILP__
            return prop;                                                                     //__SILP__
        }                                                                                    //__SILP__
                                                                                             //__SILP__
        public static LongProperty SetupLongProperty(this Extra ext,                         //__SILP__
                string fragment, Func<long> getter,                                          //__SILP__
                Func<IVar<long>, long, bool> checker,                                        //__SILP__
                Action<IVar<long>, long> watcher) {                                          //__SILP__
            return ext.SetupProperty<LongProperty, long>(PropertiesConsts.TypeLongProperty,  //__SILP__
                fragment, getter,                                                            //__SILP__
                checker == null ? null : new BlockValueChecker<long>(ext, checker),          //__SILP__
                watcher == null ? null : new BlockValueWatcher<long>(ext, watcher)           //__SILP__
            );                                                                               //__SILP__
        }                                                                                    //__SILP__
                                                                                             //__SILP__
        public static LongProperty SetupLongProperty(this Extra ext,                         //__SILP__
                string fragment,                                                             //__SILP__
                Func<long> getter,                                                           //__SILP__
                Action<IVar<long>, long> watcher) {                                          //__SILP__
            return SetupLongProperty(ext, fragment, getter, null, watcher);                  //__SILP__
        }                                                                                    //__SILP__
                                                                                             //__SILP__
        public static LongProperty SetupLongProperty(this Extra ext,                         //__SILP__
                string fragment, Func<long> getter) {                                        //__SILP__
            return SetupLongProperty(ext, fragment, getter, null, null);                     //__SILP__
        }                                                                                    //__SILP__
                                                                                             //__SILP__
        //SILP: EXTRA_SETUP_PROPERTY(Float, float)
        public static FloatProperty SetupFloatProperty(this Extra ext,                          //__SILP__
                string fragment, float val) {                                                   //__SILP__
            FloatProperty prop = ext.SetupProperty<FloatProperty>(                              //__SILP__
                                        PropertiesConsts.TypeFloatProperty, fragment);          //__SILP__
            if (prop != null) {                                                                 //__SILP__
                prop.Setup(val);                                                                //__SILP__
            }                                                                                   //__SILP__
            return prop;                                                                        //__SILP__
        }                                                                                       //__SILP__
                                                                                                //__SILP__
        public static FloatProperty SetupFloatProperty(this Extra ext,                          //__SILP__
                string fragment, Func<float> getter,                                            //__SILP__
                Func<IVar<float>, float, bool> checker,                                         //__SILP__
                Action<IVar<float>, float> watcher) {                                           //__SILP__
            return ext.SetupProperty<FloatProperty, float>(PropertiesConsts.TypeFloatProperty,  //__SILP__
                fragment, getter,                                                               //__SILP__
                checker == null ? null : new BlockValueChecker<float>(ext, checker),            //__SILP__
                watcher == null ? null : new BlockValueWatcher<float>(ext, watcher)             //__SILP__
            );                                                                                  //__SILP__
        }                                                                                       //__SILP__
                                                                                                //__SILP__
        public static FloatProperty SetupFloatProperty(this Extra ext,                          //__SILP__
                string fragment,                                                                //__SILP__
                Func<float> getter,                                                             //__SILP__
                Action<IVar<float>, float> watcher) {                                           //__SILP__
            return SetupFloatProperty(ext, fragment, getter, null, watcher);                    //__SILP__
        }                                                                                       //__SILP__
                                                                                                //__SILP__
        public static FloatProperty SetupFloatProperty(this Extra ext,                          //__SILP__
                string fragment, Func<float> getter) {                                          //__SILP__
            return SetupFloatProperty(ext, fragment, getter, null, null);                       //__SILP__
        }                                                                                       //__SILP__
                                                                                                //__SILP__
        //SILP: EXTRA_SETUP_PROPERTY(Double, double)
        public static DoubleProperty SetupDoubleProperty(this Extra ext,                           //__SILP__
                string fragment, double val) {                                                     //__SILP__
            DoubleProperty prop = ext.SetupProperty<DoubleProperty>(                               //__SILP__
                                        PropertiesConsts.TypeDoubleProperty, fragment);            //__SILP__
            if (prop != null) {                                                                    //__SILP__
                prop.Setup(val);                                                                   //__SILP__
            }                                                                                      //__SILP__
            return prop;                                                                           //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        public static DoubleProperty SetupDoubleProperty(this Extra ext,                           //__SILP__
                string fragment, Func<double> getter,                                              //__SILP__
                Func<IVar<double>, double, bool> checker,                                          //__SILP__
                Action<IVar<double>, double> watcher) {                                            //__SILP__
            return ext.SetupProperty<DoubleProperty, double>(PropertiesConsts.TypeDoubleProperty,  //__SILP__
                fragment, getter,                                                                  //__SILP__
                checker == null ? null : new BlockValueChecker<double>(ext, checker),              //__SILP__
                watcher == null ? null : new BlockValueWatcher<double>(ext, watcher)               //__SILP__
            );                                                                                     //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        public static DoubleProperty SetupDoubleProperty(this Extra ext,                           //__SILP__
                string fragment,                                                                   //__SILP__
                Func<double> getter,                                                               //__SILP__
                Action<IVar<double>, double> watcher) {                                            //__SILP__
            return SetupDoubleProperty(ext, fragment, getter, null, watcher);                      //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        public static DoubleProperty SetupDoubleProperty(this Extra ext,                           //__SILP__
                string fragment, Func<double> getter) {                                            //__SILP__
            return SetupDoubleProperty(ext, fragment, getter, null, null);                         //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        //SILP: EXTRA_SETUP_PROPERTY(String, string)
        public static StringProperty SetupStringProperty(this Extra ext,                           //__SILP__
                string fragment, string val) {                                                     //__SILP__
            StringProperty prop = ext.SetupProperty<StringProperty>(                               //__SILP__
                                        PropertiesConsts.TypeStringProperty, fragment);            //__SILP__
            if (prop != null) {                                                                    //__SILP__
                prop.Setup(val);                                                                   //__SILP__
            }                                                                                      //__SILP__
            return prop;                                                                           //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        public static StringProperty SetupStringProperty(this Extra ext,                           //__SILP__
                string fragment, Func<string> getter,                                              //__SILP__
                Func<IVar<string>, string, bool> checker,                                          //__SILP__
                Action<IVar<string>, string> watcher) {                                            //__SILP__
            return ext.SetupProperty<StringProperty, string>(PropertiesConsts.TypeStringProperty,  //__SILP__
                fragment, getter,                                                                  //__SILP__
                checker == null ? null : new BlockValueChecker<string>(ext, checker),              //__SILP__
                watcher == null ? null : new BlockValueWatcher<string>(ext, watcher)               //__SILP__
            );                                                                                     //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        public static StringProperty SetupStringProperty(this Extra ext,                           //__SILP__
                string fragment,                                                                   //__SILP__
                Func<string> getter,                                                               //__SILP__
                Action<IVar<string>, string> watcher) {                                            //__SILP__
            return SetupStringProperty(ext, fragment, getter, null, watcher);                      //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        public static StringProperty SetupStringProperty(this Extra ext,                           //__SILP__
                string fragment, Func<string> getter) {                                            //__SILP__
            return SetupStringProperty(ext, fragment, getter, null, null);                         //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        //SILP: EXTRA_SETUP_PROPERTY(Data, Data)
        public static DataProperty SetupDataProperty(this Extra ext,                         //__SILP__
                string fragment, Data val) {                                                 //__SILP__
            DataProperty prop = ext.SetupProperty<DataProperty>(                             //__SILP__
                                        PropertiesConsts.TypeDataProperty, fragment);        //__SILP__
            if (prop != null) {                                                              //__SILP__
                prop.Setup(val);                                                             //__SILP__
            }                                                                                //__SILP__
            return prop;                                                                     //__SILP__
        }                                                                                    //__SILP__
                                                                                             //__SILP__
        public static DataProperty SetupDataProperty(this Extra ext,                         //__SILP__
                string fragment, Func<Data> getter,                                          //__SILP__
                Func<IVar<Data>, Data, bool> checker,                                        //__SILP__
                Action<IVar<Data>, Data> watcher) {                                          //__SILP__
            return ext.SetupProperty<DataProperty, Data>(PropertiesConsts.TypeDataProperty,  //__SILP__
                fragment, getter,                                                            //__SILP__
                checker == null ? null : new BlockValueChecker<Data>(ext, checker),          //__SILP__
                watcher == null ? null : new BlockValueWatcher<Data>(ext, watcher)           //__SILP__
            );                                                                               //__SILP__
        }                                                                                    //__SILP__
                                                                                             //__SILP__
        public static DataProperty SetupDataProperty(this Extra ext,                         //__SILP__
                string fragment,                                                             //__SILP__
                Func<Data> getter,                                                           //__SILP__
                Action<IVar<Data>, Data> watcher) {                                          //__SILP__
            return SetupDataProperty(ext, fragment, getter, null, watcher);                  //__SILP__
        }                                                                                    //__SILP__
                                                                                             //__SILP__
        public static DataProperty SetupDataProperty(this Extra ext,                         //__SILP__
                string fragment, Func<Data> getter) {                                        //__SILP__
            return SetupDataProperty(ext, fragment, getter, null, null);                     //__SILP__
        }                                                                                    //__SILP__
                                                                                             //__SILP__
    }
}
