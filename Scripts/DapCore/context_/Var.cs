using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface VarWatcher {
        void OnVarChanged(Var v);
    }

    public interface Var : SecurableAspect {
        Object GetValue();
        int VarWatcherCount { get; }
        bool AddVarWatcher(VarWatcher watcher);
        bool RemoveVarWatcher(VarWatcher watcher);
    }

    public sealed class BlockVarWatcher : VarWatcher {
        public delegate void WatcherBlock(Var v);

        private readonly WatcherBlock _Block;

        public BlockVarWatcher(WatcherBlock block) {
            _Block = block;
        }

        public void OnVarChanged(Var v) {
            _Block(v);
        }
    }

    public class Var<T> : BaseSecurableAspect, Var {
        public delegate T GetterBlock();

        private T _Value;
        public T Value {
            get { return _Value; }
        }

        private bool _Setup = false;
        protected bool NeedSetup {
            get { return !_Setup; }
        }

        public bool Setup(Pass pass, T defaultValue) {
            if (!CheckAdminPass(pass)) return false;
            if (!_Setup) {
                _Setup = true;
                UpdateValue(defaultValue);
                return true;
            } else {
                Error("Already Setup: {0} -> {1}", _Value, defaultValue);
                return false;
            }
        }

        public bool Setup(T defaultValue) {
            return Setup(null, defaultValue);
        }

        protected virtual T GetDefaultValue() {
            return default(T);
        }

        //SILP: DECLARE_LIST(VarWatcher, watcher, VarWatcher, _VarWatchers)
        protected List<VarWatcher> _VarWatchers = null;                       //__SILP__
                                                                              //__SILP__
        public int VarWatcherCount {                                          //__SILP__
            get {                                                             //__SILP__
                if (_VarWatchers == null) {                                   //__SILP__
                    return 0;                                                 //__SILP__
                }                                                             //__SILP__
                return _VarWatchers.Count;                                    //__SILP__
            }                                                                 //__SILP__
        }                                                                     //__SILP__
                                                                              //__SILP__
        public virtual bool AddVarWatcher(VarWatcher watcher) {               //__SILP__
            if (_VarWatchers == null) _VarWatchers = new List<VarWatcher>();  //__SILP__
            if (!_VarWatchers.Contains(watcher)) {                            //__SILP__
                _VarWatchers.Add(watcher);                                    //__SILP__
                return true;                                                  //__SILP__
            }                                                                 //__SILP__
            return false;                                                     //__SILP__
        }                                                                     //__SILP__
                                                                              //__SILP__
        public virtual bool RemoveVarWatcher(VarWatcher watcher) {            //__SILP__
            if (_VarWatchers != null && _VarWatchers.Contains(watcher)) {     //__SILP__
                _VarWatchers.Remove(watcher);                                 //__SILP__
                return true;                                                  //__SILP__
            }                                                                 //__SILP__
            return false;                                                     //__SILP__
        }                                                                     //__SILP__
                                                                              //__SILP__

        private void UpdateValue(T newValue) {
            _Value = newValue;
            AdvanceRevision();

            if (_VarWatchers != null) {
                for (int i = 0; i < _VarWatchers.Count; i++) {
                    _VarWatchers[i].OnVarChanged(this);
                }
            }
        }

        public virtual bool SetValue(Pass pass, T newValue) {
            if (!CheckWritePass(pass)) return false;
            if (!_Setup) Setup(pass, GetDefaultValue());

            UpdateValue(newValue);
            return true;
        }

        public bool SetValue(T newValue) {
            return SetValue(null, newValue);
        }

        public virtual Object GetValue() {
            return _Value;
        }
    }
}
