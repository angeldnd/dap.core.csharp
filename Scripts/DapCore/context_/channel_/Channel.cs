using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public sealed class Channel : InDictAspect<Channels>, IChannel {
        public Channel(Channels owner, string key) : base(owner, key) {
        }

        public bool FireEvent(Data evt) {
            if (!WeakListHelper.IsValid(_EventCheckers, (IEventChecker checker) => {
                return checker.IsValidEvent(this, evt);
            })) {
                return false;
            }
            AdvanceRevision();

            WeakListHelper.Notify(_EventWatchers, (IEventWatcher listener) => {
                listener.OnEvent(this, evt);
            });
            return true;
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
