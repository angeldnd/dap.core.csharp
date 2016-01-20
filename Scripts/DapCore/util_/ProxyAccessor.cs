using System;

namespace angeldnd.dap {
    public class ProxyAccessor<T> : ILogger, IAccessor<T>
                                            where T : IObject {
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

        public void Critical(string format, params object[] values) {
            Obj.CriticalFromProxy(format, values);
        }

        public void Error(string format, params object[] values) {
            Obj.ErrorFromProxy(format, values);
        }

        public void Info(string format, params object[] values) {
            Obj.InfoFromProxy(format, values);
        }

        public void Debug(string format, params object[] values) {
            Obj.DebugFromProxy(format, values);
        }

        public void Custom(string kind, string format, params object[] values) {
            Obj.CustomFromProxy(kind, format, values);
        }
    }
}
