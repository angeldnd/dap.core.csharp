using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IVarWatcher {
        void OnChanged(IVar v);
    }

    public interface IValueWatcher {
    }

    public interface IValueWatcher<T> : IValueWatcher {
        void OnChanged(IVar<T> v, T oldValue);
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
