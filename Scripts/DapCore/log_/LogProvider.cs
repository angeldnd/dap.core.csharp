using System;
using System.Diagnostics;
using System.IO;

namespace angeldnd.dap {
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
    public abstract class LogProvider : ILogger {
        public abstract void AddLog(object source, string kind, string msg, StackTrace stackTrace);
        public abstract void Flush();

        private bool _LogDebug;
        public bool LogDebug {
            get { return _LogDebug; }
        }

        protected LogProvider(bool logDebug) {
            _LogDebug = logDebug;
        }

        public void Critical(string format, params object[] values) {
            StackTrace stackTrace = new StackTrace(1, true);
            AddLog(this, LoggerConsts.CRITICAL, Log.GetMsg(format, values), stackTrace);
        }

        public void Error(string format, params object[] values) {
            StackTrace stackTrace = new StackTrace(1, true);
            AddLog(this, LoggerConsts.ERROR, Log.GetMsg(format, values), stackTrace);
        }

        public void Info(string format, params object[] values) {
            AddLog(this, LoggerConsts.INFO, Log.GetMsg(format, values), null);
        }

        public void Debug(string format, params object[] values) {
            if (_LogDebug) AddLog(this, LoggerConsts.DEBUG, Log.GetMsg(format, values), null);
        }

        public void Custom(string kind, string format, params object[] values) {
            AddLog(this, kind, Log.GetMsg(format, values), null);
        }

        public string FormatStackTrace(StackTrace stackTrace, string prefix, int max) {
            StringWriter writer = new StringWriter();
            for (int i = 0; i< stackTrace.FrameCount; i++) {
                if (i >= max) break;
                StackFrame stackFrame = stackTrace.GetFrame(i);
                var method = stackFrame.GetMethod();
                string line = string.Format("{0}{1}<{2}>{3}.{4}:{5}()", prefix,
                                            Path.GetFileName(stackFrame.GetFileName()),
                                            stackFrame.GetFileLineNumber(),
                                            method.ReflectedType.Namespace,
                                            method.ReflectedType.Name,
                                            method.Name
                );
                writer.WriteLine(line);
            }
            return writer.ToString();
        }
    }
}
