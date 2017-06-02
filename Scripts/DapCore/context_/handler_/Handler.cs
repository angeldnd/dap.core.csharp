using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public sealed class Handler : InDictAspect<Handlers>, IHandler, ISetupAspect {
        public Handler(Handlers owner, string key) : base(owner, key) {
        }

        private IRequestHandler _Handler = null;
        private int _CheckFailedCount = 0;

        public bool IsValid {
            get { return _Handler != null; }
        }

        public bool NeedSetup() {
            return _Handler == null;
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
            if (req != null) req.Seal();

            if (!IsValid) {
                return ResponseHelper.InternalError(this, req, "Invalid Handler");
            }

            _HandleRequest_Req = req;
            _HandleRequest_LastChecker = null;

            if (!WeakListHelper.IsValid(_RequestCheckers, HandleRequest_Checker)) {
                if (LogDebug) {
                    Debug("Invalid Request: {0} => {1}", _HandleRequest_LastChecker, req.ToFullString());
                }
                _CheckFailedCount++;
                return ResponseHelper.BadRequest(this, req, "Invalid Request");
            }

            WeakListHelper.Notify(_RequestWatchers, HandleRequest_RequestWatcher);

            Data res = null;
            try {
                res = _Handler.DoHandle(this, req);
            } catch (HandlerException e) {
                res = e.Response;
            } catch (Exception e) {
                Error("DoHandle Got Exception: {0}\n{1}", e.Message, e.ToString());
                if (LogDebug) {
                    res = ResponseHelper.InternalError(this, req, e.ToString());
                } else {
                    res = ResponseHelper.InternalError(this, req, e.Message);
                }
            }
            if (res != null) res.Seal();
            AdvanceRevision();
            _HandleRequest_Res = res;

            WeakListHelper.Notify(_ResponseWatchers, HandleRequest_ResponseWatcher);

            if (ResponseHelper.IsResFailed(res)) {
                Error("HandleRequest Failed: {0} -> {1}", req.ToFullString(), res.ToFullString());
            } else if (LogDebug) {
                Debug("HandleRequest: {0} -> {1}", req.ToFullString(), res.ToFullString());
            }
            return res;
        }

        private Data _HandleRequest_Req = null;
        private IRequestChecker _HandleRequest_LastChecker = null;

        private Func<IRequestChecker, bool> _HandleRequest_Checker = null;
        private Func<IRequestChecker, bool> HandleRequest_Checker {
            get {
                if (_HandleRequest_Checker == null) {
                    _HandleRequest_Checker = new Func<IRequestChecker, bool>((IRequestChecker checker) => {
                        #if UNITY_EDITOR
                        UnityEngine.Profiling.Profiler.BeginSample("Handler.IsValidRequest: " + Key + " " + checker.ToString());
                        #endif
                        _HandleRequest_LastChecker = checker;
                        bool result = checker.IsValidRequest(this, _HandleRequest_Req);
                        #if UNITY_EDITOR
                        UnityEngine.Profiling.Profiler.EndSample();
                        #endif
                        return result;
                    });
                }
                return _HandleRequest_Checker;
            }
        }

        private Action<IRequestWatcher> _HandleRequest_RequestWatcher = null;
        private Action<IRequestWatcher> HandleRequest_RequestWatcher {
            get {
                if (_HandleRequest_RequestWatcher == null) {
                    _HandleRequest_RequestWatcher = new Action<IRequestWatcher>((IRequestWatcher watcher) => {
                        #if UNITY_EDITOR
                        UnityEngine.Profiling.Profiler.BeginSample("Handler.OnRequest: " + Key + " " + watcher.ToString());
                        #endif
                        watcher.OnRequest(this, _HandleRequest_Req);
                        #if UNITY_EDITOR
                        UnityEngine.Profiling.Profiler.EndSample();
                        #endif
                    });
                }
                return _HandleRequest_RequestWatcher;
            }
        }

        private Data _HandleRequest_Res = null;

        private Action<IResponseWatcher> _HandleRequest_ResponseWatcher = null;
        private Action<IResponseWatcher> HandleRequest_ResponseWatcher {
            get {
                if (_HandleRequest_ResponseWatcher == null) {
                    _HandleRequest_ResponseWatcher = new Action<IResponseWatcher>((IResponseWatcher watcher) => {
                        #if UNITY_EDITOR
                        UnityEngine.Profiling.Profiler.BeginSample("Handler.OnResponse: " + Key + " " + watcher.ToString());
                        #endif
                        watcher.OnResponse(this, _HandleRequest_Req, _HandleRequest_Res);
                        #if UNITY_EDITOR
                        UnityEngine.Profiling.Profiler.EndSample();
                        #endif
                    });
                }
                return _HandleRequest_ResponseWatcher;
            }
        }

        public BlockSetupWatcher AddSetupWatcher(IBlockOwner owner, Action<ISetupAspect> block) {
            BlockSetupWatcher result = new BlockSetupWatcher(owner, block);
            if (AddSetupWatcher(result)) {
                return result;
            }
            return null;
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
                   .I(ContextConsts.Summary3rdWatcherCount, SetupWatcherCount)
                   .I(ContextConsts.SummaryCheckFailedCount, _CheckFailedCount);
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
