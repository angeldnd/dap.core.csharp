using System;

namespace angeldnd.dap {
    public interface IAspect : IElement, IContextAccessor {
        IContext Context { get; }
    }

    public interface IInDictAspect : IAspect, IInDictElement {
    }

    public interface IInTableAspect : IAspect, IInTableElement {
    }
}
