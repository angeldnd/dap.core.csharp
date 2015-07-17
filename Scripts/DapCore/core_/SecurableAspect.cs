using System;

namespace angeldnd.dap {
    public interface SecurableAspect : Aspect {
        bool Secured { get; }
        bool SetPass(Pass pass);
        bool CheckAdminPass(Pass pass);
        bool CheckWritePass(Pass pass);
    }

    public abstract class BaseSecurableAspect : BaseAspect, SecurableAspect {
        //SILP:SECURABLE_ASPECT_MIXIN()
        private Pass _Pass = null;                                              //__SILP__
        protected Pass Pass {                                                   //__SILP__
            get { return _Pass; }                                               //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        public bool IsSecured {                                                 //__SILP__
            get {                                                               //__SILP__
                return _Pass != null;                                           //__SILP__
            }                                                                   //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        public bool IsPublic {                                                  //__SILP__
            get {                                                               //__SILP__
                if (_Pass == null) return true;                                 //__SILP__
                if (_Pass is OpenPass) return true;                             //__SILP__
                return false;                                                   //__SILP__
            }                                                                   //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        public bool SetPass(Pass pass) {                                        //__SILP__
            if (_Pass == pass) {                                                //__SILP__
                return true;                                                    //__SILP__
            } else if (_Pass == null) {                                         //__SILP__
                _Pass = pass;                                                   //__SILP__
                return true;                                                    //__SILP__
            } else if (_Pass.Equals(pass)) {                                    //__SILP__
                return true;                                                    //__SILP__
            }                                                                   //__SILP__
            Error("SetPass Failed: {0} -> {1}", _Pass, pass);                   //__SILP__
            return false;                                                       //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        public bool CheckAdminPass(Pass pass) {                                 //__SILP__
            if (_Pass == null) return true;                                     //__SILP__
            if (_Pass.CheckAdminPass(pass)) return true;                        //__SILP__
                                                                                //__SILP__
            Error("Invalid Admin Pass: _Pass = {0}, pass = {1}", _Pass, pass);  //__SILP__
            return false;                                                       //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        public bool CheckWritePass(Pass pass) {                                 //__SILP__
            if (_Pass == null) return true;                                     //__SILP__
            if (_Pass.CheckWritePass(pass)) return true;                        //__SILP__
                                                                                //__SILP__
            Error("Invalid Write Pass: _Pass = {0}, pass = {1}", _Pass, pass);  //__SILP__
            return false;                                                       //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
    }

    public abstract class SecurableEntityAspect : EntityAspect, SecurableAspect {
        //SILP:SECURABLE_ASPECT_MIXIN()
        private Pass _Pass = null;                                              //__SILP__
        protected Pass Pass {                                                   //__SILP__
            get { return _Pass; }                                               //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        public bool IsSecured {                                                 //__SILP__
            get {                                                               //__SILP__
                return _Pass != null;                                           //__SILP__
            }                                                                   //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        public bool IsPublic {                                                  //__SILP__
            get {                                                               //__SILP__
                if (_Pass == null) return true;                                 //__SILP__
                if (_Pass is OpenPass) return true;                             //__SILP__
                return false;                                                   //__SILP__
            }                                                                   //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        public bool SetPass(Pass pass) {                                        //__SILP__
            if (_Pass == pass) {                                                //__SILP__
                return true;                                                    //__SILP__
            } else if (_Pass == null) {                                         //__SILP__
                _Pass = pass;                                                   //__SILP__
                return true;                                                    //__SILP__
            } else if (_Pass.Equals(pass)) {                                    //__SILP__
                return true;                                                    //__SILP__
            }                                                                   //__SILP__
            Error("SetPass Failed: {0} -> {1}", _Pass, pass);                   //__SILP__
            return false;                                                       //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        public bool CheckAdminPass(Pass pass) {                                 //__SILP__
            if (_Pass == null) return true;                                     //__SILP__
            if (_Pass.CheckAdminPass(pass)) return true;                        //__SILP__
                                                                                //__SILP__
            Error("Invalid Admin Pass: _Pass = {0}, pass = {1}", _Pass, pass);  //__SILP__
            return false;                                                       //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        public bool CheckWritePass(Pass pass) {                                 //__SILP__
            if (_Pass == null) return true;                                     //__SILP__
            if (_Pass.CheckWritePass(pass)) return true;                        //__SILP__
                                                                                //__SILP__
            Error("Invalid Write Pass: _Pass = {0}, pass = {1}", _Pass, pass);  //__SILP__
            return false;                                                       //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
    }
}
