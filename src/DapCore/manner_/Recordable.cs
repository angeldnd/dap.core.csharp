using System;
using System.Collections.Generic;

using angeldnd.dap;

namespace angeldnd.dap {
    public static class RecordableConsts {
        public const string TypeRecordable = "Recordable";

        public const string MannerRecordable = "recordable";
        public const string MannerRecorder = "recorder";
    }

    public static class RecordableExtension {
        public static Recordable AddRecordable(this Manners manners) {
            return manners.Add<Recordable>(RecordableConsts.MannerRecordable);
        }

        public static Recordable GetRecordable(this Manners manners) {
            return manners.Get<Recordable>(RecordableConsts.MannerRecordable);
        }

        public static Recordable GetOrAddRecordable(this Manners manners) {
            return manners.GetOrAdd<Recordable>(RecordableConsts.MannerRecordable);
        }
    }

    public interface IRecordable : IManner {
        IRecorder GetRecorder();
        string RelPath { get; }
    }

    [DapType(RecordableConsts.TypeRecordable)]
    [DapOrder(DapOrders.Manner)]
    public abstract class Recordable<T> : Decorator, IRecordable
                                            where T : class, IRecorder {
        private bool _RecorderInited = false;

        private T _Recorder;
        public T Recorder {
            get {
                //Use this way since will be used in Decorator's constructor
                if (_Recorder == null && !_RecorderInited) {
                    _RecorderInited = true;
                    _Recorder = Owner.Get<T>(RecordableConsts.MannerRecorder, true);
                    if (_Recorder == null) {
                        _Recorder = Context.GetOwnOrAncestorManner<T>(RecordableConsts.MannerRecorder);
                    }
                    if (_Recorder != null) {
                        OnRecorderInit();
                    }
                }
                return _Recorder;
            }
        }

        public IRecorder GetRecorder() {
            return Recorder;
        }

        private string _RelPath = null;
        public string RelPath {
            get {
                //Use this way since will be used in Decorator's constructor
                if (_RelPath == null && Recorder != null) {
                    if (Context == Recorder.Context) {
                        _RelPath = "";
                    } else {
                        _RelPath = Recorder.Context.GetRelativePath(Context);
                    }
                }
                return _RelPath;
            }
        }

        public virtual bool TryResetRecorder() {
            if (_Recorder != null) {
                Error("TryResetRecorder Failed: _Recorder = {0}", _Recorder);
                return false;
            }
            _RecorderInited = false;
            if (Recorder == null) {
                Error("Invalid Recordable: Recorder Not Found");
                return false;
            }
            if (RelPath == null) {
                Error("Invalid Recordable: RelPath == null");
                return false;
            }
            return true;
        }

        protected abstract void OnRecorderInit();

        protected override bool ShouldWatchProperties() {
            return true;
        }

        protected override bool ShouldWatchChannels() {
            return true;
        }

        protected override bool ShouldWatchHandlers() {
            return true;
        }

        protected override bool ShouldWatchBus() {
            return true;
        }

        public Recordable(Manners owner, string key) : base(owner, key) {
        }

        protected bool ShouldRecord(IProperty prop) {
            if (Recorder == null) return false;

            return Recorder.ShouldRecord(this, prop);
        }

        protected bool ShouldRecord(Channel channel) {
            if (Recorder == null) return false;

            return Recorder.ShouldRecord(this, channel);
        }

        protected bool ShouldRecord(Handler handler) {
            if (Recorder == null) return false;

            return Recorder.ShouldRecord(this, handler);
        }

        protected bool ShouldRecord(Bus bus, string msg) {
            if (Recorder == null) return false;

            return Recorder.ShouldRecord(this, bus, msg);
        }
    }

    public class Recordable : Recordable<Recorder>, IVarWatcher, IEventWatcher, IResponseWatcher {
        public Recordable(Manners owner, string key) : base(owner, key) {
            if (Recorder == null) {
                Error("Invalid Recordable: Recorder Not Found");
            }
            if (RelPath == null) {
                Error("Invalid Recordable: RelPath == null");
            }
        }

        protected override void OnRecorderInit() {
            Recorder.OnJoin(this);
        }

        protected override void OnPropertyAdded(IProperty property, bool isNew) {
            if (ShouldRecord(property)) {
                property.AddVarWatcher(this);
                Recorder.OnPropertyAdded(this, property);
            }
        }

        protected override void OnPropertyRemoved(IProperty property) {
            if (ShouldRecord(property)) {
                property.RemoveVarWatcher(this);
                Recorder.OnPropertyRemoved(this, property);
            }
        }

        protected override void OnChannelAdded(Channel channel, bool isNew) {
            if (ShouldRecord(channel)) {
                channel.AddEventWatcher(this);
                Recorder.OnChannelAdded(this, channel);
            }
        }

        protected override void OnChannelRemoved(Channel channel) {
            if (ShouldRecord(channel)) {
                channel.RemoveEventWatcher(this);
                Recorder.OnChannelRemoved(this, channel);
            }
        }

        protected override void OnHandlerAdded(Handler handler, bool isNew) {
            if (ShouldRecord(handler)) {
                handler.AddResponseWatcher(this);
                Recorder.OnHandlerAdded(this, handler);
            }
        }

        protected override void OnHandlerRemoved(Handler handler) {
            if (ShouldRecord(handler)) {
                handler.RemoveResponseWatcher(this);
                Recorder.OnHandlerRemoved(this, handler);
            }
        }

        protected override void OnBusMsg(Bus bus, string msg) {
            if (ShouldRecord(bus, msg)) {
                Recorder.OnBusMsg(this, bus, msg);
            }
        }

        public void OnChanged(IVar v) {
            Recorder.OnChanged(this, v);
        }

        public void OnEvent(Channel channel, Data evt) {
            Recorder.OnEvent(this, channel, evt);
        }

        public void OnResponse(Handler handler, Data req, Data res) {
            Recorder.OnResponse(this, handler, req, res);
        }
    }
}
