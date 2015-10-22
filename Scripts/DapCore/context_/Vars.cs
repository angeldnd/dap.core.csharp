using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public class Vars : SecurableEntityAspect {
        public Var<T> AddVar<T>(string path, T val) {
            return AddVar<T>(path, null, val);
        }

        public Var<T> AddVar<T>(string path, Pass pass, T val) {
            Var<T> result = Add<Var<T>>(path, pass);
            if (result != null && !result.Setup(pass, val)) {
                Remove<Var<T>>(path);
                result = null;
            }
            return result;
        }

        public Var<T> RemoveVar<T>(string path) {
            return Remove<Var<T>>(path);
        }

        public Var<T> RemoveVar<T>(string path, Pass pass) {
            return Remove<Var<T>>(path, pass);
        }

        public bool HasVar<T>(string path) {
            return Get<Var<T>>(path) != null;
        }

        public Var<T> GetVar<T>(string path) {
            return Get<Var<T>>(path);
        }

        public T GetValue<T>(string path, T defaultValue) {
            Var<T> v = GetVar<T>(path);
            if (v != null) {
                return v.Value;
            }
            return defaultValue;
        }

        public T GetValue<T>(string path) {
            Var<T> v = GetVar<T>(path);
            if (v != null) {
                return v.Value;
            }
            return default(T);
        }

        public bool SetValue<T>(string path, Pass pass, T val) {
            Var<T> v = GetVar<T>(path);
            if (v != null) {
                return v.SetValue(pass, val);
            } else {
                v = AddVar<T>(path, pass, val);
                return v != null;
            }
        }

        public bool SetValue<T>(string path, T val) {
            return SetValue<T>(path, null, val);
        }

        /*
         * Return as T here, for easily chaining to other usage,
         * if thed deposit failed, an error will be trigger all
         * the time (also the default value will be returned)
         */
        public T DepositValue<T>(string path, Pass pass, T val) {
            Var<T> v = GetVar<T>(path);
            if (v == null) {
                v = AddVar<T>(path, pass, val);
                return v.Value;
            } else {
                Error("Already Exist: {0} {1} -> {2}", path, v.Value, val);
                return default(T);
            }
        }

        public T DepositValue<T>(string path, T val) {
            return DepositValue<T>(path, null, val);
        }

        public T WithdrawValue<T>(string path, Pass pass, T defaultValue) {
            if (HasVar<T>(path)) {
                T result = GetValue<T>(path, defaultValue);
                RemoveVar<T>(path, pass);
                return result;
            } else {
                Error("Not Exist: {0}", path);
                return defaultValue;
            }
        }

        public T WithdrawValue<T>(string path, T defaultValue) {
            return WithdrawValue<T>(path, null, defaultValue);
        }

        public T WithdrawValue<T>(string path, Pass pass) {
            return WithdrawValue<T>(path, pass, default(T));
        }

        public T WithdrawValue<T>(string path) {
            return WithdrawValue<T>(path, null, default(T));
        }
    }
}
