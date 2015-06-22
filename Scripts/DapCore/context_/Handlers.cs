using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public class Handlers : EntityAspect {
        public Handler GetHandler(string handlerPath) {
            return Get<Handler>(handlerPath);
        }

        public Handler AddHandler(string handlerPath) {
            return Add<Handler>(handlerPath);
        }

        //SILP: ADD_REMOVE_HELPER(RequestChecker, handlerPath, handler, Handler, RequestChecker, checker, RequestChecker)
        public bool AddRequestChecker(string handlerPath, RequestChecker checker) {    //__SILP__
            Handler handler = Get<Handler>(handlerPath);                               //__SILP__
            if (handler != null) {                                                     //__SILP__
                return handler.AddRequestChecker(checker);                             //__SILP__
            } else {                                                                   //__SILP__
                Error("Handler Not Found: {0}", handlerPath);                          //__SILP__
            }                                                                          //__SILP__
            return false;                                                              //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        public bool RemoveRequestChecker(string handlerPath, RequestChecker checker) { //__SILP__
            Handler handler = Get<Handler>(handlerPath);                               //__SILP__
            if (handler != null) {                                                     //__SILP__
                return handler.RemoveRequestChecker(checker);                          //__SILP__
            } else {                                                                   //__SILP__
                Error("Handler Not Found: {0}", handlerPath);                          //__SILP__
            }                                                                          //__SILP__
            return false;                                                              //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        //SILP: ADD_REMOVE_HELPER(RequestListener, handlerPath, handler, Handler, RequestListener, listener, RequestListener)
        public bool AddRequestListener(string handlerPath, RequestListener listener) {    //__SILP__
            Handler handler = Get<Handler>(handlerPath);                                  //__SILP__
            if (handler != null) {                                                        //__SILP__
                return handler.AddRequestListener(listener);                              //__SILP__
            } else {                                                                      //__SILP__
                Error("Handler Not Found: {0}", handlerPath);                             //__SILP__
            }                                                                             //__SILP__
            return false;                                                                 //__SILP__
        }                                                                                 //__SILP__
                                                                                          //__SILP__
        public bool RemoveRequestListener(string handlerPath, RequestListener listener) { //__SILP__
            Handler handler = Get<Handler>(handlerPath);                                  //__SILP__
            if (handler != null) {                                                        //__SILP__
                return handler.RemoveRequestListener(listener);                           //__SILP__
            } else {                                                                      //__SILP__
                Error("Handler Not Found: {0}", handlerPath);                             //__SILP__
            }                                                                             //__SILP__
            return false;                                                                 //__SILP__
        }                                                                                 //__SILP__
                                                                                          //__SILP__
        //SILP: ADD_REMOVE_HELPER(ResponseListener, handlerPath, handler, Handler, ResponseListener, listener, ResponseListener)
        public bool AddResponseListener(string handlerPath, ResponseListener listener) {    //__SILP__
            Handler handler = Get<Handler>(handlerPath);                                    //__SILP__
            if (handler != null) {                                                          //__SILP__
                return handler.AddResponseListener(listener);                               //__SILP__
            } else {                                                                        //__SILP__
                Error("Handler Not Found: {0}", handlerPath);                               //__SILP__
            }                                                                               //__SILP__
            return false;                                                                   //__SILP__
        }                                                                                   //__SILP__
                                                                                            //__SILP__
        public bool RemoveResponseListener(string handlerPath, ResponseListener listener) { //__SILP__
            Handler handler = Get<Handler>(handlerPath);                                    //__SILP__
            if (handler != null) {                                                          //__SILP__
                return handler.RemoveResponseListener(listener);                            //__SILP__
            } else {                                                                        //__SILP__
                Error("Handler Not Found: {0}", handlerPath);                               //__SILP__
            }                                                                               //__SILP__
            return false;                                                                   //__SILP__
        }                                                                                   //__SILP__
                                                                                            //__SILP__

        public Data HandleRequest(string handlerPath, Data req) {
            Handler handler = Get<Handler>(handlerPath);
            if (handler != null) {
                return handler.HandleRequest(req);
            }
            return null;
        }
    }
}
