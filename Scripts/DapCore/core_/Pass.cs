using System;

namespace angeldnd.dap {
    public abstract class Pass {
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
            return string.Format("[{0}:{1}]", GetType().Name, _HashCode);
        }

        public override bool Equals(object obj) {
            if (this == obj) return true;
            if (obj.GetType() != this.GetType()) return false;
            if (_HashCode == obj.GetHashCode()) return true;
            return false;
        }

        public override int GetHashCode() {
            return _HashCode;
        }

        public bool CheckAdminPass(Pass pass) {
            if (this == pass) return true;
            if (this.Equals(pass)) return true;
        }

        public virtual bool CheckWritePass(Pass pass) {
            return CheckAdminPass(pass);
        }
    }

    public class OpenPass {
        public OpenPass(int hashCode) : base(hashCode) {
        }

        public OpenPass(object obj) : base(obj) {
        }

        public override bool CheckWritePass(Pass pass) {
            return true;
        }
    }
}
