using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public sealed class Channel : InDictAspect<Channels>, IChannel {
        public Channel(Channels owner, string key) : base(owner, key) {
        }

        public bool FireEvent(Data evt) {
            IProfiler profiler = Log.BeginSample(Key);

            if (evt != null) evt.Seal();

            bool isValid = IsValidEvent(evt, profiler);;
            if (isValid) {
                AdvanceRevision();

                NotifyWatchers(evt, profiler);
            }

            if (LogDebug) {
                Debug("FireEvent: {0} -> {1}", evt, isValid);
            }
            if (profiler != null) profiler.EndSample();
            return isValid;
        }

        private bool IsValidEvent(Data evt, IProfiler profiler) {
            bool result = true;
            //SILP: WEAK_LIST_FOREACH_BEGIN(Channel.IsValidEvent, checker, IEventChecker, _EventCheckers)
            if (_EventCheckers != null) {                                               //__SILP__
                if (profiler != null) profiler.BeginSample("Channel.IsValidEvent");     //__SILP__
                bool needGc = false;                                                    //__SILP__
                foreach (var r in _EventCheckers.RetainLock()) {                        //__SILP__
                    IEventChecker checker = _EventCheckers.GetTarget(r);                //__SILP__
                    if (checker == null) {                                              //__SILP__
                        needGc = true;                                                  //__SILP__
                    } else {                                                            //__SILP__
                        if (profiler != null) profiler.BeginSample(checker.BlockName);  //__SILP__
                        if (!checker.IsValidEvent(this, evt)) {
                            if (LogDebug) {
                                Debug("Invalid Event: {0} => {1}", checker, evt.ToFullString());
                            }
                            result = false;
                            if (profiler != null) profiler.EndSample();
                            break;
                        }
            //SILP: WEAK_LIST_FOREACH_END(Channel.IsValidEvent, checker, IEventChecker, _EventCheckers)
                        if (profiler != null) profiler.EndSample();   //__SILP__
                    }                                                 //__SILP__
                }                                                     //__SILP__
                _EventCheckers.ReleaseLock(needGc);                   //__SILP__
                if (profiler != null) profiler.EndSample();           //__SILP__
            }                                                         //__SILP__
            return result;
        }

        private void NotifyWatchers(Data evt, IProfiler profiler) {
            //SILP: WEAK_LIST_FOREACH_BEGIN(Channel.OnEvent, watcher, IEventWatcher, _EventWatchers)
            if (_EventWatchers != null) {                                               //__SILP__
                if (profiler != null) profiler.BeginSample("Channel.OnEvent");          //__SILP__
                bool needGc = false;                                                    //__SILP__
                foreach (var r in _EventWatchers.RetainLock()) {                        //__SILP__
                    IEventWatcher watcher = _EventWatchers.GetTarget(r);                //__SILP__
                    if (watcher == null) {                                              //__SILP__
                        needGc = true;                                                  //__SILP__
                    } else {                                                            //__SILP__
                        if (profiler != null) profiler.BeginSample(watcher.BlockName);  //__SILP__
                        watcher.OnEvent(this, evt);
            //SILP: WEAK_LIST_FOREACH_END(Channel.OnEvent, watcher, IEventWatcher, _EventWatchers)
                        if (profiler != null) profiler.EndSample();   //__SILP__
                    }                                                 //__SILP__
                }                                                     //__SILP__
                _EventWatchers.ReleaseLock(needGc);                   //__SILP__
                if (profiler != null) profiler.EndSample();           //__SILP__
            }                                                         //__SILP__
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
