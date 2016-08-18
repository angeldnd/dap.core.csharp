using System;
using System.Text;
using System.Collections.Generic;

using angeldnd.dap;
using angeldnd.dap.util;

namespace angeldnd.dap {
    public abstract class HandlerException : Exception {
        public readonly Data Response;

        public HandlerException(Data response) {
            Response = response;
        }
    }
}
