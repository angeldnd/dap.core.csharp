using System;

namespace angeldnd.dap {
    public interface Accessor {
        DapObject GetObject();
    }

    public interface Accessor<T> : Accessor, Logger where T : class, DapObject {
        T Object { get; }
    }

    public abstract class BaseAccessor<T> : DapLogger, Accessor<T> where T : class, DapObject {
        private T _Object = null;
        public T Object {
            get { return _Object; }
        }

        public DapObject GetObject() {
            return _Object;
        }

        protected bool _Setup(Object source, T obj) {
            if (_Object == null && obj != null) {
                if (source != null) {
                    SetLogSource(source);
                }
                _Object = obj;
                OnSetup();
                return true;
            } else {
                Error("Already Setup: LogSource = {0}, _Object = {1}, source = {2}, obj = {3}",
                        LogSource, _Object, source, obj);
            }
            return false;
        }

        protected virtual void OnSetup() {}

        public bool Setup(T obj) {
            return _Setup(null, obj);
        }

        public override string GetLogPrefix() {
            if (_Object != null) {
                return string.Format("{0}[{1}] ", _Object.GetLogPrefix(), LogSource.GetType().Name);
            } else {
                return string.Format("[] [{0}] ", LogSource.GetType().Name);
            }
        }

        //SILP: ACCESSOR_LOG_MIXIN(_LogSource, _Object, _Object)
        public override bool DebugMode {                                    //__SILP__
            get { return _Object != null && _Object.DebugMode; }            //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        public override string[] DebugPatterns {                            //__SILP__
            get { return _Object != null ? _Object.DebugPatterns : null; }  //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
    }

    public sealed class ProxyAccessor<T> : BaseAccessor<T> where T : class, DapObject {
        public bool Setup(Object source, T obj) {
            return _Setup(source, obj);
        }
    }
}
