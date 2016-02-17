using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;

namespace angeldnd.dap {
    public static class RegistryConsts {
        public const string TypeRegistry = "Registry";
    }

    [DapType(RegistryConsts.TypeRegistry)]
    [DapOrder(-9)]
    public sealed class Registry : DictContext<Env, IContext> {
        public Registry(Env owner, string key) : base(owner, key) {
            Channels.Add(EnvConsts.ChannelTick);
        }
    }
}
