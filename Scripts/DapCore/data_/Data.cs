using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public interface IDataChecker {
        bool IsValid(Data data);
    }

    public enum DataType : byte {Invalid = 0, Bool, Int, Long, Float, Double, String, Data};

    public static class DataExtension {
        public static string ToFullString(this Data data, string indent = "\t") {
            return Convertor.DataConvertor.Convert(data, indent);
        }
    }

    public abstract class Data : ISealable {
        public const char TempPrefix = '_';
        public static bool IsTempKey(string key) {
            return key.Length > 0 && key[0] == TempPrefix;
        }

        public static Data Clone(Data data) {
            if (data == null) return null;

            return data.Clone();
        }

        public static bool IsNullOrEmpty(Data data) {
            if (data == null) return true;
            if (data.Count == 0) return true;
            return false;
        }

        public static bool IsStringEquals(string v1, string v2) {
            bool v1NullOrEmpty = string.IsNullOrEmpty(v1);
            bool v2NullOrEmpty = string.IsNullOrEmpty(v2);
            if (v1NullOrEmpty && v2NullOrEmpty) return true;
            if (v1NullOrEmpty || v2NullOrEmpty) return false;
            return v1.Equals(v2);
        }

        public static bool IsDataEquals(Data v1, Data v2) {
            bool v1NullOrEmpty = IsNullOrEmpty(v1);
            bool v2NullOrEmpty = IsNullOrEmpty(v2);
            if (v1NullOrEmpty && v2NullOrEmpty) return true;
            if (v1NullOrEmpty || v2NullOrEmpty) return false;
            return v1.Equals(v2);
        }

        private bool _Sealed = false;
        public bool Sealed {
            get { return _Sealed; }
        }

        public void Seal() {
            _Sealed = true;
        }

        public void _Recycle() {
            _Sealed = false;
            OnRecycle();
            Clear();
        }

        private bool IsValueEquals(Data data, string key, DataType valueType) {
            switch (valueType) {
                case DataType.Bool:
                    return GetBool(key) == data.GetBool(key);
                case DataType.Int:
                    return GetInt(key) == data.GetInt(key);
                case DataType.Long:
                    return GetLong(key) == data.GetLong(key);
                case DataType.Float:
                    return GetFloat(key) == data.GetFloat(key);
                case DataType.Double:
                    return GetDouble(key) == data.GetDouble(key);
                case DataType.String:
                    return IsStringEquals(GetString(key), data.GetString(key));
                case DataType.Data:
                    return IsDataEquals(GetData(key), data.GetData(key));
            }
            return false;
        }

        public override bool Equals(object obj) {
            if (this == obj) return true;
            if (obj == null) return false;

            Data data = obj as Data;
            if (data == null) return false;
            if (data.Count != this.Count) return false;

            foreach (string key in Keys) {
                DataType valueType = GetValueType(key);
                if (valueType != data.GetValueType(key)) {
                    return false;
                }
                if (!IsValueEquals(data, key, valueType)) {
                    return false;
                }
            }
            return true;
        }

        public override string ToString() {
            return string.Format("[{0}: {1}]", GetType().Name, Count);
        }

        public bool CopyValueTo(string key, Data toData, string toKey) {
            if (toData == null) return false;

            DataType valueType = GetValueType(key);
            if (valueType == DataType.Invalid) return false;

            switch (valueType) {
                case DataType.Bool:
                    return toData.SetBool(toKey, GetBool(key));
                case DataType.Int:
                    return toData.SetInt(toKey, GetInt(key));
                case DataType.Long:
                    return toData.SetLong(toKey, GetLong(key));
                case DataType.Float:
                    return toData.SetFloat(toKey, GetFloat(key));
                case DataType.Double:
                    return toData.SetDouble(toKey, GetDouble(key));
                case DataType.String:
                    return toData.SetString(toKey, GetString(key));
                case DataType.Data:
                    return toData.SetData(toKey, GetData(key));
            }
            return false;
        }

        protected virtual void OnRecycle() {}

        public abstract int Count { get; }
        public abstract IEnumerable<string> Keys { get; }

        public abstract Data Clone();
        public abstract void Clear();

        public abstract bool HasKey(string key);
        public abstract DataType GetValueType(string key);
        public abstract object GetValue(string key);
        public abstract bool TryGetValue(string key, out object val);

        //SILP: DATA_TYPE(Bool, bool)
        public abstract bool IsBool(string key);                                          //__SILP__
        public abstract bool TryGetBool(string key, out bool val, bool isDebug = false);  //__SILP__
        public abstract bool GetBool(string key);                                         //__SILP__
        public abstract bool GetBool(string key, bool defaultValue);                      //__SILP__
        public abstract bool SetBool(string key, bool val);                               //__SILP__
        public abstract void ForEachBool(Action<int, bool> callback);                     //__SILP__
        public abstract bool UntilTrueBool(Func<int, bool, bool> callback);               //__SILP__
        public abstract bool UntilFalseBool(Func<int, bool, bool> callback);              //__SILP__
                                                                                          //__SILP__
        //SILP: DATA_TYPE(Int, int)
        public abstract bool IsInt(string key);                                         //__SILP__
        public abstract bool TryGetInt(string key, out int val, bool isDebug = false);  //__SILP__
        public abstract int GetInt(string key);                                         //__SILP__
        public abstract int GetInt(string key, int defaultValue);                       //__SILP__
        public abstract bool SetInt(string key, int val);                               //__SILP__
        public abstract void ForEachInt(Action<int, int> callback);                     //__SILP__
        public abstract bool UntilTrueInt(Func<int, int, bool> callback);               //__SILP__
        public abstract bool UntilFalseInt(Func<int, int, bool> callback);              //__SILP__
                                                                                        //__SILP__
        //SILP: DATA_TYPE(Long, long)
        public abstract bool IsLong(string key);                                          //__SILP__
        public abstract bool TryGetLong(string key, out long val, bool isDebug = false);  //__SILP__
        public abstract long GetLong(string key);                                         //__SILP__
        public abstract long GetLong(string key, long defaultValue);                      //__SILP__
        public abstract bool SetLong(string key, long val);                               //__SILP__
        public abstract void ForEachLong(Action<int, long> callback);                     //__SILP__
        public abstract bool UntilTrueLong(Func<int, long, bool> callback);               //__SILP__
        public abstract bool UntilFalseLong(Func<int, long, bool> callback);              //__SILP__
                                                                                          //__SILP__
        //SILP: DATA_TYPE(Float, float)
        public abstract bool IsFloat(string key);                                           //__SILP__
        public abstract bool TryGetFloat(string key, out float val, bool isDebug = false);  //__SILP__
        public abstract float GetFloat(string key);                                         //__SILP__
        public abstract float GetFloat(string key, float defaultValue);                     //__SILP__
        public abstract bool SetFloat(string key, float val);                               //__SILP__
        public abstract void ForEachFloat(Action<int, float> callback);                     //__SILP__
        public abstract bool UntilTrueFloat(Func<int, float, bool> callback);               //__SILP__
        public abstract bool UntilFalseFloat(Func<int, float, bool> callback);              //__SILP__
                                                                                            //__SILP__
        //SILP: DATA_TYPE(Double, double)
        public abstract bool IsDouble(string key);                                            //__SILP__
        public abstract bool TryGetDouble(string key, out double val, bool isDebug = false);  //__SILP__
        public abstract double GetDouble(string key);                                         //__SILP__
        public abstract double GetDouble(string key, double defaultValue);                    //__SILP__
        public abstract bool SetDouble(string key, double val);                               //__SILP__
        public abstract void ForEachDouble(Action<int, double> callback);                     //__SILP__
        public abstract bool UntilTrueDouble(Func<int, double, bool> callback);               //__SILP__
        public abstract bool UntilFalseDouble(Func<int, double, bool> callback);              //__SILP__
                                                                                              //__SILP__
        //SILP: DATA_TYPE(String, string)
        public abstract bool IsString(string key);                                            //__SILP__
        public abstract bool TryGetString(string key, out string val, bool isDebug = false);  //__SILP__
        public abstract string GetString(string key);                                         //__SILP__
        public abstract string GetString(string key, string defaultValue);                    //__SILP__
        public abstract bool SetString(string key, string val);                               //__SILP__
        public abstract void ForEachString(Action<int, string> callback);                     //__SILP__
        public abstract bool UntilTrueString(Func<int, string, bool> callback);               //__SILP__
        public abstract bool UntilFalseString(Func<int, string, bool> callback);              //__SILP__
                                                                                              //__SILP__
        //SILP: DATA_TYPE(Data, Data)
        public abstract bool IsData(string key);                                          //__SILP__
        public abstract bool TryGetData(string key, out Data val, bool isDebug = false);  //__SILP__
        public abstract Data GetData(string key);                                         //__SILP__
        public abstract Data GetData(string key, Data defaultValue);                      //__SILP__
        public abstract bool SetData(string key, Data val);                               //__SILP__
        public abstract void ForEachData(Action<int, Data> callback);                     //__SILP__
        public abstract bool UntilTrueData(Func<int, Data, bool> callback);               //__SILP__
        public abstract bool UntilFalseData(Func<int, Data, bool> callback);              //__SILP__
                                                                                          //__SILP__

        //SILP: DATA_QUICK_SETTER(B, Bool, bool)
        public Data B(string key, bool val) {                         //__SILP__
            SetBool(key, val);                                        //__SILP__
            return this;                                              //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        //SILP: DATA_QUICK_SETTER(I, Int, int)
        public Data I(string key, int val) {                          //__SILP__
            SetInt(key, val);                                         //__SILP__
            return this;                                              //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        //SILP: DATA_QUICK_SETTER(L, Long, long)
        public Data L(string key, long val) {                         //__SILP__
            SetLong(key, val);                                        //__SILP__
            return this;                                              //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        //SILP: DATA_QUICK_SETTER(F, Float, float)
        public Data F(string key, float val) {                        //__SILP__
            SetFloat(key, val);                                       //__SILP__
            return this;                                              //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        //SILP: DATA_QUICK_SETTER(D, Double, double)
        public Data D(string key, double val) {                       //__SILP__
            SetDouble(key, val);                                      //__SILP__
            return this;                                              //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        //SILP: DATA_QUICK_SETTER(S, String, string)
        public Data S(string key, string val) {                       //__SILP__
            SetString(key, val);                                      //__SILP__
            return this;                                              //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        //SILP: DATA_QUICK_SETTER(A, Data, Data)
        public Data A(string key, Data val) {                         //__SILP__
            SetData(key, val);                                        //__SILP__
            return this;                                              //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
    }
}
