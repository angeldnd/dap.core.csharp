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
        public virtual bool DebugMode {
            get { return false; }
        }

        public virtual string LogPrefix {
            get {
                return string.Format("[{0}] ", GetType().Name);
            }
        }

        public bool LogDebug {
            get { return DebugMode || Log.LogDebug; }
        }

        public void Critical(string format, params object[] values) {
            Log.AddLogWithStackTrace(this, LoggerConsts.CRITICAL, LogPrefix + format, values);
        }

        public void Error(string format, params object[] values) {
            Log.AddLogWithStackTrace(this, LoggerConsts.ERROR, LogPrefix + format, values);
        }

        public void Info(string format, params object[] values) {
            if (DebugMode) {
                Log.AddLogWithStackTrace(this, LoggerConsts.INFO, LogPrefix + format, values);
            } else {
                Log.AddLog(this, LoggerConsts.INFO, LogPrefix + format, values);
            }
        }

        public void Debug(string format, params object[] values) {
            if (DebugMode) {
                Log.AddLogWithStackTrace(this, LoggerConsts.DEBUG, LogPrefix + format, values);
            } else {
                if (LogDebug) {
                    Log.AddLog(this, LoggerConsts.DEBUG, LogPrefix + format, values);
                }
            }
        }

        public void Custom(string kind, string format, params object[] values) {
            if (DebugMode) {
                Log.AddLogWithStackTrace(this, kind, LogPrefix + format, values);
            } else {
                Log.AddLog(this, kind, format, values);
            }
        }
    }
}
