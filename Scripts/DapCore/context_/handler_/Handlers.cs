using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public sealed class Handlers : DictAspect<IContext, Handler> {
        public Handlers(IContext owner, string key) : base(owner, key) {
        }

        public bool WaitHandler(string handlerKey, Action<Handler, bool> callback, bool waitSetup = true) {
            return Owner.Utils.WaitSetupAspect(this, handlerKey, callback, waitSetup);
        }

        public bool WaitAndWatchRequest(string handlerKey, IRequestWatcher watcher, bool waitSetup = true) {
            return Owner.Utils.WaitSetupAspect(this, handlerKey, (Handler handler, bool isNew) => {
                handler.AddRequestWatcher(watcher);
            }, waitSetup);
        }

        public bool WaitAndWatchRequest(string handlerKey, IBlockOwner owner, Action<Handler, Data> block) {
            return WaitAndWatchRequest(handlerKey, new BlockRequestWatcher(owner, block));
        }

        public bool WaitAndWatchResponse(string handlerKey, IResponseWatcher watcher, bool waitSetup = true) {
            return Owner.Utils.WaitSetupAspect(this, handlerKey, (Handler handler, bool isNew) => {
                handler.AddResponseWatcher(watcher);
            }, waitSetup);
        }

        public bool WaitAndWatchResponse(string handlerKey, IBlockOwner owner, Action<Handler, Data, Data> block) {
            return WaitAndWatchResponse(handlerKey, new BlockResponseWatcher(owner, block));
        }

        public bool WaitAndWatchAsyncResponse(string handlerKey, IAsyncResponseWatcher watcher, bool waitSetup = true) {
            return Owner.Utils.WaitSetupAspect(this, handlerKey, (Handler handler, bool isNew) => {
                handler.AddAsyncResponseWatcher(watcher);
            }, waitSetup);
        }

        public bool WaitAndWatchAsyncResponse(string handlerKey, IBlockOwner owner, Action<Handler, Data> block) {
            return WaitAndWatchAsyncResponse(handlerKey, new BlockAsyncResponseWatcher(owner, block));
        }

        public Data HandleRequest(string handlerKey, Data req) {
            Handler handler = Get(handlerKey);
            if (handler != null) {
                return handler.HandleRequest(req);
            }
            return null;
        }

        public bool OnAsyncResponse(string handlerKey, Data res) {
            Handler handler = Get(handlerKey);
            if (handler != null) {
                return handler.OnAsyncResponse(res);
            }
            return false;
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

        public BlockAsyncResponseWatcher AddAsyncResponseWatcher(string handlerKey, IBlockOwner owner, Action<Handler, Data> block) {
            Handler handler = Get(handlerKey);
            if (handler != null) {
                return handler.AddAsyncResponseWatcher(owner, block);
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

        //SILP: ADD_REMOVE_HELPER(AsyncResponseWatcher, handlerKey, handler, Handler, AsyncResponseWatcher, listener, IAsyncResponseWatcher)
        public bool AddAsyncResponseWatcher(string handlerKey, IAsyncResponseWatcher listener) {     //__SILP__
            Handler handler = Get(handlerKey);                                                       //__SILP__
            if (handler != null) {                                                                   //__SILP__
                return handler.AddAsyncResponseWatcher(listener);                                    //__SILP__
            }                                                                                        //__SILP__
            return false;                                                                            //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        public bool RemoveAsyncResponseWatcher(string handlerKey, IAsyncResponseWatcher listener) {  //__SILP__
            Handler handler = Get(handlerKey);                                                       //__SILP__
            if (handler != null) {                                                                   //__SILP__
                return handler.RemoveAsyncResponseWatcher(listener);                                 //__SILP__
            }                                                                                        //__SILP__
            return false;                                                                            //__SILP__
        }                                                                                            //__SILP__
    }
}
