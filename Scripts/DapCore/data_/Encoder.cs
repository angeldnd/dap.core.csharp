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

        public static Encoder<T> GetEncoder<T>() {
            return _Encoders.GetValue<Encoder<T>>(typeof(T).FullName);
        }

        public static T Decode<T>(Data data, string key) {
            Encoder<T> encoder = GetEncoder<T>();
            if (encoder != null) {
                return encoder.Decode(data, key);
            } else {
                Log.Error("Decode Failed, Unknown Type: <{0}> {1} {2}",
                            Convertor.GetTypeStr(typeof(T)), data, key);
            }
            return default(T);
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
        public T Decode(Data data, string key) {
            if (data == null) return default(T);
            if (key == null) return default(T);
            return DoDecode(data, key);
        }

        public bool Encode(Data data, string key, T val) {
            if (data == null) return false;
            if (key == null) return false;
            return DoEncode(data, key, val);
        }

        protected abstract T DoDecode(Data data, string key);
        protected abstract bool DoEncode(Data data, string key, T val);
    }

    public class BoolEncoder : Encoder<bool> {
        protected override bool DoEncode(Data data, string key, bool val) {
            return data.SetBool(key, val);
        }

        protected override bool DoDecode(Data data, string key) {
            return data.GetBool(key);
        }
    }

    public class IntEncoder : Encoder<int> {
        protected override bool DoEncode(Data data, string key, int val) {
            return data.SetInt(key, val);
        }

        protected override int DoDecode(Data data, string key) {
            return data.GetInt(key);
        }
    }

    public class LongEncoder : Encoder<long> {
        protected override bool DoEncode(Data data, string key, long val) {
            return data.SetLong(key, val);
        }

        protected override long DoDecode(Data data, string key) {
            return data.GetLong(key);
        }
    }

    public class FloatEncoder : Encoder<float> {
        protected override bool DoEncode(Data data, string key, float val) {
            return data.SetFloat(key, val);
        }

        protected override float DoDecode(Data data, string key) {
            return data.GetFloat(key);
        }
    }

    public class DoubleEncoder : Encoder<double> {
        protected override bool DoEncode(Data data, string key, double val) {
            return data.SetDouble(key, val);
        }

        protected override double DoDecode(Data data, string key) {
            return data.GetDouble(key);
        }
    }

    public class StringEncoder : Encoder<string> {
        protected override bool DoEncode(Data data, string key, string val) {
            return data.SetString(key, val);
        }

        protected override string DoDecode(Data data, string key) {
            return data.GetString(key);
        }
    }

    public abstract class DataEncoder<T> : Encoder<T> {
        protected override bool DoEncode(Data data, string key, T val) {
            return data.SetData(key, EncodeValue(val));
        }

        protected override T DoDecode(Data data, string key) {
            Data valueData = data.GetData(key);
            if (valueData == null) return default(T);
            return DecodeValue(valueData);
        }

        protected abstract Data EncodeValue(T val);
        protected abstract T DecodeValue(Data valueData);
    }

    public class DataEncoder : DataEncoder<Data> {
        protected override Data EncodeValue(Data val) {
            return val;
        }

        protected override Data DecodeValue(Data valueData) {
            return valueData;
        }
    }
}
