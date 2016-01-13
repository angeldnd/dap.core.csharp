using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IOwner : IObject {
    }

    public interface IElement : IObject {
        IOwner GetOwner();

        void OnAdded();
        void OnRemoved();
    }

    public interface IElement<TO> : IElement
                                        where TO : IOwner {
        TO Owner { get; }
    }
}
