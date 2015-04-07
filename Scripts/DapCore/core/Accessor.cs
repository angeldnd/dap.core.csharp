using System;

namespace angeldnd.dap {
    public abstract class Accessor : Logger {
        public abstract Entity Entity { get; }

        //SILP: ASPECT_LOG_MIXIN()
        private DebugLogger _DebugLogger = DebugLogger.Instance;                                          //__SILP__
                                                                                                          //__SILP__
        public bool DebugMode {                                                                           //__SILP__
            get { return Entity != null && Entity.DebugMode; }                                            //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public bool LogDebug {                                                                            //__SILP__
            get { return (Entity != null && Entity.LogDebug) || Log.LogDebug; }                           //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public virtual string GetLogPrefix() {                                                            //__SILP__
            if (_Entity != null) {                                                                        //__SILP__
                return string.Format("{0}[{1}] ", _Entity.GetLogPrefix(), GetType().Name);                //__SILP__
            } else {                                                                                      //__SILP__
                return string.Format("[] [] [{0}] ", GetType().Name);                                     //__SILP__
            }                                                                                             //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public void Critical(string format, params object[] values) {                                     //__SILP__
            if (DebugMode) {                                                                              //__SILP__
                _DebugLogger.Critical(GetLogPrefix() + string.Format(format, values));                    //__SILP__
            } else {                                                                                      //__SILP__
                Log.Critical(GetLogPrefix() + string.Format(format, values));                             //__SILP__
            }                                                                                             //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public void Error(string format, params object[] values) {                                        //__SILP__
            if (DebugMode) {                                                                              //__SILP__
                _DebugLogger.Error(GetLogPrefix() + string.Format(format, values));                       //__SILP__
            } else {                                                                                      //__SILP__
                Log.Error(GetLogPrefix() + string.Format(format, values));                                //__SILP__
            }                                                                                             //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public void Info(string format, params object[] values) {                                         //__SILP__
            if (DebugMode) {                                                                              //__SILP__
                _DebugLogger.LogWithPatterns(LoggerConsts.INFO, Entity.DebugPatterns,                     //__SILP__
                        GetLogPrefix() + _DebugLogger.GetMethodPrefix() + string.Format(format, values)); //__SILP__
            } else {                                                                                      //__SILP__
                Log.Info(GetLogPrefix() + string.Format(format, values));                                 //__SILP__
            }                                                                                             //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public void Debug(string format, params object[] values) {                                        //__SILP__
            if (DebugMode) {                                                                              //__SILP__
                _DebugLogger.LogWithPatterns(LoggerConsts.DEBUG, Entity.DebugPatterns,                    //__SILP__
                        GetLogPrefix() + _DebugLogger.GetMethodPrefix() + string.Format(format, values)); //__SILP__
            } else {                                                                                      //__SILP__
                Log.Debug(GetLogPrefix() + string.Format(format, values));                                //__SILP__
            }                                                                                             //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
    }
}
