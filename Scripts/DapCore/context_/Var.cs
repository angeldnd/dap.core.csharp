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

    public class Var<T> : BaseSecurableAspect, Var {
        private T _Value;
        public T Value {
            get { return _Value; }
        }

        private bool _Setup = false;

        public virtual bool Setup(Pass pass, T defaultValue) {
            if (!_Setup) {
                if (!SetPass(pass)) return false;
                _Setup = true;
                _Value = defaultValue;
                AdvanceRevision();
                return true;
            } else {
                Error("Already Setup: {0} -> {1}", _Value, defaultValue);
                return false;
            }
        }

        public virtual bool Setup(T defaultValue) {
            return Setup(Pass, defaultValue);
        }

        public virtual bool Setup() {
            return Setup(Pass, default(T));
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
        public virtual bool SetValue(Pass pass, T newValue) {
            if (!_Setup) Setup();
            if (!CheckWritePass(pass)) return false;

            _Value = newValue;
            AdvanceRevision();

            if (_VarWatchers != null) {
                for (int i = 0; i < _VarWatchers.Count; i++) {
                    _VarWatchers[i].OnVarChanged(this);
                }
            }
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
