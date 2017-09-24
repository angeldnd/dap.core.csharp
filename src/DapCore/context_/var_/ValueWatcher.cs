using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IValueWatcher : IBlock {
    }

    public interface IValueWatcher<T> : IValueWatcher {
        void OnChanged(IVar<T> v, T oldValue);
    }

    public sealed class BlockValueWatcher<T> : WeakBlock, IValueWatcher<T> {
        private readonly Action<IVar<T>, T> Watcher;

        public BlockValueWatcher(IBlockOwner owner, Action<IVar<T>, T> watcher) : base(owner) {
            Watcher = watcher;
        }

        public void OnChanged(IVar<T> v, T oldValue) {
            Watcher(v, oldValue);
        }
    }
}
