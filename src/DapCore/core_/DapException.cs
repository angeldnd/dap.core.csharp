using System;
using System.Text;
using System.Collections.Generic;

namespace angeldnd.dap {
    public class DapException : Exception {
        public DapException(string format, params object[] values)
                    : base(Log.GetMsg(format, values)) {
        }

        public DapException(Exception innerException,
                                string format, params object[] values)
                    : base(Log.GetMsg(format, values), innerException) {
        }
    }
}
