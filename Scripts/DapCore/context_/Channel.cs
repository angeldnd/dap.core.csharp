using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IEventChecker {
        bool IsValidEvent(Channel channel, Data evt);
    }

    public interface IEventListener {
        void OnEvent(Channel channel, Data evt);
    }

    public sealed class BlockEventChecker : WeakBlock, IEventChecker {
        public delegate bool CheckerBlock(Channel channel, Data evt);

        private readonly CheckerBlock _Block;

        public BlockEventChecker(IBlockOwner owner, CheckerBlock block) : base(owner) {
            _Block = block;
        }

        public bool IsValidEvent(Channel channel, Data evt) {
            return _Block(channel, evt);
        }
    }

    public sealed class BlockEventListener : WeakBlock, IEventListener {
        public delegate void ListenerBlock(Channel channel, Data evt);

        private readonly ListenerBlock _Block;

        public BlockEventListener(IBlockOwner owner, ListenerBlock block) : base(owner) {
            _Block = block;
        }

        public void OnEvent(Channel channel, Data evt) {
            _Block(channel, evt);
        }
    }

    public sealed class Channel : InTreeAspect<Channels> {
        public Channel(Channels owner, string path, Pass pass) : base(owner, path, pass) {
        }

        //SILP: DECLARE_SECURE_LIST(EventChecker, listener, IEventChecker, _EventCheckers)
        private WeakList<IEventChecker> _EventCheckers = null;               //__SILP__
                                                                             //__SILP__
        public int EventCheckerCount {                                       //__SILP__
            get { return WeakListHelper.Count(_EventCheckers); }             //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        public bool AddEventChecker(Pass pass, IEventChecker listener) {     //__SILP__
            if (!CheckAdminPass(pass)) return false;                         //__SILP__
            return WeakListHelper.Add(ref _EventCheckers, listener);         //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        public bool AddEventChecker(IEventChecker listener) {                //__SILP__
            return AddEventChecker(null, listener);                          //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        public bool RemoveEventChecker(Pass pass, IEventChecker listener) {  //__SILP__
            if (!CheckAdminPass(pass)) return false;                         //__SILP__
            return WeakListHelper.Remove(_EventCheckers, listener);          //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        public bool RemoveEventChecker(IEventChecker listener) {             //__SILP__
            return RemoveEventChecker(null, listener);                       //__SILP__
        }                                                                    //__SILP__
        //SILP: DECLARE_LIST(EventListener, listener, IEventListener, _EventListeners)
        private WeakList<IEventListener> _EventListeners = null;       //__SILP__
                                                                       //__SILP__
        public int EventListenerCount {                                //__SILP__
            get { return WeakListHelper.Count(_EventListeners); }      //__SILP__
        }                                                              //__SILP__
                                                                       //__SILP__
        public bool AddEventListener(IEventListener listener) {        //__SILP__
            return WeakListHelper.Add(ref _EventListeners, listener);  //__SILP__
        }                                                              //__SILP__
                                                                       //__SILP__
        public bool RemoveEventListener(IEventListener listener) {     //__SILP__
            return WeakListHelper.Remove(_EventListeners, listener);   //__SILP__
        }                                                              //__SILP__
        public bool FireEvent(Pass pass, Data evt) {
            if (!CheckWritePass(pass)) return false;

            if (!WeakListHelper.IsValid(_EventCheckers, (IEventChecker checker) => {
                return checker.IsValidEvent(this, evt);
            })) {
                return false;
            }
            AdvanceRevision();

            WeakListHelper.Notify(_EventListeners, (IEventListener listener) => {
                listener.OnEvent(this, evt);
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
