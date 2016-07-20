using System;
using System.Text;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class CaretException : DapException {
        public static string GetMessage(string source, Exception e) {
            if (e == null) return source;
            string msg;
            CaretException ge = e as CaretException;
            if (ge != null) {
                msg = ge.MessageWithCaret;
            } else {
                msg = string.Format("{0} {1} [{2}]", source, e.Message, e.GetType().FullName);
            }
            return msg;
        }

        public abstract Caret Caret { get; }

        public virtual string Hint {
            get { return ""; }
        }

        public CaretException(string format, params object[] values)
                    : base(format, values) {
        }

        public CaretException(Exception innerException,
                                string format, params object[] values)
                    : base(innerException, format, values) {
        }

        public string MessageWithCaret {
            get {
                if (InnerException != null) {
                    return string.Format("{0}{1} {2} [{3}] <- {4}", Caret, Hint,
                            Message, GetType().FullName, InnerException.Message);
                }
                return string.Format("{0}{1} {2} [{3}]",
                            Caret, Hint, Message, GetType().FullName);
            }
        }
    }
}
