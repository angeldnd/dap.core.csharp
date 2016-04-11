using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public sealed class Handler : InDictAspect<Handlers>, IHandler {
        public Handler(Handlers owner, string key) : base(owner, key) {
        }

        private IRequestHandler _Handler = null;

        public bool IsValid {
            get { return _Handler == null; }
        }

        public bool Setup(IRequestHandler handler) {
            if (_Handler == null) {
                _Handler = handler;
                return true;
            }
            Error("Alread Setup: {0} -> {1}", _Handler, handler);
            return false;
        }

        public bool Setup(Func<Handler, Data, bool> block) {
            return Setup(new BlockRequestHandler(block));
        }

        public Data HandleRequest(Data req) {
            if (_Handler == null) return null;

            if (!WeakListHelper.IsValid(_RequestCheckers, (IRequestChecker checker) => {
                return checker.IsValidRequest(this, req);
            })) {
                return null;
            }

            WeakListHelper.Notify(_RequestWatchers, (IRequestWatcher listener) => {
                listener.OnRequest(this, req);
            });

            Data res = _Handler.DoHandle(this, req);
            AdvanceRevision();

            WeakListHelper.Notify(_ResponseWatchers, (IResponseWatcher listener) => {
                listener.OnResponse(this, req, res);
            });
            return res;
        }

        public BlockRequestChecker AddRequestChecker(IBlockOwner owner, Func<Handler, Data, bool> block) {
            BlockRequestChecker result = new BlockRequestChecker(owner, block);
            if (AddRequestChecker(result)) {
                return result;
            }
            return null;
        }

        public BlockRequestWatcher AddRequestWatcher(IBlockOwner owner, Action<Handler, Data> block) {
            BlockRequestWatcher result = new BlockRequestWatcher(owner, block);
            if (AddRequestWatcher(result)) {
                return result;
            }
            return null;
        }

        public BlockResponseWatcher AddResponseWatcher(IBlockOwner owner, Action<Handler, Data, Data> block) {
            BlockResponseWatcher result = new BlockResponseWatcher(owner, block);
            if (AddResponseWatcher(result)) {
                return result;
            }
            return null;
        }

        protected override void AddSummaryFields(Data summary) {
            base.AddSummaryFields(summary);
            summary.I(ContextConsts.SummaryCheckerCount, RequestCheckerCount)
                   .I(ContextConsts.SummaryWatcherCount, RequestWatcherCount)
                   .I(ContextConsts.Summary2ndWatcherCount, ResponseWatcherCount);
        }

        //SILP: DECLARE_LIST(RequestChecker, checker, IRequestChecker, _RequestCheckers)
        private WeakList<IRequestChecker> _RequestCheckers = null;     //__SILP__
                                                                       //__SILP__
        public int RequestCheckerCount {                               //__SILP__
            get { return WeakListHelper.Count(_RequestCheckers); }     //__SILP__
        }                                                              //__SILP__
                                                                       //__SILP__
        public bool AddRequestChecker(IRequestChecker checker) {       //__SILP__
            return WeakListHelper.Add(ref _RequestCheckers, checker);  //__SILP__
        }                                                              //__SILP__
                                                                       //__SILP__
        public bool RemoveRequestChecker(IRequestChecker checker) {    //__SILP__
            return WeakListHelper.Remove(_RequestCheckers, checker);   //__SILP__
        }                                                              //__SILP__

        //SILP: DECLARE_LIST(RequestWatcher, listener, IRequestWatcher, _RequestWatchers)
        private WeakList<IRequestWatcher> _RequestWatchers = null;      //__SILP__
                                                                        //__SILP__
        public int RequestWatcherCount {                                //__SILP__
            get { return WeakListHelper.Count(_RequestWatchers); }      //__SILP__
        }                                                               //__SILP__
                                                                        //__SILP__
        public bool AddRequestWatcher(IRequestWatcher listener) {       //__SILP__
            return WeakListHelper.Add(ref _RequestWatchers, listener);  //__SILP__
        }                                                               //__SILP__
                                                                        //__SILP__
        public bool RemoveRequestWatcher(IRequestWatcher listener) {    //__SILP__
            return WeakListHelper.Remove(_RequestWatchers, listener);   //__SILP__
        }                                                               //__SILP__

        //SILP: DECLARE_LIST(ResponseWatcher, listener, IResponseWatcher, _ResponseWatchers)
        private WeakList<IResponseWatcher> _ResponseWatchers = null;     //__SILP__
                                                                         //__SILP__
        public int ResponseWatcherCount {                                //__SILP__
            get { return WeakListHelper.Count(_ResponseWatchers); }      //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        public bool AddResponseWatcher(IResponseWatcher listener) {      //__SILP__
            return WeakListHelper.Add(ref _ResponseWatchers, listener);  //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        public bool RemoveResponseWatcher(IResponseWatcher listener) {   //__SILP__
            return WeakListHelper.Remove(_ResponseWatchers, listener);   //__SILP__
        }                                                                //__SILP__
    }
}
