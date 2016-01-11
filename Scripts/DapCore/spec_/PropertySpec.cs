using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface SpecValueChecker : IValueChecker {
        bool DoEncode(Data spec);
    }

    public abstract class SpecValueChecker<T> : SpecValueChecker, IValueChecker<T> {
        public bool IsValid(Property<T> property, T val, T newVal) {
            return IsValid(newVal);
        }

        protected abstract bool IsValid(T val);
        public abstract bool DoEncode(Data spec);
    }

    //SILP: PROPERTY_SPEC(Int, int)
    public class IntSpecValueCheckerBigger : SpecValueChecker<int> {                                        //__SILP__
        public readonly int MinValue;                                                                       //__SILP__
        public IntSpecValueCheckerBigger(int minValue) {                                                    //__SILP__
            MinValue = minValue;                                                                            //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        protected override bool IsValid(int val) {                                                          //__SILP__
            return val > MinValue;                                                                          //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public override bool DoEncode(Data spec) {                                                          //__SILP__
            return spec.SetInt(SpecConsts.KindBigger, MinValue);                                            //__SILP__
        }                                                                                                   //__SILP__
    }                                                                                                       //__SILP__
                                                                                                            //__SILP__
    public class SubIntSpecValueCheckerBigger<T> : SpecValueChecker<T> {                                    //__SILP__
        public readonly string SubKey;                                                                      //__SILP__
        public readonly int MinValue;                                                                       //__SILP__
        public readonly Func<T, int> ValueGetter;                                                           //__SILP__
        public SubIntSpecValueCheckerBigger(string subKey,                                                  //__SILP__
                            int minValue, Func<T, int> valueGetter) {                                       //__SILP__
            SubKey = subKey;                                                                                //__SILP__
            MinValue = minValue;                                                                            //__SILP__
            ValueGetter = valueGetter;                                                                      //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        protected override bool IsValid(T _val) {                                                           //__SILP__
            int val = ValueGetter(_val);                                                                    //__SILP__
            return val > MinValue;                                                                          //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public override bool DoEncode(Data spec) {                                                          //__SILP__
            return spec.SetInt(SpecConsts.GetSubSpecKey(SubKey, SpecConsts.KindBigger), MinValue);          //__SILP__
        }                                                                                                   //__SILP__
    }                                                                                                       //__SILP__
                                                                                                            //__SILP__
    public class DataIntSpecValueCheckerBigger : SubIntSpecValueCheckerBigger<Data> {                       //__SILP__
        public DataIntSpecValueCheckerBigger(string subKey, int minValue)                                   //__SILP__
                : base(subKey, minValue, (Data val) => val.GetInt(subKey)) {                                //__SILP__
        }                                                                                                   //__SILP__
    }                                                                                                       //__SILP__
                                                                                                            //__SILP__
    public class IntSpecValueCheckerBiggerOrEqual : SpecValueChecker<int> {                                 //__SILP__
        public readonly int MinValue;                                                                       //__SILP__
        public IntSpecValueCheckerBiggerOrEqual(int minValue) {                                             //__SILP__
            MinValue = minValue;                                                                            //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        protected override bool IsValid(int val) {                                                          //__SILP__
            return val >= MinValue;                                                                         //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public override bool DoEncode(Data spec) {                                                          //__SILP__
            return spec.SetInt(SpecConsts.KindBiggerOrEqual, MinValue);                                     //__SILP__
        }                                                                                                   //__SILP__
    }                                                                                                       //__SILP__
                                                                                                            //__SILP__
    public class SubIntSpecValueCheckerBiggerOrEqual<T> : SpecValueChecker<T> {                             //__SILP__
        public readonly string SubKey;                                                                      //__SILP__
        public readonly int MinValue;                                                                       //__SILP__
        public readonly Func<T, int> ValueGetter;                                                           //__SILP__
        public SubIntSpecValueCheckerBiggerOrEqual(string subKey,                                           //__SILP__
                            int minValue, Func<T, int> valueGetter) {                                       //__SILP__
            SubKey = subKey;                                                                                //__SILP__
            MinValue = minValue;                                                                            //__SILP__
            ValueGetter = valueGetter;                                                                      //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        protected override bool IsValid(T _val) {                                                           //__SILP__
            int val = ValueGetter(_val);                                                                    //__SILP__
            return val >= MinValue;                                                                         //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public override bool DoEncode(Data spec) {                                                          //__SILP__
            return spec.SetInt(SpecConsts.GetSubSpecKey(SubKey, SpecConsts.KindBiggerOrEqual), MinValue);   //__SILP__
        }                                                                                                   //__SILP__
    }                                                                                                       //__SILP__
                                                                                                            //__SILP__
    public class DataIntSpecValueCheckerBiggerOrEqual : SubIntSpecValueCheckerBiggerOrEqual<Data> {         //__SILP__
        public DataIntSpecValueCheckerBiggerOrEqual(string subKey, int minValue)                            //__SILP__
                : base(subKey, minValue, (Data val) => val.GetInt(subKey)) {                                //__SILP__
        }                                                                                                   //__SILP__
    }                                                                                                       //__SILP__
                                                                                                            //__SILP__
    public class IntSpecValueCheckerSmaller : SpecValueChecker<int> {                                       //__SILP__
        public readonly int MaxValue;                                                                       //__SILP__
        public IntSpecValueCheckerSmaller(int maxValue) {                                                   //__SILP__
            MaxValue = maxValue;                                                                            //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        protected override bool IsValid(int val) {                                                          //__SILP__
            return val < MaxValue;                                                                          //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public override bool DoEncode(Data spec) {                                                          //__SILP__
            return spec.SetInt(SpecConsts.KindSmaller, MaxValue);                                           //__SILP__
        }                                                                                                   //__SILP__
    }                                                                                                       //__SILP__
                                                                                                            //__SILP__
    public class SubIntSpecValueCheckerSmaller<T> : SpecValueChecker<T> {                                   //__SILP__
        public readonly string SubKey;                                                                      //__SILP__
        public readonly int MaxValue;                                                                       //__SILP__
        public readonly Func<T, int> ValueGetter;                                                           //__SILP__
        public SubIntSpecValueCheckerSmaller(string subKey,                                                 //__SILP__
                            int maxValue, Func<T, int> valueGetter) {                                       //__SILP__
            SubKey = subKey;                                                                                //__SILP__
            MaxValue = maxValue;                                                                            //__SILP__
            ValueGetter = valueGetter;                                                                      //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        protected override bool IsValid(T _val) {                                                           //__SILP__
            int val = ValueGetter(_val);                                                                    //__SILP__
            return val < MaxValue;                                                                          //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public override bool DoEncode(Data spec) {                                                          //__SILP__
            return spec.SetInt(SpecConsts.GetSubSpecKey(SubKey, SpecConsts.KindSmaller), MaxValue);         //__SILP__
        }                                                                                                   //__SILP__
    }                                                                                                       //__SILP__
                                                                                                            //__SILP__
    public class DataIntSpecValueCheckerSmaller : SubIntSpecValueCheckerSmaller<Data> {                     //__SILP__
        public DataIntSpecValueCheckerSmaller(string subKey, int maxValue)                                  //__SILP__
                : base(subKey, maxValue, (Data val) => val.GetInt(subKey)) {                                //__SILP__
        }                                                                                                   //__SILP__
    }                                                                                                       //__SILP__
                                                                                                            //__SILP__
    public class IntSpecValueCheckerSmallerOrEqual : SpecValueChecker<int> {                                //__SILP__
        public readonly int MaxValue;                                                                       //__SILP__
        public IntSpecValueCheckerSmallerOrEqual(int maxValue) {                                            //__SILP__
            MaxValue = maxValue;                                                                            //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        protected override bool IsValid(int val) {                                                          //__SILP__
            return val <= MaxValue;                                                                         //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public override bool DoEncode(Data spec) {                                                          //__SILP__
            return spec.SetInt(SpecConsts.KindSmallerOrEqual, MaxValue);                                    //__SILP__
        }                                                                                                   //__SILP__
    }                                                                                                       //__SILP__
                                                                                                            //__SILP__
    public class SubIntSpecValueCheckerSmallerOrEqual<T> : SpecValueChecker<T> {                            //__SILP__
        public readonly string SubKey;                                                                      //__SILP__
        public readonly int MaxValue;                                                                       //__SILP__
        public readonly Func<T, int> ValueGetter;                                                           //__SILP__
        public SubIntSpecValueCheckerSmallerOrEqual(string subKey,                                          //__SILP__
                            int maxValue, Func<T, int> valueGetter) {                                       //__SILP__
            SubKey = subKey;                                                                                //__SILP__
            MaxValue = maxValue;                                                                            //__SILP__
            ValueGetter = valueGetter;                                                                      //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        protected override bool IsValid(T _val) {                                                           //__SILP__
            int val = ValueGetter(_val);                                                                    //__SILP__
            return val <= MaxValue;                                                                         //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public override bool DoEncode(Data spec) {                                                          //__SILP__
            return spec.SetInt(SpecConsts.GetSubSpecKey(SubKey, SpecConsts.KindSmallerOrEqual), MaxValue);  //__SILP__
        }                                                                                                   //__SILP__
    }                                                                                                       //__SILP__
                                                                                                            //__SILP__
    public class DataIntSpecValueCheckerSmallerOrEqual : SubIntSpecValueCheckerSmallerOrEqual<Data> {       //__SILP__
        public DataIntSpecValueCheckerSmallerOrEqual(string subKey, int maxValue)                           //__SILP__
                : base(subKey, maxValue, (Data val) => val.GetInt(subKey)) {                                //__SILP__
        }                                                                                                   //__SILP__
    }                                                                                                       //__SILP__
                                                                                                            //__SILP__
    //SILP: PROPERTY_SPEC_IN(Int, int)
    public class IntSpecValueCheckerIn : SpecValueChecker<int> {                                  //__SILP__
        public readonly int[] Values;                                                             //__SILP__
        public IntSpecValueCheckerIn(int[] values) {                                              //__SILP__
            Values = values;                                                                      //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        protected override bool IsValid(int val) {                                                //__SILP__
            foreach (int v in Values) {                                                           //__SILP__
                if (v == val) return true;                                                        //__SILP__
            }                                                                                     //__SILP__
            return false;                                                                         //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        public override bool DoEncode(Data spec) {                                                //__SILP__
            Data values = new Data();                                                             //__SILP__
            for (int i = 0; i < Values.Length; i++) {                                             //__SILP__
                int v = Values[i];                                                                //__SILP__
                values.SetInt(i.ToString(), v);                                                   //__SILP__
            }                                                                                     //__SILP__
            return spec.SetData(SpecConsts.KindIn, values);                                       //__SILP__
        }                                                                                         //__SILP__
    }                                                                                             //__SILP__
                                                                                                  //__SILP__
    public class SubIntSpecValueCheckerIn<T> : SpecValueChecker<T> {                              //__SILP__
        public readonly string SubKey;                                                            //__SILP__
        public readonly int[] Values;                                                             //__SILP__
        public readonly Func<T, int> ValueGetter;                                                 //__SILP__
        public SubIntSpecValueCheckerIn(string subKey,                                            //__SILP__
                                        int[] values, Func<T, int> valueGetter) {                 //__SILP__
            SubKey = subKey;                                                                      //__SILP__
            Values = values;                                                                      //__SILP__
            ValueGetter = valueGetter;                                                            //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        protected override bool IsValid(T _val) {                                                 //__SILP__
            int val = ValueGetter(_val);                                                          //__SILP__
            foreach (int v in Values) {                                                           //__SILP__
                if (v == val) return true;                                                        //__SILP__
            }                                                                                     //__SILP__
            return false;                                                                         //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        public override bool DoEncode(Data spec) {                                                //__SILP__
            Data values = new Data();                                                             //__SILP__
            for (int i = 0; i < Values.Length; i++) {                                             //__SILP__
                int v = Values[i];                                                                //__SILP__
                values.SetInt(i.ToString(), v);                                                   //__SILP__
            }                                                                                     //__SILP__
            return spec.SetData(SpecConsts.GetSubSpecKey(SubKey, SpecConsts.KindIn), values);     //__SILP__
        }                                                                                         //__SILP__
    }                                                                                             //__SILP__
                                                                                                  //__SILP__
    public class DataIntSpecValueCheckerIn : SubIntSpecValueCheckerIn<Data> {                     //__SILP__
        public DataIntSpecValueCheckerIn(string subKey, int[] values)                             //__SILP__
                : base(subKey, values, (Data val) => val.GetInt(subKey)) {                        //__SILP__
        }                                                                                         //__SILP__
    }                                                                                             //__SILP__
                                                                                                  //__SILP__
    public class IntSpecValueCheckerNotIn : SpecValueChecker<int> {                               //__SILP__
        public readonly int[] Values;                                                             //__SILP__
        public IntSpecValueCheckerNotIn(int[] values) {                                           //__SILP__
            Values = values;                                                                      //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        protected override bool IsValid(int val) {                                                //__SILP__
            foreach (int v in Values) {                                                           //__SILP__
                if (v == val) return false;                                                       //__SILP__
            }                                                                                     //__SILP__
            return true;                                                                          //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        public override bool DoEncode(Data spec) {                                                //__SILP__
            Data values = new Data();                                                             //__SILP__
            for (int i = 0; i < Values.Length; i++) {                                             //__SILP__
                int v = Values[i];                                                                //__SILP__
                values.SetInt(i.ToString(), v);                                                   //__SILP__
            }                                                                                     //__SILP__
            return spec.SetData(SpecConsts.KindNotIn, values);                                    //__SILP__
        }                                                                                         //__SILP__
    }                                                                                             //__SILP__
                                                                                                  //__SILP__
    public class SubIntSpecValueCheckerNotIn<T> : SpecValueChecker<T> {                           //__SILP__
        public readonly string SubKey;                                                            //__SILP__
        public readonly int[] Values;                                                             //__SILP__
        public readonly Func<T, int> ValueGetter;                                                 //__SILP__
        public SubIntSpecValueCheckerNotIn(string subKey,                                         //__SILP__
                                        int[] values, Func<T, int> valueGetter) {                 //__SILP__
            SubKey = subKey;                                                                      //__SILP__
            Values = values;                                                                      //__SILP__
            ValueGetter = valueGetter;                                                            //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        protected override bool IsValid(T _val) {                                                 //__SILP__
            int val = ValueGetter(_val);                                                          //__SILP__
            foreach (int v in Values) {                                                           //__SILP__
                if (v == val) return false;                                                       //__SILP__
            }                                                                                     //__SILP__
            return true;                                                                          //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        public override bool DoEncode(Data spec) {                                                //__SILP__
            Data values = new Data();                                                             //__SILP__
            for (int i = 0; i < Values.Length; i++) {                                             //__SILP__
                int v = Values[i];                                                                //__SILP__
                values.SetInt(i.ToString(), v);                                                   //__SILP__
            }                                                                                     //__SILP__
            return spec.SetData(SpecConsts.GetSubSpecKey(SubKey, SpecConsts.KindNotIn), values);  //__SILP__
        }                                                                                         //__SILP__
    }                                                                                             //__SILP__
                                                                                                  //__SILP__
    public class DataIntSpecValueCheckerNotIn : SubIntSpecValueCheckerNotIn<Data> {               //__SILP__
        public DataIntSpecValueCheckerNotIn(string subKey, int[] values)                          //__SILP__
                : base(subKey, values, (Data val) => val.GetInt(subKey)) {                        //__SILP__
        }                                                                                         //__SILP__
    }                                                                                             //__SILP__
                                                                                                  //__SILP__
    //SILP: PROPERTY_SPEC(Long, long)
    public class LongSpecValueCheckerBigger : SpecValueChecker<long> {                                       //__SILP__
        public readonly long MinValue;                                                                       //__SILP__
        public LongSpecValueCheckerBigger(long minValue) {                                                   //__SILP__
            MinValue = minValue;                                                                             //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        protected override bool IsValid(long val) {                                                          //__SILP__
            return val > MinValue;                                                                           //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        public override bool DoEncode(Data spec) {                                                           //__SILP__
            return spec.SetLong(SpecConsts.KindBigger, MinValue);                                            //__SILP__
        }                                                                                                    //__SILP__
    }                                                                                                        //__SILP__
                                                                                                             //__SILP__
    public class SubLongSpecValueCheckerBigger<T> : SpecValueChecker<T> {                                    //__SILP__
        public readonly string SubKey;                                                                       //__SILP__
        public readonly long MinValue;                                                                       //__SILP__
        public readonly Func<T, long> ValueGetter;                                                           //__SILP__
        public SubLongSpecValueCheckerBigger(string subKey,                                                  //__SILP__
                            long minValue, Func<T, long> valueGetter) {                                      //__SILP__
            SubKey = subKey;                                                                                 //__SILP__
            MinValue = minValue;                                                                             //__SILP__
            ValueGetter = valueGetter;                                                                       //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        protected override bool IsValid(T _val) {                                                            //__SILP__
            long val = ValueGetter(_val);                                                                    //__SILP__
            return val > MinValue;                                                                           //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        public override bool DoEncode(Data spec) {                                                           //__SILP__
            return spec.SetLong(SpecConsts.GetSubSpecKey(SubKey, SpecConsts.KindBigger), MinValue);          //__SILP__
        }                                                                                                    //__SILP__
    }                                                                                                        //__SILP__
                                                                                                             //__SILP__
    public class DataLongSpecValueCheckerBigger : SubLongSpecValueCheckerBigger<Data> {                      //__SILP__
        public DataLongSpecValueCheckerBigger(string subKey, long minValue)                                  //__SILP__
                : base(subKey, minValue, (Data val) => val.GetLong(subKey)) {                                //__SILP__
        }                                                                                                    //__SILP__
    }                                                                                                        //__SILP__
                                                                                                             //__SILP__
    public class LongSpecValueCheckerBiggerOrEqual : SpecValueChecker<long> {                                //__SILP__
        public readonly long MinValue;                                                                       //__SILP__
        public LongSpecValueCheckerBiggerOrEqual(long minValue) {                                            //__SILP__
            MinValue = minValue;                                                                             //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        protected override bool IsValid(long val) {                                                          //__SILP__
            return val >= MinValue;                                                                          //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        public override bool DoEncode(Data spec) {                                                           //__SILP__
            return spec.SetLong(SpecConsts.KindBiggerOrEqual, MinValue);                                     //__SILP__
        }                                                                                                    //__SILP__
    }                                                                                                        //__SILP__
                                                                                                             //__SILP__
    public class SubLongSpecValueCheckerBiggerOrEqual<T> : SpecValueChecker<T> {                             //__SILP__
        public readonly string SubKey;                                                                       //__SILP__
        public readonly long MinValue;                                                                       //__SILP__
        public readonly Func<T, long> ValueGetter;                                                           //__SILP__
        public SubLongSpecValueCheckerBiggerOrEqual(string subKey,                                           //__SILP__
                            long minValue, Func<T, long> valueGetter) {                                      //__SILP__
            SubKey = subKey;                                                                                 //__SILP__
            MinValue = minValue;                                                                             //__SILP__
            ValueGetter = valueGetter;                                                                       //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        protected override bool IsValid(T _val) {                                                            //__SILP__
            long val = ValueGetter(_val);                                                                    //__SILP__
            return val >= MinValue;                                                                          //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        public override bool DoEncode(Data spec) {                                                           //__SILP__
            return spec.SetLong(SpecConsts.GetSubSpecKey(SubKey, SpecConsts.KindBiggerOrEqual), MinValue);   //__SILP__
        }                                                                                                    //__SILP__
    }                                                                                                        //__SILP__
                                                                                                             //__SILP__
    public class DataLongSpecValueCheckerBiggerOrEqual : SubLongSpecValueCheckerBiggerOrEqual<Data> {        //__SILP__
        public DataLongSpecValueCheckerBiggerOrEqual(string subKey, long minValue)                           //__SILP__
                : base(subKey, minValue, (Data val) => val.GetLong(subKey)) {                                //__SILP__
        }                                                                                                    //__SILP__
    }                                                                                                        //__SILP__
                                                                                                             //__SILP__
    public class LongSpecValueCheckerSmaller : SpecValueChecker<long> {                                      //__SILP__
        public readonly long MaxValue;                                                                       //__SILP__
        public LongSpecValueCheckerSmaller(long maxValue) {                                                  //__SILP__
            MaxValue = maxValue;                                                                             //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        protected override bool IsValid(long val) {                                                          //__SILP__
            return val < MaxValue;                                                                           //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        public override bool DoEncode(Data spec) {                                                           //__SILP__
            return spec.SetLong(SpecConsts.KindSmaller, MaxValue);                                           //__SILP__
        }                                                                                                    //__SILP__
    }                                                                                                        //__SILP__
                                                                                                             //__SILP__
    public class SubLongSpecValueCheckerSmaller<T> : SpecValueChecker<T> {                                   //__SILP__
        public readonly string SubKey;                                                                       //__SILP__
        public readonly long MaxValue;                                                                       //__SILP__
        public readonly Func<T, long> ValueGetter;                                                           //__SILP__
        public SubLongSpecValueCheckerSmaller(string subKey,                                                 //__SILP__
                            long maxValue, Func<T, long> valueGetter) {                                      //__SILP__
            SubKey = subKey;                                                                                 //__SILP__
            MaxValue = maxValue;                                                                             //__SILP__
            ValueGetter = valueGetter;                                                                       //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        protected override bool IsValid(T _val) {                                                            //__SILP__
            long val = ValueGetter(_val);                                                                    //__SILP__
            return val < MaxValue;                                                                           //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        public override bool DoEncode(Data spec) {                                                           //__SILP__
            return spec.SetLong(SpecConsts.GetSubSpecKey(SubKey, SpecConsts.KindSmaller), MaxValue);         //__SILP__
        }                                                                                                    //__SILP__
    }                                                                                                        //__SILP__
                                                                                                             //__SILP__
    public class DataLongSpecValueCheckerSmaller : SubLongSpecValueCheckerSmaller<Data> {                    //__SILP__
        public DataLongSpecValueCheckerSmaller(string subKey, long maxValue)                                 //__SILP__
                : base(subKey, maxValue, (Data val) => val.GetLong(subKey)) {                                //__SILP__
        }                                                                                                    //__SILP__
    }                                                                                                        //__SILP__
                                                                                                             //__SILP__
    public class LongSpecValueCheckerSmallerOrEqual : SpecValueChecker<long> {                               //__SILP__
        public readonly long MaxValue;                                                                       //__SILP__
        public LongSpecValueCheckerSmallerOrEqual(long maxValue) {                                           //__SILP__
            MaxValue = maxValue;                                                                             //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        protected override bool IsValid(long val) {                                                          //__SILP__
            return val <= MaxValue;                                                                          //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        public override bool DoEncode(Data spec) {                                                           //__SILP__
            return spec.SetLong(SpecConsts.KindSmallerOrEqual, MaxValue);                                    //__SILP__
        }                                                                                                    //__SILP__
    }                                                                                                        //__SILP__
                                                                                                             //__SILP__
    public class SubLongSpecValueCheckerSmallerOrEqual<T> : SpecValueChecker<T> {                            //__SILP__
        public readonly string SubKey;                                                                       //__SILP__
        public readonly long MaxValue;                                                                       //__SILP__
        public readonly Func<T, long> ValueGetter;                                                           //__SILP__
        public SubLongSpecValueCheckerSmallerOrEqual(string subKey,                                          //__SILP__
                            long maxValue, Func<T, long> valueGetter) {                                      //__SILP__
            SubKey = subKey;                                                                                 //__SILP__
            MaxValue = maxValue;                                                                             //__SILP__
            ValueGetter = valueGetter;                                                                       //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        protected override bool IsValid(T _val) {                                                            //__SILP__
            long val = ValueGetter(_val);                                                                    //__SILP__
            return val <= MaxValue;                                                                          //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        public override bool DoEncode(Data spec) {                                                           //__SILP__
            return spec.SetLong(SpecConsts.GetSubSpecKey(SubKey, SpecConsts.KindSmallerOrEqual), MaxValue);  //__SILP__
        }                                                                                                    //__SILP__
    }                                                                                                        //__SILP__
                                                                                                             //__SILP__
    public class DataLongSpecValueCheckerSmallerOrEqual : SubLongSpecValueCheckerSmallerOrEqual<Data> {      //__SILP__
        public DataLongSpecValueCheckerSmallerOrEqual(string subKey, long maxValue)                          //__SILP__
                : base(subKey, maxValue, (Data val) => val.GetLong(subKey)) {                                //__SILP__
        }                                                                                                    //__SILP__
    }                                                                                                        //__SILP__
                                                                                                             //__SILP__
    //SILP: PROPERTY_SPEC_IN(Long, long)
    public class LongSpecValueCheckerIn : SpecValueChecker<long> {                                //__SILP__
        public readonly long[] Values;                                                            //__SILP__
        public LongSpecValueCheckerIn(long[] values) {                                            //__SILP__
            Values = values;                                                                      //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        protected override bool IsValid(long val) {                                               //__SILP__
            foreach (long v in Values) {                                                          //__SILP__
                if (v == val) return true;                                                        //__SILP__
            }                                                                                     //__SILP__
            return false;                                                                         //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        public override bool DoEncode(Data spec) {                                                //__SILP__
            Data values = new Data();                                                             //__SILP__
            for (int i = 0; i < Values.Length; i++) {                                             //__SILP__
                long v = Values[i];                                                               //__SILP__
                values.SetLong(i.ToString(), v);                                                  //__SILP__
            }                                                                                     //__SILP__
            return spec.SetData(SpecConsts.KindIn, values);                                       //__SILP__
        }                                                                                         //__SILP__
    }                                                                                             //__SILP__
                                                                                                  //__SILP__
    public class SubLongSpecValueCheckerIn<T> : SpecValueChecker<T> {                             //__SILP__
        public readonly string SubKey;                                                            //__SILP__
        public readonly long[] Values;                                                            //__SILP__
        public readonly Func<T, long> ValueGetter;                                                //__SILP__
        public SubLongSpecValueCheckerIn(string subKey,                                           //__SILP__
                                        long[] values, Func<T, long> valueGetter) {               //__SILP__
            SubKey = subKey;                                                                      //__SILP__
            Values = values;                                                                      //__SILP__
            ValueGetter = valueGetter;                                                            //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        protected override bool IsValid(T _val) {                                                 //__SILP__
            long val = ValueGetter(_val);                                                         //__SILP__
            foreach (long v in Values) {                                                          //__SILP__
                if (v == val) return true;                                                        //__SILP__
            }                                                                                     //__SILP__
            return false;                                                                         //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        public override bool DoEncode(Data spec) {                                                //__SILP__
            Data values = new Data();                                                             //__SILP__
            for (int i = 0; i < Values.Length; i++) {                                             //__SILP__
                long v = Values[i];                                                               //__SILP__
                values.SetLong(i.ToString(), v);                                                  //__SILP__
            }                                                                                     //__SILP__
            return spec.SetData(SpecConsts.GetSubSpecKey(SubKey, SpecConsts.KindIn), values);     //__SILP__
        }                                                                                         //__SILP__
    }                                                                                             //__SILP__
                                                                                                  //__SILP__
    public class DataLongSpecValueCheckerIn : SubLongSpecValueCheckerIn<Data> {                   //__SILP__
        public DataLongSpecValueCheckerIn(string subKey, long[] values)                           //__SILP__
                : base(subKey, values, (Data val) => val.GetLong(subKey)) {                       //__SILP__
        }                                                                                         //__SILP__
    }                                                                                             //__SILP__
                                                                                                  //__SILP__
    public class LongSpecValueCheckerNotIn : SpecValueChecker<long> {                             //__SILP__
        public readonly long[] Values;                                                            //__SILP__
        public LongSpecValueCheckerNotIn(long[] values) {                                         //__SILP__
            Values = values;                                                                      //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        protected override bool IsValid(long val) {                                               //__SILP__
            foreach (long v in Values) {                                                          //__SILP__
                if (v == val) return false;                                                       //__SILP__
            }                                                                                     //__SILP__
            return true;                                                                          //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        public override bool DoEncode(Data spec) {                                                //__SILP__
            Data values = new Data();                                                             //__SILP__
            for (int i = 0; i < Values.Length; i++) {                                             //__SILP__
                long v = Values[i];                                                               //__SILP__
                values.SetLong(i.ToString(), v);                                                  //__SILP__
            }                                                                                     //__SILP__
            return spec.SetData(SpecConsts.KindNotIn, values);                                    //__SILP__
        }                                                                                         //__SILP__
    }                                                                                             //__SILP__
                                                                                                  //__SILP__
    public class SubLongSpecValueCheckerNotIn<T> : SpecValueChecker<T> {                          //__SILP__
        public readonly string SubKey;                                                            //__SILP__
        public readonly long[] Values;                                                            //__SILP__
        public readonly Func<T, long> ValueGetter;                                                //__SILP__
        public SubLongSpecValueCheckerNotIn(string subKey,                                        //__SILP__
                                        long[] values, Func<T, long> valueGetter) {               //__SILP__
            SubKey = subKey;                                                                      //__SILP__
            Values = values;                                                                      //__SILP__
            ValueGetter = valueGetter;                                                            //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        protected override bool IsValid(T _val) {                                                 //__SILP__
            long val = ValueGetter(_val);                                                         //__SILP__
            foreach (long v in Values) {                                                          //__SILP__
                if (v == val) return false;                                                       //__SILP__
            }                                                                                     //__SILP__
            return true;                                                                          //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        public override bool DoEncode(Data spec) {                                                //__SILP__
            Data values = new Data();                                                             //__SILP__
            for (int i = 0; i < Values.Length; i++) {                                             //__SILP__
                long v = Values[i];                                                               //__SILP__
                values.SetLong(i.ToString(), v);                                                  //__SILP__
            }                                                                                     //__SILP__
            return spec.SetData(SpecConsts.GetSubSpecKey(SubKey, SpecConsts.KindNotIn), values);  //__SILP__
        }                                                                                         //__SILP__
    }                                                                                             //__SILP__
                                                                                                  //__SILP__
    public class DataLongSpecValueCheckerNotIn : SubLongSpecValueCheckerNotIn<Data> {             //__SILP__
        public DataLongSpecValueCheckerNotIn(string subKey, long[] values)                        //__SILP__
                : base(subKey, values, (Data val) => val.GetLong(subKey)) {                       //__SILP__
        }                                                                                         //__SILP__
    }                                                                                             //__SILP__
                                                                                                  //__SILP__
    //SILP: PROPERTY_SPEC(Float, float)
    public class FloatSpecValueCheckerBigger : SpecValueChecker<float> {                                      //__SILP__
        public readonly float MinValue;                                                                       //__SILP__
        public FloatSpecValueCheckerBigger(float minValue) {                                                  //__SILP__
            MinValue = minValue;                                                                              //__SILP__
        }                                                                                                     //__SILP__
                                                                                                              //__SILP__
        protected override bool IsValid(float val) {                                                          //__SILP__
            return val > MinValue;                                                                            //__SILP__
        }                                                                                                     //__SILP__
                                                                                                              //__SILP__
        public override bool DoEncode(Data spec) {                                                            //__SILP__
            return spec.SetFloat(SpecConsts.KindBigger, MinValue);                                            //__SILP__
        }                                                                                                     //__SILP__
    }                                                                                                         //__SILP__
                                                                                                              //__SILP__
    public class SubFloatSpecValueCheckerBigger<T> : SpecValueChecker<T> {                                    //__SILP__
        public readonly string SubKey;                                                                        //__SILP__
        public readonly float MinValue;                                                                       //__SILP__
        public readonly Func<T, float> ValueGetter;                                                           //__SILP__
        public SubFloatSpecValueCheckerBigger(string subKey,                                                  //__SILP__
                            float minValue, Func<T, float> valueGetter) {                                     //__SILP__
            SubKey = subKey;                                                                                  //__SILP__
            MinValue = minValue;                                                                              //__SILP__
            ValueGetter = valueGetter;                                                                        //__SILP__
        }                                                                                                     //__SILP__
                                                                                                              //__SILP__
        protected override bool IsValid(T _val) {                                                             //__SILP__
            float val = ValueGetter(_val);                                                                    //__SILP__
            return val > MinValue;                                                                            //__SILP__
        }                                                                                                     //__SILP__
                                                                                                              //__SILP__
        public override bool DoEncode(Data spec) {                                                            //__SILP__
            return spec.SetFloat(SpecConsts.GetSubSpecKey(SubKey, SpecConsts.KindBigger), MinValue);          //__SILP__
        }                                                                                                     //__SILP__
    }                                                                                                         //__SILP__
                                                                                                              //__SILP__
    public class DataFloatSpecValueCheckerBigger : SubFloatSpecValueCheckerBigger<Data> {                     //__SILP__
        public DataFloatSpecValueCheckerBigger(string subKey, float minValue)                                 //__SILP__
                : base(subKey, minValue, (Data val) => val.GetFloat(subKey)) {                                //__SILP__
        }                                                                                                     //__SILP__
    }                                                                                                         //__SILP__
                                                                                                              //__SILP__
    public class FloatSpecValueCheckerBiggerOrEqual : SpecValueChecker<float> {                               //__SILP__
        public readonly float MinValue;                                                                       //__SILP__
        public FloatSpecValueCheckerBiggerOrEqual(float minValue) {                                           //__SILP__
            MinValue = minValue;                                                                              //__SILP__
        }                                                                                                     //__SILP__
                                                                                                              //__SILP__
        protected override bool IsValid(float val) {                                                          //__SILP__
            return val >= MinValue;                                                                           //__SILP__
        }                                                                                                     //__SILP__
                                                                                                              //__SILP__
        public override bool DoEncode(Data spec) {                                                            //__SILP__
            return spec.SetFloat(SpecConsts.KindBiggerOrEqual, MinValue);                                     //__SILP__
        }                                                                                                     //__SILP__
    }                                                                                                         //__SILP__
                                                                                                              //__SILP__
    public class SubFloatSpecValueCheckerBiggerOrEqual<T> : SpecValueChecker<T> {                             //__SILP__
        public readonly string SubKey;                                                                        //__SILP__
        public readonly float MinValue;                                                                       //__SILP__
        public readonly Func<T, float> ValueGetter;                                                           //__SILP__
        public SubFloatSpecValueCheckerBiggerOrEqual(string subKey,                                           //__SILP__
                            float minValue, Func<T, float> valueGetter) {                                     //__SILP__
            SubKey = subKey;                                                                                  //__SILP__
            MinValue = minValue;                                                                              //__SILP__
            ValueGetter = valueGetter;                                                                        //__SILP__
        }                                                                                                     //__SILP__
                                                                                                              //__SILP__
        protected override bool IsValid(T _val) {                                                             //__SILP__
            float val = ValueGetter(_val);                                                                    //__SILP__
            return val >= MinValue;                                                                           //__SILP__
        }                                                                                                     //__SILP__
                                                                                                              //__SILP__
        public override bool DoEncode(Data spec) {                                                            //__SILP__
            return spec.SetFloat(SpecConsts.GetSubSpecKey(SubKey, SpecConsts.KindBiggerOrEqual), MinValue);   //__SILP__
        }                                                                                                     //__SILP__
    }                                                                                                         //__SILP__
                                                                                                              //__SILP__
    public class DataFloatSpecValueCheckerBiggerOrEqual : SubFloatSpecValueCheckerBiggerOrEqual<Data> {       //__SILP__
        public DataFloatSpecValueCheckerBiggerOrEqual(string subKey, float minValue)                          //__SILP__
                : base(subKey, minValue, (Data val) => val.GetFloat(subKey)) {                                //__SILP__
        }                                                                                                     //__SILP__
    }                                                                                                         //__SILP__
                                                                                                              //__SILP__
    public class FloatSpecValueCheckerSmaller : SpecValueChecker<float> {                                     //__SILP__
        public readonly float MaxValue;                                                                       //__SILP__
        public FloatSpecValueCheckerSmaller(float maxValue) {                                                 //__SILP__
            MaxValue = maxValue;                                                                              //__SILP__
        }                                                                                                     //__SILP__
                                                                                                              //__SILP__
        protected override bool IsValid(float val) {                                                          //__SILP__
            return val < MaxValue;                                                                            //__SILP__
        }                                                                                                     //__SILP__
                                                                                                              //__SILP__
        public override bool DoEncode(Data spec) {                                                            //__SILP__
            return spec.SetFloat(SpecConsts.KindSmaller, MaxValue);                                           //__SILP__
        }                                                                                                     //__SILP__
    }                                                                                                         //__SILP__
                                                                                                              //__SILP__
    public class SubFloatSpecValueCheckerSmaller<T> : SpecValueChecker<T> {                                   //__SILP__
        public readonly string SubKey;                                                                        //__SILP__
        public readonly float MaxValue;                                                                       //__SILP__
        public readonly Func<T, float> ValueGetter;                                                           //__SILP__
        public SubFloatSpecValueCheckerSmaller(string subKey,                                                 //__SILP__
                            float maxValue, Func<T, float> valueGetter) {                                     //__SILP__
            SubKey = subKey;                                                                                  //__SILP__
            MaxValue = maxValue;                                                                              //__SILP__
            ValueGetter = valueGetter;                                                                        //__SILP__
        }                                                                                                     //__SILP__
                                                                                                              //__SILP__
        protected override bool IsValid(T _val) {                                                             //__SILP__
            float val = ValueGetter(_val);                                                                    //__SILP__
            return val < MaxValue;                                                                            //__SILP__
        }                                                                                                     //__SILP__
                                                                                                              //__SILP__
        public override bool DoEncode(Data spec) {                                                            //__SILP__
            return spec.SetFloat(SpecConsts.GetSubSpecKey(SubKey, SpecConsts.KindSmaller), MaxValue);         //__SILP__
        }                                                                                                     //__SILP__
    }                                                                                                         //__SILP__
                                                                                                              //__SILP__
    public class DataFloatSpecValueCheckerSmaller : SubFloatSpecValueCheckerSmaller<Data> {                   //__SILP__
        public DataFloatSpecValueCheckerSmaller(string subKey, float maxValue)                                //__SILP__
                : base(subKey, maxValue, (Data val) => val.GetFloat(subKey)) {                                //__SILP__
        }                                                                                                     //__SILP__
    }                                                                                                         //__SILP__
                                                                                                              //__SILP__
    public class FloatSpecValueCheckerSmallerOrEqual : SpecValueChecker<float> {                              //__SILP__
        public readonly float MaxValue;                                                                       //__SILP__
        public FloatSpecValueCheckerSmallerOrEqual(float maxValue) {                                          //__SILP__
            MaxValue = maxValue;                                                                              //__SILP__
        }                                                                                                     //__SILP__
                                                                                                              //__SILP__
        protected override bool IsValid(float val) {                                                          //__SILP__
            return val <= MaxValue;                                                                           //__SILP__
        }                                                                                                     //__SILP__
                                                                                                              //__SILP__
        public override bool DoEncode(Data spec) {                                                            //__SILP__
            return spec.SetFloat(SpecConsts.KindSmallerOrEqual, MaxValue);                                    //__SILP__
        }                                                                                                     //__SILP__
    }                                                                                                         //__SILP__
                                                                                                              //__SILP__
    public class SubFloatSpecValueCheckerSmallerOrEqual<T> : SpecValueChecker<T> {                            //__SILP__
        public readonly string SubKey;                                                                        //__SILP__
        public readonly float MaxValue;                                                                       //__SILP__
        public readonly Func<T, float> ValueGetter;                                                           //__SILP__
        public SubFloatSpecValueCheckerSmallerOrEqual(string subKey,                                          //__SILP__
                            float maxValue, Func<T, float> valueGetter) {                                     //__SILP__
            SubKey = subKey;                                                                                  //__SILP__
            MaxValue = maxValue;                                                                              //__SILP__
            ValueGetter = valueGetter;                                                                        //__SILP__
        }                                                                                                     //__SILP__
                                                                                                              //__SILP__
        protected override bool IsValid(T _val) {                                                             //__SILP__
            float val = ValueGetter(_val);                                                                    //__SILP__
            return val <= MaxValue;                                                                           //__SILP__
        }                                                                                                     //__SILP__
                                                                                                              //__SILP__
        public override bool DoEncode(Data spec) {                                                            //__SILP__
            return spec.SetFloat(SpecConsts.GetSubSpecKey(SubKey, SpecConsts.KindSmallerOrEqual), MaxValue);  //__SILP__
        }                                                                                                     //__SILP__
    }                                                                                                         //__SILP__
                                                                                                              //__SILP__
    public class DataFloatSpecValueCheckerSmallerOrEqual : SubFloatSpecValueCheckerSmallerOrEqual<Data> {     //__SILP__
        public DataFloatSpecValueCheckerSmallerOrEqual(string subKey, float maxValue)                         //__SILP__
                : base(subKey, maxValue, (Data val) => val.GetFloat(subKey)) {                                //__SILP__
        }                                                                                                     //__SILP__
    }                                                                                                         //__SILP__
                                                                                                              //__SILP__
    //SILP: PROPERTY_SPEC(Double, double)
    public class DoubleSpecValueCheckerBigger : SpecValueChecker<double> {                                     //__SILP__
        public readonly double MinValue;                                                                       //__SILP__
        public DoubleSpecValueCheckerBigger(double minValue) {                                                 //__SILP__
            MinValue = minValue;                                                                               //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        protected override bool IsValid(double val) {                                                          //__SILP__
            return val > MinValue;                                                                             //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public override bool DoEncode(Data spec) {                                                             //__SILP__
            return spec.SetDouble(SpecConsts.KindBigger, MinValue);                                            //__SILP__
        }                                                                                                      //__SILP__
    }                                                                                                          //__SILP__
                                                                                                               //__SILP__
    public class SubDoubleSpecValueCheckerBigger<T> : SpecValueChecker<T> {                                    //__SILP__
        public readonly string SubKey;                                                                         //__SILP__
        public readonly double MinValue;                                                                       //__SILP__
        public readonly Func<T, double> ValueGetter;                                                           //__SILP__
        public SubDoubleSpecValueCheckerBigger(string subKey,                                                  //__SILP__
                            double minValue, Func<T, double> valueGetter) {                                    //__SILP__
            SubKey = subKey;                                                                                   //__SILP__
            MinValue = minValue;                                                                               //__SILP__
            ValueGetter = valueGetter;                                                                         //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        protected override bool IsValid(T _val) {                                                              //__SILP__
            double val = ValueGetter(_val);                                                                    //__SILP__
            return val > MinValue;                                                                             //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public override bool DoEncode(Data spec) {                                                             //__SILP__
            return spec.SetDouble(SpecConsts.GetSubSpecKey(SubKey, SpecConsts.KindBigger), MinValue);          //__SILP__
        }                                                                                                      //__SILP__
    }                                                                                                          //__SILP__
                                                                                                               //__SILP__
    public class DataDoubleSpecValueCheckerBigger : SubDoubleSpecValueCheckerBigger<Data> {                    //__SILP__
        public DataDoubleSpecValueCheckerBigger(string subKey, double minValue)                                //__SILP__
                : base(subKey, minValue, (Data val) => val.GetDouble(subKey)) {                                //__SILP__
        }                                                                                                      //__SILP__
    }                                                                                                          //__SILP__
                                                                                                               //__SILP__
    public class DoubleSpecValueCheckerBiggerOrEqual : SpecValueChecker<double> {                              //__SILP__
        public readonly double MinValue;                                                                       //__SILP__
        public DoubleSpecValueCheckerBiggerOrEqual(double minValue) {                                          //__SILP__
            MinValue = minValue;                                                                               //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        protected override bool IsValid(double val) {                                                          //__SILP__
            return val >= MinValue;                                                                            //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public override bool DoEncode(Data spec) {                                                             //__SILP__
            return spec.SetDouble(SpecConsts.KindBiggerOrEqual, MinValue);                                     //__SILP__
        }                                                                                                      //__SILP__
    }                                                                                                          //__SILP__
                                                                                                               //__SILP__
    public class SubDoubleSpecValueCheckerBiggerOrEqual<T> : SpecValueChecker<T> {                             //__SILP__
        public readonly string SubKey;                                                                         //__SILP__
        public readonly double MinValue;                                                                       //__SILP__
        public readonly Func<T, double> ValueGetter;                                                           //__SILP__
        public SubDoubleSpecValueCheckerBiggerOrEqual(string subKey,                                           //__SILP__
                            double minValue, Func<T, double> valueGetter) {                                    //__SILP__
            SubKey = subKey;                                                                                   //__SILP__
            MinValue = minValue;                                                                               //__SILP__
            ValueGetter = valueGetter;                                                                         //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        protected override bool IsValid(T _val) {                                                              //__SILP__
            double val = ValueGetter(_val);                                                                    //__SILP__
            return val >= MinValue;                                                                            //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public override bool DoEncode(Data spec) {                                                             //__SILP__
            return spec.SetDouble(SpecConsts.GetSubSpecKey(SubKey, SpecConsts.KindBiggerOrEqual), MinValue);   //__SILP__
        }                                                                                                      //__SILP__
    }                                                                                                          //__SILP__
                                                                                                               //__SILP__
    public class DataDoubleSpecValueCheckerBiggerOrEqual : SubDoubleSpecValueCheckerBiggerOrEqual<Data> {      //__SILP__
        public DataDoubleSpecValueCheckerBiggerOrEqual(string subKey, double minValue)                         //__SILP__
                : base(subKey, minValue, (Data val) => val.GetDouble(subKey)) {                                //__SILP__
        }                                                                                                      //__SILP__
    }                                                                                                          //__SILP__
                                                                                                               //__SILP__
    public class DoubleSpecValueCheckerSmaller : SpecValueChecker<double> {                                    //__SILP__
        public readonly double MaxValue;                                                                       //__SILP__
        public DoubleSpecValueCheckerSmaller(double maxValue) {                                                //__SILP__
            MaxValue = maxValue;                                                                               //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        protected override bool IsValid(double val) {                                                          //__SILP__
            return val < MaxValue;                                                                             //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public override bool DoEncode(Data spec) {                                                             //__SILP__
            return spec.SetDouble(SpecConsts.KindSmaller, MaxValue);                                           //__SILP__
        }                                                                                                      //__SILP__
    }                                                                                                          //__SILP__
                                                                                                               //__SILP__
    public class SubDoubleSpecValueCheckerSmaller<T> : SpecValueChecker<T> {                                   //__SILP__
        public readonly string SubKey;                                                                         //__SILP__
        public readonly double MaxValue;                                                                       //__SILP__
        public readonly Func<T, double> ValueGetter;                                                           //__SILP__
        public SubDoubleSpecValueCheckerSmaller(string subKey,                                                 //__SILP__
                            double maxValue, Func<T, double> valueGetter) {                                    //__SILP__
            SubKey = subKey;                                                                                   //__SILP__
            MaxValue = maxValue;                                                                               //__SILP__
            ValueGetter = valueGetter;                                                                         //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        protected override bool IsValid(T _val) {                                                              //__SILP__
            double val = ValueGetter(_val);                                                                    //__SILP__
            return val < MaxValue;                                                                             //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public override bool DoEncode(Data spec) {                                                             //__SILP__
            return spec.SetDouble(SpecConsts.GetSubSpecKey(SubKey, SpecConsts.KindSmaller), MaxValue);         //__SILP__
        }                                                                                                      //__SILP__
    }                                                                                                          //__SILP__
                                                                                                               //__SILP__
    public class DataDoubleSpecValueCheckerSmaller : SubDoubleSpecValueCheckerSmaller<Data> {                  //__SILP__
        public DataDoubleSpecValueCheckerSmaller(string subKey, double maxValue)                               //__SILP__
                : base(subKey, maxValue, (Data val) => val.GetDouble(subKey)) {                                //__SILP__
        }                                                                                                      //__SILP__
    }                                                                                                          //__SILP__
                                                                                                               //__SILP__
    public class DoubleSpecValueCheckerSmallerOrEqual : SpecValueChecker<double> {                             //__SILP__
        public readonly double MaxValue;                                                                       //__SILP__
        public DoubleSpecValueCheckerSmallerOrEqual(double maxValue) {                                         //__SILP__
            MaxValue = maxValue;                                                                               //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        protected override bool IsValid(double val) {                                                          //__SILP__
            return val <= MaxValue;                                                                            //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public override bool DoEncode(Data spec) {                                                             //__SILP__
            return spec.SetDouble(SpecConsts.KindSmallerOrEqual, MaxValue);                                    //__SILP__
        }                                                                                                      //__SILP__
    }                                                                                                          //__SILP__
                                                                                                               //__SILP__
    public class SubDoubleSpecValueCheckerSmallerOrEqual<T> : SpecValueChecker<T> {                            //__SILP__
        public readonly string SubKey;                                                                         //__SILP__
        public readonly double MaxValue;                                                                       //__SILP__
        public readonly Func<T, double> ValueGetter;                                                           //__SILP__
        public SubDoubleSpecValueCheckerSmallerOrEqual(string subKey,                                          //__SILP__
                            double maxValue, Func<T, double> valueGetter) {                                    //__SILP__
            SubKey = subKey;                                                                                   //__SILP__
            MaxValue = maxValue;                                                                               //__SILP__
            ValueGetter = valueGetter;                                                                         //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        protected override bool IsValid(T _val) {                                                              //__SILP__
            double val = ValueGetter(_val);                                                                    //__SILP__
            return val <= MaxValue;                                                                            //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public override bool DoEncode(Data spec) {                                                             //__SILP__
            return spec.SetDouble(SpecConsts.GetSubSpecKey(SubKey, SpecConsts.KindSmallerOrEqual), MaxValue);  //__SILP__
        }                                                                                                      //__SILP__
    }                                                                                                          //__SILP__
                                                                                                               //__SILP__
    public class DataDoubleSpecValueCheckerSmallerOrEqual : SubDoubleSpecValueCheckerSmallerOrEqual<Data> {    //__SILP__
        public DataDoubleSpecValueCheckerSmallerOrEqual(string subKey, double maxValue)                        //__SILP__
                : base(subKey, maxValue, (Data val) => val.GetDouble(subKey)) {                                //__SILP__
        }                                                                                                      //__SILP__
    }                                                                                                          //__SILP__
                                                                                                               //__SILP__
    //SILP: PROPERTY_SPEC_IN(String, string)
    public class StringSpecValueCheckerIn : SpecValueChecker<string> {                            //__SILP__
        public readonly string[] Values;                                                          //__SILP__
        public StringSpecValueCheckerIn(string[] values) {                                        //__SILP__
            Values = values;                                                                      //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        protected override bool IsValid(string val) {                                             //__SILP__
            foreach (string v in Values) {                                                        //__SILP__
                if (v == val) return true;                                                        //__SILP__
            }                                                                                     //__SILP__
            return false;                                                                         //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        public override bool DoEncode(Data spec) {                                                //__SILP__
            Data values = new Data();                                                             //__SILP__
            for (int i = 0; i < Values.Length; i++) {                                             //__SILP__
                string v = Values[i];                                                             //__SILP__
                values.SetString(i.ToString(), v);                                                //__SILP__
            }                                                                                     //__SILP__
            return spec.SetData(SpecConsts.KindIn, values);                                       //__SILP__
        }                                                                                         //__SILP__
    }                                                                                             //__SILP__
                                                                                                  //__SILP__
    public class SubStringSpecValueCheckerIn<T> : SpecValueChecker<T> {                           //__SILP__
        public readonly string SubKey;                                                            //__SILP__
        public readonly string[] Values;                                                          //__SILP__
        public readonly Func<T, string> ValueGetter;                                              //__SILP__
        public SubStringSpecValueCheckerIn(string subKey,                                         //__SILP__
                                        string[] values, Func<T, string> valueGetter) {           //__SILP__
            SubKey = subKey;                                                                      //__SILP__
            Values = values;                                                                      //__SILP__
            ValueGetter = valueGetter;                                                            //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        protected override bool IsValid(T _val) {                                                 //__SILP__
            string val = ValueGetter(_val);                                                       //__SILP__
            foreach (string v in Values) {                                                        //__SILP__
                if (v == val) return true;                                                        //__SILP__
            }                                                                                     //__SILP__
            return false;                                                                         //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        public override bool DoEncode(Data spec) {                                                //__SILP__
            Data values = new Data();                                                             //__SILP__
            for (int i = 0; i < Values.Length; i++) {                                             //__SILP__
                string v = Values[i];                                                             //__SILP__
                values.SetString(i.ToString(), v);                                                //__SILP__
            }                                                                                     //__SILP__
            return spec.SetData(SpecConsts.GetSubSpecKey(SubKey, SpecConsts.KindIn), values);     //__SILP__
        }                                                                                         //__SILP__
    }                                                                                             //__SILP__
                                                                                                  //__SILP__
    public class DataStringSpecValueCheckerIn : SubStringSpecValueCheckerIn<Data> {               //__SILP__
        public DataStringSpecValueCheckerIn(string subKey, string[] values)                       //__SILP__
                : base(subKey, values, (Data val) => val.GetString(subKey)) {                     //__SILP__
        }                                                                                         //__SILP__
    }                                                                                             //__SILP__
                                                                                                  //__SILP__
    public class StringSpecValueCheckerNotIn : SpecValueChecker<string> {                         //__SILP__
        public readonly string[] Values;                                                          //__SILP__
        public StringSpecValueCheckerNotIn(string[] values) {                                     //__SILP__
            Values = values;                                                                      //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        protected override bool IsValid(string val) {                                             //__SILP__
            foreach (string v in Values) {                                                        //__SILP__
                if (v == val) return false;                                                       //__SILP__
            }                                                                                     //__SILP__
            return true;                                                                          //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        public override bool DoEncode(Data spec) {                                                //__SILP__
            Data values = new Data();                                                             //__SILP__
            for (int i = 0; i < Values.Length; i++) {                                             //__SILP__
                string v = Values[i];                                                             //__SILP__
                values.SetString(i.ToString(), v);                                                //__SILP__
            }                                                                                     //__SILP__
            return spec.SetData(SpecConsts.KindNotIn, values);                                    //__SILP__
        }                                                                                         //__SILP__
    }                                                                                             //__SILP__
                                                                                                  //__SILP__
    public class SubStringSpecValueCheckerNotIn<T> : SpecValueChecker<T> {                        //__SILP__
        public readonly string SubKey;                                                            //__SILP__
        public readonly string[] Values;                                                          //__SILP__
        public readonly Func<T, string> ValueGetter;                                              //__SILP__
        public SubStringSpecValueCheckerNotIn(string subKey,                                      //__SILP__
                                        string[] values, Func<T, string> valueGetter) {           //__SILP__
            SubKey = subKey;                                                                      //__SILP__
            Values = values;                                                                      //__SILP__
            ValueGetter = valueGetter;                                                            //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        protected override bool IsValid(T _val) {                                                 //__SILP__
            string val = ValueGetter(_val);                                                       //__SILP__
            foreach (string v in Values) {                                                        //__SILP__
                if (v == val) return false;                                                       //__SILP__
            }                                                                                     //__SILP__
            return true;                                                                          //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        public override bool DoEncode(Data spec) {                                                //__SILP__
            Data values = new Data();                                                             //__SILP__
            for (int i = 0; i < Values.Length; i++) {                                             //__SILP__
                string v = Values[i];                                                             //__SILP__
                values.SetString(i.ToString(), v);                                                //__SILP__
            }                                                                                     //__SILP__
            return spec.SetData(SpecConsts.GetSubSpecKey(SubKey, SpecConsts.KindNotIn), values);  //__SILP__
        }                                                                                         //__SILP__
    }                                                                                             //__SILP__
                                                                                                  //__SILP__
    public class DataStringSpecValueCheckerNotIn : SubStringSpecValueCheckerNotIn<Data> {         //__SILP__
        public DataStringSpecValueCheckerNotIn(string subKey, string[] values)                    //__SILP__
                : base(subKey, values, (Data val) => val.GetString(subKey)) {                     //__SILP__
        }                                                                                         //__SILP__
    }                                                                                             //__SILP__
                                                                                                  //__SILP__
}
