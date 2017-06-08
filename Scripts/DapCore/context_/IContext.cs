using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IContextElement : IElement {
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
        Utils Utils { get; }
        Manners Manners { get; }
        bool Removed { get; }

        T GetAspect<T>(string aspectPath, bool isDebug = false) where T : class, IAspect;
        IAspect GetAspect(string aspectPath, bool isDebug = false);

        void ForEachTopAspects(Action<IAspect> callback);
        void ForEachAspects(Action<IAspect> callback);

        void AddDetailFields(Data summary);
    }

    public interface IDictContext : IContext, IDict {
    }
}
