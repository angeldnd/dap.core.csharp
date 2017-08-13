using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IContextElement : IElement {
        IContext GetContext();
        string Path { get; }
        bool Debugging { get; set; }
    }

    public interface IContext : IOwner, IInDictElement, IContextElement, IMapping {
        Properties Properties { get; }
        Channels Channels { get; }
        Handlers Handlers { get; }
        Bus Bus { get; }
        Vars Vars { get; }
        Utils Utils { get; }
        Manners Manners { get; }
        bool Removed { get; }

        bool HasMappings();
        Mappings Mappings { get; }

        T GetAspect<T>(string aspectPath, bool isDebug = false) where T : class, IAspect;
        IAspect GetAspect(string aspectPath, bool isDebug = false);

        void ForEachTopAspects(Action<IAspect> callback);
        void ForEachAspects(Action<IAspect> callback);

        void AddDetailFields(Data summary);
        void AddDescendants(Data summary, bool treeMode);
    }

    public interface IDictContext : IContext, IDict {
    }
}
