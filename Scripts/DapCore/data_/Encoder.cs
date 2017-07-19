using System;

namespace angeldnd.dap {
    public static class Encoder {
        public readonly static BoolEncoder BoolEncoder = new BoolEncoder();
        public readonly static IntEncoder IntEncoder = new IntEncoder();
        public readonly static LongEncoder LongEncoder = new LongEncoder();
        public readonly static FloatEncoder FloatEncoder = new FloatEncoder();
        public readonly static DoubleEncoder DoubleEncoder = new DoubleEncoder();
        public readonly static StringEncoder StringEncoder = new StringEncoder();
        public readonly static DataEncoder DataEncoder = new DataEncoder();

        private static Vars _Encoders;

        static Encoder() {
            _Encoders = new Vars(null, "Encoders");

            RegisterEncoder<bool>(BoolEncoder);
            RegisterEncoder<int>(IntEncoder);
            RegisterEncoder<long>(LongEncoder);
            RegisterEncoder<float>(FloatEncoder);
            RegisterEncoder<double>(DoubleEncoder);
            RegisterEncoder<string>(StringEncoder);
            RegisterEncoder<Data>(DataEncoder);
        }

        public static bool RegisterEncoder<T>(Encoder<T> encoder) {
            return _Encoders.AddVar(typeof(T).FullName, encoder) != null;
        }

        public static Encoder<T> GetEncoder<T>(bool isDebug = false) {
            if (isDebug) {
                return _Encoders.GetValue<Encoder<T>>(typeof(T).FullName, null);
            } else {
                return _Encoders.GetValue<Encoder<T>>(typeof(T).FullName);
            }
        }

        public static bool TryDecode<T>(Data data, string key, out T val, bool isDebug = false) {
            Encoder<T> encoder = GetEncoder<T>();
            if (encoder != null) {
                return encoder.TryDecode(data, key, out val, isDebug);
            } else {
                Log.Error("Decode Failed, Unknown Type: <{0}> {1} {2}",
                            Convertor.GetTypeStr(typeof(T)), data, key);
            }
            val = default(T);
            return false;
        }

        public static T Decode<T>(Data data, string key, T defaultValue) {
            T val;
            if (TryDecode(data, key, out val)) {
                return val;
            }
            return defaultValue;
        }

        public static bool Encode<T>(Data data, string key, T val) {
            Encoder<T> encoder = GetEncoder<T>();
            if (encoder != null) {
                return encoder.Encode(data, key, val);
            } else {
                Log.Error("Encode Failed, Unknown Type: <{0}> {1} {2} {3}",
                            Convertor.GetTypeStr(typeof(T)), data, key, val);
            }
            return false;
        }
    }

    public abstract class Encoder<T> {
        public bool TryDecode(Data data, string key, out T val, bool isDebug = false) {
            if (data != null && key != null) {
                return DoDecode(data, key, out val, isDebug);
            }
            val = default(T);
            return false;
        }

        public bool Encode(Data data, string key, T val) {
            if (data == null) return false;
            if (key == null) return false;
            return DoEncode(data, key, val);
        }

        public abstract string GetDapType();
        protected abstract bool DoDecode(Data data, string key, out T val, bool isDebug = false);
        protected abstract bool DoEncode(Data data, string key, T val);
    }

    public class BoolEncoder : Encoder<bool> {
        public override string GetDapType() {
            return PropertiesConsts.TypeBoolProperty;
        }

        protected override bool DoEncode(Data data, string key, bool val) {
            return data.SetBool(key, val);
        }

        protected override bool DoDecode(Data data, string key, out bool val, bool isDebug = false) {
            return data.TryGetBool(key, out val, isDebug);
        }
    }

    public class IntEncoder : Encoder<int> {
        public override string GetDapType() {
            return PropertiesConsts.TypeIntProperty;
        }

        protected override bool DoEncode(Data data, string key, int val) {
            return data.SetInt(key, val);
        }

        protected override bool DoDecode(Data data, string key, out int val, bool isDebug = false) {
            return data.TryGetInt(key, out val, isDebug);
        }
    }

    public class LongEncoder : Encoder<long> {
        public override string GetDapType() {
            return PropertiesConsts.TypeLongProperty;
        }

        protected override bool DoEncode(Data data, string key, long val) {
            return data.SetLong(key, val);
        }

        protected override bool DoDecode(Data data, string key, out long val, bool isDebug = false) {
            return data.TryGetLong(key, out val, isDebug);
        }
    }

    public class FloatEncoder : Encoder<float> {
        public override string GetDapType() {
            return PropertiesConsts.TypeFloatProperty;
        }

        protected override bool DoEncode(Data data, string key, float val) {
            return data.SetFloat(key, val);
        }

        protected override bool DoDecode(Data data, string key, out float val, bool isDebug = false) {
            return data.TryGetFloat(key, out val, isDebug);
        }
    }

    public class DoubleEncoder : Encoder<double> {
        public override string GetDapType() {
            return PropertiesConsts.TypeDoubleProperty;
        }

        protected override bool DoEncode(Data data, string key, double val) {
            return data.SetDouble(key, val);
        }

        protected override bool DoDecode(Data data, string key, out double val, bool isDebug = false) {
            return data.TryGetDouble(key, out val, isDebug);
        }
    }

    public class StringEncoder : Encoder<string> {
        public override string GetDapType() {
            return PropertiesConsts.TypeStringProperty;
        }

        protected override bool DoEncode(Data data, string key, string val) {
            return data.SetString(key, val);
        }

        protected override bool DoDecode(Data data, string key, out string val, bool isDebug = false) {
            return data.TryGetString(key, out val, isDebug);
        }
    }

    public class DataEncoder : Encoder<Data> {
        public override string GetDapType() {
            return PropertiesConsts.TypeDataProperty;
        }

        protected override bool DoEncode(Data data, string key, Data val) {
            return data.SetData(key, val);
        }

        protected override bool DoDecode(Data data, string key, out Data val, bool isDebug = false) {
            return data.TryGetData(key, out val, isDebug);
        }
    }

    public abstract class DataEncoder<T> : Encoder<T> {
        protected override bool DoEncode(Data data, string key, T val) {
            return data.SetData(key, EncodeValue(val));
        }

        protected override bool DoDecode(Data data, string key, out T val, bool isDebug = false) {
            Data valueData;
            if (data.TryGetData(key, out valueData, isDebug)) {
                return DoDecodeValue(valueData, out val, isDebug);
            }
            val = default(T);
            return false;
        }

        protected Data EncodeValue(T val) {
            return EncodeValue(DataCache.Take(typeof(T).FullName), val);
        }

        protected abstract Data EncodeValue(Data valueData, T val);
        protected abstract bool DoDecodeValue(Data valueData, out T val, bool isDebug = false);
    }
}
