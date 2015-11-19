using System;

namespace angeldnd.dap {
    public sealed class Pass {
        /*
         * In Debug Mode, the pass check is not really enforced,
         * only log errors will be created.
         * Once the system is started, debugmode can only be turn
         * off, can't be turned on again.
         */
        private static bool _DebugMode = true;
        public static bool DebugMode {
            get { return _DebugMode; }
        }
        public static bool TurnOffDebugMode() {
            if (_DebugMode) {
                _DebugMode = false;
                return true;
            }
            return false;
        }
        /*
         * Can change whether log as error or info in debugmode,
         * useful for use cases that expect permission denied
         */
        public static bool DebugModeLogError = true;

        private readonly bool _Writable;
        private readonly int _HashCode;

        public bool Writable {
            get { return _Writable; }
        }

        public Pass() {
            _Writable = false;
            _HashCode = Guid.NewGuid().GetHashCode();
        }

        public Pass(bool writable, int hashCode) {
            _Writable = writable;
            _HashCode = hashCode;
        }

        private Pass _Open = null;
        public Pass Open {
            get {
                if (_Writable) return this;
                if (_Open == null) {
                    _Open = new Pass(true, _HashCode);
                }
                return _Open;
            }
        }

        public override string ToString() {
            return string.Format("[{0}:{1}{2}]", GetType().Name,
                            _Writable ? "^" : "@",
                            _HashCode);
        }

        public override bool Equals(object obj) {
            if (this == obj) return true;
            if (obj == null) return false;
            if (_HashCode == obj.GetHashCode()) return true;
            return false;
        }

        public override int GetHashCode() {
            return _HashCode;
        }

        public bool CheckAdminPass(Logger logger, Pass pass) {
            if (this == pass) return true;
            if (this.Equals(pass)) return true;
            if (_DebugMode) {
                string errMsg = string.Format("DebugMode Permission Denied: {0} -> {1}", this, pass);
                if (DebugModeLogError) {
                    Log.Error(errMsg);
                } else {
                    Log.Info(errMsg);
                }
                return true;
            }
            return false;
        }

        public bool CheckWritePass(Logger logger, Pass pass) {
            if (_Writable) {
                return true;
            }
            return CheckAdminPass(logger, pass);
        }
    }
}
