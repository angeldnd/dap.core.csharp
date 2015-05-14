using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface RequestChecker {
        bool IsValidRequest(string handlerPath, Data req);
    }

    public interface RequestListener {
        void OnRequest(string handlerPath, Data req);
    }

    public interface ResponseListener {
        void OnResponse(string handlerPath, Data req, Data res);
    }

    public interface RequestHandler {
        Data DoHandle(string handlerPath, Data req);
    }

    public class Handler : BaseAspect {
        private RequestHandler _Handler = null;

        public bool Setup(RequestHandler handler) {
            if (_Handler == null) {
                _Handler = handler;
                return true;
            }
            Error("Alread Setup: {0} -> {1}", _Handler, handler);
            return false;
        }

        //SILP: DECLARE_LIST(RequestChecker, checker, RequestChecker, _RequestCheckers)
        protected List<RequestChecker> _RequestCheckers = null;                          //__SILP__
                                                                                         //__SILP__
        public bool AddRequestChecker(RequestChecker checker) {                          //__SILP__
            if (_RequestCheckers == null) _RequestCheckers = new List<RequestChecker>(); //__SILP__
            if (!_RequestCheckers.Contains(checker)) {                                   //__SILP__
                _RequestCheckers.Add(checker);                                           //__SILP__
                return true;                                                             //__SILP__
            }                                                                            //__SILP__
            return false;                                                                //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        public bool RemoveRequestChecker(RequestChecker checker) {                       //__SILP__
            if (_RequestCheckers != null && _RequestCheckers.Contains(checker)) {        //__SILP__
                _RequestCheckers.Remove(checker);                                        //__SILP__
                return true;                                                             //__SILP__
            }                                                                            //__SILP__
            return false;                                                                //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        //SILP: DECLARE_LIST(RequestListener, listener, RequestListener, _RequestListeners)
        protected List<RequestListener> _RequestListeners = null;                           //__SILP__
                                                                                            //__SILP__
        public bool AddRequestListener(RequestListener listener) {                          //__SILP__
            if (_RequestListeners == null) _RequestListeners = new List<RequestListener>(); //__SILP__
            if (!_RequestListeners.Contains(listener)) {                                    //__SILP__
                _RequestListeners.Add(listener);                                            //__SILP__
                return true;                                                                //__SILP__
            }                                                                               //__SILP__
            return false;                                                                   //__SILP__
        }                                                                                   //__SILP__
                                                                                            //__SILP__
        public bool RemoveRequestListener(RequestListener listener) {                       //__SILP__
            if (_RequestListeners != null && _RequestListeners.Contains(listener)) {        //__SILP__
                _RequestListeners.Remove(listener);                                         //__SILP__
                return true;                                                                //__SILP__
            }                                                                               //__SILP__
            return false;                                                                   //__SILP__
        }                                                                                   //__SILP__
                                                                                            //__SILP__
        //SILP: DECLARE_LIST(ResponseListener, listener, ResponseListener, _ResponseListeners)
        protected List<ResponseListener> _ResponseListeners = null;                            //__SILP__
                                                                                               //__SILP__
        public bool AddResponseListener(ResponseListener listener) {                           //__SILP__
            if (_ResponseListeners == null) _ResponseListeners = new List<ResponseListener>(); //__SILP__
            if (!_ResponseListeners.Contains(listener)) {                                      //__SILP__
                _ResponseListeners.Add(listener);                                              //__SILP__
                return true;                                                                   //__SILP__
            }                                                                                  //__SILP__
            return false;                                                                      //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
        public bool RemoveResponseListener(ResponseListener listener) {                        //__SILP__
            if (_ResponseListeners != null && _ResponseListeners.Contains(listener)) {         //__SILP__
                _ResponseListeners.Remove(listener);                                           //__SILP__
                return true;                                                                   //__SILP__
            }                                                                                  //__SILP__
            return false;                                                                      //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
        public Data HandleRequest(Data req) {
            if (_Handler == null) return null;
            if (_RequestCheckers != null) {
                for (int i = 0; i < _RequestCheckers.Count; i++) {
                    if (!_RequestCheckers[i].IsValidRequest(Path, req)) {
                        return null;
                    }
                }
            }
            if (_RequestListeners != null) {
                for (int i = 0; i < _RequestListeners.Count; i++) {
                    _RequestListeners[i].OnRequest(Path, req);
                }
            }

            Data res = _Handler.DoHandle(Path, req);
            if (_ResponseListeners != null) {
                for (int i = 0; i < _ResponseListeners.Count; i++) {
                    _ResponseListeners[i].OnResponse(Path, req, res);
                }
            }
            return res;
        }
    }
}
