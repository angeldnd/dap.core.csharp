using System;
using System.Collections.Generic;
using System.Linq;

namespace angeldnd.dap {
    public class TableProperty<T> : TableInBothAspect<IProperties, T>, ITableProperties, IProperty
                                                where T : class, IProperty {
        public Type ValueType {
            get { return typeof(T); }
        }

        private Data DoEncodeValue(bool fullMode) {
            Data values = new RealData();

            bool ok = UntilFalse((T element) => {
                if (fullMode) {
                    Data subData = element.Encode();
                    return subData != null && values.SetData(element.Index.ToString(), subData);
                } else {
                    Data subData = element.EncodeValue();
                    return subData != null && subData.CopyValueTo(PropertiesConsts.KeyValue, values, element.Index.ToString());
                }
            });
            return ok ? values : null;
        }

        private bool DecodeElements(Data values, Func<string, IProperty>factory) {
            Clear();
            if (Count > 0) {
                Error("Orghan Elements Found: {0}", Count);
                return false;
            }
            if (values == null) {
                return true;
            }
            for (int i = 0; i < values.Count; i++) {
                string key = i.ToString();
                IProperty prop = factory(key);
                if (prop == null) {
                    return false;
                }
                if (prop.Index != i) {
                    Error("Index Mismatched: {0} -> {1}", key, prop.Index);
                    return false;
                }
            }
            return true;
        }

        private bool DoDecodeValue(bool fullMode, Data values) {
            return DecodeElements(values, (string key) => {
                if (fullMode) {
                    Data subData = values.GetData(key);
                    if (subData == null) {
                        Error("Invalid Elements Data: {0} -> {1}", key, values.GetValue(key));
                        return null;
                    }
                    return SpecHelper.AddPropertyWithSpec(this, subData);
                } else {
                    T element = Add();
                    Data valueData = new RealData();
                    if (values.CopyValueTo(key, valueData, PropertiesConsts.KeyValue)) {
                        if (!element.DecodeValue(valueData)) {
                            element.Error("DecodeValue Failed: {0} ->\n{1}", key,
                                        Convertor.DataConvertor.Convert(valueData, "\t"));
                        }
                    }
                    return element;
                }
            });
        }

        private ITableWatcher<T> _WatcherWrapper = null;

        private void CheckWatcherWrapper() {
            if (_WatcherWrapper == null) {
                _WatcherWrapper = new BlockTableWatcher<T>(this, OnPropertyChanged, OnPropertyChanged, OnPropertyChanged);
                AddTableWatcher(_WatcherWrapper);
            }
        }

        private void OnPropertyChanged(IProperty prop) {
            FireOnChanged();
        }

        //SILP: GROUP_PROPERTY_MIXIN(TableProperty)
        public TableProperty(IDictProperties owner, string key) : base(owner, key) {               //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        public TableProperty(ITableProperties owner, int index) : base(owner, index) {             //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        //IProperty                                                                                //__SILP__
        public Data Encode() {                                                                     //__SILP__
            if (!string.IsNullOrEmpty(DapType)) {                                                  //__SILP__
                Data data = EncodeValue(true);                                                     //__SILP__
                if (data.SetString(ObjectConsts.KeyDapType, DapType)) {                            //__SILP__
                    return data;                                                                   //__SILP__
                }                                                                                  //__SILP__
            }                                                                                      //__SILP__
            if (LogDebug) Debug("Not Encodable!");                                                 //__SILP__
            return null;                                                                           //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        private Data EncodeValue(bool fullMode) {                                                  //__SILP__
            Data data = new RealData();                                                                //__SILP__
            if (data.SetData(PropertiesConsts.KeyValue, DoEncodeValue(fullMode))) {                //__SILP__
                return data;                                                                       //__SILP__
            }                                                                                      //__SILP__
            return null;                                                                           //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        public Data EncodeValue() {                                                                //__SILP__
            return EncodeValue(false);                                                             //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        private bool DecodeValue(bool fullMode, Data data) {                                       //__SILP__
            if (data == null) return false;                                                        //__SILP__
            Data v = data.GetData(PropertiesConsts.KeyValue);                                      //__SILP__
            if (v == null) return false;                                                           //__SILP__
                                                                                                   //__SILP__
            return DoDecodeValue(fullMode, v);                                                     //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        public bool DecodeValue(Data data) {                                                       //__SILP__
            return DecodeValue(false, data);                                                       //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        public bool Decode(Data data) {                                                            //__SILP__
            if (data == null) return false;                                                        //__SILP__
            string dapType = data.GetString(ObjectConsts.KeyDapType);                              //__SILP__
            if (dapType == DapType) {                                                              //__SILP__
                return DecodeValue(true, data);                                                    //__SILP__
            } else {                                                                               //__SILP__
                Error("Dap Type Mismatched: {0}, {1}", DapType, dapType);                          //__SILP__
            }                                                                                      //__SILP__
            return false;                                                                          //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        public void FireOnChanged() {                                                              //__SILP__
            WeakListHelper.Notify(_VarWatchers, (IVarWatcher watcher) => {                         //__SILP__
                watcher.OnChanged(this);                                                           //__SILP__
            });                                                                                    //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        //IVar                                                                                     //__SILP__
        public object GetValue() {                                                                 //__SILP__
            Error("Not Supported");                                                                //__SILP__
            return null;                                                                           //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        public bool SetValue(object newValue) {                                                    //__SILP__
            Error("Not Supported");                                                                //__SILP__
            return false;                                                                          //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        private WeakList<IVarWatcher> _VarWatchers = null;                                         //__SILP__
                                                                                                   //__SILP__
        public int VarWatcherCount {                                                               //__SILP__
            get { return WeakListHelper.Count(_VarWatchers); }                                     //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        public bool AddVarWatcher(IVarWatcher watcher) {                                           //__SILP__
            if (WeakListHelper.Add(ref _VarWatchers, watcher)){                                    //__SILP__
                CheckWatcherWrapper();                                                             //__SILP__
                return true;                                                                       //__SILP__
            }                                                                                      //__SILP__
            return false;                                                                          //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        public bool RemoveVarWatcher(IVarWatcher watcher) {                                        //__SILP__
            if (WeakListHelper.Remove(_VarWatchers, watcher)) {                                    //__SILP__
                return true;                                                                       //__SILP__
            }                                                                                      //__SILP__
            return false;                                                                          //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        public BlockVarWatcher AddVarWatcher(IBlockOwner owner,                                    //__SILP__
                                             Action<IVar> _watcher) {                              //__SILP__
            BlockVarWatcher watcher = new BlockVarWatcher(owner, _watcher);                        //__SILP__
            if (AddVarWatcher(watcher)) {                                                          //__SILP__
                return watcher;                                                                    //__SILP__
            }                                                                                      //__SILP__
            return null;                                                                           //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        public int ValueCheckerCount {                                                             //__SILP__
            get { return 0; }                                                                      //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        public void AllValueCheckers<T1>(Action<T1> callback) where T1 : IValueChecker {           //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        public int ValueWatcherCount {                                                             //__SILP__
            get { return 0; }                                                                      //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        //ISetupAspect                                                                             //__SILP__
        public int SetupWatcherCount {                                                             //__SILP__
            get { return 0; }                                                                      //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        public bool AddSetupWatcher(ISetupWatcher watcher) {                                       //__SILP__
            return false;                                                                          //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        public bool RemoveSetupWatcher(ISetupWatcher watcher) {                                    //__SILP__
            return false;                                                                          //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        public BlockSetupWatcher AddSetupWatcher(IBlockOwner owner, Action<ISetupAspect> block) {  //__SILP__
            return null;                                                                           //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        public bool NeedSetup() {                                                                  //__SILP__
            return false;                                                                          //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
    }
}
