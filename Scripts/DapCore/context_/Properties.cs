using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public struct PropertiesConsts {
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

    public class Properties : SecurableEntityAspect {
        public override string Type {
            get { return PropertiesConsts.TypeProperties; }
        }

        //SILP: PROPERTIES_HELPER(Bool, bool)
        public BoolProperty AddBool(string path, Object pass, bool val) {                                               //__SILP__
            BoolProperty v = Add<BoolProperty>(path);                                                                   //__SILP__
            if (v != null && !v.Setup(pass, val)) {                                                                     //__SILP__
                Remove<BoolProperty>(path);                                                                             //__SILP__
                v = null;                                                                                               //__SILP__
            }                                                                                                           //__SILP__
            return v;                                                                                                   //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public BoolProperty AddBool(string path, bool val) {                                                            //__SILP__
            return AddBool(path, null, val);                                                                            //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public BoolProperty RemoveBool(string path, Object pass) {                                                      //__SILP__
            return Remove<BoolProperty>(path, pass);                                                                    //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public BoolProperty RemoveBool(string path) {                                                                   //__SILP__
            return Remove<BoolProperty>(path);                                                                          //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public bool AddBoolValueChecker(string path, ValueChecker<bool> checker) {                                      //__SILP__
             BoolProperty p = Get<BoolProperty>(path);                                                                  //__SILP__
             if (p != null) {                                                                                           //__SILP__
                return p.AddValueChecker(checker);                                                                      //__SILP__
             }                                                                                                          //__SILP__
             return false;                                                                                              //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public bool RemoveBoolValueChecker(string path, ValueChecker<bool> checker) {                                   //__SILP__
            BoolProperty p = Get<BoolProperty>(path);                                                                   //__SILP__
            if (p != null) {                                                                                            //__SILP__
                return p.RemoveValueChecker(checker);                                                                   //__SILP__
            }                                                                                                           //__SILP__
            return false;                                                                                               //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public BoolBlockValueChecker AddBoolBlockValueChecker(string path, BoolBlockValueChecker.CheckerBlock block) {  //__SILP__
            BoolProperty p = Get<BoolProperty>(path);                                                                   //__SILP__
            if (p != null) {                                                                                            //__SILP__
                return p.AddBlockValueChecker(block);                                                                   //__SILP__
            }                                                                                                           //__SILP__
            return null;                                                                                                //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public bool AddBoolValueWatcher(string path, ValueWatcher<bool> watcher) {                                      //__SILP__
            BoolProperty p = Get<BoolProperty>(path);                                                                   //__SILP__
            if (p != null) {                                                                                            //__SILP__
                return p.AddValueWatcher(watcher);                                                                      //__SILP__
            }                                                                                                           //__SILP__
            return false;                                                                                               //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public bool RemoveBoolValueWatcher(string path, ValueWatcher<bool> watcher) {                                   //__SILP__
            BoolProperty p = Get<BoolProperty>(path);                                                                   //__SILP__
            if (p != null) {                                                                                            //__SILP__
                return p.RemoveValueWatcher(watcher);                                                                   //__SILP__
            }                                                                                                           //__SILP__
            return false;                                                                                               //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public BoolBlockValueWatcher AddBoolBlockValueWatcher(string path, BoolBlockValueWatcher.WatcherBlock block) {  //__SILP__
            BoolProperty p = Get<BoolProperty>(path);                                                                   //__SILP__
            if (p != null) {                                                                                            //__SILP__
                return p.AddBlockValueWatcher(block);                                                                   //__SILP__
            }                                                                                                           //__SILP__
            return null;                                                                                                //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public bool IsBool(string path) {                                                                               //__SILP__
            return Get<BoolProperty>(path) != null;                                                                     //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public bool GetBool(string path) {                                                                              //__SILP__
            BoolProperty v = Get<BoolProperty>(path);                                                                   //__SILP__
            if (v != null) {                                                                                            //__SILP__
                return v.Value;                                                                                         //__SILP__
            }                                                                                                           //__SILP__
            return default(bool);                                                                                       //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public bool GetBool(string path, bool defaultValue) {                                                           //__SILP__
            BoolProperty v = Get<BoolProperty>(path);                                                                   //__SILP__
            if (v != null) {                                                                                            //__SILP__
                return v.Value;                                                                                         //__SILP__
            }                                                                                                           //__SILP__
            return defaultValue;                                                                                        //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
                                                                                                                        //__SILP__
        public bool SetBool(string path, bool val) {                                                                    //__SILP__
            BoolProperty v = Get<BoolProperty>(path);                                                                   //__SILP__
            if (v != null) {                                                                                            //__SILP__
                return v.SetValue(val);                                                                                 //__SILP__
            }                                                                                                           //__SILP__
            return false;                                                                                               //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public bool SetBool(string path, Object pass, bool val) {                                                       //__SILP__
            BoolProperty v = Get<BoolProperty>(path);                                                                   //__SILP__
            if (v != null) {                                                                                            //__SILP__
                return v.SetValue(pass, val);                                                                           //__SILP__
            }                                                                                                           //__SILP__
            return false;                                                                                               //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        //SILP: PROPERTIES_HELPER(Int, int)
        public IntProperty AddInt(string path, Object pass, int val) {                                               //__SILP__
            IntProperty v = Add<IntProperty>(path);                                                                  //__SILP__
            if (v != null && !v.Setup(pass, val)) {                                                                  //__SILP__
                Remove<IntProperty>(path);                                                                           //__SILP__
                v = null;                                                                                            //__SILP__
            }                                                                                                        //__SILP__
            return v;                                                                                                //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
        public IntProperty AddInt(string path, int val) {                                                            //__SILP__
            return AddInt(path, null, val);                                                                          //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
        public IntProperty RemoveInt(string path, Object pass) {                                                     //__SILP__
            return Remove<IntProperty>(path, pass);                                                                  //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
        public IntProperty RemoveInt(string path) {                                                                  //__SILP__
            return Remove<IntProperty>(path);                                                                        //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
        public bool AddIntValueChecker(string path, ValueChecker<int> checker) {                                     //__SILP__
             IntProperty p = Get<IntProperty>(path);                                                                 //__SILP__
             if (p != null) {                                                                                        //__SILP__
                return p.AddValueChecker(checker);                                                                   //__SILP__
             }                                                                                                       //__SILP__
             return false;                                                                                           //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
        public bool RemoveIntValueChecker(string path, ValueChecker<int> checker) {                                  //__SILP__
            IntProperty p = Get<IntProperty>(path);                                                                  //__SILP__
            if (p != null) {                                                                                         //__SILP__
                return p.RemoveValueChecker(checker);                                                                //__SILP__
            }                                                                                                        //__SILP__
            return false;                                                                                            //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
        public IntBlockValueChecker AddIntBlockValueChecker(string path, IntBlockValueChecker.CheckerBlock block) {  //__SILP__
            IntProperty p = Get<IntProperty>(path);                                                                  //__SILP__
            if (p != null) {                                                                                         //__SILP__
                return p.AddBlockValueChecker(block);                                                                //__SILP__
            }                                                                                                        //__SILP__
            return null;                                                                                             //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
        public bool AddIntValueWatcher(string path, ValueWatcher<int> watcher) {                                     //__SILP__
            IntProperty p = Get<IntProperty>(path);                                                                  //__SILP__
            if (p != null) {                                                                                         //__SILP__
                return p.AddValueWatcher(watcher);                                                                   //__SILP__
            }                                                                                                        //__SILP__
            return false;                                                                                            //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
        public bool RemoveIntValueWatcher(string path, ValueWatcher<int> watcher) {                                  //__SILP__
            IntProperty p = Get<IntProperty>(path);                                                                  //__SILP__
            if (p != null) {                                                                                         //__SILP__
                return p.RemoveValueWatcher(watcher);                                                                //__SILP__
            }                                                                                                        //__SILP__
            return false;                                                                                            //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
        public IntBlockValueWatcher AddIntBlockValueWatcher(string path, IntBlockValueWatcher.WatcherBlock block) {  //__SILP__
            IntProperty p = Get<IntProperty>(path);                                                                  //__SILP__
            if (p != null) {                                                                                         //__SILP__
                return p.AddBlockValueWatcher(block);                                                                //__SILP__
            }                                                                                                        //__SILP__
            return null;                                                                                             //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
        public bool IsInt(string path) {                                                                             //__SILP__
            return Get<IntProperty>(path) != null;                                                                   //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
        public int GetInt(string path) {                                                                             //__SILP__
            IntProperty v = Get<IntProperty>(path);                                                                  //__SILP__
            if (v != null) {                                                                                         //__SILP__
                return v.Value;                                                                                      //__SILP__
            }                                                                                                        //__SILP__
            return default(int);                                                                                     //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
        public int GetInt(string path, int defaultValue) {                                                           //__SILP__
            IntProperty v = Get<IntProperty>(path);                                                                  //__SILP__
            if (v != null) {                                                                                         //__SILP__
                return v.Value;                                                                                      //__SILP__
            }                                                                                                        //__SILP__
            return defaultValue;                                                                                     //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
                                                                                                                     //__SILP__
        public bool SetInt(string path, int val) {                                                                   //__SILP__
            IntProperty v = Get<IntProperty>(path);                                                                  //__SILP__
            if (v != null) {                                                                                         //__SILP__
                return v.SetValue(val);                                                                              //__SILP__
            }                                                                                                        //__SILP__
            return false;                                                                                            //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
        public bool SetInt(string path, Object pass, int val) {                                                      //__SILP__
            IntProperty v = Get<IntProperty>(path);                                                                  //__SILP__
            if (v != null) {                                                                                         //__SILP__
                return v.SetValue(pass, val);                                                                        //__SILP__
            }                                                                                                        //__SILP__
            return false;                                                                                            //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
        //SILP: PROPERTIES_HELPER(Long, long)
        public LongProperty AddLong(string path, Object pass, long val) {                                               //__SILP__
            LongProperty v = Add<LongProperty>(path);                                                                   //__SILP__
            if (v != null && !v.Setup(pass, val)) {                                                                     //__SILP__
                Remove<LongProperty>(path);                                                                             //__SILP__
                v = null;                                                                                               //__SILP__
            }                                                                                                           //__SILP__
            return v;                                                                                                   //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public LongProperty AddLong(string path, long val) {                                                            //__SILP__
            return AddLong(path, null, val);                                                                            //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public LongProperty RemoveLong(string path, Object pass) {                                                      //__SILP__
            return Remove<LongProperty>(path, pass);                                                                    //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public LongProperty RemoveLong(string path) {                                                                   //__SILP__
            return Remove<LongProperty>(path);                                                                          //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public bool AddLongValueChecker(string path, ValueChecker<long> checker) {                                      //__SILP__
             LongProperty p = Get<LongProperty>(path);                                                                  //__SILP__
             if (p != null) {                                                                                           //__SILP__
                return p.AddValueChecker(checker);                                                                      //__SILP__
             }                                                                                                          //__SILP__
             return false;                                                                                              //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public bool RemoveLongValueChecker(string path, ValueChecker<long> checker) {                                   //__SILP__
            LongProperty p = Get<LongProperty>(path);                                                                   //__SILP__
            if (p != null) {                                                                                            //__SILP__
                return p.RemoveValueChecker(checker);                                                                   //__SILP__
            }                                                                                                           //__SILP__
            return false;                                                                                               //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public LongBlockValueChecker AddLongBlockValueChecker(string path, LongBlockValueChecker.CheckerBlock block) {  //__SILP__
            LongProperty p = Get<LongProperty>(path);                                                                   //__SILP__
            if (p != null) {                                                                                            //__SILP__
                return p.AddBlockValueChecker(block);                                                                   //__SILP__
            }                                                                                                           //__SILP__
            return null;                                                                                                //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public bool AddLongValueWatcher(string path, ValueWatcher<long> watcher) {                                      //__SILP__
            LongProperty p = Get<LongProperty>(path);                                                                   //__SILP__
            if (p != null) {                                                                                            //__SILP__
                return p.AddValueWatcher(watcher);                                                                      //__SILP__
            }                                                                                                           //__SILP__
            return false;                                                                                               //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public bool RemoveLongValueWatcher(string path, ValueWatcher<long> watcher) {                                   //__SILP__
            LongProperty p = Get<LongProperty>(path);                                                                   //__SILP__
            if (p != null) {                                                                                            //__SILP__
                return p.RemoveValueWatcher(watcher);                                                                   //__SILP__
            }                                                                                                           //__SILP__
            return false;                                                                                               //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public LongBlockValueWatcher AddLongBlockValueWatcher(string path, LongBlockValueWatcher.WatcherBlock block) {  //__SILP__
            LongProperty p = Get<LongProperty>(path);                                                                   //__SILP__
            if (p != null) {                                                                                            //__SILP__
                return p.AddBlockValueWatcher(block);                                                                   //__SILP__
            }                                                                                                           //__SILP__
            return null;                                                                                                //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public bool IsLong(string path) {                                                                               //__SILP__
            return Get<LongProperty>(path) != null;                                                                     //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public long GetLong(string path) {                                                                              //__SILP__
            LongProperty v = Get<LongProperty>(path);                                                                   //__SILP__
            if (v != null) {                                                                                            //__SILP__
                return v.Value;                                                                                         //__SILP__
            }                                                                                                           //__SILP__
            return default(long);                                                                                       //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public long GetLong(string path, long defaultValue) {                                                           //__SILP__
            LongProperty v = Get<LongProperty>(path);                                                                   //__SILP__
            if (v != null) {                                                                                            //__SILP__
                return v.Value;                                                                                         //__SILP__
            }                                                                                                           //__SILP__
            return defaultValue;                                                                                        //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
                                                                                                                        //__SILP__
        public bool SetLong(string path, long val) {                                                                    //__SILP__
            LongProperty v = Get<LongProperty>(path);                                                                   //__SILP__
            if (v != null) {                                                                                            //__SILP__
                return v.SetValue(val);                                                                                 //__SILP__
            }                                                                                                           //__SILP__
            return false;                                                                                               //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public bool SetLong(string path, Object pass, long val) {                                                       //__SILP__
            LongProperty v = Get<LongProperty>(path);                                                                   //__SILP__
            if (v != null) {                                                                                            //__SILP__
                return v.SetValue(pass, val);                                                                           //__SILP__
            }                                                                                                           //__SILP__
            return false;                                                                                               //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        //SILP: PROPERTIES_HELPER(Float, float)
        public FloatProperty AddFloat(string path, Object pass, float val) {                                               //__SILP__
            FloatProperty v = Add<FloatProperty>(path);                                                                    //__SILP__
            if (v != null && !v.Setup(pass, val)) {                                                                        //__SILP__
                Remove<FloatProperty>(path);                                                                               //__SILP__
                v = null;                                                                                                  //__SILP__
            }                                                                                                              //__SILP__
            return v;                                                                                                      //__SILP__
        }                                                                                                                  //__SILP__
                                                                                                                           //__SILP__
        public FloatProperty AddFloat(string path, float val) {                                                            //__SILP__
            return AddFloat(path, null, val);                                                                              //__SILP__
        }                                                                                                                  //__SILP__
                                                                                                                           //__SILP__
        public FloatProperty RemoveFloat(string path, Object pass) {                                                       //__SILP__
            return Remove<FloatProperty>(path, pass);                                                                      //__SILP__
        }                                                                                                                  //__SILP__
                                                                                                                           //__SILP__
        public FloatProperty RemoveFloat(string path) {                                                                    //__SILP__
            return Remove<FloatProperty>(path);                                                                            //__SILP__
        }                                                                                                                  //__SILP__
                                                                                                                           //__SILP__
        public bool AddFloatValueChecker(string path, ValueChecker<float> checker) {                                       //__SILP__
             FloatProperty p = Get<FloatProperty>(path);                                                                   //__SILP__
             if (p != null) {                                                                                              //__SILP__
                return p.AddValueChecker(checker);                                                                         //__SILP__
             }                                                                                                             //__SILP__
             return false;                                                                                                 //__SILP__
        }                                                                                                                  //__SILP__
                                                                                                                           //__SILP__
        public bool RemoveFloatValueChecker(string path, ValueChecker<float> checker) {                                    //__SILP__
            FloatProperty p = Get<FloatProperty>(path);                                                                    //__SILP__
            if (p != null) {                                                                                               //__SILP__
                return p.RemoveValueChecker(checker);                                                                      //__SILP__
            }                                                                                                              //__SILP__
            return false;                                                                                                  //__SILP__
        }                                                                                                                  //__SILP__
                                                                                                                           //__SILP__
        public FloatBlockValueChecker AddFloatBlockValueChecker(string path, FloatBlockValueChecker.CheckerBlock block) {  //__SILP__
            FloatProperty p = Get<FloatProperty>(path);                                                                    //__SILP__
            if (p != null) {                                                                                               //__SILP__
                return p.AddBlockValueChecker(block);                                                                      //__SILP__
            }                                                                                                              //__SILP__
            return null;                                                                                                   //__SILP__
        }                                                                                                                  //__SILP__
                                                                                                                           //__SILP__
        public bool AddFloatValueWatcher(string path, ValueWatcher<float> watcher) {                                       //__SILP__
            FloatProperty p = Get<FloatProperty>(path);                                                                    //__SILP__
            if (p != null) {                                                                                               //__SILP__
                return p.AddValueWatcher(watcher);                                                                         //__SILP__
            }                                                                                                              //__SILP__
            return false;                                                                                                  //__SILP__
        }                                                                                                                  //__SILP__
                                                                                                                           //__SILP__
        public bool RemoveFloatValueWatcher(string path, ValueWatcher<float> watcher) {                                    //__SILP__
            FloatProperty p = Get<FloatProperty>(path);                                                                    //__SILP__
            if (p != null) {                                                                                               //__SILP__
                return p.RemoveValueWatcher(watcher);                                                                      //__SILP__
            }                                                                                                              //__SILP__
            return false;                                                                                                  //__SILP__
        }                                                                                                                  //__SILP__
                                                                                                                           //__SILP__
        public FloatBlockValueWatcher AddFloatBlockValueWatcher(string path, FloatBlockValueWatcher.WatcherBlock block) {  //__SILP__
            FloatProperty p = Get<FloatProperty>(path);                                                                    //__SILP__
            if (p != null) {                                                                                               //__SILP__
                return p.AddBlockValueWatcher(block);                                                                      //__SILP__
            }                                                                                                              //__SILP__
            return null;                                                                                                   //__SILP__
        }                                                                                                                  //__SILP__
                                                                                                                           //__SILP__
        public bool IsFloat(string path) {                                                                                 //__SILP__
            return Get<FloatProperty>(path) != null;                                                                       //__SILP__
        }                                                                                                                  //__SILP__
                                                                                                                           //__SILP__
        public float GetFloat(string path) {                                                                               //__SILP__
            FloatProperty v = Get<FloatProperty>(path);                                                                    //__SILP__
            if (v != null) {                                                                                               //__SILP__
                return v.Value;                                                                                            //__SILP__
            }                                                                                                              //__SILP__
            return default(float);                                                                                         //__SILP__
        }                                                                                                                  //__SILP__
                                                                                                                           //__SILP__
        public float GetFloat(string path, float defaultValue) {                                                           //__SILP__
            FloatProperty v = Get<FloatProperty>(path);                                                                    //__SILP__
            if (v != null) {                                                                                               //__SILP__
                return v.Value;                                                                                            //__SILP__
            }                                                                                                              //__SILP__
            return defaultValue;                                                                                           //__SILP__
        }                                                                                                                  //__SILP__
                                                                                                                           //__SILP__
                                                                                                                           //__SILP__
        public bool SetFloat(string path, float val) {                                                                     //__SILP__
            FloatProperty v = Get<FloatProperty>(path);                                                                    //__SILP__
            if (v != null) {                                                                                               //__SILP__
                return v.SetValue(val);                                                                                    //__SILP__
            }                                                                                                              //__SILP__
            return false;                                                                                                  //__SILP__
        }                                                                                                                  //__SILP__
                                                                                                                           //__SILP__
        public bool SetFloat(string path, Object pass, float val) {                                                        //__SILP__
            FloatProperty v = Get<FloatProperty>(path);                                                                    //__SILP__
            if (v != null) {                                                                                               //__SILP__
                return v.SetValue(pass, val);                                                                              //__SILP__
            }                                                                                                              //__SILP__
            return false;                                                                                                  //__SILP__
        }                                                                                                                  //__SILP__
                                                                                                                           //__SILP__
        //SILP: PROPERTIES_HELPER(Double, double)
        public DoubleProperty AddDouble(string path, Object pass, double val) {                                               //__SILP__
            DoubleProperty v = Add<DoubleProperty>(path);                                                                     //__SILP__
            if (v != null && !v.Setup(pass, val)) {                                                                           //__SILP__
                Remove<DoubleProperty>(path);                                                                                 //__SILP__
                v = null;                                                                                                     //__SILP__
            }                                                                                                                 //__SILP__
            return v;                                                                                                         //__SILP__
        }                                                                                                                     //__SILP__
                                                                                                                              //__SILP__
        public DoubleProperty AddDouble(string path, double val) {                                                            //__SILP__
            return AddDouble(path, null, val);                                                                                //__SILP__
        }                                                                                                                     //__SILP__
                                                                                                                              //__SILP__
        public DoubleProperty RemoveDouble(string path, Object pass) {                                                        //__SILP__
            return Remove<DoubleProperty>(path, pass);                                                                        //__SILP__
        }                                                                                                                     //__SILP__
                                                                                                                              //__SILP__
        public DoubleProperty RemoveDouble(string path) {                                                                     //__SILP__
            return Remove<DoubleProperty>(path);                                                                              //__SILP__
        }                                                                                                                     //__SILP__
                                                                                                                              //__SILP__
        public bool AddDoubleValueChecker(string path, ValueChecker<double> checker) {                                        //__SILP__
             DoubleProperty p = Get<DoubleProperty>(path);                                                                    //__SILP__
             if (p != null) {                                                                                                 //__SILP__
                return p.AddValueChecker(checker);                                                                            //__SILP__
             }                                                                                                                //__SILP__
             return false;                                                                                                    //__SILP__
        }                                                                                                                     //__SILP__
                                                                                                                              //__SILP__
        public bool RemoveDoubleValueChecker(string path, ValueChecker<double> checker) {                                     //__SILP__
            DoubleProperty p = Get<DoubleProperty>(path);                                                                     //__SILP__
            if (p != null) {                                                                                                  //__SILP__
                return p.RemoveValueChecker(checker);                                                                         //__SILP__
            }                                                                                                                 //__SILP__
            return false;                                                                                                     //__SILP__
        }                                                                                                                     //__SILP__
                                                                                                                              //__SILP__
        public DoubleBlockValueChecker AddDoubleBlockValueChecker(string path, DoubleBlockValueChecker.CheckerBlock block) {  //__SILP__
            DoubleProperty p = Get<DoubleProperty>(path);                                                                     //__SILP__
            if (p != null) {                                                                                                  //__SILP__
                return p.AddBlockValueChecker(block);                                                                         //__SILP__
            }                                                                                                                 //__SILP__
            return null;                                                                                                      //__SILP__
        }                                                                                                                     //__SILP__
                                                                                                                              //__SILP__
        public bool AddDoubleValueWatcher(string path, ValueWatcher<double> watcher) {                                        //__SILP__
            DoubleProperty p = Get<DoubleProperty>(path);                                                                     //__SILP__
            if (p != null) {                                                                                                  //__SILP__
                return p.AddValueWatcher(watcher);                                                                            //__SILP__
            }                                                                                                                 //__SILP__
            return false;                                                                                                     //__SILP__
        }                                                                                                                     //__SILP__
                                                                                                                              //__SILP__
        public bool RemoveDoubleValueWatcher(string path, ValueWatcher<double> watcher) {                                     //__SILP__
            DoubleProperty p = Get<DoubleProperty>(path);                                                                     //__SILP__
            if (p != null) {                                                                                                  //__SILP__
                return p.RemoveValueWatcher(watcher);                                                                         //__SILP__
            }                                                                                                                 //__SILP__
            return false;                                                                                                     //__SILP__
        }                                                                                                                     //__SILP__
                                                                                                                              //__SILP__
        public DoubleBlockValueWatcher AddDoubleBlockValueWatcher(string path, DoubleBlockValueWatcher.WatcherBlock block) {  //__SILP__
            DoubleProperty p = Get<DoubleProperty>(path);                                                                     //__SILP__
            if (p != null) {                                                                                                  //__SILP__
                return p.AddBlockValueWatcher(block);                                                                         //__SILP__
            }                                                                                                                 //__SILP__
            return null;                                                                                                      //__SILP__
        }                                                                                                                     //__SILP__
                                                                                                                              //__SILP__
        public bool IsDouble(string path) {                                                                                   //__SILP__
            return Get<DoubleProperty>(path) != null;                                                                         //__SILP__
        }                                                                                                                     //__SILP__
                                                                                                                              //__SILP__
        public double GetDouble(string path) {                                                                                //__SILP__
            DoubleProperty v = Get<DoubleProperty>(path);                                                                     //__SILP__
            if (v != null) {                                                                                                  //__SILP__
                return v.Value;                                                                                               //__SILP__
            }                                                                                                                 //__SILP__
            return default(double);                                                                                           //__SILP__
        }                                                                                                                     //__SILP__
                                                                                                                              //__SILP__
        public double GetDouble(string path, double defaultValue) {                                                           //__SILP__
            DoubleProperty v = Get<DoubleProperty>(path);                                                                     //__SILP__
            if (v != null) {                                                                                                  //__SILP__
                return v.Value;                                                                                               //__SILP__
            }                                                                                                                 //__SILP__
            return defaultValue;                                                                                              //__SILP__
        }                                                                                                                     //__SILP__
                                                                                                                              //__SILP__
                                                                                                                              //__SILP__
        public bool SetDouble(string path, double val) {                                                                      //__SILP__
            DoubleProperty v = Get<DoubleProperty>(path);                                                                     //__SILP__
            if (v != null) {                                                                                                  //__SILP__
                return v.SetValue(val);                                                                                       //__SILP__
            }                                                                                                                 //__SILP__
            return false;                                                                                                     //__SILP__
        }                                                                                                                     //__SILP__
                                                                                                                              //__SILP__
        public bool SetDouble(string path, Object pass, double val) {                                                         //__SILP__
            DoubleProperty v = Get<DoubleProperty>(path);                                                                     //__SILP__
            if (v != null) {                                                                                                  //__SILP__
                return v.SetValue(pass, val);                                                                                 //__SILP__
            }                                                                                                                 //__SILP__
            return false;                                                                                                     //__SILP__
        }                                                                                                                     //__SILP__
                                                                                                                              //__SILP__
        //SILP: PROPERTIES_HELPER(String, string)
        public StringProperty AddString(string path, Object pass, string val) {                                               //__SILP__
            StringProperty v = Add<StringProperty>(path);                                                                     //__SILP__
            if (v != null && !v.Setup(pass, val)) {                                                                           //__SILP__
                Remove<StringProperty>(path);                                                                                 //__SILP__
                v = null;                                                                                                     //__SILP__
            }                                                                                                                 //__SILP__
            return v;                                                                                                         //__SILP__
        }                                                                                                                     //__SILP__
                                                                                                                              //__SILP__
        public StringProperty AddString(string path, string val) {                                                            //__SILP__
            return AddString(path, null, val);                                                                                //__SILP__
        }                                                                                                                     //__SILP__
                                                                                                                              //__SILP__
        public StringProperty RemoveString(string path, Object pass) {                                                        //__SILP__
            return Remove<StringProperty>(path, pass);                                                                        //__SILP__
        }                                                                                                                     //__SILP__
                                                                                                                              //__SILP__
        public StringProperty RemoveString(string path) {                                                                     //__SILP__
            return Remove<StringProperty>(path);                                                                              //__SILP__
        }                                                                                                                     //__SILP__
                                                                                                                              //__SILP__
        public bool AddStringValueChecker(string path, ValueChecker<string> checker) {                                        //__SILP__
             StringProperty p = Get<StringProperty>(path);                                                                    //__SILP__
             if (p != null) {                                                                                                 //__SILP__
                return p.AddValueChecker(checker);                                                                            //__SILP__
             }                                                                                                                //__SILP__
             return false;                                                                                                    //__SILP__
        }                                                                                                                     //__SILP__
                                                                                                                              //__SILP__
        public bool RemoveStringValueChecker(string path, ValueChecker<string> checker) {                                     //__SILP__
            StringProperty p = Get<StringProperty>(path);                                                                     //__SILP__
            if (p != null) {                                                                                                  //__SILP__
                return p.RemoveValueChecker(checker);                                                                         //__SILP__
            }                                                                                                                 //__SILP__
            return false;                                                                                                     //__SILP__
        }                                                                                                                     //__SILP__
                                                                                                                              //__SILP__
        public StringBlockValueChecker AddStringBlockValueChecker(string path, StringBlockValueChecker.CheckerBlock block) {  //__SILP__
            StringProperty p = Get<StringProperty>(path);                                                                     //__SILP__
            if (p != null) {                                                                                                  //__SILP__
                return p.AddBlockValueChecker(block);                                                                         //__SILP__
            }                                                                                                                 //__SILP__
            return null;                                                                                                      //__SILP__
        }                                                                                                                     //__SILP__
                                                                                                                              //__SILP__
        public bool AddStringValueWatcher(string path, ValueWatcher<string> watcher) {                                        //__SILP__
            StringProperty p = Get<StringProperty>(path);                                                                     //__SILP__
            if (p != null) {                                                                                                  //__SILP__
                return p.AddValueWatcher(watcher);                                                                            //__SILP__
            }                                                                                                                 //__SILP__
            return false;                                                                                                     //__SILP__
        }                                                                                                                     //__SILP__
                                                                                                                              //__SILP__
        public bool RemoveStringValueWatcher(string path, ValueWatcher<string> watcher) {                                     //__SILP__
            StringProperty p = Get<StringProperty>(path);                                                                     //__SILP__
            if (p != null) {                                                                                                  //__SILP__
                return p.RemoveValueWatcher(watcher);                                                                         //__SILP__
            }                                                                                                                 //__SILP__
            return false;                                                                                                     //__SILP__
        }                                                                                                                     //__SILP__
                                                                                                                              //__SILP__
        public StringBlockValueWatcher AddStringBlockValueWatcher(string path, StringBlockValueWatcher.WatcherBlock block) {  //__SILP__
            StringProperty p = Get<StringProperty>(path);                                                                     //__SILP__
            if (p != null) {                                                                                                  //__SILP__
                return p.AddBlockValueWatcher(block);                                                                         //__SILP__
            }                                                                                                                 //__SILP__
            return null;                                                                                                      //__SILP__
        }                                                                                                                     //__SILP__
                                                                                                                              //__SILP__
        public bool IsString(string path) {                                                                                   //__SILP__
            return Get<StringProperty>(path) != null;                                                                         //__SILP__
        }                                                                                                                     //__SILP__
                                                                                                                              //__SILP__
        public string GetString(string path) {                                                                                //__SILP__
            StringProperty v = Get<StringProperty>(path);                                                                     //__SILP__
            if (v != null) {                                                                                                  //__SILP__
                return v.Value;                                                                                               //__SILP__
            }                                                                                                                 //__SILP__
            return default(string);                                                                                           //__SILP__
        }                                                                                                                     //__SILP__
                                                                                                                              //__SILP__
        public string GetString(string path, string defaultValue) {                                                           //__SILP__
            StringProperty v = Get<StringProperty>(path);                                                                     //__SILP__
            if (v != null) {                                                                                                  //__SILP__
                return v.Value;                                                                                               //__SILP__
            }                                                                                                                 //__SILP__
            return defaultValue;                                                                                              //__SILP__
        }                                                                                                                     //__SILP__
                                                                                                                              //__SILP__
                                                                                                                              //__SILP__
        public bool SetString(string path, string val) {                                                                      //__SILP__
            StringProperty v = Get<StringProperty>(path);                                                                     //__SILP__
            if (v != null) {                                                                                                  //__SILP__
                return v.SetValue(val);                                                                                       //__SILP__
            }                                                                                                                 //__SILP__
            return false;                                                                                                     //__SILP__
        }                                                                                                                     //__SILP__
                                                                                                                              //__SILP__
        public bool SetString(string path, Object pass, string val) {                                                         //__SILP__
            StringProperty v = Get<StringProperty>(path);                                                                     //__SILP__
            if (v != null) {                                                                                                  //__SILP__
                return v.SetValue(pass, val);                                                                                 //__SILP__
            }                                                                                                                 //__SILP__
            return false;                                                                                                     //__SILP__
        }                                                                                                                     //__SILP__
                                                                                                                              //__SILP__
        //SILP: PROPERTIES_HELPER(Data, Data)
        public DataProperty AddData(string path, Object pass, Data val) {                                               //__SILP__
            DataProperty v = Add<DataProperty>(path);                                                                   //__SILP__
            if (v != null && !v.Setup(pass, val)) {                                                                     //__SILP__
                Remove<DataProperty>(path);                                                                             //__SILP__
                v = null;                                                                                               //__SILP__
            }                                                                                                           //__SILP__
            return v;                                                                                                   //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public DataProperty AddData(string path, Data val) {                                                            //__SILP__
            return AddData(path, null, val);                                                                            //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public DataProperty RemoveData(string path, Object pass) {                                                      //__SILP__
            return Remove<DataProperty>(path, pass);                                                                    //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public DataProperty RemoveData(string path) {                                                                   //__SILP__
            return Remove<DataProperty>(path);                                                                          //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public bool AddDataValueChecker(string path, ValueChecker<Data> checker) {                                      //__SILP__
             DataProperty p = Get<DataProperty>(path);                                                                  //__SILP__
             if (p != null) {                                                                                           //__SILP__
                return p.AddValueChecker(checker);                                                                      //__SILP__
             }                                                                                                          //__SILP__
             return false;                                                                                              //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public bool RemoveDataValueChecker(string path, ValueChecker<Data> checker) {                                   //__SILP__
            DataProperty p = Get<DataProperty>(path);                                                                   //__SILP__
            if (p != null) {                                                                                            //__SILP__
                return p.RemoveValueChecker(checker);                                                                   //__SILP__
            }                                                                                                           //__SILP__
            return false;                                                                                               //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public DataBlockValueChecker AddDataBlockValueChecker(string path, DataBlockValueChecker.CheckerBlock block) {  //__SILP__
            DataProperty p = Get<DataProperty>(path);                                                                   //__SILP__
            if (p != null) {                                                                                            //__SILP__
                return p.AddBlockValueChecker(block);                                                                   //__SILP__
            }                                                                                                           //__SILP__
            return null;                                                                                                //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public bool AddDataValueWatcher(string path, ValueWatcher<Data> watcher) {                                      //__SILP__
            DataProperty p = Get<DataProperty>(path);                                                                   //__SILP__
            if (p != null) {                                                                                            //__SILP__
                return p.AddValueWatcher(watcher);                                                                      //__SILP__
            }                                                                                                           //__SILP__
            return false;                                                                                               //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public bool RemoveDataValueWatcher(string path, ValueWatcher<Data> watcher) {                                   //__SILP__
            DataProperty p = Get<DataProperty>(path);                                                                   //__SILP__
            if (p != null) {                                                                                            //__SILP__
                return p.RemoveValueWatcher(watcher);                                                                   //__SILP__
            }                                                                                                           //__SILP__
            return false;                                                                                               //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public DataBlockValueWatcher AddDataBlockValueWatcher(string path, DataBlockValueWatcher.WatcherBlock block) {  //__SILP__
            DataProperty p = Get<DataProperty>(path);                                                                   //__SILP__
            if (p != null) {                                                                                            //__SILP__
                return p.AddBlockValueWatcher(block);                                                                   //__SILP__
            }                                                                                                           //__SILP__
            return null;                                                                                                //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public bool IsData(string path) {                                                                               //__SILP__
            return Get<DataProperty>(path) != null;                                                                     //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public Data GetData(string path) {                                                                              //__SILP__
            DataProperty v = Get<DataProperty>(path);                                                                   //__SILP__
            if (v != null) {                                                                                            //__SILP__
                return v.Value;                                                                                         //__SILP__
            }                                                                                                           //__SILP__
            return default(Data);                                                                                       //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public Data GetData(string path, Data defaultValue) {                                                           //__SILP__
            DataProperty v = Get<DataProperty>(path);                                                                   //__SILP__
            if (v != null) {                                                                                            //__SILP__
                return v.Value;                                                                                         //__SILP__
            }                                                                                                           //__SILP__
            return defaultValue;                                                                                        //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
                                                                                                                        //__SILP__
        public bool SetData(string path, Data val) {                                                                    //__SILP__
            DataProperty v = Get<DataProperty>(path);                                                                   //__SILP__
            if (v != null) {                                                                                            //__SILP__
                return v.SetValue(val);                                                                                 //__SILP__
            }                                                                                                           //__SILP__
            return false;                                                                                               //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public bool SetData(string path, Object pass, Data val) {                                                       //__SILP__
            DataProperty v = Get<DataProperty>(path);                                                                   //__SILP__
            if (v != null) {                                                                                            //__SILP__
                return v.SetValue(pass, val);                                                                           //__SILP__
            }                                                                                                           //__SILP__
            return false;                                                                                               //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
    }
}
