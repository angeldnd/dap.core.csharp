using System;

namespace angeldnd.dap {
    public interface SecurableAspect : Aspect {
        bool Secured { get; }
        bool SetPass(Pass pass);
        bool CheckPass(Pass pass);
    }

    public abstract class BaseSecurableAspect : BaseAspect, SecurableAspect {
        //SILP:SECURABLE_ASPECT_MIXIN()
        private static readonly Pass OPEN_PASS = new Pass();              //__SILP__
                                                                          //__SILP__
        private Pass _Pass = null;                                        //__SILP__
        protected Pass Pass {                                             //__SILP__
            get { return _Pass; }                                         //__SILP__
        }                                                                 //__SILP__
                                                                          //__SILP__
        public bool Secured {                                             //__SILP__
            get {                                                         //__SILP__
                if (_Pass == null) return false;                          //__SILP__
                if (OPEN_PASS == _Pass) return false;                     //__SILP__
                return true;                                              //__SILP__
            }                                                             //__SILP__
        }                                                                 //__SILP__
                                                                          //__SILP__
        public bool SetPass(Pass pass) {                                  //__SILP__
            /*                                                            //__SILP__
                * The OPEN_PASS trick is to set the pass, so it can't     //__SILP__
                * be set in the future, but it's "open", any pass can     //__SILP__
                * pass the check.                                         //__SILP__
                */                                                        //__SILP__
            if (_Pass == null) {                                          //__SILP__
                if (pass == null) {                                       //__SILP__
                    _Pass = OPEN_PASS;                                    //__SILP__
                } else {                                                  //__SILP__
                    _Pass = pass;                                         //__SILP__
                }                                                         //__SILP__
                return true;                                              //__SILP__
            } else if (_Pass == pass) {                                   //__SILP__
                return true;                                              //__SILP__
            } else if (OPEN_PASS == _Pass && pass == null) {              //__SILP__
                return true;                                              //__SILP__
            } else if (_Pass.Equals(pass)) {                              //__SILP__
                return true;                                              //__SILP__
            }                                                             //__SILP__
            Error("SetPass Failed: {0} -> {1}", _Pass, pass);             //__SILP__
            return false;                                                 //__SILP__
        }                                                                 //__SILP__
                                                                          //__SILP__
        public bool CheckPass(Pass pass) {                                //__SILP__
            if (_Pass == null) return true;                               //__SILP__
            if (_Pass == pass) return true;                               //__SILP__
            if (OPEN_PASS == _Pass) return true;                          //__SILP__
            if (_Pass.Equals(pass)) return true;                          //__SILP__
                                                                          //__SILP__
            Error("Invalid Pass: _Pass = {0}, pass = {1}", _Pass, pass);  //__SILP__
            return false;                                                 //__SILP__
        }                                                                 //__SILP__
    }

    public abstract class SecurableEntityAspect : EntityAspect, SecurableAspect {
        //SILP:SECURABLE_ASPECT_MIXIN()
        private static readonly Pass OPEN_PASS = new Pass();              //__SILP__
                                                                          //__SILP__
        private Pass _Pass = null;                                        //__SILP__
        protected Pass Pass {                                             //__SILP__
            get { return _Pass; }                                         //__SILP__
        }                                                                 //__SILP__
                                                                          //__SILP__
        public bool Secured {                                             //__SILP__
            get {                                                         //__SILP__
                if (_Pass == null) return false;                          //__SILP__
                if (OPEN_PASS == _Pass) return false;                     //__SILP__
                return true;                                              //__SILP__
            }                                                             //__SILP__
        }                                                                 //__SILP__
                                                                          //__SILP__
        public bool SetPass(Pass pass) {                                  //__SILP__
            /*                                                            //__SILP__
                * The OPEN_PASS trick is to set the pass, so it can't     //__SILP__
                * be set in the future, but it's "open", any pass can     //__SILP__
                * pass the check.                                         //__SILP__
                */                                                        //__SILP__
            if (_Pass == null) {                                          //__SILP__
                if (pass == null) {                                       //__SILP__
                    _Pass = OPEN_PASS;                                    //__SILP__
                } else {                                                  //__SILP__
                    _Pass = pass;                                         //__SILP__
                }                                                         //__SILP__
                return true;                                              //__SILP__
            } else if (_Pass == pass) {                                   //__SILP__
                return true;                                              //__SILP__
            } else if (OPEN_PASS == _Pass && pass == null) {              //__SILP__
                return true;                                              //__SILP__
            } else if (_Pass.Equals(pass)) {                              //__SILP__
                return true;                                              //__SILP__
            }                                                             //__SILP__
            Error("SetPass Failed: {0} -> {1}", _Pass, pass);             //__SILP__
            return false;                                                 //__SILP__
        }                                                                 //__SILP__
                                                                          //__SILP__
        public bool CheckPass(Pass pass) {                                //__SILP__
            if (_Pass == null) return true;                               //__SILP__
            if (_Pass == pass) return true;                               //__SILP__
            if (OPEN_PASS == _Pass) return true;                          //__SILP__
            if (_Pass.Equals(pass)) return true;                          //__SILP__
                                                                          //__SILP__
            Error("Invalid Pass: _Pass = {0}, pass = {1}", _Pass, pass);  //__SILP__
            return false;                                                 //__SILP__
        }                                                                 //__SILP__
    }
}
