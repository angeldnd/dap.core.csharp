using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public sealed class RealData : Data {
        private Dictionary<string, DataType> _ValueTypes = new Dictionary<string, DataType>();

        private Dictionary<string, bool> _BoolValues = null;
        private Dictionary<string, int> _IntValues = null;
        private Dictionary<string, long> _LongValues = null;
        private Dictionary<string, float> _FloatValues = null;
        private Dictionary<string, double> _DoubleValues = null;
        private Dictionary<string, string> _StringValues = null;
        private Dictionary<string, Data> _DataValues = null;

        private string _CapacityTip = "";
        public string CapacityTip {
            get { return _CapacityTip; }
        }

        public override Data Clone() {
            RealData clone = new RealData();
            _CopyTo(clone);
            return clone;
        }

        public Data _CopyTo(RealData clone) {
            var en = _ValueTypes.GetEnumerator();
            while (en.MoveNext()) {
                var kv = en.Current;
                clone._ValueTypes[kv.Key] = kv.Value;
            }

            clone._BoolValues = CloneDictionary<bool>(_BoolValues);
            clone._IntValues = CloneDictionary<int>(_IntValues);
            clone._LongValues = CloneDictionary<long>(_LongValues);
            clone._FloatValues = CloneDictionary<float>(_FloatValues);
            clone._DoubleValues = CloneDictionary<double>(_DoubleValues);
            clone._StringValues = CloneDictionary<string>(_StringValues);
            clone._DataValues = CloneDictionary<Data>(_DataValues);
            return clone;
        }

        protected override void OnRecycle() {
            _CapacityTip = string.Format("({0} B:{1} I:{2} L:{3} F:{4} D:{5} S:{6} A:{7})",
                    _ValueTypes.Count,
                    _BoolValues == null ? 0 : _BoolValues.Count,
                    _IntValues == null ? 0 : _IntValues.Count,
                    _LongValues == null ? 0 : _LongValues.Count,
                    _FloatValues == null ? 0 : _FloatValues.Count,
                    _DoubleValues == null ? 0 : _DoubleValues.Count,
                    _StringValues == null ? 0 : _StringValues.Count,
                    _DataValues == null ? 0 : _DataValues.Count);
        }

        public override void Clear() {
            if (Sealed) {
                Log.Error("Already Sealed");
            }

            ClearDictionary(_ValueTypes);
            ClearDictionary(_BoolValues);
            ClearDictionary(_IntValues);
            ClearDictionary(_LongValues);
            ClearDictionary(_FloatValues);
            ClearDictionary(_DoubleValues);
            ClearDictionary(_StringValues);
            ClearDictionary(_DataValues);
        }

        private void ClearDictionary<T>(Dictionary<string, T> dict) {
            if (dict != null) {
                dict.Clear();
            }
        }

        private Dictionary<string, T> CloneDictionary<T>(Dictionary<string, T> src) {
            if (src == null) return null;
            Dictionary<string, T> clone = null;
            var en = src.GetEnumerator();
            while (en.MoveNext()) {
                var kv = en.Current;
                if (clone == null) clone = new Dictionary<string, T>();
                clone[kv.Key] = kv.Value;
            }
            return clone;
        }

        public override int Count {
            get {
                return _ValueTypes.Count;
            }
        }

        public override IEnumerable<string> Keys {
            get {
                return _ValueTypes.Keys;
            }
        }

        public override bool HasKey(string key) {
            return _ValueTypes.ContainsKey(key);
        }

        public override DataType GetValueType(string key) {
            DataType type;
            if (_ValueTypes.TryGetValue(key, out type)) {
                return type;
            }
            return DataType.Invalid;
        }

        public override object GetValue(string key) {
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

        public override bool TryGetValue(string key, out object val) {
            if (HasKey(key)) {
                val = GetValue(key);
                return true;
            }
            val = null;
            return false;
        }

        //SILP: REAL_DATA_TYPE(Bool, bool)
        public override bool IsBool(string key) {                                          //__SILP__
            DataType type = GetValueType(key);                                             //__SILP__
            return type == DataType.Bool;                                                  //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
        public override bool TryGetBool(string key, out bool val, bool isDebug = false) {  //__SILP__
            if (_BoolValues == null) {                                                     //__SILP__
                Log.ErrorOrDebug(isDebug, "Value Not Exist: {0}", key);                    //__SILP__
            } else if (!IsBool(key)) {                                                     //__SILP__
                Log.ErrorOrDebug(isDebug, "Value Is Not Bool: {0} -> {1} -> {2}",          //__SILP__
                    key, GetValueType(key), GetValue(key));                                //__SILP__
            } else {                                                                       //__SILP__
                bool _val;                                                                 //__SILP__
                if (_BoolValues.TryGetValue(key, out _val)) {                              //__SILP__
                    val = _val;                                                            //__SILP__
                    return true;                                                           //__SILP__
                } else {                                                                   //__SILP__
                    Log.Error("Value Not Found: {0}", key);                                //__SILP__
                }                                                                          //__SILP__
            }                                                                              //__SILP__
            val = default(bool);                                                           //__SILP__
            return false;                                                                  //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
        public override bool GetBool(string key) {                                         //__SILP__
            bool result;                                                                   //__SILP__
            TryGetBool(key, out result);                                                   //__SILP__
            return result;                                                                 //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
        public override bool GetBool(string key, bool defaultValue) {                      //__SILP__
            bool result;                                                                   //__SILP__
            if (TryGetBool(key, out result, true)) {                                       //__SILP__
                return result;                                                             //__SILP__
            }                                                                              //__SILP__
            return defaultValue;                                                           //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
        public override bool SetBool(string key, bool val) {                               //__SILP__
            bool isTempKey = IsTempKey(key);                                               //__SILP__
            if (Sealed && !isTempKey) {                                                    //__SILP__
                Log.Error("Already Sealed: {0} -> {1}", key, val);                         //__SILP__
                return false;                                                              //__SILP__
            }                                                                              //__SILP__
            if (isTempKey || !_ValueTypes.ContainsKey(key)) {                              //__SILP__
                _ValueTypes[key] = DataType.Bool;                                          //__SILP__
                if (_BoolValues == null) {                                                 //__SILP__
                    _BoolValues = new Dictionary<string, bool>();                          //__SILP__
                }                                                                          //__SILP__
                _BoolValues[key] = val;                                                    //__SILP__
                return true;                                                               //__SILP__
            }                                                                              //__SILP__
            Log.Error("Value Exist: {0} {1} -> {2}", key, GetValue(key), val);             //__SILP__
            return false;                                                                  //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
        public override void ForEachBool(Action<int, bool> callback) {                     //__SILP__
            for (int i = 0; i < Count; i++) {                                              //__SILP__
                string key = i.ToString();                                                 //__SILP__
                callback(i, GetBool(key));                                                 //__SILP__
            }                                                                              //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
        public override bool UntilTrueBool(Func<int, bool, bool> callback) {               //__SILP__
            for (int i = 0; i < Count; i++) {                                              //__SILP__
                string key = i.ToString();                                                 //__SILP__
                if (callback(i, GetBool(key))) {                                           //__SILP__
                    return true;                                                           //__SILP__
                }                                                                          //__SILP__
            }                                                                              //__SILP__
            return false;                                                                  //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
        public override bool UntilFalseBool(Func<int, bool, bool> callback) {              //__SILP__
            for (int i = 0; i < Count; i++) {                                              //__SILP__
                string key = i.ToString();                                                 //__SILP__
                if (!callback(i, GetBool(key))) {                                          //__SILP__
                    return false;                                                          //__SILP__
                }                                                                          //__SILP__
            }                                                                              //__SILP__
            return true;                                                                   //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
                                                                                           //__SILP__
        //SILP: REAL_DATA_TYPE(Int, int)
        public override bool IsInt(string key) {                                         //__SILP__
            DataType type = GetValueType(key);                                           //__SILP__
            return type == DataType.Int;                                                 //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        public override bool TryGetInt(string key, out int val, bool isDebug = false) {  //__SILP__
            if (_IntValues == null) {                                                    //__SILP__
                Log.ErrorOrDebug(isDebug, "Value Not Exist: {0}", key);                  //__SILP__
            } else if (!IsInt(key)) {                                                    //__SILP__
                Log.ErrorOrDebug(isDebug, "Value Is Not Int: {0} -> {1} -> {2}",         //__SILP__
                    key, GetValueType(key), GetValue(key));                              //__SILP__
            } else {                                                                     //__SILP__
                int _val;                                                                //__SILP__
                if (_IntValues.TryGetValue(key, out _val)) {                             //__SILP__
                    val = _val;                                                          //__SILP__
                    return true;                                                         //__SILP__
                } else {                                                                 //__SILP__
                    Log.Error("Value Not Found: {0}", key);                              //__SILP__
                }                                                                        //__SILP__
            }                                                                            //__SILP__
            val = default(int);                                                          //__SILP__
            return false;                                                                //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        public override int GetInt(string key) {                                         //__SILP__
            int result;                                                                  //__SILP__
            TryGetInt(key, out result);                                                  //__SILP__
            return result;                                                               //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        public override int GetInt(string key, int defaultValue) {                       //__SILP__
            int result;                                                                  //__SILP__
            if (TryGetInt(key, out result, true)) {                                      //__SILP__
                return result;                                                           //__SILP__
            }                                                                            //__SILP__
            return defaultValue;                                                         //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        public override bool SetInt(string key, int val) {                               //__SILP__
            bool isTempKey = IsTempKey(key);                                             //__SILP__
            if (Sealed && !isTempKey) {                                                  //__SILP__
                Log.Error("Already Sealed: {0} -> {1}", key, val);                       //__SILP__
                return false;                                                            //__SILP__
            }                                                                            //__SILP__
            if (isTempKey || !_ValueTypes.ContainsKey(key)) {                            //__SILP__
                _ValueTypes[key] = DataType.Int;                                         //__SILP__
                if (_IntValues == null) {                                                //__SILP__
                    _IntValues = new Dictionary<string, int>();                          //__SILP__
                }                                                                        //__SILP__
                _IntValues[key] = val;                                                   //__SILP__
                return true;                                                             //__SILP__
            }                                                                            //__SILP__
            Log.Error("Value Exist: {0} {1} -> {2}", key, GetValue(key), val);           //__SILP__
            return false;                                                                //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        public override void ForEachInt(Action<int, int> callback) {                     //__SILP__
            for (int i = 0; i < Count; i++) {                                            //__SILP__
                string key = i.ToString();                                               //__SILP__
                callback(i, GetInt(key));                                                //__SILP__
            }                                                                            //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        public override bool UntilTrueInt(Func<int, int, bool> callback) {               //__SILP__
            for (int i = 0; i < Count; i++) {                                            //__SILP__
                string key = i.ToString();                                               //__SILP__
                if (callback(i, GetInt(key))) {                                          //__SILP__
                    return true;                                                         //__SILP__
                }                                                                        //__SILP__
            }                                                                            //__SILP__
            return false;                                                                //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        public override bool UntilFalseInt(Func<int, int, bool> callback) {              //__SILP__
            for (int i = 0; i < Count; i++) {                                            //__SILP__
                string key = i.ToString();                                               //__SILP__
                if (!callback(i, GetInt(key))) {                                         //__SILP__
                    return false;                                                        //__SILP__
                }                                                                        //__SILP__
            }                                                                            //__SILP__
            return true;                                                                 //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
                                                                                         //__SILP__
        //SILP: REAL_DATA_TYPE(Long, long)
        public override bool IsLong(string key) {                                          //__SILP__
            DataType type = GetValueType(key);                                             //__SILP__
            return type == DataType.Long;                                                  //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
        public override bool TryGetLong(string key, out long val, bool isDebug = false) {  //__SILP__
            if (_LongValues == null) {                                                     //__SILP__
                Log.ErrorOrDebug(isDebug, "Value Not Exist: {0}", key);                    //__SILP__
            } else if (!IsLong(key)) {                                                     //__SILP__
                Log.ErrorOrDebug(isDebug, "Value Is Not Long: {0} -> {1} -> {2}",          //__SILP__
                    key, GetValueType(key), GetValue(key));                                //__SILP__
            } else {                                                                       //__SILP__
                long _val;                                                                 //__SILP__
                if (_LongValues.TryGetValue(key, out _val)) {                              //__SILP__
                    val = _val;                                                            //__SILP__
                    return true;                                                           //__SILP__
                } else {                                                                   //__SILP__
                    Log.Error("Value Not Found: {0}", key);                                //__SILP__
                }                                                                          //__SILP__
            }                                                                              //__SILP__
            val = default(long);                                                           //__SILP__
            return false;                                                                  //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
        public override long GetLong(string key) {                                         //__SILP__
            long result;                                                                   //__SILP__
            TryGetLong(key, out result);                                                   //__SILP__
            return result;                                                                 //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
        public override long GetLong(string key, long defaultValue) {                      //__SILP__
            long result;                                                                   //__SILP__
            if (TryGetLong(key, out result, true)) {                                       //__SILP__
                return result;                                                             //__SILP__
            }                                                                              //__SILP__
            return defaultValue;                                                           //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
        public override bool SetLong(string key, long val) {                               //__SILP__
            bool isTempKey = IsTempKey(key);                                               //__SILP__
            if (Sealed && !isTempKey) {                                                    //__SILP__
                Log.Error("Already Sealed: {0} -> {1}", key, val);                         //__SILP__
                return false;                                                              //__SILP__
            }                                                                              //__SILP__
            if (isTempKey || !_ValueTypes.ContainsKey(key)) {                              //__SILP__
                _ValueTypes[key] = DataType.Long;                                          //__SILP__
                if (_LongValues == null) {                                                 //__SILP__
                    _LongValues = new Dictionary<string, long>();                          //__SILP__
                }                                                                          //__SILP__
                _LongValues[key] = val;                                                    //__SILP__
                return true;                                                               //__SILP__
            }                                                                              //__SILP__
            Log.Error("Value Exist: {0} {1} -> {2}", key, GetValue(key), val);             //__SILP__
            return false;                                                                  //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
        public override void ForEachLong(Action<int, long> callback) {                     //__SILP__
            for (int i = 0; i < Count; i++) {                                              //__SILP__
                string key = i.ToString();                                                 //__SILP__
                callback(i, GetLong(key));                                                 //__SILP__
            }                                                                              //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
        public override bool UntilTrueLong(Func<int, long, bool> callback) {               //__SILP__
            for (int i = 0; i < Count; i++) {                                              //__SILP__
                string key = i.ToString();                                                 //__SILP__
                if (callback(i, GetLong(key))) {                                           //__SILP__
                    return true;                                                           //__SILP__
                }                                                                          //__SILP__
            }                                                                              //__SILP__
            return false;                                                                  //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
        public override bool UntilFalseLong(Func<int, long, bool> callback) {              //__SILP__
            for (int i = 0; i < Count; i++) {                                              //__SILP__
                string key = i.ToString();                                                 //__SILP__
                if (!callback(i, GetLong(key))) {                                          //__SILP__
                    return false;                                                          //__SILP__
                }                                                                          //__SILP__
            }                                                                              //__SILP__
            return true;                                                                   //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
                                                                                           //__SILP__
        //SILP: REAL_DATA_TYPE(Float, float)
        public override bool IsFloat(string key) {                                           //__SILP__
            DataType type = GetValueType(key);                                               //__SILP__
            return type == DataType.Float;                                                   //__SILP__
        }                                                                                    //__SILP__
                                                                                             //__SILP__
        public override bool TryGetFloat(string key, out float val, bool isDebug = false) {  //__SILP__
            if (_FloatValues == null) {                                                      //__SILP__
                Log.ErrorOrDebug(isDebug, "Value Not Exist: {0}", key);                      //__SILP__
            } else if (!IsFloat(key)) {                                                      //__SILP__
                Log.ErrorOrDebug(isDebug, "Value Is Not Float: {0} -> {1} -> {2}",           //__SILP__
                    key, GetValueType(key), GetValue(key));                                  //__SILP__
            } else {                                                                         //__SILP__
                float _val;                                                                  //__SILP__
                if (_FloatValues.TryGetValue(key, out _val)) {                               //__SILP__
                    val = _val;                                                              //__SILP__
                    return true;                                                             //__SILP__
                } else {                                                                     //__SILP__
                    Log.Error("Value Not Found: {0}", key);                                  //__SILP__
                }                                                                            //__SILP__
            }                                                                                //__SILP__
            val = default(float);                                                            //__SILP__
            return false;                                                                    //__SILP__
        }                                                                                    //__SILP__
                                                                                             //__SILP__
        public override float GetFloat(string key) {                                         //__SILP__
            float result;                                                                    //__SILP__
            TryGetFloat(key, out result);                                                    //__SILP__
            return result;                                                                   //__SILP__
        }                                                                                    //__SILP__
                                                                                             //__SILP__
        public override float GetFloat(string key, float defaultValue) {                     //__SILP__
            float result;                                                                    //__SILP__
            if (TryGetFloat(key, out result, true)) {                                        //__SILP__
                return result;                                                               //__SILP__
            }                                                                                //__SILP__
            return defaultValue;                                                             //__SILP__
        }                                                                                    //__SILP__
                                                                                             //__SILP__
        public override bool SetFloat(string key, float val) {                               //__SILP__
            bool isTempKey = IsTempKey(key);                                                 //__SILP__
            if (Sealed && !isTempKey) {                                                      //__SILP__
                Log.Error("Already Sealed: {0} -> {1}", key, val);                           //__SILP__
                return false;                                                                //__SILP__
            }                                                                                //__SILP__
            if (isTempKey || !_ValueTypes.ContainsKey(key)) {                                //__SILP__
                _ValueTypes[key] = DataType.Float;                                           //__SILP__
                if (_FloatValues == null) {                                                  //__SILP__
                    _FloatValues = new Dictionary<string, float>();                          //__SILP__
                }                                                                            //__SILP__
                _FloatValues[key] = val;                                                     //__SILP__
                return true;                                                                 //__SILP__
            }                                                                                //__SILP__
            Log.Error("Value Exist: {0} {1} -> {2}", key, GetValue(key), val);               //__SILP__
            return false;                                                                    //__SILP__
        }                                                                                    //__SILP__
                                                                                             //__SILP__
        public override void ForEachFloat(Action<int, float> callback) {                     //__SILP__
            for (int i = 0; i < Count; i++) {                                                //__SILP__
                string key = i.ToString();                                                   //__SILP__
                callback(i, GetFloat(key));                                                  //__SILP__
            }                                                                                //__SILP__
        }                                                                                    //__SILP__
                                                                                             //__SILP__
        public override bool UntilTrueFloat(Func<int, float, bool> callback) {               //__SILP__
            for (int i = 0; i < Count; i++) {                                                //__SILP__
                string key = i.ToString();                                                   //__SILP__
                if (callback(i, GetFloat(key))) {                                            //__SILP__
                    return true;                                                             //__SILP__
                }                                                                            //__SILP__
            }                                                                                //__SILP__
            return false;                                                                    //__SILP__
        }                                                                                    //__SILP__
                                                                                             //__SILP__
        public override bool UntilFalseFloat(Func<int, float, bool> callback) {              //__SILP__
            for (int i = 0; i < Count; i++) {                                                //__SILP__
                string key = i.ToString();                                                   //__SILP__
                if (!callback(i, GetFloat(key))) {                                           //__SILP__
                    return false;                                                            //__SILP__
                }                                                                            //__SILP__
            }                                                                                //__SILP__
            return true;                                                                     //__SILP__
        }                                                                                    //__SILP__
                                                                                             //__SILP__
                                                                                             //__SILP__
        //SILP: REAL_DATA_TYPE(Double, double)
        public override bool IsDouble(string key) {                                            //__SILP__
            DataType type = GetValueType(key);                                                 //__SILP__
            return type == DataType.Double;                                                    //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
        public override bool TryGetDouble(string key, out double val, bool isDebug = false) {  //__SILP__
            if (_DoubleValues == null) {                                                       //__SILP__
                Log.ErrorOrDebug(isDebug, "Value Not Exist: {0}", key);                        //__SILP__
            } else if (!IsDouble(key)) {                                                       //__SILP__
                Log.ErrorOrDebug(isDebug, "Value Is Not Double: {0} -> {1} -> {2}",            //__SILP__
                    key, GetValueType(key), GetValue(key));                                    //__SILP__
            } else {                                                                           //__SILP__
                double _val;                                                                   //__SILP__
                if (_DoubleValues.TryGetValue(key, out _val)) {                                //__SILP__
                    val = _val;                                                                //__SILP__
                    return true;                                                               //__SILP__
                } else {                                                                       //__SILP__
                    Log.Error("Value Not Found: {0}", key);                                    //__SILP__
                }                                                                              //__SILP__
            }                                                                                  //__SILP__
            val = default(double);                                                             //__SILP__
            return false;                                                                      //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
        public override double GetDouble(string key) {                                         //__SILP__
            double result;                                                                     //__SILP__
            TryGetDouble(key, out result);                                                     //__SILP__
            return result;                                                                     //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
        public override double GetDouble(string key, double defaultValue) {                    //__SILP__
            double result;                                                                     //__SILP__
            if (TryGetDouble(key, out result, true)) {                                         //__SILP__
                return result;                                                                 //__SILP__
            }                                                                                  //__SILP__
            return defaultValue;                                                               //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
        public override bool SetDouble(string key, double val) {                               //__SILP__
            bool isTempKey = IsTempKey(key);                                                   //__SILP__
            if (Sealed && !isTempKey) {                                                        //__SILP__
                Log.Error("Already Sealed: {0} -> {1}", key, val);                             //__SILP__
                return false;                                                                  //__SILP__
            }                                                                                  //__SILP__
            if (isTempKey || !_ValueTypes.ContainsKey(key)) {                                  //__SILP__
                _ValueTypes[key] = DataType.Double;                                            //__SILP__
                if (_DoubleValues == null) {                                                   //__SILP__
                    _DoubleValues = new Dictionary<string, double>();                          //__SILP__
                }                                                                              //__SILP__
                _DoubleValues[key] = val;                                                      //__SILP__
                return true;                                                                   //__SILP__
            }                                                                                  //__SILP__
            Log.Error("Value Exist: {0} {1} -> {2}", key, GetValue(key), val);                 //__SILP__
            return false;                                                                      //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
        public override void ForEachDouble(Action<int, double> callback) {                     //__SILP__
            for (int i = 0; i < Count; i++) {                                                  //__SILP__
                string key = i.ToString();                                                     //__SILP__
                callback(i, GetDouble(key));                                                   //__SILP__
            }                                                                                  //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
        public override bool UntilTrueDouble(Func<int, double, bool> callback) {               //__SILP__
            for (int i = 0; i < Count; i++) {                                                  //__SILP__
                string key = i.ToString();                                                     //__SILP__
                if (callback(i, GetDouble(key))) {                                             //__SILP__
                    return true;                                                               //__SILP__
                }                                                                              //__SILP__
            }                                                                                  //__SILP__
            return false;                                                                      //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
        public override bool UntilFalseDouble(Func<int, double, bool> callback) {              //__SILP__
            for (int i = 0; i < Count; i++) {                                                  //__SILP__
                string key = i.ToString();                                                     //__SILP__
                if (!callback(i, GetDouble(key))) {                                            //__SILP__
                    return false;                                                              //__SILP__
                }                                                                              //__SILP__
            }                                                                                  //__SILP__
            return true;                                                                       //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
                                                                                               //__SILP__
        //SILP: REAL_DATA_TYPE(String, string)
        public override bool IsString(string key) {                                            //__SILP__
            DataType type = GetValueType(key);                                                 //__SILP__
            return type == DataType.String;                                                    //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
        public override bool TryGetString(string key, out string val, bool isDebug = false) {  //__SILP__
            if (_StringValues == null) {                                                       //__SILP__
                Log.ErrorOrDebug(isDebug, "Value Not Exist: {0}", key);                        //__SILP__
            } else if (!IsString(key)) {                                                       //__SILP__
                Log.ErrorOrDebug(isDebug, "Value Is Not String: {0} -> {1} -> {2}",            //__SILP__
                    key, GetValueType(key), GetValue(key));                                    //__SILP__
            } else {                                                                           //__SILP__
                string _val;                                                                   //__SILP__
                if (_StringValues.TryGetValue(key, out _val)) {                                //__SILP__
                    val = _val;                                                                //__SILP__
                    return true;                                                               //__SILP__
                } else {                                                                       //__SILP__
                    Log.Error("Value Not Found: {0}", key);                                    //__SILP__
                }                                                                              //__SILP__
            }                                                                                  //__SILP__
            val = default(string);                                                             //__SILP__
            return false;                                                                      //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
        public override string GetString(string key) {                                         //__SILP__
            string result;                                                                     //__SILP__
            TryGetString(key, out result);                                                     //__SILP__
            return result;                                                                     //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
        public override string GetString(string key, string defaultValue) {                    //__SILP__
            string result;                                                                     //__SILP__
            if (TryGetString(key, out result, true)) {                                         //__SILP__
                return result;                                                                 //__SILP__
            }                                                                                  //__SILP__
            return defaultValue;                                                               //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
        public override bool SetString(string key, string val) {                               //__SILP__
            bool isTempKey = IsTempKey(key);                                                   //__SILP__
            if (Sealed && !isTempKey) {                                                        //__SILP__
                Log.Error("Already Sealed: {0} -> {1}", key, val);                             //__SILP__
                return false;                                                                  //__SILP__
            }                                                                                  //__SILP__
            if (isTempKey || !_ValueTypes.ContainsKey(key)) {                                  //__SILP__
                _ValueTypes[key] = DataType.String;                                            //__SILP__
                if (_StringValues == null) {                                                   //__SILP__
                    _StringValues = new Dictionary<string, string>();                          //__SILP__
                }                                                                              //__SILP__
                _StringValues[key] = val;                                                      //__SILP__
                return true;                                                                   //__SILP__
            }                                                                                  //__SILP__
            Log.Error("Value Exist: {0} {1} -> {2}", key, GetValue(key), val);                 //__SILP__
            return false;                                                                      //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
        public override void ForEachString(Action<int, string> callback) {                     //__SILP__
            for (int i = 0; i < Count; i++) {                                                  //__SILP__
                string key = i.ToString();                                                     //__SILP__
                callback(i, GetString(key));                                                   //__SILP__
            }                                                                                  //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
        public override bool UntilTrueString(Func<int, string, bool> callback) {               //__SILP__
            for (int i = 0; i < Count; i++) {                                                  //__SILP__
                string key = i.ToString();                                                     //__SILP__
                if (callback(i, GetString(key))) {                                             //__SILP__
                    return true;                                                               //__SILP__
                }                                                                              //__SILP__
            }                                                                                  //__SILP__
            return false;                                                                      //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
        public override bool UntilFalseString(Func<int, string, bool> callback) {              //__SILP__
            for (int i = 0; i < Count; i++) {                                                  //__SILP__
                string key = i.ToString();                                                     //__SILP__
                if (!callback(i, GetString(key))) {                                            //__SILP__
                    return false;                                                              //__SILP__
                }                                                                              //__SILP__
            }                                                                                  //__SILP__
            return true;                                                                       //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
                                                                                               //__SILP__
        //SILP: REAL_DATA_TYPE(Data, Data)
        public override bool IsData(string key) {                                          //__SILP__
            DataType type = GetValueType(key);                                             //__SILP__
            return type == DataType.Data;                                                  //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
        public override bool TryGetData(string key, out Data val, bool isDebug = false) {  //__SILP__
            if (_DataValues == null) {                                                     //__SILP__
                Log.ErrorOrDebug(isDebug, "Value Not Exist: {0}", key);                    //__SILP__
            } else if (!IsData(key)) {                                                     //__SILP__
                Log.ErrorOrDebug(isDebug, "Value Is Not Data: {0} -> {1} -> {2}",          //__SILP__
                    key, GetValueType(key), GetValue(key));                                //__SILP__
            } else {                                                                       //__SILP__
                Data _val;                                                                 //__SILP__
                if (_DataValues.TryGetValue(key, out _val)) {                              //__SILP__
                    val = _val;                                                            //__SILP__
                    return true;                                                           //__SILP__
                } else {                                                                   //__SILP__
                    Log.Error("Value Not Found: {0}", key);                                //__SILP__
                }                                                                          //__SILP__
            }                                                                              //__SILP__
            val = default(Data);                                                           //__SILP__
            return false;                                                                  //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
        public override Data GetData(string key) {                                         //__SILP__
            Data result;                                                                   //__SILP__
            TryGetData(key, out result);                                                   //__SILP__
            return result;                                                                 //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
        public override Data GetData(string key, Data defaultValue) {                      //__SILP__
            Data result;                                                                   //__SILP__
            if (TryGetData(key, out result, true)) {                                       //__SILP__
                return result;                                                             //__SILP__
            }                                                                              //__SILP__
            return defaultValue;                                                           //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
        public override bool SetData(string key, Data val) {                               //__SILP__
            bool isTempKey = IsTempKey(key);                                               //__SILP__
            if (Sealed && !isTempKey) {                                                    //__SILP__
                Log.Error("Already Sealed: {0} -> {1}", key, val);                         //__SILP__
                return false;                                                              //__SILP__
            }                                                                              //__SILP__
            if (isTempKey || !_ValueTypes.ContainsKey(key)) {                              //__SILP__
                _ValueTypes[key] = DataType.Data;                                          //__SILP__
                if (_DataValues == null) {                                                 //__SILP__
                    _DataValues = new Dictionary<string, Data>();                          //__SILP__
                }                                                                          //__SILP__
                _DataValues[key] = val;                                                    //__SILP__
                return true;                                                               //__SILP__
            }                                                                              //__SILP__
            Log.Error("Value Exist: {0} {1} -> {2}", key, GetValue(key), val);             //__SILP__
            return false;                                                                  //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
        public override void ForEachData(Action<int, Data> callback) {                     //__SILP__
            for (int i = 0; i < Count; i++) {                                              //__SILP__
                string key = i.ToString();                                                 //__SILP__
                callback(i, GetData(key));                                                 //__SILP__
            }                                                                              //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
        public override bool UntilTrueData(Func<int, Data, bool> callback) {               //__SILP__
            for (int i = 0; i < Count; i++) {                                              //__SILP__
                string key = i.ToString();                                                 //__SILP__
                if (callback(i, GetData(key))) {                                           //__SILP__
                    return true;                                                           //__SILP__
                }                                                                          //__SILP__
            }                                                                              //__SILP__
            return false;                                                                  //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
        public override bool UntilFalseData(Func<int, Data, bool> callback) {              //__SILP__
            for (int i = 0; i < Count; i++) {                                              //__SILP__
                string key = i.ToString();                                                 //__SILP__
                if (!callback(i, GetData(key))) {                                          //__SILP__
                    return false;                                                          //__SILP__
                }                                                                          //__SILP__
            }                                                                              //__SILP__
            return true;                                                                   //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
                                                                                           //__SILP__
    }
}
