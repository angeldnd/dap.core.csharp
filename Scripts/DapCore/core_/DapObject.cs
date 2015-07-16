using System;

namespace angeldnd.dap {
    public interface DapObject {
        string Type { get; }
        int Revision { get; } //Mainly For Debugging

        Data Encode();
        bool Decode(Data data);

        bool DebugMode { get; }
        string GetLogPrefix();
    }

    public struct DapObjectConsts {
        public const string KeyType = "type";
    }
}
