using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public class WeakDataRefPool : Pool<WeakDataRef> {
        public WeakDataRefPool() : base() {
        }

        protected override bool CheckAdd(WeakDataRef item) {
            if (item == null) return false;

            item._Recycle();
            return true;
        }
    }
}
