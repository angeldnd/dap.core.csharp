using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IVars : IAspect, IOwner {
    }

    public sealed class Vars : DictAspect<IContext, IVar>, IVars {
        public Vars(IContext owner, string key) : base(owner, key) {
        }

        public bool WaitVar(string key, Action<IVar, bool> callback, bool waitSetup = true) {
            return Owner.Utils.WaitSetupAspect(this, key, callback, waitSetup);
        }

        public bool WaitAndWatchVar(string key, IVarWatcher watcher) {
            return Owner.Utils.WaitSetupAspect(this, key, (IVar v, bool isNew) => {
                v.AddVarWatcher(watcher);
                if (!isNew) {
                    watcher.OnChanged(v);
                }
            });
        }

        public bool WaitAndWatchVar(string key, IBlockOwner owner, Action<IVar> block) {
            return WaitAndWatchVar(key, new BlockVarWatcher(owner, block));
        }

        public bool WaitVar<T>(string key, Action<IVar<T>, bool> callback, bool waitSetup = true) {
            return Owner.Utils.WaitSetupAspect(this, key, callback, waitSetup);
        }

        public bool WaitAndWatchVar<T>(string key, IValueWatcher<T> watcher) {
            return Owner.Utils.WaitSetupAspect(this, key, (IVar<T> v, bool isNew) => {
                v.AddValueWatcher(watcher);
                if (!isNew) {
                    watcher.OnChanged(v, v.Value);
                }
            });
        }

        public bool WaitAndWatchVar<T>(string key, IBlockOwner owner, Action<IVar<T>, T> block) {
            return WaitAndWatchVar<T>(key, new BlockValueWatcher<T>(owner, block));
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

        private T _GetValue<T>(string key, T defaultValue, bool isDebug) {
            Var<T> v = GetVar<T>(key, isDebug);
            if (v != null) {
                return v.Value;
            }
            return defaultValue;
        }

        public T GetValue<T>(string key, T defaultValue) {
            return _GetValue<T>(key, defaultValue, true);
        }

        public T GetValue<T>(string key) {
            return _GetValue<T>(key, default(T), false);
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

        private T _GetWeakValue<T>(string key, T defaultValue, bool isDebug) where T : class {
            WeakVar<T> v = Get<WeakVar<T>>(key, isDebug);
            if (v != null) {
                return v.Target;
            }
            return defaultValue;
        }

        public T GetWeakValue<T>(string key, T defaultValue) where T : class {
            return _GetWeakValue<T>(key, defaultValue, true);
        }

        public T GetWeakValue<T>(string key) where T : class {
            return _GetWeakValue<T>(key, default(T), false);
        }

        public bool SetWeakValue<T>(string key, T val) where T : class {
            WeakVar<T> v = Get<WeakVar<T>>(key, true);
            if (v == null) {
                v = Add<WeakVar<T>>(key);
            }
            if (v != null) {
                return v.SetTarget(val);
            }
            return false;
        }
    }
}
