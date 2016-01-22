using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;

namespace angeldnd.dap {
    public static class RegistryConsts {
        public const string TypeRegistry = "Registry";
    }

    public sealed class Registry : TreeInTreeContext<Env, IInTreeContext> {
        public override string Type {
            get { return RegistryConsts.TypeRegistry; }
        }

        public Registry(Env owner, string path, Pass pass) : base(owner, path, pass) {
        }
    }
}
