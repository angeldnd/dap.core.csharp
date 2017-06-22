using System;

namespace angeldnd.dap {
    public interface IProfiler {
        void BeginSample(string name);
        void EndSample();
    }
}
