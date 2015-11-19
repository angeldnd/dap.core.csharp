using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public interface DataChecker {
        bool IsValid(Data data);
    }

    public enum DataType : byte {Invalid = 0, Bool, Int, Long, Float, Double, String, Data};

    public sealed class Data {
        public const string VarPrefix = "$";
        public static string GetVarKey(string key) {
            return VarPrefix + key;
        }

        public static string ToString(Data data) {
            if (data == null) {
                return "null";
            }
            return data.ToString();
        }

        public static string ToFullString(Data data) {
            if (data == null) {
                return "null";
            }
            return data.ToFullString(true);
        }

        public static Data Clone(Data data) {
            if (data == null) return new Data();

            return data.Clone();
        }

        private bool _Sealed = false;
        public bool Sealed {
            get { return _Sealed; }
        }
        public void Seal() {
            _Sealed = true;
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
            if (DeepCount <= 12) {
                return ToFullString(true);
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
            Dictionary<string, T> clone = new Dictionary<string, T>();
            foreach (var kv in src) {
                clone[kv.Key] = kv.Value;
            }
            return clone;
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

        public DataType GetValueType(string key) {
            DataType type;
            if (_ValueTypes.TryGetValue(key, out type)) {
                return type;
            }
            return DataType.Invalid;
        }

        public Object GetValue(string key) {
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

        public string ToFullString(bool oneLine) {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            AppendData(builder, oneLine, "", this);
            return builder.ToString();
        }

        private void AppendLine(StringBuilder builder, bool oneLine, string prefix, string line) {
            builder.Append(prefix);
            builder.Append(line);
            if (!oneLine) {
                builder.AppendLine();
            }
        }

        private void AppendData(StringBuilder builder, bool oneLine, string prefix, Data data) {
            AppendLine(builder, oneLine, prefix, "{");
            if (data != null) {
                string innerPrefix = prefix;
                if (!oneLine) {
                    innerPrefix = prefix + "\t";
                }
                int count = data.Count;
                int index = 0;
                foreach (string key in data.Keys) {
                    AppendLine(builder, oneLine, innerPrefix, string.Format("{0}:", key));
                    AppendValue(builder, oneLine, innerPrefix, data, key);
                    index++;
                    if (index < count) {
                        AppendLine(builder, oneLine, innerPrefix, ",");
                    }
                }
            }
            AppendLine(builder, oneLine, prefix, "}");
        }

        public void AppendValue(StringBuilder builder, bool oneLine, string prefix, Data data, string key) {
            DataType valueType = data.GetValueType(key);
            switch (valueType) {
                case DataType.Bool:
                    AppendLine(builder, oneLine, prefix, string.Format("{0}", data.GetBool(key)));
                    break;
                case DataType.Int:
                    AppendLine(builder, oneLine, prefix, string.Format("{0}i", data.GetInt(key)));
                    break;
                case DataType.Long:
                    AppendLine(builder, oneLine, prefix, string.Format("{0}l", data.GetLong(key)));
                    break;
                case DataType.Float:
                    AppendLine(builder, oneLine, prefix, string.Format("{0}f", data.GetFloat(key)));
                    break;
                case DataType.Double:
                    AppendLine(builder, oneLine, prefix, string.Format("{0}d", data.GetDouble(key)));
                    break;
                case DataType.String:
                    AppendLine(builder, oneLine, prefix, string.Format("\"{0}\"", QuoteString(data.GetString(key))));
                    break;
                case DataType.Data:
                    AppendData(builder, oneLine, prefix, data.GetData(key));
                    break;
            }
        }

        public string QuoteString(string str) {
            //TODO:
            return str;
        }

        //SILP: DATA_TYPE(Bool, bool)
        public bool IsBool(string key) {                              //__SILP__
            DataType type = GetValueType(key);                        //__SILP__
            return type == DataType.Bool;                             //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool GetBool(string key) {                             //__SILP__
            if (_BoolValues != null && IsBool(key)) {                 //__SILP__
                bool result;                                          //__SILP__
                if (_BoolValues.TryGetValue(key, out result)) {       //__SILP__
                    return result;                                    //__SILP__
                }                                                     //__SILP__
            }                                                         //__SILP__
            return default(bool);                                     //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool GetBool(string key, bool defaultValue) {          //__SILP__
            if (_BoolValues != null && IsBool(key)) {                 //__SILP__
                bool result;                                          //__SILP__
                if (_BoolValues.TryGetValue(key, out result)) {       //__SILP__
                    return result;                                    //__SILP__
                }                                                     //__SILP__
            }                                                         //__SILP__
            return defaultValue;                                      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool SetBool(string key, bool val) {                   //__SILP__
            if (_Sealed && !key.StartsWith(VarPrefix)) {              //__SILP__
                return false;                                         //__SILP__
            }                                                         //__SILP__
            if (!_ValueTypes.ContainsKey(key)) {                      //__SILP__
                _ValueTypes[key] = DataType.Bool;                     //__SILP__
                if (_BoolValues == null) {                            //__SILP__
                    _BoolValues = new Dictionary<string, bool>();     //__SILP__
                }                                                     //__SILP__
                _BoolValues[key] = val;                               //__SILP__
                return true;                                          //__SILP__
            }                                                         //__SILP__
            return false;                                             //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        //SILP: DATA_TYPE(Int, int)
        public bool IsInt(string key) {                               //__SILP__
            DataType type = GetValueType(key);                        //__SILP__
            return type == DataType.Int;                              //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public int GetInt(string key) {                               //__SILP__
            if (_IntValues != null && IsInt(key)) {                   //__SILP__
                int result;                                           //__SILP__
                if (_IntValues.TryGetValue(key, out result)) {        //__SILP__
                    return result;                                    //__SILP__
                }                                                     //__SILP__
            }                                                         //__SILP__
            return default(int);                                      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public int GetInt(string key, int defaultValue) {             //__SILP__
            if (_IntValues != null && IsInt(key)) {                   //__SILP__
                int result;                                           //__SILP__
                if (_IntValues.TryGetValue(key, out result)) {        //__SILP__
                    return result;                                    //__SILP__
                }                                                     //__SILP__
            }                                                         //__SILP__
            return defaultValue;                                      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool SetInt(string key, int val) {                     //__SILP__
            if (_Sealed && !key.StartsWith(VarPrefix)) {              //__SILP__
                return false;                                         //__SILP__
            }                                                         //__SILP__
            if (!_ValueTypes.ContainsKey(key)) {                      //__SILP__
                _ValueTypes[key] = DataType.Int;                      //__SILP__
                if (_IntValues == null) {                             //__SILP__
                    _IntValues = new Dictionary<string, int>();       //__SILP__
                }                                                     //__SILP__
                _IntValues[key] = val;                                //__SILP__
                return true;                                          //__SILP__
            }                                                         //__SILP__
            return false;                                             //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        //SILP: DATA_TYPE(Long, long)
        public bool IsLong(string key) {                              //__SILP__
            DataType type = GetValueType(key);                        //__SILP__
            return type == DataType.Long;                             //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public long GetLong(string key) {                             //__SILP__
            if (_LongValues != null && IsLong(key)) {                 //__SILP__
                long result;                                          //__SILP__
                if (_LongValues.TryGetValue(key, out result)) {       //__SILP__
                    return result;                                    //__SILP__
                }                                                     //__SILP__
            }                                                         //__SILP__
            return default(long);                                     //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public long GetLong(string key, long defaultValue) {          //__SILP__
            if (_LongValues != null && IsLong(key)) {                 //__SILP__
                long result;                                          //__SILP__
                if (_LongValues.TryGetValue(key, out result)) {       //__SILP__
                    return result;                                    //__SILP__
                }                                                     //__SILP__
            }                                                         //__SILP__
            return defaultValue;                                      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool SetLong(string key, long val) {                   //__SILP__
            if (_Sealed && !key.StartsWith(VarPrefix)) {              //__SILP__
                return false;                                         //__SILP__
            }                                                         //__SILP__
            if (!_ValueTypes.ContainsKey(key)) {                      //__SILP__
                _ValueTypes[key] = DataType.Long;                     //__SILP__
                if (_LongValues == null) {                            //__SILP__
                    _LongValues = new Dictionary<string, long>();     //__SILP__
                }                                                     //__SILP__
                _LongValues[key] = val;                               //__SILP__
                return true;                                          //__SILP__
            }                                                         //__SILP__
            return false;                                             //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        //SILP: DATA_TYPE(Float, float)
        public bool IsFloat(string key) {                             //__SILP__
            DataType type = GetValueType(key);                        //__SILP__
            return type == DataType.Float;                            //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public float GetFloat(string key) {                           //__SILP__
            if (_FloatValues != null && IsFloat(key)) {               //__SILP__
                float result;                                         //__SILP__
                if (_FloatValues.TryGetValue(key, out result)) {      //__SILP__
                    return result;                                    //__SILP__
                }                                                     //__SILP__
            }                                                         //__SILP__
            return default(float);                                    //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public float GetFloat(string key, float defaultValue) {       //__SILP__
            if (_FloatValues != null && IsFloat(key)) {               //__SILP__
                float result;                                         //__SILP__
                if (_FloatValues.TryGetValue(key, out result)) {      //__SILP__
                    return result;                                    //__SILP__
                }                                                     //__SILP__
            }                                                         //__SILP__
            return defaultValue;                                      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool SetFloat(string key, float val) {                 //__SILP__
            if (_Sealed && !key.StartsWith(VarPrefix)) {              //__SILP__
                return false;                                         //__SILP__
            }                                                         //__SILP__
            if (!_ValueTypes.ContainsKey(key)) {                      //__SILP__
                _ValueTypes[key] = DataType.Float;                    //__SILP__
                if (_FloatValues == null) {                           //__SILP__
                    _FloatValues = new Dictionary<string, float>();   //__SILP__
                }                                                     //__SILP__
                _FloatValues[key] = val;                              //__SILP__
                return true;                                          //__SILP__
            }                                                         //__SILP__
            return false;                                             //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        //SILP: DATA_TYPE(Double, double)
        public bool IsDouble(string key) {                             //__SILP__
            DataType type = GetValueType(key);                         //__SILP__
            return type == DataType.Double;                            //__SILP__
        }                                                              //__SILP__
                                                                       //__SILP__
        public double GetDouble(string key) {                          //__SILP__
            if (_DoubleValues != null && IsDouble(key)) {              //__SILP__
                double result;                                         //__SILP__
                if (_DoubleValues.TryGetValue(key, out result)) {      //__SILP__
                    return result;                                     //__SILP__
                }                                                      //__SILP__
            }                                                          //__SILP__
            return default(double);                                    //__SILP__
        }                                                              //__SILP__
                                                                       //__SILP__
        public double GetDouble(string key, double defaultValue) {     //__SILP__
            if (_DoubleValues != null && IsDouble(key)) {              //__SILP__
                double result;                                         //__SILP__
                if (_DoubleValues.TryGetValue(key, out result)) {      //__SILP__
                    return result;                                     //__SILP__
                }                                                      //__SILP__
            }                                                          //__SILP__
            return defaultValue;                                       //__SILP__
        }                                                              //__SILP__
                                                                       //__SILP__
        public bool SetDouble(string key, double val) {                //__SILP__
            if (_Sealed && !key.StartsWith(VarPrefix)) {               //__SILP__
                return false;                                          //__SILP__
            }                                                          //__SILP__
            if (!_ValueTypes.ContainsKey(key)) {                       //__SILP__
                _ValueTypes[key] = DataType.Double;                    //__SILP__
                if (_DoubleValues == null) {                           //__SILP__
                    _DoubleValues = new Dictionary<string, double>();  //__SILP__
                }                                                      //__SILP__
                _DoubleValues[key] = val;                              //__SILP__
                return true;                                           //__SILP__
            }                                                          //__SILP__
            return false;                                              //__SILP__
        }                                                              //__SILP__
                                                                       //__SILP__
        //SILP: DATA_TYPE(String, string)
        public bool IsString(string key) {                             //__SILP__
            DataType type = GetValueType(key);                         //__SILP__
            return type == DataType.String;                            //__SILP__
        }                                                              //__SILP__
                                                                       //__SILP__
        public string GetString(string key) {                          //__SILP__
            if (_StringValues != null && IsString(key)) {              //__SILP__
                string result;                                         //__SILP__
                if (_StringValues.TryGetValue(key, out result)) {      //__SILP__
                    return result;                                     //__SILP__
                }                                                      //__SILP__
            }                                                          //__SILP__
            return default(string);                                    //__SILP__
        }                                                              //__SILP__
                                                                       //__SILP__
        public string GetString(string key, string defaultValue) {     //__SILP__
            if (_StringValues != null && IsString(key)) {              //__SILP__
                string result;                                         //__SILP__
                if (_StringValues.TryGetValue(key, out result)) {      //__SILP__
                    return result;                                     //__SILP__
                }                                                      //__SILP__
            }                                                          //__SILP__
            return defaultValue;                                       //__SILP__
        }                                                              //__SILP__
                                                                       //__SILP__
        public bool SetString(string key, string val) {                //__SILP__
            if (_Sealed && !key.StartsWith(VarPrefix)) {               //__SILP__
                return false;                                          //__SILP__
            }                                                          //__SILP__
            if (!_ValueTypes.ContainsKey(key)) {                       //__SILP__
                _ValueTypes[key] = DataType.String;                    //__SILP__
                if (_StringValues == null) {                           //__SILP__
                    _StringValues = new Dictionary<string, string>();  //__SILP__
                }                                                      //__SILP__
                _StringValues[key] = val;                              //__SILP__
                return true;                                           //__SILP__
            }                                                          //__SILP__
            return false;                                              //__SILP__
        }                                                              //__SILP__
                                                                       //__SILP__
        //SILP: DATA_TYPE(Data, Data)
        public bool IsData(string key) {                              //__SILP__
            DataType type = GetValueType(key);                        //__SILP__
            return type == DataType.Data;                             //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public Data GetData(string key) {                             //__SILP__
            if (_DataValues != null && IsData(key)) {                 //__SILP__
                Data result;                                          //__SILP__
                if (_DataValues.TryGetValue(key, out result)) {       //__SILP__
                    return result;                                    //__SILP__
                }                                                     //__SILP__
            }                                                         //__SILP__
            return default(Data);                                     //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public Data GetData(string key, Data defaultValue) {          //__SILP__
            if (_DataValues != null && IsData(key)) {                 //__SILP__
                Data result;                                          //__SILP__
                if (_DataValues.TryGetValue(key, out result)) {       //__SILP__
                    return result;                                    //__SILP__
                }                                                     //__SILP__
            }                                                         //__SILP__
            return defaultValue;                                      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool SetData(string key, Data val) {                   //__SILP__
            if (_Sealed && !key.StartsWith(VarPrefix)) {              //__SILP__
                return false;                                         //__SILP__
            }                                                         //__SILP__
            if (!_ValueTypes.ContainsKey(key)) {                      //__SILP__
                _ValueTypes[key] = DataType.Data;                     //__SILP__
                if (_DataValues == null) {                            //__SILP__
                    _DataValues = new Dictionary<string, Data>();     //__SILP__
                }                                                     //__SILP__
                _DataValues[key] = val;                               //__SILP__
                return true;                                          //__SILP__
            }                                                         //__SILP__
            return false;                                             //__SILP__
        }                                                             //__SILP__
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
