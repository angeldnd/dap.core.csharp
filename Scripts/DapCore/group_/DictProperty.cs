using System;
using System.Collections.Generic;
using System.Linq;

namespace angeldnd.dap {
    public abstract class DictProperty<T> : DictInBothAspect<IProperties, T>, IDictProperties, IProperty
                                                where T : class, IProperty {
        public Type ValueType {
            get { return typeof(T); }
        }

        private bool DoEncode(Data data) {
            Data values = new Data();
            if (!data.SetData(PropertiesConsts.KeyValue, values)) return false;

            return UntilFalse((T element) => {
                Data subData = element.Encode();
                return subData != null && values.SetData(element.Key, subData);
            });
        }

        private bool DecodeElements(Data data, Func<Data, string, IProperty>factory) {
            Clear();
            if (Count > 0) {
                Error("Orghan Elements Found: {0}", Count);
                return false;
            }
            Data values = data.GetData(PropertiesConsts.KeyValue);
            if (values == null) {
                return true;
            }
            foreach (var key in data.Keys) {
                IProperty prop = factory(values, key);
                if (prop == null) {
                    return false;
                }
            }
            return true;
        }

        private bool DoDecode(Data data) {
            return DecodeElements(data, (Data values, string key) => {
                Data subData = values.GetData(key);
                if (subData == null) {
                    Error("Invalid Elements Data: {0} -> {1}", key, values.GetValue(key));
                    return null;
                }
                return SpecHelper.AddPropertyWithSpec(this, key, subData);
            });
        }

        public bool DecodeValue(Data data) {
            if (data == null) return false;

            return DecodeElements(data, (Data values, string key) => {
                T element = Add(key);
                Data valueData = new Data();
                if (values.CopyValueTo(key, valueData, PropertiesConsts.KeyValue)) {
                    if (!element.DecodeValue(valueData)) {
                        element.Error("DecodeValue Failed: {0} ->\n{1}", key,
                                    Convertor.DataConvertor.Convert(valueData, "\t"));
                    }
                }
                return element;
            });
        }

        //SILP: GROUP_PROPERTY_MIXIN(DictProperty)
        public DictProperty(IDictProperties owner, string key) : base(owner, key) {       //__SILP__
        }                                                                                 //__SILP__
                                                                                          //__SILP__
        public DictProperty(ITableProperties owner, int index) : base(owner, index) {     //__SILP__
        }                                                                                 //__SILP__
                                                                                          //__SILP__
        //IProperty                                                                       //__SILP__
        public Data Encode() {                                                            //__SILP__
            if (!string.IsNullOrEmpty(DapType)) {                                         //__SILP__
                Data data = new Data();                                                   //__SILP__
                if (data.SetString(ObjectConsts.KeyDapType, DapType)) {                   //__SILP__
                    if (DoEncode(data)) {                                                 //__SILP__
                        return data;                                                      //__SILP__
                    }                                                                     //__SILP__
                }                                                                         //__SILP__
            }                                                                             //__SILP__
            if (LogDebug) Debug("Not Encodable!");                                        //__SILP__
            return null;                                                                  //__SILP__
        }                                                                                 //__SILP__
                                                                                          //__SILP__
        public bool Decode(Data data) {                                                   //__SILP__
            if (data == null) return false;                                               //__SILP__
            string dapType = data.GetString(ObjectConsts.KeyDapType);                     //__SILP__
            if (dapType == DapType) {                                                     //__SILP__
                return DoDecode(data);                                                    //__SILP__
            } else {                                                                      //__SILP__
                Error("Dap Type Mismatched: {0}, {1}", DapType, dapType);                 //__SILP__
            }                                                                             //__SILP__
            return false;                                                                 //__SILP__
        }                                                                                 //__SILP__
                                                                                          //__SILP__
        private void FireOnChanged() {                                                    //__SILP__
            WeakListHelper.Notify(_VarWatchers, (IVarWatcher watcher) => {                //__SILP__
                watcher.OnChanged(this);                                                  //__SILP__
            });                                                                           //__SILP__
        }                                                                                 //__SILP__
                                                                                          //__SILP__
        //IVar                                                                            //__SILP__
        public object GetValue() {                                                        //__SILP__
            Error("Not Supported");                                                       //__SILP__
            return null;                                                                  //__SILP__
        }                                                                                 //__SILP__
                                                                                          //__SILP__
        public bool SetValue(object newValue) {                                           //__SILP__
            Error("Not Supported");                                                       //__SILP__
            return false;                                                                 //__SILP__
        }                                                                                 //__SILP__
                                                                                          //__SILP__
        private WeakList<IVarWatcher> _VarWatchers = null;                                //__SILP__
                                                                                          //__SILP__
        public int VarWatcherCount {                                                      //__SILP__
            get { return WeakListHelper.Count(_VarWatchers); }                            //__SILP__
        }                                                                                 //__SILP__
                                                                                          //__SILP__
        public bool AddVarWatcher(IVarWatcher watcher) {                                  //__SILP__
            if (WeakListHelper.Add(ref _VarWatchers, watcher)){                           //__SILP__
                return true;                                                              //__SILP__
            }                                                                             //__SILP__
            return false;                                                                 //__SILP__
        }                                                                                 //__SILP__
                                                                                          //__SILP__
        public bool RemoveVarWatcher(IVarWatcher watcher) {                               //__SILP__
            if (WeakListHelper.Remove(_VarWatchers, watcher)) {                           //__SILP__
                return true;                                                              //__SILP__
            }                                                                             //__SILP__
            return false;                                                                 //__SILP__
        }                                                                                 //__SILP__
                                                                                          //__SILP__
        public BlockVarWatcher AddVarWatcher(IBlockOwner owner,                           //__SILP__
                                             Action<IVar> _watcher) {                     //__SILP__
            BlockVarWatcher watcher = new BlockVarWatcher(owner, _watcher);               //__SILP__
            if (AddVarWatcher(watcher)) {                                                 //__SILP__
                return watcher;                                                           //__SILP__
            }                                                                             //__SILP__
            return null;                                                                  //__SILP__
        }                                                                                 //__SILP__
                                                                                          //__SILP__
        public int ValueCheckerCount {                                                    //__SILP__
            get { return 0; }                                                             //__SILP__
        }                                                                                 //__SILP__
                                                                                          //__SILP__
        public void AllValueCheckers<T1>(Action<T1> callback) where T1 : IValueChecker {  //__SILP__
        }                                                                                 //__SILP__
                                                                                          //__SILP__
        public int ValueWatcherCount {                                                    //__SILP__
            get { return 0; }                                                             //__SILP__
        }                                                                                 //__SILP__
    }
}
