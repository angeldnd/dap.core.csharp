using System;

namespace angeldnd.dap {
    public abstract class DapLogger : Logger {
        private readonly static DefaultLogger _DefaultLogger = new DefaultLogger(2);
        private readonly static DebugLogger _DebugLogger = new DebugLogger(2);

        private object _LogSource = null;
        public object LogSource {
            get { return _LogSource; }
        }

        public DapLogger() {
            _LogSource = this;
        }

        protected bool SetLogSource(object source) {
            if (source != null) {
                _LogSource = source;
                return true;
            } else {
                Error("Already Set: {0} -> {1}", _LogSource, source);
            }
            return false;
        }

        public virtual bool DebugMode {
            get { return false; }
        }

        public virtual string[] DebugPatterns {
            get { return null; }
        }

        public abstract string GetLogPrefix();

        public bool LogDebug {
            get { return DebugMode || Log.LogDebug; }
        }

        private string GetLogMsg(string format, params object[] values) {
            string msg = GetLogPrefix() + string.Format(format, values);
            if (DebugMode) {
                msg = _DebugLogger.GetLogHint() + msg;
            }
            return msg;
        }

        public void Critical(string format, params object[] values) {
            string msg = GetLogMsg(format, values);
            if (DebugMode) {
                _DebugLogger.CriticalFrom(_LogSource, msg);
            } else {
                _DefaultLogger.CriticalFrom(_LogSource, msg);
            }
        }

        public void Error(string format, params object[] values) {
            string msg = GetLogMsg(format, values);
            if (DebugMode) {
                _DebugLogger.ErrorFrom(_LogSource, msg);
            } else {
                _DefaultLogger.ErrorFrom(_LogSource, msg);
            }
        }

        public void Info(string format, params object[] values) {
            string msg = GetLogMsg(format, values);
            if (DebugMode) {
                _DebugLogger.LogWithPatternsFrom(_LogSource, LoggerConsts.INFO, DebugPatterns, msg);
            } else {
                _DefaultLogger.InfoFrom(_LogSource, msg);
            }
        }

        public void Debug(string format, params object[] values) {
            if (LogDebug) {
                string msg = GetLogMsg(format, values);
                if (DebugMode) {
                    _DebugLogger.LogWithPatternsFrom(_LogSource, LoggerConsts.DEBUG, DebugPatterns, msg);
                } else {
                    _DefaultLogger.DebugFrom(_LogSource, msg);
                }
            }
        }

        public void Custom(string kind, string format, params object[] values) {
            string msg = GetLogMsg(format, values);
            if (DebugMode) {
                _DebugLogger.LogWithPatternsFrom(_LogSource, kind, DebugPatterns, msg);
            } else {
                _DefaultLogger.CustomFrom(_LogSource, kind, msg);
            }
        }
    }
}
