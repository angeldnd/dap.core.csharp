using System;

namespace angeldnd.dap {
    public interface IManner : IAspect, IInDictElement {
        Mapping Mapping { get; }
        Properties Properties { get; }
        Channels Channels { get; }
        Handlers Handlers { get; }
        Bus Bus { get; }
        Vars Vars { get; }
        Manners Manners { get; }
    }

    public interface IDictManner : IManner, IDict {
    }
}
