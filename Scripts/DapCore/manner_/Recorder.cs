using System;
using System.Collections.Generic;

using angeldnd.dap;

namespace angeldnd.dap {
    public static class RecorderExtension {
        public static T AddRecorder<T>(this Manners manners)
                                                    where T : class, IRecorder {
            return manners.Add<T>(RecordableConsts.MannerRecorder);
        }

        public static T GetRecorder<T>(this Manners manners)
                                                    where T : class, IRecorder {
            return manners.Get<T>(RecordableConsts.MannerRecorder);
        }

        public static T GetOrAddRecorder<T>(this Manners manners)
                                                    where T : class, IRecorder {
            return manners.GetOrAdd<T>(RecordableConsts.MannerRecorder);
        }
    }

    public interface IRecorderDelegate {
        bool ShouldRecord(Recordable src, IProperty prop);
        bool ShouldRecord(Recordable src, Channel channel);
        bool ShouldRecord(Recordable src, Handler handler);
        bool ShouldRecord(Recordable src, Bus bus, string msg);
    }

    public interface IRecorder : IRecorderDelegate, IManner, ISealable {
        void OnJoin(Recordable src);

        void OnChanged(Recordable src, IVar v);
        void OnEvent(Recordable src, Channel channel, Data evt);
        void OnResponse(Recordable src, Handler handler, Data req, Data res);
        void OnBusMsg(Recordable src, Bus bus, string msg);

        void OnPropertyAdded(Recordable src, IProperty property);
        void OnPropertyRemoved(Recordable src, IProperty property);
        void OnChannelAdded(Recordable src, Channel channel);
        void OnChannelRemoved(Recordable src, Channel channel);
        void OnHandlerAdded(Recordable src, Handler handler);
        void OnHandlerRemoved(Recordable src, Handler handler);
    }

    public abstract class Recorder<T> : Manner, IRecorder
                                            where T : class, IRecorderDelegate {
        private T _Delegate = null;
        public T Delegate {
            get { return _Delegate; }
        }

        private bool _Sealed = false;
        public bool Sealed {
            get { return _Sealed; }
        }

        public void Seal() {
            _Sealed = true;
            OnSeal();
        }

        public Recorder(Manners owner, string key) : base(owner, key) {
            _Delegate = Context.As<T>();
            if (_Delegate == null) {
                Error("Invalid Recorder: Delegate Not Found");
            }
        }

        public bool ShouldRecord(Recordable src, IProperty prop) {
            if (_Delegate == null) return false;

            return _Delegate.ShouldRecord(src, prop);
        }

        public bool ShouldRecord(Recordable src, Channel channel) {
            if (_Delegate == null) return false;

            return _Delegate.ShouldRecord(src, channel);
        }

        public bool ShouldRecord(Recordable src, Handler handler) {
            if (_Delegate == null) return false;

            return _Delegate.ShouldRecord(src, handler);
        }

        public bool ShouldRecord(Recordable src, Bus bus, string msg) {
            if (_Delegate == null) return false;

            return _Delegate.ShouldRecord(src, bus, msg);
        }

        public abstract void OnJoin(Recordable src);

        public abstract void OnChanged(Recordable src, IVar v);
        public abstract void OnEvent(Recordable src, Channel channel, Data evt);
        public abstract void OnResponse(Recordable src, Handler handler, Data req, Data res);
        public abstract void OnBusMsg(Recordable src, Bus bus, string msg);

        public abstract void OnPropertyAdded(Recordable src, IProperty property);
        public abstract void OnPropertyRemoved(Recordable src, IProperty property);
        public abstract void OnChannelAdded(Recordable src, Channel channel);
        public abstract void OnChannelRemoved(Recordable src, Channel channel);
        public abstract void OnHandlerAdded(Recordable src, Handler handler);
        public abstract void OnHandlerRemoved(Recordable src, Handler handler);

        protected virtual void OnSeal() {}
    }
}
