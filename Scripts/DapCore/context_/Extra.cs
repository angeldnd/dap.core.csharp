using System;
using System.Collections.Generic;

using angeldnd.dap;

namespace angeldnd.dap.binding {
    public sealed class Extra : Accessor<IContext>, IBlockOwner {
        public readonly string Key;

        public string GetSubKey(string fragment) {
            if (string.IsNullOrEmpty(Key)) {
                return fragment;
            } else {
                return string.Format("{0}{1}{2}", Key, TreeConsts.Separator, fragment);
            }
        }

        private Dictionary<string, Pass> _VarPasses;
        private Dictionary<string, Pass> _PropertyPasses;
        private Dictionary<string, Pass> _ChannelPasses;
        private Dictionary<string, Pass> _HandlerPasses;

        public delegate void SyncPropertyBlock(string key);
        private Dictionary<string, SyncPropertyBlock> _PropertySyncers;

        public Extra(IContext obj, string key) : base(obj) {
            Key = key;
        }

        private void SaveVarPass(string key, Pass pass) {
            if (_VarPasses == null) _VarPasses = new Dictionary<string, Pass>();
            _VarPasses[key] = pass;
        }

        private void SavePropertyPass(string key, Pass pass) {
            if (_PropertyPasses == null) _PropertyPasses = new Dictionary<string, Pass>();
            _PropertyPasses[key] = pass;
        }

        private void SaveChannelPass(string key, Pass pass) {
            if (_ChannelPasses == null) _ChannelPasses = new Dictionary<string, Pass>();
            _ChannelPasses[key] = pass;
        }

        private void SaveHandlerPass(string key, Pass pass) {
            if (_HandlerPasses == null) _HandlerPasses = new Dictionary<string, Pass>();
            _HandlerPasses[key] = pass;
        }

        private void SavePropertySyncer<TP, T>(string key, TP property, Func<T> getter) where TP : IProperty<T> {
            if (_PropertySyncers == null) _PropertySyncers = new Dictionary<string, SyncPropertyBlock>();

            _PropertySyncers[key] = (string _key) => {
                Pass pass = null;
                if (_PropertyPasses.TryGetValue(_key, out pass)) {
                    T val = getter();
                    if (!property.SetValue(pass, val)) {
                        Error("Sync Property Faild: {0}: {1} -> {2}", _key, property.Value, val);
                    }
                } else {
                    Critical("Pass Not Found: {0}", _key);
                }
            };
        }

        public void ClearExtra() {
            if (_HandlerPasses != null) {
                foreach (var kv in _HandlerPasses) {
                    Obj.Handlers.Remove(kv.Key, kv.Value);
                }
                _HandlerPasses.Clear();
            }
            if (_ChannelPasses != null) {
                foreach (var kv in _ChannelPasses) {
                    Obj.Channels.Remove(kv.Key, kv.Value);
                }
                _ChannelPasses.Clear();
            }
            if (_PropertyPasses != null) {
                foreach (var kv in _PropertyPasses) {
                    Obj.Properties.Remove(kv.Key, kv.Value);
                }
                _PropertyPasses.Clear();
            }
            if (_PropertySyncers != null) {
                _PropertySyncers.Clear();
            }
            if (_VarPasses != null) {
                foreach (var kv in _VarPasses) {
                    Obj.Vars.Remove(kv.Key, kv.Value);
                }
                _VarPasses.Clear();
            }
        }

        public void SyncExtra() {
            if (_PropertySyncers != null) {
                foreach (var kv in _PropertySyncers) {
                    kv.Value(kv.Key);
                }
            }
        }

        public Channel SetupChannel(string fragment, Pass pass) {
            string key = GetSubKey(fragment);
            Channel channel = Obj.Channels.AddChannel(key, pass);
            if (channel != null) {
                SaveChannelPass(key, pass);
            }
            return channel;
        }

        public Channel SetupChannel(string fragment) {
            return SetupChannel(fragment, null);
        }

        public Handler SetupHandler(string fragment, Pass pass) {
            string key = GetSubKey(fragment);
            Handler handler = Obj.Handlers.AddHandler(key, pass);
            if (handler != null) {
                SaveHandlerPass(key, pass);
            }
            return handler;
        }

        public Handler SetupHandler(string fragment) {
            return SetupHandler(fragment, null);
        }

        public bool FireEvent(string fragment, Pass pass, Data evt) {
            string key = GetSubKey(fragment);
            return Obj.Channels.FireEvent(key, pass, evt);
        }

        public bool FireEvent(string fragment, Data evt) {
            return FireEvent(fragment, null, evt);
        }

        public Data HandleRequest(string fragment, Pass pass, Data req) {
            string key = GetSubKey(fragment);
            return Obj.Handlers.HandleRequest(key, pass, req);
        }

        public Data HandleRequest(string fragment, Data req) {
            return HandleRequest(fragment, null, req);
        }

        public Var<T> SetupVar<T>(string fragment, Pass pass, T val, Action<IVar> watcher) {
            string key = GetSubKey(fragment);
            Var<T> v = Obj.Vars.AddVar<T>(key, pass, val);
            if (v != null) {
                if (watcher != null && !v.AddVarWatcher(new BlockVarWatcher(this, watcher))) {
                    Error("Add Watcher Failed: {0} -> {1}, {2}", this, typeof(T).FullName, fragment);
                }
            }
            return v;
        }

        public Var<T> SetupVar<T>(string fragment, Pass pass, T val) {
            return SetupVar<T>(fragment, pass, val, null);
        }

        public TP SetupProperty<TP, T>(string type, string fragment, Pass pass) where TP : class, IProperty<T> {
            string key = GetSubKey(fragment);
            TP prop = Obj.Properties.New<TP>(type, key, pass);
            if (prop != null) {
                SavePropertyPass(key, pass);
            }
            return prop;
        }

        public TP SetupProperty<TP, T>(string type, string fragment,
                Pass pass, Func<T> getter,
                IValueChecker<T> checker,
                IValueWatcher<T> watcher) 
                    where TP : class, IProperty<T> {
            string key = GetSubKey(fragment);

            TP prop = SetupProperty<TP, T>(type, fragment, pass);

            if (prop != null) {
                T val = getter();
                prop.Setup(pass, val);
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
    }
}
