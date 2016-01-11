using System;
using System.Collections.Generic;

using angeldnd.dap;

namespace angeldnd.dap.binding {
    public sealed class Extension : Accessor<IContext>, IBlockOwner {
        public readonly string Key;

        public string GetSubKey(string fragment) {
            if (string.IsNullOrEmpty(Key)) {
                return fragment;
            } else {
                return string.Format("{0}{1}{2}", Key, EntityConsts.Separator, fragment);
            }
        }

        private Dictionary<string, Pass> _VarPasses;
        private Dictionary<string, Pass> _PropertyPasses;
        private Dictionary<string, Pass> _ChannelPasses;
        private Dictionary<string, Pass> _HandlerPasses;

        public delegate void SyncPropertyBlock(string key);
        private Dictionary<string, SyncPropertyBlock> _PropertySyncers;

        public Extension(IContext obj, string key) : base(obj) {
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

        public void ClearExtension() {
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

        public void SyncExtension() {
            if (_PropertySyncers != null) {
                foreach (var kv in _PropertySyncers) {
                    kv.Value(kv.Key);
                }
            }
        }

        public Channel SetupChannel(string fragment, Pass pass) {
            string key = GetSubKey(fragment);
            Channel channel = Obj.AddChannel(key, pass);
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
            Handler handler = Obj.AddHandler(key, pass);
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
            return Obj.FireEvent(key, pass, evt);
        }

        public bool FireEvent(string fragment, Data evt) {
            return FireEvent(fragment, null, evt);
        }

        public Data HandleRequest(string fragment, Pass pass, Data req) {
            string key = GetSubKey(fragment);
            return Obj.HandleRequest(key, pass, req);
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

        //SILP:BLOCK_OWNER()
        private List<WeakBlock> _Blocks = null;                       //__SILP__
                                                                      //__SILP__
        public void AddBlock(WeakBlock block) {                       //__SILP__
            if (_Blocks == null) {                                    //__SILP__
                _Blocks = new List<WeakBlock>();                      //__SILP__
            }                                                         //__SILP__
            if (!_Blocks.Contains(block)) {                           //__SILP__
                _Blocks.Add(block);                                   //__SILP__
            }                                                         //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public void RemoveBlock(WeakBlock block) {                    //__SILP__
            if (_Blocks == null) {                                    //__SILP__
                return;                                               //__SILP__
            }                                                         //__SILP__
            int index = _Blocks.IndexOf(block);                       //__SILP__
            if (index >= 0) {                                         //__SILP__
                _Blocks.RemoveAt(index);                              //__SILP__
            }                                                         //__SILP__
        }                                                             //__SILP__

        //SILP: EXTENSION_SETUP_PROPERTY(Bool, bool)
        public BoolProperty SetupBoolProperty(string fragment,                           //__SILP__
                Pass pass, Func<bool> getter,                                            //__SILP__
                Func<IVar<bool>, bool, bool> checker,                                    //__SILP__
                Action<IVar<bool>, bool> watcher) {                                      //__SILP__
            return SetupProperty<BoolProperty, bool>(PropertiesConsts.TypeBoolProperty,  //__SILP__
                fragment, pass, getter,                                                  //__SILP__
                checker == null ? null : new BlockValueChecker<bool>(this, checker),     //__SILP__
                watcher == null ? null : new BlockValueWatcher<bool>(this, watcher)      //__SILP__
            ) as BoolProperty;                                                           //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        public BoolProperty SetupBoolProperty(string fragment, Pass pass,                //__SILP__
                Func<bool> getter) {                                                     //__SILP__
            return SetupBoolProperty(fragment, pass, getter, null, null);                //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        public BoolProperty SetupBoolProperty(string fragment, Pass pass,                //__SILP__
                Func<bool> getter,                                                       //__SILP__
                Action<IVar<bool>, bool> watcher) {                                      //__SILP__
            return SetupBoolProperty(fragment, pass, getter, null, watcher);             //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        //SILP: EXTENSION_SETUP_PROPERTY(Int, int)
        public IntProperty SetupIntProperty(string fragment,                          //__SILP__
                Pass pass, Func<int> getter,                                          //__SILP__
                Func<IVar<int>, int, bool> checker,                                   //__SILP__
                Action<IVar<int>, int> watcher) {                                     //__SILP__
            return SetupProperty<IntProperty, int>(PropertiesConsts.TypeIntProperty,  //__SILP__
                fragment, pass, getter,                                               //__SILP__
                checker == null ? null : new BlockValueChecker<int>(this, checker),   //__SILP__
                watcher == null ? null : new BlockValueWatcher<int>(this, watcher)    //__SILP__
            ) as IntProperty;                                                         //__SILP__
        }                                                                             //__SILP__
                                                                                      //__SILP__
        public IntProperty SetupIntProperty(string fragment, Pass pass,               //__SILP__
                Func<int> getter) {                                                   //__SILP__
            return SetupIntProperty(fragment, pass, getter, null, null);              //__SILP__
        }                                                                             //__SILP__
                                                                                      //__SILP__
        public IntProperty SetupIntProperty(string fragment, Pass pass,               //__SILP__
                Func<int> getter,                                                     //__SILP__
                Action<IVar<int>, int> watcher) {                                     //__SILP__
            return SetupIntProperty(fragment, pass, getter, null, watcher);           //__SILP__
        }                                                                             //__SILP__
                                                                                      //__SILP__
        //SILP: EXTENSION_SETUP_PROPERTY(Long, long)
        public LongProperty SetupLongProperty(string fragment,                           //__SILP__
                Pass pass, Func<long> getter,                                            //__SILP__
                Func<IVar<long>, long, bool> checker,                                    //__SILP__
                Action<IVar<long>, long> watcher) {                                      //__SILP__
            return SetupProperty<LongProperty, long>(PropertiesConsts.TypeLongProperty,  //__SILP__
                fragment, pass, getter,                                                  //__SILP__
                checker == null ? null : new BlockValueChecker<long>(this, checker),     //__SILP__
                watcher == null ? null : new BlockValueWatcher<long>(this, watcher)      //__SILP__
            ) as LongProperty;                                                           //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        public LongProperty SetupLongProperty(string fragment, Pass pass,                //__SILP__
                Func<long> getter) {                                                     //__SILP__
            return SetupLongProperty(fragment, pass, getter, null, null);                //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        public LongProperty SetupLongProperty(string fragment, Pass pass,                //__SILP__
                Func<long> getter,                                                       //__SILP__
                Action<IVar<long>, long> watcher) {                                      //__SILP__
            return SetupLongProperty(fragment, pass, getter, null, watcher);             //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        //SILP: EXTENSION_SETUP_PROPERTY(Float, float)
        public FloatProperty SetupFloatProperty(string fragment,                            //__SILP__
                Pass pass, Func<float> getter,                                              //__SILP__
                Func<IVar<float>, float, bool> checker,                                     //__SILP__
                Action<IVar<float>, float> watcher) {                                       //__SILP__
            return SetupProperty<FloatProperty, float>(PropertiesConsts.TypeFloatProperty,  //__SILP__
                fragment, pass, getter,                                                     //__SILP__
                checker == null ? null : new BlockValueChecker<float>(this, checker),       //__SILP__
                watcher == null ? null : new BlockValueWatcher<float>(this, watcher)        //__SILP__
            ) as FloatProperty;                                                             //__SILP__
        }                                                                                   //__SILP__
                                                                                            //__SILP__
        public FloatProperty SetupFloatProperty(string fragment, Pass pass,                 //__SILP__
                Func<float> getter) {                                                       //__SILP__
            return SetupFloatProperty(fragment, pass, getter, null, null);                  //__SILP__
        }                                                                                   //__SILP__
                                                                                            //__SILP__
        public FloatProperty SetupFloatProperty(string fragment, Pass pass,                 //__SILP__
                Func<float> getter,                                                         //__SILP__
                Action<IVar<float>, float> watcher) {                                       //__SILP__
            return SetupFloatProperty(fragment, pass, getter, null, watcher);               //__SILP__
        }                                                                                   //__SILP__
                                                                                            //__SILP__
        //SILP: EXTENSION_SETUP_PROPERTY(Double, double)
        public DoubleProperty SetupDoubleProperty(string fragment,                             //__SILP__
                Pass pass, Func<double> getter,                                                //__SILP__
                Func<IVar<double>, double, bool> checker,                                      //__SILP__
                Action<IVar<double>, double> watcher) {                                        //__SILP__
            return SetupProperty<DoubleProperty, double>(PropertiesConsts.TypeDoubleProperty,  //__SILP__
                fragment, pass, getter,                                                        //__SILP__
                checker == null ? null : new BlockValueChecker<double>(this, checker),         //__SILP__
                watcher == null ? null : new BlockValueWatcher<double>(this, watcher)          //__SILP__
            ) as DoubleProperty;                                                               //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
        public DoubleProperty SetupDoubleProperty(string fragment, Pass pass,                  //__SILP__
                Func<double> getter) {                                                         //__SILP__
            return SetupDoubleProperty(fragment, pass, getter, null, null);                    //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
        public DoubleProperty SetupDoubleProperty(string fragment, Pass pass,                  //__SILP__
                Func<double> getter,                                                           //__SILP__
                Action<IVar<double>, double> watcher) {                                        //__SILP__
            return SetupDoubleProperty(fragment, pass, getter, null, watcher);                 //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
        //SILP: EXTENSION_SETUP_PROPERTY(String, string)
        public StringProperty SetupStringProperty(string fragment,                             //__SILP__
                Pass pass, Func<string> getter,                                                //__SILP__
                Func<IVar<string>, string, bool> checker,                                      //__SILP__
                Action<IVar<string>, string> watcher) {                                        //__SILP__
            return SetupProperty<StringProperty, string>(PropertiesConsts.TypeStringProperty,  //__SILP__
                fragment, pass, getter,                                                        //__SILP__
                checker == null ? null : new BlockValueChecker<string>(this, checker),         //__SILP__
                watcher == null ? null : new BlockValueWatcher<string>(this, watcher)          //__SILP__
            ) as StringProperty;                                                               //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
        public StringProperty SetupStringProperty(string fragment, Pass pass,                  //__SILP__
                Func<string> getter) {                                                         //__SILP__
            return SetupStringProperty(fragment, pass, getter, null, null);                    //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
        public StringProperty SetupStringProperty(string fragment, Pass pass,                  //__SILP__
                Func<string> getter,                                                           //__SILP__
                Action<IVar<string>, string> watcher) {                                        //__SILP__
            return SetupStringProperty(fragment, pass, getter, null, watcher);                 //__SILP__
        }                                                                                      //__SILP__
                                                                                               //__SILP__
        //SILP: EXTENSION_SETUP_PROPERTY(Data, Data)
        public DataProperty SetupDataProperty(string fragment,                           //__SILP__
                Pass pass, Func<Data> getter,                                            //__SILP__
                Func<IVar<Data>, Data, bool> checker,                                    //__SILP__
                Action<IVar<Data>, Data> watcher) {                                      //__SILP__
            return SetupProperty<DataProperty, Data>(PropertiesConsts.TypeDataProperty,  //__SILP__
                fragment, pass, getter,                                                  //__SILP__
                checker == null ? null : new BlockValueChecker<Data>(this, checker),     //__SILP__
                watcher == null ? null : new BlockValueWatcher<Data>(this, watcher)      //__SILP__
            ) as DataProperty;                                                           //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        public DataProperty SetupDataProperty(string fragment, Pass pass,                //__SILP__
                Func<Data> getter) {                                                     //__SILP__
            return SetupDataProperty(fragment, pass, getter, null, null);                //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        public DataProperty SetupDataProperty(string fragment, Pass pass,                //__SILP__
                Func<Data> getter,                                                       //__SILP__
                Action<IVar<Data>, Data> watcher) {                                      //__SILP__
            return SetupDataProperty(fragment, pass, getter, null, watcher);             //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
    }
}
