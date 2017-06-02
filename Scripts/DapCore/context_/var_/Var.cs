using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public sealed class Var<T> : Var<Vars, T> {
        public Var(Vars owner, string key) : base(owner, key) {
        }
    }

    public abstract class Var<TO, T> : InBothAspect<TO>, IVar<T>
                                            where TO : class, IVars {
        public Type ValueType {
            get { return typeof(T); }
        }

        private T _Value = default(T);
        public T Value {
            get { return _Value; }
        }

        public virtual object GetValue() {
            return _Value;
        }

        public virtual bool SetValue(object _newValue) {
            T newValue = _newValue.As<T>();
            if (newValue != null) {
                return SetValue(newValue);
            }
            return false;
        }

        private int _CheckFailedCount = 0;

        public Var(TO owner, string key) : base(owner, key) {
        }

        public Var(TO owner, int index) : base(owner, index) {
        }

        private bool _Setup = false;

        public bool NeedSetup() {
            return !_Setup;
        }

        public bool Setup(T defaultValue) {
            if (!_Setup) {
                _Setup = true;
                //Not checking whether need to update here, so access code can either
                //get the current value in setup triggers, or watcher all changes.
                UpdateValue(defaultValue, () => {
                    OnSetup();
                    WeakListHelper.Notify(_SetupWatchers, (ISetupWatcher watcher) => {
                        watcher.OnSetup(this);
                    });
                    if (LogDebug) {
                        Debug("Setup: {0}", defaultValue);
                    }
                });
                return true;
            } else {
                Error("Already Setup: {0} -> {1}", _Value, defaultValue);
                return false;
            }
        }

        protected virtual void OnSetup() {}

        protected virtual bool NeedUpdate(T newValue) {
            if (NeedSetup()) return true;

            if (Object.ReferenceEquals(Value, newValue)) return false;

            if (Value == null) {
                return !newValue.Equals(Value);
            } else {
                return !Value.Equals(newValue);
            }
        }

        private void UpdateValue(T newValue, Action callback = null) {
            _UpdateValue_LastVal = Value;

            _Value = newValue;
            AdvanceRevision();

            if (callback != null) {
                callback();
            }

            WeakListHelper.Notify(_VarWatchers, UpdateValue_VarWatcher);
            WeakListHelper.Notify(_ValueWatchers, UpdateValue_ValueWatcher);
        }

        private T _UpdateValue_LastVal = default(T);

        private Action<IVarWatcher> _UpdateValue_VarWatcher = null;
        private Action<IVarWatcher> UpdateValue_VarWatcher {
            get {
                if (_UpdateValue_VarWatcher == null) {
                    _UpdateValue_VarWatcher = new Action<IVarWatcher>((IVarWatcher watcher) => {
                        #if UNITY_EDITOR
                        UnityEngine.Profiling.Profiler.BeginSample("Var.OnChanged: " + Key + " " + watcher.ToString());
                        #endif
                        watcher.OnChanged(this);
                        #if UNITY_EDITOR
                        UnityEngine.Profiling.Profiler.EndSample();
                        #endif
                    });
                }
                return _UpdateValue_VarWatcher;
            }
        }

        private Action<IValueWatcher<T>> _UpdateValue_ValueWatcher = null;
        private Action<IValueWatcher<T>> UpdateValue_ValueWatcher {
            get {
                if (_UpdateValue_ValueWatcher == null) {
                    _UpdateValue_ValueWatcher = new Action<IValueWatcher<T>>((IValueWatcher<T> watcher) => {
                        #if UNITY_EDITOR
                        UnityEngine.Profiling.Profiler.BeginSample("Var.OnChanged<T>: " + Key + " " + watcher.ToString());
                        #endif
                        watcher.OnChanged(this, _UpdateValue_LastVal);
                        #if UNITY_EDITOR
                        UnityEngine.Profiling.Profiler.EndSample();
                        #endif
                    });
                }
                return _UpdateValue_ValueWatcher;
            }
        }

        public bool CheckNewValue(T newValue) {
            return WeakListHelper.IsValid(_ValueCheckers, (IValueChecker<T> checker) => {
                if (!checker.IsValid(this, newValue)) {
                    if (LogDebug) {
                        Debug("Invalid Value: {0} => {1} -> {2}",
                                checker, Value, newValue);
                    }
                    return false;
                }
                return true;
            });
        }

        public bool SetValue(T newValue) {
            if (NeedUpdate(newValue)) {
                if (!CheckNewValue(newValue)) {
                    _CheckFailedCount++;
                    return false;
                }
                T lastVal = Value;
                if (!_Setup) {
                    Setup(newValue);
                } else {
                    UpdateValue(newValue);
                }
                if (LogDebug) {
                    Debug("SetValue: {0} -> {1}", lastVal, newValue);
                }
            }
            return true;
        }

        public BlockSetupWatcher AddSetupWatcher(IBlockOwner owner, Action<ISetupAspect> block) {
            BlockSetupWatcher result = new BlockSetupWatcher(owner, block);
            if (AddSetupWatcher(result)) {
                return result;
            }
            return null;
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

        protected override void AddSummaryFields(Data summary) {
            base.AddSummaryFields(summary);
            summary.S(ContextConsts.SummaryValueType, ValueType.FullName)
                   .S(ContextConsts.SummaryValue, string.Format("{0}", _Value))
                   .I(ContextConsts.SummaryCheckerCount, ValueCheckerCount)
                   .I(ContextConsts.SummaryWatcherCount, ValueWatcherCount)
                   .I(ContextConsts.Summary2ndWatcherCount, VarWatcherCount)
                   .I(ContextConsts.SummaryCheckFailedCount, _CheckFailedCount);
        }

        //SILP: DECLARE_LIST(SetupWatcher, watcher, ISetupWatcher, _SetupWatchers)
        private WeakList<ISetupWatcher> _SetupWatchers = null;        //__SILP__
                                                                      //__SILP__
        public int SetupWatcherCount {                                //__SILP__
            get { return WeakListHelper.Count(_SetupWatchers); }      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool AddSetupWatcher(ISetupWatcher watcher) {          //__SILP__
            return WeakListHelper.Add(ref _SetupWatchers, watcher);   //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool RemoveSetupWatcher(ISetupWatcher watcher) {       //__SILP__
            return WeakListHelper.Remove(_SetupWatchers, watcher);    //__SILP__
        }                                                             //__SILP__

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
