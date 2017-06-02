using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public class RealDataPool : Pool<RealData> {
        public RealDataPool(int capacity) : base(capacity) {
        }

        protected override RealData NewItem() {
            return new RealData();
        }

        protected override bool CheckAdd(RealData item) {
            if (item == null) return false;

            item._Recycle();
            return true;
        }
    }
}
