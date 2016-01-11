using System;

namespace angeldnd.dap {
    public class ProxyAccessor<T> : ILogger, IAccessor<T>
                                            where T : IObject {
        private readonly static DefaultLogWriter _DefaultWriter = new DefaultLogWriter(3);
        private readonly static DebugLogWriter _DebugWriter = new DebugLogWriter(3);

        private readonly object _Source;

        private readonly T _Obj;
        public T Obj {
            get { return _Obj; }
        }

        public IObject GetObj() {
            return _Obj;
        }

        public ProxyAccessor(object source, T obj) {
            if (source != null) {
                throw new NullReferenceException(LogPrefix + "source is null");
            }
            if (obj != null) {
                throw new NullReferenceException(LogPrefix + "obj is null");
            }
            _Source = source;
            _Obj = obj;
        }

        public string LogPrefix {
            get {
                return string.Format("[{0}] {1}", _Source.GetType().Name, Obj.LogPrefix);
            }
        }

        public bool LogDebug {
            get { return Obj.DebugMode || Log.LogDebug; }
        }

        private string GetLogMsg(string format, params object[] values) {
            string msg = LogPrefix + string.Format(format, values);
            if (Obj.DebugMode) {
                msg = _DebugWriter.GetLogHint() + msg;
            }
            return msg;
        }

        public void Critical(string format, params object[] values) {
            string msg = GetLogMsg(format, values);
            if (Obj.DebugMode) {
                _DebugWriter.CriticalFrom(_Source, msg);
            } else {
                _DefaultWriter.CriticalFrom(_Source, msg);
            }
        }

        public void Error(string format, params object[] values) {
            string msg = GetLogMsg(format, values);
            if (Obj.DebugMode) {
                _DebugWriter.ErrorFrom(_Source, msg);
            } else {
                _DefaultWriter.ErrorFrom(_Source, msg);
            }
        }

        public void Info(string format, params object[] values) {
            string msg = GetLogMsg(format, values);
            if (Obj.DebugMode) {
                _DebugWriter.LogWithPatternsFrom(_Source, LoggerConsts.INFO, Obj.DebugPatterns, msg);
            } else {
                _DefaultWriter.InfoFrom(_Source, msg);
            }
        }

        public void Debug(string format, params object[] values) {
            if (LogDebug) {
                string msg = GetLogMsg(format, values);
                if (Obj.DebugMode) {
                    _DebugWriter.LogWithPatternsFrom(_Source, LoggerConsts.DEBUG, Obj.DebugPatterns, msg);
                } else {
                    _DefaultWriter.DebugFrom(_Source, msg);
                }
            }
        }

        public void Custom(string kind, string format, params object[] values) {
            string msg = GetLogMsg(format, values);
            if (Obj.DebugMode) {
                _DebugWriter.LogWithPatternsFrom(_Source, kind, Obj.DebugPatterns, msg);
            } else {
                _DefaultWriter.CustomFrom(_Source, kind, msg);
            }
        }
    }
}
