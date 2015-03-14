using System;
using System.Collections.Generic;

namespace ADD.Dap {
    public abstract class Var<T> : BaseAspect {
        private T _Value;
        public T Value {
            get { return _Value; }
        }

        public virtual bool SetValue(T newValue) {
            _Value = newValue;
            return true;
        }
    }

    public class AnyVar<T> : Var<T> {
        protected override bool DoEncode(Data data) {
            return false;
        }

        protected override bool DoDecode(Data data) {
            return false;
        }
    }

    //SILP: VAR_CLASS(Bool, bool)
    public class BoolVar : Var<bool> {                                //__SILP__
        public override string Type {                                 //__SILP__
            get { return VarsConsts.TypeBoolVar; }                    //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        protected override bool DoEncode(Data data) {                 //__SILP__
            return data.SetBool(VarsConsts.KeyValue, Value);          //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        protected override bool DoDecode(Data data) {                 //__SILP__
            SetValue(data.GetBool(VarsConsts.KeyValue));              //__SILP__
            return true;                                              //__SILP__
        }                                                             //__SILP__
    }                                                                 //__SILP__
                                                                      //__SILP__
    //SILP: VAR_CLASS(Int, int)
    public class IntVar : Var<int> {                                  //__SILP__
        public override string Type {                                 //__SILP__
            get { return VarsConsts.TypeIntVar; }                     //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        protected override bool DoEncode(Data data) {                 //__SILP__
            return data.SetInt(VarsConsts.KeyValue, Value);           //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        protected override bool DoDecode(Data data) {                 //__SILP__
            SetValue(data.GetInt(VarsConsts.KeyValue));               //__SILP__
            return true;                                              //__SILP__
        }                                                             //__SILP__
    }                                                                 //__SILP__
                                                                      //__SILP__
    //SILP: VAR_CLASS(Long, long)
    public class LongVar : Var<long> {                                //__SILP__
        public override string Type {                                 //__SILP__
            get { return VarsConsts.TypeLongVar; }                    //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        protected override bool DoEncode(Data data) {                 //__SILP__
            return data.SetLong(VarsConsts.KeyValue, Value);          //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        protected override bool DoDecode(Data data) {                 //__SILP__
            SetValue(data.GetLong(VarsConsts.KeyValue));              //__SILP__
            return true;                                              //__SILP__
        }                                                             //__SILP__
    }                                                                 //__SILP__
                                                                      //__SILP__
    //SILP: VAR_CLASS(Float, float)
    public class FloatVar : Var<float> {                              //__SILP__
        public override string Type {                                 //__SILP__
            get { return VarsConsts.TypeFloatVar; }                   //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        protected override bool DoEncode(Data data) {                 //__SILP__
            return data.SetFloat(VarsConsts.KeyValue, Value);         //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        protected override bool DoDecode(Data data) {                 //__SILP__
            SetValue(data.GetFloat(VarsConsts.KeyValue));             //__SILP__
            return true;                                              //__SILP__
        }                                                             //__SILP__
    }                                                                 //__SILP__
                                                                      //__SILP__
    //SILP: VAR_CLASS(Double, double)
    public class DoubleVar : Var<double> {                            //__SILP__
        public override string Type {                                 //__SILP__
            get { return VarsConsts.TypeDoubleVar; }                  //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        protected override bool DoEncode(Data data) {                 //__SILP__
            return data.SetDouble(VarsConsts.KeyValue, Value);        //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        protected override bool DoDecode(Data data) {                 //__SILP__
            SetValue(data.GetDouble(VarsConsts.KeyValue));            //__SILP__
            return true;                                              //__SILP__
        }                                                             //__SILP__
    }                                                                 //__SILP__
                                                                      //__SILP__
    //SILP: VAR_CLASS(String, string)
    public class StringVar : Var<string> {                            //__SILP__
        public override string Type {                                 //__SILP__
            get { return VarsConsts.TypeStringVar; }                  //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        protected override bool DoEncode(Data data) {                 //__SILP__
            return data.SetString(VarsConsts.KeyValue, Value);        //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        protected override bool DoDecode(Data data) {                 //__SILP__
            SetValue(data.GetString(VarsConsts.KeyValue));            //__SILP__
            return true;                                              //__SILP__
        }                                                             //__SILP__
    }                                                                 //__SILP__
                                                                      //__SILP__
    //SILP: VAR_CLASS(Data, Data)
    public class DataVar : Var<Data> {                                //__SILP__
        public override string Type {                                 //__SILP__
            get { return VarsConsts.TypeDataVar; }                    //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        protected override bool DoEncode(Data data) {                 //__SILP__
            return data.SetData(VarsConsts.KeyValue, Value);          //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        protected override bool DoDecode(Data data) {                 //__SILP__
            SetValue(data.GetData(VarsConsts.KeyValue));              //__SILP__
            return true;                                              //__SILP__
        }                                                             //__SILP__
    }                                                                 //__SILP__
                                                                      //__SILP__
}
