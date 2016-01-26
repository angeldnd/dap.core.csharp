using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IChannel : IInDictAspect {
        int EventCheckerCount { get; }
        bool AddEventChecker(IEventChecker listener);
        bool RemoveEventChecker(IEventChecker listener);
        BlockEventChecker AddEventChecker(IBlockOwner owner, Func<Channel, Data, bool> block);

        int EventWatcherCount { get; }
        bool AddEventWatcher(IEventWatcher listener);
        bool RemoveEventWatcher(IEventWatcher listener);
        BlockEventWatcher AddEventWatcher(IBlockOwner owner, Action<Channel, Data> block);

        bool FireEvent(Data evt);
    }

    public interface IEventChecker {
        bool IsValidEvent(Channel channel, Data evt);
    }

    public interface IEventWatcher {
        void OnEvent(Channel channel, Data evt);
    }

    public sealed class BlockEventChecker : WeakBlock, IEventChecker {
        private readonly Func<Channel, Data, bool> _Block;

        public BlockEventChecker(IBlockOwner owner, Func<Channel, Data, bool> block) : base(owner) {
            _Block = block;
        }

        public bool IsValidEvent(Channel channel, Data evt) {
            return _Block(channel, evt);
        }
    }

    public sealed class BlockEventWatcher : WeakBlock, IEventWatcher {
        private readonly Action<Channel, Data> _Block;

        public BlockEventWatcher(IBlockOwner owner, Action<Channel, Data> block) : base(owner) {
            _Block = block;
        }

        public void OnEvent(Channel channel, Data evt) {
            _Block(channel, evt);
        }
    }
}
