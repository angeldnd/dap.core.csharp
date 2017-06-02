using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public sealed class WeakData : Data {
        public readonly string Kind = null;
        private RealData _Real = null;

        public WeakData(string kind, RealData real) {
            Kind = kind;
            _Real = real;
        }

        public override Data Clone() {
            WeakData clone = null;
            if (Kind != null) {
                clone = DataCache.Take(Kind);
                _Real._CopyTo(clone._Real);
            } else {
                clone = new WeakData(Kind, _Real.Clone() as RealData);
            }
            return clone;
        }

        protected override void OnRecycle() {
            _Real._Recycle();
        }

        public override void Clear() {
            _Real.Clear();
        }

        public override int Count {
            get {
                return _Real.Count;
            }
        }

        public override IEnumerable<string> Keys {
            get {
                return _Real.Keys;
            }
        }

        public override bool HasKey(string key) {
            return _Real.HasKey(key);
        }

        public override DataType GetValueType(string key) {
            return _Real.GetValueType(key);
        }

        public override object GetValue(string key) {
            return _Real.GetValue(key);
        }

        public override bool TryGetValue(string key, out object val) {
            return _Real.TryGetValue(key, out val);
        }

        //SILP: WEAK_DATA_TYPE(Bool, bool)
        public override bool IsBool(string key) {                              //__SILP__
            return _Real.IsBool(key);                                          //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public override bool GetBool(string key) {                             //__SILP__
            return _Real.GetBool(key);                                         //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public override bool GetBool(string key, bool defaultValue) {          //__SILP__
            return _Real.GetBool(key, defaultValue);                           //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public override bool SetBool(string key, bool val) {                   //__SILP__
            return _Real.SetBool(key, val);                                    //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public override void ForEachBool(Action<int, bool> callback) {         //__SILP__
            _Real.ForEachBool(callback);                                       //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public override bool UntilTrueBool(Func<int, bool, bool> callback) {   //__SILP__
            return _Real.UntilTrueBool(callback);                              //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public override bool UntilFalseBool(Func<int, bool, bool> callback) {  //__SILP__
            return _Real.UntilFalseBool(callback);                             //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        //SILP: WEAK_DATA_TYPE(Int, int)
        public override bool IsInt(string key) {                             //__SILP__
            return _Real.IsInt(key);                                         //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        public override int GetInt(string key) {                             //__SILP__
            return _Real.GetInt(key);                                        //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        public override int GetInt(string key, int defaultValue) {           //__SILP__
            return _Real.GetInt(key, defaultValue);                          //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        public override bool SetInt(string key, int val) {                   //__SILP__
            return _Real.SetInt(key, val);                                   //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        public override void ForEachInt(Action<int, int> callback) {         //__SILP__
            _Real.ForEachInt(callback);                                      //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        public override bool UntilTrueInt(Func<int, int, bool> callback) {   //__SILP__
            return _Real.UntilTrueInt(callback);                             //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        public override bool UntilFalseInt(Func<int, int, bool> callback) {  //__SILP__
            return _Real.UntilFalseInt(callback);                            //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        //SILP: WEAK_DATA_TYPE(Long, long)
        public override bool IsLong(string key) {                              //__SILP__
            return _Real.IsLong(key);                                          //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public override long GetLong(string key) {                             //__SILP__
            return _Real.GetLong(key);                                         //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public override long GetLong(string key, long defaultValue) {          //__SILP__
            return _Real.GetLong(key, defaultValue);                           //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public override bool SetLong(string key, long val) {                   //__SILP__
            return _Real.SetLong(key, val);                                    //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public override void ForEachLong(Action<int, long> callback) {         //__SILP__
            _Real.ForEachLong(callback);                                       //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public override bool UntilTrueLong(Func<int, long, bool> callback) {   //__SILP__
            return _Real.UntilTrueLong(callback);                              //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public override bool UntilFalseLong(Func<int, long, bool> callback) {  //__SILP__
            return _Real.UntilFalseLong(callback);                             //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        //SILP: WEAK_DATA_TYPE(Float, float)
        public override bool IsFloat(string key) {                               //__SILP__
            return _Real.IsFloat(key);                                           //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
        public override float GetFloat(string key) {                             //__SILP__
            return _Real.GetFloat(key);                                          //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
        public override float GetFloat(string key, float defaultValue) {         //__SILP__
            return _Real.GetFloat(key, defaultValue);                            //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
        public override bool SetFloat(string key, float val) {                   //__SILP__
            return _Real.SetFloat(key, val);                                     //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
        public override void ForEachFloat(Action<int, float> callback) {         //__SILP__
            _Real.ForEachFloat(callback);                                        //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
        public override bool UntilTrueFloat(Func<int, float, bool> callback) {   //__SILP__
            return _Real.UntilTrueFloat(callback);                               //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
        public override bool UntilFalseFloat(Func<int, float, bool> callback) {  //__SILP__
            return _Real.UntilFalseFloat(callback);                              //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
        //SILP: WEAK_DATA_TYPE(Double, double)
        public override bool IsDouble(string key) {                                //__SILP__
            return _Real.IsDouble(key);                                            //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public override double GetDouble(string key) {                             //__SILP__
            return _Real.GetDouble(key);                                           //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public override double GetDouble(string key, double defaultValue) {        //__SILP__
            return _Real.GetDouble(key, defaultValue);                             //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public override bool SetDouble(string key, double val) {                   //__SILP__
            return _Real.SetDouble(key, val);                                      //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public override void ForEachDouble(Action<int, double> callback) {         //__SILP__
            _Real.ForEachDouble(callback);                                         //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public override bool UntilTrueDouble(Func<int, double, bool> callback) {   //__SILP__
            return _Real.UntilTrueDouble(callback);                                //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public override bool UntilFalseDouble(Func<int, double, bool> callback) {  //__SILP__
            return _Real.UntilFalseDouble(callback);                               //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        //SILP: WEAK_DATA_TYPE(String, string)
        public override bool IsString(string key) {                                //__SILP__
            return _Real.IsString(key);                                            //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public override string GetString(string key) {                             //__SILP__
            return _Real.GetString(key);                                           //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public override string GetString(string key, string defaultValue) {        //__SILP__
            return _Real.GetString(key, defaultValue);                             //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public override bool SetString(string key, string val) {                   //__SILP__
            return _Real.SetString(key, val);                                      //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public override void ForEachString(Action<int, string> callback) {         //__SILP__
            _Real.ForEachString(callback);                                         //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public override bool UntilTrueString(Func<int, string, bool> callback) {   //__SILP__
            return _Real.UntilTrueString(callback);                                //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public override bool UntilFalseString(Func<int, string, bool> callback) {  //__SILP__
            return _Real.UntilFalseString(callback);                               //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__

        public override bool IsData(string key) {
            return _Real.IsData(key);
        }

        public override Data GetData(string key) {
            return _Real.GetData(key);
        }

        public override Data GetData(string key, Data defaultValue) {
            return _Real.GetData(key, defaultValue);
        }

        public override bool SetData(string key, Data val) {
            return _Real.SetData(key, val);
        }

        public override void ForEachData(Action<int, Data> callback) {
            _Real.ForEachData(callback);
        }

        public override bool UntilTrueData(Func<int, Data, bool> callback) {
            return _Real.UntilTrueData(callback);
        }

        public override bool UntilFalseData(Func<int, Data, bool> callback) {
            return _Real.UntilFalseData(callback);
        }
    }
}
