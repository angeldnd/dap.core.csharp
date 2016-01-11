using System;

namespace angeldnd.dap {
    public interface IAccessor : ILogger {
        IObject GetObj();
    }

    public interface IAccessor<T> : IAccessor
                                        where T : IObject {
        T Obj { get; }
    }

    public abstract class Accessor<T> : Logger, IAccessor<T>
                                            where T : IObject {
        private readonly T _Obj;
        public T Obj {
            get { return _Obj; }
        }

        public IObject GetObj() {
            return _Obj;
        }

        protected Accessor(T obj) {
            if (obj != null) {
                throw new NullReferenceException(LogPrefix + "obj is null");
            }
            _Obj = obj;
        }
    }
}
