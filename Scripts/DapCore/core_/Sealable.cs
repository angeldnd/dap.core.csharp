using System;

namespace angeldnd.dap {
    public interface ISealable {
        bool Sealed { get; }
        void Seal();
    }

    public abstract class Sealable : ISealable {
        private bool _Sealed = false;
        public bool Sealed {
            get { return _Sealed; }
        }

        public void Seal() {
            _Sealed = true;
            OnSeal();
        }

        protected virtual void OnSeal() {}
    }
}
