using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface ValueChecker<T> {
        bool IsValid(string path, T val, T newVal);
    }

    public interface ValueWatcher<T> {
        void OnChanged(string path, T lastVal, T val);
    }

    public interface Property : Aspect {
        Object GetValue();
    }

    public abstract class Property<T>: Var<T>, Property {
        //SILP: DECLARE_LIST(ValueChecker, checker, ValueChecker<T>, _Checkers)
        protected List<ValueChecker<T>> _Checkers = null;                   //__SILP__
                                                                            //__SILP__
        public bool AddValueChecker(ValueChecker<T> checker) {              //__SILP__
            if (_Checkers == null) _Checkers = new List<ValueChecker<T>>(); //__SILP__
            if (!_Checkers.Contains(checker)) {                             //__SILP__
                _Checkers.Add(checker);                                     //__SILP__
                return true;                                                //__SILP__
            }                                                               //__SILP__
            return false;                                                   //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        public bool RemoveValueChecker(ValueChecker<T> checker) {           //__SILP__
            if (_Checkers != null && _Checkers.Contains(checker)) {         //__SILP__
                _Checkers.Remove(checker);                                  //__SILP__
                return true;                                                //__SILP__
            }                                                               //__SILP__
            return false;                                                   //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        //SILP: DECLARE_LIST(ValueWatcher, watcher, ValueWatcher<T>, _Watchers)
        protected List<ValueWatcher<T>> _Watchers = null;                   //__SILP__
                                                                            //__SILP__
        public bool AddValueWatcher(ValueWatcher<T> watcher) {              //__SILP__
            if (_Watchers == null) _Watchers = new List<ValueWatcher<T>>(); //__SILP__
            if (!_Watchers.Contains(watcher)) {                             //__SILP__
                _Watchers.Add(watcher);                                     //__SILP__
                return true;                                                //__SILP__
            }                                                               //__SILP__
            return false;                                                   //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        public bool RemoveValueWatcher(ValueWatcher<T> watcher) {           //__SILP__
            if (_Watchers != null && _Watchers.Contains(watcher)) {         //__SILP__
                _Watchers.Remove(watcher);                                  //__SILP__
                return true;                                                //__SILP__
            }                                                               //__SILP__
            return false;                                                   //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
    }

    //SILP: PROPERTY_CLASS(Bool, bool)
    public sealed class BlockBoolValueChecker : ValueChecker<bool> {           //__SILP__
        public delegate bool CheckerBlock(string path, bool val, bool newVal); //__SILP__
                                                                               //__SILP__
        private readonly CheckerBlock _Block;                                  //__SILP__
                                                                               //__SILP__
        public BlockBoolValueChecker(CheckerBlock block) {                     //__SILP__
            _Block = block;                                                    //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public bool IsValid(string path, bool val, bool newVal) {              //__SILP__
            return _Block(path, val, newVal);                                  //__SILP__
        }                                                                      //__SILP__
    }                                                                          //__SILP__
                                                                               //__SILP__
    public sealed class BlockBoolValueWatcher : ValueWatcher<bool> {           //__SILP__
        public delegate void WatcherBlock(string path, bool val, bool newVal); //__SILP__
                                                                               //__SILP__
        private readonly WatcherBlock _Block;                                  //__SILP__
                                                                               //__SILP__
        public BlockBoolValueWatcher(WatcherBlock block) {                     //__SILP__
            _Block = block;                                                    //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public void OnChanged(string path, bool lastVal, bool val) {           //__SILP__
            _Block(path, lastVal, val);                                        //__SILP__
        }                                                                      //__SILP__
    }                                                                          //__SILP__
                                                                               //__SILP__
    public class BoolProperty : Property<bool> {                               //__SILP__
        public override string Type {                                          //__SILP__
            get { return PropertiesConsts.TypeBoolProperty; }                  //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        protected override bool DoEncode(Data data) {                          //__SILP__
            return data.SetBool(PropertiesConsts.KeyValue, Value);             //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        protected override bool DoDecode(Data data) {                          //__SILP__
            SetValue(data.GetBool(PropertiesConsts.KeyValue));                 //__SILP__
            return true;                                                       //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        private bool _CheckingValue = false;                                   //__SILP__
        private bool _UpdatingValue = false;                                   //__SILP__
                                                                               //__SILP__
        public override bool SetValue(bool newVal) {                           //__SILP__
            if (_CheckingValue) return false;                                  //__SILP__
            if (_UpdatingValue) return false;                                  //__SILP__
                                                                               //__SILP__
            if (Value != newVal) {                                             //__SILP__
                if (_Checkers != null) {                                       //__SILP__
                    _CheckingValue = true;                                     //__SILP__
                    for (int i = 0; i < _Checkers.Count; i++) {                //__SILP__
                        if (!_Checkers[i].IsValid(Path, Value, newVal)) {      //__SILP__
                            _CheckingValue = false;                            //__SILP__
                            return false;                                      //__SILP__
                        }                                                      //__SILP__
                    }                                                          //__SILP__
                    _CheckingValue = false;                                    //__SILP__
                }                                                              //__SILP__
                _UpdatingValue = true;                                         //__SILP__
                bool lastVal = Value;                                          //__SILP__
                if (!base.SetValue(newVal)) {                                  //__SILP__
                    _UpdatingValue = false;                                    //__SILP__
                    return false;                                              //__SILP__
                }                                                              //__SILP__
                if (_Watchers != null) {                                       //__SILP__
                    for (int i = 0; i < _Watchers.Count; i++) {                //__SILP__
                        _Watchers[i].OnChanged(Path, lastVal, Value);          //__SILP__
                    }                                                          //__SILP__
                }                                                              //__SILP__
                _UpdatingValue = false;                                        //__SILP__
                return true;                                                   //__SILP__
            }                                                                  //__SILP__
            return false;                                                      //__SILP__
        }                                                                      //__SILP__
    }                                                                          //__SILP__
                                                                               //__SILP__
    //SILP: PROPERTY_CLASS(Int, int)
    public sealed class BlockIntValueChecker : ValueChecker<int> {           //__SILP__
        public delegate bool CheckerBlock(string path, int val, int newVal); //__SILP__
                                                                             //__SILP__
        private readonly CheckerBlock _Block;                                //__SILP__
                                                                             //__SILP__
        public BlockIntValueChecker(CheckerBlock block) {                    //__SILP__
            _Block = block;                                                  //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        public bool IsValid(string path, int val, int newVal) {              //__SILP__
            return _Block(path, val, newVal);                                //__SILP__
        }                                                                    //__SILP__
    }                                                                        //__SILP__
                                                                             //__SILP__
    public sealed class BlockIntValueWatcher : ValueWatcher<int> {           //__SILP__
        public delegate void WatcherBlock(string path, int val, int newVal); //__SILP__
                                                                             //__SILP__
        private readonly WatcherBlock _Block;                                //__SILP__
                                                                             //__SILP__
        public BlockIntValueWatcher(WatcherBlock block) {                    //__SILP__
            _Block = block;                                                  //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        public void OnChanged(string path, int lastVal, int val) {           //__SILP__
            _Block(path, lastVal, val);                                      //__SILP__
        }                                                                    //__SILP__
    }                                                                        //__SILP__
                                                                             //__SILP__
    public class IntProperty : Property<int> {                               //__SILP__
        public override string Type {                                        //__SILP__
            get { return PropertiesConsts.TypeIntProperty; }                 //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        protected override bool DoEncode(Data data) {                        //__SILP__
            return data.SetInt(PropertiesConsts.KeyValue, Value);            //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        protected override bool DoDecode(Data data) {                        //__SILP__
            SetValue(data.GetInt(PropertiesConsts.KeyValue));                //__SILP__
            return true;                                                     //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        private bool _CheckingValue = false;                                 //__SILP__
        private bool _UpdatingValue = false;                                 //__SILP__
                                                                             //__SILP__
        public override bool SetValue(int newVal) {                          //__SILP__
            if (_CheckingValue) return false;                                //__SILP__
            if (_UpdatingValue) return false;                                //__SILP__
                                                                             //__SILP__
            if (Value != newVal) {                                           //__SILP__
                if (_Checkers != null) {                                     //__SILP__
                    _CheckingValue = true;                                   //__SILP__
                    for (int i = 0; i < _Checkers.Count; i++) {              //__SILP__
                        if (!_Checkers[i].IsValid(Path, Value, newVal)) {    //__SILP__
                            _CheckingValue = false;                          //__SILP__
                            return false;                                    //__SILP__
                        }                                                    //__SILP__
                    }                                                        //__SILP__
                    _CheckingValue = false;                                  //__SILP__
                }                                                            //__SILP__
                _UpdatingValue = true;                                       //__SILP__
                int lastVal = Value;                                         //__SILP__
                if (!base.SetValue(newVal)) {                                //__SILP__
                    _UpdatingValue = false;                                  //__SILP__
                    return false;                                            //__SILP__
                }                                                            //__SILP__
                if (_Watchers != null) {                                     //__SILP__
                    for (int i = 0; i < _Watchers.Count; i++) {              //__SILP__
                        _Watchers[i].OnChanged(Path, lastVal, Value);        //__SILP__
                    }                                                        //__SILP__
                }                                                            //__SILP__
                _UpdatingValue = false;                                      //__SILP__
                return true;                                                 //__SILP__
            }                                                                //__SILP__
            return false;                                                    //__SILP__
        }                                                                    //__SILP__
    }                                                                        //__SILP__
                                                                             //__SILP__
    //SILP: PROPERTY_CLASS(Long, long)
    public sealed class BlockLongValueChecker : ValueChecker<long> {           //__SILP__
        public delegate bool CheckerBlock(string path, long val, long newVal); //__SILP__
                                                                               //__SILP__
        private readonly CheckerBlock _Block;                                  //__SILP__
                                                                               //__SILP__
        public BlockLongValueChecker(CheckerBlock block) {                     //__SILP__
            _Block = block;                                                    //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public bool IsValid(string path, long val, long newVal) {              //__SILP__
            return _Block(path, val, newVal);                                  //__SILP__
        }                                                                      //__SILP__
    }                                                                          //__SILP__
                                                                               //__SILP__
    public sealed class BlockLongValueWatcher : ValueWatcher<long> {           //__SILP__
        public delegate void WatcherBlock(string path, long val, long newVal); //__SILP__
                                                                               //__SILP__
        private readonly WatcherBlock _Block;                                  //__SILP__
                                                                               //__SILP__
        public BlockLongValueWatcher(WatcherBlock block) {                     //__SILP__
            _Block = block;                                                    //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public void OnChanged(string path, long lastVal, long val) {           //__SILP__
            _Block(path, lastVal, val);                                        //__SILP__
        }                                                                      //__SILP__
    }                                                                          //__SILP__
                                                                               //__SILP__
    public class LongProperty : Property<long> {                               //__SILP__
        public override string Type {                                          //__SILP__
            get { return PropertiesConsts.TypeLongProperty; }                  //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        protected override bool DoEncode(Data data) {                          //__SILP__
            return data.SetLong(PropertiesConsts.KeyValue, Value);             //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        protected override bool DoDecode(Data data) {                          //__SILP__
            SetValue(data.GetLong(PropertiesConsts.KeyValue));                 //__SILP__
            return true;                                                       //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        private bool _CheckingValue = false;                                   //__SILP__
        private bool _UpdatingValue = false;                                   //__SILP__
                                                                               //__SILP__
        public override bool SetValue(long newVal) {                           //__SILP__
            if (_CheckingValue) return false;                                  //__SILP__
            if (_UpdatingValue) return false;                                  //__SILP__
                                                                               //__SILP__
            if (Value != newVal) {                                             //__SILP__
                if (_Checkers != null) {                                       //__SILP__
                    _CheckingValue = true;                                     //__SILP__
                    for (int i = 0; i < _Checkers.Count; i++) {                //__SILP__
                        if (!_Checkers[i].IsValid(Path, Value, newVal)) {      //__SILP__
                            _CheckingValue = false;                            //__SILP__
                            return false;                                      //__SILP__
                        }                                                      //__SILP__
                    }                                                          //__SILP__
                    _CheckingValue = false;                                    //__SILP__
                }                                                              //__SILP__
                _UpdatingValue = true;                                         //__SILP__
                long lastVal = Value;                                          //__SILP__
                if (!base.SetValue(newVal)) {                                  //__SILP__
                    _UpdatingValue = false;                                    //__SILP__
                    return false;                                              //__SILP__
                }                                                              //__SILP__
                if (_Watchers != null) {                                       //__SILP__
                    for (int i = 0; i < _Watchers.Count; i++) {                //__SILP__
                        _Watchers[i].OnChanged(Path, lastVal, Value);          //__SILP__
                    }                                                          //__SILP__
                }                                                              //__SILP__
                _UpdatingValue = false;                                        //__SILP__
                return true;                                                   //__SILP__
            }                                                                  //__SILP__
            return false;                                                      //__SILP__
        }                                                                      //__SILP__
    }                                                                          //__SILP__
                                                                               //__SILP__
    //SILP: PROPERTY_CLASS(Float, float)
    public sealed class BlockFloatValueChecker : ValueChecker<float> {           //__SILP__
        public delegate bool CheckerBlock(string path, float val, float newVal); //__SILP__
                                                                                 //__SILP__
        private readonly CheckerBlock _Block;                                    //__SILP__
                                                                                 //__SILP__
        public BlockFloatValueChecker(CheckerBlock block) {                      //__SILP__
            _Block = block;                                                      //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
        public bool IsValid(string path, float val, float newVal) {              //__SILP__
            return _Block(path, val, newVal);                                    //__SILP__
        }                                                                        //__SILP__
    }                                                                            //__SILP__
                                                                                 //__SILP__
    public sealed class BlockFloatValueWatcher : ValueWatcher<float> {           //__SILP__
        public delegate void WatcherBlock(string path, float val, float newVal); //__SILP__
                                                                                 //__SILP__
        private readonly WatcherBlock _Block;                                    //__SILP__
                                                                                 //__SILP__
        public BlockFloatValueWatcher(WatcherBlock block) {                      //__SILP__
            _Block = block;                                                      //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
        public void OnChanged(string path, float lastVal, float val) {           //__SILP__
            _Block(path, lastVal, val);                                          //__SILP__
        }                                                                        //__SILP__
    }                                                                            //__SILP__
                                                                                 //__SILP__
    public class FloatProperty : Property<float> {                               //__SILP__
        public override string Type {                                            //__SILP__
            get { return PropertiesConsts.TypeFloatProperty; }                   //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
        protected override bool DoEncode(Data data) {                            //__SILP__
            return data.SetFloat(PropertiesConsts.KeyValue, Value);              //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
        protected override bool DoDecode(Data data) {                            //__SILP__
            SetValue(data.GetFloat(PropertiesConsts.KeyValue));                  //__SILP__
            return true;                                                         //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
        private bool _CheckingValue = false;                                     //__SILP__
        private bool _UpdatingValue = false;                                     //__SILP__
                                                                                 //__SILP__
        public override bool SetValue(float newVal) {                            //__SILP__
            if (_CheckingValue) return false;                                    //__SILP__
            if (_UpdatingValue) return false;                                    //__SILP__
                                                                                 //__SILP__
            if (Value != newVal) {                                               //__SILP__
                if (_Checkers != null) {                                         //__SILP__
                    _CheckingValue = true;                                       //__SILP__
                    for (int i = 0; i < _Checkers.Count; i++) {                  //__SILP__
                        if (!_Checkers[i].IsValid(Path, Value, newVal)) {        //__SILP__
                            _CheckingValue = false;                              //__SILP__
                            return false;                                        //__SILP__
                        }                                                        //__SILP__
                    }                                                            //__SILP__
                    _CheckingValue = false;                                      //__SILP__
                }                                                                //__SILP__
                _UpdatingValue = true;                                           //__SILP__
                float lastVal = Value;                                           //__SILP__
                if (!base.SetValue(newVal)) {                                    //__SILP__
                    _UpdatingValue = false;                                      //__SILP__
                    return false;                                                //__SILP__
                }                                                                //__SILP__
                if (_Watchers != null) {                                         //__SILP__
                    for (int i = 0; i < _Watchers.Count; i++) {                  //__SILP__
                        _Watchers[i].OnChanged(Path, lastVal, Value);            //__SILP__
                    }                                                            //__SILP__
                }                                                                //__SILP__
                _UpdatingValue = false;                                          //__SILP__
                return true;                                                     //__SILP__
            }                                                                    //__SILP__
            return false;                                                        //__SILP__
        }                                                                        //__SILP__
    }                                                                            //__SILP__
                                                                                 //__SILP__
    //SILP: PROPERTY_CLASS(Double, double)
    public sealed class BlockDoubleValueChecker : ValueChecker<double> {           //__SILP__
        public delegate bool CheckerBlock(string path, double val, double newVal); //__SILP__
                                                                                   //__SILP__
        private readonly CheckerBlock _Block;                                      //__SILP__
                                                                                   //__SILP__
        public BlockDoubleValueChecker(CheckerBlock block) {                       //__SILP__
            _Block = block;                                                        //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public bool IsValid(string path, double val, double newVal) {              //__SILP__
            return _Block(path, val, newVal);                                      //__SILP__
        }                                                                          //__SILP__
    }                                                                              //__SILP__
                                                                                   //__SILP__
    public sealed class BlockDoubleValueWatcher : ValueWatcher<double> {           //__SILP__
        public delegate void WatcherBlock(string path, double val, double newVal); //__SILP__
                                                                                   //__SILP__
        private readonly WatcherBlock _Block;                                      //__SILP__
                                                                                   //__SILP__
        public BlockDoubleValueWatcher(WatcherBlock block) {                       //__SILP__
            _Block = block;                                                        //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public void OnChanged(string path, double lastVal, double val) {           //__SILP__
            _Block(path, lastVal, val);                                            //__SILP__
        }                                                                          //__SILP__
    }                                                                              //__SILP__
                                                                                   //__SILP__
    public class DoubleProperty : Property<double> {                               //__SILP__
        public override string Type {                                              //__SILP__
            get { return PropertiesConsts.TypeDoubleProperty; }                    //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        protected override bool DoEncode(Data data) {                              //__SILP__
            return data.SetDouble(PropertiesConsts.KeyValue, Value);               //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        protected override bool DoDecode(Data data) {                              //__SILP__
            SetValue(data.GetDouble(PropertiesConsts.KeyValue));                   //__SILP__
            return true;                                                           //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        private bool _CheckingValue = false;                                       //__SILP__
        private bool _UpdatingValue = false;                                       //__SILP__
                                                                                   //__SILP__
        public override bool SetValue(double newVal) {                             //__SILP__
            if (_CheckingValue) return false;                                      //__SILP__
            if (_UpdatingValue) return false;                                      //__SILP__
                                                                                   //__SILP__
            if (Value != newVal) {                                                 //__SILP__
                if (_Checkers != null) {                                           //__SILP__
                    _CheckingValue = true;                                         //__SILP__
                    for (int i = 0; i < _Checkers.Count; i++) {                    //__SILP__
                        if (!_Checkers[i].IsValid(Path, Value, newVal)) {          //__SILP__
                            _CheckingValue = false;                                //__SILP__
                            return false;                                          //__SILP__
                        }                                                          //__SILP__
                    }                                                              //__SILP__
                    _CheckingValue = false;                                        //__SILP__
                }                                                                  //__SILP__
                _UpdatingValue = true;                                             //__SILP__
                double lastVal = Value;                                            //__SILP__
                if (!base.SetValue(newVal)) {                                      //__SILP__
                    _UpdatingValue = false;                                        //__SILP__
                    return false;                                                  //__SILP__
                }                                                                  //__SILP__
                if (_Watchers != null) {                                           //__SILP__
                    for (int i = 0; i < _Watchers.Count; i++) {                    //__SILP__
                        _Watchers[i].OnChanged(Path, lastVal, Value);              //__SILP__
                    }                                                              //__SILP__
                }                                                                  //__SILP__
                _UpdatingValue = false;                                            //__SILP__
                return true;                                                       //__SILP__
            }                                                                      //__SILP__
            return false;                                                          //__SILP__
        }                                                                          //__SILP__
    }                                                                              //__SILP__
                                                                                   //__SILP__
    //SILP: PROPERTY_CLASS(String, string)
    public sealed class BlockStringValueChecker : ValueChecker<string> {           //__SILP__
        public delegate bool CheckerBlock(string path, string val, string newVal); //__SILP__
                                                                                   //__SILP__
        private readonly CheckerBlock _Block;                                      //__SILP__
                                                                                   //__SILP__
        public BlockStringValueChecker(CheckerBlock block) {                       //__SILP__
            _Block = block;                                                        //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public bool IsValid(string path, string val, string newVal) {              //__SILP__
            return _Block(path, val, newVal);                                      //__SILP__
        }                                                                          //__SILP__
    }                                                                              //__SILP__
                                                                                   //__SILP__
    public sealed class BlockStringValueWatcher : ValueWatcher<string> {           //__SILP__
        public delegate void WatcherBlock(string path, string val, string newVal); //__SILP__
                                                                                   //__SILP__
        private readonly WatcherBlock _Block;                                      //__SILP__
                                                                                   //__SILP__
        public BlockStringValueWatcher(WatcherBlock block) {                       //__SILP__
            _Block = block;                                                        //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public void OnChanged(string path, string lastVal, string val) {           //__SILP__
            _Block(path, lastVal, val);                                            //__SILP__
        }                                                                          //__SILP__
    }                                                                              //__SILP__
                                                                                   //__SILP__
    public class StringProperty : Property<string> {                               //__SILP__
        public override string Type {                                              //__SILP__
            get { return PropertiesConsts.TypeStringProperty; }                    //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        protected override bool DoEncode(Data data) {                              //__SILP__
            return data.SetString(PropertiesConsts.KeyValue, Value);               //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        protected override bool DoDecode(Data data) {                              //__SILP__
            SetValue(data.GetString(PropertiesConsts.KeyValue));                   //__SILP__
            return true;                                                           //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        private bool _CheckingValue = false;                                       //__SILP__
        private bool _UpdatingValue = false;                                       //__SILP__
                                                                                   //__SILP__
        public override bool SetValue(string newVal) {                             //__SILP__
            if (_CheckingValue) return false;                                      //__SILP__
            if (_UpdatingValue) return false;                                      //__SILP__
                                                                                   //__SILP__
            if (Value != newVal) {                                                 //__SILP__
                if (_Checkers != null) {                                           //__SILP__
                    _CheckingValue = true;                                         //__SILP__
                    for (int i = 0; i < _Checkers.Count; i++) {                    //__SILP__
                        if (!_Checkers[i].IsValid(Path, Value, newVal)) {          //__SILP__
                            _CheckingValue = false;                                //__SILP__
                            return false;                                          //__SILP__
                        }                                                          //__SILP__
                    }                                                              //__SILP__
                    _CheckingValue = false;                                        //__SILP__
                }                                                                  //__SILP__
                _UpdatingValue = true;                                             //__SILP__
                string lastVal = Value;                                            //__SILP__
                if (!base.SetValue(newVal)) {                                      //__SILP__
                    _UpdatingValue = false;                                        //__SILP__
                    return false;                                                  //__SILP__
                }                                                                  //__SILP__
                if (_Watchers != null) {                                           //__SILP__
                    for (int i = 0; i < _Watchers.Count; i++) {                    //__SILP__
                        _Watchers[i].OnChanged(Path, lastVal, Value);              //__SILP__
                    }                                                              //__SILP__
                }                                                                  //__SILP__
                _UpdatingValue = false;                                            //__SILP__
                return true;                                                       //__SILP__
            }                                                                      //__SILP__
            return false;                                                          //__SILP__
        }                                                                          //__SILP__
    }                                                                              //__SILP__
                                                                                   //__SILP__
    //SILP: PROPERTY_CLASS(Data, Data)
    public sealed class BlockDataValueChecker : ValueChecker<Data> {           //__SILP__
        public delegate bool CheckerBlock(string path, Data val, Data newVal); //__SILP__
                                                                               //__SILP__
        private readonly CheckerBlock _Block;                                  //__SILP__
                                                                               //__SILP__
        public BlockDataValueChecker(CheckerBlock block) {                     //__SILP__
            _Block = block;                                                    //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public bool IsValid(string path, Data val, Data newVal) {              //__SILP__
            return _Block(path, val, newVal);                                  //__SILP__
        }                                                                      //__SILP__
    }                                                                          //__SILP__
                                                                               //__SILP__
    public sealed class BlockDataValueWatcher : ValueWatcher<Data> {           //__SILP__
        public delegate void WatcherBlock(string path, Data val, Data newVal); //__SILP__
                                                                               //__SILP__
        private readonly WatcherBlock _Block;                                  //__SILP__
                                                                               //__SILP__
        public BlockDataValueWatcher(WatcherBlock block) {                     //__SILP__
            _Block = block;                                                    //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public void OnChanged(string path, Data lastVal, Data val) {           //__SILP__
            _Block(path, lastVal, val);                                        //__SILP__
        }                                                                      //__SILP__
    }                                                                          //__SILP__
                                                                               //__SILP__
    public class DataProperty : Property<Data> {                               //__SILP__
        public override string Type {                                          //__SILP__
            get { return PropertiesConsts.TypeDataProperty; }                  //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        protected override bool DoEncode(Data data) {                          //__SILP__
            return data.SetData(PropertiesConsts.KeyValue, Value);             //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        protected override bool DoDecode(Data data) {                          //__SILP__
            SetValue(data.GetData(PropertiesConsts.KeyValue));                 //__SILP__
            return true;                                                       //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        private bool _CheckingValue = false;                                   //__SILP__
        private bool _UpdatingValue = false;                                   //__SILP__
                                                                               //__SILP__
        public override bool SetValue(Data newVal) {                           //__SILP__
            if (_CheckingValue) return false;                                  //__SILP__
            if (_UpdatingValue) return false;                                  //__SILP__
                                                                               //__SILP__
            if (Value != newVal) {                                             //__SILP__
                if (_Checkers != null) {                                       //__SILP__
                    _CheckingValue = true;                                     //__SILP__
                    for (int i = 0; i < _Checkers.Count; i++) {                //__SILP__
                        if (!_Checkers[i].IsValid(Path, Value, newVal)) {      //__SILP__
                            _CheckingValue = false;                            //__SILP__
                            return false;                                      //__SILP__
                        }                                                      //__SILP__
                    }                                                          //__SILP__
                    _CheckingValue = false;                                    //__SILP__
                }                                                              //__SILP__
                _UpdatingValue = true;                                         //__SILP__
                Data lastVal = Value;                                          //__SILP__
                if (!base.SetValue(newVal)) {                                  //__SILP__
                    _UpdatingValue = false;                                    //__SILP__
                    return false;                                              //__SILP__
                }                                                              //__SILP__
                if (_Watchers != null) {                                       //__SILP__
                    for (int i = 0; i < _Watchers.Count; i++) {                //__SILP__
                        _Watchers[i].OnChanged(Path, lastVal, Value);          //__SILP__
                    }                                                          //__SILP__
                }                                                              //__SILP__
                _UpdatingValue = false;                                        //__SILP__
                return true;                                                   //__SILP__
            }                                                                  //__SILP__
            return false;                                                      //__SILP__
        }                                                                      //__SILP__
    }                                                                          //__SILP__
                                                                               //__SILP__
}
