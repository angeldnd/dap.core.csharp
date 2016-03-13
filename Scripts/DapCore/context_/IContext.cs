using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IContextAccessor {
        IContext GetContext();
    }

    public interface IContext : IOwner, IInDictElement, IContextAccessor {
        string Path { get; }

        bool Debugging { get; set; }

        Properties Properties { get; }
        Channels Channels { get; }
        Handlers Handlers { get; }
        Bus Bus { get; }
        Vars Vars { get; }
        Manners Manners { get; }

        void ForEachTopAspects(Action<IAspect> callback);
        void ForEachAspects(Action<IAspect> callback);
    }


    public interface IDictContext : IContext, IDict {
    }
}
