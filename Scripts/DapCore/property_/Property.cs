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

    public abstract class Property<T>: Var<IProperties, T>, IProperty<T> {
        public Property(IProperties owner, string path, Pass pass) : base(owner, path, pass) {
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

        public bool Decode(Data data) {
            return Decode(null, data);
        }

        protected abstract bool DoEncode(Data data);
        protected abstract bool DoDecode(Pass pass, Data data);
    }
}
