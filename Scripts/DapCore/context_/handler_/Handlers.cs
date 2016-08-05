using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public sealed class Handlers : DictAspect<IContext, Handler> {
        public Handlers(IContext owner, string key) : base(owner, key) {
        }

        public bool WaitHandler(string handlerKey, Action<Handler, bool> callback, bool waitSetup = true) {
            bool waitingForSetup = false;
            bool waitingForHandler = Owner.Utils.WaitElement(this, handlerKey, (Handler handler, bool isNew) => {
                if (handler.IsValid || !waitSetup) {
                    callback(handler, isNew);
                    return;
                }
                BlockOwner setupOwner = Owner.Utils.RetainBlockOwner();
                handler.AddSetupWatcher(new BlockSetupWatcher(setupOwner, (Handler _handler) => {
                    if (Owner.Utils.ReleaseBlockOwner(ref setupOwner)) {
                        callback(handler, isNew);
                    }
                }));
                waitingForSetup = true;
            });
            return waitingForHandler || waitingForSetup;
        }

        public Data HandleRequest(string handlerKey, Data req) {
            Handler handler = Get(handlerKey);
            if (handler != null) {
                return handler.HandleRequest(req);
            }
            return null;
        }

        public BlockRequestChecker AddRequestChecker(string handlerKey, IBlockOwner owner, Func<Handler, Data, bool> block) {
            Handler handler = Get(handlerKey);
            if (handler != null) {
                return handler.AddRequestChecker(owner, block);
            }
            return null;
        }

        public BlockRequestWatcher AddRequestWatcher(string handlerKey, IBlockOwner owner, Action<Handler, Data> block) {
            Handler handler = Get(handlerKey);
            if (handler != null) {
                return handler.AddRequestWatcher(owner, block);
            }
            return null;
        }

        public BlockResponseWatcher AddResponseWatcher(string handlerKey, IBlockOwner owner, Action<Handler, Data, Data> block) {
            Handler handler = Get(handlerKey);
            if (handler != null) {
                return handler.AddResponseWatcher(owner, block);
            }
            return null;
        }

        //SILP: ADD_REMOVE_HELPER(RequestChecker, handlerKey, handler, Handler, RequestChecker, checker, IRequestChecker)
        public bool AddRequestChecker(string handlerKey, IRequestChecker checker) {     //__SILP__
            Handler handler = Get(handlerKey);                                          //__SILP__
            if (handler != null) {                                                      //__SILP__
                return handler.AddRequestChecker(checker);                              //__SILP__
            }                                                                           //__SILP__
            return false;                                                               //__SILP__
        }                                                                               //__SILP__
                                                                                        //__SILP__
        public bool RemoveRequestChecker(string handlerKey, IRequestChecker checker) {  //__SILP__
            Handler handler = Get(handlerKey);                                          //__SILP__
            if (handler != null) {                                                      //__SILP__
                return handler.RemoveRequestChecker(checker);                           //__SILP__
            }                                                                           //__SILP__
            return false;                                                               //__SILP__
        }                                                                               //__SILP__

        //SILP: ADD_REMOVE_HELPER(RequestWatcher, handlerKey, handler, Handler, RequestWatcher, listener, IRequestWatcher)
        public bool AddRequestWatcher(string handlerKey, IRequestWatcher listener) {     //__SILP__
            Handler handler = Get(handlerKey);                                           //__SILP__
            if (handler != null) {                                                       //__SILP__
                return handler.AddRequestWatcher(listener);                              //__SILP__
            }                                                                            //__SILP__
            return false;                                                                //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        public bool RemoveRequestWatcher(string handlerKey, IRequestWatcher listener) {  //__SILP__
            Handler handler = Get(handlerKey);                                           //__SILP__
            if (handler != null) {                                                       //__SILP__
                return handler.RemoveRequestWatcher(listener);                           //__SILP__
            }                                                                            //__SILP__
            return false;                                                                //__SILP__
        }                                                                                //__SILP__

        //SILP: ADD_REMOVE_HELPER(ResponseWatcher, handlerKey, handler, Handler, ResponseWatcher, listener, IResponseWatcher)
        public bool AddResponseWatcher(string handlerKey, IResponseWatcher listener) {     //__SILP__
            Handler handler = Get(handlerKey);                                             //__SILP__
            if (handler != null) {                                                         //__SILP__
                return handler.AddResponseWatcher(listener);                               //__SILP__
            }                                                                              //__SILP__
            return false;                                                                  //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
        public bool RemoveResponseWatcher(string handlerKey, IResponseWatcher listener) {  //__SILP__
            Handler handler = Get(handlerKey);                                             //__SILP__
            if (handler != null) {                                                         //__SILP__
                return handler.RemoveResponseWatcher(listener);                            //__SILP__
            }                                                                              //__SILP__
            return false;                                                                  //__SILP__
        }                                                                                  //__SILP__
    }
}
