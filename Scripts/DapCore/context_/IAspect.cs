using System;

namespace angeldnd.dap {
    public interface IAspect : IContextElement {
        IContext Context { get; }
    }

    public interface ISetupAspect : IAspect {
        int SetupWatcherCount { get; }
        bool AddSetupWatcher(ISetupWatcher watcher);
        bool RemoveSetupWatcher(ISetupWatcher watcher);
        BlockSetupWatcher AddSetupWatcher(IBlockOwner owner, Action<ISetupAspect> block);

        bool NeedSetup();
    }

    public interface ISetupWatcher : IBlock {
        void OnSetup(ISetupAspect handler);
    }

    public sealed class BlockSetupWatcher : WeakBlock, ISetupWatcher {
        private readonly Action<ISetupAspect> _Block;

        public BlockSetupWatcher(IBlockOwner owner, Action<ISetupAspect> block) : base(owner) {
            _Block = block;
        }

        public void OnSetup(ISetupAspect aspect) {
            _Block(aspect);
        }
    }
}
