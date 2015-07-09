using System;

namespace angeldnd.dap {
    public interface DapObject {
        string Type { get; }
        int Revision { get; } //Mainly For Debugging

        Data Encode();
        bool Decode(Data data);
    }

    public struct DapObjectConsts {
        public const string KeyType = "type";
    }
}
