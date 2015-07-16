using System;

namespace angeldnd.dap {
    public class Pass {
        private readonly int _HashCode;

        public Pass() {
            _HashCode = Guid.NewGuid().GetHashCode();
        }

        public Pass(int hashCode) {
            _HashCode = hashCode;
        }

        public Pass(object obj) {
            _HashCode = obj.GetHashCode();
        }

        public override string ToString() {
            return string.Format("[Pass:{0}]", _HashCode);
        }

        public override bool Equals(object obj) {
            if (this == obj) return true;
            if (_HashCode == obj.GetHashCode()) return true;
            return false;
        }

        public override int GetHashCode() {
            return _HashCode;
        }
    }
}
