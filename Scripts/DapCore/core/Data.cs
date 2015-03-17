using System;
using System.Collections.Generic;

namespace angeldnd.dap {

    public interface DataChecker {
        bool IsValid(Data data);
    }

    public enum DataType {Invalid = 0, Bool, Int, Long, Float, Double, String, Data};

    public sealed class Data {

        private Dictionary<string, DataType> _ValueTypes = new Dictionary<string, DataType>();

        private Dictionary<string, bool> _BoolValues = null;
        private Dictionary<string, int> _IntValues = null;
        private Dictionary<string, long> _LongValues = null;
        private Dictionary<string, float> _FloatValues = null;
        private Dictionary<string, double> _DoubleValues = null;
        private Dictionary<string, string> _StringValues = null;
        private Dictionary<string, Data> _DataValues = null;

        public int Count {
            get {
                return _ValueTypes.Count;
            }
        }

        public IEnumerable<string> Keys {
            get {
                return _ValueTypes.Keys;
            }
        }

        public DataType GetType(string key) {
            DataType type;
            if (_ValueTypes.TryGetValue(key, out type)) {
                return type;
            }
            return DataType.Invalid;
        }


        //SILP: DATA_TYPE(Bool, bool)
        public bool IsBool(string key) {                              //__SILP__
            DataType type = GetType(key);                             //__SILP__
            return type == DataType.Bool;                             //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool GetBool(string key) {                             //__SILP__
            if (IsBool(key)) {                                        //__SILP__
                bool result;                                          //__SILP__
                if (_BoolValues.TryGetValue(key, out result)) {       //__SILP__
                    return result;                                    //__SILP__
                }                                                     //__SILP__
            }                                                         //__SILP__
            return default(bool);                                     //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool GetBool(string key, bool defaultValue) {          //__SILP__
            if (IsBool(key)) {                                        //__SILP__
                bool result;                                          //__SILP__
                if (_BoolValues.TryGetValue(key, out result)) {       //__SILP__
                    return result;                                    //__SILP__
                }                                                     //__SILP__
            }                                                         //__SILP__
            return defaultValue;                                      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool SetBool(string key, bool value) {                 //__SILP__
            if (!_ValueTypes.ContainsKey(key)) {                      //__SILP__
                _ValueTypes[key] = DataType.Bool;                     //__SILP__
                _BoolValues[key] = value;                             //__SILP__
                return true;                                          //__SILP__
            }                                                         //__SILP__
            return false;                                             //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        //SILP: DATA_TYPE(Int, int)
        public bool IsInt(string key) {                               //__SILP__
            DataType type = GetType(key);                             //__SILP__
            return type == DataType.Int;                              //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public int GetInt(string key) {                               //__SILP__
            if (IsInt(key)) {                                         //__SILP__
                int result;                                           //__SILP__
                if (_IntValues.TryGetValue(key, out result)) {        //__SILP__
                    return result;                                    //__SILP__
                }                                                     //__SILP__
            }                                                         //__SILP__
            return default(int);                                      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public int GetInt(string key, int defaultValue) {             //__SILP__
            if (IsInt(key)) {                                         //__SILP__
                int result;                                           //__SILP__
                if (_IntValues.TryGetValue(key, out result)) {        //__SILP__
                    return result;                                    //__SILP__
                }                                                     //__SILP__
            }                                                         //__SILP__
            return defaultValue;                                      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool SetInt(string key, int value) {                   //__SILP__
            if (!_ValueTypes.ContainsKey(key)) {                      //__SILP__
                _ValueTypes[key] = DataType.Int;                      //__SILP__
                _IntValues[key] = value;                              //__SILP__
                return true;                                          //__SILP__
            }                                                         //__SILP__
            return false;                                             //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        //SILP: DATA_TYPE(Long, long)
        public bool IsLong(string key) {                              //__SILP__
            DataType type = GetType(key);                             //__SILP__
            return type == DataType.Long;                             //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public long GetLong(string key) {                             //__SILP__
            if (IsLong(key)) {                                        //__SILP__
                long result;                                          //__SILP__
                if (_LongValues.TryGetValue(key, out result)) {       //__SILP__
                    return result;                                    //__SILP__
                }                                                     //__SILP__
            }                                                         //__SILP__
            return default(long);                                     //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public long GetLong(string key, long defaultValue) {          //__SILP__
            if (IsLong(key)) {                                        //__SILP__
                long result;                                          //__SILP__
                if (_LongValues.TryGetValue(key, out result)) {       //__SILP__
                    return result;                                    //__SILP__
                }                                                     //__SILP__
            }                                                         //__SILP__
            return defaultValue;                                      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool SetLong(string key, long value) {                 //__SILP__
            if (!_ValueTypes.ContainsKey(key)) {                      //__SILP__
                _ValueTypes[key] = DataType.Long;                     //__SILP__
                _LongValues[key] = value;                             //__SILP__
                return true;                                          //__SILP__
            }                                                         //__SILP__
            return false;                                             //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        //SILP: DATA_TYPE(Float, float)
        public bool IsFloat(string key) {                             //__SILP__
            DataType type = GetType(key);                             //__SILP__
            return type == DataType.Float;                            //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public float GetFloat(string key) {                           //__SILP__
            if (IsFloat(key)) {                                       //__SILP__
                float result;                                         //__SILP__
                if (_FloatValues.TryGetValue(key, out result)) {      //__SILP__
                    return result;                                    //__SILP__
                }                                                     //__SILP__
            }                                                         //__SILP__
            return default(float);                                    //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public float GetFloat(string key, float defaultValue) {       //__SILP__
            if (IsFloat(key)) {                                       //__SILP__
                float result;                                         //__SILP__
                if (_FloatValues.TryGetValue(key, out result)) {      //__SILP__
                    return result;                                    //__SILP__
                }                                                     //__SILP__
            }                                                         //__SILP__
            return defaultValue;                                      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool SetFloat(string key, float value) {               //__SILP__
            if (!_ValueTypes.ContainsKey(key)) {                      //__SILP__
                _ValueTypes[key] = DataType.Float;                    //__SILP__
                _FloatValues[key] = value;                            //__SILP__
                return true;                                          //__SILP__
            }                                                         //__SILP__
            return false;                                             //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        //SILP: DATA_TYPE(Double, double)
        public bool IsDouble(string key) {                            //__SILP__
            DataType type = GetType(key);                             //__SILP__
            return type == DataType.Double;                           //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public double GetDouble(string key) {                         //__SILP__
            if (IsDouble(key)) {                                      //__SILP__
                double result;                                        //__SILP__
                if (_DoubleValues.TryGetValue(key, out result)) {     //__SILP__
                    return result;                                    //__SILP__
                }                                                     //__SILP__
            }                                                         //__SILP__
            return default(double);                                   //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public double GetDouble(string key, double defaultValue) {    //__SILP__
            if (IsDouble(key)) {                                      //__SILP__
                double result;                                        //__SILP__
                if (_DoubleValues.TryGetValue(key, out result)) {     //__SILP__
                    return result;                                    //__SILP__
                }                                                     //__SILP__
            }                                                         //__SILP__
            return defaultValue;                                      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool SetDouble(string key, double value) {             //__SILP__
            if (!_ValueTypes.ContainsKey(key)) {                      //__SILP__
                _ValueTypes[key] = DataType.Double;                   //__SILP__
                _DoubleValues[key] = value;                           //__SILP__
                return true;                                          //__SILP__
            }                                                         //__SILP__
            return false;                                             //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        //SILP: DATA_TYPE(String, string)
        public bool IsString(string key) {                            //__SILP__
            DataType type = GetType(key);                             //__SILP__
            return type == DataType.String;                           //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public string GetString(string key) {                         //__SILP__
            if (IsString(key)) {                                      //__SILP__
                string result;                                        //__SILP__
                if (_StringValues.TryGetValue(key, out result)) {     //__SILP__
                    return result;                                    //__SILP__
                }                                                     //__SILP__
            }                                                         //__SILP__
            return default(string);                                   //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public string GetString(string key, string defaultValue) {    //__SILP__
            if (IsString(key)) {                                      //__SILP__
                string result;                                        //__SILP__
                if (_StringValues.TryGetValue(key, out result)) {     //__SILP__
                    return result;                                    //__SILP__
                }                                                     //__SILP__
            }                                                         //__SILP__
            return defaultValue;                                      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool SetString(string key, string value) {             //__SILP__
            if (!_ValueTypes.ContainsKey(key)) {                      //__SILP__
                _ValueTypes[key] = DataType.String;                   //__SILP__
                _StringValues[key] = value;                           //__SILP__
                return true;                                          //__SILP__
            }                                                         //__SILP__
            return false;                                             //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        //SILP: DATA_TYPE(Data, Data)
        public bool IsData(string key) {                              //__SILP__
            DataType type = GetType(key);                             //__SILP__
            return type == DataType.Data;                             //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public Data GetData(string key) {                             //__SILP__
            if (IsData(key)) {                                        //__SILP__
                Data result;                                          //__SILP__
                if (_DataValues.TryGetValue(key, out result)) {       //__SILP__
                    return result;                                    //__SILP__
                }                                                     //__SILP__
            }                                                         //__SILP__
            return default(Data);                                     //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public Data GetData(string key, Data defaultValue) {          //__SILP__
            if (IsData(key)) {                                        //__SILP__
                Data result;                                          //__SILP__
                if (_DataValues.TryGetValue(key, out result)) {       //__SILP__
                    return result;                                    //__SILP__
                }                                                     //__SILP__
            }                                                         //__SILP__
            return defaultValue;                                      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool SetData(string key, Data value) {                 //__SILP__
            if (!_ValueTypes.ContainsKey(key)) {                      //__SILP__
                _ValueTypes[key] = DataType.Data;                     //__SILP__
                _DataValues[key] = value;                             //__SILP__
                return true;                                          //__SILP__
            }                                                         //__SILP__
            return false;                                             //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
    }
}
