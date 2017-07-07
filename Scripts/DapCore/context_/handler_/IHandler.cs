using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IHandler {
        int RequestCheckerCount { get; }
        bool AddRequestChecker(IRequestChecker checker);
        bool RemoveRequestChecker(IRequestChecker checker);
        BlockRequestChecker AddRequestChecker(IBlockOwner owner, Func<Handler, Data, bool> block);

        int RequestWatcherCount { get; }
        bool AddRequestWatcher(IRequestWatcher watcher);
        bool RemoveRequestWatcher(IRequestWatcher watcher);
        BlockRequestWatcher AddRequestWatcher(IBlockOwner owner, Action<Handler, Data> block);

        int ResponseWatcherCount { get; }
        bool AddResponseWatcher(IResponseWatcher watcher);
        bool RemoveResponseWatcher(IResponseWatcher watcher);
        BlockResponseWatcher AddResponseWatcher(IBlockOwner owner, Action<Handler, Data, Data> block);

        bool IsValid { get; }
        bool Setup(IRequestHandler handler);
        Data HandleRequest(Data req);
    }

    public interface IRequestChecker : IBlock {
        bool IsValidRequest(Handler handler, Data req);
    }

    public interface IRequestWatcher : IBlock {
        void OnRequest(Handler handler, Data req);
    }

    public interface IResponseWatcher : IBlock {
        void OnResponse(Handler handler, Data req, Data res);
    }

    public interface IRequestHandler {
        Data DoHandle(Handler handler, Data req);
    }

    public sealed class BlockRequestChecker : WeakBlock, IRequestChecker {
        private readonly Func<Handler, Data, bool> _Block;

        public BlockRequestChecker(IBlockOwner owner, Func<Handler, Data, bool> block) : base(owner) {
            _Block = block;
        }

        public bool IsValidRequest(Handler handler, Data req) {
            return _Block(handler, req);
        }
    }

    public sealed class BlockRequestWatcher : WeakBlock, IRequestWatcher {
        private readonly Action<Handler, Data> _Block;

        public BlockRequestWatcher(IBlockOwner owner, Action<Handler, Data> block) : base(owner) {
            _Block = block;
        }

        public void OnRequest(Handler handler, Data req) {
            _Block(handler, req);
        }
    }

    public sealed class BlockResponseWatcher : WeakBlock, IResponseWatcher {
        private readonly Action<Handler, Data, Data> _Block;

        public BlockResponseWatcher(IBlockOwner owner, Action<Handler, Data, Data> block) : base(owner) {
            _Block = block;
        }

        public void OnResponse(Handler handler, Data req, Data res) {
            _Block(handler, req, res);
        }
    }

    /* The BlockRequestHandler should NOT be weak */
    public sealed class BlockRequestHandler : IRequestHandler {
        private readonly Func<Handler, Data, Data> _Block;

        public BlockRequestHandler(Func<Handler, Data, Data> block) {
            _Block = block;
        }

        public Data DoHandle(Handler handler, Data req) {
            try {
                return _Block(handler, req);
            } catch (HandlerException e) {
                return e.Response;
            }
        }
    }
}
