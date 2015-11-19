using System;
using System.Collections.Generic;

using angeldnd.dap;
using angeldnd.dap.util;

namespace angeldnd.dap {
    public abstract class Entry : ItemType {
        public abstract bool Setup(byte[] bytes);
    }
}

