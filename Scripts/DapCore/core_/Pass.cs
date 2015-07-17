using System;

namespace angeldnd.dap {
    public sealed class Pass {
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

        public bool CheckAdminPass(Pass pass) {
            if (this == pass) return true;
            if (this.Equals(pass)) return true;
            return false;
        }

        public bool CheckWritePass(Pass pass) {
            if (_Writable) {
                return true;
            }
            return CheckAdminPass(pass);
        }
    }
}
