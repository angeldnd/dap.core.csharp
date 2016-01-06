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

    public sealed class BlockRequestChecker : WeakBlock, RequestChecker {
        public delegate bool CheckerBlock(string handlerPath, Data req);

        private readonly CheckerBlock _Block;

        public BlockRequestChecker(BlockOwner owner, CheckerBlock block) : base(owner) {
            _Block = block;
        }

        public bool IsValidRequest(string handlerPath, Data req) {
            return _Block(handlerPath, req);
        }
    }

    public sealed class BlockRequestListener : WeakBlock, RequestListener {
        public delegate void ListenerBlock(string handlerPath, Data req);

        private readonly ListenerBlock _Block;

        public BlockRequestListener(BlockOwner owner, ListenerBlock block) : base(owner) {
            _Block = block;
        }

        public void OnRequest(string handlerPath, Data req) {
            _Block(handlerPath, req);
        }
    }

    public sealed class BlockResponseListener : WeakBlock, ResponseListener {
        public delegate void ListenerBlock(string handlerPath, Data req, Data res);

        private readonly ListenerBlock _Block;

        public BlockResponseListener(BlockOwner owner, ListenerBlock block) : base(owner) {
            _Block = block;
        }

        public void OnResponse(string handlerPath, Data req, Data res) {
            _Block(handlerPath, req, res);
        }
    }

    /* The Handler should NOT be weak */
    public sealed class BlockRequestHandler : RequestHandler {
        public delegate Data HandlerBlock(string handlerPath, Data req);

        private readonly HandlerBlock _Block;

        public BlockRequestHandler(HandlerBlock block) {
            _Block = block;
        }

        public Data DoHandle(string handlerPath, Data req) {
            return _Block(handlerPath, req);
        }
    }

    public sealed class Handler : BaseSecurableAspect {
        private RequestHandler _Handler = null;

        public bool IsEmpty {
            get { return _Handler == null; }
        }

        public bool Setup(Pass pass, RequestHandler handler) {
            if (!CheckAdminPass(pass)) return false;
            if (_Handler == null) {
                _Handler = handler;
                return true;
            }
            Error("Alread Setup: {0} -> {1}", _Handler, handler);
            return false;
        }

        public bool Setup(RequestHandler handler) {
            return Setup(null, handler);
        }

        //SILP: DECLARE_SECURE_LIST(RequestChecker, checker, RequestChecker, _RequestCheckers)
        private WeakList<RequestChecker> _RequestCheckers = null;              //__SILP__
                                                                               //__SILP__
        public int RequestCheckerCount {                                       //__SILP__
            get { return WeakListHelper.Count(_RequestCheckers); }             //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public bool AddRequestChecker(Pass pass, RequestChecker checker) {     //__SILP__
            if (!CheckAdminPass(pass)) return false;                           //__SILP__
            return WeakListHelper.Add(ref _RequestCheckers, checker);          //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public bool AddRequestChecker(RequestChecker checker) {                //__SILP__
            return AddRequestChecker(null, checker);                           //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public bool RemoveRequestChecker(Pass pass, RequestChecker checker) {  //__SILP__
            if (!CheckAdminPass(pass)) return false;                           //__SILP__
            return WeakListHelper.Remove(_RequestCheckers, checker);           //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public bool RemoveRequestChecker(RequestChecker checker) {             //__SILP__
            return RemoveRequestChecker(null, checker);                        //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        //SILP: DECLARE_LIST(RequestListener, listener, RequestListener, _RequestListeners)
        private WeakList<RequestListener> _RequestListeners = null;      //__SILP__
                                                                         //__SILP__
        public int RequestListenerCount {                                //__SILP__
            get { return WeakListHelper.Count(_RequestListeners); }      //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        public bool AddRequestListener(RequestListener listener) {       //__SILP__
            return WeakListHelper.Add(ref _RequestListeners, listener);  //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        public bool RemoveRequestListener(RequestListener listener) {    //__SILP__
            return WeakListHelper.Remove(_RequestListeners, listener);   //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        //SILP: DECLARE_LIST(ResponseListener, listener, ResponseListener, _ResponseListeners)
        private WeakList<ResponseListener> _ResponseListeners = null;     //__SILP__
                                                                          //__SILP__
        public int ResponseListenerCount {                                //__SILP__
            get { return WeakListHelper.Count(_ResponseListeners); }      //__SILP__
        }                                                                 //__SILP__
                                                                          //__SILP__
        public bool AddResponseListener(ResponseListener listener) {      //__SILP__
            return WeakListHelper.Add(ref _ResponseListeners, listener);  //__SILP__
        }                                                                 //__SILP__
                                                                          //__SILP__
        public bool RemoveResponseListener(ResponseListener listener) {   //__SILP__
            return WeakListHelper.Remove(_ResponseListeners, listener);   //__SILP__
        }                                                                 //__SILP__
                                                                          //__SILP__
        public Data HandleRequest(Pass pass, Data req) {
            if (!CheckWritePass(pass)) return null;

            if (_Handler == null) return null;

            if (!WeakListHelper.IsValid(_RequestCheckers, (RequestChecker checker) => {
                return checker.IsValidRequest(Path, req);
            })) {
                return null;
            }

            WeakListHelper.Notify(_RequestListeners, (RequestListener listener) => {
                listener.OnRequest(Path, req);
            });

            Data res = _Handler.DoHandle(Path, req);
            AdvanceRevision();

            WeakListHelper.Notify(_ResponseListeners, (ResponseListener listener) => {
                listener.OnResponse(Path, req, res);
            });
            return res;
        }

        public Data HandleRequest(Data req) {
            return HandleRequest(null, req);
        }
    }
}
