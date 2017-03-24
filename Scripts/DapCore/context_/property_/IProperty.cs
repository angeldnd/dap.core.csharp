using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IProperty : IVar {
        Data Encode();
        bool Decode(Data data);

        /*
         * Not have logic with DapType check, the format of the data is more compact,
         * still having the "v" as key, for group properties, the sub data is not having
         * "v" key inside, this is the format in config/metadata files.
         */
        Data EncodeValue();
        bool DecodeValue(Data data);
    }

    public interface IProperty<T> : IVar<T>, IProperty {
    }
}
