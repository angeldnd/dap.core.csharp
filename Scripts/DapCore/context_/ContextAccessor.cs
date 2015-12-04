using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface ContextAccessor : Accessor {
        Context GetContext();
    }

    public interface ContextAccessor<T> : Accessor<T>, ContextAccessor where T : Context {
        T Context { get; }
    }

    public class BaseContextAccessor : BaseAccessor<Context>, ContextAccessor<Context> {
        public Context GetContext() {
            return Object;
        }

        public Context Context {
            get { return Object; }
        }
    }

    public class BaseContextAccessor<T> : BaseAccessor<T>, ContextAccessor<T> where T : Context {
        public Context GetContext() {
            return Object;
        }

        public T Context {
            get { return Object; }
        }
    }
}
