using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface EventChecker {
        bool IsValidEvent(string channelPath, Data evt);
    }

    public interface EventListener {
        void OnEvent(string channelPath, Data evt);
    }

    public class Channel : BaseAspect {
        //SILP: DECLARE_LIST(EventChecker, listener, EventChecker, _EventCheckers)
        protected List<EventChecker> _EventCheckers = null;                        //__SILP__
                                                                                   //__SILP__
        public bool AddEventChecker(EventChecker listener) {                       //__SILP__
            if (_EventCheckers == null) _EventCheckers = new List<EventChecker>(); //__SILP__
            if (!_EventCheckers.Contains(listener)) {                              //__SILP__
                _EventCheckers.Add(listener);                                      //__SILP__
                return true;                                                       //__SILP__
            }                                                                      //__SILP__
            return false;                                                          //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public bool RemoveEventChecker(EventChecker listener) {                    //__SILP__
            if (_EventCheckers != null && _EventCheckers.Contains(listener)) {     //__SILP__
                _EventCheckers.Remove(listener);                                   //__SILP__
                return true;                                                       //__SILP__
            }                                                                      //__SILP__
            return false;                                                          //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        //SILP: DECLARE_LIST(EventListener, listener, EventListener, _EventListeners)
        protected List<EventListener> _EventListeners = null;                         //__SILP__
                                                                                      //__SILP__
        public bool AddEventListener(EventListener listener) {                        //__SILP__
            if (_EventListeners == null) _EventListeners = new List<EventListener>(); //__SILP__
            if (!_EventListeners.Contains(listener)) {                                //__SILP__
                _EventListeners.Add(listener);                                        //__SILP__
                return true;                                                          //__SILP__
            }                                                                         //__SILP__
            return false;                                                             //__SILP__
        }                                                                             //__SILP__
                                                                                      //__SILP__
        public bool RemoveEventListener(EventListener listener) {                     //__SILP__
            if (_EventListeners != null && _EventListeners.Contains(listener)) {      //__SILP__
                _EventListeners.Remove(listener);                                     //__SILP__
                return true;                                                          //__SILP__
            }                                                                         //__SILP__
            return false;                                                             //__SILP__
        }                                                                             //__SILP__
                                                                                      //__SILP__
        public bool FireEvent(Data evt) {
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
            return true;
        }

    }
}
