using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IVar : IInTreeAspect, IInTableAspect {
        object GetValue();

        int VarWatcherCount { get; }
        bool AddVarWatcher(IVarWatcher watcher);
        bool RemoveVarWatcher(IVarWatcher watcher);

        int ValueCheckerCount { get; }
        int ValueWatcherCount { get; }
    }

    public interface IVar<T> : IVar {
        T Value { get; }
        bool SetValue(Pass pass, T newValue);
        bool SetValue(T newValue);

        bool Setup(Pass pass, T defaultValue);

        bool AddValueChecker(IValueChecker<T> checker);
        bool RemoveValueChecker(IValueChecker<T> checker);

        void AllValueCheckers<T1>(Action<T1> callback) where T1 : IValueChecker<T>;
        void AllValueCheckers(Action<IValueChecker<T>> callback);

        bool AddValueWatcher(IValueWatcher<T> watcher);
        bool RemoveValueWatcher(IValueWatcher<T> watcher);
    }

    public sealed class Var<T> : Var<Vars, T> {
        public Var(Vars owner, string path, Pass pass) : base(owner, path, pass) {
        }
    }

    public abstract class Var<TO, T> : InBothAspect<TO>, IVar<T>
                                            where TO : IVars {
        private T _Value;
        public T Value {
            get { return _Value; }
        }

        public virtual object GetValue() {
            return _Value;
        }

        public Var(TO owner, string path, Pass pass) : base(owner, path, pass) {
        }

        public Var(TO owner, int index, Pass pass) : base(owner, index, pass) {
        }

        private bool _Setup = false;

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

        public virtual bool SetValue(Pass pass, T newValue) {
            if (!CheckWritePass(pass)) return false;

            if (NeedUpdate(newValue)) {
                if (!CheckNewValue(newValue)) {
                    return false;
                }
                T lastVal = Value;

                if (!_Setup) Setup(pass, GetDefaultValue());
                UpdateValue(newValue);

                WeakListHelper.Notify(_ValueWatchers, (IValueWatcher<T> watcher) => {
                    watcher.OnChanged(this, lastVal);
                });
                return true;
            } else {
                return true;
            }
        }

        public bool SetValue(T newValue) {
            return SetValue(null, newValue);
        }

        public BlockVarWatcher AddBlockVarWatcher(IBlockOwner owner,
                                                            Action<IVar> _watcher) {
            BlockVarWatcher watcher = new BlockVarWatcher(owner, _watcher);
            if (AddVarWatcher(watcher)) {
                return watcher;
            }
            return null;
        }

        public BlockValueChecker<T> AddBlockValueChecker(Pass pass, IBlockOwner owner,
                                                            Func<IVar<T>, T, bool> _checker) {
            if (!CheckAdminPass(pass)) return null;

            BlockValueChecker<T> checker = new BlockValueChecker<T>(owner, _checker);
            if (AddValueChecker(pass, checker)) {
                return checker;
            }
            return null;
        }

        public BlockValueChecker<T> AddBlockValueChecker(IBlockOwner owner,
                                                            Func<IVar<T>, T, bool> checker) {
            return AddBlockValueChecker(null, checker);
        }

        public BlockValueWatcher<T> AddBlockValueWatcher(IBlockOwner owner,
                                                            Action<IVar<T>, T> _watcher) {
            BlockValueWatcher<T> watcher = new BlockValueWatcher<T>(owner, _watcher);
            if (AddValueWatcher(watcher)) {
                return watcher;
            }
            return null;
        }

        public void AllValueCheckers<T1>(Action<T1> callback) where T1 : IValueChecker<T> {
            WeakListHelper.ForEach(_ValueCheckers, (IValueChecker<T> checker) => {
                if (checker is T1) {
                    callback((T1)checker);
                }
            });
        }

        public void AllValueCheckers(Action<IValueChecker<T>> callback) {
            WeakListHelper.ForEach(_ValueCheckers, callback);
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

        //SILP: DECLARE_SECURE_LIST(ValueChecker, checker, IValueChecker<T>, _ValueCheckers)
        private WeakList<IValueChecker<T>> _ValueCheckers = null;              //__SILP__
                                                                               //__SILP__
        public int ValueCheckerCount {                                         //__SILP__
            get { return WeakListHelper.Count(_ValueCheckers); }               //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public bool AddValueChecker(Pass pass, IValueChecker<T> checker) {     //__SILP__
            if (!CheckAdminPass(pass)) return false;                           //__SILP__
            return WeakListHelper.Add(ref _ValueCheckers, checker);            //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public bool AddValueChecker(IValueChecker<T> checker) {                //__SILP__
            return AddValueChecker(null, checker);                             //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public bool RemoveValueChecker(Pass pass, IValueChecker<T> checker) {  //__SILP__
            if (!CheckAdminPass(pass)) return false;                           //__SILP__
            return WeakListHelper.Remove(_ValueCheckers, checker);             //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public bool RemoveValueChecker(IValueChecker<T> checker) {             //__SILP__
            return RemoveValueChecker(null, checker);                          //__SILP__
        }                                                                      //__SILP__

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
