using System;
using System.Diagnostics;
using System.IO;
//using System.Text.RegularExpressions;

namespace angeldnd.dap {
    public struct LoggerConsts {
        public const string CRITICAL = "CRITICAL";
        public const string ERROR = "ERROR";
        public const string INFO = "INFO";
        public const string DEBUG = "DEBUG";
    }

    public interface Logger {
        bool LogDebug { get; }
        void Critical(string format, params object[] values);
        void Error(string format, params object[] values);
        void Info(string format, params object[] values);
        void Debug(string format, params object[] values);
        void Custom(string kind, string format, params object[] values);
    }

    /*
     * Check Bootstrapper.cs for detail of the bootstrap, if there is no specific
     * config. then the AutoBootstrapper will check all LogProvider subclasses,
     * the one with highest priority will be selected.
     *
    [DapPriority(2)]
    public class TestLogProvider : FileLogProvider {
        public static bool SetupLogging() {
            Log.Provider = new TestLogProvider("dap", "init", -1, true);
            return true;
        }
    }
    */
    public abstract class LogProvider {
        public abstract void AddLog(object source, string kind, StackTrace stackTrace, string format, params object[] values);
        public abstract void Flush();
    }

    public static class Log {
        private static LogProvider _Provider = null;
        public static LogProvider Provider {
            get { return _Provider; }
        }
        private static bool _LogDebug = true;
        public static bool LogDebug {
            get { return _LogDebug; }
        }
        public static bool Init(bool logDebug, LogProvider provider) {
            if (provider != null) {
                _LogDebug = logDebug;
                _Provider = provider;
                return true;
            }
            return false;
        }

        public readonly static DefaultLogger Default = new DefaultLogger(1);

        public static void AddLog(object source, string kind, StackTrace stackTrace, string format, params object[] values) {
            if (_Provider == null) return;
            _Provider.AddLog(source, kind, stackTrace, format, values);
        }

        public static void Flush() {
            if (_Provider == null) return;
            _Provider.Flush();
        }

        private readonly static DefaultLogger _Default = new DefaultLogger(2);

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

    public abstract class BaseLogger : Logger {
        public readonly int IgnoreStackTraceCount;

        protected BaseLogger(int ignoreStackTraceCount) {
            IgnoreStackTraceCount = ignoreStackTraceCount;
        }

        public abstract bool LogDebug { get; }

        public void CriticalFrom(object source, string format, params object[] values) {
            StackTrace stackTrace = new StackTrace(IgnoreStackTraceCount, true);
            Log.AddLog(source, LoggerConsts.CRITICAL, stackTrace, format, values);
        }

        public void ErrorFrom(object source, string format, params object[] values) {
            StackTrace stackTrace = new StackTrace(IgnoreStackTraceCount, true);
            Log.AddLog(source, LoggerConsts.ERROR, stackTrace, format, values);
        }

        public void InfoFrom(object source, string format, params object[] values) {
            Log.AddLog(source, LoggerConsts.INFO, null, format, values);
        }

        public void DebugFrom(object source, string format, params object[] values) {
            if (LogDebug) {
                Log.AddLog(source, LoggerConsts.DEBUG, null, format, values);
            }
        }

        public void CustomFrom(object source, string kind, string format, params object[] values) {
            Log.AddLog(source, kind, null, format, values);
        }

        public void Critical(string format, params object[] values) {
            StackTrace stackTrace = new StackTrace(IgnoreStackTraceCount, true);
            Log.AddLog(null, LoggerConsts.CRITICAL, stackTrace, format, values);
        }

        public void Error(string format, params object[] values) {
            StackTrace stackTrace = new StackTrace(IgnoreStackTraceCount, true);
            Log.AddLog(null, LoggerConsts.ERROR, stackTrace, format, values);
        }

        public void Info(string format, params object[] values) {
            Log.AddLog(null, LoggerConsts.INFO, null, format, values);
        }

        public void Debug(string format, params object[] values) {
            if (LogDebug) {
                Log.AddLog(null, LoggerConsts.DEBUG, null, format, values);
            }
        }

        public void Custom(string kind, string format, params object[] values) {
            Log.AddLog(null, kind, null, format, values);
        }
    }

    public sealed class DefaultLogger : BaseLogger {
        public DefaultLogger(int ignoreStackTraceCount) : base(ignoreStackTraceCount) {
        }

        public override bool LogDebug {
            get { return Log.LogDebug; }
        }
    }

    public sealed class DebugLogger : BaseLogger {
        public DebugLogger(int ignoreStackTraceCount) : base(ignoreStackTraceCount) {
        }

        public override bool LogDebug {
            get { return true; }
        }

        public string GetLogHint() {
            StackTrace stackTrace = new StackTrace(IgnoreStackTraceCount, true);
            if (stackTrace == null || stackTrace.FrameCount < 1) {
                return "() ";
            }
            StackFrame stackFrame = stackTrace.GetFrame(0);
            var fileName = stackFrame.GetFileName();
            fileName = fileName.Substring(fileName.LastIndexOf('/') + 1);
            var lineNumber = stackFrame.GetFileLineNumber();
            var method = stackFrame.GetMethod();
            return string.Format("{0}[{1}]: {2}() ", fileName, lineNumber, method.Name);
        }

        public void LogWithPatternsFrom(object source, string kind, string[] patterns, string format, params object[] values) {
            string msg = format;
            if (values != null && values.Length > 0) msg = string.Format(format, values);

            if (IsMatchPatterns(patterns, msg)) {
                StackTrace stackTrace = new StackTrace(IgnoreStackTraceCount, true);
                Log.AddLog(source, kind, stackTrace, format, values);
            } else {
                Log.AddLog(source, kind, null, format, values);
            }
        }

        public void LogWithPatternFrom(object source, string kind, string pattern, string format, params object[] values) {
            string msg = format;
            if (values != null && values.Length > 0) msg = string.Format(format, values);

            if (IsMatchPattern(pattern, msg)) {
                StackTrace stackTrace = new StackTrace(IgnoreStackTraceCount, true);
                Log.AddLog(source, kind, stackTrace, format, values);
            } else {
                Log.AddLog(source, kind, null, format, values);
            }
        }

        public bool IsMatchPatterns(string[] patterns, string msg) {
            if (patterns == null) return false;
            foreach (string pattern in patterns) {
                if (IsMatchPattern(pattern, msg)) {
                    return true;
                }
            }
            return false;
        }

        public bool IsMatchPattern(string pattern, string msg) {
            if (string.IsNullOrEmpty(pattern)) {
                return true;
            }

            string[] segments = pattern.Split(' ');
            foreach (string segment in segments) {
                if (!IsMatchSegment(segment, msg)) {
                    return false;
                }
            }
            return true;
        }

        public bool IsMatchSegment(string segment, string msg) {
            string[] conditions = segment.Split('|');
            foreach (string condition in conditions) {
                if (IsMatchCondition(condition, msg)) {
                    return true;
                }
            }
            return false;
        }

        public bool IsMatchCondition(string condition, string msg) {
            if (condition.StartsWith("!")) {
                condition = condition.Replace("!", "");
                return !IsMatchCondition(condition, msg);
            }

            //use "+" to match spaces between words
            condition = condition.Replace("+", " ");

            //Smart Case
            string lowerCondition = condition.ToLower();
            if (lowerCondition == condition) {
                msg = msg.ToLower();
            }
            if (condition.StartsWith("^")) {
                if (condition.EndsWith("$")) {
                    return msg == condition.Replace("^", "").Replace("$", "");
                } else {
                    return msg.StartsWith(condition.Replace("^", ""));
                }
            } else if (condition.EndsWith("$")) {
                return msg.EndsWith(condition.Replace("$", ""));
            } else {
                return msg.Contains(condition);
            }
        }
    }
}
