using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public sealed class Channel : InDictAspect<Channels>, IChannel {
        public Channel(Channels owner, string key) : base(owner, key) {
        }

        public bool FireEvent(Data evt) {
            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.BeginSample("Channel.FireEvent: " + Key);
            #endif

            if (evt != null) evt.Seal();

            _FireEvent_Evt = evt;
            _FireEvent_LastChecker = null;

            bool isValid = true;
            if (!WeakListHelper.IsValid(_EventCheckers, FireEvent_Checker)) {
                if (LogDebug) {
                    Debug("Invalid Event: {0} => {1}", _FireEvent_LastChecker, evt.ToFullString());
                }
                isValid = false;
            }
            if (isValid) {
                AdvanceRevision();

                WeakListHelper.Notify(_EventWatchers, FireEvent_Watcher);
                if (LogDebug) {
                    Debug("FireEvent: {0}", evt.ToFullString());
                }
            }
            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.EndSample();
            #endif
            return isValid;
        }

        private Data _FireEvent_Evt = null;
        private IEventChecker _FireEvent_LastChecker = null;

        private Func<IEventChecker, bool> _FireEvent_Checker = null;
        private Func<IEventChecker, bool> FireEvent_Checker {
            get {
                if (_FireEvent_Checker == null) {
                    _FireEvent_Checker = new Func<IEventChecker, bool>((IEventChecker checker) => {
                        #if UNITY_EDITOR
                        UnityEngine.Profiling.Profiler.BeginSample("Channel.IsValidEvent: " + Key + " " + checker.ToString());
                        #endif
                        _FireEvent_LastChecker = checker;
                        bool result = checker.IsValidEvent(this, _FireEvent_Evt);
                        #if UNITY_EDITOR
                        UnityEngine.Profiling.Profiler.EndSample();
                        #endif
                        return result;
                    });
                }
                return _FireEvent_Checker;
            }
        }

        private Action<IEventWatcher> _FireEvent_Watcher = null;
        private Action<IEventWatcher> FireEvent_Watcher {
            get {
                if (_FireEvent_Watcher == null) {
                    _FireEvent_Watcher = new Action<IEventWatcher>((IEventWatcher watcher) => {
                        #if UNITY_EDITOR
                        UnityEngine.Profiling.Profiler.BeginSample("Channel.OnEvent: " + Key + " " + watcher.ToString());
                        #endif
                        watcher.OnEvent(this, _FireEvent_Evt);
                        #if UNITY_EDITOR
                        UnityEngine.Profiling.Profiler.EndSample();
                        #endif
                    });
                }
                return _FireEvent_Watcher;
            }
        }

        public BlockEventChecker AddEventChecker(IBlockOwner owner, Func<Channel, Data, bool> block) {
            BlockEventChecker result = new BlockEventChecker(owner, block);
            if (AddEventChecker(result)) {
                return result;
            }
            return null;
        }

        public BlockEventWatcher AddEventWatcher(IBlockOwner owner, Action<Channel, Data> block) {
            BlockEventWatcher result = new BlockEventWatcher(owner, block);
            if (AddEventWatcher(result)) {
                return result;
            }
            return null;
        }

        protected override void AddSummaryFields(Data summary) {
            base.AddSummaryFields(summary);
            summary.I(ContextConsts.SummaryCheckerCount, EventCheckerCount)
                   .I(ContextConsts.SummaryWatcherCount, EventWatcherCount);
        }

        //SILP: DECLARE_LIST(EventChecker, listener, IEventChecker, _EventCheckers)
        private WeakList<IEventChecker> _EventCheckers = null;        //__SILP__
                                                                      //__SILP__
        public int EventCheckerCount {                                //__SILP__
            get { return WeakListHelper.Count(_EventCheckers); }      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool AddEventChecker(IEventChecker listener) {         //__SILP__
            return WeakListHelper.Add(ref _EventCheckers, listener);  //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool RemoveEventChecker(IEventChecker listener) {      //__SILP__
            return WeakListHelper.Remove(_EventCheckers, listener);   //__SILP__
        }                                                             //__SILP__

        //SILP: DECLARE_LIST(EventWatcher, listener, IEventWatcher, _EventWatchers)
        private WeakList<IEventWatcher> _EventWatchers = null;        //__SILP__
                                                                      //__SILP__
        public int EventWatcherCount {                                //__SILP__
            get { return WeakListHelper.Count(_EventWatchers); }      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool AddEventWatcher(IEventWatcher listener) {         //__SILP__
            return WeakListHelper.Add(ref _EventWatchers, listener);  //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool RemoveEventWatcher(IEventWatcher listener) {      //__SILP__
            return WeakListHelper.Remove(_EventWatchers, listener);   //__SILP__
        }                                                             //__SILP__
    }
}
