using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface ContextAspect : Aspect {
        Context GetContext();
    }

    public abstract class ContextAspect<T> : BaseSecurableAspect, ContextAccessor<T>, ContextAspect where T : Context {
        private T _Context = null;
        public T Context {
            get { return _Context; }
        }

        public Context GetContext() {
            return _Context;
        }

        public T Object {
            get { return _Context; }
        }

        public DapObject GetObject() {
            return _Context;
        }

        public override bool Init(Entity entity, string path, Pass pass) {
            if (!base.Init(entity, path, pass)) {
                return false;
            }
            if (entity is T) {
                _Context = (T)entity;
                return true;
            } else {
                Error("Invalid Entity: {0} -> {1}", typeof(T).FullName, entity.GetType().FullName);
                return false;
            }
        }
    }
}
