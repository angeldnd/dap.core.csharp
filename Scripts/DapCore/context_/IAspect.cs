using System;

namespace angeldnd.dap {
    public interface IAspect : IContextElement {
        IContext Context { get; }
    }
}
