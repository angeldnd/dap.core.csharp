using System;
using System.Diagnostics;
using System.IO;
using System.Text;

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

        public virtual bool AutoFlush {
            get { return true; }
        }
        public virtual void Flush() {}

        private bool _LogDebug;
        public bool LogDebug {
            get { return _LogDebug; }
        }

        private StringBuilder _StackBuilder = new StringBuilder(1024);

        protected LogProvider(bool logDebug) {
            _LogDebug = logDebug;
        }

        public void Critical(string format, params object[] values) {
#if DOTNET_CORE
            StackTrace stackTrace = Log.FakeStackTrace;
#else
            StackTrace stackTrace = new StackTrace(1, true);
#endif
            string msg = Log.GetMsg(format, values);
            AddLog(this, LoggerConsts.CRITICAL, msg, stackTrace);
            throw new DapException(msg);
        }

        public void Error(string format, params object[] values) {
#if DOTNET_CORE
            StackTrace stackTrace = Log.FakeStackTrace;
#else
            StackTrace stackTrace = new StackTrace(1, true);
#endif
            AddLog(this, LoggerConsts.ERROR, Log.GetMsg(format, values), stackTrace);
        }

        public void Info(string format, params object[] values) {
            AddLog(this, LoggerConsts.INFO, Log.GetMsg(format, values), null);
        }

        public void Debug(string format, params object[] values) {
            if (_LogDebug) {
                AddLog(this, LoggerConsts.DEBUG, Log.GetMsg(format, values), null);
            }
        }

        public void ErrorOrDebug(bool isDebug, string format, params object[] values) {
            if (!isDebug) {
                AddLog(this, LoggerConsts.ERROR, Log.GetMsg(format, values), null);
            } else if (_LogDebug) {
                AddLog(this, LoggerConsts.DEBUG, Log.GetMsg(format, values), null);
            }
        }

        public void Custom(string kind, string format, params object[] values) {
            AddLog(this, kind, Log.GetMsg(format, values), null);
        }

        public string FormatStackTrace(StackTrace stackTrace, string prefix, int max) {
#if DOTNET_CORE
            return Environment.StackTrace;
#else
            _StackBuilder.Length = 0;
            for (int i = 0; i< stackTrace.FrameCount; i++) {
                if (i >= max) break;
                StackFrame stackFrame = stackTrace.GetFrame(i);
                var method = stackFrame.GetMethod();
                _StackBuilder.Append(prefix);
                _StackBuilder.Append(Path.GetFileName(stackFrame.GetFileName()));
                _StackBuilder.Append("<");
                _StackBuilder.Append(stackFrame.GetFileLineNumber());
                _StackBuilder.Append(">\t");
                _StackBuilder.Append(method.ReflectedType.Namespace);
                _StackBuilder.Append(".");
                _StackBuilder.Append(method.ReflectedType.Name);
                _StackBuilder.Append("\t");
                _StackBuilder.Append(method.Name);
                _StackBuilder.Append("()\n");
            }
            return _StackBuilder.ToString();
#endif
        }
    }
}
