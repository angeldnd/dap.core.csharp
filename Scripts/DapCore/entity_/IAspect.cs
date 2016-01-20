using System;

namespace angeldnd.dap {
    public interface IAspect : IElement, IEntityAccessor {
    }

    public interface IAspect<TO> : IElement<TO>, IAspect
                                            where TO : ISection {
    }
}
