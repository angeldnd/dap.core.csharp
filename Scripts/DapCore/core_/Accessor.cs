using System;

namespace angeldnd.dap {
    public interface Accessor {
        DapObject GetObject();
    }

    public interface Accessor<T> : Accessor, Logger where T : class, DapObject {
        T Object { get; }
    }

    public abstract class BaseAccessor<T> : Accessor<T> where T : class, DapObject {
        private Object _LogSource = null;

        private T _Object = null;
        public T Object {
            get { return _Object; }
        }

        public DapObject GetObject() {
            return _Object;
        }

        public Accessor() {
            _LogSource = this;
        }

        protected bool _Setup(Object source, T obj) {
            if (_Object == null && obj != null) {
                if (source != null) {
                    _LogSource = source;
                }
                _Object = obj;
                OnSetup();
                return true;
            } else {
                Error("Already Setup: _LogSource = {0}, _Object = {1}, source = {2}, obj = {3}",
                        _LogSource, _Object, source, obj);
            }
            return false;
        }

        protected virtual void OnSetup() {}

        public bool Setup(T obj) {
            return _Setup(null, obj);
        }

        public virtual string GetLogPrefix() {
            if (_Object != null) {
                return string.Format("{0}[{1}] ", _Object.GetLogPrefix(), _LogSource.GetType().Name);
            } else {
                return string.Format("[] [{0}] ", _LogSource.GetType().Name);
            }
        }

        //SILP: ACCESSOR_LOG_MIXIN(_LogSource, _Object, _Object)
        private DebugLogger _DebugLogger = DebugLogger.Instance;                                      //__SILP__
                                                                                                      //__SILP__
        public bool DebugMode {                                                                       //__SILP__
            get { return _Object != null && _Object.DebugMode; }                                      //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public bool LogDebug {                                                                        //__SILP__
            get { return (_Object != null && _Object.LogDebug) || Log.LogDebug; }                     //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public void Critical(string format, params object[] values) {                                 //__SILP__
            Log.Source = _LogSource;                                                                  //__SILP__
            if (DebugMode) {                                                                          //__SILP__
                _DebugLogger.Critical(                                                                //__SILP__
                        _DebugLogger.GetLogHint() + GetLogPrefix() + string.Format(format, values));  //__SILP__
            } else {                                                                                  //__SILP__
                Log.Critical(GetLogPrefix() + string.Format(format, values));                         //__SILP__
            }                                                                                         //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public void Error(string format, params object[] values) {                                    //__SILP__
            Log.Source = _LogSource;                                                                  //__SILP__
            if (DebugMode) {                                                                          //__SILP__
                _DebugLogger.Error(                                                                   //__SILP__
                        _DebugLogger.GetLogHint() + GetLogPrefix() + string.Format(format, values));  //__SILP__
            } else {                                                                                  //__SILP__
                Log.Error(GetLogPrefix() + string.Format(format, values));                            //__SILP__
            }                                                                                         //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public void Info(string format, params object[] values) {                                     //__SILP__
            Log.Source = _LogSource;                                                                  //__SILP__
            if (DebugMode) {                                                                          //__SILP__
                _DebugLogger.LogWithPatterns(LoggerConsts.INFO, _Object.DebugPatterns,                //__SILP__
                        _DebugLogger.GetLogHint() + GetLogPrefix() + string.Format(format, values));  //__SILP__
            } else {                                                                                  //__SILP__
                Log.Info(GetLogPrefix() + string.Format(format, values));                             //__SILP__
            }                                                                                         //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public void Debug(string format, params object[] values) {                                    //__SILP__
            Log.Source = _LogSource;                                                                  //__SILP__
            if (DebugMode) {                                                                          //__SILP__
                _DebugLogger.LogWithPatterns(LoggerConsts.DEBUG, _Object.DebugPatterns,               //__SILP__
                        _DebugLogger.GetLogHint() + GetLogPrefix() + string.Format(format, values));  //__SILP__
            } else {                                                                                  //__SILP__
                Log.Debug(GetLogPrefix() + string.Format(format, values));                            //__SILP__
            }                                                                                         //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
    }

    public sealed class ProxyAccessor<T> : BaseAccessor<T> where T : class, DapObject {
        public bool Setup(Object source, T obj) {
            return _Setup(source, obj);
        }
    }
}
