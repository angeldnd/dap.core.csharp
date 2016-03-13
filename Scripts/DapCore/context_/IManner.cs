using System;

namespace angeldnd.dap {
    public interface IManner {
        Properties Properties { get; }
        Channels Channels { get; }
        Handlers Handlers { get; }
        Bus Bus { get; }
        Vars Vars { get; }
        Manners Manners { get; }
    }
}
