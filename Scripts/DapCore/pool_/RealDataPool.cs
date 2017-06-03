using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public class RealDataPool : Pool<RealData> {
        public RealDataPool(int capacity) : base(capacity) {
        }

        protected override RealData NewItem() {
            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.BeginSample("RealDataPool.NewItem");
            #endif
            RealData result = new RealData();
            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.EndSample();
            #endif
            return result;
        }

        protected override bool CheckAdd(RealData item) {
            if (item == null) return false;

            item._Recycle();
            return true;
        }
    }
}
