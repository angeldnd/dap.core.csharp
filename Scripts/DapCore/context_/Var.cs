using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IVarWatcher {
        void OnVarChanged(IVar v);
    }

    public interface IVar : IAspect<Context> {
        object GetValue();
        int VarWatcherCount { get; }
        bool AddVarWatcher(IVarWatcher watcher);
        bool RemoveVarWatcher(IVarWatcher watcher);
    }

    public sealed class BlockVarWatcher : WeakBlock, IVarWatcher {
        public delegate void WatcherBlock(Var v);

        private readonly WatcherBlock _Block;

        public BlockVarWatcher(BlockOwner owner, WatcherBlock block) : base(owner) {
            _Block = block;
        }

        public void OnVarChanged(Var v) {
            _Block(v);
        }
    }

    public class Var<T> : Var<Vars, T> {
        public Var(Context owner, string path, Pass pass) : base(owner, path, pass) {
        }
    }

    public abstract class Var<TO, T> : Aspect<Context, TO>, IVar
                                where TO : ISection<Context> {
        public delegate T GetterBlock();

        private T _Value;
        public T Value {
            get { return _Value; }
        }

        public Var(Context owner, string path, Pass pass) : base(owner, path, pass) {
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

        //SILP: DECLARE_LIST(VarWatcher, watcher, IVarWatcher, _VarWatchers)
        private WeakList<IVarWatcher> _VarWatchers = null;            //__SILP__
                                                                      //__SILP__
        public int VarWatcherCount {                                  //__SILP__
            get { return WeakListHelper.Count(_VarWatchers); }        //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool AddVarWatcher(IVarWatcher watcher) {              //__SILP__
            return WeakListHelper.Add(ref _VarWatchers, watcher);     //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool RemoveVarWatcher(IVarWatcher watcher) {           //__SILP__
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
