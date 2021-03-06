using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface ISecurable {
        bool SetPass(Pass pass);

        bool AdminSecured { get; }
        bool WriteSecured { get; }
        bool CheckAdminPass(Pass pass, bool isDebug = false);
        bool CheckWritePass(Pass pass, bool isDebug = false);
    }
}
