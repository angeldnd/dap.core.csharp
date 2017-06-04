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
                NotifySetupWatchers();
                return true;
            }
            Error("Alread Setup: {0} -> {1}", _Handler, handler);
            return false;
        }

        private void NotifySetupWatchers() {
            //SILP: WEAK_LIST_FOREACH_BEGIN(Var.OnSetup, watcher, ISetupWatcher, _SetupWatchers)
            if (_SetupWatchers != null) {                                   //__SILP__
                #if UNITY_EDITOR                                            //__SILP__
                UnityEngine.Profiling.Profiler.BeginSample("Var.OnSetup");  //__SILP__
                #endif                                                      //__SILP__
                bool needGc = false;                                        //__SILP__
                foreach (var r in _SetupWatchers.RetainLock()) {            //__SILP__
                    ISetupWatcher watcher = _SetupWatchers.GetTarget(r);    //__SILP__
                    if (watcher == null) {                                  //__SILP__
                        needGc = true;                                      //__SILP__
                    } else {                                                //__SILP__
                        #if UNITY_EDITOR                                    //__SILP__
                        UnityEngine.Profiling.Profiler.BeginSample(         //__SILP__
                                                watcher.ToString());        //__SILP__
                        #endif                                              //__SILP__
                        watcher.OnSetup(this);
            //SILP: WEAK_LIST_FOREACH_END(Var.OnSetup, watcher, ISetupWatcher, _SetupWatchers)
                        #if UNITY_EDITOR                              //__SILP__
                        UnityEngine.Profiling.Profiler.EndSample();   //__SILP__
                        #endif                                        //__SILP__
                    }                                                 //__SILP__
                }                                                     //__SILP__
                _SetupWatchers.ReleaseLock(needGc);                   //__SILP__
                #if UNITY_EDITOR                                      //__SILP__
                UnityEngine.Profiling.Profiler.EndSample();           //__SILP__
                #endif                                                //__SILP__
            }                                                         //__SILP__
        }

        public bool Setup(Func<Handler, Data, Data> block) {
            return Setup(new BlockRequestHandler(block));
        }

        public Data HandleRequest(Data req) {
            if (req != null) req.Seal();

            if (!IsValid) {
                return ResponseHelper.InternalError(this, req, "Invalid Handler");
            }

            if (!IsValidRequest(req)) {
                _CheckFailedCount++;
                return ResponseHelper.BadRequest(this, req, "Invalid Request");
            }

            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.BeginSample(Key);
            #endif

            NotifyRequestWatchers(req);

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

            NotifyResponseWatchers(req, res);

            if (ResponseHelper.IsResFailed(res)) {
                Error("HandleRequest Failed: {0} -> {1}", req.ToFullString(), res.ToFullString());
            } else if (LogDebug) {
                Debug("HandleRequest: {0} -> {1}", req.ToFullString(), res.ToFullString());
            }
            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.EndSample();
            #endif
            return res;
        }

        private bool IsValidRequest(Data req) {
            bool result = true;
            //SILP: WEAK_LIST_FOREACH_BEGIN(Handler.IsValidRequest, checker, IRequestChecker, _RequestCheckers)
            if (_RequestCheckers != null) {                                            //__SILP__
                #if UNITY_EDITOR                                                       //__SILP__
                UnityEngine.Profiling.Profiler.BeginSample("Handler.IsValidRequest");  //__SILP__
                #endif                                                                 //__SILP__
                bool needGc = false;                                                   //__SILP__
                foreach (var r in _RequestCheckers.RetainLock()) {                     //__SILP__
                    IRequestChecker checker = _RequestCheckers.GetTarget(r);           //__SILP__
                    if (checker == null) {                                             //__SILP__
                        needGc = true;                                                 //__SILP__
                    } else {                                                           //__SILP__
                        #if UNITY_EDITOR                                               //__SILP__
                        UnityEngine.Profiling.Profiler.BeginSample(                    //__SILP__
                                                checker.ToString());                   //__SILP__
                        #endif                                                         //__SILP__
                        if (!checker.IsValidRequest(this, req)) {
                            if (LogDebug) {
                                Debug("Invalid Request: {0} => {1}",
                                        checker, req.ToFullString());
                            }
                            result = false;
                            #if UNITY_EDITOR
                            UnityEngine.Profiling.Profiler.EndSample();
                            #endif
                            break;
                        }
            //SILP: WEAK_LIST_FOREACH_END(Handler.IsValidRequest, checker, IRequestChecker, _RequestCheckers)
                        #if UNITY_EDITOR                              //__SILP__
                        UnityEngine.Profiling.Profiler.EndSample();   //__SILP__
                        #endif                                        //__SILP__
                    }                                                 //__SILP__
                }                                                     //__SILP__
                _RequestCheckers.ReleaseLock(needGc);                 //__SILP__
                #if UNITY_EDITOR                                      //__SILP__
                UnityEngine.Profiling.Profiler.EndSample();           //__SILP__
                #endif                                                //__SILP__
            }                                                         //__SILP__
            return result;
        }

        private void NotifyRequestWatchers(Data req) {
            //SILP: WEAK_LIST_FOREACH_BEGIN(Handler.OnRequest, watcher, IRequestWatcher, _RequestWatchers)
            if (_RequestWatchers != null) {                                       //__SILP__
                #if UNITY_EDITOR                                                  //__SILP__
                UnityEngine.Profiling.Profiler.BeginSample("Handler.OnRequest");  //__SILP__
                #endif                                                            //__SILP__
                bool needGc = false;                                              //__SILP__
                foreach (var r in _RequestWatchers.RetainLock()) {                //__SILP__
                    IRequestWatcher watcher = _RequestWatchers.GetTarget(r);      //__SILP__
                    if (watcher == null) {                                        //__SILP__
                        needGc = true;                                            //__SILP__
                    } else {                                                      //__SILP__
                        #if UNITY_EDITOR                                          //__SILP__
                        UnityEngine.Profiling.Profiler.BeginSample(               //__SILP__
                                                watcher.ToString());              //__SILP__
                        #endif                                                    //__SILP__
                        watcher.OnRequest(this, req);
            //SILP: WEAK_LIST_FOREACH_END(Handler.OnRequest, watcher, IRequestWatcher, _RequestWatchers)
                        #if UNITY_EDITOR                              //__SILP__
                        UnityEngine.Profiling.Profiler.EndSample();   //__SILP__
                        #endif                                        //__SILP__
                    }                                                 //__SILP__
                }                                                     //__SILP__
                _RequestWatchers.ReleaseLock(needGc);                 //__SILP__
                #if UNITY_EDITOR                                      //__SILP__
                UnityEngine.Profiling.Profiler.EndSample();           //__SILP__
                #endif                                                //__SILP__
            }                                                         //__SILP__
        }

        private void NotifyResponseWatchers(Data req, Data res) {
            //SILP: WEAK_LIST_FOREACH_BEGIN(Handler.OnResponse, watcher, IResponseWatcher, _ResponseWatchers)
            if (_ResponseWatchers != null) {                                       //__SILP__
                #if UNITY_EDITOR                                                   //__SILP__
                UnityEngine.Profiling.Profiler.BeginSample("Handler.OnResponse");  //__SILP__
                #endif                                                             //__SILP__
                bool needGc = false;                                               //__SILP__
                foreach (var r in _ResponseWatchers.RetainLock()) {                //__SILP__
                    IResponseWatcher watcher = _ResponseWatchers.GetTarget(r);     //__SILP__
                    if (watcher == null) {                                         //__SILP__
                        needGc = true;                                             //__SILP__
                    } else {                                                       //__SILP__
                        #if UNITY_EDITOR                                           //__SILP__
                        UnityEngine.Profiling.Profiler.BeginSample(                //__SILP__
                                                watcher.ToString());               //__SILP__
                        #endif                                                     //__SILP__
                        watcher.OnResponse(this, req, res);
            //SILP: WEAK_LIST_FOREACH_END(Handler.OnResponse, watcher, IResponseWatcher, _ResponseWatchers)
                        #if UNITY_EDITOR                              //__SILP__
                        UnityEngine.Profiling.Profiler.EndSample();   //__SILP__
                        #endif                                        //__SILP__
                    }                                                 //__SILP__
                }                                                     //__SILP__
                _ResponseWatchers.ReleaseLock(needGc);                //__SILP__
                #if UNITY_EDITOR                                      //__SILP__
                UnityEngine.Profiling.Profiler.EndSample();           //__SILP__
                #endif                                                //__SILP__
            }                                                         //__SILP__
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
