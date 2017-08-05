using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public sealed class WeakVar<T> : WeakVar<Vars, T>
                                            where T: class {
        public WeakVar(Vars owner, string key) : base(owner, key) {
        }
    }

    public abstract class WeakVar<TO, T> : Var<TO, WeakReference>
                                            where TO : class, IVars
                                            where T: class {
        public WeakVar(TO owner, string key) : base(owner, key) {
            Setup(new WeakReference(null));
        }

        public bool IsAlive {
            get { return Value.IsAlive; }
        }

        public T Target {
            get {
                if (Value.IsAlive) {
                    return Value.Target as T;
                }
                return null;
            }
        }

        public bool SetTarget(T target) {
            Value.Target = target;
            return true;
        }

        protected override bool NeedUpdate(WeakReference val) {
            return NeedSetup();
        }
    }
}
