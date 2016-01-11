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

        public virtual bool DebugMode {
            get { return false; }
        }

        public virtual string[] DebugPatterns {
            get { return null; }
        }

        public virtual string LogPrefix {
            get {
                return string.Format("[{0}] ", GetType().Name);
            }
        }

        public bool LogDebug {
            get { return DebugMode || Log.LogDebug; }
        }

        private string GetLogMsg(string format, params object[] values) {
            string msg = LogPrefix + string.Format(format, values);
            if (DebugMode) {
                msg = _DebugWriter.GetLogHint() + msg;
            }
            return msg;
        }

        public void Critical(string format, params object[] values) {
            string msg = GetLogMsg(format, values);
            if (DebugMode) {
                _DebugWriter.CriticalFrom(this, msg);
            } else {
                _DefaultWriter.CriticalFrom(this, msg);
            }
        }

        public void Error(string format, params object[] values) {
            string msg = GetLogMsg(format, values);
            if (DebugMode) {
                _DebugWriter.ErrorFrom(this, msg);
            } else {
                _DefaultWriter.ErrorFrom(this, msg);
            }
        }

        public void Info(string format, params object[] values) {
            string msg = GetLogMsg(format, values);
            if (DebugMode) {
                _DebugWriter.LogWithPatternsFrom(this, LoggerConsts.INFO, DebugPatterns, msg);
            } else {
                _DefaultWriter.InfoFrom(this, msg);
            }
        }

        public void Debug(string format, params object[] values) {
            if (LogDebug) {
                string msg = GetLogMsg(format, values);
                if (DebugMode) {
                    _DebugWriter.LogWithPatternsFrom(this, LoggerConsts.DEBUG, DebugPatterns, msg);
                } else {
                    _DefaultWriter.DebugFrom(this, msg);
                }
            }
        }

        public void Custom(string kind, string format, params object[] values) {
            string msg = GetLogMsg(format, values);
            if (DebugMode) {
                _DebugWriter.LogWithPatternsFrom(this, kind, DebugPatterns, msg);
            } else {
                _DefaultWriter.CustomFrom(this, kind, msg);
            }
        }
    }
}
