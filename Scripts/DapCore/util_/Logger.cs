using System;
using System.Diagnostics;
using System.IO;

namespace angeldnd.dap {
    public interface ILogger {
        bool LogDebug { get; }
        void Critical(string format, params object[] values);
        void Error(string format, params object[] values);
        void Info(string format, params object[] values);
        void Debug(string format, params object[] values);
        void Custom(string kind, string format, params object[] values);
    }

    public static class LoggerConsts {
        public const string CRITICAL = "CRITICAL";
        public const string ERROR = "ERROR";
        public const string INFO = "INFO";
        public const string DEBUG = "DEBUG";
    }

    public abstract class Logger : ILogger {
        private readonly static DefaultLogWriter _DefaultWriter = new DefaultLogWriter(2);
        private readonly static DebugLogWriter _DebugWriter = new DebugLogWriter(2);

        private readonly static DefaultLogWriter _ProxyDefaultWriter = new DefaultLogWriter(3);
        private readonly static DebugLogWriter _ProxyDebugWriter = new DebugLogWriter(3);

        private object _LogSource = null;
        public object LogSource {
            get { return _LogSource; }
        }

        public Logger() {
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
                msg = _DebugWriter.GetLogHint() + msg;
            }
            return msg;
        }

        public void Critical(string format, params object[] values) {
            string msg = GetLogMsg(format, values);
            if (DebugMode) {
                _DebugWriter.CriticalFrom(_LogSource, msg);
            } else {
                _DefaultWriter.CriticalFrom(_LogSource, msg);
            }
        }

        public void Error(string format, params object[] values) {
            string msg = GetLogMsg(format, values);
            if (DebugMode) {
                _DebugWriter.ErrorFrom(_LogSource, msg);
            } else {
                _DefaultWriter.ErrorFrom(_LogSource, msg);
            }
        }

        public void Info(string format, params object[] values) {
            string msg = GetLogMsg(format, values);
            if (DebugMode) {
                _DebugWriter.LogWithPatternsFrom(_LogSource, LoggerConsts.INFO, DebugPatterns, msg);
            } else {
                _DefaultWriter.InfoFrom(_LogSource, msg);
            }
        }

        public void Debug(string format, params object[] values) {
            if (LogDebug) {
                string msg = GetLogMsg(format, values);
                if (DebugMode) {
                    _DebugWriter.LogWithPatternsFrom(_LogSource, LoggerConsts.DEBUG, DebugPatterns, msg);
                } else {
                    _DefaultWriter.DebugFrom(_LogSource, msg);
                }
            }
        }

        public void Custom(string kind, string format, params object[] values) {
            string msg = GetLogMsg(format, values);
            if (DebugMode) {
                _DebugWriter.LogWithPatternsFrom(_LogSource, kind, DebugPatterns, msg);
            } else {
                _DefaultWriter.CustomFrom(_LogSource, kind, msg);
            }
        }

        public void CriticalFromProxy(string format, params object[] values) {
            string msg = GetLogMsg(format, values);
            if (DebugMode) {
                _ProxyDebugWriter.CriticalFrom(_LogSource, msg);
            } else {
                _ProxyDefaultWriter.CriticalFrom(_LogSource, msg);
            }
        }

        public void ErrorFromProxy(string format, params object[] values) {
            string msg = GetLogMsg(format, values);
            if (DebugMode) {
                _ProxyDebugWriter.ErrorFrom(_LogSource, msg);
            } else {
                _ProxyDefaultWriter.ErrorFrom(_LogSource, msg);
            }
        }

        public void InfoFromProxy(string format, params object[] values) {
            string msg = GetLogMsg(format, values);
            if (DebugMode) {
                _ProxyDebugWriter.LogWithPatternsFrom(_LogSource, LoggerConsts.INFO, DebugPatterns, msg);
            } else {
                _ProxyDefaultWriter.InfoFrom(_LogSource, msg);
            }
        }

        public void DebugFromProxy(string format, params object[] values) {
            if (LogDebug) {
                string msg = GetLogMsg(format, values);
                if (DebugMode) {
                    _ProxyDebugWriter.LogWithPatternsFrom(_LogSource, LoggerConsts.DEBUG, DebugPatterns, msg);
                } else {
                    _ProxyDefaultWriter.DebugFrom(_LogSource, msg);
                }
            }
        }

        public void CustomFromProxy(string kind, string format, params object[] values) {
            string msg = GetLogMsg(format, values);
            if (DebugMode) {
                _ProxyDebugWriter.LogWithPatternsFrom(_LogSource, kind, DebugPatterns, msg);
            } else {
                _ProxyDefaultWriter.CustomFrom(_LogSource, kind, msg);
            }
        }
    }
}
