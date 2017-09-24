using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public sealed class WeakDataRef {
        private WeakReference _Ref = null;

        private RealData _Real = null;
        public RealData Real {
            get { return _Real; }
        }

        public WeakDataRef(WeakData weak, RealData real) {
            _Ref = new WeakReference(weak);
            _Real = real;
        }

        public void _Reuse(WeakData weak, RealData real) {
            _Ref.Target = weak;
            _Real = real;
        }

        public void _Recycle() {
            _Real = null;
        }

        public bool IsAlive {
            get { return _Ref.IsAlive; }
        }
    }
}
