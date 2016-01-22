using System;

namespace angeldnd.dap {
    public interface IAspect : IElement, IContextAccessor {
        IContext Context { get; }
    }

    public interface IInTreeAspect : IAspect, IInTreeElement {
    }

    public interface IInTableAspect : IAspect, IInTableElement {
    }
}
