using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public sealed class WeakDataRef {
        private readonly WeakReference _Ref = null;
        public readonly RealData Real = null;

        public WeakDataRef(WeakData weak, RealData real) {
            _Ref = new WeakReference(weak);
            Real = real;
        }

        public bool IsAlive {
            get { return _Ref.IsAlive; }
        }
    }
}
