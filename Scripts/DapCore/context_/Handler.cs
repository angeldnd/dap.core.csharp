using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IRequestChecker {
        bool IsValidRequest(Handler handler, Data req);
    }

    public interface IRequestListener {
        void OnRequest(Handler handler, Data req);
    }

    public interface IResponseListener {
        void OnResponse(Handler handler, Data req, Data res);
    }

    public interface IRequestHandler {
        Data DoHandle(Handler handler, Data req);
    }

    public sealed class BlockRequestChecker : WeakBlock, IRequestChecker {
        public delegate bool CheckerBlock(Handler handler, Data req);

        private readonly CheckerBlock _Block;

        public BlockRequestChecker(BlockOwner owner, CheckerBlock block) : base(owner) {
            _Block = block;
        }

        public bool IsValidRequest(Handler handler, Data req) {
            return _Block(handler, req);
        }
    }

    public sealed class BlockRequestListener : WeakBlock, IRequestListener {
        public delegate void ListenerBlock(Handler handler, Data req);

        private readonly ListenerBlock _Block;

        public BlockRequestListener(BlockOwner owner, ListenerBlock block) : base(owner) {
            _Block = block;
        }

        public void OnRequest(Handler handler, Data req) {
            _Block(handler, req);
        }
    }

    public sealed class BlockResponseListener : WeakBlock, IResponseListener {
        public delegate void ListenerBlock(Handler handler, Data req, Data res);

        private readonly ListenerBlock _Block;

        public BlockResponseListener(BlockOwner owner, ListenerBlock block) : base(owner) {
            _Block = block;
        }

        public void OnResponse(Handler handler, Data req, Data res) {
            _Block(handler, req, res);
        }
    }

    /* The Handler should NOT be weak */
    public sealed class BlockRequestHandler : IRequestHandler {
        public delegate Data HandlerBlock(Handler handler, Data req);

        private readonly HandlerBlock _Block;

        public BlockRequestHandler(HandlerBlock block) {
            _Block = block;
        }

        public Data DoHandle(Handler handler, Data req) {
            return _Block(handler, req);
        }
    }

    public sealed class Handler : Aspect<Context, Handlers> {
        private IRequestHandler _Handler = null;

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

        public bool Setup(IRequestHandler handler) {
            return Setup(null, handler);
        }

        //SILP: DECLARE_SECURE_LIST(RequestChecker, checker, IRequestChecker, _RequestCheckers)
        private WeakList<IRequestChecker> _RequestCheckers = null;              //__SILP__
                                                                                //__SILP__
        public int RequestCheckerCount {                                        //__SILP__
            get { return WeakListHelper.Count(_RequestCheckers); }              //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        public bool AddRequestChecker(Pass pass, IRequestChecker checker) {     //__SILP__
            if (!CheckAdminPass(pass)) return false;                            //__SILP__
            return WeakListHelper.Add(ref _RequestCheckers, checker);           //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        public bool AddRequestChecker(IRequestChecker checker) {                //__SILP__
            return AddRequestChecker(null, checker);                            //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        public bool RemoveRequestChecker(Pass pass, IRequestChecker checker) {  //__SILP__
            if (!CheckAdminPass(pass)) return false;                            //__SILP__
            return WeakListHelper.Remove(_RequestCheckers, checker);            //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        public bool RemoveRequestChecker(IRequestChecker checker) {             //__SILP__
            return RemoveRequestChecker(null, checker);                         //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        //SILP: DECLARE_LIST(RequestListener, listener, IRequestListener, _RequestListeners)
        private WeakList<IRequestListener> _RequestListeners = null;     //__SILP__
                                                                         //__SILP__
        public int RequestListenerCount {                                //__SILP__
            get { return WeakListHelper.Count(_RequestListeners); }      //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        public bool AddRequestListener(IRequestListener listener) {      //__SILP__
            return WeakListHelper.Add(ref _RequestListeners, listener);  //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        public bool RemoveRequestListener(IRequestListener listener) {   //__SILP__
            return WeakListHelper.Remove(_RequestListeners, listener);   //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        //SILP: DECLARE_LIST(ResponseListener, listener, IResponseListener, _ResponseListeners)
        private WeakList<IResponseListener> _ResponseListeners = null;    //__SILP__
                                                                          //__SILP__
        public int ResponseListenerCount {                                //__SILP__
            get { return WeakListHelper.Count(_ResponseListeners); }      //__SILP__
        }                                                                 //__SILP__
                                                                          //__SILP__
        public bool AddResponseListener(IResponseListener listener) {     //__SILP__
            return WeakListHelper.Add(ref _ResponseListeners, listener);  //__SILP__
        }                                                                 //__SILP__
                                                                          //__SILP__
        public bool RemoveResponseListener(IResponseListener listener) {  //__SILP__
            return WeakListHelper.Remove(_ResponseListeners, listener);   //__SILP__
        }                                                                 //__SILP__
                                                                          //__SILP__
        public Data HandleRequest(Pass pass, Data req) {
            if (!CheckWritePass(pass)) return null;

            if (_Handler == null) return null;

            if (!WeakListHelper.IsValid(_RequestCheckers, (IRequestChecker checker) => {
                return checker.IsValidRequest(this, req);
            })) {
                return null;
            }

            WeakListHelper.Notify(_RequestListeners, (IRequestListener listener) => {
                listener.OnRequest(this, req);
            });

            Data res = _Handler.DoHandle(this, req);
            AdvanceRevision();

            WeakListHelper.Notify(_ResponseListeners, (IResponseListener listener) => {
                listener.OnResponse(this, req, res);
            });
            return res;
        }

        public Data HandleRequest(Data req) {
            return HandleRequest(null, req);
        }
    }
}
