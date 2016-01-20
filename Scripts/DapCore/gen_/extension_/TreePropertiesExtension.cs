using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class PropertiesExtension {
        public static IProperty AddProperty<T>(this ITreeProperties<T> properties,
                Pass pass, string path, Pass propertyPass, bool open, Data data)
                                                    where T : class, IProperty {
            if (data == null) return null;
            if (!properties.CheckAdminPass(pass)) return null;

            string type = data.GetString(ObjectConsts.KeyType);
            if (string.IsNullOrEmpty(type)) {
                properties.Error("Invalid Property data: {0}, {1}", path, data);
                return null;
            }
            IProperty prop = properties.New(type, path, open ? propertyPass.Open : propertyPass);
            if (prop == null) {
                properties.Error("Failed to Add Property: {0}, {1}", path, data);
                return null;
            }
            if (!prop.Decode(pass, data)) {
                properties.Error("Failed to Decode Property: {0}, {1} -> {2}", path, data, prop);
            }

            return prop;
        }

        public static IProperty AddProperty<T>(this ITreeProperties<T> properties,
                string path, Pass propertyPass, bool open, Data data)
                                                    where T : class, IProperty {
            return AddProperty<T>(properties, null, path, propertyPass, open, data);
        }

        //SILP: TREE_PROPERTIES_HELPER(Bool, bool)
        public static BoolProperty AddBool(this ITreeProperties<IProperty> properties, string path, Pass propertyPass, bool val) {                            //__SILP__
            BoolProperty v = properties.Add<BoolProperty>(path, propertyPass);                                                                                //__SILP__
            if (v != null && !v.Setup(propertyPass, val)) {                                                                                                   //__SILP__
                properties.Remove<BoolProperty>(path);                                                                                                        //__SILP__
                v = null;                                                                                                                                     //__SILP__
            }                                                                                                                                                 //__SILP__
            return v;                                                                                                                                         //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static BoolProperty AddBool(this ITreeProperties<IProperty> properties, string path, bool val) {                                               //__SILP__
            return properties.AddBool(path, null, val);                                                                                                       //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static BoolProperty RemoveBool(this ITreeProperties<IProperty> properties, string path, Pass propertyPass) {                                   //__SILP__
            return properties.Remove<BoolProperty>(path, propertyPass);                                                                                       //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static BoolProperty RemoveBool(this ITreeProperties<IProperty> properties, string path) {                                                      //__SILP__
            return properties.Remove<BoolProperty>(path);                                                                                                     //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static bool AddBoolValueChecker(this ITreeProperties<IProperty> properties, string path, Pass propertyPass, IValueChecker<bool> checker) {     //__SILP__
            BoolProperty p = properties.Get<BoolProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                  //__SILP__
                return p.AddValueChecker(propertyPass, checker);                                                                                              //__SILP__
            } else {                                                                                                                                          //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                            //__SILP__
            }                                                                                                                                                 //__SILP__
            return false;                                                                                                                                     //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static bool AddBoolValueChecker(this ITreeProperties<IProperty> properties, string path, IValueChecker<bool> checker) {                        //__SILP__
            return AddBoolValueChecker(properties, path, null, checker);                                                                                      //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static bool RemoveBoolValueChecker(this ITreeProperties<IProperty> properties, string path, Pass propertyPass, IValueChecker<bool> checker) {  //__SILP__
            BoolProperty p = properties.Get<BoolProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                  //__SILP__
                return p.RemoveValueChecker(propertyPass, checker);                                                                                           //__SILP__
            } else {                                                                                                                                          //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                            //__SILP__
            }                                                                                                                                                 //__SILP__
            return false;                                                                                                                                     //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static bool RemoveBoolValueChecker(this ITreeProperties<IProperty> properties, string path, IValueChecker<bool> checker) {                     //__SILP__
            return RemoveBoolValueChecker(properties, path, null, checker);                                                                                   //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static BlockValueChecker<bool> AddBoolBlockValueChecker(this ITreeProperties<IProperty> properties, string path, Pass propertyPass,            //__SILP__
                                            IBlockOwner owner, Func<IVar<bool>, bool, bool> checker) {                                                        //__SILP__
            BoolProperty p = properties.Get<BoolProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                  //__SILP__
                return p.AddBlockValueChecker(propertyPass, owner, checker);                                                                                  //__SILP__
            } else {                                                                                                                                          //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                            //__SILP__
            }                                                                                                                                                 //__SILP__
            return null;                                                                                                                                      //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static BlockValueChecker<bool> AddBoolBlockValueChecker(this ITreeProperties<IProperty> properties, string path,                               //__SILP__
                                            IBlockOwner owner, Func<IVar<bool>, bool, bool> checker) {                                                        //__SILP__
            return AddBoolBlockValueChecker(properties, path, null, owner, checker);                                                                          //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static bool AddBoolValueWatcher(this ITreeProperties<IProperty> properties, string path, IValueWatcher<bool> watcher) {                        //__SILP__
            BoolProperty p = properties.Get<BoolProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                  //__SILP__
                return p.AddValueWatcher(watcher);                                                                                                            //__SILP__
            } else {                                                                                                                                          //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                            //__SILP__
            }                                                                                                                                                 //__SILP__
            return false;                                                                                                                                     //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static bool RemoveBoolValueWatcher(this ITreeProperties<IProperty> properties, string path, IValueWatcher<bool> watcher) {                     //__SILP__
            BoolProperty p = properties.Get<BoolProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                  //__SILP__
                return p.RemoveValueWatcher(watcher);                                                                                                         //__SILP__
            } else {                                                                                                                                          //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                            //__SILP__
            }                                                                                                                                                 //__SILP__
            return false;                                                                                                                                     //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static BlockValueWatcher<bool> AddBoolBlockValueWatcher(this ITreeProperties<IProperty> properties, string path,                               //__SILP__
                                            IBlockOwner owner, Action<IVar<bool>, bool> watcher) {                                                            //__SILP__
            BoolProperty p = properties.Get<BoolProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                  //__SILP__
                return p.AddBlockValueWatcher(owner, watcher);                                                                                                //__SILP__
            } else {                                                                                                                                          //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                            //__SILP__
            }                                                                                                                                                 //__SILP__
            return null;                                                                                                                                      //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static bool IsBool(this ITreeProperties<IProperty> properties, string path) {                                                                  //__SILP__
            return properties.Is<BoolProperty>(path);                                                                                                         //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static bool GetBool(this ITreeProperties<IProperty> properties, string path) {                                                                 //__SILP__
            BoolProperty v = properties.Get<BoolProperty>(path);                                                                                              //__SILP__
            if (v != null) {                                                                                                                                  //__SILP__
                return v.Value;                                                                                                                               //__SILP__
            } else {                                                                                                                                          //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                            //__SILP__
            }                                                                                                                                                 //__SILP__
            return default(bool);                                                                                                                             //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static bool GetBool(this ITreeProperties<IProperty> properties, string path, bool defaultValue) {                                              //__SILP__
            BoolProperty v = properties.Get<BoolProperty>(path);                                                                                              //__SILP__
            if (v != null) {                                                                                                                                  //__SILP__
                return v.Value;                                                                                                                               //__SILP__
            } else {                                                                                                                                          //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                            //__SILP__
            }                                                                                                                                                 //__SILP__
            return defaultValue;                                                                                                                              //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
                                                                                                                                                              //__SILP__
        public static bool SetBool(this ITreeProperties<IProperty> properties, string path, Pass propertyPass, bool val) {                                    //__SILP__
            BoolProperty v = properties.Get<BoolProperty>(path);                                                                                              //__SILP__
            if (v != null) {                                                                                                                                  //__SILP__
                return v.SetValue(propertyPass, val);                                                                                                         //__SILP__
            } else {                                                                                                                                          //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                            //__SILP__
            }                                                                                                                                                 //__SILP__
            return false;                                                                                                                                     //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static bool SetBool(this ITreeProperties<IProperty> properties, string path, bool val) {                                                       //__SILP__
            BoolProperty v = properties.Get<BoolProperty>(path);                                                                                              //__SILP__
            if (v != null) {                                                                                                                                  //__SILP__
                return v.SetValue(val);                                                                                                                       //__SILP__
            } else {                                                                                                                                          //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                            //__SILP__
            }                                                                                                                                                 //__SILP__
            return false;                                                                                                                                     //__SILP__
        }                                                                                                                                                     //__SILP__
        //SILP: TREE_PROPERTIES_HELPER(Int, int)
        public static IntProperty AddInt(this ITreeProperties<IProperty> properties, string path, Pass propertyPass, int val) {                             //__SILP__
            IntProperty v = properties.Add<IntProperty>(path, propertyPass);                                                                                //__SILP__
            if (v != null && !v.Setup(propertyPass, val)) {                                                                                                 //__SILP__
                properties.Remove<IntProperty>(path);                                                                                                       //__SILP__
                v = null;                                                                                                                                   //__SILP__
            }                                                                                                                                               //__SILP__
            return v;                                                                                                                                       //__SILP__
        }                                                                                                                                                   //__SILP__
                                                                                                                                                            //__SILP__
        public static IntProperty AddInt(this ITreeProperties<IProperty> properties, string path, int val) {                                                //__SILP__
            return properties.AddInt(path, null, val);                                                                                                      //__SILP__
        }                                                                                                                                                   //__SILP__
                                                                                                                                                            //__SILP__
        public static IntProperty RemoveInt(this ITreeProperties<IProperty> properties, string path, Pass propertyPass) {                                   //__SILP__
            return properties.Remove<IntProperty>(path, propertyPass);                                                                                      //__SILP__
        }                                                                                                                                                   //__SILP__
                                                                                                                                                            //__SILP__
        public static IntProperty RemoveInt(this ITreeProperties<IProperty> properties, string path) {                                                      //__SILP__
            return properties.Remove<IntProperty>(path);                                                                                                    //__SILP__
        }                                                                                                                                                   //__SILP__
                                                                                                                                                            //__SILP__
        public static bool AddIntValueChecker(this ITreeProperties<IProperty> properties, string path, Pass propertyPass, IValueChecker<int> checker) {     //__SILP__
            IntProperty p = properties.Get<IntProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                //__SILP__
                return p.AddValueChecker(propertyPass, checker);                                                                                            //__SILP__
            } else {                                                                                                                                        //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                          //__SILP__
            }                                                                                                                                               //__SILP__
            return false;                                                                                                                                   //__SILP__
        }                                                                                                                                                   //__SILP__
                                                                                                                                                            //__SILP__
        public static bool AddIntValueChecker(this ITreeProperties<IProperty> properties, string path, IValueChecker<int> checker) {                        //__SILP__
            return AddIntValueChecker(properties, path, null, checker);                                                                                     //__SILP__
        }                                                                                                                                                   //__SILP__
                                                                                                                                                            //__SILP__
        public static bool RemoveIntValueChecker(this ITreeProperties<IProperty> properties, string path, Pass propertyPass, IValueChecker<int> checker) {  //__SILP__
            IntProperty p = properties.Get<IntProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                //__SILP__
                return p.RemoveValueChecker(propertyPass, checker);                                                                                         //__SILP__
            } else {                                                                                                                                        //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                          //__SILP__
            }                                                                                                                                               //__SILP__
            return false;                                                                                                                                   //__SILP__
        }                                                                                                                                                   //__SILP__
                                                                                                                                                            //__SILP__
        public static bool RemoveIntValueChecker(this ITreeProperties<IProperty> properties, string path, IValueChecker<int> checker) {                     //__SILP__
            return RemoveIntValueChecker(properties, path, null, checker);                                                                                  //__SILP__
        }                                                                                                                                                   //__SILP__
                                                                                                                                                            //__SILP__
        public static BlockValueChecker<int> AddIntBlockValueChecker(this ITreeProperties<IProperty> properties, string path, Pass propertyPass,            //__SILP__
                                            IBlockOwner owner, Func<IVar<int>, int, bool> checker) {                                                        //__SILP__
            IntProperty p = properties.Get<IntProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                //__SILP__
                return p.AddBlockValueChecker(propertyPass, owner, checker);                                                                                //__SILP__
            } else {                                                                                                                                        //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                          //__SILP__
            }                                                                                                                                               //__SILP__
            return null;                                                                                                                                    //__SILP__
        }                                                                                                                                                   //__SILP__
                                                                                                                                                            //__SILP__
        public static BlockValueChecker<int> AddIntBlockValueChecker(this ITreeProperties<IProperty> properties, string path,                               //__SILP__
                                            IBlockOwner owner, Func<IVar<int>, int, bool> checker) {                                                        //__SILP__
            return AddIntBlockValueChecker(properties, path, null, owner, checker);                                                                         //__SILP__
        }                                                                                                                                                   //__SILP__
                                                                                                                                                            //__SILP__
        public static bool AddIntValueWatcher(this ITreeProperties<IProperty> properties, string path, IValueWatcher<int> watcher) {                        //__SILP__
            IntProperty p = properties.Get<IntProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                //__SILP__
                return p.AddValueWatcher(watcher);                                                                                                          //__SILP__
            } else {                                                                                                                                        //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                          //__SILP__
            }                                                                                                                                               //__SILP__
            return false;                                                                                                                                   //__SILP__
        }                                                                                                                                                   //__SILP__
                                                                                                                                                            //__SILP__
        public static bool RemoveIntValueWatcher(this ITreeProperties<IProperty> properties, string path, IValueWatcher<int> watcher) {                     //__SILP__
            IntProperty p = properties.Get<IntProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                //__SILP__
                return p.RemoveValueWatcher(watcher);                                                                                                       //__SILP__
            } else {                                                                                                                                        //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                          //__SILP__
            }                                                                                                                                               //__SILP__
            return false;                                                                                                                                   //__SILP__
        }                                                                                                                                                   //__SILP__
                                                                                                                                                            //__SILP__
        public static BlockValueWatcher<int> AddIntBlockValueWatcher(this ITreeProperties<IProperty> properties, string path,                               //__SILP__
                                            IBlockOwner owner, Action<IVar<int>, int> watcher) {                                                            //__SILP__
            IntProperty p = properties.Get<IntProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                //__SILP__
                return p.AddBlockValueWatcher(owner, watcher);                                                                                              //__SILP__
            } else {                                                                                                                                        //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                          //__SILP__
            }                                                                                                                                               //__SILP__
            return null;                                                                                                                                    //__SILP__
        }                                                                                                                                                   //__SILP__
                                                                                                                                                            //__SILP__
        public static bool IsInt(this ITreeProperties<IProperty> properties, string path) {                                                                 //__SILP__
            return properties.Is<IntProperty>(path);                                                                                                        //__SILP__
        }                                                                                                                                                   //__SILP__
                                                                                                                                                            //__SILP__
        public static int GetInt(this ITreeProperties<IProperty> properties, string path) {                                                                 //__SILP__
            IntProperty v = properties.Get<IntProperty>(path);                                                                                              //__SILP__
            if (v != null) {                                                                                                                                //__SILP__
                return v.Value;                                                                                                                             //__SILP__
            } else {                                                                                                                                        //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                          //__SILP__
            }                                                                                                                                               //__SILP__
            return default(int);                                                                                                                            //__SILP__
        }                                                                                                                                                   //__SILP__
                                                                                                                                                            //__SILP__
        public static int GetInt(this ITreeProperties<IProperty> properties, string path, int defaultValue) {                                               //__SILP__
            IntProperty v = properties.Get<IntProperty>(path);                                                                                              //__SILP__
            if (v != null) {                                                                                                                                //__SILP__
                return v.Value;                                                                                                                             //__SILP__
            } else {                                                                                                                                        //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                          //__SILP__
            }                                                                                                                                               //__SILP__
            return defaultValue;                                                                                                                            //__SILP__
        }                                                                                                                                                   //__SILP__
                                                                                                                                                            //__SILP__
                                                                                                                                                            //__SILP__
        public static bool SetInt(this ITreeProperties<IProperty> properties, string path, Pass propertyPass, int val) {                                    //__SILP__
            IntProperty v = properties.Get<IntProperty>(path);                                                                                              //__SILP__
            if (v != null) {                                                                                                                                //__SILP__
                return v.SetValue(propertyPass, val);                                                                                                       //__SILP__
            } else {                                                                                                                                        //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                          //__SILP__
            }                                                                                                                                               //__SILP__
            return false;                                                                                                                                   //__SILP__
        }                                                                                                                                                   //__SILP__
                                                                                                                                                            //__SILP__
        public static bool SetInt(this ITreeProperties<IProperty> properties, string path, int val) {                                                       //__SILP__
            IntProperty v = properties.Get<IntProperty>(path);                                                                                              //__SILP__
            if (v != null) {                                                                                                                                //__SILP__
                return v.SetValue(val);                                                                                                                     //__SILP__
            } else {                                                                                                                                        //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                          //__SILP__
            }                                                                                                                                               //__SILP__
            return false;                                                                                                                                   //__SILP__
        }                                                                                                                                                   //__SILP__
        //SILP: TREE_PROPERTIES_HELPER(Long, long)
        public static LongProperty AddLong(this ITreeProperties<IProperty> properties, string path, Pass propertyPass, long val) {                            //__SILP__
            LongProperty v = properties.Add<LongProperty>(path, propertyPass);                                                                                //__SILP__
            if (v != null && !v.Setup(propertyPass, val)) {                                                                                                   //__SILP__
                properties.Remove<LongProperty>(path);                                                                                                        //__SILP__
                v = null;                                                                                                                                     //__SILP__
            }                                                                                                                                                 //__SILP__
            return v;                                                                                                                                         //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static LongProperty AddLong(this ITreeProperties<IProperty> properties, string path, long val) {                                               //__SILP__
            return properties.AddLong(path, null, val);                                                                                                       //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static LongProperty RemoveLong(this ITreeProperties<IProperty> properties, string path, Pass propertyPass) {                                   //__SILP__
            return properties.Remove<LongProperty>(path, propertyPass);                                                                                       //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static LongProperty RemoveLong(this ITreeProperties<IProperty> properties, string path) {                                                      //__SILP__
            return properties.Remove<LongProperty>(path);                                                                                                     //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static bool AddLongValueChecker(this ITreeProperties<IProperty> properties, string path, Pass propertyPass, IValueChecker<long> checker) {     //__SILP__
            LongProperty p = properties.Get<LongProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                  //__SILP__
                return p.AddValueChecker(propertyPass, checker);                                                                                              //__SILP__
            } else {                                                                                                                                          //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                            //__SILP__
            }                                                                                                                                                 //__SILP__
            return false;                                                                                                                                     //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static bool AddLongValueChecker(this ITreeProperties<IProperty> properties, string path, IValueChecker<long> checker) {                        //__SILP__
            return AddLongValueChecker(properties, path, null, checker);                                                                                      //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static bool RemoveLongValueChecker(this ITreeProperties<IProperty> properties, string path, Pass propertyPass, IValueChecker<long> checker) {  //__SILP__
            LongProperty p = properties.Get<LongProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                  //__SILP__
                return p.RemoveValueChecker(propertyPass, checker);                                                                                           //__SILP__
            } else {                                                                                                                                          //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                            //__SILP__
            }                                                                                                                                                 //__SILP__
            return false;                                                                                                                                     //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static bool RemoveLongValueChecker(this ITreeProperties<IProperty> properties, string path, IValueChecker<long> checker) {                     //__SILP__
            return RemoveLongValueChecker(properties, path, null, checker);                                                                                   //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static BlockValueChecker<long> AddLongBlockValueChecker(this ITreeProperties<IProperty> properties, string path, Pass propertyPass,            //__SILP__
                                            IBlockOwner owner, Func<IVar<long>, long, bool> checker) {                                                        //__SILP__
            LongProperty p = properties.Get<LongProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                  //__SILP__
                return p.AddBlockValueChecker(propertyPass, owner, checker);                                                                                  //__SILP__
            } else {                                                                                                                                          //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                            //__SILP__
            }                                                                                                                                                 //__SILP__
            return null;                                                                                                                                      //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static BlockValueChecker<long> AddLongBlockValueChecker(this ITreeProperties<IProperty> properties, string path,                               //__SILP__
                                            IBlockOwner owner, Func<IVar<long>, long, bool> checker) {                                                        //__SILP__
            return AddLongBlockValueChecker(properties, path, null, owner, checker);                                                                          //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static bool AddLongValueWatcher(this ITreeProperties<IProperty> properties, string path, IValueWatcher<long> watcher) {                        //__SILP__
            LongProperty p = properties.Get<LongProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                  //__SILP__
                return p.AddValueWatcher(watcher);                                                                                                            //__SILP__
            } else {                                                                                                                                          //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                            //__SILP__
            }                                                                                                                                                 //__SILP__
            return false;                                                                                                                                     //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static bool RemoveLongValueWatcher(this ITreeProperties<IProperty> properties, string path, IValueWatcher<long> watcher) {                     //__SILP__
            LongProperty p = properties.Get<LongProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                  //__SILP__
                return p.RemoveValueWatcher(watcher);                                                                                                         //__SILP__
            } else {                                                                                                                                          //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                            //__SILP__
            }                                                                                                                                                 //__SILP__
            return false;                                                                                                                                     //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static BlockValueWatcher<long> AddLongBlockValueWatcher(this ITreeProperties<IProperty> properties, string path,                               //__SILP__
                                            IBlockOwner owner, Action<IVar<long>, long> watcher) {                                                            //__SILP__
            LongProperty p = properties.Get<LongProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                  //__SILP__
                return p.AddBlockValueWatcher(owner, watcher);                                                                                                //__SILP__
            } else {                                                                                                                                          //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                            //__SILP__
            }                                                                                                                                                 //__SILP__
            return null;                                                                                                                                      //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static bool IsLong(this ITreeProperties<IProperty> properties, string path) {                                                                  //__SILP__
            return properties.Is<LongProperty>(path);                                                                                                         //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static long GetLong(this ITreeProperties<IProperty> properties, string path) {                                                                 //__SILP__
            LongProperty v = properties.Get<LongProperty>(path);                                                                                              //__SILP__
            if (v != null) {                                                                                                                                  //__SILP__
                return v.Value;                                                                                                                               //__SILP__
            } else {                                                                                                                                          //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                            //__SILP__
            }                                                                                                                                                 //__SILP__
            return default(long);                                                                                                                             //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static long GetLong(this ITreeProperties<IProperty> properties, string path, long defaultValue) {                                              //__SILP__
            LongProperty v = properties.Get<LongProperty>(path);                                                                                              //__SILP__
            if (v != null) {                                                                                                                                  //__SILP__
                return v.Value;                                                                                                                               //__SILP__
            } else {                                                                                                                                          //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                            //__SILP__
            }                                                                                                                                                 //__SILP__
            return defaultValue;                                                                                                                              //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
                                                                                                                                                              //__SILP__
        public static bool SetLong(this ITreeProperties<IProperty> properties, string path, Pass propertyPass, long val) {                                    //__SILP__
            LongProperty v = properties.Get<LongProperty>(path);                                                                                              //__SILP__
            if (v != null) {                                                                                                                                  //__SILP__
                return v.SetValue(propertyPass, val);                                                                                                         //__SILP__
            } else {                                                                                                                                          //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                            //__SILP__
            }                                                                                                                                                 //__SILP__
            return false;                                                                                                                                     //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static bool SetLong(this ITreeProperties<IProperty> properties, string path, long val) {                                                       //__SILP__
            LongProperty v = properties.Get<LongProperty>(path);                                                                                              //__SILP__
            if (v != null) {                                                                                                                                  //__SILP__
                return v.SetValue(val);                                                                                                                       //__SILP__
            } else {                                                                                                                                          //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                            //__SILP__
            }                                                                                                                                                 //__SILP__
            return false;                                                                                                                                     //__SILP__
        }                                                                                                                                                     //__SILP__
        //SILP: TREE_PROPERTIES_HELPER(Float, float)
        public static FloatProperty AddFloat(this ITreeProperties<IProperty> properties, string path, Pass propertyPass, float val) {                           //__SILP__
            FloatProperty v = properties.Add<FloatProperty>(path, propertyPass);                                                                                //__SILP__
            if (v != null && !v.Setup(propertyPass, val)) {                                                                                                     //__SILP__
                properties.Remove<FloatProperty>(path);                                                                                                         //__SILP__
                v = null;                                                                                                                                       //__SILP__
            }                                                                                                                                                   //__SILP__
            return v;                                                                                                                                           //__SILP__
        }                                                                                                                                                       //__SILP__
                                                                                                                                                                //__SILP__
        public static FloatProperty AddFloat(this ITreeProperties<IProperty> properties, string path, float val) {                                              //__SILP__
            return properties.AddFloat(path, null, val);                                                                                                        //__SILP__
        }                                                                                                                                                       //__SILP__
                                                                                                                                                                //__SILP__
        public static FloatProperty RemoveFloat(this ITreeProperties<IProperty> properties, string path, Pass propertyPass) {                                   //__SILP__
            return properties.Remove<FloatProperty>(path, propertyPass);                                                                                        //__SILP__
        }                                                                                                                                                       //__SILP__
                                                                                                                                                                //__SILP__
        public static FloatProperty RemoveFloat(this ITreeProperties<IProperty> properties, string path) {                                                      //__SILP__
            return properties.Remove<FloatProperty>(path);                                                                                                      //__SILP__
        }                                                                                                                                                       //__SILP__
                                                                                                                                                                //__SILP__
        public static bool AddFloatValueChecker(this ITreeProperties<IProperty> properties, string path, Pass propertyPass, IValueChecker<float> checker) {     //__SILP__
            FloatProperty p = properties.Get<FloatProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                    //__SILP__
                return p.AddValueChecker(propertyPass, checker);                                                                                                //__SILP__
            } else {                                                                                                                                            //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                              //__SILP__
            }                                                                                                                                                   //__SILP__
            return false;                                                                                                                                       //__SILP__
        }                                                                                                                                                       //__SILP__
                                                                                                                                                                //__SILP__
        public static bool AddFloatValueChecker(this ITreeProperties<IProperty> properties, string path, IValueChecker<float> checker) {                        //__SILP__
            return AddFloatValueChecker(properties, path, null, checker);                                                                                       //__SILP__
        }                                                                                                                                                       //__SILP__
                                                                                                                                                                //__SILP__
        public static bool RemoveFloatValueChecker(this ITreeProperties<IProperty> properties, string path, Pass propertyPass, IValueChecker<float> checker) {  //__SILP__
            FloatProperty p = properties.Get<FloatProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                    //__SILP__
                return p.RemoveValueChecker(propertyPass, checker);                                                                                             //__SILP__
            } else {                                                                                                                                            //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                              //__SILP__
            }                                                                                                                                                   //__SILP__
            return false;                                                                                                                                       //__SILP__
        }                                                                                                                                                       //__SILP__
                                                                                                                                                                //__SILP__
        public static bool RemoveFloatValueChecker(this ITreeProperties<IProperty> properties, string path, IValueChecker<float> checker) {                     //__SILP__
            return RemoveFloatValueChecker(properties, path, null, checker);                                                                                    //__SILP__
        }                                                                                                                                                       //__SILP__
                                                                                                                                                                //__SILP__
        public static BlockValueChecker<float> AddFloatBlockValueChecker(this ITreeProperties<IProperty> properties, string path, Pass propertyPass,            //__SILP__
                                            IBlockOwner owner, Func<IVar<float>, float, bool> checker) {                                                        //__SILP__
            FloatProperty p = properties.Get<FloatProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                    //__SILP__
                return p.AddBlockValueChecker(propertyPass, owner, checker);                                                                                    //__SILP__
            } else {                                                                                                                                            //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                              //__SILP__
            }                                                                                                                                                   //__SILP__
            return null;                                                                                                                                        //__SILP__
        }                                                                                                                                                       //__SILP__
                                                                                                                                                                //__SILP__
        public static BlockValueChecker<float> AddFloatBlockValueChecker(this ITreeProperties<IProperty> properties, string path,                               //__SILP__
                                            IBlockOwner owner, Func<IVar<float>, float, bool> checker) {                                                        //__SILP__
            return AddFloatBlockValueChecker(properties, path, null, owner, checker);                                                                           //__SILP__
        }                                                                                                                                                       //__SILP__
                                                                                                                                                                //__SILP__
        public static bool AddFloatValueWatcher(this ITreeProperties<IProperty> properties, string path, IValueWatcher<float> watcher) {                        //__SILP__
            FloatProperty p = properties.Get<FloatProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                    //__SILP__
                return p.AddValueWatcher(watcher);                                                                                                              //__SILP__
            } else {                                                                                                                                            //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                              //__SILP__
            }                                                                                                                                                   //__SILP__
            return false;                                                                                                                                       //__SILP__
        }                                                                                                                                                       //__SILP__
                                                                                                                                                                //__SILP__
        public static bool RemoveFloatValueWatcher(this ITreeProperties<IProperty> properties, string path, IValueWatcher<float> watcher) {                     //__SILP__
            FloatProperty p = properties.Get<FloatProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                    //__SILP__
                return p.RemoveValueWatcher(watcher);                                                                                                           //__SILP__
            } else {                                                                                                                                            //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                              //__SILP__
            }                                                                                                                                                   //__SILP__
            return false;                                                                                                                                       //__SILP__
        }                                                                                                                                                       //__SILP__
                                                                                                                                                                //__SILP__
        public static BlockValueWatcher<float> AddFloatBlockValueWatcher(this ITreeProperties<IProperty> properties, string path,                               //__SILP__
                                            IBlockOwner owner, Action<IVar<float>, float> watcher) {                                                            //__SILP__
            FloatProperty p = properties.Get<FloatProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                    //__SILP__
                return p.AddBlockValueWatcher(owner, watcher);                                                                                                  //__SILP__
            } else {                                                                                                                                            //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                              //__SILP__
            }                                                                                                                                                   //__SILP__
            return null;                                                                                                                                        //__SILP__
        }                                                                                                                                                       //__SILP__
                                                                                                                                                                //__SILP__
        public static bool IsFloat(this ITreeProperties<IProperty> properties, string path) {                                                                   //__SILP__
            return properties.Is<FloatProperty>(path);                                                                                                          //__SILP__
        }                                                                                                                                                       //__SILP__
                                                                                                                                                                //__SILP__
        public static float GetFloat(this ITreeProperties<IProperty> properties, string path) {                                                                 //__SILP__
            FloatProperty v = properties.Get<FloatProperty>(path);                                                                                              //__SILP__
            if (v != null) {                                                                                                                                    //__SILP__
                return v.Value;                                                                                                                                 //__SILP__
            } else {                                                                                                                                            //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                              //__SILP__
            }                                                                                                                                                   //__SILP__
            return default(float);                                                                                                                              //__SILP__
        }                                                                                                                                                       //__SILP__
                                                                                                                                                                //__SILP__
        public static float GetFloat(this ITreeProperties<IProperty> properties, string path, float defaultValue) {                                             //__SILP__
            FloatProperty v = properties.Get<FloatProperty>(path);                                                                                              //__SILP__
            if (v != null) {                                                                                                                                    //__SILP__
                return v.Value;                                                                                                                                 //__SILP__
            } else {                                                                                                                                            //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                              //__SILP__
            }                                                                                                                                                   //__SILP__
            return defaultValue;                                                                                                                                //__SILP__
        }                                                                                                                                                       //__SILP__
                                                                                                                                                                //__SILP__
                                                                                                                                                                //__SILP__
        public static bool SetFloat(this ITreeProperties<IProperty> properties, string path, Pass propertyPass, float val) {                                    //__SILP__
            FloatProperty v = properties.Get<FloatProperty>(path);                                                                                              //__SILP__
            if (v != null) {                                                                                                                                    //__SILP__
                return v.SetValue(propertyPass, val);                                                                                                           //__SILP__
            } else {                                                                                                                                            //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                              //__SILP__
            }                                                                                                                                                   //__SILP__
            return false;                                                                                                                                       //__SILP__
        }                                                                                                                                                       //__SILP__
                                                                                                                                                                //__SILP__
        public static bool SetFloat(this ITreeProperties<IProperty> properties, string path, float val) {                                                       //__SILP__
            FloatProperty v = properties.Get<FloatProperty>(path);                                                                                              //__SILP__
            if (v != null) {                                                                                                                                    //__SILP__
                return v.SetValue(val);                                                                                                                         //__SILP__
            } else {                                                                                                                                            //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                              //__SILP__
            }                                                                                                                                                   //__SILP__
            return false;                                                                                                                                       //__SILP__
        }                                                                                                                                                       //__SILP__
        //SILP: TREE_PROPERTIES_HELPER(Double, double)
        public static DoubleProperty AddDouble(this ITreeProperties<IProperty> properties, string path, Pass propertyPass, double val) {                          //__SILP__
            DoubleProperty v = properties.Add<DoubleProperty>(path, propertyPass);                                                                                //__SILP__
            if (v != null && !v.Setup(propertyPass, val)) {                                                                                                       //__SILP__
                properties.Remove<DoubleProperty>(path);                                                                                                          //__SILP__
                v = null;                                                                                                                                         //__SILP__
            }                                                                                                                                                     //__SILP__
            return v;                                                                                                                                             //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
        public static DoubleProperty AddDouble(this ITreeProperties<IProperty> properties, string path, double val) {                                             //__SILP__
            return properties.AddDouble(path, null, val);                                                                                                         //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
        public static DoubleProperty RemoveDouble(this ITreeProperties<IProperty> properties, string path, Pass propertyPass) {                                   //__SILP__
            return properties.Remove<DoubleProperty>(path, propertyPass);                                                                                         //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
        public static DoubleProperty RemoveDouble(this ITreeProperties<IProperty> properties, string path) {                                                      //__SILP__
            return properties.Remove<DoubleProperty>(path);                                                                                                       //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
        public static bool AddDoubleValueChecker(this ITreeProperties<IProperty> properties, string path, Pass propertyPass, IValueChecker<double> checker) {     //__SILP__
            DoubleProperty p = properties.Get<DoubleProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                      //__SILP__
                return p.AddValueChecker(propertyPass, checker);                                                                                                  //__SILP__
            } else {                                                                                                                                              //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                                //__SILP__
            }                                                                                                                                                     //__SILP__
            return false;                                                                                                                                         //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
        public static bool AddDoubleValueChecker(this ITreeProperties<IProperty> properties, string path, IValueChecker<double> checker) {                        //__SILP__
            return AddDoubleValueChecker(properties, path, null, checker);                                                                                        //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
        public static bool RemoveDoubleValueChecker(this ITreeProperties<IProperty> properties, string path, Pass propertyPass, IValueChecker<double> checker) {  //__SILP__
            DoubleProperty p = properties.Get<DoubleProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                      //__SILP__
                return p.RemoveValueChecker(propertyPass, checker);                                                                                               //__SILP__
            } else {                                                                                                                                              //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                                //__SILP__
            }                                                                                                                                                     //__SILP__
            return false;                                                                                                                                         //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
        public static bool RemoveDoubleValueChecker(this ITreeProperties<IProperty> properties, string path, IValueChecker<double> checker) {                     //__SILP__
            return RemoveDoubleValueChecker(properties, path, null, checker);                                                                                     //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
        public static BlockValueChecker<double> AddDoubleBlockValueChecker(this ITreeProperties<IProperty> properties, string path, Pass propertyPass,            //__SILP__
                                            IBlockOwner owner, Func<IVar<double>, double, bool> checker) {                                                        //__SILP__
            DoubleProperty p = properties.Get<DoubleProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                      //__SILP__
                return p.AddBlockValueChecker(propertyPass, owner, checker);                                                                                      //__SILP__
            } else {                                                                                                                                              //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                                //__SILP__
            }                                                                                                                                                     //__SILP__
            return null;                                                                                                                                          //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
        public static BlockValueChecker<double> AddDoubleBlockValueChecker(this ITreeProperties<IProperty> properties, string path,                               //__SILP__
                                            IBlockOwner owner, Func<IVar<double>, double, bool> checker) {                                                        //__SILP__
            return AddDoubleBlockValueChecker(properties, path, null, owner, checker);                                                                            //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
        public static bool AddDoubleValueWatcher(this ITreeProperties<IProperty> properties, string path, IValueWatcher<double> watcher) {                        //__SILP__
            DoubleProperty p = properties.Get<DoubleProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                      //__SILP__
                return p.AddValueWatcher(watcher);                                                                                                                //__SILP__
            } else {                                                                                                                                              //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                                //__SILP__
            }                                                                                                                                                     //__SILP__
            return false;                                                                                                                                         //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
        public static bool RemoveDoubleValueWatcher(this ITreeProperties<IProperty> properties, string path, IValueWatcher<double> watcher) {                     //__SILP__
            DoubleProperty p = properties.Get<DoubleProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                      //__SILP__
                return p.RemoveValueWatcher(watcher);                                                                                                             //__SILP__
            } else {                                                                                                                                              //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                                //__SILP__
            }                                                                                                                                                     //__SILP__
            return false;                                                                                                                                         //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
        public static BlockValueWatcher<double> AddDoubleBlockValueWatcher(this ITreeProperties<IProperty> properties, string path,                               //__SILP__
                                            IBlockOwner owner, Action<IVar<double>, double> watcher) {                                                            //__SILP__
            DoubleProperty p = properties.Get<DoubleProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                      //__SILP__
                return p.AddBlockValueWatcher(owner, watcher);                                                                                                    //__SILP__
            } else {                                                                                                                                              //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                                //__SILP__
            }                                                                                                                                                     //__SILP__
            return null;                                                                                                                                          //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
        public static bool IsDouble(this ITreeProperties<IProperty> properties, string path) {                                                                    //__SILP__
            return properties.Is<DoubleProperty>(path);                                                                                                           //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
        public static double GetDouble(this ITreeProperties<IProperty> properties, string path) {                                                                 //__SILP__
            DoubleProperty v = properties.Get<DoubleProperty>(path);                                                                                              //__SILP__
            if (v != null) {                                                                                                                                      //__SILP__
                return v.Value;                                                                                                                                   //__SILP__
            } else {                                                                                                                                              //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                                //__SILP__
            }                                                                                                                                                     //__SILP__
            return default(double);                                                                                                                               //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
        public static double GetDouble(this ITreeProperties<IProperty> properties, string path, double defaultValue) {                                            //__SILP__
            DoubleProperty v = properties.Get<DoubleProperty>(path);                                                                                              //__SILP__
            if (v != null) {                                                                                                                                      //__SILP__
                return v.Value;                                                                                                                                   //__SILP__
            } else {                                                                                                                                              //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                                //__SILP__
            }                                                                                                                                                     //__SILP__
            return defaultValue;                                                                                                                                  //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
                                                                                                                                                                  //__SILP__
        public static bool SetDouble(this ITreeProperties<IProperty> properties, string path, Pass propertyPass, double val) {                                    //__SILP__
            DoubleProperty v = properties.Get<DoubleProperty>(path);                                                                                              //__SILP__
            if (v != null) {                                                                                                                                      //__SILP__
                return v.SetValue(propertyPass, val);                                                                                                             //__SILP__
            } else {                                                                                                                                              //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                                //__SILP__
            }                                                                                                                                                     //__SILP__
            return false;                                                                                                                                         //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
        public static bool SetDouble(this ITreeProperties<IProperty> properties, string path, double val) {                                                       //__SILP__
            DoubleProperty v = properties.Get<DoubleProperty>(path);                                                                                              //__SILP__
            if (v != null) {                                                                                                                                      //__SILP__
                return v.SetValue(val);                                                                                                                           //__SILP__
            } else {                                                                                                                                              //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                                //__SILP__
            }                                                                                                                                                     //__SILP__
            return false;                                                                                                                                         //__SILP__
        }                                                                                                                                                         //__SILP__
        //SILP: TREE_PROPERTIES_HELPER(String, string)
        public static StringProperty AddString(this ITreeProperties<IProperty> properties, string path, Pass propertyPass, string val) {                          //__SILP__
            StringProperty v = properties.Add<StringProperty>(path, propertyPass);                                                                                //__SILP__
            if (v != null && !v.Setup(propertyPass, val)) {                                                                                                       //__SILP__
                properties.Remove<StringProperty>(path);                                                                                                          //__SILP__
                v = null;                                                                                                                                         //__SILP__
            }                                                                                                                                                     //__SILP__
            return v;                                                                                                                                             //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
        public static StringProperty AddString(this ITreeProperties<IProperty> properties, string path, string val) {                                             //__SILP__
            return properties.AddString(path, null, val);                                                                                                         //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
        public static StringProperty RemoveString(this ITreeProperties<IProperty> properties, string path, Pass propertyPass) {                                   //__SILP__
            return properties.Remove<StringProperty>(path, propertyPass);                                                                                         //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
        public static StringProperty RemoveString(this ITreeProperties<IProperty> properties, string path) {                                                      //__SILP__
            return properties.Remove<StringProperty>(path);                                                                                                       //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
        public static bool AddStringValueChecker(this ITreeProperties<IProperty> properties, string path, Pass propertyPass, IValueChecker<string> checker) {     //__SILP__
            StringProperty p = properties.Get<StringProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                      //__SILP__
                return p.AddValueChecker(propertyPass, checker);                                                                                                  //__SILP__
            } else {                                                                                                                                              //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                                //__SILP__
            }                                                                                                                                                     //__SILP__
            return false;                                                                                                                                         //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
        public static bool AddStringValueChecker(this ITreeProperties<IProperty> properties, string path, IValueChecker<string> checker) {                        //__SILP__
            return AddStringValueChecker(properties, path, null, checker);                                                                                        //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
        public static bool RemoveStringValueChecker(this ITreeProperties<IProperty> properties, string path, Pass propertyPass, IValueChecker<string> checker) {  //__SILP__
            StringProperty p = properties.Get<StringProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                      //__SILP__
                return p.RemoveValueChecker(propertyPass, checker);                                                                                               //__SILP__
            } else {                                                                                                                                              //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                                //__SILP__
            }                                                                                                                                                     //__SILP__
            return false;                                                                                                                                         //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
        public static bool RemoveStringValueChecker(this ITreeProperties<IProperty> properties, string path, IValueChecker<string> checker) {                     //__SILP__
            return RemoveStringValueChecker(properties, path, null, checker);                                                                                     //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
        public static BlockValueChecker<string> AddStringBlockValueChecker(this ITreeProperties<IProperty> properties, string path, Pass propertyPass,            //__SILP__
                                            IBlockOwner owner, Func<IVar<string>, string, bool> checker) {                                                        //__SILP__
            StringProperty p = properties.Get<StringProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                      //__SILP__
                return p.AddBlockValueChecker(propertyPass, owner, checker);                                                                                      //__SILP__
            } else {                                                                                                                                              //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                                //__SILP__
            }                                                                                                                                                     //__SILP__
            return null;                                                                                                                                          //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
        public static BlockValueChecker<string> AddStringBlockValueChecker(this ITreeProperties<IProperty> properties, string path,                               //__SILP__
                                            IBlockOwner owner, Func<IVar<string>, string, bool> checker) {                                                        //__SILP__
            return AddStringBlockValueChecker(properties, path, null, owner, checker);                                                                            //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
        public static bool AddStringValueWatcher(this ITreeProperties<IProperty> properties, string path, IValueWatcher<string> watcher) {                        //__SILP__
            StringProperty p = properties.Get<StringProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                      //__SILP__
                return p.AddValueWatcher(watcher);                                                                                                                //__SILP__
            } else {                                                                                                                                              //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                                //__SILP__
            }                                                                                                                                                     //__SILP__
            return false;                                                                                                                                         //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
        public static bool RemoveStringValueWatcher(this ITreeProperties<IProperty> properties, string path, IValueWatcher<string> watcher) {                     //__SILP__
            StringProperty p = properties.Get<StringProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                      //__SILP__
                return p.RemoveValueWatcher(watcher);                                                                                                             //__SILP__
            } else {                                                                                                                                              //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                                //__SILP__
            }                                                                                                                                                     //__SILP__
            return false;                                                                                                                                         //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
        public static BlockValueWatcher<string> AddStringBlockValueWatcher(this ITreeProperties<IProperty> properties, string path,                               //__SILP__
                                            IBlockOwner owner, Action<IVar<string>, string> watcher) {                                                            //__SILP__
            StringProperty p = properties.Get<StringProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                      //__SILP__
                return p.AddBlockValueWatcher(owner, watcher);                                                                                                    //__SILP__
            } else {                                                                                                                                              //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                                //__SILP__
            }                                                                                                                                                     //__SILP__
            return null;                                                                                                                                          //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
        public static bool IsString(this ITreeProperties<IProperty> properties, string path) {                                                                    //__SILP__
            return properties.Is<StringProperty>(path);                                                                                                           //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
        public static string GetString(this ITreeProperties<IProperty> properties, string path) {                                                                 //__SILP__
            StringProperty v = properties.Get<StringProperty>(path);                                                                                              //__SILP__
            if (v != null) {                                                                                                                                      //__SILP__
                return v.Value;                                                                                                                                   //__SILP__
            } else {                                                                                                                                              //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                                //__SILP__
            }                                                                                                                                                     //__SILP__
            return default(string);                                                                                                                               //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
        public static string GetString(this ITreeProperties<IProperty> properties, string path, string defaultValue) {                                            //__SILP__
            StringProperty v = properties.Get<StringProperty>(path);                                                                                              //__SILP__
            if (v != null) {                                                                                                                                      //__SILP__
                return v.Value;                                                                                                                                   //__SILP__
            } else {                                                                                                                                              //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                                //__SILP__
            }                                                                                                                                                     //__SILP__
            return defaultValue;                                                                                                                                  //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
                                                                                                                                                                  //__SILP__
        public static bool SetString(this ITreeProperties<IProperty> properties, string path, Pass propertyPass, string val) {                                    //__SILP__
            StringProperty v = properties.Get<StringProperty>(path);                                                                                              //__SILP__
            if (v != null) {                                                                                                                                      //__SILP__
                return v.SetValue(propertyPass, val);                                                                                                             //__SILP__
            } else {                                                                                                                                              //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                                //__SILP__
            }                                                                                                                                                     //__SILP__
            return false;                                                                                                                                         //__SILP__
        }                                                                                                                                                         //__SILP__
                                                                                                                                                                  //__SILP__
        public static bool SetString(this ITreeProperties<IProperty> properties, string path, string val) {                                                       //__SILP__
            StringProperty v = properties.Get<StringProperty>(path);                                                                                              //__SILP__
            if (v != null) {                                                                                                                                      //__SILP__
                return v.SetValue(val);                                                                                                                           //__SILP__
            } else {                                                                                                                                              //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                                //__SILP__
            }                                                                                                                                                     //__SILP__
            return false;                                                                                                                                         //__SILP__
        }                                                                                                                                                         //__SILP__
        //SILP: TREE_PROPERTIES_HELPER(Data, Data)
        public static DataProperty AddData(this ITreeProperties<IProperty> properties, string path, Pass propertyPass, Data val) {                            //__SILP__
            DataProperty v = properties.Add<DataProperty>(path, propertyPass);                                                                                //__SILP__
            if (v != null && !v.Setup(propertyPass, val)) {                                                                                                   //__SILP__
                properties.Remove<DataProperty>(path);                                                                                                        //__SILP__
                v = null;                                                                                                                                     //__SILP__
            }                                                                                                                                                 //__SILP__
            return v;                                                                                                                                         //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static DataProperty AddData(this ITreeProperties<IProperty> properties, string path, Data val) {                                               //__SILP__
            return properties.AddData(path, null, val);                                                                                                       //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static DataProperty RemoveData(this ITreeProperties<IProperty> properties, string path, Pass propertyPass) {                                   //__SILP__
            return properties.Remove<DataProperty>(path, propertyPass);                                                                                       //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static DataProperty RemoveData(this ITreeProperties<IProperty> properties, string path) {                                                      //__SILP__
            return properties.Remove<DataProperty>(path);                                                                                                     //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static bool AddDataValueChecker(this ITreeProperties<IProperty> properties, string path, Pass propertyPass, IValueChecker<Data> checker) {     //__SILP__
            DataProperty p = properties.Get<DataProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                  //__SILP__
                return p.AddValueChecker(propertyPass, checker);                                                                                              //__SILP__
            } else {                                                                                                                                          //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                            //__SILP__
            }                                                                                                                                                 //__SILP__
            return false;                                                                                                                                     //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static bool AddDataValueChecker(this ITreeProperties<IProperty> properties, string path, IValueChecker<Data> checker) {                        //__SILP__
            return AddDataValueChecker(properties, path, null, checker);                                                                                      //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static bool RemoveDataValueChecker(this ITreeProperties<IProperty> properties, string path, Pass propertyPass, IValueChecker<Data> checker) {  //__SILP__
            DataProperty p = properties.Get<DataProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                  //__SILP__
                return p.RemoveValueChecker(propertyPass, checker);                                                                                           //__SILP__
            } else {                                                                                                                                          //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                            //__SILP__
            }                                                                                                                                                 //__SILP__
            return false;                                                                                                                                     //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static bool RemoveDataValueChecker(this ITreeProperties<IProperty> properties, string path, IValueChecker<Data> checker) {                     //__SILP__
            return RemoveDataValueChecker(properties, path, null, checker);                                                                                   //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static BlockValueChecker<Data> AddDataBlockValueChecker(this ITreeProperties<IProperty> properties, string path, Pass propertyPass,            //__SILP__
                                            IBlockOwner owner, Func<IVar<Data>, Data, bool> checker) {                                                        //__SILP__
            DataProperty p = properties.Get<DataProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                  //__SILP__
                return p.AddBlockValueChecker(propertyPass, owner, checker);                                                                                  //__SILP__
            } else {                                                                                                                                          //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                            //__SILP__
            }                                                                                                                                                 //__SILP__
            return null;                                                                                                                                      //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static BlockValueChecker<Data> AddDataBlockValueChecker(this ITreeProperties<IProperty> properties, string path,                               //__SILP__
                                            IBlockOwner owner, Func<IVar<Data>, Data, bool> checker) {                                                        //__SILP__
            return AddDataBlockValueChecker(properties, path, null, owner, checker);                                                                          //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static bool AddDataValueWatcher(this ITreeProperties<IProperty> properties, string path, IValueWatcher<Data> watcher) {                        //__SILP__
            DataProperty p = properties.Get<DataProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                  //__SILP__
                return p.AddValueWatcher(watcher);                                                                                                            //__SILP__
            } else {                                                                                                                                          //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                            //__SILP__
            }                                                                                                                                                 //__SILP__
            return false;                                                                                                                                     //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static bool RemoveDataValueWatcher(this ITreeProperties<IProperty> properties, string path, IValueWatcher<Data> watcher) {                     //__SILP__
            DataProperty p = properties.Get<DataProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                  //__SILP__
                return p.RemoveValueWatcher(watcher);                                                                                                         //__SILP__
            } else {                                                                                                                                          //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                            //__SILP__
            }                                                                                                                                                 //__SILP__
            return false;                                                                                                                                     //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static BlockValueWatcher<Data> AddDataBlockValueWatcher(this ITreeProperties<IProperty> properties, string path,                               //__SILP__
                                            IBlockOwner owner, Action<IVar<Data>, Data> watcher) {                                                            //__SILP__
            DataProperty p = properties.Get<DataProperty>(path);                                                                                              //__SILP__
            if (p != null) {                                                                                                                                  //__SILP__
                return p.AddBlockValueWatcher(owner, watcher);                                                                                                //__SILP__
            } else {                                                                                                                                          //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                            //__SILP__
            }                                                                                                                                                 //__SILP__
            return null;                                                                                                                                      //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static bool IsData(this ITreeProperties<IProperty> properties, string path) {                                                                  //__SILP__
            return properties.Is<DataProperty>(path);                                                                                                         //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static Data GetData(this ITreeProperties<IProperty> properties, string path) {                                                                 //__SILP__
            DataProperty v = properties.Get<DataProperty>(path);                                                                                              //__SILP__
            if (v != null) {                                                                                                                                  //__SILP__
                return v.Value;                                                                                                                               //__SILP__
            } else {                                                                                                                                          //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                            //__SILP__
            }                                                                                                                                                 //__SILP__
            return default(Data);                                                                                                                             //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static Data GetData(this ITreeProperties<IProperty> properties, string path, Data defaultValue) {                                              //__SILP__
            DataProperty v = properties.Get<DataProperty>(path);                                                                                              //__SILP__
            if (v != null) {                                                                                                                                  //__SILP__
                return v.Value;                                                                                                                               //__SILP__
            } else {                                                                                                                                          //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                            //__SILP__
            }                                                                                                                                                 //__SILP__
            return defaultValue;                                                                                                                              //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
                                                                                                                                                              //__SILP__
        public static bool SetData(this ITreeProperties<IProperty> properties, string path, Pass propertyPass, Data val) {                                    //__SILP__
            DataProperty v = properties.Get<DataProperty>(path);                                                                                              //__SILP__
            if (v != null) {                                                                                                                                  //__SILP__
                return v.SetValue(propertyPass, val);                                                                                                         //__SILP__
            } else {                                                                                                                                          //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                            //__SILP__
            }                                                                                                                                                 //__SILP__
            return false;                                                                                                                                     //__SILP__
        }                                                                                                                                                     //__SILP__
                                                                                                                                                              //__SILP__
        public static bool SetData(this ITreeProperties<IProperty> properties, string path, Data val) {                                                       //__SILP__
            DataProperty v = properties.Get<DataProperty>(path);                                                                                              //__SILP__
            if (v != null) {                                                                                                                                  //__SILP__
                return v.SetValue(val);                                                                                                                       //__SILP__
            } else {                                                                                                                                          //__SILP__
                properties.Error("Property Not Exist: {0}", path);                                                                                            //__SILP__
            }                                                                                                                                                 //__SILP__
            return false;                                                                                                                                     //__SILP__
        }                                                                                                                                                     //__SILP__
    }
}
