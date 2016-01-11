using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class PropertiesConsts {
        public const string TypeProperties = "Properties";

        public const string TypeBoolProperty = "Bool";
        public const string TypeIntProperty = "Int";
        public const string TypeLongProperty = "Long";
        public const string TypeFloatProperty = "Float";
        public const string TypeDoubleProperty = "Double";
        public const string TypeStringProperty = "String";
        public const string TypeDataProperty = "Data";

        public const string KeyValue = "v";
    }

    public sealed class Properties : Section<IContext, IProperty> {
        public override string Type {
            get { return PropertiesConsts.TypeProperties; }
        }

        public Properties(IContext owner, string path, Pass pass) : base(owner, path, pass) {
        }

        public IProperty Add(string path, Pass pass, bool open, Data data) {
            if (data == null) return null;

            string type = data.GetString(ObjectConsts.KeyType);
            if (string.IsNullOrEmpty(type)) {
                Error("Invalid Property data: {0}, {1}", path, data);
                return null;
            }
            IProperty prop = New(type, path, open ? pass.Open : pass);
            if (prop == null) {
                Error("Failed to Add Property: {0}, {1}", path, data);
                return null;
            }
            if (!prop.Decode(pass, data)) {
                Error("Failed to Decode Property: {0}, {1} -> {2}", path, data, prop);
            }

            return prop;
        }

        //SILP: PROPERTIES_HELPER(Bool, bool)
        public BoolProperty AddBool(string path, Pass pass, bool val) {                                 //__SILP__
            BoolProperty v = Add<BoolProperty>(path, pass);                                             //__SILP__
            if (v != null && !v.Setup(pass, val)) {                                                     //__SILP__
                Remove<BoolProperty>(path);                                                             //__SILP__
                v = null;                                                                               //__SILP__
            }                                                                                           //__SILP__
            return v;                                                                                   //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public BoolProperty AddBool(string path, bool val) {                                            //__SILP__
            return AddBool(path, null, val);                                                            //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public BoolProperty RemoveBool(string path, Pass pass) {                                        //__SILP__
            return Remove<BoolProperty>(path, pass);                                                    //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public BoolProperty RemoveBool(string path) {                                                   //__SILP__
            return Remove<BoolProperty>(path);                                                          //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public bool AddBoolValueChecker(string path, Pass pass, IValueChecker<bool> checker) {          //__SILP__
            BoolProperty p = Get<BoolProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                            //__SILP__
                return p.AddValueChecker(pass, checker);                                                //__SILP__
            } else {                                                                                    //__SILP__
                Error("Property Not Exist: {0}", path);                                                 //__SILP__
            }                                                                                           //__SILP__
            return false;                                                                               //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public bool AddBoolValueChecker(string path, IValueChecker<bool> checker) {                     //__SILP__
            return AddBoolValueChecker(path, checker);                                                  //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public bool RemoveBoolValueChecker(string path, Pass pass, IValueChecker<bool> checker) {       //__SILP__
            BoolProperty p = Get<BoolProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                            //__SILP__
                return p.RemoveValueChecker(pass, checker);                                             //__SILP__
            } else {                                                                                    //__SILP__
                Error("Property Not Exist: {0}", path);                                                 //__SILP__
            }                                                                                           //__SILP__
            return false;                                                                               //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public bool RemoveBoolValueChecker(string path, IValueChecker<bool> checker) {                  //__SILP__
            return RemoveBoolValueChecker(path, checker);                                               //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public BlockValueChecker<bool> AddBoolBlockValueChecker(string path, Pass pass,                 //__SILP__
                                            IBlockOwner owner, Func<IVar<bool>, bool, bool> checker) {  //__SILP__
            BoolProperty p = Get<BoolProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                            //__SILP__
                return p.AddBlockValueChecker(pass, owner, checker);                                    //__SILP__
            } else {                                                                                    //__SILP__
                Error("Property Not Exist: {0}", path);                                                 //__SILP__
            }                                                                                           //__SILP__
            return null;                                                                                //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public BlockValueChecker<bool> AddBoolBlockValueChecker(string path,                            //__SILP__
                                            IBlockOwner owner, Func<IVar<bool>, bool, bool> checker) {  //__SILP__
            return AddBoolBlockValueChecker(path, owner, checker);                                      //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public bool AddBoolValueWatcher(string path, IValueWatcher<bool> watcher) {                     //__SILP__
            BoolProperty p = Get<BoolProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                            //__SILP__
                return p.AddValueWatcher(watcher);                                                      //__SILP__
            } else {                                                                                    //__SILP__
                Error("Property Not Exist: {0}", path);                                                 //__SILP__
            }                                                                                           //__SILP__
            return false;                                                                               //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public bool RemoveBoolValueWatcher(string path, IValueWatcher<bool> watcher) {                  //__SILP__
            BoolProperty p = Get<BoolProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                            //__SILP__
                return p.RemoveValueWatcher(watcher);                                                   //__SILP__
            } else {                                                                                    //__SILP__
                Error("Property Not Exist: {0}", path);                                                 //__SILP__
            }                                                                                           //__SILP__
            return false;                                                                               //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public BlockValueWatcher<bool> AddBoolBlockValueWatcher(string path,                            //__SILP__
                                            IBlockOwner owner, Action<IVar<bool>, bool> watcher) {      //__SILP__
            BoolProperty p = Get<BoolProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                            //__SILP__
                return p.AddBlockValueWatcher(owner, watcher);                                          //__SILP__
            } else {                                                                                    //__SILP__
                Error("Property Not Exist: {0}", path);                                                 //__SILP__
            }                                                                                           //__SILP__
            return null;                                                                                //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public bool IsBool(string path) {                                                               //__SILP__
            return Get<BoolProperty>(path) != null;                                                     //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public bool GetBool(string path) {                                                              //__SILP__
            BoolProperty v = Get<BoolProperty>(path);                                                   //__SILP__
            if (v != null) {                                                                            //__SILP__
                return v.Value;                                                                         //__SILP__
            } else {                                                                                    //__SILP__
                Error("Property Not Exist: {0}", path);                                                 //__SILP__
            }                                                                                           //__SILP__
            return default(bool);                                                                       //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public bool GetBool(string path, bool defaultValue) {                                           //__SILP__
            BoolProperty v = Get<BoolProperty>(path);                                                   //__SILP__
            if (v != null) {                                                                            //__SILP__
                return v.Value;                                                                         //__SILP__
            } else {                                                                                    //__SILP__
                Error("Property Not Exist: {0}", path);                                                 //__SILP__
            }                                                                                           //__SILP__
            return defaultValue;                                                                        //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
                                                                                                        //__SILP__
        public bool SetBool(string path, bool val) {                                                    //__SILP__
            BoolProperty v = Get<BoolProperty>(path);                                                   //__SILP__
            if (v != null) {                                                                            //__SILP__
                return v.SetValue(val);                                                                 //__SILP__
            } else {                                                                                    //__SILP__
                Error("Property Not Exist: {0}", path);                                                 //__SILP__
            }                                                                                           //__SILP__
            return false;                                                                               //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public bool SetBool(string path, Pass pass, bool val) {                                         //__SILP__
            BoolProperty v = Get<BoolProperty>(path);                                                   //__SILP__
            if (v != null) {                                                                            //__SILP__
                return v.SetValue(pass, val);                                                           //__SILP__
            } else {                                                                                    //__SILP__
                Error("Property Not Exist: {0}", path);                                                 //__SILP__
            }                                                                                           //__SILP__
            return false;                                                                               //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        //SILP: PROPERTIES_HELPER(Int, int)
        public IntProperty AddInt(string path, Pass pass, int val) {                                  //__SILP__
            IntProperty v = Add<IntProperty>(path, pass);                                             //__SILP__
            if (v != null && !v.Setup(pass, val)) {                                                   //__SILP__
                Remove<IntProperty>(path);                                                            //__SILP__
                v = null;                                                                             //__SILP__
            }                                                                                         //__SILP__
            return v;                                                                                 //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public IntProperty AddInt(string path, int val) {                                             //__SILP__
            return AddInt(path, null, val);                                                           //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public IntProperty RemoveInt(string path, Pass pass) {                                        //__SILP__
            return Remove<IntProperty>(path, pass);                                                   //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public IntProperty RemoveInt(string path) {                                                   //__SILP__
            return Remove<IntProperty>(path);                                                         //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public bool AddIntValueChecker(string path, Pass pass, IValueChecker<int> checker) {          //__SILP__
            IntProperty p = Get<IntProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                          //__SILP__
                return p.AddValueChecker(pass, checker);                                              //__SILP__
            } else {                                                                                  //__SILP__
                Error("Property Not Exist: {0}", path);                                               //__SILP__
            }                                                                                         //__SILP__
            return false;                                                                             //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public bool AddIntValueChecker(string path, IValueChecker<int> checker) {                     //__SILP__
            return AddIntValueChecker(path, checker);                                                 //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public bool RemoveIntValueChecker(string path, Pass pass, IValueChecker<int> checker) {       //__SILP__
            IntProperty p = Get<IntProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                          //__SILP__
                return p.RemoveValueChecker(pass, checker);                                           //__SILP__
            } else {                                                                                  //__SILP__
                Error("Property Not Exist: {0}", path);                                               //__SILP__
            }                                                                                         //__SILP__
            return false;                                                                             //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public bool RemoveIntValueChecker(string path, IValueChecker<int> checker) {                  //__SILP__
            return RemoveIntValueChecker(path, checker);                                              //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public BlockValueChecker<int> AddIntBlockValueChecker(string path, Pass pass,                 //__SILP__
                                            IBlockOwner owner, Func<IVar<int>, int, bool> checker) {  //__SILP__
            IntProperty p = Get<IntProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                          //__SILP__
                return p.AddBlockValueChecker(pass, owner, checker);                                  //__SILP__
            } else {                                                                                  //__SILP__
                Error("Property Not Exist: {0}", path);                                               //__SILP__
            }                                                                                         //__SILP__
            return null;                                                                              //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public BlockValueChecker<int> AddIntBlockValueChecker(string path,                            //__SILP__
                                            IBlockOwner owner, Func<IVar<int>, int, bool> checker) {  //__SILP__
            return AddIntBlockValueChecker(path, owner, checker);                                     //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public bool AddIntValueWatcher(string path, IValueWatcher<int> watcher) {                     //__SILP__
            IntProperty p = Get<IntProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                          //__SILP__
                return p.AddValueWatcher(watcher);                                                    //__SILP__
            } else {                                                                                  //__SILP__
                Error("Property Not Exist: {0}", path);                                               //__SILP__
            }                                                                                         //__SILP__
            return false;                                                                             //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public bool RemoveIntValueWatcher(string path, IValueWatcher<int> watcher) {                  //__SILP__
            IntProperty p = Get<IntProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                          //__SILP__
                return p.RemoveValueWatcher(watcher);                                                 //__SILP__
            } else {                                                                                  //__SILP__
                Error("Property Not Exist: {0}", path);                                               //__SILP__
            }                                                                                         //__SILP__
            return false;                                                                             //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public BlockValueWatcher<int> AddIntBlockValueWatcher(string path,                            //__SILP__
                                            IBlockOwner owner, Action<IVar<int>, int> watcher) {      //__SILP__
            IntProperty p = Get<IntProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                          //__SILP__
                return p.AddBlockValueWatcher(owner, watcher);                                        //__SILP__
            } else {                                                                                  //__SILP__
                Error("Property Not Exist: {0}", path);                                               //__SILP__
            }                                                                                         //__SILP__
            return null;                                                                              //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public bool IsInt(string path) {                                                              //__SILP__
            return Get<IntProperty>(path) != null;                                                    //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public int GetInt(string path) {                                                              //__SILP__
            IntProperty v = Get<IntProperty>(path);                                                   //__SILP__
            if (v != null) {                                                                          //__SILP__
                return v.Value;                                                                       //__SILP__
            } else {                                                                                  //__SILP__
                Error("Property Not Exist: {0}", path);                                               //__SILP__
            }                                                                                         //__SILP__
            return default(int);                                                                      //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public int GetInt(string path, int defaultValue) {                                            //__SILP__
            IntProperty v = Get<IntProperty>(path);                                                   //__SILP__
            if (v != null) {                                                                          //__SILP__
                return v.Value;                                                                       //__SILP__
            } else {                                                                                  //__SILP__
                Error("Property Not Exist: {0}", path);                                               //__SILP__
            }                                                                                         //__SILP__
            return defaultValue;                                                                      //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
                                                                                                      //__SILP__
        public bool SetInt(string path, int val) {                                                    //__SILP__
            IntProperty v = Get<IntProperty>(path);                                                   //__SILP__
            if (v != null) {                                                                          //__SILP__
                return v.SetValue(val);                                                               //__SILP__
            } else {                                                                                  //__SILP__
                Error("Property Not Exist: {0}", path);                                               //__SILP__
            }                                                                                         //__SILP__
            return false;                                                                             //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public bool SetInt(string path, Pass pass, int val) {                                         //__SILP__
            IntProperty v = Get<IntProperty>(path);                                                   //__SILP__
            if (v != null) {                                                                          //__SILP__
                return v.SetValue(pass, val);                                                         //__SILP__
            } else {                                                                                  //__SILP__
                Error("Property Not Exist: {0}", path);                                               //__SILP__
            }                                                                                         //__SILP__
            return false;                                                                             //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        //SILP: PROPERTIES_HELPER(Long, long)
        public LongProperty AddLong(string path, Pass pass, long val) {                                 //__SILP__
            LongProperty v = Add<LongProperty>(path, pass);                                             //__SILP__
            if (v != null && !v.Setup(pass, val)) {                                                     //__SILP__
                Remove<LongProperty>(path);                                                             //__SILP__
                v = null;                                                                               //__SILP__
            }                                                                                           //__SILP__
            return v;                                                                                   //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public LongProperty AddLong(string path, long val) {                                            //__SILP__
            return AddLong(path, null, val);                                                            //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public LongProperty RemoveLong(string path, Pass pass) {                                        //__SILP__
            return Remove<LongProperty>(path, pass);                                                    //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public LongProperty RemoveLong(string path) {                                                   //__SILP__
            return Remove<LongProperty>(path);                                                          //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public bool AddLongValueChecker(string path, Pass pass, IValueChecker<long> checker) {          //__SILP__
            LongProperty p = Get<LongProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                            //__SILP__
                return p.AddValueChecker(pass, checker);                                                //__SILP__
            } else {                                                                                    //__SILP__
                Error("Property Not Exist: {0}", path);                                                 //__SILP__
            }                                                                                           //__SILP__
            return false;                                                                               //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public bool AddLongValueChecker(string path, IValueChecker<long> checker) {                     //__SILP__
            return AddLongValueChecker(path, checker);                                                  //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public bool RemoveLongValueChecker(string path, Pass pass, IValueChecker<long> checker) {       //__SILP__
            LongProperty p = Get<LongProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                            //__SILP__
                return p.RemoveValueChecker(pass, checker);                                             //__SILP__
            } else {                                                                                    //__SILP__
                Error("Property Not Exist: {0}", path);                                                 //__SILP__
            }                                                                                           //__SILP__
            return false;                                                                               //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public bool RemoveLongValueChecker(string path, IValueChecker<long> checker) {                  //__SILP__
            return RemoveLongValueChecker(path, checker);                                               //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public BlockValueChecker<long> AddLongBlockValueChecker(string path, Pass pass,                 //__SILP__
                                            IBlockOwner owner, Func<IVar<long>, long, bool> checker) {  //__SILP__
            LongProperty p = Get<LongProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                            //__SILP__
                return p.AddBlockValueChecker(pass, owner, checker);                                    //__SILP__
            } else {                                                                                    //__SILP__
                Error("Property Not Exist: {0}", path);                                                 //__SILP__
            }                                                                                           //__SILP__
            return null;                                                                                //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public BlockValueChecker<long> AddLongBlockValueChecker(string path,                            //__SILP__
                                            IBlockOwner owner, Func<IVar<long>, long, bool> checker) {  //__SILP__
            return AddLongBlockValueChecker(path, owner, checker);                                      //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public bool AddLongValueWatcher(string path, IValueWatcher<long> watcher) {                     //__SILP__
            LongProperty p = Get<LongProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                            //__SILP__
                return p.AddValueWatcher(watcher);                                                      //__SILP__
            } else {                                                                                    //__SILP__
                Error("Property Not Exist: {0}", path);                                                 //__SILP__
            }                                                                                           //__SILP__
            return false;                                                                               //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public bool RemoveLongValueWatcher(string path, IValueWatcher<long> watcher) {                  //__SILP__
            LongProperty p = Get<LongProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                            //__SILP__
                return p.RemoveValueWatcher(watcher);                                                   //__SILP__
            } else {                                                                                    //__SILP__
                Error("Property Not Exist: {0}", path);                                                 //__SILP__
            }                                                                                           //__SILP__
            return false;                                                                               //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public BlockValueWatcher<long> AddLongBlockValueWatcher(string path,                            //__SILP__
                                            IBlockOwner owner, Action<IVar<long>, long> watcher) {      //__SILP__
            LongProperty p = Get<LongProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                            //__SILP__
                return p.AddBlockValueWatcher(owner, watcher);                                          //__SILP__
            } else {                                                                                    //__SILP__
                Error("Property Not Exist: {0}", path);                                                 //__SILP__
            }                                                                                           //__SILP__
            return null;                                                                                //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public bool IsLong(string path) {                                                               //__SILP__
            return Get<LongProperty>(path) != null;                                                     //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public long GetLong(string path) {                                                              //__SILP__
            LongProperty v = Get<LongProperty>(path);                                                   //__SILP__
            if (v != null) {                                                                            //__SILP__
                return v.Value;                                                                         //__SILP__
            } else {                                                                                    //__SILP__
                Error("Property Not Exist: {0}", path);                                                 //__SILP__
            }                                                                                           //__SILP__
            return default(long);                                                                       //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public long GetLong(string path, long defaultValue) {                                           //__SILP__
            LongProperty v = Get<LongProperty>(path);                                                   //__SILP__
            if (v != null) {                                                                            //__SILP__
                return v.Value;                                                                         //__SILP__
            } else {                                                                                    //__SILP__
                Error("Property Not Exist: {0}", path);                                                 //__SILP__
            }                                                                                           //__SILP__
            return defaultValue;                                                                        //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
                                                                                                        //__SILP__
        public bool SetLong(string path, long val) {                                                    //__SILP__
            LongProperty v = Get<LongProperty>(path);                                                   //__SILP__
            if (v != null) {                                                                            //__SILP__
                return v.SetValue(val);                                                                 //__SILP__
            } else {                                                                                    //__SILP__
                Error("Property Not Exist: {0}", path);                                                 //__SILP__
            }                                                                                           //__SILP__
            return false;                                                                               //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public bool SetLong(string path, Pass pass, long val) {                                         //__SILP__
            LongProperty v = Get<LongProperty>(path);                                                   //__SILP__
            if (v != null) {                                                                            //__SILP__
                return v.SetValue(pass, val);                                                           //__SILP__
            } else {                                                                                    //__SILP__
                Error("Property Not Exist: {0}", path);                                                 //__SILP__
            }                                                                                           //__SILP__
            return false;                                                                               //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        //SILP: PROPERTIES_HELPER(Float, float)
        public FloatProperty AddFloat(string path, Pass pass, float val) {                                //__SILP__
            FloatProperty v = Add<FloatProperty>(path, pass);                                             //__SILP__
            if (v != null && !v.Setup(pass, val)) {                                                       //__SILP__
                Remove<FloatProperty>(path);                                                              //__SILP__
                v = null;                                                                                 //__SILP__
            }                                                                                             //__SILP__
            return v;                                                                                     //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public FloatProperty AddFloat(string path, float val) {                                           //__SILP__
            return AddFloat(path, null, val);                                                             //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public FloatProperty RemoveFloat(string path, Pass pass) {                                        //__SILP__
            return Remove<FloatProperty>(path, pass);                                                     //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public FloatProperty RemoveFloat(string path) {                                                   //__SILP__
            return Remove<FloatProperty>(path);                                                           //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public bool AddFloatValueChecker(string path, Pass pass, IValueChecker<float> checker) {          //__SILP__
            FloatProperty p = Get<FloatProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                              //__SILP__
                return p.AddValueChecker(pass, checker);                                                  //__SILP__
            } else {                                                                                      //__SILP__
                Error("Property Not Exist: {0}", path);                                                   //__SILP__
            }                                                                                             //__SILP__
            return false;                                                                                 //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public bool AddFloatValueChecker(string path, IValueChecker<float> checker) {                     //__SILP__
            return AddFloatValueChecker(path, checker);                                                   //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public bool RemoveFloatValueChecker(string path, Pass pass, IValueChecker<float> checker) {       //__SILP__
            FloatProperty p = Get<FloatProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                              //__SILP__
                return p.RemoveValueChecker(pass, checker);                                               //__SILP__
            } else {                                                                                      //__SILP__
                Error("Property Not Exist: {0}", path);                                                   //__SILP__
            }                                                                                             //__SILP__
            return false;                                                                                 //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public bool RemoveFloatValueChecker(string path, IValueChecker<float> checker) {                  //__SILP__
            return RemoveFloatValueChecker(path, checker);                                                //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public BlockValueChecker<float> AddFloatBlockValueChecker(string path, Pass pass,                 //__SILP__
                                            IBlockOwner owner, Func<IVar<float>, float, bool> checker) {  //__SILP__
            FloatProperty p = Get<FloatProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                              //__SILP__
                return p.AddBlockValueChecker(pass, owner, checker);                                      //__SILP__
            } else {                                                                                      //__SILP__
                Error("Property Not Exist: {0}", path);                                                   //__SILP__
            }                                                                                             //__SILP__
            return null;                                                                                  //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public BlockValueChecker<float> AddFloatBlockValueChecker(string path,                            //__SILP__
                                            IBlockOwner owner, Func<IVar<float>, float, bool> checker) {  //__SILP__
            return AddFloatBlockValueChecker(path, owner, checker);                                       //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public bool AddFloatValueWatcher(string path, IValueWatcher<float> watcher) {                     //__SILP__
            FloatProperty p = Get<FloatProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                              //__SILP__
                return p.AddValueWatcher(watcher);                                                        //__SILP__
            } else {                                                                                      //__SILP__
                Error("Property Not Exist: {0}", path);                                                   //__SILP__
            }                                                                                             //__SILP__
            return false;                                                                                 //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public bool RemoveFloatValueWatcher(string path, IValueWatcher<float> watcher) {                  //__SILP__
            FloatProperty p = Get<FloatProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                              //__SILP__
                return p.RemoveValueWatcher(watcher);                                                     //__SILP__
            } else {                                                                                      //__SILP__
                Error("Property Not Exist: {0}", path);                                                   //__SILP__
            }                                                                                             //__SILP__
            return false;                                                                                 //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public BlockValueWatcher<float> AddFloatBlockValueWatcher(string path,                            //__SILP__
                                            IBlockOwner owner, Action<IVar<float>, float> watcher) {      //__SILP__
            FloatProperty p = Get<FloatProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                              //__SILP__
                return p.AddBlockValueWatcher(owner, watcher);                                            //__SILP__
            } else {                                                                                      //__SILP__
                Error("Property Not Exist: {0}", path);                                                   //__SILP__
            }                                                                                             //__SILP__
            return null;                                                                                  //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public bool IsFloat(string path) {                                                                //__SILP__
            return Get<FloatProperty>(path) != null;                                                      //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public float GetFloat(string path) {                                                              //__SILP__
            FloatProperty v = Get<FloatProperty>(path);                                                   //__SILP__
            if (v != null) {                                                                              //__SILP__
                return v.Value;                                                                           //__SILP__
            } else {                                                                                      //__SILP__
                Error("Property Not Exist: {0}", path);                                                   //__SILP__
            }                                                                                             //__SILP__
            return default(float);                                                                        //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public float GetFloat(string path, float defaultValue) {                                          //__SILP__
            FloatProperty v = Get<FloatProperty>(path);                                                   //__SILP__
            if (v != null) {                                                                              //__SILP__
                return v.Value;                                                                           //__SILP__
            } else {                                                                                      //__SILP__
                Error("Property Not Exist: {0}", path);                                                   //__SILP__
            }                                                                                             //__SILP__
            return defaultValue;                                                                          //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
                                                                                                          //__SILP__
        public bool SetFloat(string path, float val) {                                                    //__SILP__
            FloatProperty v = Get<FloatProperty>(path);                                                   //__SILP__
            if (v != null) {                                                                              //__SILP__
                return v.SetValue(val);                                                                   //__SILP__
            } else {                                                                                      //__SILP__
                Error("Property Not Exist: {0}", path);                                                   //__SILP__
            }                                                                                             //__SILP__
            return false;                                                                                 //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public bool SetFloat(string path, Pass pass, float val) {                                         //__SILP__
            FloatProperty v = Get<FloatProperty>(path);                                                   //__SILP__
            if (v != null) {                                                                              //__SILP__
                return v.SetValue(pass, val);                                                             //__SILP__
            } else {                                                                                      //__SILP__
                Error("Property Not Exist: {0}", path);                                                   //__SILP__
            }                                                                                             //__SILP__
            return false;                                                                                 //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        //SILP: PROPERTIES_HELPER(Double, double)
        public DoubleProperty AddDouble(string path, Pass pass, double val) {                               //__SILP__
            DoubleProperty v = Add<DoubleProperty>(path, pass);                                             //__SILP__
            if (v != null && !v.Setup(pass, val)) {                                                         //__SILP__
                Remove<DoubleProperty>(path);                                                               //__SILP__
                v = null;                                                                                   //__SILP__
            }                                                                                               //__SILP__
            return v;                                                                                       //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public DoubleProperty AddDouble(string path, double val) {                                          //__SILP__
            return AddDouble(path, null, val);                                                              //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public DoubleProperty RemoveDouble(string path, Pass pass) {                                        //__SILP__
            return Remove<DoubleProperty>(path, pass);                                                      //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public DoubleProperty RemoveDouble(string path) {                                                   //__SILP__
            return Remove<DoubleProperty>(path);                                                            //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public bool AddDoubleValueChecker(string path, Pass pass, IValueChecker<double> checker) {          //__SILP__
            DoubleProperty p = Get<DoubleProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                                //__SILP__
                return p.AddValueChecker(pass, checker);                                                    //__SILP__
            } else {                                                                                        //__SILP__
                Error("Property Not Exist: {0}", path);                                                     //__SILP__
            }                                                                                               //__SILP__
            return false;                                                                                   //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public bool AddDoubleValueChecker(string path, IValueChecker<double> checker) {                     //__SILP__
            return AddDoubleValueChecker(path, checker);                                                    //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public bool RemoveDoubleValueChecker(string path, Pass pass, IValueChecker<double> checker) {       //__SILP__
            DoubleProperty p = Get<DoubleProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                                //__SILP__
                return p.RemoveValueChecker(pass, checker);                                                 //__SILP__
            } else {                                                                                        //__SILP__
                Error("Property Not Exist: {0}", path);                                                     //__SILP__
            }                                                                                               //__SILP__
            return false;                                                                                   //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public bool RemoveDoubleValueChecker(string path, IValueChecker<double> checker) {                  //__SILP__
            return RemoveDoubleValueChecker(path, checker);                                                 //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public BlockValueChecker<double> AddDoubleBlockValueChecker(string path, Pass pass,                 //__SILP__
                                            IBlockOwner owner, Func<IVar<double>, double, bool> checker) {  //__SILP__
            DoubleProperty p = Get<DoubleProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                                //__SILP__
                return p.AddBlockValueChecker(pass, owner, checker);                                        //__SILP__
            } else {                                                                                        //__SILP__
                Error("Property Not Exist: {0}", path);                                                     //__SILP__
            }                                                                                               //__SILP__
            return null;                                                                                    //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public BlockValueChecker<double> AddDoubleBlockValueChecker(string path,                            //__SILP__
                                            IBlockOwner owner, Func<IVar<double>, double, bool> checker) {  //__SILP__
            return AddDoubleBlockValueChecker(path, owner, checker);                                        //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public bool AddDoubleValueWatcher(string path, IValueWatcher<double> watcher) {                     //__SILP__
            DoubleProperty p = Get<DoubleProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                                //__SILP__
                return p.AddValueWatcher(watcher);                                                          //__SILP__
            } else {                                                                                        //__SILP__
                Error("Property Not Exist: {0}", path);                                                     //__SILP__
            }                                                                                               //__SILP__
            return false;                                                                                   //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public bool RemoveDoubleValueWatcher(string path, IValueWatcher<double> watcher) {                  //__SILP__
            DoubleProperty p = Get<DoubleProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                                //__SILP__
                return p.RemoveValueWatcher(watcher);                                                       //__SILP__
            } else {                                                                                        //__SILP__
                Error("Property Not Exist: {0}", path);                                                     //__SILP__
            }                                                                                               //__SILP__
            return false;                                                                                   //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public BlockValueWatcher<double> AddDoubleBlockValueWatcher(string path,                            //__SILP__
                                            IBlockOwner owner, Action<IVar<double>, double> watcher) {      //__SILP__
            DoubleProperty p = Get<DoubleProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                                //__SILP__
                return p.AddBlockValueWatcher(owner, watcher);                                              //__SILP__
            } else {                                                                                        //__SILP__
                Error("Property Not Exist: {0}", path);                                                     //__SILP__
            }                                                                                               //__SILP__
            return null;                                                                                    //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public bool IsDouble(string path) {                                                                 //__SILP__
            return Get<DoubleProperty>(path) != null;                                                       //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public double GetDouble(string path) {                                                              //__SILP__
            DoubleProperty v = Get<DoubleProperty>(path);                                                   //__SILP__
            if (v != null) {                                                                                //__SILP__
                return v.Value;                                                                             //__SILP__
            } else {                                                                                        //__SILP__
                Error("Property Not Exist: {0}", path);                                                     //__SILP__
            }                                                                                               //__SILP__
            return default(double);                                                                         //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public double GetDouble(string path, double defaultValue) {                                         //__SILP__
            DoubleProperty v = Get<DoubleProperty>(path);                                                   //__SILP__
            if (v != null) {                                                                                //__SILP__
                return v.Value;                                                                             //__SILP__
            } else {                                                                                        //__SILP__
                Error("Property Not Exist: {0}", path);                                                     //__SILP__
            }                                                                                               //__SILP__
            return defaultValue;                                                                            //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
                                                                                                            //__SILP__
        public bool SetDouble(string path, double val) {                                                    //__SILP__
            DoubleProperty v = Get<DoubleProperty>(path);                                                   //__SILP__
            if (v != null) {                                                                                //__SILP__
                return v.SetValue(val);                                                                     //__SILP__
            } else {                                                                                        //__SILP__
                Error("Property Not Exist: {0}", path);                                                     //__SILP__
            }                                                                                               //__SILP__
            return false;                                                                                   //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public bool SetDouble(string path, Pass pass, double val) {                                         //__SILP__
            DoubleProperty v = Get<DoubleProperty>(path);                                                   //__SILP__
            if (v != null) {                                                                                //__SILP__
                return v.SetValue(pass, val);                                                               //__SILP__
            } else {                                                                                        //__SILP__
                Error("Property Not Exist: {0}", path);                                                     //__SILP__
            }                                                                                               //__SILP__
            return false;                                                                                   //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        //SILP: PROPERTIES_HELPER(String, string)
        public StringProperty AddString(string path, Pass pass, string val) {                               //__SILP__
            StringProperty v = Add<StringProperty>(path, pass);                                             //__SILP__
            if (v != null && !v.Setup(pass, val)) {                                                         //__SILP__
                Remove<StringProperty>(path);                                                               //__SILP__
                v = null;                                                                                   //__SILP__
            }                                                                                               //__SILP__
            return v;                                                                                       //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public StringProperty AddString(string path, string val) {                                          //__SILP__
            return AddString(path, null, val);                                                              //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public StringProperty RemoveString(string path, Pass pass) {                                        //__SILP__
            return Remove<StringProperty>(path, pass);                                                      //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public StringProperty RemoveString(string path) {                                                   //__SILP__
            return Remove<StringProperty>(path);                                                            //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public bool AddStringValueChecker(string path, Pass pass, IValueChecker<string> checker) {          //__SILP__
            StringProperty p = Get<StringProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                                //__SILP__
                return p.AddValueChecker(pass, checker);                                                    //__SILP__
            } else {                                                                                        //__SILP__
                Error("Property Not Exist: {0}", path);                                                     //__SILP__
            }                                                                                               //__SILP__
            return false;                                                                                   //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public bool AddStringValueChecker(string path, IValueChecker<string> checker) {                     //__SILP__
            return AddStringValueChecker(path, checker);                                                    //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public bool RemoveStringValueChecker(string path, Pass pass, IValueChecker<string> checker) {       //__SILP__
            StringProperty p = Get<StringProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                                //__SILP__
                return p.RemoveValueChecker(pass, checker);                                                 //__SILP__
            } else {                                                                                        //__SILP__
                Error("Property Not Exist: {0}", path);                                                     //__SILP__
            }                                                                                               //__SILP__
            return false;                                                                                   //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public bool RemoveStringValueChecker(string path, IValueChecker<string> checker) {                  //__SILP__
            return RemoveStringValueChecker(path, checker);                                                 //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public BlockValueChecker<string> AddStringBlockValueChecker(string path, Pass pass,                 //__SILP__
                                            IBlockOwner owner, Func<IVar<string>, string, bool> checker) {  //__SILP__
            StringProperty p = Get<StringProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                                //__SILP__
                return p.AddBlockValueChecker(pass, owner, checker);                                        //__SILP__
            } else {                                                                                        //__SILP__
                Error("Property Not Exist: {0}", path);                                                     //__SILP__
            }                                                                                               //__SILP__
            return null;                                                                                    //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public BlockValueChecker<string> AddStringBlockValueChecker(string path,                            //__SILP__
                                            IBlockOwner owner, Func<IVar<string>, string, bool> checker) {  //__SILP__
            return AddStringBlockValueChecker(path, owner, checker);                                        //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public bool AddStringValueWatcher(string path, IValueWatcher<string> watcher) {                     //__SILP__
            StringProperty p = Get<StringProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                                //__SILP__
                return p.AddValueWatcher(watcher);                                                          //__SILP__
            } else {                                                                                        //__SILP__
                Error("Property Not Exist: {0}", path);                                                     //__SILP__
            }                                                                                               //__SILP__
            return false;                                                                                   //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public bool RemoveStringValueWatcher(string path, IValueWatcher<string> watcher) {                  //__SILP__
            StringProperty p = Get<StringProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                                //__SILP__
                return p.RemoveValueWatcher(watcher);                                                       //__SILP__
            } else {                                                                                        //__SILP__
                Error("Property Not Exist: {0}", path);                                                     //__SILP__
            }                                                                                               //__SILP__
            return false;                                                                                   //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public BlockValueWatcher<string> AddStringBlockValueWatcher(string path,                            //__SILP__
                                            IBlockOwner owner, Action<IVar<string>, string> watcher) {      //__SILP__
            StringProperty p = Get<StringProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                                //__SILP__
                return p.AddBlockValueWatcher(owner, watcher);                                              //__SILP__
            } else {                                                                                        //__SILP__
                Error("Property Not Exist: {0}", path);                                                     //__SILP__
            }                                                                                               //__SILP__
            return null;                                                                                    //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public bool IsString(string path) {                                                                 //__SILP__
            return Get<StringProperty>(path) != null;                                                       //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public string GetString(string path) {                                                              //__SILP__
            StringProperty v = Get<StringProperty>(path);                                                   //__SILP__
            if (v != null) {                                                                                //__SILP__
                return v.Value;                                                                             //__SILP__
            } else {                                                                                        //__SILP__
                Error("Property Not Exist: {0}", path);                                                     //__SILP__
            }                                                                                               //__SILP__
            return default(string);                                                                         //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public string GetString(string path, string defaultValue) {                                         //__SILP__
            StringProperty v = Get<StringProperty>(path);                                                   //__SILP__
            if (v != null) {                                                                                //__SILP__
                return v.Value;                                                                             //__SILP__
            } else {                                                                                        //__SILP__
                Error("Property Not Exist: {0}", path);                                                     //__SILP__
            }                                                                                               //__SILP__
            return defaultValue;                                                                            //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
                                                                                                            //__SILP__
        public bool SetString(string path, string val) {                                                    //__SILP__
            StringProperty v = Get<StringProperty>(path);                                                   //__SILP__
            if (v != null) {                                                                                //__SILP__
                return v.SetValue(val);                                                                     //__SILP__
            } else {                                                                                        //__SILP__
                Error("Property Not Exist: {0}", path);                                                     //__SILP__
            }                                                                                               //__SILP__
            return false;                                                                                   //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public bool SetString(string path, Pass pass, string val) {                                         //__SILP__
            StringProperty v = Get<StringProperty>(path);                                                   //__SILP__
            if (v != null) {                                                                                //__SILP__
                return v.SetValue(pass, val);                                                               //__SILP__
            } else {                                                                                        //__SILP__
                Error("Property Not Exist: {0}", path);                                                     //__SILP__
            }                                                                                               //__SILP__
            return false;                                                                                   //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        //SILP: PROPERTIES_HELPER(Data, Data)
        public DataProperty AddData(string path, Pass pass, Data val) {                                 //__SILP__
            DataProperty v = Add<DataProperty>(path, pass);                                             //__SILP__
            if (v != null && !v.Setup(pass, val)) {                                                     //__SILP__
                Remove<DataProperty>(path);                                                             //__SILP__
                v = null;                                                                               //__SILP__
            }                                                                                           //__SILP__
            return v;                                                                                   //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public DataProperty AddData(string path, Data val) {                                            //__SILP__
            return AddData(path, null, val);                                                            //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public DataProperty RemoveData(string path, Pass pass) {                                        //__SILP__
            return Remove<DataProperty>(path, pass);                                                    //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public DataProperty RemoveData(string path) {                                                   //__SILP__
            return Remove<DataProperty>(path);                                                          //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public bool AddDataValueChecker(string path, Pass pass, IValueChecker<Data> checker) {          //__SILP__
            DataProperty p = Get<DataProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                            //__SILP__
                return p.AddValueChecker(pass, checker);                                                //__SILP__
            } else {                                                                                    //__SILP__
                Error("Property Not Exist: {0}", path);                                                 //__SILP__
            }                                                                                           //__SILP__
            return false;                                                                               //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public bool AddDataValueChecker(string path, IValueChecker<Data> checker) {                     //__SILP__
            return AddDataValueChecker(path, checker);                                                  //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public bool RemoveDataValueChecker(string path, Pass pass, IValueChecker<Data> checker) {       //__SILP__
            DataProperty p = Get<DataProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                            //__SILP__
                return p.RemoveValueChecker(pass, checker);                                             //__SILP__
            } else {                                                                                    //__SILP__
                Error("Property Not Exist: {0}", path);                                                 //__SILP__
            }                                                                                           //__SILP__
            return false;                                                                               //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public bool RemoveDataValueChecker(string path, IValueChecker<Data> checker) {                  //__SILP__
            return RemoveDataValueChecker(path, checker);                                               //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public BlockValueChecker<Data> AddDataBlockValueChecker(string path, Pass pass,                 //__SILP__
                                            IBlockOwner owner, Func<IVar<Data>, Data, bool> checker) {  //__SILP__
            DataProperty p = Get<DataProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                            //__SILP__
                return p.AddBlockValueChecker(pass, owner, checker);                                    //__SILP__
            } else {                                                                                    //__SILP__
                Error("Property Not Exist: {0}", path);                                                 //__SILP__
            }                                                                                           //__SILP__
            return null;                                                                                //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public BlockValueChecker<Data> AddDataBlockValueChecker(string path,                            //__SILP__
                                            IBlockOwner owner, Func<IVar<Data>, Data, bool> checker) {  //__SILP__
            return AddDataBlockValueChecker(path, owner, checker);                                      //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public bool AddDataValueWatcher(string path, IValueWatcher<Data> watcher) {                     //__SILP__
            DataProperty p = Get<DataProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                            //__SILP__
                return p.AddValueWatcher(watcher);                                                      //__SILP__
            } else {                                                                                    //__SILP__
                Error("Property Not Exist: {0}", path);                                                 //__SILP__
            }                                                                                           //__SILP__
            return false;                                                                               //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public bool RemoveDataValueWatcher(string path, IValueWatcher<Data> watcher) {                  //__SILP__
            DataProperty p = Get<DataProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                            //__SILP__
                return p.RemoveValueWatcher(watcher);                                                   //__SILP__
            } else {                                                                                    //__SILP__
                Error("Property Not Exist: {0}", path);                                                 //__SILP__
            }                                                                                           //__SILP__
            return false;                                                                               //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public BlockValueWatcher<Data> AddDataBlockValueWatcher(string path,                            //__SILP__
                                            IBlockOwner owner, Action<IVar<Data>, Data> watcher) {      //__SILP__
            DataProperty p = Get<DataProperty>(path);                                                   //__SILP__
            if (p != null) {                                                                            //__SILP__
                return p.AddBlockValueWatcher(owner, watcher);                                          //__SILP__
            } else {                                                                                    //__SILP__
                Error("Property Not Exist: {0}", path);                                                 //__SILP__
            }                                                                                           //__SILP__
            return null;                                                                                //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public bool IsData(string path) {                                                               //__SILP__
            return Get<DataProperty>(path) != null;                                                     //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public Data GetData(string path) {                                                              //__SILP__
            DataProperty v = Get<DataProperty>(path);                                                   //__SILP__
            if (v != null) {                                                                            //__SILP__
                return v.Value;                                                                         //__SILP__
            } else {                                                                                    //__SILP__
                Error("Property Not Exist: {0}", path);                                                 //__SILP__
            }                                                                                           //__SILP__
            return default(Data);                                                                       //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public Data GetData(string path, Data defaultValue) {                                           //__SILP__
            DataProperty v = Get<DataProperty>(path);                                                   //__SILP__
            if (v != null) {                                                                            //__SILP__
                return v.Value;                                                                         //__SILP__
            } else {                                                                                    //__SILP__
                Error("Property Not Exist: {0}", path);                                                 //__SILP__
            }                                                                                           //__SILP__
            return defaultValue;                                                                        //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
                                                                                                        //__SILP__
        public bool SetData(string path, Data val) {                                                    //__SILP__
            DataProperty v = Get<DataProperty>(path);                                                   //__SILP__
            if (v != null) {                                                                            //__SILP__
                return v.SetValue(val);                                                                 //__SILP__
            } else {                                                                                    //__SILP__
                Error("Property Not Exist: {0}", path);                                                 //__SILP__
            }                                                                                           //__SILP__
            return false;                                                                               //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
        public bool SetData(string path, Pass pass, Data val) {                                         //__SILP__
            DataProperty v = Get<DataProperty>(path);                                                   //__SILP__
            if (v != null) {                                                                            //__SILP__
                return v.SetValue(pass, val);                                                           //__SILP__
            } else {                                                                                    //__SILP__
                Error("Property Not Exist: {0}", path);                                                 //__SILP__
            }                                                                                           //__SILP__
            return false;                                                                               //__SILP__
        }                                                                                               //__SILP__
                                                                                                        //__SILP__
    }
}
