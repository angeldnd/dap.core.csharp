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
        Vars Vars { get; }
        Manners Manners { get; }

        /* ContextExtension.cs
        string[] GetKeys();
         */
    }

    public interface IDictContext : IContext, IDict {
    }
}
