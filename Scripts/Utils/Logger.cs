using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

using UnityEngine;

namespace ADD.Utils {
    public interface Logger {
        bool LogDebug { get; }
        void Critical(string format, params object[] values);
        void Error(string format, params object[] values);
        void Info(string format, params object[] values);
        void Debug(string format, params object[] values);
    }

    public class DebugLogger : Logger {
        public static readonly DebugLogger Instance = new DebugLogger(2);

        public readonly int IgnoreStackTraceCount;

        private DebugLogger(int ignoreStackTraceCount) {
            IgnoreStackTraceCount = ignoreStackTraceCount;
        }

        public bool LogDebug {
            get { return true; }
        }

        public string GetMethodPrefix() {
            StackTrace stackTrace = new StackTrace(IgnoreStackTraceCount, true);
            if (stackTrace == null || stackTrace.FrameCount < 1) {
                return "() ";
            }
            StackFrame stackFrame = stackTrace.GetFrame(0);
            var method = stackFrame.GetMethod();
            return method.Name + "() ";
        }

        public void Critical(string format, params object[] values) {
            StackTrace stackTrace = new StackTrace(IgnoreStackTraceCount, true);
            Log.AddLog("CRICICAL", stackTrace, format, values);
            Log.FlushLogging();
        }

        public void Error(string format, params object[] values) {
            StackTrace stackTrace = new StackTrace(IgnoreStackTraceCount, true);
            Log.AddLog("ERROR", stackTrace, format, values);
        }

        public void Info(string format, params object[] values) {
            StackTrace stackTrace = new StackTrace(IgnoreStackTraceCount, true);
            Log.AddLog("INFO", stackTrace, format, values);
        }

        public void Debug(string format, params object[] values) {
            StackTrace stackTrace = new StackTrace(IgnoreStackTraceCount, true);
            Log.AddLog("DEBUG", stackTrace, format, values);
        }

        public void LogWithPatterns(string type, string[] patterns, string format, params object[] values) {
            string msg = format;
            if (values != null && values.Length > 0) msg = string.Format(format, values);

            if (IsMatchPatterns(patterns, msg)) {
                StackTrace stackTrace = new StackTrace(IgnoreStackTraceCount, true);
                Log.AddLog(type, stackTrace, format, values);
            } else {
                Log.AddLog(type, null, format, values);
            }
        }

        public void LogWithPattern(string type, string pattern, string format, params object[] values) {
            string msg = format;
            if (values != null && values.Length > 0) msg = string.Format(format, values);

            if (IsMatchPattern(pattern, msg)) {
                StackTrace stackTrace = new StackTrace(IgnoreStackTraceCount, true);
                Log.AddLog(type, stackTrace, format, values);
            } else {
                Log.AddLog(type, null, format, values);
            }
        }

        public bool IsMatchPatterns(string[] patterns, string msg) {
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
                condition = condition.Replace("^", "");
                return msg.StartsWith(condition);
            } else if (condition.StartsWith("$")) {
                condition = condition.Replace("$", "");
                return msg.EndsWith(condition);
            } else {
                return msg.Contains(condition);
            }
        }
    }
}
