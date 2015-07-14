using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public class Vars : SecurableEntityAspect {
        public Var<T> AddVar<T>(string path, T val) {
            return AddVar<T>(path, null, val);
        }

        public Var<T> AddVar<T>(string path, Object pass, T val) {
            Var<T> result = Add<Var<T>>(path);
            if (result != null && !result.Setup(pass, val)) {
                Remove<Var<T>>(path);
                result = null;
            }
            return result;
        }

        public Var<T> RemoveVar<T>(string path) {
            return Remove<Var<T>>(path);
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

        public bool SetValue<T>(string path, T val) {
            return SetValue<T>(path, null, val);
        }

        public bool SetValue<T>(string path, Object pass, T val) {
            Var<T> v = GetVar<T>(path);
            if (v != null) {
                return v.SetValue(pass, val);
            }
            return false;
        }
    }
}
