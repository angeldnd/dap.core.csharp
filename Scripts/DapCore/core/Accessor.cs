using System;

namespace angeldnd.dap {
    public abstract class Accessor : Logger {
        public abstract Entity Entity { get; }

        public virtual string GetLogPrefix() {
            if (Entity != null) {
                return string.Format("{0}[{1}]", Entity.GetLogPrefix(), GetType().Name);
            } else {
                return string.Format("[] [{0}]", GetType().Name);
            }
        }

        //SILP: ACCESSOR_LOG_MIXIN()
        private DebugLogger _DebugLogger = DebugLogger.Instance;                                     //__SILP__
                                                                                                     //__SILP__
        public bool DebugMode {                                                                      //__SILP__
            get { return Entity != null && Entity.DebugMode; }                                       //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        public bool LogDebug {                                                                       //__SILP__
            get { return (Entity != null && Entity.LogDebug) || Log.LogDebug; }                      //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        public void Critical(string format, params object[] values) {                                //__SILP__
            if (DebugMode) {                                                                         //__SILP__
                _DebugLogger.Critical(                                                               //__SILP__
                        _DebugLogger.GetLogHint() + GetLogPrefix() + string.Format(format, values)); //__SILP__
            } else {                                                                                 //__SILP__
                Log.Critical(GetLogPrefix() + string.Format(format, values));                        //__SILP__
            }                                                                                        //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        public void Error(string format, params object[] values) {                                   //__SILP__
            if (DebugMode) {                                                                         //__SILP__
                _DebugLogger.Error(                                                                  //__SILP__
                        _DebugLogger.GetLogHint() + GetLogPrefix() + string.Format(format, values)); //__SILP__
            } else {                                                                                 //__SILP__
                Log.Error(GetLogPrefix() + string.Format(format, values));                           //__SILP__
            }                                                                                        //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        public void Info(string format, params object[] values) {                                    //__SILP__
            if (DebugMode) {                                                                         //__SILP__
                _DebugLogger.LogWithPatterns(LoggerConsts.INFO, Entity.DebugPatterns,                //__SILP__
                        _DebugLogger.GetLogHint() + GetLogPrefix() + string.Format(format, values)); //__SILP__
            } else {                                                                                 //__SILP__
                Log.Info(GetLogPrefix() + string.Format(format, values));                            //__SILP__
            }                                                                                        //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        public void Debug(string format, params object[] values) {                                   //__SILP__
            if (DebugMode) {                                                                         //__SILP__
                _DebugLogger.LogWithPatterns(LoggerConsts.DEBUG, Entity.DebugPatterns,               //__SILP__
                        _DebugLogger.GetLogHint() + GetLogPrefix() + string.Format(format, values)); //__SILP__
            } else {                                                                                 //__SILP__
                Log.Debug(GetLogPrefix() + string.Format(format, values));                           //__SILP__
            }                                                                                        //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
    }
}
