using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public sealed class Handler : InDictAspect<Handlers>, IHandler {
        public Handler(Handlers owner, string key) : base(owner, key) {
        }

        private IRequestHandler _Handler = null;
        private int _SucceedCount = 0;
        private int _FailedCount = 0;

        public bool IsValid {
            get { return _Handler != null; }
        }

        public bool Setup(IRequestHandler handler) {
            if (_Handler == null) {
                _Handler = handler;
                WeakListHelper.Notify(_SetupWatchers, (ISetupWatcher watcher) => {
                    watcher.OnSetup(this);
                });
                return true;
            }
            Error("Alread Setup: {0} -> {1}", _Handler, handler);
            return false;
        }

        public bool Setup(Func<Handler, Data, Data> block) {
            return Setup(new BlockRequestHandler(block));
        }

        public Data HandleRequest(Data req) {
            if (!IsValid) {
                Error("Invalid Handler: {0}", req);
                return null;
            }

            IRequestChecker lastChecker = null;
            if (!WeakListHelper.IsValid(_RequestCheckers, (IRequestChecker checker) => {
                lastChecker = checker;
                return checker.IsValidRequest(this, req);
            })) {
                if (LogDebug) {
                    Debug("Invalid Request: {0} => {1}", lastChecker, Data.ToFullString(req));
                }
                return null;
            }

            WeakListHelper.Notify(_RequestWatchers, (IRequestWatcher watcher) => {
                watcher.OnRequest(this, req);
            });

            Data res = _Handler.DoHandle(this, req);
            AdvanceRevision();

            WeakListHelper.Notify(_ResponseWatchers, (IResponseWatcher watcher) => {
                watcher.OnResponse(this, req, res);
            });
            if (LogDebug) {
                Debug("HandleRequest: {0} -> {1}", Data.ToFullString(req), Data.ToFullString(res));
            }
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
            summary.B(ContextConsts.SummaryIsValid, IsValid)
                   .I(ContextConsts.SummaryCheckerCount, RequestCheckerCount)
                   .I(ContextConsts.SummaryWatcherCount, RequestWatcherCount)
                   .I(ContextConsts.Summary2ndWatcherCount, ResponseWatcherCount)
                   .I(ContextConsts.Summary3rdWatcherCount, SetupWatcherCount);
        }

        //SILP: DECLARE_LIST(SetupWatcher, watcher, ISetupWatcher, _SetupWatchers)
        private WeakList<ISetupWatcher> _SetupWatchers = null;        //__SILP__
                                                                      //__SILP__
        public int SetupWatcherCount {                                //__SILP__
            get { return WeakListHelper.Count(_SetupWatchers); }      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool AddSetupWatcher(ISetupWatcher watcher) {          //__SILP__
            return WeakListHelper.Add(ref _SetupWatchers, watcher);   //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool RemoveSetupWatcher(ISetupWatcher watcher) {       //__SILP__
            return WeakListHelper.Remove(_SetupWatchers, watcher);    //__SILP__
        }                                                             //__SILP__

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

        //SILP: DECLARE_LIST(RequestWatcher, watcher, IRequestWatcher, _RequestWatchers)
        private WeakList<IRequestWatcher> _RequestWatchers = null;     //__SILP__
                                                                       //__SILP__
        public int RequestWatcherCount {                               //__SILP__
            get { return WeakListHelper.Count(_RequestWatchers); }     //__SILP__
        }                                                              //__SILP__
                                                                       //__SILP__
        public bool AddRequestWatcher(IRequestWatcher watcher) {       //__SILP__
            return WeakListHelper.Add(ref _RequestWatchers, watcher);  //__SILP__
        }                                                              //__SILP__
                                                                       //__SILP__
        public bool RemoveRequestWatcher(IRequestWatcher watcher) {    //__SILP__
            return WeakListHelper.Remove(_RequestWatchers, watcher);   //__SILP__
        }                                                              //__SILP__

        //SILP: DECLARE_LIST(ResponseWatcher, watcher, IResponseWatcher, _ResponseWatchers)
        private WeakList<IResponseWatcher> _ResponseWatchers = null;    //__SILP__
                                                                        //__SILP__
        public int ResponseWatcherCount {                               //__SILP__
            get { return WeakListHelper.Count(_ResponseWatchers); }     //__SILP__
        }                                                               //__SILP__
                                                                        //__SILP__
        public bool AddResponseWatcher(IResponseWatcher watcher) {      //__SILP__
            return WeakListHelper.Add(ref _ResponseWatchers, watcher);  //__SILP__
        }                                                               //__SILP__
                                                                        //__SILP__
        public bool RemoveResponseWatcher(IResponseWatcher watcher) {   //__SILP__
            return WeakListHelper.Remove(_ResponseWatchers, watcher);   //__SILP__
        }                                                               //__SILP__
    }
}
