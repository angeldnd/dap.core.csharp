using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface ValueChecker<T> {
        bool IsValid(string path, T val, T newVal);
    }

    public interface ValueWatcher<T> {
        void OnChanged(string path, T lastVal, T val);
    }

    public interface Property : Var {
        Object GetValue();

        bool AddToSpec(Data spec);
    }

    public abstract class Property<T>: Var<T>, Property {
        public abstract AddToSpec(Data spec);

        //SILP: DECLARE_LIST(ValueChecker, checker, ValueChecker<T>, _Checkers)
        protected List<ValueChecker<T>> _Checkers = null;                    //__SILP__
                                                                             //__SILP__
        public int ValueCheckerCount {                                       //__SILP__
            get {                                                            //__SILP__
                if (_Checkers == null) {                                     //__SILP__
                    return 0;                                                //__SILP__
                }                                                            //__SILP__
                return _Checkers.Count;                                      //__SILP__
            }                                                                //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        public bool AddValueChecker(ValueChecker<T> checker) {               //__SILP__
            if (_Checkers == null) _Checkers = new List<ValueChecker<T>>();  //__SILP__
            if (!_Checkers.Contains(checker)) {                              //__SILP__
                _Checkers.Add(checker);                                      //__SILP__
                return true;                                                 //__SILP__
            }                                                                //__SILP__
            return false;                                                    //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        public bool RemoveValueChecker(ValueChecker<T> checker) {            //__SILP__
            if (_Checkers != null && _Checkers.Contains(checker)) {          //__SILP__
                _Checkers.Remove(checker);                                   //__SILP__
                return true;                                                 //__SILP__
            }                                                                //__SILP__
            return false;                                                    //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        //SILP: DECLARE_LIST(ValueWatcher, watcher, ValueWatcher<T>, _Watchers)
        protected List<ValueWatcher<T>> _Watchers = null;                    //__SILP__
                                                                             //__SILP__
        public int ValueWatcherCount {                                       //__SILP__
            get {                                                            //__SILP__
                if (_Watchers == null) {                                     //__SILP__
                    return 0;                                                //__SILP__
                }                                                            //__SILP__
                return _Watchers.Count;                                      //__SILP__
            }                                                                //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        public bool AddValueWatcher(ValueWatcher<T> watcher) {               //__SILP__
            if (_Watchers == null) _Watchers = new List<ValueWatcher<T>>();  //__SILP__
            if (!_Watchers.Contains(watcher)) {                              //__SILP__
                _Watchers.Add(watcher);                                      //__SILP__
                return true;                                                 //__SILP__
            }                                                                //__SILP__
            return false;                                                    //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        public bool RemoveValueWatcher(ValueWatcher<T> watcher) {            //__SILP__
            if (_Watchers != null && _Watchers.Contains(watcher)) {          //__SILP__
                _Watchers.Remove(watcher);                                   //__SILP__
                return true;                                                 //__SILP__
            }                                                                //__SILP__
            return false;                                                    //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
    }

    //SILP: PROPERTY_CLASS(Bool, bool)
    public sealed class BoolBlockValueChecker : ValueChecker<bool> {                                   //__SILP__
        public delegate bool CheckerBlock(string path, bool val, bool newVal);                         //__SILP__
                                                                                                       //__SILP__
        private readonly CheckerBlock _Block;                                                          //__SILP__
                                                                                                       //__SILP__
        public BoolBlockValueChecker(CheckerBlock block) {                                             //__SILP__
            _Block = block;                                                                            //__SILP__
        }                                                                                              //__SILP__
                                                                                                       //__SILP__
        public bool IsValid(string path, bool val, bool newVal) {                                      //__SILP__
            return _Block(path, val, newVal);                                                          //__SILP__
        }                                                                                              //__SILP__
    }                                                                                                  //__SILP__
                                                                                                       //__SILP__
    public sealed class BoolBlockValueWatcher : ValueWatcher<bool> {                                   //__SILP__
        public delegate void WatcherBlock(string path, bool val, bool newVal);                         //__SILP__
                                                                                                       //__SILP__
        private readonly WatcherBlock _Block;                                                          //__SILP__
                                                                                                       //__SILP__
        public BoolBlockValueWatcher(WatcherBlock block) {                                             //__SILP__
            _Block = block;                                                                            //__SILP__
        }                                                                                              //__SILP__
                                                                                                       //__SILP__
        public void OnChanged(string path, bool lastVal, bool val) {                                   //__SILP__
            _Block(path, lastVal, val);                                                                //__SILP__
        }                                                                                              //__SILP__
    }                                                                                                  //__SILP__
                                                                                                       //__SILP__
    public class BoolProperty : Property<bool> {                                                       //__SILP__
        public override string Type {                                                                  //__SILP__
            get { return PropertiesConsts.TypeBoolProperty; }                                          //__SILP__
        }                                                                                              //__SILP__
                                                                                                       //__SILP__
        protected override bool DoEncode(Data data) {                                                  //__SILP__
            return data.SetBool(PropertiesConsts.KeyValue, Value);                                     //__SILP__
        }                                                                                              //__SILP__
                                                                                                       //__SILP__
        protected override bool DoDecode(Data data) {                                                  //__SILP__
            SetValue(data.GetBool(PropertiesConsts.KeyValue));                                         //__SILP__
            return true;                                                                               //__SILP__
        }                                                                                              //__SILP__
                                                                                                       //__SILP__
        public override bool AddToSpec(Data spec) {                                                    //__SILP__
            return data.SetBool(Path, Value);                                                          //__SILP__
        }                                                                                              //__SILP__
                                                                                                       //__SILP__
        private bool _CheckingValue = false;                                                           //__SILP__
        private bool _UpdatingValue = false;                                                           //__SILP__
                                                                                                       //__SILP__
        public override bool SetValue(bool newVal) {                                                   //__SILP__
            if (_CheckingValue) return false;                                                          //__SILP__
            if (_UpdatingValue) return false;                                                          //__SILP__
                                                                                                       //__SILP__
            if (Value != newVal) {                                                                     //__SILP__
                if (_Checkers != null) {                                                               //__SILP__
                    _CheckingValue = true;                                                             //__SILP__
                    for (int i = 0; i < _Checkers.Count; i++) {                                        //__SILP__
                        if (!_Checkers[i].IsValid(Path, Value, newVal)) {                              //__SILP__
                            _CheckingValue = false;                                                    //__SILP__
                            return false;                                                              //__SILP__
                        }                                                                              //__SILP__
                    }                                                                                  //__SILP__
                    _CheckingValue = false;                                                            //__SILP__
                }                                                                                      //__SILP__
                _UpdatingValue = true;                                                                 //__SILP__
                bool lastVal = Value;                                                                  //__SILP__
                if (!base.SetValue(newVal)) {                                                          //__SILP__
                    _UpdatingValue = false;                                                            //__SILP__
                    return false;                                                                      //__SILP__
                }                                                                                      //__SILP__
                if (_Watchers != null) {                                                               //__SILP__
                    for (int i = 0; i < _Watchers.Count; i++) {                                        //__SILP__
                        _Watchers[i].OnChanged(Path, lastVal, Value);                                  //__SILP__
                    }                                                                                  //__SILP__
                }                                                                                      //__SILP__
                _UpdatingValue = false;                                                                //__SILP__
                return true;                                                                           //__SILP__
            }                                                                                          //__SILP__
            return false;                                                                              //__SILP__
        }                                                                                              //__SILP__
                                                                                                       //__SILP__
        public BoolBlockValueChecker AddBlockValueChecker(BoolBlockValueChecker.CheckerBlock block) {  //__SILP__
            BoolBlockValueChecker checker = new BoolBlockValueChecker(block);                          //__SILP__
            if (AddValueChecker(checker)) {                                                            //__SILP__
                return checker;                                                                        //__SILP__
            }                                                                                          //__SILP__
            return null;                                                                               //__SILP__
        }                                                                                              //__SILP__
                                                                                                       //__SILP__
        public BoolBlockValueWatcher AddBlockValueWatcher(BoolBlockValueWatcher.WatcherBlock block) {  //__SILP__
            BoolBlockValueWatcher watcher = new BoolBlockValueWatcher(block);                          //__SILP__
            if (AddValueWatcher(watcher)) {                                                            //__SILP__
                return watcher;                                                                        //__SILP__
            }                                                                                          //__SILP__
            return null;                                                                               //__SILP__
        }                                                                                              //__SILP__
    }                                                                                                  //__SILP__
                                                                                                       //__SILP__
    //SILP: PROPERTY_CLASS(Int, int)
    public sealed class IntBlockValueChecker : ValueChecker<int> {                                   //__SILP__
        public delegate bool CheckerBlock(string path, int val, int newVal);                         //__SILP__
                                                                                                     //__SILP__
        private readonly CheckerBlock _Block;                                                        //__SILP__
                                                                                                     //__SILP__
        public IntBlockValueChecker(CheckerBlock block) {                                            //__SILP__
            _Block = block;                                                                          //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        public bool IsValid(string path, int val, int newVal) {                                      //__SILP__
            return _Block(path, val, newVal);                                                        //__SILP__
        }                                                                                            //__SILP__
    }                                                                                                //__SILP__
                                                                                                     //__SILP__
    public sealed class IntBlockValueWatcher : ValueWatcher<int> {                                   //__SILP__
        public delegate void WatcherBlock(string path, int val, int newVal);                         //__SILP__
                                                                                                     //__SILP__
        private readonly WatcherBlock _Block;                                                        //__SILP__
                                                                                                     //__SILP__
        public IntBlockValueWatcher(WatcherBlock block) {                                            //__SILP__
            _Block = block;                                                                          //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        public void OnChanged(string path, int lastVal, int val) {                                   //__SILP__
            _Block(path, lastVal, val);                                                              //__SILP__
        }                                                                                            //__SILP__
    }                                                                                                //__SILP__
                                                                                                     //__SILP__
    public class IntProperty : Property<int> {                                                       //__SILP__
        public override string Type {                                                                //__SILP__
            get { return PropertiesConsts.TypeIntProperty; }                                         //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        protected override bool DoEncode(Data data) {                                                //__SILP__
            return data.SetInt(PropertiesConsts.KeyValue, Value);                                    //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        protected override bool DoDecode(Data data) {                                                //__SILP__
            SetValue(data.GetInt(PropertiesConsts.KeyValue));                                        //__SILP__
            return true;                                                                             //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        public override bool AddToSpec(Data spec) {                                                  //__SILP__
            return data.SetInt(Path, Value);                                                         //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        private bool _CheckingValue = false;                                                         //__SILP__
        private bool _UpdatingValue = false;                                                         //__SILP__
                                                                                                     //__SILP__
        public override bool SetValue(int newVal) {                                                  //__SILP__
            if (_CheckingValue) return false;                                                        //__SILP__
            if (_UpdatingValue) return false;                                                        //__SILP__
                                                                                                     //__SILP__
            if (Value != newVal) {                                                                   //__SILP__
                if (_Checkers != null) {                                                             //__SILP__
                    _CheckingValue = true;                                                           //__SILP__
                    for (int i = 0; i < _Checkers.Count; i++) {                                      //__SILP__
                        if (!_Checkers[i].IsValid(Path, Value, newVal)) {                            //__SILP__
                            _CheckingValue = false;                                                  //__SILP__
                            return false;                                                            //__SILP__
                        }                                                                            //__SILP__
                    }                                                                                //__SILP__
                    _CheckingValue = false;                                                          //__SILP__
                }                                                                                    //__SILP__
                _UpdatingValue = true;                                                               //__SILP__
                int lastVal = Value;                                                                 //__SILP__
                if (!base.SetValue(newVal)) {                                                        //__SILP__
                    _UpdatingValue = false;                                                          //__SILP__
                    return false;                                                                    //__SILP__
                }                                                                                    //__SILP__
                if (_Watchers != null) {                                                             //__SILP__
                    for (int i = 0; i < _Watchers.Count; i++) {                                      //__SILP__
                        _Watchers[i].OnChanged(Path, lastVal, Value);                                //__SILP__
                    }                                                                                //__SILP__
                }                                                                                    //__SILP__
                _UpdatingValue = false;                                                              //__SILP__
                return true;                                                                         //__SILP__
            }                                                                                        //__SILP__
            return false;                                                                            //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        public IntBlockValueChecker AddBlockValueChecker(IntBlockValueChecker.CheckerBlock block) {  //__SILP__
            IntBlockValueChecker checker = new IntBlockValueChecker(block);                          //__SILP__
            if (AddValueChecker(checker)) {                                                          //__SILP__
                return checker;                                                                      //__SILP__
            }                                                                                        //__SILP__
            return null;                                                                             //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        public IntBlockValueWatcher AddBlockValueWatcher(IntBlockValueWatcher.WatcherBlock block) {  //__SILP__
            IntBlockValueWatcher watcher = new IntBlockValueWatcher(block);                          //__SILP__
            if (AddValueWatcher(watcher)) {                                                          //__SILP__
                return watcher;                                                                      //__SILP__
            }                                                                                        //__SILP__
            return null;                                                                             //__SILP__
        }                                                                                            //__SILP__
    }                                                                                                //__SILP__
                                                                                                     //__SILP__
    //SILP: PROPERTY_CLASS(Long, long)
    public sealed class LongBlockValueChecker : ValueChecker<long> {                                   //__SILP__
        public delegate bool CheckerBlock(string path, long val, long newVal);                         //__SILP__
                                                                                                       //__SILP__
        private readonly CheckerBlock _Block;                                                          //__SILP__
                                                                                                       //__SILP__
        public LongBlockValueChecker(CheckerBlock block) {                                             //__SILP__
            _Block = block;                                                                            //__SILP__
        }                                                                                              //__SILP__
                                                                                                       //__SILP__
        public bool IsValid(string path, long val, long newVal) {                                      //__SILP__
            return _Block(path, val, newVal);                                                          //__SILP__
        }                                                                                              //__SILP__
    }                                                                                                  //__SILP__
                                                                                                       //__SILP__
    public sealed class LongBlockValueWatcher : ValueWatcher<long> {                                   //__SILP__
        public delegate void WatcherBlock(string path, long val, long newVal);                         //__SILP__
                                                                                                       //__SILP__
        private readonly WatcherBlock _Block;                                                          //__SILP__
                                                                                                       //__SILP__
        public LongBlockValueWatcher(WatcherBlock block) {                                             //__SILP__
            _Block = block;                                                                            //__SILP__
        }                                                                                              //__SILP__
                                                                                                       //__SILP__
        public void OnChanged(string path, long lastVal, long val) {                                   //__SILP__
            _Block(path, lastVal, val);                                                                //__SILP__
        }                                                                                              //__SILP__
    }                                                                                                  //__SILP__
                                                                                                       //__SILP__
    public class LongProperty : Property<long> {                                                       //__SILP__
        public override string Type {                                                                  //__SILP__
            get { return PropertiesConsts.TypeLongProperty; }                                          //__SILP__
        }                                                                                              //__SILP__
                                                                                                       //__SILP__
        protected override bool DoEncode(Data data) {                                                  //__SILP__
            return data.SetLong(PropertiesConsts.KeyValue, Value);                                     //__SILP__
        }                                                                                              //__SILP__
                                                                                                       //__SILP__
        protected override bool DoDecode(Data data) {                                                  //__SILP__
            SetValue(data.GetLong(PropertiesConsts.KeyValue));                                         //__SILP__
            return true;                                                                               //__SILP__
        }                                                                                              //__SILP__
                                                                                                       //__SILP__
        public override bool AddToSpec(Data spec) {                                                    //__SILP__
            return data.SetLong(Path, Value);                                                          //__SILP__
        }                                                                                              //__SILP__
                                                                                                       //__SILP__
        private bool _CheckingValue = false;                                                           //__SILP__
        private bool _UpdatingValue = false;                                                           //__SILP__
                                                                                                       //__SILP__
        public override bool SetValue(long newVal) {                                                   //__SILP__
            if (_CheckingValue) return false;                                                          //__SILP__
            if (_UpdatingValue) return false;                                                          //__SILP__
                                                                                                       //__SILP__
            if (Value != newVal) {                                                                     //__SILP__
                if (_Checkers != null) {                                                               //__SILP__
                    _CheckingValue = true;                                                             //__SILP__
                    for (int i = 0; i < _Checkers.Count; i++) {                                        //__SILP__
                        if (!_Checkers[i].IsValid(Path, Value, newVal)) {                              //__SILP__
                            _CheckingValue = false;                                                    //__SILP__
                            return false;                                                              //__SILP__
                        }                                                                              //__SILP__
                    }                                                                                  //__SILP__
                    _CheckingValue = false;                                                            //__SILP__
                }                                                                                      //__SILP__
                _UpdatingValue = true;                                                                 //__SILP__
                long lastVal = Value;                                                                  //__SILP__
                if (!base.SetValue(newVal)) {                                                          //__SILP__
                    _UpdatingValue = false;                                                            //__SILP__
                    return false;                                                                      //__SILP__
                }                                                                                      //__SILP__
                if (_Watchers != null) {                                                               //__SILP__
                    for (int i = 0; i < _Watchers.Count; i++) {                                        //__SILP__
                        _Watchers[i].OnChanged(Path, lastVal, Value);                                  //__SILP__
                    }                                                                                  //__SILP__
                }                                                                                      //__SILP__
                _UpdatingValue = false;                                                                //__SILP__
                return true;                                                                           //__SILP__
            }                                                                                          //__SILP__
            return false;                                                                              //__SILP__
        }                                                                                              //__SILP__
                                                                                                       //__SILP__
        public LongBlockValueChecker AddBlockValueChecker(LongBlockValueChecker.CheckerBlock block) {  //__SILP__
            LongBlockValueChecker checker = new LongBlockValueChecker(block);                          //__SILP__
            if (AddValueChecker(checker)) {                                                            //__SILP__
                return checker;                                                                        //__SILP__
            }                                                                                          //__SILP__
            return null;                                                                               //__SILP__
        }                                                                                              //__SILP__
                                                                                                       //__SILP__
        public LongBlockValueWatcher AddBlockValueWatcher(LongBlockValueWatcher.WatcherBlock block) {  //__SILP__
            LongBlockValueWatcher watcher = new LongBlockValueWatcher(block);                          //__SILP__
            if (AddValueWatcher(watcher)) {                                                            //__SILP__
                return watcher;                                                                        //__SILP__
            }                                                                                          //__SILP__
            return null;                                                                               //__SILP__
        }                                                                                              //__SILP__
    }                                                                                                  //__SILP__
                                                                                                       //__SILP__
    //SILP: PROPERTY_CLASS(Float, float)
    public sealed class FloatBlockValueChecker : ValueChecker<float> {                                   //__SILP__
        public delegate bool CheckerBlock(string path, float val, float newVal);                         //__SILP__
                                                                                                         //__SILP__
        private readonly CheckerBlock _Block;                                                            //__SILP__
                                                                                                         //__SILP__
        public FloatBlockValueChecker(CheckerBlock block) {                                              //__SILP__
            _Block = block;                                                                              //__SILP__
        }                                                                                                //__SILP__
                                                                                                         //__SILP__
        public bool IsValid(string path, float val, float newVal) {                                      //__SILP__
            return _Block(path, val, newVal);                                                            //__SILP__
        }                                                                                                //__SILP__
    }                                                                                                    //__SILP__
                                                                                                         //__SILP__
    public sealed class FloatBlockValueWatcher : ValueWatcher<float> {                                   //__SILP__
        public delegate void WatcherBlock(string path, float val, float newVal);                         //__SILP__
                                                                                                         //__SILP__
        private readonly WatcherBlock _Block;                                                            //__SILP__
                                                                                                         //__SILP__
        public FloatBlockValueWatcher(WatcherBlock block) {                                              //__SILP__
            _Block = block;                                                                              //__SILP__
        }                                                                                                //__SILP__
                                                                                                         //__SILP__
        public void OnChanged(string path, float lastVal, float val) {                                   //__SILP__
            _Block(path, lastVal, val);                                                                  //__SILP__
        }                                                                                                //__SILP__
    }                                                                                                    //__SILP__
                                                                                                         //__SILP__
    public class FloatProperty : Property<float> {                                                       //__SILP__
        public override string Type {                                                                    //__SILP__
            get { return PropertiesConsts.TypeFloatProperty; }                                           //__SILP__
        }                                                                                                //__SILP__
                                                                                                         //__SILP__
        protected override bool DoEncode(Data data) {                                                    //__SILP__
            return data.SetFloat(PropertiesConsts.KeyValue, Value);                                      //__SILP__
        }                                                                                                //__SILP__
                                                                                                         //__SILP__
        protected override bool DoDecode(Data data) {                                                    //__SILP__
            SetValue(data.GetFloat(PropertiesConsts.KeyValue));                                          //__SILP__
            return true;                                                                                 //__SILP__
        }                                                                                                //__SILP__
                                                                                                         //__SILP__
        public override bool AddToSpec(Data spec) {                                                      //__SILP__
            return data.SetFloat(Path, Value);                                                           //__SILP__
        }                                                                                                //__SILP__
                                                                                                         //__SILP__
        private bool _CheckingValue = false;                                                             //__SILP__
        private bool _UpdatingValue = false;                                                             //__SILP__
                                                                                                         //__SILP__
        public override bool SetValue(float newVal) {                                                    //__SILP__
            if (_CheckingValue) return false;                                                            //__SILP__
            if (_UpdatingValue) return false;                                                            //__SILP__
                                                                                                         //__SILP__
            if (Value != newVal) {                                                                       //__SILP__
                if (_Checkers != null) {                                                                 //__SILP__
                    _CheckingValue = true;                                                               //__SILP__
                    for (int i = 0; i < _Checkers.Count; i++) {                                          //__SILP__
                        if (!_Checkers[i].IsValid(Path, Value, newVal)) {                                //__SILP__
                            _CheckingValue = false;                                                      //__SILP__
                            return false;                                                                //__SILP__
                        }                                                                                //__SILP__
                    }                                                                                    //__SILP__
                    _CheckingValue = false;                                                              //__SILP__
                }                                                                                        //__SILP__
                _UpdatingValue = true;                                                                   //__SILP__
                float lastVal = Value;                                                                   //__SILP__
                if (!base.SetValue(newVal)) {                                                            //__SILP__
                    _UpdatingValue = false;                                                              //__SILP__
                    return false;                                                                        //__SILP__
                }                                                                                        //__SILP__
                if (_Watchers != null) {                                                                 //__SILP__
                    for (int i = 0; i < _Watchers.Count; i++) {                                          //__SILP__
                        _Watchers[i].OnChanged(Path, lastVal, Value);                                    //__SILP__
                    }                                                                                    //__SILP__
                }                                                                                        //__SILP__
                _UpdatingValue = false;                                                                  //__SILP__
                return true;                                                                             //__SILP__
            }                                                                                            //__SILP__
            return false;                                                                                //__SILP__
        }                                                                                                //__SILP__
                                                                                                         //__SILP__
        public FloatBlockValueChecker AddBlockValueChecker(FloatBlockValueChecker.CheckerBlock block) {  //__SILP__
            FloatBlockValueChecker checker = new FloatBlockValueChecker(block);                          //__SILP__
            if (AddValueChecker(checker)) {                                                              //__SILP__
                return checker;                                                                          //__SILP__
            }                                                                                            //__SILP__
            return null;                                                                                 //__SILP__
        }                                                                                                //__SILP__
                                                                                                         //__SILP__
        public FloatBlockValueWatcher AddBlockValueWatcher(FloatBlockValueWatcher.WatcherBlock block) {  //__SILP__
            FloatBlockValueWatcher watcher = new FloatBlockValueWatcher(block);                          //__SILP__
            if (AddValueWatcher(watcher)) {                                                              //__SILP__
                return watcher;                                                                          //__SILP__
            }                                                                                            //__SILP__
            return null;                                                                                 //__SILP__
        }                                                                                                //__SILP__
    }                                                                                                    //__SILP__
                                                                                                         //__SILP__
    //SILP: PROPERTY_CLASS(Double, double)
    public sealed class DoubleBlockValueChecker : ValueChecker<double> {                                   //__SILP__
        public delegate bool CheckerBlock(string path, double val, double newVal);                         //__SILP__
                                                                                                           //__SILP__
        private readonly CheckerBlock _Block;                                                              //__SILP__
                                                                                                           //__SILP__
        public DoubleBlockValueChecker(CheckerBlock block) {                                               //__SILP__
            _Block = block;                                                                                //__SILP__
        }                                                                                                  //__SILP__
                                                                                                           //__SILP__
        public bool IsValid(string path, double val, double newVal) {                                      //__SILP__
            return _Block(path, val, newVal);                                                              //__SILP__
        }                                                                                                  //__SILP__
    }                                                                                                      //__SILP__
                                                                                                           //__SILP__
    public sealed class DoubleBlockValueWatcher : ValueWatcher<double> {                                   //__SILP__
        public delegate void WatcherBlock(string path, double val, double newVal);                         //__SILP__
                                                                                                           //__SILP__
        private readonly WatcherBlock _Block;                                                              //__SILP__
                                                                                                           //__SILP__
        public DoubleBlockValueWatcher(WatcherBlock block) {                                               //__SILP__
            _Block = block;                                                                                //__SILP__
        }                                                                                                  //__SILP__
                                                                                                           //__SILP__
        public void OnChanged(string path, double lastVal, double val) {                                   //__SILP__
            _Block(path, lastVal, val);                                                                    //__SILP__
        }                                                                                                  //__SILP__
    }                                                                                                      //__SILP__
                                                                                                           //__SILP__
    public class DoubleProperty : Property<double> {                                                       //__SILP__
        public override string Type {                                                                      //__SILP__
            get { return PropertiesConsts.TypeDoubleProperty; }                                            //__SILP__
        }                                                                                                  //__SILP__
                                                                                                           //__SILP__
        protected override bool DoEncode(Data data) {                                                      //__SILP__
            return data.SetDouble(PropertiesConsts.KeyValue, Value);                                       //__SILP__
        }                                                                                                  //__SILP__
                                                                                                           //__SILP__
        protected override bool DoDecode(Data data) {                                                      //__SILP__
            SetValue(data.GetDouble(PropertiesConsts.KeyValue));                                           //__SILP__
            return true;                                                                                   //__SILP__
        }                                                                                                  //__SILP__
                                                                                                           //__SILP__
        public override bool AddToSpec(Data spec) {                                                        //__SILP__
            return data.SetDouble(Path, Value);                                                            //__SILP__
        }                                                                                                  //__SILP__
                                                                                                           //__SILP__
        private bool _CheckingValue = false;                                                               //__SILP__
        private bool _UpdatingValue = false;                                                               //__SILP__
                                                                                                           //__SILP__
        public override bool SetValue(double newVal) {                                                     //__SILP__
            if (_CheckingValue) return false;                                                              //__SILP__
            if (_UpdatingValue) return false;                                                              //__SILP__
                                                                                                           //__SILP__
            if (Value != newVal) {                                                                         //__SILP__
                if (_Checkers != null) {                                                                   //__SILP__
                    _CheckingValue = true;                                                                 //__SILP__
                    for (int i = 0; i < _Checkers.Count; i++) {                                            //__SILP__
                        if (!_Checkers[i].IsValid(Path, Value, newVal)) {                                  //__SILP__
                            _CheckingValue = false;                                                        //__SILP__
                            return false;                                                                  //__SILP__
                        }                                                                                  //__SILP__
                    }                                                                                      //__SILP__
                    _CheckingValue = false;                                                                //__SILP__
                }                                                                                          //__SILP__
                _UpdatingValue = true;                                                                     //__SILP__
                double lastVal = Value;                                                                    //__SILP__
                if (!base.SetValue(newVal)) {                                                              //__SILP__
                    _UpdatingValue = false;                                                                //__SILP__
                    return false;                                                                          //__SILP__
                }                                                                                          //__SILP__
                if (_Watchers != null) {                                                                   //__SILP__
                    for (int i = 0; i < _Watchers.Count; i++) {                                            //__SILP__
                        _Watchers[i].OnChanged(Path, lastVal, Value);                                      //__SILP__
                    }                                                                                      //__SILP__
                }                                                                                          //__SILP__
                _UpdatingValue = false;                                                                    //__SILP__
                return true;                                                                               //__SILP__
            }                                                                                              //__SILP__
            return false;                                                                                  //__SILP__
        }                                                                                                  //__SILP__
                                                                                                           //__SILP__
        public DoubleBlockValueChecker AddBlockValueChecker(DoubleBlockValueChecker.CheckerBlock block) {  //__SILP__
            DoubleBlockValueChecker checker = new DoubleBlockValueChecker(block);                          //__SILP__
            if (AddValueChecker(checker)) {                                                                //__SILP__
                return checker;                                                                            //__SILP__
            }                                                                                              //__SILP__
            return null;                                                                                   //__SILP__
        }                                                                                                  //__SILP__
                                                                                                           //__SILP__
        public DoubleBlockValueWatcher AddBlockValueWatcher(DoubleBlockValueWatcher.WatcherBlock block) {  //__SILP__
            DoubleBlockValueWatcher watcher = new DoubleBlockValueWatcher(block);                          //__SILP__
            if (AddValueWatcher(watcher)) {                                                                //__SILP__
                return watcher;                                                                            //__SILP__
            }                                                                                              //__SILP__
            return null;                                                                                   //__SILP__
        }                                                                                                  //__SILP__
    }                                                                                                      //__SILP__
                                                                                                           //__SILP__
    //SILP: PROPERTY_CLASS(String, string)
    public sealed class StringBlockValueChecker : ValueChecker<string> {                                   //__SILP__
        public delegate bool CheckerBlock(string path, string val, string newVal);                         //__SILP__
                                                                                                           //__SILP__
        private readonly CheckerBlock _Block;                                                              //__SILP__
                                                                                                           //__SILP__
        public StringBlockValueChecker(CheckerBlock block) {                                               //__SILP__
            _Block = block;                                                                                //__SILP__
        }                                                                                                  //__SILP__
                                                                                                           //__SILP__
        public bool IsValid(string path, string val, string newVal) {                                      //__SILP__
            return _Block(path, val, newVal);                                                              //__SILP__
        }                                                                                                  //__SILP__
    }                                                                                                      //__SILP__
                                                                                                           //__SILP__
    public sealed class StringBlockValueWatcher : ValueWatcher<string> {                                   //__SILP__
        public delegate void WatcherBlock(string path, string val, string newVal);                         //__SILP__
                                                                                                           //__SILP__
        private readonly WatcherBlock _Block;                                                              //__SILP__
                                                                                                           //__SILP__
        public StringBlockValueWatcher(WatcherBlock block) {                                               //__SILP__
            _Block = block;                                                                                //__SILP__
        }                                                                                                  //__SILP__
                                                                                                           //__SILP__
        public void OnChanged(string path, string lastVal, string val) {                                   //__SILP__
            _Block(path, lastVal, val);                                                                    //__SILP__
        }                                                                                                  //__SILP__
    }                                                                                                      //__SILP__
                                                                                                           //__SILP__
    public class StringProperty : Property<string> {                                                       //__SILP__
        public override string Type {                                                                      //__SILP__
            get { return PropertiesConsts.TypeStringProperty; }                                            //__SILP__
        }                                                                                                  //__SILP__
                                                                                                           //__SILP__
        protected override bool DoEncode(Data data) {                                                      //__SILP__
            return data.SetString(PropertiesConsts.KeyValue, Value);                                       //__SILP__
        }                                                                                                  //__SILP__
                                                                                                           //__SILP__
        protected override bool DoDecode(Data data) {                                                      //__SILP__
            SetValue(data.GetString(PropertiesConsts.KeyValue));                                           //__SILP__
            return true;                                                                                   //__SILP__
        }                                                                                                  //__SILP__
                                                                                                           //__SILP__
        public override bool AddToSpec(Data spec) {                                                        //__SILP__
            return data.SetString(Path, Value);                                                            //__SILP__
        }                                                                                                  //__SILP__
                                                                                                           //__SILP__
        private bool _CheckingValue = false;                                                               //__SILP__
        private bool _UpdatingValue = false;                                                               //__SILP__
                                                                                                           //__SILP__
        public override bool SetValue(string newVal) {                                                     //__SILP__
            if (_CheckingValue) return false;                                                              //__SILP__
            if (_UpdatingValue) return false;                                                              //__SILP__
                                                                                                           //__SILP__
            if (Value != newVal) {                                                                         //__SILP__
                if (_Checkers != null) {                                                                   //__SILP__
                    _CheckingValue = true;                                                                 //__SILP__
                    for (int i = 0; i < _Checkers.Count; i++) {                                            //__SILP__
                        if (!_Checkers[i].IsValid(Path, Value, newVal)) {                                  //__SILP__
                            _CheckingValue = false;                                                        //__SILP__
                            return false;                                                                  //__SILP__
                        }                                                                                  //__SILP__
                    }                                                                                      //__SILP__
                    _CheckingValue = false;                                                                //__SILP__
                }                                                                                          //__SILP__
                _UpdatingValue = true;                                                                     //__SILP__
                string lastVal = Value;                                                                    //__SILP__
                if (!base.SetValue(newVal)) {                                                              //__SILP__
                    _UpdatingValue = false;                                                                //__SILP__
                    return false;                                                                          //__SILP__
                }                                                                                          //__SILP__
                if (_Watchers != null) {                                                                   //__SILP__
                    for (int i = 0; i < _Watchers.Count; i++) {                                            //__SILP__
                        _Watchers[i].OnChanged(Path, lastVal, Value);                                      //__SILP__
                    }                                                                                      //__SILP__
                }                                                                                          //__SILP__
                _UpdatingValue = false;                                                                    //__SILP__
                return true;                                                                               //__SILP__
            }                                                                                              //__SILP__
            return false;                                                                                  //__SILP__
        }                                                                                                  //__SILP__
                                                                                                           //__SILP__
        public StringBlockValueChecker AddBlockValueChecker(StringBlockValueChecker.CheckerBlock block) {  //__SILP__
            StringBlockValueChecker checker = new StringBlockValueChecker(block);                          //__SILP__
            if (AddValueChecker(checker)) {                                                                //__SILP__
                return checker;                                                                            //__SILP__
            }                                                                                              //__SILP__
            return null;                                                                                   //__SILP__
        }                                                                                                  //__SILP__
                                                                                                           //__SILP__
        public StringBlockValueWatcher AddBlockValueWatcher(StringBlockValueWatcher.WatcherBlock block) {  //__SILP__
            StringBlockValueWatcher watcher = new StringBlockValueWatcher(block);                          //__SILP__
            if (AddValueWatcher(watcher)) {                                                                //__SILP__
                return watcher;                                                                            //__SILP__
            }                                                                                              //__SILP__
            return null;                                                                                   //__SILP__
        }                                                                                                  //__SILP__
    }                                                                                                      //__SILP__
                                                                                                           //__SILP__
    //SILP: PROPERTY_CLASS(Data, Data)
    public sealed class DataBlockValueChecker : ValueChecker<Data> {                                   //__SILP__
        public delegate bool CheckerBlock(string path, Data val, Data newVal);                         //__SILP__
                                                                                                       //__SILP__
        private readonly CheckerBlock _Block;                                                          //__SILP__
                                                                                                       //__SILP__
        public DataBlockValueChecker(CheckerBlock block) {                                             //__SILP__
            _Block = block;                                                                            //__SILP__
        }                                                                                              //__SILP__
                                                                                                       //__SILP__
        public bool IsValid(string path, Data val, Data newVal) {                                      //__SILP__
            return _Block(path, val, newVal);                                                          //__SILP__
        }                                                                                              //__SILP__
    }                                                                                                  //__SILP__
                                                                                                       //__SILP__
    public sealed class DataBlockValueWatcher : ValueWatcher<Data> {                                   //__SILP__
        public delegate void WatcherBlock(string path, Data val, Data newVal);                         //__SILP__
                                                                                                       //__SILP__
        private readonly WatcherBlock _Block;                                                          //__SILP__
                                                                                                       //__SILP__
        public DataBlockValueWatcher(WatcherBlock block) {                                             //__SILP__
            _Block = block;                                                                            //__SILP__
        }                                                                                              //__SILP__
                                                                                                       //__SILP__
        public void OnChanged(string path, Data lastVal, Data val) {                                   //__SILP__
            _Block(path, lastVal, val);                                                                //__SILP__
        }                                                                                              //__SILP__
    }                                                                                                  //__SILP__
                                                                                                       //__SILP__
    public class DataProperty : Property<Data> {                                                       //__SILP__
        public override string Type {                                                                  //__SILP__
            get { return PropertiesConsts.TypeDataProperty; }                                          //__SILP__
        }                                                                                              //__SILP__
                                                                                                       //__SILP__
        protected override bool DoEncode(Data data) {                                                  //__SILP__
            return data.SetData(PropertiesConsts.KeyValue, Value);                                     //__SILP__
        }                                                                                              //__SILP__
                                                                                                       //__SILP__
        protected override bool DoDecode(Data data) {                                                  //__SILP__
            SetValue(data.GetData(PropertiesConsts.KeyValue));                                         //__SILP__
            return true;                                                                               //__SILP__
        }                                                                                              //__SILP__
                                                                                                       //__SILP__
        public override bool AddToSpec(Data spec) {                                                    //__SILP__
            return data.SetData(Path, Value);                                                          //__SILP__
        }                                                                                              //__SILP__
                                                                                                       //__SILP__
        private bool _CheckingValue = false;                                                           //__SILP__
        private bool _UpdatingValue = false;                                                           //__SILP__
                                                                                                       //__SILP__
        public override bool SetValue(Data newVal) {                                                   //__SILP__
            if (_CheckingValue) return false;                                                          //__SILP__
            if (_UpdatingValue) return false;                                                          //__SILP__
                                                                                                       //__SILP__
            if (Value != newVal) {                                                                     //__SILP__
                if (_Checkers != null) {                                                               //__SILP__
                    _CheckingValue = true;                                                             //__SILP__
                    for (int i = 0; i < _Checkers.Count; i++) {                                        //__SILP__
                        if (!_Checkers[i].IsValid(Path, Value, newVal)) {                              //__SILP__
                            _CheckingValue = false;                                                    //__SILP__
                            return false;                                                              //__SILP__
                        }                                                                              //__SILP__
                    }                                                                                  //__SILP__
                    _CheckingValue = false;                                                            //__SILP__
                }                                                                                      //__SILP__
                _UpdatingValue = true;                                                                 //__SILP__
                Data lastVal = Value;                                                                  //__SILP__
                if (!base.SetValue(newVal)) {                                                          //__SILP__
                    _UpdatingValue = false;                                                            //__SILP__
                    return false;                                                                      //__SILP__
                }                                                                                      //__SILP__
                if (_Watchers != null) {                                                               //__SILP__
                    for (int i = 0; i < _Watchers.Count; i++) {                                        //__SILP__
                        _Watchers[i].OnChanged(Path, lastVal, Value);                                  //__SILP__
                    }                                                                                  //__SILP__
                }                                                                                      //__SILP__
                _UpdatingValue = false;                                                                //__SILP__
                return true;                                                                           //__SILP__
            }                                                                                          //__SILP__
            return false;                                                                              //__SILP__
        }                                                                                              //__SILP__
                                                                                                       //__SILP__
        public DataBlockValueChecker AddBlockValueChecker(DataBlockValueChecker.CheckerBlock block) {  //__SILP__
            DataBlockValueChecker checker = new DataBlockValueChecker(block);                          //__SILP__
            if (AddValueChecker(checker)) {                                                            //__SILP__
                return checker;                                                                        //__SILP__
            }                                                                                          //__SILP__
            return null;                                                                               //__SILP__
        }                                                                                              //__SILP__
                                                                                                       //__SILP__
        public DataBlockValueWatcher AddBlockValueWatcher(DataBlockValueWatcher.WatcherBlock block) {  //__SILP__
            DataBlockValueWatcher watcher = new DataBlockValueWatcher(block);                          //__SILP__
            if (AddValueWatcher(watcher)) {                                                            //__SILP__
                return watcher;                                                                        //__SILP__
            }                                                                                          //__SILP__
            return null;                                                                               //__SILP__
        }                                                                                              //__SILP__
    }                                                                                                  //__SILP__
                                                                                                       //__SILP__
}
