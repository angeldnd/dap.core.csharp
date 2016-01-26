using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public sealed class Lock : Accessor<IObject>, ISecurable {
        public Lock(Object obj) : base(obj) {
        }

        private Pass _Pass;

        public bool SetPass(Pass pass) {
            if (_Pass == pass) {
                return true;
            } else if (_Pass == null) {
                _Pass = pass;
                return true;
            }
            Obj.Error("Already Locked: {0}", pass);
            return false;
        }

        public bool AdminSecured {
            get {
                return _Pass != null;
            }
        }

        public bool WriteSecured {
            get {
                if (_Pass == null) return false;
                if (_Pass.Writable) return false;
                return true;
            }
        }

        public bool CheckAdminPass(Pass pass, bool logError) {
            if (_Pass == null) return true;
            if (_Pass.CheckAdminPass(this, pass)) return true;

            if (logError) {
                Error("Invalid Admin Pass: Pass = {0}, pass = {1}", _Pass, pass);
            }
            return false;
        }

        public bool CheckAdminPass(Pass pass) {
            return CheckAdminPass(pass, true);
        }

        public bool CheckWritePass(Pass pass, bool logError) {
            if (_Pass == null) return true;
            if (_Pass.CheckWritePass(this, pass)) return true;

            if (logError) {
                Error("Invalid Write Pass: Pass = {0}, pass = {1}", _Pass, pass);
            }
            return false;
        }

        public bool CheckWritePass(Pass pass) {
            return CheckWritePass(pass, true);
        }
    }
}
