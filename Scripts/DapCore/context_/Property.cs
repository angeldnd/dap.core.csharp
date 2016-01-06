using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface ValueChecker {
    }

    public interface ValueChecker<T> : ValueChecker {
        bool IsValid(string path, T val, T newVal);
    }

    public interface ValueWatcher {
    }

    public interface ValueWatcher<T> : ValueWatcher {
        void OnChanged(string path, T lastVal, T val);
    }

    public delegate void OnValueChecker<T1>(T1 checker) where T1 : ValueChecker;

    public interface Property : Var {
        Data Encode();
        bool Decode(Data data);
        bool Decode(Pass pass, Data data);

        int ValueCheckerCount { get; }
        int ValueWatcherCount { get; }

        void AllValueCheckers<T1>(OnValueChecker<T1> callback) where T1 : ValueChecker;
    }

    public abstract class Property<T>: Var<T>, Property {
        public Data Encode() {
            if (!string.IsNullOrEmpty(Type)) {
                Data data = new Data();
                if (data.SetString(DapObjectConsts.KeyType, Type)) {
                    if (DoEncode(data)) {
                        return data;
                    }
                }
            }
            if (LogDebug) Debug("Not Encodable!");
            return null;
        }

        public bool Decode(Data data) {
            return Decode(null, data);
        }

        public bool Decode(Pass pass, Data data) {
            if (!CheckWritePass(pass)) return false;

            string type = data.GetString(DapObjectConsts.KeyType);
            if (type == Type) {
                return DoDecode(pass, data);
            } else {
                Error("Type Mismatched: {0}, {1}", Type, type);
            }
            return false;
        }

        protected abstract bool DoEncode(Data data);
        protected abstract bool DoDecode(Pass pass, Data data);
        protected abstract bool NeedUpdate(T newVal);

        public bool CheckNewValue(T newVal) {
            return WeakListHelper.IsValid(_Checkers, (ValueChecker<T> checker) => {
                if (!checker.IsValid(Path, Value, newVal)) {
                    if (LogDebug) {
                        Debug("Check Not Passed: {0}: {1} -> {2} => {3}",
                            Path, Value, newVal, checker);
                    }
                    return false;
                }
                return true;
            });
        }

        public override bool SetValue(Pass pass, T newVal) {
            if (!CheckWritePass(pass)) return false;

            if (NeedUpdate(newVal)) {
                if (!CheckNewValue(newVal)) {
                    return false;
                }
                T lastVal = Value;
                if (!base.SetValue(pass, newVal)) {
                    return false;
                }
                WeakListHelper.Notify(_Watchers, (ValueWatcher<T> watcher) => {
                    watcher.OnChanged(Path, lastVal, Value);
                });
                return true;
            } else {
                return true;
            }
        }

        public void AllValueCheckers<T1>(OnValueChecker<T1> callback) where T1 : ValueChecker {
            if (_Checkers == null) return;

            foreach (var checker in _Checkers) {
                if (checker is T1) {
                    callback((T1)checker);
                }
            }
        }

        //SILP: DECLARE_SECURE_LIST(ValueChecker, checker, ValueChecker<T>, _Checkers)
        private WeakList<ValueChecker<T>> _Checkers = null;                   //__SILP__
                                                                              //__SILP__
        public int ValueCheckerCount {                                        //__SILP__
            get { return WeakListHelper.Count(_Checkers); }                   //__SILP__
        }                                                                     //__SILP__
                                                                              //__SILP__
        public bool AddValueChecker(Pass pass, ValueChecker<T> checker) {     //__SILP__
            if (!CheckAdminPass(pass)) return false;                          //__SILP__
            return WeakListHelper.Add(ref _Checkers, checker);                //__SILP__
        }                                                                     //__SILP__
                                                                              //__SILP__
        public bool AddValueChecker(ValueChecker<T> checker) {                //__SILP__
            return AddValueChecker(null, checker);                            //__SILP__
        }                                                                     //__SILP__
                                                                              //__SILP__
        public bool RemoveValueChecker(Pass pass, ValueChecker<T> checker) {  //__SILP__
            if (!CheckAdminPass(pass)) return false;                          //__SILP__
            return WeakListHelper.Remove(_Checkers, checker);                 //__SILP__
        }                                                                     //__SILP__
                                                                              //__SILP__
        public bool RemoveValueChecker(ValueChecker<T> checker) {             //__SILP__
            return RemoveValueChecker(null, checker);                         //__SILP__
        }                                                                     //__SILP__
                                                                              //__SILP__
        //SILP: DECLARE_LIST(ValueWatcher, watcher, ValueWatcher<T>, _Watchers)
        private WeakList<ValueWatcher<T>> _Watchers = null;           //__SILP__
                                                                      //__SILP__
        public int ValueWatcherCount {                                //__SILP__
            get { return WeakListHelper.Count(_Watchers); }           //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool AddValueWatcher(ValueWatcher<T> watcher) {        //__SILP__
            return WeakListHelper.Add(ref _Watchers, watcher);        //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool RemoveValueWatcher(ValueWatcher<T> watcher) {     //__SILP__
            return WeakListHelper.Remove(_Watchers, watcher);         //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__

    }

    //SILP: PROPERTY_CLASS(Bool, bool)
    public sealed class BoolBlockValueChecker : WeakBlock, ValueChecker<bool> {                              //__SILP__
        public delegate bool CheckerBlock(string path, bool val, bool newVal);                               //__SILP__
                                                                                                             //__SILP__
        private readonly CheckerBlock _Block;                                                                //__SILP__
                                                                                                             //__SILP__
        public BoolBlockValueChecker(BlockOwner owner, CheckerBlock block) : base(owner) {                   //__SILP__
            _Block = block;                                                                                  //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        public bool IsValid(string path, bool val, bool newVal) {                                            //__SILP__
            return _Block(path, val, newVal);                                                                //__SILP__
        }                                                                                                    //__SILP__
    }                                                                                                        //__SILP__
                                                                                                             //__SILP__
    public sealed class BoolBlockValueWatcher : WeakBlock, ValueWatcher<bool> {                              //__SILP__
        public delegate void WatcherBlock(string path, bool val, bool newVal);                               //__SILP__
                                                                                                             //__SILP__
        private readonly WatcherBlock _Block;                                                                //__SILP__
                                                                                                             //__SILP__
        public BoolBlockValueWatcher(BlockOwner owner, WatcherBlock block) : base(owner) {                   //__SILP__
            _Block = block;                                                                                  //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        public void OnChanged(string path, bool lastVal, bool val) {                                         //__SILP__
            _Block(path, lastVal, val);                                                                      //__SILP__
        }                                                                                                    //__SILP__
    }                                                                                                        //__SILP__
                                                                                                             //__SILP__
    public class BoolProperty : Property<bool> {                                                             //__SILP__
        public override string Type {                                                                        //__SILP__
            get { return PropertiesConsts.TypeBoolProperty; }                                                //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        protected override bool DoEncode(Data data) {                                                        //__SILP__
            return data.SetBool(PropertiesConsts.KeyValue, Value);                                           //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        protected override bool DoDecode(Pass pass, Data data) {                                             //__SILP__
            return SetValue(pass, data.GetBool(PropertiesConsts.KeyValue));                                  //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        protected override bool NeedUpdate(bool newVal) {                                                    //__SILP__
            return NeedSetup || (Value != newVal);                                                           //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        public BoolBlockValueChecker AddBlockValueChecker(Pass pass, BlockOwner owner,                       //__SILP__
                                                             BoolBlockValueChecker.CheckerBlock _checker) {  //__SILP__
            if (!CheckAdminPass(pass)) return null;                                                          //__SILP__
                                                                                                             //__SILP__
            BoolBlockValueChecker checker = new BoolBlockValueChecker(owner, _checker);                      //__SILP__
            if (AddValueChecker(pass, checker)) {                                                            //__SILP__
                return checker;                                                                              //__SILP__
            }                                                                                                //__SILP__
            return null;                                                                                     //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        public BoolBlockValueChecker AddBlockValueChecker(BlockOwner owner,                                  //__SILP__
                                                             BoolBlockValueChecker.CheckerBlock checker) {   //__SILP__
            return AddBlockValueChecker(null, checker);                                                      //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        public BoolBlockValueWatcher AddBlockValueWatcher(BlockOwner owner,                                  //__SILP__
                                                             BoolBlockValueWatcher.WatcherBlock _watcher) {  //__SILP__
            BoolBlockValueWatcher watcher = new BoolBlockValueWatcher(owner, _watcher);                      //__SILP__
            if (AddValueWatcher(watcher)) {                                                                  //__SILP__
                return watcher;                                                                              //__SILP__
            }                                                                                                //__SILP__
            return null;                                                                                     //__SILP__
        }                                                                                                    //__SILP__
    }                                                                                                        //__SILP__
                                                                                                             //__SILP__
    //SILP: PROPERTY_CLASS(Int, int)
    public sealed class IntBlockValueChecker : WeakBlock, ValueChecker<int> {                               //__SILP__
        public delegate bool CheckerBlock(string path, int val, int newVal);                                //__SILP__
                                                                                                            //__SILP__
        private readonly CheckerBlock _Block;                                                               //__SILP__
                                                                                                            //__SILP__
        public IntBlockValueChecker(BlockOwner owner, CheckerBlock block) : base(owner) {                   //__SILP__
            _Block = block;                                                                                 //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public bool IsValid(string path, int val, int newVal) {                                             //__SILP__
            return _Block(path, val, newVal);                                                               //__SILP__
        }                                                                                                   //__SILP__
    }                                                                                                       //__SILP__
                                                                                                            //__SILP__
    public sealed class IntBlockValueWatcher : WeakBlock, ValueWatcher<int> {                               //__SILP__
        public delegate void WatcherBlock(string path, int val, int newVal);                                //__SILP__
                                                                                                            //__SILP__
        private readonly WatcherBlock _Block;                                                               //__SILP__
                                                                                                            //__SILP__
        public IntBlockValueWatcher(BlockOwner owner, WatcherBlock block) : base(owner) {                   //__SILP__
            _Block = block;                                                                                 //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public void OnChanged(string path, int lastVal, int val) {                                          //__SILP__
            _Block(path, lastVal, val);                                                                     //__SILP__
        }                                                                                                   //__SILP__
    }                                                                                                       //__SILP__
                                                                                                            //__SILP__
    public class IntProperty : Property<int> {                                                              //__SILP__
        public override string Type {                                                                       //__SILP__
            get { return PropertiesConsts.TypeIntProperty; }                                                //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        protected override bool DoEncode(Data data) {                                                       //__SILP__
            return data.SetInt(PropertiesConsts.KeyValue, Value);                                           //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        protected override bool DoDecode(Pass pass, Data data) {                                            //__SILP__
            return SetValue(pass, data.GetInt(PropertiesConsts.KeyValue));                                  //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        protected override bool NeedUpdate(int newVal) {                                                    //__SILP__
            return NeedSetup || (Value != newVal);                                                          //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public IntBlockValueChecker AddBlockValueChecker(Pass pass, BlockOwner owner,                       //__SILP__
                                                             IntBlockValueChecker.CheckerBlock _checker) {  //__SILP__
            if (!CheckAdminPass(pass)) return null;                                                         //__SILP__
                                                                                                            //__SILP__
            IntBlockValueChecker checker = new IntBlockValueChecker(owner, _checker);                       //__SILP__
            if (AddValueChecker(pass, checker)) {                                                           //__SILP__
                return checker;                                                                             //__SILP__
            }                                                                                               //__SILP__
            return null;                                                                                    //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public IntBlockValueChecker AddBlockValueChecker(BlockOwner owner,                                  //__SILP__
                                                             IntBlockValueChecker.CheckerBlock checker) {   //__SILP__
            return AddBlockValueChecker(null, checker);                                                     //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public IntBlockValueWatcher AddBlockValueWatcher(BlockOwner owner,                                  //__SILP__
                                                             IntBlockValueWatcher.WatcherBlock _watcher) {  //__SILP__
            IntBlockValueWatcher watcher = new IntBlockValueWatcher(owner, _watcher);                       //__SILP__
            if (AddValueWatcher(watcher)) {                                                                 //__SILP__
                return watcher;                                                                             //__SILP__
            }                                                                                               //__SILP__
            return null;                                                                                    //__SILP__
        }                                                                                                   //__SILP__
    }                                                                                                       //__SILP__
                                                                                                            //__SILP__
    //SILP: PROPERTY_CLASS(Long, long)
    public sealed class LongBlockValueChecker : WeakBlock, ValueChecker<long> {                              //__SILP__
        public delegate bool CheckerBlock(string path, long val, long newVal);                               //__SILP__
                                                                                                             //__SILP__
        private readonly CheckerBlock _Block;                                                                //__SILP__
                                                                                                             //__SILP__
        public LongBlockValueChecker(BlockOwner owner, CheckerBlock block) : base(owner) {                   //__SILP__
            _Block = block;                                                                                  //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        public bool IsValid(string path, long val, long newVal) {                                            //__SILP__
            return _Block(path, val, newVal);                                                                //__SILP__
        }                                                                                                    //__SILP__
    }                                                                                                        //__SILP__
                                                                                                             //__SILP__
    public sealed class LongBlockValueWatcher : WeakBlock, ValueWatcher<long> {                              //__SILP__
        public delegate void WatcherBlock(string path, long val, long newVal);                               //__SILP__
                                                                                                             //__SILP__
        private readonly WatcherBlock _Block;                                                                //__SILP__
                                                                                                             //__SILP__
        public LongBlockValueWatcher(BlockOwner owner, WatcherBlock block) : base(owner) {                   //__SILP__
            _Block = block;                                                                                  //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        public void OnChanged(string path, long lastVal, long val) {                                         //__SILP__
            _Block(path, lastVal, val);                                                                      //__SILP__
        }                                                                                                    //__SILP__
    }                                                                                                        //__SILP__
                                                                                                             //__SILP__
    public class LongProperty : Property<long> {                                                             //__SILP__
        public override string Type {                                                                        //__SILP__
            get { return PropertiesConsts.TypeLongProperty; }                                                //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        protected override bool DoEncode(Data data) {                                                        //__SILP__
            return data.SetLong(PropertiesConsts.KeyValue, Value);                                           //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        protected override bool DoDecode(Pass pass, Data data) {                                             //__SILP__
            return SetValue(pass, data.GetLong(PropertiesConsts.KeyValue));                                  //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        protected override bool NeedUpdate(long newVal) {                                                    //__SILP__
            return NeedSetup || (Value != newVal);                                                           //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        public LongBlockValueChecker AddBlockValueChecker(Pass pass, BlockOwner owner,                       //__SILP__
                                                             LongBlockValueChecker.CheckerBlock _checker) {  //__SILP__
            if (!CheckAdminPass(pass)) return null;                                                          //__SILP__
                                                                                                             //__SILP__
            LongBlockValueChecker checker = new LongBlockValueChecker(owner, _checker);                      //__SILP__
            if (AddValueChecker(pass, checker)) {                                                            //__SILP__
                return checker;                                                                              //__SILP__
            }                                                                                                //__SILP__
            return null;                                                                                     //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        public LongBlockValueChecker AddBlockValueChecker(BlockOwner owner,                                  //__SILP__
                                                             LongBlockValueChecker.CheckerBlock checker) {   //__SILP__
            return AddBlockValueChecker(null, checker);                                                      //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        public LongBlockValueWatcher AddBlockValueWatcher(BlockOwner owner,                                  //__SILP__
                                                             LongBlockValueWatcher.WatcherBlock _watcher) {  //__SILP__
            LongBlockValueWatcher watcher = new LongBlockValueWatcher(owner, _watcher);                      //__SILP__
            if (AddValueWatcher(watcher)) {                                                                  //__SILP__
                return watcher;                                                                              //__SILP__
            }                                                                                                //__SILP__
            return null;                                                                                     //__SILP__
        }                                                                                                    //__SILP__
    }                                                                                                        //__SILP__
                                                                                                             //__SILP__
    //SILP: PROPERTY_CLASS(Float, float)
    public sealed class FloatBlockValueChecker : WeakBlock, ValueChecker<float> {                             //__SILP__
        public delegate bool CheckerBlock(string path, float val, float newVal);                              //__SILP__
                                                                                                              //__SILP__
        private readonly CheckerBlock _Block;                                                                 //__SILP__
                                                                                                              //__SILP__
        public FloatBlockValueChecker(BlockOwner owner, CheckerBlock block) : base(owner) {                   //__SILP__
            _Block = block;                                                                                   //__SILP__
        }                                                                                                     //__SILP__
                                                                                                              //__SILP__
        public bool IsValid(string path, float val, float newVal) {                                           //__SILP__
            return _Block(path, val, newVal);                                                                 //__SILP__
        }                                                                                                     //__SILP__
    }                                                                                                         //__SILP__
                                                                                                              //__SILP__
    public sealed class FloatBlockValueWatcher : WeakBlock, ValueWatcher<float> {                             //__SILP__
        public delegate void WatcherBlock(string path, float val, float newVal);                              //__SILP__
                                                                                                              //__SILP__
        private readonly WatcherBlock _Block;                                                                 //__SILP__
                                                                                                              //__SILP__
        public FloatBlockValueWatcher(BlockOwner owner, WatcherBlock block) : base(owner) {                   //__SILP__
            _Block = block;                                                                                   //__SILP__
        }                                                                                                     //__SILP__
                                                                                                              //__SILP__
        public void OnChanged(string path, float lastVal, float val) {                                        //__SILP__
            _Block(path, lastVal, val);                                                                       //__SILP__
        }                                                                                                     //__SILP__
    }                                                                                                         //__SILP__
                                                                                                              //__SILP__
    public class FloatProperty : Property<float> {                                                            //__SILP__
        public override string Type {                                                                         //__SILP__
            get { return PropertiesConsts.TypeFloatProperty; }                                                //__SILP__
        }                                                                                                     //__SILP__
                                                                                                              //__SILP__
        protected override bool DoEncode(Data data) {                                                         //__SILP__
            return data.SetFloat(PropertiesConsts.KeyValue, Value);                                           //__SILP__
        }                                                                                                     //__SILP__
                                                                                                              //__SILP__
        protected override bool DoDecode(Pass pass, Data data) {                                              //__SILP__
            return SetValue(pass, data.GetFloat(PropertiesConsts.KeyValue));                                  //__SILP__
        }                                                                                                     //__SILP__
                                                                                                              //__SILP__
        protected override bool NeedUpdate(float newVal) {                                                    //__SILP__
            return NeedSetup || (Value != newVal);                                                            //__SILP__
        }                                                                                                     //__SILP__
                                                                                                              //__SILP__
        public FloatBlockValueChecker AddBlockValueChecker(Pass pass, BlockOwner owner,                       //__SILP__
                                                             FloatBlockValueChecker.CheckerBlock _checker) {  //__SILP__
            if (!CheckAdminPass(pass)) return null;                                                           //__SILP__
                                                                                                              //__SILP__
            FloatBlockValueChecker checker = new FloatBlockValueChecker(owner, _checker);                     //__SILP__
            if (AddValueChecker(pass, checker)) {                                                             //__SILP__
                return checker;                                                                               //__SILP__
            }                                                                                                 //__SILP__
            return null;                                                                                      //__SILP__
        }                                                                                                     //__SILP__
                                                                                                              //__SILP__
        public FloatBlockValueChecker AddBlockValueChecker(BlockOwner owner,                                  //__SILP__
                                                             FloatBlockValueChecker.CheckerBlock checker) {   //__SILP__
            return AddBlockValueChecker(null, checker);                                                       //__SILP__
        }                                                                                                     //__SILP__
                                                                                                              //__SILP__
        public FloatBlockValueWatcher AddBlockValueWatcher(BlockOwner owner,                                  //__SILP__
                                                             FloatBlockValueWatcher.WatcherBlock _watcher) {  //__SILP__
            FloatBlockValueWatcher watcher = new FloatBlockValueWatcher(owner, _watcher);                     //__SILP__
            if (AddValueWatcher(watcher)) {                                                                   //__SILP__
                return watcher;                                                                               //__SILP__
            }                                                                                                 //__SILP__
            return null;                                                                                      //__SILP__
        }                                                                                                     //__SILP__
    }                                                                                                         //__SILP__
                                                                                                              //__SILP__
    //SILP: PROPERTY_CLASS(Double, double)
    public sealed class DoubleBlockValueChecker : WeakBlock, ValueChecker<double> {                            //__SILP__
        public delegate bool CheckerBlock(string path, double val, double newVal);                             //__SILP__
                                                                                                               //__SILP__
        private readonly CheckerBlock _Block;                                                                  //__SILP__
                                                                                                               //__SILP__
        public DoubleBlockValueChecker(BlockOwner owner, CheckerBlock block) : base(owner) {                   //__SILP__
            _Block = block;                                                                                    //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public bool IsValid(string path, double val, double newVal) {                                          //__SILP__
            return _Block(path, val, newVal);                                                                  //__SILP__
        }                                                                                                      //__SILP__
    }                                                                                                          //__SILP__
                                                                                                               //__SILP__
    public sealed class DoubleBlockValueWatcher : WeakBlock, ValueWatcher<double> {                            //__SILP__
        public delegate void WatcherBlock(string path, double val, double newVal);                             //__SILP__
                                                                                                               //__SILP__
        private readonly WatcherBlock _Block;                                                                  //__SILP__
                                                                                                               //__SILP__
        public DoubleBlockValueWatcher(BlockOwner owner, WatcherBlock block) : base(owner) {                   //__SILP__
            _Block = block;                                                                                    //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public void OnChanged(string path, double lastVal, double val) {                                       //__SILP__
            _Block(path, lastVal, val);                                                                        //__SILP__
        }                                                                                                      //__SILP__
    }                                                                                                          //__SILP__
                                                                                                               //__SILP__
    public class DoubleProperty : Property<double> {                                                           //__SILP__
        public override string Type {                                                                          //__SILP__
            get { return PropertiesConsts.TypeDoubleProperty; }                                                //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        protected override bool DoEncode(Data data) {                                                          //__SILP__
            return data.SetDouble(PropertiesConsts.KeyValue, Value);                                           //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        protected override bool DoDecode(Pass pass, Data data) {                                               //__SILP__
            return SetValue(pass, data.GetDouble(PropertiesConsts.KeyValue));                                  //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        protected override bool NeedUpdate(double newVal) {                                                    //__SILP__
            return NeedSetup || (Value != newVal);                                                             //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public DoubleBlockValueChecker AddBlockValueChecker(Pass pass, BlockOwner owner,                       //__SILP__
                                                             DoubleBlockValueChecker.CheckerBlock _checker) {  //__SILP__
            if (!CheckAdminPass(pass)) return null;                                                            //__SILP__
                                                                                                               //__SILP__
            DoubleBlockValueChecker checker = new DoubleBlockValueChecker(owner, _checker);                    //__SILP__
            if (AddValueChecker(pass, checker)) {                                                              //__SILP__
                return checker;                                                                                //__SILP__
            }                                                                                                  //__SILP__
            return null;                                                                                       //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public DoubleBlockValueChecker AddBlockValueChecker(BlockOwner owner,                                  //__SILP__
                                                             DoubleBlockValueChecker.CheckerBlock checker) {   //__SILP__
            return AddBlockValueChecker(null, checker);                                                        //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public DoubleBlockValueWatcher AddBlockValueWatcher(BlockOwner owner,                                  //__SILP__
                                                             DoubleBlockValueWatcher.WatcherBlock _watcher) {  //__SILP__
            DoubleBlockValueWatcher watcher = new DoubleBlockValueWatcher(owner, _watcher);                    //__SILP__
            if (AddValueWatcher(watcher)) {                                                                    //__SILP__
                return watcher;                                                                                //__SILP__
            }                                                                                                  //__SILP__
            return null;                                                                                       //__SILP__
        }                                                                                                      //__SILP__
    }                                                                                                          //__SILP__
                                                                                                               //__SILP__
    //SILP: PROPERTY_CLASS(String, string)
    public sealed class StringBlockValueChecker : WeakBlock, ValueChecker<string> {                            //__SILP__
        public delegate bool CheckerBlock(string path, string val, string newVal);                             //__SILP__
                                                                                                               //__SILP__
        private readonly CheckerBlock _Block;                                                                  //__SILP__
                                                                                                               //__SILP__
        public StringBlockValueChecker(BlockOwner owner, CheckerBlock block) : base(owner) {                   //__SILP__
            _Block = block;                                                                                    //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public bool IsValid(string path, string val, string newVal) {                                          //__SILP__
            return _Block(path, val, newVal);                                                                  //__SILP__
        }                                                                                                      //__SILP__
    }                                                                                                          //__SILP__
                                                                                                               //__SILP__
    public sealed class StringBlockValueWatcher : WeakBlock, ValueWatcher<string> {                            //__SILP__
        public delegate void WatcherBlock(string path, string val, string newVal);                             //__SILP__
                                                                                                               //__SILP__
        private readonly WatcherBlock _Block;                                                                  //__SILP__
                                                                                                               //__SILP__
        public StringBlockValueWatcher(BlockOwner owner, WatcherBlock block) : base(owner) {                   //__SILP__
            _Block = block;                                                                                    //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public void OnChanged(string path, string lastVal, string val) {                                       //__SILP__
            _Block(path, lastVal, val);                                                                        //__SILP__
        }                                                                                                      //__SILP__
    }                                                                                                          //__SILP__
                                                                                                               //__SILP__
    public class StringProperty : Property<string> {                                                           //__SILP__
        public override string Type {                                                                          //__SILP__
            get { return PropertiesConsts.TypeStringProperty; }                                                //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        protected override bool DoEncode(Data data) {                                                          //__SILP__
            return data.SetString(PropertiesConsts.KeyValue, Value);                                           //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        protected override bool DoDecode(Pass pass, Data data) {                                               //__SILP__
            return SetValue(pass, data.GetString(PropertiesConsts.KeyValue));                                  //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        protected override bool NeedUpdate(string newVal) {                                                    //__SILP__
            return NeedSetup || (Value != newVal);                                                             //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public StringBlockValueChecker AddBlockValueChecker(Pass pass, BlockOwner owner,                       //__SILP__
                                                             StringBlockValueChecker.CheckerBlock _checker) {  //__SILP__
            if (!CheckAdminPass(pass)) return null;                                                            //__SILP__
                                                                                                               //__SILP__
            StringBlockValueChecker checker = new StringBlockValueChecker(owner, _checker);                    //__SILP__
            if (AddValueChecker(pass, checker)) {                                                              //__SILP__
                return checker;                                                                                //__SILP__
            }                                                                                                  //__SILP__
            return null;                                                                                       //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public StringBlockValueChecker AddBlockValueChecker(BlockOwner owner,                                  //__SILP__
                                                             StringBlockValueChecker.CheckerBlock checker) {   //__SILP__
            return AddBlockValueChecker(null, checker);                                                        //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public StringBlockValueWatcher AddBlockValueWatcher(BlockOwner owner,                                  //__SILP__
                                                             StringBlockValueWatcher.WatcherBlock _watcher) {  //__SILP__
            StringBlockValueWatcher watcher = new StringBlockValueWatcher(owner, _watcher);                    //__SILP__
            if (AddValueWatcher(watcher)) {                                                                    //__SILP__
                return watcher;                                                                                //__SILP__
            }                                                                                                  //__SILP__
            return null;                                                                                       //__SILP__
        }                                                                                                      //__SILP__
    }                                                                                                          //__SILP__
                                                                                                               //__SILP__
    //SILP: PROPERTY_CLASS(Data, Data)
    public sealed class DataBlockValueChecker : WeakBlock, ValueChecker<Data> {                              //__SILP__
        public delegate bool CheckerBlock(string path, Data val, Data newVal);                               //__SILP__
                                                                                                             //__SILP__
        private readonly CheckerBlock _Block;                                                                //__SILP__
                                                                                                             //__SILP__
        public DataBlockValueChecker(BlockOwner owner, CheckerBlock block) : base(owner) {                   //__SILP__
            _Block = block;                                                                                  //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        public bool IsValid(string path, Data val, Data newVal) {                                            //__SILP__
            return _Block(path, val, newVal);                                                                //__SILP__
        }                                                                                                    //__SILP__
    }                                                                                                        //__SILP__
                                                                                                             //__SILP__
    public sealed class DataBlockValueWatcher : WeakBlock, ValueWatcher<Data> {                              //__SILP__
        public delegate void WatcherBlock(string path, Data val, Data newVal);                               //__SILP__
                                                                                                             //__SILP__
        private readonly WatcherBlock _Block;                                                                //__SILP__
                                                                                                             //__SILP__
        public DataBlockValueWatcher(BlockOwner owner, WatcherBlock block) : base(owner) {                   //__SILP__
            _Block = block;                                                                                  //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        public void OnChanged(string path, Data lastVal, Data val) {                                         //__SILP__
            _Block(path, lastVal, val);                                                                      //__SILP__
        }                                                                                                    //__SILP__
    }                                                                                                        //__SILP__
                                                                                                             //__SILP__
    public class DataProperty : Property<Data> {                                                             //__SILP__
        public override string Type {                                                                        //__SILP__
            get { return PropertiesConsts.TypeDataProperty; }                                                //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        protected override bool DoEncode(Data data) {                                                        //__SILP__
            return data.SetData(PropertiesConsts.KeyValue, Value);                                           //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        protected override bool DoDecode(Pass pass, Data data) {                                             //__SILP__
            return SetValue(pass, data.GetData(PropertiesConsts.KeyValue));                                  //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        protected override bool NeedUpdate(Data newVal) {                                                    //__SILP__
            return NeedSetup || (Value != newVal);                                                           //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        public DataBlockValueChecker AddBlockValueChecker(Pass pass, BlockOwner owner,                       //__SILP__
                                                             DataBlockValueChecker.CheckerBlock _checker) {  //__SILP__
            if (!CheckAdminPass(pass)) return null;                                                          //__SILP__
                                                                                                             //__SILP__
            DataBlockValueChecker checker = new DataBlockValueChecker(owner, _checker);                      //__SILP__
            if (AddValueChecker(pass, checker)) {                                                            //__SILP__
                return checker;                                                                              //__SILP__
            }                                                                                                //__SILP__
            return null;                                                                                     //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        public DataBlockValueChecker AddBlockValueChecker(BlockOwner owner,                                  //__SILP__
                                                             DataBlockValueChecker.CheckerBlock checker) {   //__SILP__
            return AddBlockValueChecker(null, checker);                                                      //__SILP__
        }                                                                                                    //__SILP__
                                                                                                             //__SILP__
        public DataBlockValueWatcher AddBlockValueWatcher(BlockOwner owner,                                  //__SILP__
                                                             DataBlockValueWatcher.WatcherBlock _watcher) {  //__SILP__
            DataBlockValueWatcher watcher = new DataBlockValueWatcher(owner, _watcher);                      //__SILP__
            if (AddValueWatcher(watcher)) {                                                                  //__SILP__
                return watcher;                                                                              //__SILP__
            }                                                                                                //__SILP__
            return null;                                                                                     //__SILP__
        }                                                                                                    //__SILP__
    }                                                                                                        //__SILP__
                                                                                                             //__SILP__
}
