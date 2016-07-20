using System;
using System.Diagnostics;
using System.IO;

namespace angeldnd.dap {
    public static class Log {
        private static LogProvider _Provider = new FileLogProvider();
        public static LogProvider Provider {
            get { return _Provider; }
        }

        private static bool _LogDebug = true;
        public static bool LogDebug {
            get { return _LogDebug; }
        }

        private static bool _Inited = false;
        public static bool Init(LogProvider provider) {
            if (!_Inited && provider != null) {
                _Inited = true;
                _Provider = provider;
                _LogDebug = provider.LogDebug;
                return true;
            } else {
                Error("Already Inited: {0} -> {1}", _Provider, provider);
            }
            return false;
        }

        public static string GetMsg(string format, params object[] values) {
            string msg = format;
            if (values != null && values.Length > 0) {
                msg = string.Format(format, values);
            }
            return msg;
        }

        public static string GetMsg(string prefix, string format, params object[] values) {
            string msg = format;
            if (values != null && values.Length > 0) {
                msg = string.Format(format, values);
            }
            return string.Format("{0}{1}", prefix, msg);
        }

        public static void AddLog(object source, string kind,
                                  string format, params object[] values) {
            _Provider.AddLog(source, kind, GetMsg(format, values), null);
        }

        public static void AddLogWithStackTrace(object source, string kind,
                                                string format, params object[] values) {
            StackTrace stackTrace = new StackTrace(2, true);
            _Provider.AddLog(source, kind, GetMsg(format, values), stackTrace);
        }

        public static void AddLog(object source, string kind, string prefix,
                                  string format, params object[] values) {
            _Provider.AddLog(source, kind, GetMsg(prefix, format, values), null);
        }

        public static void AddLogWithStackTrace(object source, string kind, string prefix,
                                                string format, params object[] values) {
            StackTrace stackTrace = new StackTrace(2, true);
            _Provider.AddLog(source, kind, GetMsg(prefix, format, values), stackTrace);
        }

        public static void Flush() {
            _Provider.Flush();
        }

        public static void CriticalFrom(object source, string format, params object[] values) {
            string msg = GetMsg(format, values);
            AddLogWithStackTrace(source, LoggerConsts.CRITICAL, msg);
            throw new DapException(msg);
        }

        public static void ErrorFrom(object source, string format, params object[] values) {
            AddLogWithStackTrace(source, LoggerConsts.ERROR, format, values);
        }

        public static void InfoFrom(object source, string format, params object[] values) {
            AddLog(source, LoggerConsts.INFO, format, values);
        }

        public static void DebugFrom(object source, string format, params object[] values) {
            if (LogDebug) {
                AddLog(source, LoggerConsts.DEBUG, format, values);
            }
        }

        public static void ErrorOrDebugFrom(bool isDebug, object source, string format, params object[] values) {
            if (!isDebug) {
                AddLogWithStackTrace(source, LoggerConsts.ERROR, format, values);
            } else if (LogDebug) {
                AddLog(source, LoggerConsts.DEBUG, format, values);
            }
        }

        public static void CustomFrom(object source, string kind, string format, params object[] values) {
            AddLog(source, kind, format, values);
        }

        public static void Critical(string format, params object[] values) {
            string msg = GetMsg(format, values);
            AddLogWithStackTrace(null, LoggerConsts.CRITICAL, format, values);
            throw new DapException(msg);
        }

        public static void Error(string format, params object[] values) {
            AddLogWithStackTrace(null, LoggerConsts.ERROR, format, values);
        }

        public static void Info(string format, params object[] values) {
            AddLog(null, LoggerConsts.INFO, format, values);
        }

        public static void Debug(string format, params object[] values) {
            if (LogDebug) {
                AddLog(null, LoggerConsts.DEBUG, format, values);
            }
        }

        public static void ErrorOrDebug(bool isDebug, string format, params object[] values) {
            if (!isDebug) {
                AddLogWithStackTrace(null, LoggerConsts.ERROR, format, values);
            } else if (LogDebug) {
                AddLog(null, LoggerConsts.DEBUG, format, values);
            }
        }

        public static void Custom(string kind, string format, params object[] values) {
            AddLog(null, kind, format, values);
        }
    }
}
