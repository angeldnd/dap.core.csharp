using System;

namespace angeldnd.dap {
    public interface Sealable {
        bool Sealed { get; }
        void Seal();
    }

    public class BaseSealable : Sealable {
        private bool _Sealed = false;
        public bool Sealed {
            get { return _Sealed; }
        }

        public void Seal() {
            _Sealed = true;
        }
    }
}
