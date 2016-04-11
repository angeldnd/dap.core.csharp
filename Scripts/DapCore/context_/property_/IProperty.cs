using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IProperty : IVar {
        Data Encode();
        bool Decode(Data data);

        Data EncodeValue();
        bool DecodeValue(Data data);
    }

    public interface IProperty<T> : IVar<T>, IProperty {
    }
}
