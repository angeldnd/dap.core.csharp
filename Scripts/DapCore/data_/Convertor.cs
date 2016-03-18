using System;

namespace angeldnd.dap {
    public static class Convertor {
        public readonly static BoolConvertor BoolConvertor = new BoolConvertor();
        public readonly static IntConvertor IntConvertor = new IntConvertor();
        public readonly static LongConvertor LongConvertor = new LongConvertor();
        public readonly static FloatConvertor FloatConvertor = new FloatConvertor();
        public readonly static DoubleConvertor DoubleConvertor = new DoubleConvertor();
        public readonly static StringConvertor StringConvertor = new StringConvertor();
        public readonly static DataConvertor DataConvertor = new DataConvertor();

        private static Vars _Convertors;

        static Convertor() {
            _Convertors = new Vars(null, "Convertors");

            RegisterConvertor<bool>(BoolConvertor);
            RegisterConvertor<int>(IntConvertor);
            RegisterConvertor<long>(LongConvertor);
            RegisterConvertor<float>(FloatConvertor);
            RegisterConvertor<double>(DoubleConvertor);
            RegisterConvertor<string>(StringConvertor);
            RegisterConvertor<Data>(DataConvertor);
        }

        public static bool RegisterConvertor<T>(Convertor<T> convertor) {
            return _Convertors.AddVar(typeof(T).FullName, convertor) != null;
        }

        public static Convertor<T> GetConvertor<T>() {
            return _Convertors.GetValue<Convertor<T>>(typeof(T).FullName);
        }
    }

    public abstract class Convertor<T> {
        public bool TryParse(string str, out T val, bool isDebug = false) {
            try {
                val = Parse(str);
                return true;
            } catch (Exception e) {
                Log.ErrorOrDebug(isDebug, "Parse Failed: <{0}> {1} -> \n{2}", typeof(T).FullName, str, e);
            }
            val = default(T);
            return false;
        }

        public abstract T Parse(string str);
        public abstract string Convert(T val);
    }

    public class BoolConvertor : Convertor<bool> {
        public override string Convert(bool val) {
            return val ? "true" : "false";
        }

        public override bool Parse(string str) {
            return str == "true";
        }
    }

    public class IntConvertor : Convertor<int> {
        public override string Convert(int val) {
            return val.ToString();
        }

        public override int Parse(string str) {
            return System.Convert.ToInt32(str);
        }
    }

    public class LongConvertor : Convertor<long> {
        public override string Convert(long val) {
            return val.ToString();
        }

        public override long Parse(string str) {
            return System.Convert.ToInt64(str);
        }
    }

    public class FloatConvertor : Convertor<float> {
        public override string Convert(float val) {
            return val.ToString();
        }

        public override float Parse(string str) {
            return System.Convert.ToSingle(str);
        }
    }

    public class DoubleConvertor : Convertor<double> {
        public override string Convert(double val) {
            return val.ToString();
        }

        public override double Parse(string str) {
            return System.Convert.ToDouble(str);
        }
    }

    public class StringConvertor : Convertor<string> {
        public override string Convert(string val) {
            return val == null ? "null" : val.ToString();
        }

        public override string Parse(string str) {
            return str;
        }
    }
}
