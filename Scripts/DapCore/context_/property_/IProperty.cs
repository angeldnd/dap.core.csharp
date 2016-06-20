using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IProperty : IVar {
        Data Encode();
        bool Decode(Data data);

        /*
         * Not have logic with DapType check, the format of the data is same,
         * still having the "v" as key.
         */
        Data EncodeValue();
        bool DecodeValue(Data data);
    }

    public interface IProperty<T> : IVar<T>, IProperty {
    }
}
