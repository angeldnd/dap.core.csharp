using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IOwner : IObject {
    }

    public interface IElement : IObject {
        IOwner GetOwner();

        string Key { get; }

        bool IsOrphan { get; }
        void _OnAdded(IOwner owner);
        void _OnRemoved(IOwner owner);
    }

    public interface IElement<TO> : IElement
                                        where TO : class, IOwner {
        TO Owner { get; }
    }
}
