using System;

namespace angeldnd.dap {
    public interface Accessor : Logger {
        Entity Entity { get; }
    }

    public abstract class BaseAccessor : Accessor {
        public abstract Entity Entity { get; }
        protected Object _Source;

        public BaseAccessor() {
            _Source = this;
        }

        protected BaseAccessor(Object source) {
            _Source = source;
        }

        public virtual string GetLogPrefix() {
            if (Entity != null) {
                return string.Format("{0}[{1}]", Entity.GetLogPrefix(), GetType().Name);
            } else {
                return string.Format("[] [{0}]", GetType().Name);
            }
        }

        //SILP: ACCESSOR_LOG_MIXIN(_Source, Entity)
        private DebugLogger _DebugLogger = DebugLogger.Instance;                                      //__SILP__
                                                                                                      //__SILP__
        public bool DebugMode {                                                                       //__SILP__
            get { return Entity != null && Entity.DebugMode; }                                        //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public bool LogDebug {                                                                        //__SILP__
            get { return (Entity != null && Entity.LogDebug) || Log.LogDebug; }                       //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public void Critical(string format, params object[] values) {                                 //__SILP__
            Log.Source = _Source;                                                                     //__SILP__
            if (DebugMode) {                                                                          //__SILP__
                _DebugLogger.Critical(                                                                //__SILP__
                        _DebugLogger.GetLogHint() + GetLogPrefix() + string.Format(format, values));  //__SILP__
            } else {                                                                                  //__SILP__
                Log.Critical(GetLogPrefix() + string.Format(format, values));                         //__SILP__
            }                                                                                         //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public void Error(string format, params object[] values) {                                    //__SILP__
            Log.Source = _Source;                                                                     //__SILP__
            if (DebugMode) {                                                                          //__SILP__
                _DebugLogger.Error(                                                                   //__SILP__
                        _DebugLogger.GetLogHint() + GetLogPrefix() + string.Format(format, values));  //__SILP__
            } else {                                                                                  //__SILP__
                Log.Error(GetLogPrefix() + string.Format(format, values));                            //__SILP__
            }                                                                                         //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public void Info(string format, params object[] values) {                                     //__SILP__
            Log.Source = _Source;                                                                     //__SILP__
            if (DebugMode) {                                                                          //__SILP__
                _DebugLogger.LogWithPatterns(LoggerConsts.INFO, Entity.DebugPatterns,                 //__SILP__
                        _DebugLogger.GetLogHint() + GetLogPrefix() + string.Format(format, values));  //__SILP__
            } else {                                                                                  //__SILP__
                Log.Info(GetLogPrefix() + string.Format(format, values));                             //__SILP__
            }                                                                                         //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public void Debug(string format, params object[] values) {                                    //__SILP__
            Log.Source = _Source;                                                                     //__SILP__
            if (DebugMode) {                                                                          //__SILP__
                _DebugLogger.LogWithPatterns(LoggerConsts.DEBUG, Entity.DebugPatterns,                //__SILP__
                        _DebugLogger.GetLogHint() + GetLogPrefix() + string.Format(format, values));  //__SILP__
            } else {                                                                                  //__SILP__
                Log.Debug(GetLogPrefix() + string.Format(format, values));                            //__SILP__
            }                                                                                         //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
    }

    public class ProxyAccessor<T> : BaseAccessor where T : Entity {
        private readonly bool _Sticky;

        public ProxyAccessor(bool sticky) {
            _Sticky = sticky;
        }

        public override Entity Entity {
            get { return _Target; }
        }

        public Object Source {
            get { return _Source; }
        }

        private T _Target = null;
        public T Target {
            get { return _Target; }
        }

        public bool Init(Object source, T target) {
            if (_Target == null && target != null) {
                _Source = source;
                _Target = target;
                return true;
            } else if (!_Sticky) {
                _Source = source;
                _Target = target;
                return true;
            } else {
                Error("Init: _Sticky = {0}, _Source = {1}, _Target = {2}, source = {3}, target = {4}",
                        _Sticky, _Source, _Target, source, target);
            }
            return false;
        }
    }
}
