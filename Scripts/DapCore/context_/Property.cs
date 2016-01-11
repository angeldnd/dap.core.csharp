using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IProperty : IVar {
        Data Encode();
        bool Decode(Data data);
        bool Decode(Pass pass, Data data);
    }

    public interface IProperty<T> : IVar<T>, IProperty {
    }

    public abstract class Property<T>: Var<Properties, T>, IProperty<T> {
        public Property(Properties owner, string path, Pass pass) : base(owner, path, pass) {
        }

        public Data Encode() {
            if (!string.IsNullOrEmpty(Type)) {
                Data data = new Data();
                if (data.SetString(ObjectConsts.KeyType, Type)) {
                    if (DoEncode(data)) {
                        return data;
                    }
                }
            }
            if (LogDebug) Debug("Not Encodable!");
            return null;
        }

        public bool Decode(Data data) {
            return Decode(null, data);
        }

        public bool Decode(Pass pass, Data data) {
            if (!CheckWritePass(pass)) return false;

            string type = data.GetString(ObjectConsts.KeyType);
            if (type == Type) {
                return DoDecode(pass, data);
            } else {
                Error("Type Mismatched: {0}, {1}", Type, type);
            }
            return false;
        }

        protected abstract bool DoEncode(Data data);
        protected abstract bool DoDecode(Pass pass, Data data);
    }

    //SILP: PROPERTY_CLASS(Bool, bool)
    public class BoolProperty : Property<bool> {                                                   //__SILP__
        public override string Type {                                                              //__SILP__
            get { return PropertiesConsts.TypeBoolProperty; }                                      //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        public BoolProperty(Properties owner, string path, Pass pass) : base(owner, path, pass) {  //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        protected override bool DoEncode(Data data) {                                              //__SILP__
            return data.SetBool(PropertiesConsts.KeyValue, Value);                                 //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        protected override bool DoDecode(Pass pass, Data data) {                                   //__SILP__
            return SetValue(pass, data.GetBool(PropertiesConsts.KeyValue));                        //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        protected override bool NeedUpdate(bool newVal) {                                          //__SILP__
            return base.NeedUpdate(newVal) || (Value != newVal);                                   //__SILP__
        }                                                                                          //__SILP__
    }                                                                                              //__SILP__
                                                                                                   //__SILP__
    //SILP: PROPERTY_CLASS(Int, int)
    public class IntProperty : Property<int> {                                                    //__SILP__
        public override string Type {                                                             //__SILP__
            get { return PropertiesConsts.TypeIntProperty; }                                      //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        public IntProperty(Properties owner, string path, Pass pass) : base(owner, path, pass) {  //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        protected override bool DoEncode(Data data) {                                             //__SILP__
            return data.SetInt(PropertiesConsts.KeyValue, Value);                                 //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        protected override bool DoDecode(Pass pass, Data data) {                                  //__SILP__
            return SetValue(pass, data.GetInt(PropertiesConsts.KeyValue));                        //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        protected override bool NeedUpdate(int newVal) {                                          //__SILP__
            return base.NeedUpdate(newVal) || (Value != newVal);                                  //__SILP__
        }                                                                                         //__SILP__
    }                                                                                             //__SILP__
                                                                                                  //__SILP__
    //SILP: PROPERTY_CLASS(Long, long)
    public class LongProperty : Property<long> {                                                   //__SILP__
        public override string Type {                                                              //__SILP__
            get { return PropertiesConsts.TypeLongProperty; }                                      //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        public LongProperty(Properties owner, string path, Pass pass) : base(owner, path, pass) {  //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        protected override bool DoEncode(Data data) {                                              //__SILP__
            return data.SetLong(PropertiesConsts.KeyValue, Value);                                 //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        protected override bool DoDecode(Pass pass, Data data) {                                   //__SILP__
            return SetValue(pass, data.GetLong(PropertiesConsts.KeyValue));                        //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        protected override bool NeedUpdate(long newVal) {                                          //__SILP__
            return base.NeedUpdate(newVal) || (Value != newVal);                                   //__SILP__
        }                                                                                          //__SILP__
    }                                                                                              //__SILP__
                                                                                                   //__SILP__
    //SILP: PROPERTY_CLASS(Float, float)
    public class FloatProperty : Property<float> {                                                  //__SILP__
        public override string Type {                                                               //__SILP__
            get { return PropertiesConsts.TypeFloatProperty; }                                      //__SILP__
        }                                                                                           //__SILP__
                                                                                                    //__SILP__
        public FloatProperty(Properties owner, string path, Pass pass) : base(owner, path, pass) {  //__SILP__
        }                                                                                           //__SILP__
                                                                                                    //__SILP__
        protected override bool DoEncode(Data data) {                                               //__SILP__
            return data.SetFloat(PropertiesConsts.KeyValue, Value);                                 //__SILP__
        }                                                                                           //__SILP__
                                                                                                    //__SILP__
        protected override bool DoDecode(Pass pass, Data data) {                                    //__SILP__
            return SetValue(pass, data.GetFloat(PropertiesConsts.KeyValue));                        //__SILP__
        }                                                                                           //__SILP__
                                                                                                    //__SILP__
        protected override bool NeedUpdate(float newVal) {                                          //__SILP__
            return base.NeedUpdate(newVal) || (Value != newVal);                                    //__SILP__
        }                                                                                           //__SILP__
    }                                                                                               //__SILP__
                                                                                                    //__SILP__
    //SILP: PROPERTY_CLASS(Double, double)
    public class DoubleProperty : Property<double> {                                                 //__SILP__
        public override string Type {                                                                //__SILP__
            get { return PropertiesConsts.TypeDoubleProperty; }                                      //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        public DoubleProperty(Properties owner, string path, Pass pass) : base(owner, path, pass) {  //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        protected override bool DoEncode(Data data) {                                                //__SILP__
            return data.SetDouble(PropertiesConsts.KeyValue, Value);                                 //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        protected override bool DoDecode(Pass pass, Data data) {                                     //__SILP__
            return SetValue(pass, data.GetDouble(PropertiesConsts.KeyValue));                        //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        protected override bool NeedUpdate(double newVal) {                                          //__SILP__
            return base.NeedUpdate(newVal) || (Value != newVal);                                     //__SILP__
        }                                                                                            //__SILP__
    }                                                                                                //__SILP__
                                                                                                     //__SILP__
    //SILP: PROPERTY_CLASS(String, string)
    public class StringProperty : Property<string> {                                                 //__SILP__
        public override string Type {                                                                //__SILP__
            get { return PropertiesConsts.TypeStringProperty; }                                      //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        public StringProperty(Properties owner, string path, Pass pass) : base(owner, path, pass) {  //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        protected override bool DoEncode(Data data) {                                                //__SILP__
            return data.SetString(PropertiesConsts.KeyValue, Value);                                 //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        protected override bool DoDecode(Pass pass, Data data) {                                     //__SILP__
            return SetValue(pass, data.GetString(PropertiesConsts.KeyValue));                        //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        protected override bool NeedUpdate(string newVal) {                                          //__SILP__
            return base.NeedUpdate(newVal) || (Value != newVal);                                     //__SILP__
        }                                                                                            //__SILP__
    }                                                                                                //__SILP__
                                                                                                     //__SILP__
    //SILP: PROPERTY_CLASS(Data, Data)
    public class DataProperty : Property<Data> {                                                   //__SILP__
        public override string Type {                                                              //__SILP__
            get { return PropertiesConsts.TypeDataProperty; }                                      //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        public DataProperty(Properties owner, string path, Pass pass) : base(owner, path, pass) {  //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        protected override bool DoEncode(Data data) {                                              //__SILP__
            return data.SetData(PropertiesConsts.KeyValue, Value);                                 //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        protected override bool DoDecode(Pass pass, Data data) {                                   //__SILP__
            return SetValue(pass, data.GetData(PropertiesConsts.KeyValue));                        //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        protected override bool NeedUpdate(Data newVal) {                                          //__SILP__
            return base.NeedUpdate(newVal) || (Value != newVal);                                   //__SILP__
        }                                                                                          //__SILP__
    }                                                                                              //__SILP__
                                                                                                   //__SILP__
}
