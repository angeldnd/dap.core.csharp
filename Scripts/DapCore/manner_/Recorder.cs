using System;
using System.Collections.Generic;

using angeldnd.dap;

namespace angeldnd.dap {
    public static class RecorderExtension {
        public static T AddRecorder<T>(this Manners manners)
                                                    where T : Recorder {
            return manners.Add<T>(RecordableConsts.MannerRecorder);
        }

        public static T GetRecorder<T>(this Manners manners)
                                                    where T : Recorder {
            return manners.Get<T>(RecordableConsts.MannerRecorder);
        }

        public static T GetOrAddRecorder<T>(this Manners manners)
                                                    where T : Recorder {
            return manners.GetOrAdd<T>(RecordableConsts.MannerRecorder);
        }
    }

    public interface IRecorderContext : IContext {
        bool ShouldRecord(Recordable src, IProperty prop);
        bool ShouldRecord(Recordable src, Channel channel);
        bool ShouldRecord(Recordable src, Handler handler);
        bool ShouldRecord(Recordable src, Bus bus, string msg);
        Data AppendSyncData(Data data);
    }

    public abstract class Recorder : Manner, ISealable {
        private IRecorderContext _RecorderContext = null;
        public IRecorderContext RecorderContext {
            get { return _RecorderContext; }
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
            _RecorderContext = Context.As<IRecorderContext>();
            if (_RecorderContext == null) {
                Error("Invalid Recorder: RecorderContext Not Found");
            }
        }

        public bool ShouldRecord(Recordable src, IProperty prop) {
            if (_RecorderContext == null) return false;

            return _RecorderContext.ShouldRecord(src, prop);
        }

        public bool ShouldRecord(Recordable src, Channel channel) {
            if (_RecorderContext == null) return false;

            return _RecorderContext.ShouldRecord(src, channel);
        }

        public bool ShouldRecord(Recordable src, Handler handler) {
            if (_RecorderContext == null) return false;

            return _RecorderContext.ShouldRecord(src, handler);
        }

        public bool ShouldRecord(Recordable src, Bus bus, string msg) {
            if (_RecorderContext == null) return false;

            return _RecorderContext.ShouldRecord(src, bus, msg);
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
