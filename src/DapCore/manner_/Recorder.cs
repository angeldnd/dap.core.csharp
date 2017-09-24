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
        bool ShouldRecord(IRecordable src, IProperty prop);
        bool ShouldRecord(IRecordable src, Channel channel);
        bool ShouldRecord(IRecordable src, Handler handler);
        bool ShouldRecord(IRecordable src, Bus bus, string msg);
    }

    public interface IRecorder : IRecorderDelegate, IManner, ISealable {
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

        public bool ShouldRecord(IRecordable src, IProperty prop) {
            if (_Delegate == null) return false;

            return _Delegate.ShouldRecord(src, prop);
        }

        public bool ShouldRecord(IRecordable src, Channel channel) {
            if (_Delegate == null) return false;

            return _Delegate.ShouldRecord(src, channel);
        }

        public bool ShouldRecord(IRecordable src, Handler handler) {
            if (_Delegate == null) return false;

            return _Delegate.ShouldRecord(src, handler);
        }

        public bool ShouldRecord(IRecordable src, Bus bus, string msg) {
            if (_Delegate == null) return false;

            return _Delegate.ShouldRecord(src, bus, msg);
        }

        protected virtual void OnSeal() {}
    }

    public abstract class Recorder : Recorder<IRecorderDelegate> {
        public Recorder(Manners owner, string key) : base(owner, key) {
        }

        public abstract void OnJoin(IRecordable src);

        public abstract void OnChanged(IRecordable src, IVar v);
        public abstract void OnEvent(IRecordable src, Channel channel, Data evt);
        public abstract void OnResponse(IRecordable src, Handler handler, Data req, Data res);
        public abstract void OnBusMsg(IRecordable src, Bus bus, string msg);

        public abstract void OnPropertyAdded(IRecordable src, IProperty property);
        public abstract void OnPropertyRemoved(IRecordable src, IProperty property);
        public abstract void OnChannelAdded(IRecordable src, Channel channel);
        public abstract void OnChannelRemoved(IRecordable src, Channel channel);
        public abstract void OnHandlerAdded(IRecordable src, Handler handler);
        public abstract void OnHandlerRemoved(IRecordable src, Handler handler);
    }
}
