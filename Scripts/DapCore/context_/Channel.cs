using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface EventChecker {
        bool IsValidEvent(string channelPath, Data evt);
    }

    public interface EventListener {
        void OnEvent(string channelPath, Data evt);
    }

    public sealed class BlockEventChecker : WeakBlock, EventChecker {
        public delegate bool CheckerBlock(string channelPath, Data evt);

        private readonly CheckerBlock _Block;

        public BlockEventChecker(BlockOwner owner, CheckerBlock block) : base(owner) {
            _Block = block;
        }

        public bool IsValidEvent(string channelPath, Data evt) {
            return _Block(channelPath, evt);
        }
    }

    public sealed class BlockEventListener : WeakBlock, EventListener {
        public delegate void ListenerBlock(string channelPath, Data evt);

        private readonly ListenerBlock _Block;

        public BlockEventListener(BlockOwner owner, ListenerBlock block) : base(owner) {
            _Block = block;
        }

        public void OnEvent(string channelPath, Data evt) {
            _Block(channelPath, evt);
        }
    }

    public sealed class Channel : BaseSecurableAspect {
        //SILP: DECLARE_SECURE_LIST(EventChecker, listener, EventChecker, _EventCheckers)
        private WeakList<EventChecker> _EventCheckers = null;               //__SILP__
                                                                            //__SILP__
        public int EventCheckerCount {                                      //__SILP__
            get { return WeakListHelper.Count(_EventCheckers); }            //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        public bool AddEventChecker(Pass pass, EventChecker listener) {     //__SILP__
            if (!CheckAdminPass(pass)) return false;                        //__SILP__
            return WeakListHelper.Add(ref _EventCheckers, listener);        //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        public bool AddEventChecker(EventChecker listener) {                //__SILP__
            return AddEventChecker(null, listener);                         //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        public bool RemoveEventChecker(Pass pass, EventChecker listener) {  //__SILP__
            if (!CheckAdminPass(pass)) return false;                        //__SILP__
            return WeakListHelper.Remove(_EventCheckers, listener);         //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        public bool RemoveEventChecker(EventChecker listener) {             //__SILP__
            return RemoveEventChecker(null, listener);                      //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        //SILP: DECLARE_LIST(EventListener, listener, EventListener, _EventListeners)
        private WeakList<EventListener> _EventListeners = null;        //__SILP__
                                                                       //__SILP__
        public int EventListenerCount {                                //__SILP__
            get { return WeakListHelper.Count(_EventListeners); }      //__SILP__
        }                                                              //__SILP__
                                                                       //__SILP__
        public bool AddEventListener(EventListener listener) {         //__SILP__
            return WeakListHelper.Add(ref _EventListeners, listener);  //__SILP__
        }                                                              //__SILP__
                                                                       //__SILP__
        public bool RemoveEventListener(EventListener listener) {      //__SILP__
            return WeakListHelper.Remove(_EventListeners, listener);   //__SILP__
        }                                                              //__SILP__
                                                                       //__SILP__
        public bool FireEvent(Pass pass, Data evt) {
            if (!CheckWritePass(pass)) return false;

            if (!WeakListHelper.IsValid(_EventCheckers, (EventChecker checker) => {
                return checker.IsValidEvent(Path, evt);
            })) {
                return false;
            }
            AdvanceRevision();

            WeakListHelper.Notify(_EventListeners, (EventListener listener) => {
                listener.OnEvent(Path, evt);
            });
            return true;
        }

        public bool FireEvent(Data evt) {
            return FireEvent(null, evt);
        }

        public bool FireEvent() {
            return FireEvent(null, null);
        }
    }
}
