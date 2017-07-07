using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IVarWatcher : IBlock {
        void OnChanged(IVar v);
    }

    public sealed class BlockVarWatcher : WeakBlock, IVarWatcher {
        private readonly Action<IVar> _Watcher;

        public BlockVarWatcher(IBlockOwner owner, Action<IVar> watcher) : base(owner) {
            _Watcher = watcher;
        }

        public void OnChanged(IVar v) {
            _Watcher(v);
        }
    }
}
