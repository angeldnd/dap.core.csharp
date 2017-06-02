using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public class RealDataPool : Pool<RealData> {
        public const int Default_Capacity = 64;
        public const int Default_TypesCapacity = 64;

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
