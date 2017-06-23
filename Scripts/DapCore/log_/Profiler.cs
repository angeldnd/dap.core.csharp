using System;

namespace angeldnd.dap {
    public interface IProfiler {
        bool BeginSample(string name);
        bool EndSample();
    }
}
