using System;

namespace angeldnd.dap {
    public interface IAspect : IContextElement {
        IContext Context { get; }
        string Path { get; }
        bool Debugging { get; set; }
    }
}
