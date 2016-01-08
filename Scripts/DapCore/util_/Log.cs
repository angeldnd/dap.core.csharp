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

        public readonly static DefaultLogWriter Default = new DefaultLogWriter(1);

        public static void AddLog(object source, string kind, StackTrace stackTrace, string format, params object[] values) {
            _Provider.AddLog(source, kind, stackTrace, format, values);
        }

        public static void Flush() {
            _Provider.Flush();
        }

        private readonly static DefaultLogWriter _Default = new DefaultLogWriter(2);

        public static void CriticalFrom(object source, string format, params object[] values) {
            _Default.CriticalFrom(source, format, values);
        }

        public static void ErrorFrom(object source, string format, params object[] values) {
            _Default.ErrorFrom(source, format, values);
        }

        public static void InfoFrom(object source, string format, params object[] values) {
            _Default.InfoFrom(source, format, values);
        }

        public static void DebugFrom(object source, string format, params object[] values) {
            if (LogDebug) _Default.DebugFrom(source, format, values);
        }

        public static void CustomFrom(object source, string kind, string format, params object[] values) {
            _Default.CustomFrom(source, kind, format, values);
        }

        public static void Critical(string format, params object[] values) {
            _Default.Critical(format, values);
        }

        public static void Error(string format, params object[] values) {
            _Default.Error(format, values);
        }

        public static void Info(string format, params object[] values) {
            _Default.Info(format, values);
        }

        public static void Debug(string format, params object[] values) {
            if (LogDebug) {
                _Default.Debug(format, values);
            }
        }

        public static void Custom(string kind, string format, params object[] values) {
            _Default.Custom(null, format, values);
        }
    }
}
