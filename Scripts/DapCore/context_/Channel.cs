using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface EventChecker {
        bool IsValidEvent(string channelPath, Data evt);
    }

    public interface EventListener {
        void OnEvent(string channelPath, Data evt);
    }

    public sealed class BlockEventChecker : EventChecker {
        public delegate bool CheckerBlock(string channelPath, Data evt);

        private readonly CheckerBlock _Block;

        public BlockEventChecker(CheckerBlock block) {
            _Block = block;
        }

        public bool IsValidEvent(string channelPath, Data evt) {
            return _Block(channelPath, evt);
        }
    }

    public sealed class BlockEventListener : EventListener {
        public delegate void ListenerBlock(string channelPath, Data evt);

        private readonly ListenerBlock _Block;

        public BlockEventListener(ListenerBlock block) {
            _Block = block;
        }

        public void OnEvent(string channelPath, Data evt) {
            _Block(channelPath, evt);
        }
    }

    public class Channel : BaseSecurableAspect {
        //SILP: DECLARE_SECURE_LIST(EventChecker, listener, EventChecker, _EventCheckers)
        protected List<EventChecker> _EventCheckers = null;                         //__SILP__
                                                                                    //__SILP__
        public int EventCheckerCount {                                              //__SILP__
            get {                                                                   //__SILP__
                if (_EventCheckers == null) {                                       //__SILP__
                    return 0;                                                       //__SILP__
                }                                                                   //__SILP__
                return _EventCheckers.Count;                                        //__SILP__
            }                                                                       //__SILP__
        }                                                                           //__SILP__
                                                                                    //__SILP__
        public virtual bool AddEventChecker(Pass pass, EventChecker listener) {     //__SILP__
            if (!CheckPass(pass)) return false;                                     //__SILP__
            if (_EventCheckers == null) _EventCheckers = new List<EventChecker>();  //__SILP__
            if (!_EventCheckers.Contains(listener)) {                               //__SILP__
                _EventCheckers.Add(listener);                                       //__SILP__
                return true;                                                        //__SILP__
            }                                                                       //__SILP__
            return false;                                                           //__SILP__
        }                                                                           //__SILP__
                                                                                    //__SILP__
        public bool AddEventChecker(EventChecker listener) {                        //__SILP__
            return AddEventChecker(null, listener);                                 //__SILP__
        }                                                                           //__SILP__
                                                                                    //__SILP__
        public virtual bool RemoveEventChecker(Pass pass, EventChecker listener) {  //__SILP__
            if (!CheckPass(pass)) return false;                                     //__SILP__
            if (_EventCheckers != null && _EventCheckers.Contains(listener)) {      //__SILP__
                _EventCheckers.Remove(listener);                                    //__SILP__
                return true;                                                        //__SILP__
            }                                                                       //__SILP__
            return false;                                                           //__SILP__
        }                                                                           //__SILP__
                                                                                    //__SILP__
        public bool RemoveEventChecker(EventChecker listener) {                     //__SILP__
            return RemoveEventChecker(null, listener);                              //__SILP__
        }                                                                           //__SILP__
                                                                                    //__SILP__
        //SILP: DECLARE_LIST(EventListener, listener, EventListener, _EventListeners)
        protected List<EventListener> _EventListeners = null;                          //__SILP__
                                                                                       //__SILP__
        public int EventListenerCount {                                                //__SILP__
            get {                                                                      //__SILP__
                if (_EventListeners == null) {                                         //__SILP__
                    return 0;                                                          //__SILP__
                }                                                                      //__SILP__
                return _EventListeners.Count;                                          //__SILP__
            }                                                                          //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        public virtual bool AddEventListener(EventListener listener) {                 //__SILP__
            if (_EventListeners == null) _EventListeners = new List<EventListener>();  //__SILP__
            if (!_EventListeners.Contains(listener)) {                                 //__SILP__
                _EventListeners.Add(listener);                                         //__SILP__
                return true;                                                           //__SILP__
            }                                                                          //__SILP__
            return false;                                                              //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        public virtual bool RemoveEventListener(EventListener listener) {              //__SILP__
            if (_EventListeners != null && _EventListeners.Contains(listener)) {       //__SILP__
                _EventListeners.Remove(listener);                                      //__SILP__
                return true;                                                           //__SILP__
            }                                                                          //__SILP__
            return false;                                                              //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        public bool FireEvent(Pass pass, Data evt) {
            if (!CheckPass(pass)) return false;

            if (_EventCheckers != null) {
                for (int i = 0; i < _EventCheckers.Count; i++) {
                    if (!_EventCheckers[i].IsValidEvent(Path, evt)) {
                        return false;
                    }
                }
            }
            if (_EventListeners != null) {
                for (int i = 0; i < _EventListeners.Count; i++) {
                    _EventListeners[i].OnEvent(Path, evt);
                }
            }
            AdvanceRevision();
            return true;
        }

        public bool FireEvent(Data evt) {
            return FireEvent(null, evt);
        }
    }
}
