using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface VarWatcher {
        void OnVarChanged(Var v);
    }

    public interface Var : SecurableAspect {
        object GetValue();
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
        private WeakList<VarWatcher> _VarWatchers = null;             //__SILP__
                                                                      //__SILP__
        public int VarWatcherCount {                                  //__SILP__
            get { return WeakListHelper.Count(_VarWatchers); }        //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool AddVarWatcher(VarWatcher watcher) {               //__SILP__
            return WeakListHelper.Add(ref _VarWatchers, watcher);     //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool RemoveVarWatcher(VarWatcher watcher) {            //__SILP__
            return WeakListHelper.Remove(_VarWatchers, watcher);      //__SILP__
        }                                                             //__SILP__
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

        public virtual object GetValue() {
            return _Value;
        }
    }
}
