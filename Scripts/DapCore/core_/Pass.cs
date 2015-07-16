using System;

namespace angeldnd.dap {
    public class Pass {
        private int _HashCode = Guid.NewGuid().GetHashCode();

        public override string ToString() {
            return string.Format("[Pass:{0}]", _HashCode);
        }

        /* Only exact same refrence is equal */
        public override bool Equals(object obj) {
            return this == obj;
        }

        public override int GetHashCode() {
            return _HashCode;
        }
    }
}
