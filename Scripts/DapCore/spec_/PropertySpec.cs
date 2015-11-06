using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public interface SpecValueChecker : ValueChecker {
        bool DoEncode(Data spec);
    }

    public abstract class SpecValueChecker<T> : SpecValueChecker, ValueChecker<T> {
        public bool IsValid(string path, T val, T newVal) {
            return IsValid(newVal);
        }

        protected abstract bool IsValid(T val);
        public abstract bool DoEncode(Data spec);
    }

    //SILP: PROPERTY_SPEC(Int, int)
    public class IntSpecValueCheckerBigger : SpecValueChecker<int> {               //__SILP__
        public readonly int MinValue;                                              //__SILP__
        public IntSpecValueCheckerBigger(int minValue) {                           //__SILP__
            MinValue = minValue;                                                   //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        protected override bool IsValid(int val) {                                 //__SILP__
            return val > MinValue;                                                 //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public override bool DoEncode(Data spec) {                                 //__SILP__
            return spec.SetInt(PropertiesSpecConsts.KeyBigger, MinValue);          //__SILP__
        }                                                                          //__SILP__
    }                                                                              //__SILP__
                                                                                   //__SILP__
    public class IntSpecValueCheckerBiggerOrEqual : SpecValueChecker<int> {        //__SILP__
        public readonly int MinValue;                                              //__SILP__
        public IntSpecValueCheckerBiggerOrEqual(int minValue) {                    //__SILP__
            MinValue = minValue;                                                   //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        protected override bool IsValid(int val) {                                 //__SILP__
            return val >= MinValue;                                                //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public override bool DoEncode(Data spec) {                                 //__SILP__
            return spec.SetInt(PropertiesSpecConsts.KeyBiggerOrEqual, MinValue);   //__SILP__
        }                                                                          //__SILP__
    }                                                                              //__SILP__
                                                                                   //__SILP__
    public class IntSpecValueCheckerSmaller : SpecValueChecker<int> {              //__SILP__
        public readonly int MaxValue;                                              //__SILP__
        public IntSpecValueCheckerSmaller(int maxValue) {                          //__SILP__
            MaxValue = maxValue;                                                   //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        protected override bool IsValid(int val) {                                 //__SILP__
            return val < MaxValue;                                                 //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public override bool DoEncode(Data spec) {                                 //__SILP__
            return spec.SetInt(PropertiesSpecConsts.KeySmaller, MaxValue);         //__SILP__
        }                                                                          //__SILP__
    }                                                                              //__SILP__
                                                                                   //__SILP__
    public class IntSpecValueCheckerSmallerOrEqual : SpecValueChecker<int> {       //__SILP__
        public readonly int MaxValue;                                              //__SILP__
        public IntSpecValueCheckerSmallerOrEqual(int maxValue) {                   //__SILP__
            MaxValue = maxValue;                                                   //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        protected override bool IsValid(int val) {                                 //__SILP__
            return val <= MaxValue;                                                //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public override bool DoEncode(Data spec) {                                 //__SILP__
            return spec.SetInt(PropertiesSpecConsts.KeySmallerOrEqual, MaxValue);  //__SILP__
        }                                                                          //__SILP__
    }                                                                              //__SILP__
                                                                                   //__SILP__
    public class IntSpecValueCheckerIn : SpecValueChecker<int> {                   //__SILP__
        public readonly int[] Values;                                              //__SILP__
        public IntSpecValueCheckerIn(int[] values) {                               //__SILP__
            Values = values;                                                       //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        protected override bool IsValid(int val) {                                 //__SILP__
            foreach (int v in Values) {                                            //__SILP__
                if (v == val) return true;                                         //__SILP__
            }                                                                      //__SILP__
            return false;                                                          //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public override bool DoEncode(Data spec) {                                 //__SILP__
            Data values = new Data();                                              //__SILP__
            for (int i = 0; i < Values.Length; i++) {                              //__SILP__
                int v = Values[i];                                                 //__SILP__
                values.SetInt(i.ToString(), v);                                    //__SILP__
            }                                                                      //__SILP__
            return spec.SetData(PropertiesSpecConsts.KeyIn, values);               //__SILP__
        }                                                                          //__SILP__
    }                                                                              //__SILP__
                                                                                   //__SILP__
    public class IntSpecValueCheckerNotIn : SpecValueChecker<int> {                //__SILP__
        public readonly int[] Values;                                              //__SILP__
        public IntSpecValueCheckerNotIn(int[] values) {                            //__SILP__
            Values = values;                                                       //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        protected override bool IsValid(int val) {                                 //__SILP__
            foreach (int v in Values) {                                            //__SILP__
                if (v == val) return false;                                        //__SILP__
            }                                                                      //__SILP__
            return true;                                                           //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public override bool DoEncode(Data spec) {                                 //__SILP__
            Data values = new Data();                                              //__SILP__
            for (int i = 0; i < Values.Length; i++) {                              //__SILP__
                int v = Values[i];                                                 //__SILP__
                values.SetInt(i.ToString(), v);                                    //__SILP__
            }                                                                      //__SILP__
            return spec.SetData(PropertiesSpecConsts.KeyNotIn, values);            //__SILP__
        }                                                                          //__SILP__
    }                                                                              //__SILP__
                                                                                   //__SILP__
    //SILP: PROPERTY_SPEC(Long, long)
    public class LongSpecValueCheckerBigger : SpecValueChecker<long> {              //__SILP__
        public readonly long MinValue;                                              //__SILP__
        public LongSpecValueCheckerBigger(long minValue) {                          //__SILP__
            MinValue = minValue;                                                    //__SILP__
        }                                                                           //__SILP__
                                                                                    //__SILP__
        protected override bool IsValid(long val) {                                 //__SILP__
            return val > MinValue;                                                  //__SILP__
        }                                                                           //__SILP__
                                                                                    //__SILP__
        public override bool DoEncode(Data spec) {                                  //__SILP__
            return spec.SetLong(PropertiesSpecConsts.KeyBigger, MinValue);          //__SILP__
        }                                                                           //__SILP__
    }                                                                               //__SILP__
                                                                                    //__SILP__
    public class LongSpecValueCheckerBiggerOrEqual : SpecValueChecker<long> {       //__SILP__
        public readonly long MinValue;                                              //__SILP__
        public LongSpecValueCheckerBiggerOrEqual(long minValue) {                   //__SILP__
            MinValue = minValue;                                                    //__SILP__
        }                                                                           //__SILP__
                                                                                    //__SILP__
        protected override bool IsValid(long val) {                                 //__SILP__
            return val >= MinValue;                                                 //__SILP__
        }                                                                           //__SILP__
                                                                                    //__SILP__
        public override bool DoEncode(Data spec) {                                  //__SILP__
            return spec.SetLong(PropertiesSpecConsts.KeyBiggerOrEqual, MinValue);   //__SILP__
        }                                                                           //__SILP__
    }                                                                               //__SILP__
                                                                                    //__SILP__
    public class LongSpecValueCheckerSmaller : SpecValueChecker<long> {             //__SILP__
        public readonly long MaxValue;                                              //__SILP__
        public LongSpecValueCheckerSmaller(long maxValue) {                         //__SILP__
            MaxValue = maxValue;                                                    //__SILP__
        }                                                                           //__SILP__
                                                                                    //__SILP__
        protected override bool IsValid(long val) {                                 //__SILP__
            return val < MaxValue;                                                  //__SILP__
        }                                                                           //__SILP__
                                                                                    //__SILP__
        public override bool DoEncode(Data spec) {                                  //__SILP__
            return spec.SetLong(PropertiesSpecConsts.KeySmaller, MaxValue);         //__SILP__
        }                                                                           //__SILP__
    }                                                                               //__SILP__
                                                                                    //__SILP__
    public class LongSpecValueCheckerSmallerOrEqual : SpecValueChecker<long> {      //__SILP__
        public readonly long MaxValue;                                              //__SILP__
        public LongSpecValueCheckerSmallerOrEqual(long maxValue) {                  //__SILP__
            MaxValue = maxValue;                                                    //__SILP__
        }                                                                           //__SILP__
                                                                                    //__SILP__
        protected override bool IsValid(long val) {                                 //__SILP__
            return val <= MaxValue;                                                 //__SILP__
        }                                                                           //__SILP__
                                                                                    //__SILP__
        public override bool DoEncode(Data spec) {                                  //__SILP__
            return spec.SetLong(PropertiesSpecConsts.KeySmallerOrEqual, MaxValue);  //__SILP__
        }                                                                           //__SILP__
    }                                                                               //__SILP__
                                                                                    //__SILP__
    public class LongSpecValueCheckerIn : SpecValueChecker<long> {                  //__SILP__
        public readonly long[] Values;                                              //__SILP__
        public LongSpecValueCheckerIn(long[] values) {                              //__SILP__
            Values = values;                                                        //__SILP__
        }                                                                           //__SILP__
                                                                                    //__SILP__
        protected override bool IsValid(long val) {                                 //__SILP__
            foreach (long v in Values) {                                            //__SILP__
                if (v == val) return true;                                          //__SILP__
            }                                                                       //__SILP__
            return false;                                                           //__SILP__
        }                                                                           //__SILP__
                                                                                    //__SILP__
        public override bool DoEncode(Data spec) {                                  //__SILP__
            Data values = new Data();                                               //__SILP__
            for (int i = 0; i < Values.Length; i++) {                               //__SILP__
                long v = Values[i];                                                 //__SILP__
                values.SetLong(i.ToString(), v);                                    //__SILP__
            }                                                                       //__SILP__
            return spec.SetData(PropertiesSpecConsts.KeyIn, values);                //__SILP__
        }                                                                           //__SILP__
    }                                                                               //__SILP__
                                                                                    //__SILP__
    public class LongSpecValueCheckerNotIn : SpecValueChecker<long> {               //__SILP__
        public readonly long[] Values;                                              //__SILP__
        public LongSpecValueCheckerNotIn(long[] values) {                           //__SILP__
            Values = values;                                                        //__SILP__
        }                                                                           //__SILP__
                                                                                    //__SILP__
        protected override bool IsValid(long val) {                                 //__SILP__
            foreach (long v in Values) {                                            //__SILP__
                if (v == val) return false;                                         //__SILP__
            }                                                                       //__SILP__
            return true;                                                            //__SILP__
        }                                                                           //__SILP__
                                                                                    //__SILP__
        public override bool DoEncode(Data spec) {                                  //__SILP__
            Data values = new Data();                                               //__SILP__
            for (int i = 0; i < Values.Length; i++) {                               //__SILP__
                long v = Values[i];                                                 //__SILP__
                values.SetLong(i.ToString(), v);                                    //__SILP__
            }                                                                       //__SILP__
            return spec.SetData(PropertiesSpecConsts.KeyNotIn, values);             //__SILP__
        }                                                                           //__SILP__
    }                                                                               //__SILP__
                                                                                    //__SILP__
    //SILP: PROPERTY_SPEC(Float, float)
    public class FloatSpecValueCheckerBigger : SpecValueChecker<float> {             //__SILP__
        public readonly float MinValue;                                              //__SILP__
        public FloatSpecValueCheckerBigger(float minValue) {                         //__SILP__
            MinValue = minValue;                                                     //__SILP__
        }                                                                            //__SILP__
                                                                                     //__SILP__
        protected override bool IsValid(float val) {                                 //__SILP__
            return val > MinValue;                                                   //__SILP__
        }                                                                            //__SILP__
                                                                                     //__SILP__
        public override bool DoEncode(Data spec) {                                   //__SILP__
            return spec.SetFloat(PropertiesSpecConsts.KeyBigger, MinValue);          //__SILP__
        }                                                                            //__SILP__
    }                                                                                //__SILP__
                                                                                     //__SILP__
    public class FloatSpecValueCheckerBiggerOrEqual : SpecValueChecker<float> {      //__SILP__
        public readonly float MinValue;                                              //__SILP__
        public FloatSpecValueCheckerBiggerOrEqual(float minValue) {                  //__SILP__
            MinValue = minValue;                                                     //__SILP__
        }                                                                            //__SILP__
                                                                                     //__SILP__
        protected override bool IsValid(float val) {                                 //__SILP__
            return val >= MinValue;                                                  //__SILP__
        }                                                                            //__SILP__
                                                                                     //__SILP__
        public override bool DoEncode(Data spec) {                                   //__SILP__
            return spec.SetFloat(PropertiesSpecConsts.KeyBiggerOrEqual, MinValue);   //__SILP__
        }                                                                            //__SILP__
    }                                                                                //__SILP__
                                                                                     //__SILP__
    public class FloatSpecValueCheckerSmaller : SpecValueChecker<float> {            //__SILP__
        public readonly float MaxValue;                                              //__SILP__
        public FloatSpecValueCheckerSmaller(float maxValue) {                        //__SILP__
            MaxValue = maxValue;                                                     //__SILP__
        }                                                                            //__SILP__
                                                                                     //__SILP__
        protected override bool IsValid(float val) {                                 //__SILP__
            return val < MaxValue;                                                   //__SILP__
        }                                                                            //__SILP__
                                                                                     //__SILP__
        public override bool DoEncode(Data spec) {                                   //__SILP__
            return spec.SetFloat(PropertiesSpecConsts.KeySmaller, MaxValue);         //__SILP__
        }                                                                            //__SILP__
    }                                                                                //__SILP__
                                                                                     //__SILP__
    public class FloatSpecValueCheckerSmallerOrEqual : SpecValueChecker<float> {     //__SILP__
        public readonly float MaxValue;                                              //__SILP__
        public FloatSpecValueCheckerSmallerOrEqual(float maxValue) {                 //__SILP__
            MaxValue = maxValue;                                                     //__SILP__
        }                                                                            //__SILP__
                                                                                     //__SILP__
        protected override bool IsValid(float val) {                                 //__SILP__
            return val <= MaxValue;                                                  //__SILP__
        }                                                                            //__SILP__
                                                                                     //__SILP__
        public override bool DoEncode(Data spec) {                                   //__SILP__
            return spec.SetFloat(PropertiesSpecConsts.KeySmallerOrEqual, MaxValue);  //__SILP__
        }                                                                            //__SILP__
    }                                                                                //__SILP__
                                                                                     //__SILP__
    public class FloatSpecValueCheckerIn : SpecValueChecker<float> {                 //__SILP__
        public readonly float[] Values;                                              //__SILP__
        public FloatSpecValueCheckerIn(float[] values) {                             //__SILP__
            Values = values;                                                         //__SILP__
        }                                                                            //__SILP__
                                                                                     //__SILP__
        protected override bool IsValid(float val) {                                 //__SILP__
            foreach (float v in Values) {                                            //__SILP__
                if (v == val) return true;                                           //__SILP__
            }                                                                        //__SILP__
            return false;                                                            //__SILP__
        }                                                                            //__SILP__
                                                                                     //__SILP__
        public override bool DoEncode(Data spec) {                                   //__SILP__
            Data values = new Data();                                                //__SILP__
            for (int i = 0; i < Values.Length; i++) {                                //__SILP__
                float v = Values[i];                                                 //__SILP__
                values.SetFloat(i.ToString(), v);                                    //__SILP__
            }                                                                        //__SILP__
            return spec.SetData(PropertiesSpecConsts.KeyIn, values);                 //__SILP__
        }                                                                            //__SILP__
    }                                                                                //__SILP__
                                                                                     //__SILP__
    public class FloatSpecValueCheckerNotIn : SpecValueChecker<float> {              //__SILP__
        public readonly float[] Values;                                              //__SILP__
        public FloatSpecValueCheckerNotIn(float[] values) {                          //__SILP__
            Values = values;                                                         //__SILP__
        }                                                                            //__SILP__
                                                                                     //__SILP__
        protected override bool IsValid(float val) {                                 //__SILP__
            foreach (float v in Values) {                                            //__SILP__
                if (v == val) return false;                                          //__SILP__
            }                                                                        //__SILP__
            return true;                                                             //__SILP__
        }                                                                            //__SILP__
                                                                                     //__SILP__
        public override bool DoEncode(Data spec) {                                   //__SILP__
            Data values = new Data();                                                //__SILP__
            for (int i = 0; i < Values.Length; i++) {                                //__SILP__
                float v = Values[i];                                                 //__SILP__
                values.SetFloat(i.ToString(), v);                                    //__SILP__
            }                                                                        //__SILP__
            return spec.SetData(PropertiesSpecConsts.KeyNotIn, values);              //__SILP__
        }                                                                            //__SILP__
    }                                                                                //__SILP__
                                                                                     //__SILP__
    //SILP: PROPERTY_SPEC(Double, double)
    public class DoubleSpecValueCheckerBigger : SpecValueChecker<double> {            //__SILP__
        public readonly double MinValue;                                              //__SILP__
        public DoubleSpecValueCheckerBigger(double minValue) {                        //__SILP__
            MinValue = minValue;                                                      //__SILP__
        }                                                                             //__SILP__
                                                                                      //__SILP__
        protected override bool IsValid(double val) {                                 //__SILP__
            return val > MinValue;                                                    //__SILP__
        }                                                                             //__SILP__
                                                                                      //__SILP__
        public override bool DoEncode(Data spec) {                                    //__SILP__
            return spec.SetDouble(PropertiesSpecConsts.KeyBigger, MinValue);          //__SILP__
        }                                                                             //__SILP__
    }                                                                                 //__SILP__
                                                                                      //__SILP__
    public class DoubleSpecValueCheckerBiggerOrEqual : SpecValueChecker<double> {     //__SILP__
        public readonly double MinValue;                                              //__SILP__
        public DoubleSpecValueCheckerBiggerOrEqual(double minValue) {                 //__SILP__
            MinValue = minValue;                                                      //__SILP__
        }                                                                             //__SILP__
                                                                                      //__SILP__
        protected override bool IsValid(double val) {                                 //__SILP__
            return val >= MinValue;                                                   //__SILP__
        }                                                                             //__SILP__
                                                                                      //__SILP__
        public override bool DoEncode(Data spec) {                                    //__SILP__
            return spec.SetDouble(PropertiesSpecConsts.KeyBiggerOrEqual, MinValue);   //__SILP__
        }                                                                             //__SILP__
    }                                                                                 //__SILP__
                                                                                      //__SILP__
    public class DoubleSpecValueCheckerSmaller : SpecValueChecker<double> {           //__SILP__
        public readonly double MaxValue;                                              //__SILP__
        public DoubleSpecValueCheckerSmaller(double maxValue) {                       //__SILP__
            MaxValue = maxValue;                                                      //__SILP__
        }                                                                             //__SILP__
                                                                                      //__SILP__
        protected override bool IsValid(double val) {                                 //__SILP__
            return val < MaxValue;                                                    //__SILP__
        }                                                                             //__SILP__
                                                                                      //__SILP__
        public override bool DoEncode(Data spec) {                                    //__SILP__
            return spec.SetDouble(PropertiesSpecConsts.KeySmaller, MaxValue);         //__SILP__
        }                                                                             //__SILP__
    }                                                                                 //__SILP__
                                                                                      //__SILP__
    public class DoubleSpecValueCheckerSmallerOrEqual : SpecValueChecker<double> {    //__SILP__
        public readonly double MaxValue;                                              //__SILP__
        public DoubleSpecValueCheckerSmallerOrEqual(double maxValue) {                //__SILP__
            MaxValue = maxValue;                                                      //__SILP__
        }                                                                             //__SILP__
                                                                                      //__SILP__
        protected override bool IsValid(double val) {                                 //__SILP__
            return val <= MaxValue;                                                   //__SILP__
        }                                                                             //__SILP__
                                                                                      //__SILP__
        public override bool DoEncode(Data spec) {                                    //__SILP__
            return spec.SetDouble(PropertiesSpecConsts.KeySmallerOrEqual, MaxValue);  //__SILP__
        }                                                                             //__SILP__
    }                                                                                 //__SILP__
                                                                                      //__SILP__
    public class DoubleSpecValueCheckerIn : SpecValueChecker<double> {                //__SILP__
        public readonly double[] Values;                                              //__SILP__
        public DoubleSpecValueCheckerIn(double[] values) {                            //__SILP__
            Values = values;                                                          //__SILP__
        }                                                                             //__SILP__
                                                                                      //__SILP__
        protected override bool IsValid(double val) {                                 //__SILP__
            foreach (double v in Values) {                                            //__SILP__
                if (v == val) return true;                                            //__SILP__
            }                                                                         //__SILP__
            return false;                                                             //__SILP__
        }                                                                             //__SILP__
                                                                                      //__SILP__
        public override bool DoEncode(Data spec) {                                    //__SILP__
            Data values = new Data();                                                 //__SILP__
            for (int i = 0; i < Values.Length; i++) {                                 //__SILP__
                double v = Values[i];                                                 //__SILP__
                values.SetDouble(i.ToString(), v);                                    //__SILP__
            }                                                                         //__SILP__
            return spec.SetData(PropertiesSpecConsts.KeyIn, values);                  //__SILP__
        }                                                                             //__SILP__
    }                                                                                 //__SILP__
                                                                                      //__SILP__
    public class DoubleSpecValueCheckerNotIn : SpecValueChecker<double> {             //__SILP__
        public readonly double[] Values;                                              //__SILP__
        public DoubleSpecValueCheckerNotIn(double[] values) {                         //__SILP__
            Values = values;                                                          //__SILP__
        }                                                                             //__SILP__
                                                                                      //__SILP__
        protected override bool IsValid(double val) {                                 //__SILP__
            foreach (double v in Values) {                                            //__SILP__
                if (v == val) return false;                                           //__SILP__
            }                                                                         //__SILP__
            return true;                                                              //__SILP__
        }                                                                             //__SILP__
                                                                                      //__SILP__
        public override bool DoEncode(Data spec) {                                    //__SILP__
            Data values = new Data();                                                 //__SILP__
            for (int i = 0; i < Values.Length; i++) {                                 //__SILP__
                double v = Values[i];                                                 //__SILP__
                values.SetDouble(i.ToString(), v);                                    //__SILP__
            }                                                                         //__SILP__
            return spec.SetData(PropertiesSpecConsts.KeyNotIn, values);               //__SILP__
        }                                                                             //__SILP__
    }                                                                                 //__SILP__
                                                                                      //__SILP__
    //SILP: PROPERTY_SPEC_IN(String, string)
    public class StringSpecValueCheckerIn : SpecValueChecker<string> {     //__SILP__
        public readonly string[] Values;                                   //__SILP__
        public StringSpecValueCheckerIn(string[] values) {                 //__SILP__
            Values = values;                                               //__SILP__
        }                                                                  //__SILP__
                                                                           //__SILP__
        protected override bool IsValid(string val) {                      //__SILP__
            foreach (string v in Values) {                                 //__SILP__
                if (v == val) return true;                                 //__SILP__
            }                                                              //__SILP__
            return false;                                                  //__SILP__
        }                                                                  //__SILP__
                                                                           //__SILP__
        public override bool DoEncode(Data spec) {                         //__SILP__
            Data values = new Data();                                      //__SILP__
            for (int i = 0; i < Values.Length; i++) {                      //__SILP__
                string v = Values[i];                                      //__SILP__
                values.SetString(i.ToString(), v);                         //__SILP__
            }                                                              //__SILP__
            return spec.SetData(PropertiesSpecConsts.KeyIn, values);       //__SILP__
        }                                                                  //__SILP__
    }                                                                      //__SILP__
                                                                           //__SILP__
    public class StringSpecValueCheckerNotIn : SpecValueChecker<string> {  //__SILP__
        public readonly string[] Values;                                   //__SILP__
        public StringSpecValueCheckerNotIn(string[] values) {              //__SILP__
            Values = values;                                               //__SILP__
        }                                                                  //__SILP__
                                                                           //__SILP__
        protected override bool IsValid(string val) {                      //__SILP__
            foreach (string v in Values) {                                 //__SILP__
                if (v == val) return false;                                //__SILP__
            }                                                              //__SILP__
            return true;                                                   //__SILP__
        }                                                                  //__SILP__
                                                                           //__SILP__
        public override bool DoEncode(Data spec) {                         //__SILP__
            Data values = new Data();                                      //__SILP__
            for (int i = 0; i < Values.Length; i++) {                      //__SILP__
                string v = Values[i];                                      //__SILP__
                values.SetString(i.ToString(), v);                         //__SILP__
            }                                                              //__SILP__
            return spec.SetData(PropertiesSpecConsts.KeyNotIn, values);    //__SILP__
        }                                                                  //__SILP__
    }                                                                      //__SILP__
                                                                           //__SILP__
}
