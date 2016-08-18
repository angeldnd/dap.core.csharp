using System;
using System.Text;
using System.Collections.Generic;

using angeldnd.dap;
using angeldnd.dap.util;

namespace angeldnd.dap {
    public class BadRequestException : HandlerException {
        public BadRequestException(Handler handler, Data req, string format, params object[] values)
                   : base(ResponseHelper.BadRequest(handler, req, format, values)) {
        }

        public BadRequestException(Handler handler, Data req, Data result)
                   : base(ResponseHelper.BadRequest(handler, req, result)) {
        }
    }
}
