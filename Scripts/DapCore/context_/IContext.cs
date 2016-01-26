using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IContextAccessor {
        IContext GetContext();
    }

    public interface IContext : IOwner, IInDictElement, IContextAccessor {
        string Path { get; }

        Properties Properties { get; }
        Channels Channels { get; }
        Handlers Handlers { get; }
        Vars Vars { get; }
        Manners Manners { get; }

        void SetDebugMode(bool debugMode);
        void SetDebugPatterns(string[] patterns);

        /* ContextExtension.cs
        string[] GetKeys();
         */
    }

    public interface IDictContext : IContext, IDict {
    }
}
