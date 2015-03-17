using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public struct VarsConsts {
        public const string TypeVars = "Vars";

        public const string TypeBoolVar = "BoolVar";
        public const string TypeIntVar = "IntVar";
        public const string TypeLongVar = "LongVar";
        public const string TypeFloatVar = "FloatVar";
        public const string TypeDoubleVar = "DoubleVar";
        public const string TypeStringVar = "StringVar";
        public const string TypeDataVar = "DataVar";

        public const string KeyValue = "value";
    }

    public class Vars : EntityAspect {
        public override string Type {
            get { return VarsConsts.TypeVars; }
        }

        public AnyVar<T> AddAnyVar<T>(string path, T val) {
            AnyVar<T> result = Add<AnyVar<T>>(path);
            if (result != null) {
                result.SetValue(val);
            }
            return result;
        }

        public bool HasAnyVar<T>(string path) {
            return Get<AnyVar<T>>(path) != null;
        }

        public AnyVar<T> GetAnyVar<T>(string path) {
            return Get<AnyVar<T>>(path);
        }

        public T GetAnyValue<T>(string path) {
            AnyVar<T> v = GetAnyVar<T>(path);
            if (v != null) {
                return v.Value;
            }
            return default(T);
        }

        public bool SetAnyValue<T>(string path, T val) {
            AnyVar<T> v = GetAnyVar<T>(path);
            if (v != null) {
                return v.SetValue(val);
            }
            return false;
        }

        //SILP: VARS_HELPER(Bool, bool)
        public BoolVar AddBool(string path, bool val) {               //__SILP__
            BoolVar v = Add<BoolVar>(path);                           //__SILP__
            if (v != null) {                                          //__SILP__
                v.SetValue(val);                                      //__SILP__
            }                                                         //__SILP__
            return v;                                                 //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public BoolVar RemoveBool(string path) {                      //__SILP__
            return Remove<BoolVar>(path);                             //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
                                                                      //__SILP__
        public bool IsBool(string path) {                             //__SILP__
            return Get<BoolVar>(path) != null;                        //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool GetBool(string path) {                            //__SILP__
            BoolVar v = Get<BoolVar>(path);                           //__SILP__
            if (v != null) {                                          //__SILP__
                return v.Value;                                       //__SILP__
            }                                                         //__SILP__
            return default(bool);                                     //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool GetBool(string path, bool defaultValue) {         //__SILP__
            BoolVar v = Get<BoolVar>(path);                           //__SILP__
            if (v != null) {                                          //__SILP__
                return v.Value;                                       //__SILP__
            }                                                         //__SILP__
            return defaultValue;                                      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool SetValue(string path, bool val) {                 //__SILP__
            BoolVar v = Get<BoolVar>(path);                           //__SILP__
            if (v != null) {                                          //__SILP__
                return v.SetValue(val);                               //__SILP__
            }                                                         //__SILP__
            return false;                                             //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        //SILP: VARS_HELPER(Int, int)
        public IntVar AddInt(string path, int val) {                  //__SILP__
            IntVar v = Add<IntVar>(path);                             //__SILP__
            if (v != null) {                                          //__SILP__
                v.SetValue(val);                                      //__SILP__
            }                                                         //__SILP__
            return v;                                                 //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public IntVar RemoveInt(string path) {                        //__SILP__
            return Remove<IntVar>(path);                              //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
                                                                      //__SILP__
        public bool IsInt(string path) {                              //__SILP__
            return Get<IntVar>(path) != null;                         //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public int GetInt(string path) {                              //__SILP__
            IntVar v = Get<IntVar>(path);                             //__SILP__
            if (v != null) {                                          //__SILP__
                return v.Value;                                       //__SILP__
            }                                                         //__SILP__
            return default(int);                                      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public int GetInt(string path, int defaultValue) {            //__SILP__
            IntVar v = Get<IntVar>(path);                             //__SILP__
            if (v != null) {                                          //__SILP__
                return v.Value;                                       //__SILP__
            }                                                         //__SILP__
            return defaultValue;                                      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool SetValue(string path, int val) {                  //__SILP__
            IntVar v = Get<IntVar>(path);                             //__SILP__
            if (v != null) {                                          //__SILP__
                return v.SetValue(val);                               //__SILP__
            }                                                         //__SILP__
            return false;                                             //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        //SILP: VARS_HELPER(Long, long)
        public LongVar AddLong(string path, long val) {               //__SILP__
            LongVar v = Add<LongVar>(path);                           //__SILP__
            if (v != null) {                                          //__SILP__
                v.SetValue(val);                                      //__SILP__
            }                                                         //__SILP__
            return v;                                                 //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public LongVar RemoveLong(string path) {                      //__SILP__
            return Remove<LongVar>(path);                             //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
                                                                      //__SILP__
        public bool IsLong(string path) {                             //__SILP__
            return Get<LongVar>(path) != null;                        //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public long GetLong(string path) {                            //__SILP__
            LongVar v = Get<LongVar>(path);                           //__SILP__
            if (v != null) {                                          //__SILP__
                return v.Value;                                       //__SILP__
            }                                                         //__SILP__
            return default(long);                                     //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public long GetLong(string path, long defaultValue) {         //__SILP__
            LongVar v = Get<LongVar>(path);                           //__SILP__
            if (v != null) {                                          //__SILP__
                return v.Value;                                       //__SILP__
            }                                                         //__SILP__
            return defaultValue;                                      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool SetValue(string path, long val) {                 //__SILP__
            LongVar v = Get<LongVar>(path);                           //__SILP__
            if (v != null) {                                          //__SILP__
                return v.SetValue(val);                               //__SILP__
            }                                                         //__SILP__
            return false;                                             //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        //SILP: VARS_HELPER(Float, float)
        public FloatVar AddFloat(string path, float val) {            //__SILP__
            FloatVar v = Add<FloatVar>(path);                         //__SILP__
            if (v != null) {                                          //__SILP__
                v.SetValue(val);                                      //__SILP__
            }                                                         //__SILP__
            return v;                                                 //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public FloatVar RemoveFloat(string path) {                    //__SILP__
            return Remove<FloatVar>(path);                            //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
                                                                      //__SILP__
        public bool IsFloat(string path) {                            //__SILP__
            return Get<FloatVar>(path) != null;                       //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public float GetFloat(string path) {                          //__SILP__
            FloatVar v = Get<FloatVar>(path);                         //__SILP__
            if (v != null) {                                          //__SILP__
                return v.Value;                                       //__SILP__
            }                                                         //__SILP__
            return default(float);                                    //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public float GetFloat(string path, float defaultValue) {      //__SILP__
            FloatVar v = Get<FloatVar>(path);                         //__SILP__
            if (v != null) {                                          //__SILP__
                return v.Value;                                       //__SILP__
            }                                                         //__SILP__
            return defaultValue;                                      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool SetValue(string path, float val) {                //__SILP__
            FloatVar v = Get<FloatVar>(path);                         //__SILP__
            if (v != null) {                                          //__SILP__
                return v.SetValue(val);                               //__SILP__
            }                                                         //__SILP__
            return false;                                             //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        //SILP: VARS_HELPER(Double, double)
        public DoubleVar AddDouble(string path, double val) {         //__SILP__
            DoubleVar v = Add<DoubleVar>(path);                       //__SILP__
            if (v != null) {                                          //__SILP__
                v.SetValue(val);                                      //__SILP__
            }                                                         //__SILP__
            return v;                                                 //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public DoubleVar RemoveDouble(string path) {                  //__SILP__
            return Remove<DoubleVar>(path);                           //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
                                                                      //__SILP__
        public bool IsDouble(string path) {                           //__SILP__
            return Get<DoubleVar>(path) != null;                      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public double GetDouble(string path) {                        //__SILP__
            DoubleVar v = Get<DoubleVar>(path);                       //__SILP__
            if (v != null) {                                          //__SILP__
                return v.Value;                                       //__SILP__
            }                                                         //__SILP__
            return default(double);                                   //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public double GetDouble(string path, double defaultValue) {   //__SILP__
            DoubleVar v = Get<DoubleVar>(path);                       //__SILP__
            if (v != null) {                                          //__SILP__
                return v.Value;                                       //__SILP__
            }                                                         //__SILP__
            return defaultValue;                                      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool SetValue(string path, double val) {               //__SILP__
            DoubleVar v = Get<DoubleVar>(path);                       //__SILP__
            if (v != null) {                                          //__SILP__
                return v.SetValue(val);                               //__SILP__
            }                                                         //__SILP__
            return false;                                             //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        //SILP: VARS_HELPER(String, string)
        public StringVar AddString(string path, string val) {         //__SILP__
            StringVar v = Add<StringVar>(path);                       //__SILP__
            if (v != null) {                                          //__SILP__
                v.SetValue(val);                                      //__SILP__
            }                                                         //__SILP__
            return v;                                                 //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public StringVar RemoveString(string path) {                  //__SILP__
            return Remove<StringVar>(path);                           //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
                                                                      //__SILP__
        public bool IsString(string path) {                           //__SILP__
            return Get<StringVar>(path) != null;                      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public string GetString(string path) {                        //__SILP__
            StringVar v = Get<StringVar>(path);                       //__SILP__
            if (v != null) {                                          //__SILP__
                return v.Value;                                       //__SILP__
            }                                                         //__SILP__
            return default(string);                                   //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public string GetString(string path, string defaultValue) {   //__SILP__
            StringVar v = Get<StringVar>(path);                       //__SILP__
            if (v != null) {                                          //__SILP__
                return v.Value;                                       //__SILP__
            }                                                         //__SILP__
            return defaultValue;                                      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool SetValue(string path, string val) {               //__SILP__
            StringVar v = Get<StringVar>(path);                       //__SILP__
            if (v != null) {                                          //__SILP__
                return v.SetValue(val);                               //__SILP__
            }                                                         //__SILP__
            return false;                                             //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        //SILP: VARS_HELPER(Data, Data)
        public DataVar AddData(string path, Data val) {               //__SILP__
            DataVar v = Add<DataVar>(path);                           //__SILP__
            if (v != null) {                                          //__SILP__
                v.SetValue(val);                                      //__SILP__
            }                                                         //__SILP__
            return v;                                                 //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public DataVar RemoveData(string path) {                      //__SILP__
            return Remove<DataVar>(path);                             //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
                                                                      //__SILP__
        public bool IsData(string path) {                             //__SILP__
            return Get<DataVar>(path) != null;                        //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public Data GetData(string path) {                            //__SILP__
            DataVar v = Get<DataVar>(path);                           //__SILP__
            if (v != null) {                                          //__SILP__
                return v.Value;                                       //__SILP__
            }                                                         //__SILP__
            return default(Data);                                     //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public Data GetData(string path, Data defaultValue) {         //__SILP__
            DataVar v = Get<DataVar>(path);                           //__SILP__
            if (v != null) {                                          //__SILP__
                return v.Value;                                       //__SILP__
            }                                                         //__SILP__
            return defaultValue;                                      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool SetValue(string path, Data val) {                 //__SILP__
            DataVar v = Get<DataVar>(path);                           //__SILP__
            if (v != null) {                                          //__SILP__
                return v.SetValue(val);                               //__SILP__
            }                                                         //__SILP__
            return false;                                             //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
    }
}
