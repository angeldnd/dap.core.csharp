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
                IProfiler profiler = Log.BeginSample(Key);
                _Setup = true;
                //Not checking whether need to update here, so access code can either
                //get the current value in setup triggers, or watcher all changes.
                UpdateValue(defaultValue, () => {
                    OnSetup();
                    NotifySetupWatchers(profiler);
                    if (LogDebug) {
                        Debug("Setup: {0}", defaultValue);
                    }
                });
                if (profiler != null) profiler.EndSample();
                return true;
            } else {
                Error("Already Setup: {0} -> {1}", _Value, defaultValue);
                return false;
            }
        }

        private void NotifySetupWatchers(IProfiler profiler) {
            //SILP: WEAK_LIST_FOREACH_BEGIN(Var.OnSetup, watcher, ISetupWatcher, _SetupWatchers)
            if (_SetupWatchers != null) {                                               //__SILP__
                if (profiler != null) profiler.BeginSample("Var.OnSetup");              //__SILP__
                bool needGc = false;                                                    //__SILP__
                foreach (var r in _SetupWatchers.RetainLock()) {                        //__SILP__
                    ISetupWatcher watcher = _SetupWatchers.GetTarget(r);                //__SILP__
                    if (watcher == null) {                                              //__SILP__
                        needGc = true;                                                  //__SILP__
                    } else {                                                            //__SILP__
                        if (profiler != null) profiler.BeginSample(watcher.BlockName);  //__SILP__
                        watcher.OnSetup(this);
            //SILP: WEAK_LIST_FOREACH_END(Var.OnSetup, watcher, ISetupWatcher, _SetupWatchers)
                        if (profiler != null) profiler.EndSample();   //__SILP__
                    }                                                 //__SILP__
                }                                                     //__SILP__
                _SetupWatchers.ReleaseLock(needGc);                   //__SILP__
                if (profiler != null) profiler.EndSample();           //__SILP__
            }                                                         //__SILP__
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
            IProfiler profiler = Log.BeginSample(Key);
            T lastVal = _Value;

            _Value = newValue;
            AdvanceRevision();

            if (callback != null) {
                callback();
            }

            NotifyVarWatchers(profiler);
            NotifyValueWatchers(lastVal, profiler);
            if (profiler != null) profiler.EndSample();
        }

        private void NotifyVarWatchers(IProfiler profiler) {
            //SILP: WEAK_LIST_FOREACH_BEGIN(Var.OnChanged, watcher, IVarWatcher, _VarWatchers)
            if (_VarWatchers != null) {                                                 //__SILP__
                if (profiler != null) profiler.BeginSample("Var.OnChanged");            //__SILP__
                bool needGc = false;                                                    //__SILP__
                foreach (var r in _VarWatchers.RetainLock()) {                          //__SILP__
                    IVarWatcher watcher = _VarWatchers.GetTarget(r);                    //__SILP__
                    if (watcher == null) {                                              //__SILP__
                        needGc = true;                                                  //__SILP__
                    } else {                                                            //__SILP__
                        if (profiler != null) profiler.BeginSample(watcher.BlockName);  //__SILP__
                        watcher.OnChanged(this);
            //SILP: WEAK_LIST_FOREACH_END(Var.IsValid, watcher, IVarWatcher, _VarWatchers)
                        if (profiler != null) profiler.EndSample();   //__SILP__
                    }                                                 //__SILP__
                }                                                     //__SILP__
                _VarWatchers.ReleaseLock(needGc);                     //__SILP__
                if (profiler != null) profiler.EndSample();           //__SILP__
            }                                                         //__SILP__
        }

        private void NotifyValueWatchers(T lastVal, IProfiler profiler) {
            //SILP: WEAK_LIST_FOREACH_BEGIN(Var.OnChanged<T>, watcher, IValueWatcher<T>, _ValueWatchers)
            if (_ValueWatchers != null) {                                               //__SILP__
                if (profiler != null) profiler.BeginSample("Var.OnChanged<T>");         //__SILP__
                bool needGc = false;                                                    //__SILP__
                foreach (var r in _ValueWatchers.RetainLock()) {                        //__SILP__
                    IValueWatcher<T> watcher = _ValueWatchers.GetTarget(r);             //__SILP__
                    if (watcher == null) {                                              //__SILP__
                        needGc = true;                                                  //__SILP__
                    } else {                                                            //__SILP__
                        if (profiler != null) profiler.BeginSample(watcher.BlockName);  //__SILP__
                        watcher.OnChanged(this, lastVal);
            //SILP: WEAK_LIST_FOREACH_END(Var.IsValid<T>, watcher, IValueWatcher<T>, _ValueWatchers)
                        if (profiler != null) profiler.EndSample();   //__SILP__
                    }                                                 //__SILP__
                }                                                     //__SILP__
                _ValueWatchers.ReleaseLock(needGc);                   //__SILP__
                if (profiler != null) profiler.EndSample();           //__SILP__
            }                                                         //__SILP__
        }

        private bool IsValid(T newValue, IProfiler profiler) {
            bool result = true;
            //SILP: WEAK_LIST_FOREACH_BEGIN(Var.IsValid, checker, IValueChecker<T>, _ValueCheckers)
            if (_ValueCheckers != null) {                                               //__SILP__
                if (profiler != null) profiler.BeginSample("Var.IsValid");              //__SILP__
                bool needGc = false;                                                    //__SILP__
                foreach (var r in _ValueCheckers.RetainLock()) {                        //__SILP__
                    IValueChecker<T> checker = _ValueCheckers.GetTarget(r);             //__SILP__
                    if (checker == null) {                                              //__SILP__
                        needGc = true;                                                  //__SILP__
                    } else {                                                            //__SILP__
                        if (profiler != null) profiler.BeginSample(checker.BlockName);  //__SILP__
                        if (!checker.IsValid(this, newValue)) {
                            if (LogDebug) {
                                Debug("Invalid Value: {0} => {1} -> {2}",
                                        checker, _Value, newValue);
                            }
                            result = false;
                            if (profiler != null) profiler.EndSample();
                            break;
                        }
            //SILP: WEAK_LIST_FOREACH_END(Var.IsValid, checker, IValueChecker<T>, _ValueCheckers)
                        if (profiler != null) profiler.EndSample();   //__SILP__
                    }                                                 //__SILP__
                }                                                     //__SILP__
                _ValueCheckers.ReleaseLock(needGc);                   //__SILP__
                if (profiler != null) profiler.EndSample();           //__SILP__
            }                                                         //__SILP__
            return result;
        }

        public bool SetValue(T newValue) {
            if (NeedUpdate(newValue)) {
                IProfiler profiler = Log.BeginSample(Key);
                if (!IsValid(newValue, profiler)) {
                    _CheckFailedCount++;
                    if (profiler != null) profiler.EndSample();
                    return false;
                }
                if (profiler != null) profiler.EndSample();
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
            Convertor<T> convertor = Convertor.GetConvertor<T>();
            string val = convertor != null ? convertor.Convert(_Value) : _Value.ToString();
            summary.S(ContextConsts.SummaryValueType, ValueType.FullName)
                   .S(ContextConsts.SummaryValue, val)
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
