using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IVars : IAspect, IOwner {
    }

    public sealed class Vars : DictAspect<IContext, IVar>, IVars {
        public Vars(IContext owner, string key) : base(owner, key) {
        }

        public Var<T> AddVar<T>(string key, T val) {
            Var<T> result = Add<Var<T>>(key);
            if (result != null) {
                if (!result.Setup(val)) {
                    Remove<Var<T>>(key);
                    result = null;
                }
            }
            return result;
        }

        public Var<T> AddVar<T>(string key) {
            return AddVar<T>(key, default(T));
        }

        public Var<T> RemoveVar<T>(string key) {
            return Remove<Var<T>>(key);
        }

        public bool HasVar<T>(string key) {
            return Get<Var<T>>(key, true) != null;
        }

        public Var<T> GetVar<T>(string key, bool isDebug = false) {
            return Get<Var<T>>(key, isDebug);
        }

        public T GetValue<T>(string key, T defaultValue) {
            Var<T> v = GetVar<T>(key, true);
            if (v != null) {
                return v.Value;
            }
            return defaultValue;
        }

        public T GetValue<T>(string key) {
            return GetValue<T>(key, default(T));
        }

        public bool SetValue<T>(string key, T val) {
            Var<T> v = Get<Var<T>>(key, true);
            if (v != null) {
                return v.SetValue(val);
            } else {
                v = AddVar<T>(key, val);
                return v != null;
            }
        }

        public bool DepositValue<T>(string key, T val) {
            Var<T> v = Get<Var<T>>(key, true);
            if (v == null) {
                v = AddVar<T>(key, val);
                return v != null;
            } else {
                Error("Already Exist: {0} {1} -> {2}", key, v.GetValue(), val);
            }
            return false;
        }

        public T WithdrawValue<T>(string key, T defaultValue) {
            Var<T> v = GetVar<T>(key);
            if (v != null) {
                Remove(key);
                return v.Value;
            }
            return defaultValue;
        }
    }
}
