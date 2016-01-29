using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IOwner : IObject {
    }

    public interface IElement : IObject {
        IOwner GetOwner();

        string Key { get; }

        void OnAdded();
        void OnRemoved();
    }

    public interface IElement<TO> : IElement
                                        where TO : class, IOwner {
        TO Owner { get; }
    }
}
