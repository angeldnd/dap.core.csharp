using System;

namespace angeldnd.dap {
    public interface IAspect : IElement, IEntityAccessor {
    }

    public interface IAspect<TO> : IElement<TO>, IAspect
                                            where TO : ISection {
    }

    public interface IInTreeAspect : IAspect, IInTreeElement {
    }

    public interface IInTreeAspect<TO> : IInTreeElement<TO>, IInTreeAspect
                                            where TO : ITreeSection {
    }

    public interface IInTableAspect : IAspect, IInTableElement {
    }

    public interface IInTableAspect<TO> : IInTableElement<TO>, IInTableAspect
                                            where TO : ITableSection {
    }
}
