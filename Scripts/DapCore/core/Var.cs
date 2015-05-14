using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface VarWatcher {
        void OnVarChanged(Var v);
    }

    public interface Var : Aspect {
        Object GetValue();
        bool AddVarWatcher(VarWatcher watcher);
        bool RemoveVarWatcher(VarWatcher watcher);
    }

    public class Var<T> : BaseAspect, Var {
        private T _Value;
        public T Value {
            get { return _Value; }
        }

        private bool _Setup = false;
        private Object _Pass = null;

        public virtual bool Setup(Object pass, T defaultValue) {
            if (!_Setup) {
                _Pass = pass;
                _Value = defaultValue;
                _Setup = true;
                return true;
            } else {
                Error("Already Setup: {0} -> {1}", _Value, defaultValue);
                return false;
            }
        }

        public virtual bool Setup(T defaultValue) {
            return Setup(null, defaultValue);
        }

        //SILP: DECLARE_LIST(VarWatcher, watcher, VarWatcher, _VarWatchers)
        protected List<VarWatcher> _VarWatchers = null;                      //__SILP__
                                                                             //__SILP__
        public bool AddVarWatcher(VarWatcher watcher) {                      //__SILP__
            if (_VarWatchers == null) _VarWatchers = new List<VarWatcher>(); //__SILP__
            if (!_VarWatchers.Contains(watcher)) {                           //__SILP__
                _VarWatchers.Add(watcher);                                   //__SILP__
                return true;                                                 //__SILP__
            }                                                                //__SILP__
            return false;                                                    //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        public bool RemoveVarWatcher(VarWatcher watcher) {                   //__SILP__
            if (_VarWatchers != null && _VarWatchers.Contains(watcher)) {    //__SILP__
                _VarWatchers.Remove(watcher);                                //__SILP__
                return true;                                                 //__SILP__
            }                                                                //__SILP__
            return false;                                                    //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        public bool SetValue(Object pass, T newValue) {
            if (!_Setup) {
                Error("Not Setup: {0} -> {1}", _Value, newValue);
            } else if (_Pass != pass) {
                Error("Access Denied: _Pass = {0}, pass = {1}: {2} -> {3}", _Pass, pass, _Value, newValue);
            } else {
                _Value = newValue;

                if (_VarWatchers != null) {
                    for (int i = 0; i < _VarWatchers.Count; i++) {
                        _VarWatchers[i].OnVarChanged(this);
                    }
                }
                return true;
            }
            return false;
        }

        public virtual bool SetValue(T newValue) {
            return SetValue(null, newValue);
        }

        public virtual Object GetValue() {
            return _Value;
        }

        protected override bool DoEncode(Data data) {
            return false;
        }

        protected override bool DoDecode(Data data) {
            return false;
        }
    }
}
