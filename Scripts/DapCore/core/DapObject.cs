using System;

namespace ADD.Dap {
    public interface DapObject {
        string Type { get; }
        Data Encode();
        bool Decode(Data data);
    }

    public struct DapObjectConsts {
        public const string KeyType = "type";
    }
}
