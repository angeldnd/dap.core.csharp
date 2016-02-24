using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public sealed class Var<T> : Var<Vars, T> {
        public Var(Vars owner, string key) : base(owner, key) {
        }
    }

    public abstract class Var<TO, T> : InBothAspect<TO>, IVar<T>
                                            where TO : class, IVars {
        private T _Value;
        public T Value {
            get { return _Value; }
        }

        public virtual object GetValue() {
            return _Value;
        }

        public Var(TO owner, string key) : base(owner, key) {
        }

        public Var(TO owner, int index) : base(owner, index) {
        }

        private bool _Setup = false;

        public virtual bool Setup(T defaultValue) {
            if (!_Setup) {
                _Setup = true;
                UpdateValue(defaultValue);
                return true;
            } else {
                Error("Already Setup: {0} -> {1}", _Value, defaultValue);
                return false;
            }
        }

        protected virtual T GetDefaultValue() {
            return default(T);
        }

        protected virtual bool NeedUpdate(T newValue) {
            return !_Setup;
        }

        private void UpdateValue(T newValue) {
            _Value = newValue;
            AdvanceRevision();

            WeakListHelper.Notify(_VarWatchers, (IVarWatcher watcher) => {
                watcher.OnChanged(this);
            });
        }

        public bool CheckNewValue(T newValue) {
            return WeakListHelper.IsValid(_ValueCheckers, (IValueChecker<T> checker) => {
                if (!checker.IsValid(this, newValue)) {
                    if (LogDebug) {
                        Debug("Check Not Passed: {0} -> {1} => {2}",
                                Value, newValue, checker);
                    }
                    return false;
                }
                return true;
            });
        }

        public virtual bool SetValue(T newValue) {
            if (NeedUpdate(newValue)) {
                if (!CheckNewValue(newValue)) {
                    return false;
                }
                T lastVal = Value;

                if (!_Setup) Setup(GetDefaultValue());
                UpdateValue(newValue);

                WeakListHelper.Notify(_ValueWatchers, (IValueWatcher<T> watcher) => {
                    watcher.OnChanged(this, lastVal);
                });
            }
            return true;
        }

        public BlockVarWatcher AddVarWatcher(IBlockOwner owner, Action<IVar> block) {
            BlockVarWatcher result = new BlockVarWatcher(owner, block);
            if (AddVarWatcher(result)) {
                return result;
            }
            return null;
        }

        public BlockValueChecker<T> AddValueChecker(IBlockOwner owner, Func<IVar<T>, T, bool> block) {
            BlockValueChecker<T> result = new BlockValueChecker<T>(owner, block);
            if (AddValueChecker(result)) {
                return result;
            }
            return null;
        }

        public BlockValueWatcher<T> AddValueWatcher(IBlockOwner owner, Action<IVar<T>, T> block) {
            BlockValueWatcher<T> result = new BlockValueWatcher<T>(owner, block);
            if (AddValueWatcher(result)) {
                return result;
            }
            return null;
        }

        public void AllValueCheckers<T1>(Action<T1> callback) where T1 : IValueChecker {
            WeakListHelper.ForEach(_ValueCheckers, (IValueChecker<T> checker) => {
                if (checker is T1) {
                    callback((T1)checker);
                }
            });
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

        //SILP: DECLARE_LIST(ValueChecker, checker, IValueChecker<T>, _ValueCheckers)
        private WeakList<IValueChecker<T>> _ValueCheckers = null;     //__SILP__
                                                                      //__SILP__
        public int ValueCheckerCount {                                //__SILP__
            get { return WeakListHelper.Count(_ValueCheckers); }      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool AddValueChecker(IValueChecker<T> checker) {       //__SILP__
            return WeakListHelper.Add(ref _ValueCheckers, checker);   //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool RemoveValueChecker(IValueChecker<T> checker) {    //__SILP__
            return WeakListHelper.Remove(_ValueCheckers, checker);    //__SILP__
        }                                                             //__SILP__

        //SILP: DECLARE_LIST(ValueWatcher, watcher, IValueWatcher<T>, _ValueWatchers)
        private WeakList<IValueWatcher<T>> _ValueWatchers = null;     //__SILP__
                                                                      //__SILP__
        public int ValueWatcherCount {                                //__SILP__
            get { return WeakListHelper.Count(_ValueWatchers); }      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool AddValueWatcher(IValueWatcher<T> watcher) {       //__SILP__
            return WeakListHelper.Add(ref _ValueWatchers, watcher);   //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool RemoveValueWatcher(IValueWatcher<T> watcher) {    //__SILP__
            return WeakListHelper.Remove(_ValueWatchers, watcher);    //__SILP__
        }                                                             //__SILP__
    }
}
