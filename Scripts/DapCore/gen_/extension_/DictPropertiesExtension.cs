using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class DictPropertiesExtension {
        public static T GetProperty<T>(this IDictProperties properties,
                                            string propertyPath, bool isDebug = false)
                                                where T : class, IProperty {
            return TreeHelper.GetDescendant<T>(properties, propertyPath, isDebug);
        }

        public static IProperty AddProperty(this IDictProperties properties,
                                            string key, Data data) {
            if (data == null) return null;
            string dapType = data.GetString(ObjectConsts.KeyDapType);
            if (string.IsNullOrEmpty(dapType)) {
                properties.Error("Invalid Property data: {0}, {1}", key, data);
                return null;
            }
            IProperty prop = properties.New<IProperty>(dapType, key);
            if (prop == null) {
                properties.Error("Failed to Add Property: {0}, {1}", key, data);
                return null;
            }
            if (data.HasValue(PropertiesConsts.KeyValue)) {
                if (!prop.Decode(data)) {
                    properties.Error("Failed to Decode Property: {0}, {1} -> {2}", key, data, prop);
                }
            }

            return prop;
        }

        //SILP: DICT_PROPERTIES_HELPER(Bool, bool)
        public static BoolProperty AddBool(this IDictProperties properties, string key, bool val) {                            //__SILP__
            BoolProperty v = properties.Add<BoolProperty>(key);                                                                //__SILP__
            if (v != null && !v.Setup(val)) {                                                                                  //__SILP__
                properties.Remove<BoolProperty>(key);                                                                          //__SILP__
                v = null;                                                                                                      //__SILP__
            }                                                                                                                  //__SILP__
            return v;                                                                                                          //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static BoolProperty RemoveBool(this IDictProperties properties, string key) {                                   //__SILP__
            return properties.Remove<BoolProperty>(key);                                                                       //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static bool AddBoolValueChecker(this IDictProperties properties, string key, IValueChecker<bool> checker) {     //__SILP__
            BoolProperty p = properties.Get<BoolProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                   //__SILP__
                return p.AddValueChecker(checker);                                                                             //__SILP__
            } else {                                                                                                           //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                              //__SILP__
            }                                                                                                                  //__SILP__
            return false;                                                                                                      //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static bool RemoveBoolValueChecker(this IDictProperties properties, string key, IValueChecker<bool> checker) {  //__SILP__
            BoolProperty p = properties.Get<BoolProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                   //__SILP__
                return p.RemoveValueChecker(checker);                                                                          //__SILP__
            } else {                                                                                                           //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                              //__SILP__
            }                                                                                                                  //__SILP__
            return false;                                                                                                      //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static BlockValueChecker<bool> AddBoolValueChecker(this IDictProperties properties, string key,                 //__SILP__
                                            IBlockOwner owner, Func<IVar<bool>, bool, bool> block) {                           //__SILP__
            BoolProperty p = properties.Get<BoolProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                   //__SILP__
                return p.AddValueChecker(owner, block);                                                                        //__SILP__
            } else {                                                                                                           //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                              //__SILP__
            }                                                                                                                  //__SILP__
            return null;                                                                                                       //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static bool AddBoolValueWatcher(this IDictProperties properties, string key, IValueWatcher<bool> watcher) {     //__SILP__
            BoolProperty p = properties.Get<BoolProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                   //__SILP__
                return p.AddValueWatcher(watcher);                                                                             //__SILP__
            } else {                                                                                                           //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                              //__SILP__
            }                                                                                                                  //__SILP__
            return false;                                                                                                      //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static bool RemoveBoolValueWatcher(this IDictProperties properties, string key, IValueWatcher<bool> watcher) {  //__SILP__
            BoolProperty p = properties.Get<BoolProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                   //__SILP__
                return p.RemoveValueWatcher(watcher);                                                                          //__SILP__
            } else {                                                                                                           //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                              //__SILP__
            }                                                                                                                  //__SILP__
            return false;                                                                                                      //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static BlockValueWatcher<bool> AddBoolValueWatcher(this IDictProperties properties, string key,                 //__SILP__
                                            IBlockOwner owner, Action<IVar<bool>, bool> block) {                               //__SILP__
            BoolProperty p = properties.Get<BoolProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                   //__SILP__
                return p.AddValueWatcher(owner, block);                                                                        //__SILP__
            } else {                                                                                                           //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                              //__SILP__
            }                                                                                                                  //__SILP__
            return null;                                                                                                       //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static bool IsBool(this IDictProperties properties, string key) {                                               //__SILP__
            return properties.Is<BoolProperty>(key);                                                                           //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static bool GetBool(this IDictProperties properties, string key) {                                              //__SILP__
            BoolProperty v = properties.Get<BoolProperty>(key);                                                                //__SILP__
            if (v != null) {                                                                                                   //__SILP__
                return v.Value;                                                                                                //__SILP__
            } else {                                                                                                           //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                              //__SILP__
            }                                                                                                                  //__SILP__
            return default(bool);                                                                                              //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static bool GetBool(this IDictProperties properties, string key, bool defaultValue) {                           //__SILP__
            BoolProperty v = properties.Get<BoolProperty>(key, true);                                                          //__SILP__
            if (v != null) {                                                                                                   //__SILP__
                return v.Value;                                                                                                //__SILP__
            } else {                                                                                                           //__SILP__
                properties.Debug("Property Not Exist: {0}", key);                                                              //__SILP__
            }                                                                                                                  //__SILP__
            return defaultValue;                                                                                               //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static bool SetBool(this IDictProperties properties, string key, bool val) {                                    //__SILP__
            BoolProperty v = properties.Get<BoolProperty>(key);                                                                //__SILP__
            if (v != null) {                                                                                                   //__SILP__
                return v.SetValue(val);                                                                                        //__SILP__
            } else {                                                                                                           //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                              //__SILP__
            }                                                                                                                  //__SILP__
            return false;                                                                                                      //__SILP__
        }                                                                                                                      //__SILP__
        //SILP: DICT_PROPERTIES_HELPER(Int, int)
        public static IntProperty AddInt(this IDictProperties properties, string key, int val) {                             //__SILP__
            IntProperty v = properties.Add<IntProperty>(key);                                                                //__SILP__
            if (v != null && !v.Setup(val)) {                                                                                //__SILP__
                properties.Remove<IntProperty>(key);                                                                         //__SILP__
                v = null;                                                                                                    //__SILP__
            }                                                                                                                //__SILP__
            return v;                                                                                                        //__SILP__
        }                                                                                                                    //__SILP__
                                                                                                                             //__SILP__
        public static IntProperty RemoveInt(this IDictProperties properties, string key) {                                   //__SILP__
            return properties.Remove<IntProperty>(key);                                                                      //__SILP__
        }                                                                                                                    //__SILP__
                                                                                                                             //__SILP__
        public static bool AddIntValueChecker(this IDictProperties properties, string key, IValueChecker<int> checker) {     //__SILP__
            IntProperty p = properties.Get<IntProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                 //__SILP__
                return p.AddValueChecker(checker);                                                                           //__SILP__
            } else {                                                                                                         //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                            //__SILP__
            }                                                                                                                //__SILP__
            return false;                                                                                                    //__SILP__
        }                                                                                                                    //__SILP__
                                                                                                                             //__SILP__
        public static bool RemoveIntValueChecker(this IDictProperties properties, string key, IValueChecker<int> checker) {  //__SILP__
            IntProperty p = properties.Get<IntProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                 //__SILP__
                return p.RemoveValueChecker(checker);                                                                        //__SILP__
            } else {                                                                                                         //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                            //__SILP__
            }                                                                                                                //__SILP__
            return false;                                                                                                    //__SILP__
        }                                                                                                                    //__SILP__
                                                                                                                             //__SILP__
        public static BlockValueChecker<int> AddIntValueChecker(this IDictProperties properties, string key,                 //__SILP__
                                            IBlockOwner owner, Func<IVar<int>, int, bool> block) {                           //__SILP__
            IntProperty p = properties.Get<IntProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                 //__SILP__
                return p.AddValueChecker(owner, block);                                                                      //__SILP__
            } else {                                                                                                         //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                            //__SILP__
            }                                                                                                                //__SILP__
            return null;                                                                                                     //__SILP__
        }                                                                                                                    //__SILP__
                                                                                                                             //__SILP__
        public static bool AddIntValueWatcher(this IDictProperties properties, string key, IValueWatcher<int> watcher) {     //__SILP__
            IntProperty p = properties.Get<IntProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                 //__SILP__
                return p.AddValueWatcher(watcher);                                                                           //__SILP__
            } else {                                                                                                         //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                            //__SILP__
            }                                                                                                                //__SILP__
            return false;                                                                                                    //__SILP__
        }                                                                                                                    //__SILP__
                                                                                                                             //__SILP__
        public static bool RemoveIntValueWatcher(this IDictProperties properties, string key, IValueWatcher<int> watcher) {  //__SILP__
            IntProperty p = properties.Get<IntProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                 //__SILP__
                return p.RemoveValueWatcher(watcher);                                                                        //__SILP__
            } else {                                                                                                         //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                            //__SILP__
            }                                                                                                                //__SILP__
            return false;                                                                                                    //__SILP__
        }                                                                                                                    //__SILP__
                                                                                                                             //__SILP__
        public static BlockValueWatcher<int> AddIntValueWatcher(this IDictProperties properties, string key,                 //__SILP__
                                            IBlockOwner owner, Action<IVar<int>, int> block) {                               //__SILP__
            IntProperty p = properties.Get<IntProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                 //__SILP__
                return p.AddValueWatcher(owner, block);                                                                      //__SILP__
            } else {                                                                                                         //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                            //__SILP__
            }                                                                                                                //__SILP__
            return null;                                                                                                     //__SILP__
        }                                                                                                                    //__SILP__
                                                                                                                             //__SILP__
        public static bool IsInt(this IDictProperties properties, string key) {                                              //__SILP__
            return properties.Is<IntProperty>(key);                                                                          //__SILP__
        }                                                                                                                    //__SILP__
                                                                                                                             //__SILP__
        public static int GetInt(this IDictProperties properties, string key) {                                              //__SILP__
            IntProperty v = properties.Get<IntProperty>(key);                                                                //__SILP__
            if (v != null) {                                                                                                 //__SILP__
                return v.Value;                                                                                              //__SILP__
            } else {                                                                                                         //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                            //__SILP__
            }                                                                                                                //__SILP__
            return default(int);                                                                                             //__SILP__
        }                                                                                                                    //__SILP__
                                                                                                                             //__SILP__
        public static int GetInt(this IDictProperties properties, string key, int defaultValue) {                            //__SILP__
            IntProperty v = properties.Get<IntProperty>(key, true);                                                          //__SILP__
            if (v != null) {                                                                                                 //__SILP__
                return v.Value;                                                                                              //__SILP__
            } else {                                                                                                         //__SILP__
                properties.Debug("Property Not Exist: {0}", key);                                                            //__SILP__
            }                                                                                                                //__SILP__
            return defaultValue;                                                                                             //__SILP__
        }                                                                                                                    //__SILP__
                                                                                                                             //__SILP__
        public static bool SetInt(this IDictProperties properties, string key, int val) {                                    //__SILP__
            IntProperty v = properties.Get<IntProperty>(key);                                                                //__SILP__
            if (v != null) {                                                                                                 //__SILP__
                return v.SetValue(val);                                                                                      //__SILP__
            } else {                                                                                                         //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                            //__SILP__
            }                                                                                                                //__SILP__
            return false;                                                                                                    //__SILP__
        }                                                                                                                    //__SILP__
        //SILP: DICT_PROPERTIES_HELPER(Long, long)
        public static LongProperty AddLong(this IDictProperties properties, string key, long val) {                            //__SILP__
            LongProperty v = properties.Add<LongProperty>(key);                                                                //__SILP__
            if (v != null && !v.Setup(val)) {                                                                                  //__SILP__
                properties.Remove<LongProperty>(key);                                                                          //__SILP__
                v = null;                                                                                                      //__SILP__
            }                                                                                                                  //__SILP__
            return v;                                                                                                          //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static LongProperty RemoveLong(this IDictProperties properties, string key) {                                   //__SILP__
            return properties.Remove<LongProperty>(key);                                                                       //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static bool AddLongValueChecker(this IDictProperties properties, string key, IValueChecker<long> checker) {     //__SILP__
            LongProperty p = properties.Get<LongProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                   //__SILP__
                return p.AddValueChecker(checker);                                                                             //__SILP__
            } else {                                                                                                           //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                              //__SILP__
            }                                                                                                                  //__SILP__
            return false;                                                                                                      //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static bool RemoveLongValueChecker(this IDictProperties properties, string key, IValueChecker<long> checker) {  //__SILP__
            LongProperty p = properties.Get<LongProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                   //__SILP__
                return p.RemoveValueChecker(checker);                                                                          //__SILP__
            } else {                                                                                                           //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                              //__SILP__
            }                                                                                                                  //__SILP__
            return false;                                                                                                      //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static BlockValueChecker<long> AddLongValueChecker(this IDictProperties properties, string key,                 //__SILP__
                                            IBlockOwner owner, Func<IVar<long>, long, bool> block) {                           //__SILP__
            LongProperty p = properties.Get<LongProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                   //__SILP__
                return p.AddValueChecker(owner, block);                                                                        //__SILP__
            } else {                                                                                                           //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                              //__SILP__
            }                                                                                                                  //__SILP__
            return null;                                                                                                       //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static bool AddLongValueWatcher(this IDictProperties properties, string key, IValueWatcher<long> watcher) {     //__SILP__
            LongProperty p = properties.Get<LongProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                   //__SILP__
                return p.AddValueWatcher(watcher);                                                                             //__SILP__
            } else {                                                                                                           //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                              //__SILP__
            }                                                                                                                  //__SILP__
            return false;                                                                                                      //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static bool RemoveLongValueWatcher(this IDictProperties properties, string key, IValueWatcher<long> watcher) {  //__SILP__
            LongProperty p = properties.Get<LongProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                   //__SILP__
                return p.RemoveValueWatcher(watcher);                                                                          //__SILP__
            } else {                                                                                                           //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                              //__SILP__
            }                                                                                                                  //__SILP__
            return false;                                                                                                      //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static BlockValueWatcher<long> AddLongValueWatcher(this IDictProperties properties, string key,                 //__SILP__
                                            IBlockOwner owner, Action<IVar<long>, long> block) {                               //__SILP__
            LongProperty p = properties.Get<LongProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                   //__SILP__
                return p.AddValueWatcher(owner, block);                                                                        //__SILP__
            } else {                                                                                                           //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                              //__SILP__
            }                                                                                                                  //__SILP__
            return null;                                                                                                       //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static bool IsLong(this IDictProperties properties, string key) {                                               //__SILP__
            return properties.Is<LongProperty>(key);                                                                           //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static long GetLong(this IDictProperties properties, string key) {                                              //__SILP__
            LongProperty v = properties.Get<LongProperty>(key);                                                                //__SILP__
            if (v != null) {                                                                                                   //__SILP__
                return v.Value;                                                                                                //__SILP__
            } else {                                                                                                           //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                              //__SILP__
            }                                                                                                                  //__SILP__
            return default(long);                                                                                              //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static long GetLong(this IDictProperties properties, string key, long defaultValue) {                           //__SILP__
            LongProperty v = properties.Get<LongProperty>(key, true);                                                          //__SILP__
            if (v != null) {                                                                                                   //__SILP__
                return v.Value;                                                                                                //__SILP__
            } else {                                                                                                           //__SILP__
                properties.Debug("Property Not Exist: {0}", key);                                                              //__SILP__
            }                                                                                                                  //__SILP__
            return defaultValue;                                                                                               //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static bool SetLong(this IDictProperties properties, string key, long val) {                                    //__SILP__
            LongProperty v = properties.Get<LongProperty>(key);                                                                //__SILP__
            if (v != null) {                                                                                                   //__SILP__
                return v.SetValue(val);                                                                                        //__SILP__
            } else {                                                                                                           //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                              //__SILP__
            }                                                                                                                  //__SILP__
            return false;                                                                                                      //__SILP__
        }                                                                                                                      //__SILP__
        //SILP: DICT_PROPERTIES_HELPER(Float, float)
        public static FloatProperty AddFloat(this IDictProperties properties, string key, float val) {                           //__SILP__
            FloatProperty v = properties.Add<FloatProperty>(key);                                                                //__SILP__
            if (v != null && !v.Setup(val)) {                                                                                    //__SILP__
                properties.Remove<FloatProperty>(key);                                                                           //__SILP__
                v = null;                                                                                                        //__SILP__
            }                                                                                                                    //__SILP__
            return v;                                                                                                            //__SILP__
        }                                                                                                                        //__SILP__
                                                                                                                                 //__SILP__
        public static FloatProperty RemoveFloat(this IDictProperties properties, string key) {                                   //__SILP__
            return properties.Remove<FloatProperty>(key);                                                                        //__SILP__
        }                                                                                                                        //__SILP__
                                                                                                                                 //__SILP__
        public static bool AddFloatValueChecker(this IDictProperties properties, string key, IValueChecker<float> checker) {     //__SILP__
            FloatProperty p = properties.Get<FloatProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                     //__SILP__
                return p.AddValueChecker(checker);                                                                               //__SILP__
            } else {                                                                                                             //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                                //__SILP__
            }                                                                                                                    //__SILP__
            return false;                                                                                                        //__SILP__
        }                                                                                                                        //__SILP__
                                                                                                                                 //__SILP__
        public static bool RemoveFloatValueChecker(this IDictProperties properties, string key, IValueChecker<float> checker) {  //__SILP__
            FloatProperty p = properties.Get<FloatProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                     //__SILP__
                return p.RemoveValueChecker(checker);                                                                            //__SILP__
            } else {                                                                                                             //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                                //__SILP__
            }                                                                                                                    //__SILP__
            return false;                                                                                                        //__SILP__
        }                                                                                                                        //__SILP__
                                                                                                                                 //__SILP__
        public static BlockValueChecker<float> AddFloatValueChecker(this IDictProperties properties, string key,                 //__SILP__
                                            IBlockOwner owner, Func<IVar<float>, float, bool> block) {                           //__SILP__
            FloatProperty p = properties.Get<FloatProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                     //__SILP__
                return p.AddValueChecker(owner, block);                                                                          //__SILP__
            } else {                                                                                                             //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                                //__SILP__
            }                                                                                                                    //__SILP__
            return null;                                                                                                         //__SILP__
        }                                                                                                                        //__SILP__
                                                                                                                                 //__SILP__
        public static bool AddFloatValueWatcher(this IDictProperties properties, string key, IValueWatcher<float> watcher) {     //__SILP__
            FloatProperty p = properties.Get<FloatProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                     //__SILP__
                return p.AddValueWatcher(watcher);                                                                               //__SILP__
            } else {                                                                                                             //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                                //__SILP__
            }                                                                                                                    //__SILP__
            return false;                                                                                                        //__SILP__
        }                                                                                                                        //__SILP__
                                                                                                                                 //__SILP__
        public static bool RemoveFloatValueWatcher(this IDictProperties properties, string key, IValueWatcher<float> watcher) {  //__SILP__
            FloatProperty p = properties.Get<FloatProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                     //__SILP__
                return p.RemoveValueWatcher(watcher);                                                                            //__SILP__
            } else {                                                                                                             //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                                //__SILP__
            }                                                                                                                    //__SILP__
            return false;                                                                                                        //__SILP__
        }                                                                                                                        //__SILP__
                                                                                                                                 //__SILP__
        public static BlockValueWatcher<float> AddFloatValueWatcher(this IDictProperties properties, string key,                 //__SILP__
                                            IBlockOwner owner, Action<IVar<float>, float> block) {                               //__SILP__
            FloatProperty p = properties.Get<FloatProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                     //__SILP__
                return p.AddValueWatcher(owner, block);                                                                          //__SILP__
            } else {                                                                                                             //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                                //__SILP__
            }                                                                                                                    //__SILP__
            return null;                                                                                                         //__SILP__
        }                                                                                                                        //__SILP__
                                                                                                                                 //__SILP__
        public static bool IsFloat(this IDictProperties properties, string key) {                                                //__SILP__
            return properties.Is<FloatProperty>(key);                                                                            //__SILP__
        }                                                                                                                        //__SILP__
                                                                                                                                 //__SILP__
        public static float GetFloat(this IDictProperties properties, string key) {                                              //__SILP__
            FloatProperty v = properties.Get<FloatProperty>(key);                                                                //__SILP__
            if (v != null) {                                                                                                     //__SILP__
                return v.Value;                                                                                                  //__SILP__
            } else {                                                                                                             //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                                //__SILP__
            }                                                                                                                    //__SILP__
            return default(float);                                                                                               //__SILP__
        }                                                                                                                        //__SILP__
                                                                                                                                 //__SILP__
        public static float GetFloat(this IDictProperties properties, string key, float defaultValue) {                          //__SILP__
            FloatProperty v = properties.Get<FloatProperty>(key, true);                                                          //__SILP__
            if (v != null) {                                                                                                     //__SILP__
                return v.Value;                                                                                                  //__SILP__
            } else {                                                                                                             //__SILP__
                properties.Debug("Property Not Exist: {0}", key);                                                                //__SILP__
            }                                                                                                                    //__SILP__
            return defaultValue;                                                                                                 //__SILP__
        }                                                                                                                        //__SILP__
                                                                                                                                 //__SILP__
        public static bool SetFloat(this IDictProperties properties, string key, float val) {                                    //__SILP__
            FloatProperty v = properties.Get<FloatProperty>(key);                                                                //__SILP__
            if (v != null) {                                                                                                     //__SILP__
                return v.SetValue(val);                                                                                          //__SILP__
            } else {                                                                                                             //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                                //__SILP__
            }                                                                                                                    //__SILP__
            return false;                                                                                                        //__SILP__
        }                                                                                                                        //__SILP__
        //SILP: DICT_PROPERTIES_HELPER(Double, double)
        public static DoubleProperty AddDouble(this IDictProperties properties, string key, double val) {                          //__SILP__
            DoubleProperty v = properties.Add<DoubleProperty>(key);                                                                //__SILP__
            if (v != null && !v.Setup(val)) {                                                                                      //__SILP__
                properties.Remove<DoubleProperty>(key);                                                                            //__SILP__
                v = null;                                                                                                          //__SILP__
            }                                                                                                                      //__SILP__
            return v;                                                                                                              //__SILP__
        }                                                                                                                          //__SILP__
                                                                                                                                   //__SILP__
        public static DoubleProperty RemoveDouble(this IDictProperties properties, string key) {                                   //__SILP__
            return properties.Remove<DoubleProperty>(key);                                                                         //__SILP__
        }                                                                                                                          //__SILP__
                                                                                                                                   //__SILP__
        public static bool AddDoubleValueChecker(this IDictProperties properties, string key, IValueChecker<double> checker) {     //__SILP__
            DoubleProperty p = properties.Get<DoubleProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                       //__SILP__
                return p.AddValueChecker(checker);                                                                                 //__SILP__
            } else {                                                                                                               //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                                  //__SILP__
            }                                                                                                                      //__SILP__
            return false;                                                                                                          //__SILP__
        }                                                                                                                          //__SILP__
                                                                                                                                   //__SILP__
        public static bool RemoveDoubleValueChecker(this IDictProperties properties, string key, IValueChecker<double> checker) {  //__SILP__
            DoubleProperty p = properties.Get<DoubleProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                       //__SILP__
                return p.RemoveValueChecker(checker);                                                                              //__SILP__
            } else {                                                                                                               //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                                  //__SILP__
            }                                                                                                                      //__SILP__
            return false;                                                                                                          //__SILP__
        }                                                                                                                          //__SILP__
                                                                                                                                   //__SILP__
        public static BlockValueChecker<double> AddDoubleValueChecker(this IDictProperties properties, string key,                 //__SILP__
                                            IBlockOwner owner, Func<IVar<double>, double, bool> block) {                           //__SILP__
            DoubleProperty p = properties.Get<DoubleProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                       //__SILP__
                return p.AddValueChecker(owner, block);                                                                            //__SILP__
            } else {                                                                                                               //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                                  //__SILP__
            }                                                                                                                      //__SILP__
            return null;                                                                                                           //__SILP__
        }                                                                                                                          //__SILP__
                                                                                                                                   //__SILP__
        public static bool AddDoubleValueWatcher(this IDictProperties properties, string key, IValueWatcher<double> watcher) {     //__SILP__
            DoubleProperty p = properties.Get<DoubleProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                       //__SILP__
                return p.AddValueWatcher(watcher);                                                                                 //__SILP__
            } else {                                                                                                               //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                                  //__SILP__
            }                                                                                                                      //__SILP__
            return false;                                                                                                          //__SILP__
        }                                                                                                                          //__SILP__
                                                                                                                                   //__SILP__
        public static bool RemoveDoubleValueWatcher(this IDictProperties properties, string key, IValueWatcher<double> watcher) {  //__SILP__
            DoubleProperty p = properties.Get<DoubleProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                       //__SILP__
                return p.RemoveValueWatcher(watcher);                                                                              //__SILP__
            } else {                                                                                                               //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                                  //__SILP__
            }                                                                                                                      //__SILP__
            return false;                                                                                                          //__SILP__
        }                                                                                                                          //__SILP__
                                                                                                                                   //__SILP__
        public static BlockValueWatcher<double> AddDoubleValueWatcher(this IDictProperties properties, string key,                 //__SILP__
                                            IBlockOwner owner, Action<IVar<double>, double> block) {                               //__SILP__
            DoubleProperty p = properties.Get<DoubleProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                       //__SILP__
                return p.AddValueWatcher(owner, block);                                                                            //__SILP__
            } else {                                                                                                               //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                                  //__SILP__
            }                                                                                                                      //__SILP__
            return null;                                                                                                           //__SILP__
        }                                                                                                                          //__SILP__
                                                                                                                                   //__SILP__
        public static bool IsDouble(this IDictProperties properties, string key) {                                                 //__SILP__
            return properties.Is<DoubleProperty>(key);                                                                             //__SILP__
        }                                                                                                                          //__SILP__
                                                                                                                                   //__SILP__
        public static double GetDouble(this IDictProperties properties, string key) {                                              //__SILP__
            DoubleProperty v = properties.Get<DoubleProperty>(key);                                                                //__SILP__
            if (v != null) {                                                                                                       //__SILP__
                return v.Value;                                                                                                    //__SILP__
            } else {                                                                                                               //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                                  //__SILP__
            }                                                                                                                      //__SILP__
            return default(double);                                                                                                //__SILP__
        }                                                                                                                          //__SILP__
                                                                                                                                   //__SILP__
        public static double GetDouble(this IDictProperties properties, string key, double defaultValue) {                         //__SILP__
            DoubleProperty v = properties.Get<DoubleProperty>(key, true);                                                          //__SILP__
            if (v != null) {                                                                                                       //__SILP__
                return v.Value;                                                                                                    //__SILP__
            } else {                                                                                                               //__SILP__
                properties.Debug("Property Not Exist: {0}", key);                                                                  //__SILP__
            }                                                                                                                      //__SILP__
            return defaultValue;                                                                                                   //__SILP__
        }                                                                                                                          //__SILP__
                                                                                                                                   //__SILP__
        public static bool SetDouble(this IDictProperties properties, string key, double val) {                                    //__SILP__
            DoubleProperty v = properties.Get<DoubleProperty>(key);                                                                //__SILP__
            if (v != null) {                                                                                                       //__SILP__
                return v.SetValue(val);                                                                                            //__SILP__
            } else {                                                                                                               //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                                  //__SILP__
            }                                                                                                                      //__SILP__
            return false;                                                                                                          //__SILP__
        }                                                                                                                          //__SILP__
        //SILP: DICT_PROPERTIES_HELPER(String, string)
        public static StringProperty AddString(this IDictProperties properties, string key, string val) {                          //__SILP__
            StringProperty v = properties.Add<StringProperty>(key);                                                                //__SILP__
            if (v != null && !v.Setup(val)) {                                                                                      //__SILP__
                properties.Remove<StringProperty>(key);                                                                            //__SILP__
                v = null;                                                                                                          //__SILP__
            }                                                                                                                      //__SILP__
            return v;                                                                                                              //__SILP__
        }                                                                                                                          //__SILP__
                                                                                                                                   //__SILP__
        public static StringProperty RemoveString(this IDictProperties properties, string key) {                                   //__SILP__
            return properties.Remove<StringProperty>(key);                                                                         //__SILP__
        }                                                                                                                          //__SILP__
                                                                                                                                   //__SILP__
        public static bool AddStringValueChecker(this IDictProperties properties, string key, IValueChecker<string> checker) {     //__SILP__
            StringProperty p = properties.Get<StringProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                       //__SILP__
                return p.AddValueChecker(checker);                                                                                 //__SILP__
            } else {                                                                                                               //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                                  //__SILP__
            }                                                                                                                      //__SILP__
            return false;                                                                                                          //__SILP__
        }                                                                                                                          //__SILP__
                                                                                                                                   //__SILP__
        public static bool RemoveStringValueChecker(this IDictProperties properties, string key, IValueChecker<string> checker) {  //__SILP__
            StringProperty p = properties.Get<StringProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                       //__SILP__
                return p.RemoveValueChecker(checker);                                                                              //__SILP__
            } else {                                                                                                               //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                                  //__SILP__
            }                                                                                                                      //__SILP__
            return false;                                                                                                          //__SILP__
        }                                                                                                                          //__SILP__
                                                                                                                                   //__SILP__
        public static BlockValueChecker<string> AddStringValueChecker(this IDictProperties properties, string key,                 //__SILP__
                                            IBlockOwner owner, Func<IVar<string>, string, bool> block) {                           //__SILP__
            StringProperty p = properties.Get<StringProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                       //__SILP__
                return p.AddValueChecker(owner, block);                                                                            //__SILP__
            } else {                                                                                                               //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                                  //__SILP__
            }                                                                                                                      //__SILP__
            return null;                                                                                                           //__SILP__
        }                                                                                                                          //__SILP__
                                                                                                                                   //__SILP__
        public static bool AddStringValueWatcher(this IDictProperties properties, string key, IValueWatcher<string> watcher) {     //__SILP__
            StringProperty p = properties.Get<StringProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                       //__SILP__
                return p.AddValueWatcher(watcher);                                                                                 //__SILP__
            } else {                                                                                                               //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                                  //__SILP__
            }                                                                                                                      //__SILP__
            return false;                                                                                                          //__SILP__
        }                                                                                                                          //__SILP__
                                                                                                                                   //__SILP__
        public static bool RemoveStringValueWatcher(this IDictProperties properties, string key, IValueWatcher<string> watcher) {  //__SILP__
            StringProperty p = properties.Get<StringProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                       //__SILP__
                return p.RemoveValueWatcher(watcher);                                                                              //__SILP__
            } else {                                                                                                               //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                                  //__SILP__
            }                                                                                                                      //__SILP__
            return false;                                                                                                          //__SILP__
        }                                                                                                                          //__SILP__
                                                                                                                                   //__SILP__
        public static BlockValueWatcher<string> AddStringValueWatcher(this IDictProperties properties, string key,                 //__SILP__
                                            IBlockOwner owner, Action<IVar<string>, string> block) {                               //__SILP__
            StringProperty p = properties.Get<StringProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                       //__SILP__
                return p.AddValueWatcher(owner, block);                                                                            //__SILP__
            } else {                                                                                                               //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                                  //__SILP__
            }                                                                                                                      //__SILP__
            return null;                                                                                                           //__SILP__
        }                                                                                                                          //__SILP__
                                                                                                                                   //__SILP__
        public static bool IsString(this IDictProperties properties, string key) {                                                 //__SILP__
            return properties.Is<StringProperty>(key);                                                                             //__SILP__
        }                                                                                                                          //__SILP__
                                                                                                                                   //__SILP__
        public static string GetString(this IDictProperties properties, string key) {                                              //__SILP__
            StringProperty v = properties.Get<StringProperty>(key);                                                                //__SILP__
            if (v != null) {                                                                                                       //__SILP__
                return v.Value;                                                                                                    //__SILP__
            } else {                                                                                                               //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                                  //__SILP__
            }                                                                                                                      //__SILP__
            return default(string);                                                                                                //__SILP__
        }                                                                                                                          //__SILP__
                                                                                                                                   //__SILP__
        public static string GetString(this IDictProperties properties, string key, string defaultValue) {                         //__SILP__
            StringProperty v = properties.Get<StringProperty>(key, true);                                                          //__SILP__
            if (v != null) {                                                                                                       //__SILP__
                return v.Value;                                                                                                    //__SILP__
            } else {                                                                                                               //__SILP__
                properties.Debug("Property Not Exist: {0}", key);                                                                  //__SILP__
            }                                                                                                                      //__SILP__
            return defaultValue;                                                                                                   //__SILP__
        }                                                                                                                          //__SILP__
                                                                                                                                   //__SILP__
        public static bool SetString(this IDictProperties properties, string key, string val) {                                    //__SILP__
            StringProperty v = properties.Get<StringProperty>(key);                                                                //__SILP__
            if (v != null) {                                                                                                       //__SILP__
                return v.SetValue(val);                                                                                            //__SILP__
            } else {                                                                                                               //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                                  //__SILP__
            }                                                                                                                      //__SILP__
            return false;                                                                                                          //__SILP__
        }                                                                                                                          //__SILP__
        //SILP: DICT_PROPERTIES_HELPER(Data, Data)
        public static DataProperty AddData(this IDictProperties properties, string key, Data val) {                            //__SILP__
            DataProperty v = properties.Add<DataProperty>(key);                                                                //__SILP__
            if (v != null && !v.Setup(val)) {                                                                                  //__SILP__
                properties.Remove<DataProperty>(key);                                                                          //__SILP__
                v = null;                                                                                                      //__SILP__
            }                                                                                                                  //__SILP__
            return v;                                                                                                          //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static DataProperty RemoveData(this IDictProperties properties, string key) {                                   //__SILP__
            return properties.Remove<DataProperty>(key);                                                                       //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static bool AddDataValueChecker(this IDictProperties properties, string key, IValueChecker<Data> checker) {     //__SILP__
            DataProperty p = properties.Get<DataProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                   //__SILP__
                return p.AddValueChecker(checker);                                                                             //__SILP__
            } else {                                                                                                           //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                              //__SILP__
            }                                                                                                                  //__SILP__
            return false;                                                                                                      //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static bool RemoveDataValueChecker(this IDictProperties properties, string key, IValueChecker<Data> checker) {  //__SILP__
            DataProperty p = properties.Get<DataProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                   //__SILP__
                return p.RemoveValueChecker(checker);                                                                          //__SILP__
            } else {                                                                                                           //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                              //__SILP__
            }                                                                                                                  //__SILP__
            return false;                                                                                                      //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static BlockValueChecker<Data> AddDataValueChecker(this IDictProperties properties, string key,                 //__SILP__
                                            IBlockOwner owner, Func<IVar<Data>, Data, bool> block) {                           //__SILP__
            DataProperty p = properties.Get<DataProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                   //__SILP__
                return p.AddValueChecker(owner, block);                                                                        //__SILP__
            } else {                                                                                                           //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                              //__SILP__
            }                                                                                                                  //__SILP__
            return null;                                                                                                       //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static bool AddDataValueWatcher(this IDictProperties properties, string key, IValueWatcher<Data> watcher) {     //__SILP__
            DataProperty p = properties.Get<DataProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                   //__SILP__
                return p.AddValueWatcher(watcher);                                                                             //__SILP__
            } else {                                                                                                           //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                              //__SILP__
            }                                                                                                                  //__SILP__
            return false;                                                                                                      //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static bool RemoveDataValueWatcher(this IDictProperties properties, string key, IValueWatcher<Data> watcher) {  //__SILP__
            DataProperty p = properties.Get<DataProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                   //__SILP__
                return p.RemoveValueWatcher(watcher);                                                                          //__SILP__
            } else {                                                                                                           //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                              //__SILP__
            }                                                                                                                  //__SILP__
            return false;                                                                                                      //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static BlockValueWatcher<Data> AddDataValueWatcher(this IDictProperties properties, string key,                 //__SILP__
                                            IBlockOwner owner, Action<IVar<Data>, Data> block) {                               //__SILP__
            DataProperty p = properties.Get<DataProperty>(key);                                                                //__SILP__
            if (p != null) {                                                                                                   //__SILP__
                return p.AddValueWatcher(owner, block);                                                                        //__SILP__
            } else {                                                                                                           //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                              //__SILP__
            }                                                                                                                  //__SILP__
            return null;                                                                                                       //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static bool IsData(this IDictProperties properties, string key) {                                               //__SILP__
            return properties.Is<DataProperty>(key);                                                                           //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static Data GetData(this IDictProperties properties, string key) {                                              //__SILP__
            DataProperty v = properties.Get<DataProperty>(key);                                                                //__SILP__
            if (v != null) {                                                                                                   //__SILP__
                return v.Value;                                                                                                //__SILP__
            } else {                                                                                                           //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                              //__SILP__
            }                                                                                                                  //__SILP__
            return default(Data);                                                                                              //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static Data GetData(this IDictProperties properties, string key, Data defaultValue) {                           //__SILP__
            DataProperty v = properties.Get<DataProperty>(key, true);                                                          //__SILP__
            if (v != null) {                                                                                                   //__SILP__
                return v.Value;                                                                                                //__SILP__
            } else {                                                                                                           //__SILP__
                properties.Debug("Property Not Exist: {0}", key);                                                              //__SILP__
            }                                                                                                                  //__SILP__
            return defaultValue;                                                                                               //__SILP__
        }                                                                                                                      //__SILP__
                                                                                                                               //__SILP__
        public static bool SetData(this IDictProperties properties, string key, Data val) {                                    //__SILP__
            DataProperty v = properties.Get<DataProperty>(key);                                                                //__SILP__
            if (v != null) {                                                                                                   //__SILP__
                return v.SetValue(val);                                                                                        //__SILP__
            } else {                                                                                                           //__SILP__
                properties.Error("Property Not Exist: {0}", key);                                                              //__SILP__
            }                                                                                                                  //__SILP__
            return false;                                                                                                      //__SILP__
        }                                                                                                                      //__SILP__
    }
}
