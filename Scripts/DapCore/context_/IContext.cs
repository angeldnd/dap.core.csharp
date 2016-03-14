using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IContextElement {
        IContext GetContext();
        string Path { get; }
        bool Debugging { get; set; }
    }

    public interface IContext : IOwner, IInDictElement, IContextElement {
        Properties Properties { get; }
        Channels Channels { get; }
        Handlers Handlers { get; }
        Bus Bus { get; }
        Vars Vars { get; }
        Manners Manners { get; }

        T GetAspect<T>(string aspectPath, bool logError) where T : class, IAspect;
        IAspect GetAspect(string aspectPath, bool logError);

        void ForEachTopAspects(Action<IAspect> callback);
        void ForEachAspects(Action<IAspect> callback);
    }


    public interface IDictContext : IContext, IDict {
    }
}
