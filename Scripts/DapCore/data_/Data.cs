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

    public sealed class Data : Sealable {
        public static int MaxFullStringKeyCount = 12;

        public const string TempPrefix = "_";
        public static bool IsTempKey(string key) {
            return key.StartsWith(TempPrefix);
        }

        public static Data Clone(Data data) {
            if (data == null) return null;

            return data.Clone();
        }

        private Dictionary<string, DataType> _ValueTypes = new Dictionary<string, DataType>();

        private Dictionary<string, bool> _BoolValues = null;
        private Dictionary<string, int> _IntValues = null;
        private Dictionary<string, long> _LongValues = null;
        private Dictionary<string, float> _FloatValues = null;
        private Dictionary<string, double> _DoubleValues = null;
        private Dictionary<string, string> _StringValues = null;
        private Dictionary<string, Data> _DataValues = null;

        public override string ToString() {
            if (DeepCount <= MaxFullStringKeyCount) {
                return this.ToFullString(null);
            }
            return string.Format("[Data:{0}]", Count);
        }

        public Data Clone() {
            Data clone = new Data();
            clone._ValueTypes = CloneDictionary<DataType>(_ValueTypes);
            clone._BoolValues = CloneDictionary<bool>(_BoolValues);
            clone._IntValues = CloneDictionary<int>(_IntValues);
            clone._LongValues = CloneDictionary<long>(_LongValues);
            clone._FloatValues = CloneDictionary<float>(_FloatValues);
            clone._DoubleValues = CloneDictionary<double>(_DoubleValues);
            clone._StringValues = CloneDictionary<string>(_StringValues);
            clone._DataValues = CloneDictionary<Data>(_DataValues);
            return clone;
        }

        private Dictionary<string, T> CloneDictionary<T>(Dictionary<string, T> src) {
            if (src == null) return null;
            Dictionary<string, T> clone = null;
            foreach (var kv in src) {
                if (!IsTempKey(kv.Key)) {
                    if (clone == null) clone = new Dictionary<string, T>();
                    clone[kv.Key] = kv.Value;
                }
            }
            return clone;
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

        public int Count {
            get {
                return _ValueTypes.Count;
            }
        }

        public int DeepCount {
            get {
                int result = _ValueTypes.Count;
                if (_DataValues != null) {
                    foreach (var val in _DataValues.Values) {
                        if (val != null) {
                            result = result + val.DeepCount;
                        }
                    }
                }
                return result;
            }
        }

        public IEnumerable<string> Keys {
            get {
                return _ValueTypes.Keys;
            }
        }

        public bool HasValue(string key) {
            return _ValueTypes.ContainsKey(key);
        }

        public DataType GetValueType(string key) {
            DataType type;
            if (_ValueTypes.TryGetValue(key, out type)) {
                return type;
            }
            return DataType.Invalid;
        }

        public object GetValue(string key) {
            DataType valueType = GetValueType(key);
            if (valueType == DataType.Invalid) return null;

            switch (valueType) {
                case DataType.Bool:
                    return GetBool(key);
                case DataType.Int:
                    return GetInt(key);
                case DataType.Long:
                    return GetLong(key);
                case DataType.Float:
                    return GetFloat(key);
                case DataType.Double:
                    return GetDouble(key);
                case DataType.String:
                    return GetString(key);
                case DataType.Data:
                    return GetData(key);
            }
            return null;
        }

        public bool TryGetValue(string key, out object val) {
            if (HasValue(key)) {
                val = GetValue(key);
                return true;
            }
            val = null;
            return false;
        }

        //SILP: DATA_TYPE(Bool, bool)
        public bool IsBool(string key) {                                         //__SILP__
            DataType type = GetValueType(key);                                   //__SILP__
            return type == DataType.Bool;                                        //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
        public bool GetBool(string key) {                                        //__SILP__
            if (_BoolValues != null && IsBool(key)) {                            //__SILP__
                bool result;                                                     //__SILP__
                if (_BoolValues.TryGetValue(key, out result)) {                  //__SILP__
                    return result;                                               //__SILP__
                }                                                                //__SILP__
            }                                                                    //__SILP__
            return default(bool);                                                //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
        public bool GetBool(string key, bool defaultValue) {                     //__SILP__
            if (_BoolValues != null && IsBool(key)) {                            //__SILP__
                bool result;                                                     //__SILP__
                if (_BoolValues.TryGetValue(key, out result)) {                  //__SILP__
                    return result;                                               //__SILP__
                }                                                                //__SILP__
            }                                                                    //__SILP__
            return defaultValue;                                                 //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
        public bool SetBool(string key, bool val) {                              //__SILP__
            bool isTempKey = IsTempKey(key);                                     //__SILP__
            if (Sealed && !isTempKey) {                                          //__SILP__
                Log.Error("Already Sealed: {0} -> {1}", key, val);               //__SILP__
                return false;                                                    //__SILP__
            }                                                                    //__SILP__
            if (isTempKey || !_ValueTypes.ContainsKey(key)) {                    //__SILP__
                _ValueTypes[key] = DataType.Bool;                                //__SILP__
                if (_BoolValues == null) {                                       //__SILP__
                    _BoolValues = new Dictionary<string, bool>();                //__SILP__
                }                                                                //__SILP__
                _BoolValues[key] = val;                                          //__SILP__
                return true;                                                     //__SILP__
            }                                                                    //__SILP__
            Log.Error("Key Exist: {0} {1} -> {2}", key, _BoolValues[key], val);  //__SILP__
            return false;                                                        //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
        public void ForEachBool(Action<int, bool> callback) {                    //__SILP__
            for (int i = 0; i < Count; i++) {                                    //__SILP__
                string key = i.ToString();                                       //__SILP__
                callback(i, GetBool(key));                                       //__SILP__
            }                                                                    //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
        public bool UntilTrueBool(Func<int, bool, bool> callback) {              //__SILP__
            for (int i = 0; i < Count; i++) {                                    //__SILP__
                string key = i.ToString();                                       //__SILP__
                if (callback(i, GetBool(key))) {                                 //__SILP__
                    return true;                                                 //__SILP__
                }                                                                //__SILP__
            }                                                                    //__SILP__
            return false;                                                        //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
        public bool UntilFalseBool(Func<int, bool, bool> callback) {             //__SILP__
            for (int i = 0; i < Count; i++) {                                    //__SILP__
                string key = i.ToString();                                       //__SILP__
                if (!callback(i, GetBool(key))) {                                //__SILP__
                    return false;                                                //__SILP__
                }                                                                //__SILP__
            }                                                                    //__SILP__
            return true;                                                         //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
                                                                                 //__SILP__
        //SILP: DATA_TYPE(Int, int)
        public bool IsInt(string key) {                                         //__SILP__
            DataType type = GetValueType(key);                                  //__SILP__
            return type == DataType.Int;                                        //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        public int GetInt(string key) {                                         //__SILP__
            if (_IntValues != null && IsInt(key)) {                             //__SILP__
                int result;                                                     //__SILP__
                if (_IntValues.TryGetValue(key, out result)) {                  //__SILP__
                    return result;                                              //__SILP__
                }                                                               //__SILP__
            }                                                                   //__SILP__
            return default(int);                                                //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        public int GetInt(string key, int defaultValue) {                       //__SILP__
            if (_IntValues != null && IsInt(key)) {                             //__SILP__
                int result;                                                     //__SILP__
                if (_IntValues.TryGetValue(key, out result)) {                  //__SILP__
                    return result;                                              //__SILP__
                }                                                               //__SILP__
            }                                                                   //__SILP__
            return defaultValue;                                                //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        public bool SetInt(string key, int val) {                               //__SILP__
            bool isTempKey = IsTempKey(key);                                    //__SILP__
            if (Sealed && !isTempKey) {                                         //__SILP__
                Log.Error("Already Sealed: {0} -> {1}", key, val);              //__SILP__
                return false;                                                   //__SILP__
            }                                                                   //__SILP__
            if (isTempKey || !_ValueTypes.ContainsKey(key)) {                   //__SILP__
                _ValueTypes[key] = DataType.Int;                                //__SILP__
                if (_IntValues == null) {                                       //__SILP__
                    _IntValues = new Dictionary<string, int>();                 //__SILP__
                }                                                               //__SILP__
                _IntValues[key] = val;                                          //__SILP__
                return true;                                                    //__SILP__
            }                                                                   //__SILP__
            Log.Error("Key Exist: {0} {1} -> {2}", key, _IntValues[key], val);  //__SILP__
            return false;                                                       //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        public void ForEachInt(Action<int, int> callback) {                     //__SILP__
            for (int i = 0; i < Count; i++) {                                   //__SILP__
                string key = i.ToString();                                      //__SILP__
                callback(i, GetInt(key));                                       //__SILP__
            }                                                                   //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        public bool UntilTrueInt(Func<int, int, bool> callback) {               //__SILP__
            for (int i = 0; i < Count; i++) {                                   //__SILP__
                string key = i.ToString();                                      //__SILP__
                if (callback(i, GetInt(key))) {                                 //__SILP__
                    return true;                                                //__SILP__
                }                                                               //__SILP__
            }                                                                   //__SILP__
            return false;                                                       //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        public bool UntilFalseInt(Func<int, int, bool> callback) {              //__SILP__
            for (int i = 0; i < Count; i++) {                                   //__SILP__
                string key = i.ToString();                                      //__SILP__
                if (!callback(i, GetInt(key))) {                                //__SILP__
                    return false;                                               //__SILP__
                }                                                               //__SILP__
            }                                                                   //__SILP__
            return true;                                                        //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
                                                                                //__SILP__
        //SILP: DATA_TYPE(Long, long)
        public bool IsLong(string key) {                                         //__SILP__
            DataType type = GetValueType(key);                                   //__SILP__
            return type == DataType.Long;                                        //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
        public long GetLong(string key) {                                        //__SILP__
            if (_LongValues != null && IsLong(key)) {                            //__SILP__
                long result;                                                     //__SILP__
                if (_LongValues.TryGetValue(key, out result)) {                  //__SILP__
                    return result;                                               //__SILP__
                }                                                                //__SILP__
            }                                                                    //__SILP__
            return default(long);                                                //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
        public long GetLong(string key, long defaultValue) {                     //__SILP__
            if (_LongValues != null && IsLong(key)) {                            //__SILP__
                long result;                                                     //__SILP__
                if (_LongValues.TryGetValue(key, out result)) {                  //__SILP__
                    return result;                                               //__SILP__
                }                                                                //__SILP__
            }                                                                    //__SILP__
            return defaultValue;                                                 //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
        public bool SetLong(string key, long val) {                              //__SILP__
            bool isTempKey = IsTempKey(key);                                     //__SILP__
            if (Sealed && !isTempKey) {                                          //__SILP__
                Log.Error("Already Sealed: {0} -> {1}", key, val);               //__SILP__
                return false;                                                    //__SILP__
            }                                                                    //__SILP__
            if (isTempKey || !_ValueTypes.ContainsKey(key)) {                    //__SILP__
                _ValueTypes[key] = DataType.Long;                                //__SILP__
                if (_LongValues == null) {                                       //__SILP__
                    _LongValues = new Dictionary<string, long>();                //__SILP__
                }                                                                //__SILP__
                _LongValues[key] = val;                                          //__SILP__
                return true;                                                     //__SILP__
            }                                                                    //__SILP__
            Log.Error("Key Exist: {0} {1} -> {2}", key, _LongValues[key], val);  //__SILP__
            return false;                                                        //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
        public void ForEachLong(Action<int, long> callback) {                    //__SILP__
            for (int i = 0; i < Count; i++) {                                    //__SILP__
                string key = i.ToString();                                       //__SILP__
                callback(i, GetLong(key));                                       //__SILP__
            }                                                                    //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
        public bool UntilTrueLong(Func<int, long, bool> callback) {              //__SILP__
            for (int i = 0; i < Count; i++) {                                    //__SILP__
                string key = i.ToString();                                       //__SILP__
                if (callback(i, GetLong(key))) {                                 //__SILP__
                    return true;                                                 //__SILP__
                }                                                                //__SILP__
            }                                                                    //__SILP__
            return false;                                                        //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
        public bool UntilFalseLong(Func<int, long, bool> callback) {             //__SILP__
            for (int i = 0; i < Count; i++) {                                    //__SILP__
                string key = i.ToString();                                       //__SILP__
                if (!callback(i, GetLong(key))) {                                //__SILP__
                    return false;                                                //__SILP__
                }                                                                //__SILP__
            }                                                                    //__SILP__
            return true;                                                         //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
                                                                                 //__SILP__
        //SILP: DATA_TYPE(Float, float)
        public bool IsFloat(string key) {                                         //__SILP__
            DataType type = GetValueType(key);                                    //__SILP__
            return type == DataType.Float;                                        //__SILP__
        }                                                                         //__SILP__
                                                                                  //__SILP__
        public float GetFloat(string key) {                                       //__SILP__
            if (_FloatValues != null && IsFloat(key)) {                           //__SILP__
                float result;                                                     //__SILP__
                if (_FloatValues.TryGetValue(key, out result)) {                  //__SILP__
                    return result;                                                //__SILP__
                }                                                                 //__SILP__
            }                                                                     //__SILP__
            return default(float);                                                //__SILP__
        }                                                                         //__SILP__
                                                                                  //__SILP__
        public float GetFloat(string key, float defaultValue) {                   //__SILP__
            if (_FloatValues != null && IsFloat(key)) {                           //__SILP__
                float result;                                                     //__SILP__
                if (_FloatValues.TryGetValue(key, out result)) {                  //__SILP__
                    return result;                                                //__SILP__
                }                                                                 //__SILP__
            }                                                                     //__SILP__
            return defaultValue;                                                  //__SILP__
        }                                                                         //__SILP__
                                                                                  //__SILP__
        public bool SetFloat(string key, float val) {                             //__SILP__
            bool isTempKey = IsTempKey(key);                                      //__SILP__
            if (Sealed && !isTempKey) {                                           //__SILP__
                Log.Error("Already Sealed: {0} -> {1}", key, val);                //__SILP__
                return false;                                                     //__SILP__
            }                                                                     //__SILP__
            if (isTempKey || !_ValueTypes.ContainsKey(key)) {                     //__SILP__
                _ValueTypes[key] = DataType.Float;                                //__SILP__
                if (_FloatValues == null) {                                       //__SILP__
                    _FloatValues = new Dictionary<string, float>();               //__SILP__
                }                                                                 //__SILP__
                _FloatValues[key] = val;                                          //__SILP__
                return true;                                                      //__SILP__
            }                                                                     //__SILP__
            Log.Error("Key Exist: {0} {1} -> {2}", key, _FloatValues[key], val);  //__SILP__
            return false;                                                         //__SILP__
        }                                                                         //__SILP__
                                                                                  //__SILP__
        public void ForEachFloat(Action<int, float> callback) {                   //__SILP__
            for (int i = 0; i < Count; i++) {                                     //__SILP__
                string key = i.ToString();                                        //__SILP__
                callback(i, GetFloat(key));                                       //__SILP__
            }                                                                     //__SILP__
        }                                                                         //__SILP__
                                                                                  //__SILP__
        public bool UntilTrueFloat(Func<int, float, bool> callback) {             //__SILP__
            for (int i = 0; i < Count; i++) {                                     //__SILP__
                string key = i.ToString();                                        //__SILP__
                if (callback(i, GetFloat(key))) {                                 //__SILP__
                    return true;                                                  //__SILP__
                }                                                                 //__SILP__
            }                                                                     //__SILP__
            return false;                                                         //__SILP__
        }                                                                         //__SILP__
                                                                                  //__SILP__
        public bool UntilFalseFloat(Func<int, float, bool> callback) {            //__SILP__
            for (int i = 0; i < Count; i++) {                                     //__SILP__
                string key = i.ToString();                                        //__SILP__
                if (!callback(i, GetFloat(key))) {                                //__SILP__
                    return false;                                                 //__SILP__
                }                                                                 //__SILP__
            }                                                                     //__SILP__
            return true;                                                          //__SILP__
        }                                                                         //__SILP__
                                                                                  //__SILP__
                                                                                  //__SILP__
        //SILP: DATA_TYPE(Double, double)
        public bool IsDouble(string key) {                                         //__SILP__
            DataType type = GetValueType(key);                                     //__SILP__
            return type == DataType.Double;                                        //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public double GetDouble(string key) {                                      //__SILP__
            if (_DoubleValues != null && IsDouble(key)) {                          //__SILP__
                double result;                                                     //__SILP__
                if (_DoubleValues.TryGetValue(key, out result)) {                  //__SILP__
                    return result;                                                 //__SILP__
                }                                                                  //__SILP__
            }                                                                      //__SILP__
            return default(double);                                                //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public double GetDouble(string key, double defaultValue) {                 //__SILP__
            if (_DoubleValues != null && IsDouble(key)) {                          //__SILP__
                double result;                                                     //__SILP__
                if (_DoubleValues.TryGetValue(key, out result)) {                  //__SILP__
                    return result;                                                 //__SILP__
                }                                                                  //__SILP__
            }                                                                      //__SILP__
            return defaultValue;                                                   //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public bool SetDouble(string key, double val) {                            //__SILP__
            bool isTempKey = IsTempKey(key);                                       //__SILP__
            if (Sealed && !isTempKey) {                                            //__SILP__
                Log.Error("Already Sealed: {0} -> {1}", key, val);                 //__SILP__
                return false;                                                      //__SILP__
            }                                                                      //__SILP__
            if (isTempKey || !_ValueTypes.ContainsKey(key)) {                      //__SILP__
                _ValueTypes[key] = DataType.Double;                                //__SILP__
                if (_DoubleValues == null) {                                       //__SILP__
                    _DoubleValues = new Dictionary<string, double>();              //__SILP__
                }                                                                  //__SILP__
                _DoubleValues[key] = val;                                          //__SILP__
                return true;                                                       //__SILP__
            }                                                                      //__SILP__
            Log.Error("Key Exist: {0} {1} -> {2}", key, _DoubleValues[key], val);  //__SILP__
            return false;                                                          //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public void ForEachDouble(Action<int, double> callback) {                  //__SILP__
            for (int i = 0; i < Count; i++) {                                      //__SILP__
                string key = i.ToString();                                         //__SILP__
                callback(i, GetDouble(key));                                       //__SILP__
            }                                                                      //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public bool UntilTrueDouble(Func<int, double, bool> callback) {            //__SILP__
            for (int i = 0; i < Count; i++) {                                      //__SILP__
                string key = i.ToString();                                         //__SILP__
                if (callback(i, GetDouble(key))) {                                 //__SILP__
                    return true;                                                   //__SILP__
                }                                                                  //__SILP__
            }                                                                      //__SILP__
            return false;                                                          //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public bool UntilFalseDouble(Func<int, double, bool> callback) {           //__SILP__
            for (int i = 0; i < Count; i++) {                                      //__SILP__
                string key = i.ToString();                                         //__SILP__
                if (!callback(i, GetDouble(key))) {                                //__SILP__
                    return false;                                                  //__SILP__
                }                                                                  //__SILP__
            }                                                                      //__SILP__
            return true;                                                           //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
                                                                                   //__SILP__
        //SILP: DATA_TYPE(String, string)
        public bool IsString(string key) {                                         //__SILP__
            DataType type = GetValueType(key);                                     //__SILP__
            return type == DataType.String;                                        //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public string GetString(string key) {                                      //__SILP__
            if (_StringValues != null && IsString(key)) {                          //__SILP__
                string result;                                                     //__SILP__
                if (_StringValues.TryGetValue(key, out result)) {                  //__SILP__
                    return result;                                                 //__SILP__
                }                                                                  //__SILP__
            }                                                                      //__SILP__
            return default(string);                                                //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public string GetString(string key, string defaultValue) {                 //__SILP__
            if (_StringValues != null && IsString(key)) {                          //__SILP__
                string result;                                                     //__SILP__
                if (_StringValues.TryGetValue(key, out result)) {                  //__SILP__
                    return result;                                                 //__SILP__
                }                                                                  //__SILP__
            }                                                                      //__SILP__
            return defaultValue;                                                   //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public bool SetString(string key, string val) {                            //__SILP__
            bool isTempKey = IsTempKey(key);                                       //__SILP__
            if (Sealed && !isTempKey) {                                            //__SILP__
                Log.Error("Already Sealed: {0} -> {1}", key, val);                 //__SILP__
                return false;                                                      //__SILP__
            }                                                                      //__SILP__
            if (isTempKey || !_ValueTypes.ContainsKey(key)) {                      //__SILP__
                _ValueTypes[key] = DataType.String;                                //__SILP__
                if (_StringValues == null) {                                       //__SILP__
                    _StringValues = new Dictionary<string, string>();              //__SILP__
                }                                                                  //__SILP__
                _StringValues[key] = val;                                          //__SILP__
                return true;                                                       //__SILP__
            }                                                                      //__SILP__
            Log.Error("Key Exist: {0} {1} -> {2}", key, _StringValues[key], val);  //__SILP__
            return false;                                                          //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public void ForEachString(Action<int, string> callback) {                  //__SILP__
            for (int i = 0; i < Count; i++) {                                      //__SILP__
                string key = i.ToString();                                         //__SILP__
                callback(i, GetString(key));                                       //__SILP__
            }                                                                      //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public bool UntilTrueString(Func<int, string, bool> callback) {            //__SILP__
            for (int i = 0; i < Count; i++) {                                      //__SILP__
                string key = i.ToString();                                         //__SILP__
                if (callback(i, GetString(key))) {                                 //__SILP__
                    return true;                                                   //__SILP__
                }                                                                  //__SILP__
            }                                                                      //__SILP__
            return false;                                                          //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public bool UntilFalseString(Func<int, string, bool> callback) {           //__SILP__
            for (int i = 0; i < Count; i++) {                                      //__SILP__
                string key = i.ToString();                                         //__SILP__
                if (!callback(i, GetString(key))) {                                //__SILP__
                    return false;                                                  //__SILP__
                }                                                                  //__SILP__
            }                                                                      //__SILP__
            return true;                                                           //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
                                                                                   //__SILP__
        //SILP: DATA_TYPE(Data, Data)
        public bool IsData(string key) {                                         //__SILP__
            DataType type = GetValueType(key);                                   //__SILP__
            return type == DataType.Data;                                        //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
        public Data GetData(string key) {                                        //__SILP__
            if (_DataValues != null && IsData(key)) {                            //__SILP__
                Data result;                                                     //__SILP__
                if (_DataValues.TryGetValue(key, out result)) {                  //__SILP__
                    return result;                                               //__SILP__
                }                                                                //__SILP__
            }                                                                    //__SILP__
            return default(Data);                                                //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
        public Data GetData(string key, Data defaultValue) {                     //__SILP__
            if (_DataValues != null && IsData(key)) {                            //__SILP__
                Data result;                                                     //__SILP__
                if (_DataValues.TryGetValue(key, out result)) {                  //__SILP__
                    return result;                                               //__SILP__
                }                                                                //__SILP__
            }                                                                    //__SILP__
            return defaultValue;                                                 //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
        public bool SetData(string key, Data val) {                              //__SILP__
            bool isTempKey = IsTempKey(key);                                     //__SILP__
            if (Sealed && !isTempKey) {                                          //__SILP__
                Log.Error("Already Sealed: {0} -> {1}", key, val);               //__SILP__
                return false;                                                    //__SILP__
            }                                                                    //__SILP__
            if (isTempKey || !_ValueTypes.ContainsKey(key)) {                    //__SILP__
                _ValueTypes[key] = DataType.Data;                                //__SILP__
                if (_DataValues == null) {                                       //__SILP__
                    _DataValues = new Dictionary<string, Data>();                //__SILP__
                }                                                                //__SILP__
                _DataValues[key] = val;                                          //__SILP__
                return true;                                                     //__SILP__
            }                                                                    //__SILP__
            Log.Error("Key Exist: {0} {1} -> {2}", key, _DataValues[key], val);  //__SILP__
            return false;                                                        //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
        public void ForEachData(Action<int, Data> callback) {                    //__SILP__
            for (int i = 0; i < Count; i++) {                                    //__SILP__
                string key = i.ToString();                                       //__SILP__
                callback(i, GetData(key));                                       //__SILP__
            }                                                                    //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
        public bool UntilTrueData(Func<int, Data, bool> callback) {              //__SILP__
            for (int i = 0; i < Count; i++) {                                    //__SILP__
                string key = i.ToString();                                       //__SILP__
                if (callback(i, GetData(key))) {                                 //__SILP__
                    return true;                                                 //__SILP__
                }                                                                //__SILP__
            }                                                                    //__SILP__
            return false;                                                        //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
        public bool UntilFalseData(Func<int, Data, bool> callback) {             //__SILP__
            for (int i = 0; i < Count; i++) {                                    //__SILP__
                string key = i.ToString();                                       //__SILP__
                if (!callback(i, GetData(key))) {                                //__SILP__
                    return false;                                                //__SILP__
                }                                                                //__SILP__
            }                                                                    //__SILP__
            return true;                                                         //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
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
