using System;
using System.Collections.Generic;

using angeldnd.dap;

namespace angeldnd.dap {
    public sealed class Extra : Accessor<IContext>, IBlockOwner {
        public readonly string Key;

        public string GetSubKey(string fragment) {
            return DictConsts.Encode(Key, fragment);
        }

        private List<string> _VarKeys;
        public int VarsCount {
            get { return _VarKeys == null ? 0 : _VarKeys.Count; }
        }

        private List<string> _PropertyKeys;
        public int PropertiesCount {
            get { return _PropertyKeys == null ? 0 : _PropertyKeys.Count; }
        }

        private List<string> _ChannelKeys;
        public int ChannelsCount {
            get { return _ChannelKeys == null ? 0 : _ChannelKeys.Count; }
        }

        private List<string> _HandlerKeys;
        public int HandlersCount {
            get { return _HandlerKeys == null ? 0 : _HandlerKeys.Count; }
        }

        private void AddAspectPathes(List<string> pathes, IDict topAspect, List<string> keys) {
            if (keys != null) {
                foreach (string key in keys) {
                    var element = topAspect.Get<IInDictElement>(key);
                    if (element != null) {
                        IAspect aspect = element.As<IAspect>();
                        if (aspect != null) {
                            pathes.Add(aspect.Path);
                        }
                    }
                }
            }
        }

        public List<string> GetAspectPathes() {
            List<string> result = new List<string>();
            AddAspectPathes(result, Obj.Vars, _VarKeys);
            AddAspectPathes(result, Obj.Properties, _PropertyKeys);
            AddAspectPathes(result, Obj.Channels, _ChannelKeys);
            AddAspectPathes(result, Obj.Handlers, _HandlerKeys);
            return result;
        }

        private Dictionary<string, Action<string>> _PropertySyncers;

        public Extra(IContext obj, string key) : base(obj) {
            Key = key;
        }

        private void SaveVarKey(string key) {
            if (_VarKeys == null) _VarKeys = new List<string>();
            _VarKeys.Add(key);
        }

        private void SavePropertyKey(string key) {
            if (_PropertyKeys == null) _PropertyKeys = new List<string>();
            _PropertyKeys.Add(key);
        }

        private void SaveChannelKey(string key) {
            if (_ChannelKeys == null) _ChannelKeys = new List<string>();
            _ChannelKeys.Add(key);
        }

        private void SaveHandlerKey(string key) {
            if (_HandlerKeys == null) _HandlerKeys = new List<string>();
            _HandlerKeys.Add(key);
        }

        private void SavePropertySyncer<TP, T>(string key, TP property, Func<T> getter) where TP : IProperty<T> {
            if (_PropertySyncers == null) _PropertySyncers = new Dictionary<string, Action<string>>();

            _PropertySyncers[key] = (string _key) => {
                T val = getter();
                if (!property.SetValue(val)) {
                    Error("Sync Property Faild: {0}: {1} -> {2}", _key, property.Value, val);
                }
            };
        }

        public void ClearExtra() {
            if (_HandlerKeys != null) {
                foreach (string key in _HandlerKeys) {
                    Obj.Handlers.Remove(key);
                }
                _HandlerKeys.Clear();
            }
            if (_ChannelKeys != null) {
                foreach (string key in _ChannelKeys) {
                    Obj.Channels.Remove(key);
                }
                _ChannelKeys.Clear();
            }
            if (_PropertyKeys != null) {
                foreach (string key in _PropertyKeys) {
                    Obj.Properties.Remove(key);
                }
                _PropertyKeys.Clear();
            }
            if (_PropertySyncers != null) {
                _PropertySyncers.Clear();
            }
            if (_VarKeys != null) {
                foreach (string key in _VarKeys) {
                    Obj.Vars.Remove(key);
                }
                _VarKeys.Clear();
            }
        }

        public void SyncExtra() {
            if (_PropertySyncers != null) {
                bool profiling = Log.Profiler == null ? false : Log.Profiler.BeginSample(Key == null ? "Extra.SyncExtra" : Key);
                var en = _PropertySyncers.GetEnumerator();
                while (en.MoveNext()) {
                    if (profiling) Log.Profiler.BeginSample(en.Current.Key);
                    en.Current.Value(en.Current.Key);
                    if (profiling) Log.Profiler.EndSample();
                }
                if (profiling) Log.Profiler.EndSample();
            }
        }

        public Channel SetupChannel(string fragment) {
            string key = GetSubKey(fragment);
            Channel channel = Obj.Channels.Add(key);
            if (channel != null) {
                SaveChannelKey(key);
            }
            return channel;
        }

        public Handler SetupHandler(string fragment) {
            string key = GetSubKey(fragment);
            Handler handler = Obj.Handlers.Add(key);
            if (handler != null) {
                SaveHandlerKey(key);
            }
            return handler;
        }

        public bool FireEvent(string fragment, Data evt = null) {
            string key = GetSubKey(fragment);
            return Obj.Channels.FireEvent(key, evt);
        }

        public Data HandleRequest(string fragment, Data req = null) {
            string key = GetSubKey(fragment);
            return Obj.Handlers.HandleRequest(key, req);
        }

        public Var<T> SetupVar<T>(string fragment, T val, Action<IVar<T>, T> watcher = null) {
            string key = GetSubKey(fragment);
            Var<T> v = Obj.Vars.AddVar<T>(key, val);
            if (v != null) {
                SaveVarKey(key);
                if (watcher != null && v.AddValueWatcher(this, watcher) == null) {
                    Error("Add Watcher Failed: {0} -> {1}, {2}", this, typeof(T).FullName, fragment);
                }
            }
            return v;
        }

        public TP SetupProperty<TP>(string type, string fragment) where TP : class, IProperty {
            string key = GetSubKey(fragment);
            TP prop = Obj.Properties.New<TP>(type, key);
            if (prop != null) {
                SavePropertyKey(key);
            }
            return prop;
        }

        public TP SetupProperty<TP>(string fragment) where TP : class, IProperty {
            string dapType = DapType.GetDapType(typeof(TP));
            if (dapType == null) {
                Error("SetupProperty Failed, DapType Not Defined: " + typeof(TP));
                return null;
            }
            return SetupProperty<TP>(dapType, fragment);
        }

        public TP SetupProperty<TP, T>(string type, string fragment,
                Func<T> getter,
                IValueChecker<T> checker,
                IValueWatcher<T> watcher) 
                    where TP : class, IProperty<T> {
            string key = GetSubKey(fragment);

            TP prop = SetupProperty<TP>(type, fragment);

            if (prop != null) {
                T val = getter();
                prop.Setup(val);
                SavePropertySyncer<TP, T>(key, prop, getter);
                if (checker != null && !prop.AddValueChecker(checker)) {
                    Error("Add Checker Failed: {0} -> {1}, {2}", this, type, fragment);
                }
                if (watcher != null && !prop.AddValueWatcher(watcher)) {
                    Error("Add Watcher Failed: {0} -> {1}, {2}", this, type, fragment);
                }
            }
            return prop;
        }

        public TP SetupProperty<TP, T>(string fragment,
                Func<T> getter,
                IValueChecker<T> checker,
                IValueWatcher<T> watcher) 
                    where TP : class, IProperty<T> {
            string dapType = DapType.GetDapType(typeof(TP));
            if (dapType == null) {
                Error("SetupProperty Failed, DapType Not Defined: " + typeof(TP));
                return null;
            }
            return SetupProperty<TP, T>(dapType, fragment, getter, checker, watcher);
        }
    }
}
