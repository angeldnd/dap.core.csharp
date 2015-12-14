using System;
using System.Collections.Generic;

using angeldnd.dap;
using angeldnd.dap.util;

namespace angeldnd.dap.binding {
    public sealed class Extension : BaseContextAccessor {
        private string _Key = null;
        public string Key {
            get { return _Key; }
        }

        public string GetSubKey(string fragment) {
            if (string.IsNullOrEmpty(_Key)) {
                return fragment;
            } else {
                return string.Format("{0}{1}{2}", Key, EntityConsts.Separator, fragment);
            }
        }

        public bool Setup(Context context, string key) {
            if (_Key == null) {
                _Key = key;
                return base.Setup(context);
            } else {
                Error("Already Setup: Context = {0}, _Key = {1}, context = {2}, key = {3}",
                        Object, _Key, context, key);
            }
            return false;
        }

        private Dictionary<string, Pass> _VarPasses;
        private Dictionary<string, Pass> _PropertyPasses;
        private Dictionary<string, Pass> _ChannelPasses;
        private Dictionary<string, Pass> _HandlerPasses;

        public delegate void SyncPropertyBlock(string key);
        private Dictionary<string, SyncPropertyBlock> _PropertySyncers;

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

        private void SavePropertySyncer<T>(string key, Property<T> property, Property<T>.GetterBlock getter) {
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
            if (Context == null) return;

            if (_HandlerPasses != null) {
                foreach (var kv in _HandlerPasses) {
                    Context.Handlers.Remove<Handler>(kv.Key, kv.Value);
                }
                _HandlerPasses.Clear();
            }
            if (_ChannelPasses != null) {
                foreach (var kv in _ChannelPasses) {
                    Context.Channels.Remove<Channel>(kv.Key, kv.Value);
                }
                _ChannelPasses.Clear();
            }
            if (_PropertyPasses != null) {
                foreach (var kv in _PropertyPasses) {
                    Context.Properties.Remove<Property>(kv.Key, kv.Value);
                }
                _PropertyPasses.Clear();
            }
            if (_PropertySyncers != null) {
                _PropertySyncers.Clear();
            }
            if (_VarPasses != null) {
                foreach (var kv in _VarPasses) {
                    Context.Vars.Remove<Var>(kv.Key, kv.Value);
                }
                _VarPasses.Clear();
            }
        }

        public void SyncExtension() {
            if (Context == null) return;

            if (_PropertySyncers != null) {
                foreach (var kv in _PropertySyncers) {
                    kv.Value(kv.Key);
                }
            }
        }

        public Channel SetupChannel(string fragment, Pass pass) {
            string key = GetSubKey(fragment);
            Channel channel = Context.AddChannel(key, pass);
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
            Handler handler = Context.AddHandler(key, pass);
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
            return Context.FireEvent(key, pass, evt);
        }

        public bool FireEvent(string fragment, Data evt) {
            return FireEvent(fragment, null, evt);
        }

        public Data HandleRequest(string fragment, Pass pass, Data req) {
            string key = GetSubKey(fragment);
            return Context.HandleRequest(key, pass, req);
        }

        public Data HandleRequest(string fragment, Data req) {
            return HandleRequest(fragment, null, req);
        }

        public Var<T> SetupVar<T>(string fragment, Pass pass, T val, BlockVarWatcher.WatcherBlock  watcher) {
            string key = GetSubKey(fragment);
            Var<T> v = Context.Vars.AddVar<T>(key, pass, val);
            if (v != null) {
                if (watcher != null && !v.AddVarWatcher(new BlockVarWatcher(watcher))) {
                    Error("Add Watcher Failed: {0} -> {1}, {2}", this, typeof(T).FullName, fragment);
                }
            }
            return v;
        }

        public Var<T> SetupVar<T>(string fragment, Pass pass, T val) {
            return SetupVar<T>(fragment, pass, val, null);
        }

        public T SetupProperty<T>(string type, string fragment, Pass pass) where T : class, Property {
            string key = GetSubKey(fragment);
            Aspect _prop = Context.Properties.Add(key, type, pass);
            T prop = null;
            if (_prop != null) {
                prop = _prop as T;
                if (prop == null) {
                    SavePropertyPass(key, pass);
                } else {
                    Error("Setup Property Failed: {0} -> {1}, {2} Type Mismatched: Property<{3}> -> {4}",
                            this, type, fragment, typeof(T).FullName, _prop.GetType().FullName);
                    Context.Properties.Remove<Aspect>(key, pass);
                }
            } else {
                Error("Setup Property Failed: {0} -> {1}, {2}", this, type, fragment);
            }
            return prop;
        }

        public Property<T> SetupProperty<T>(string type, string fragment,
                Pass pass, Property<T>.GetterBlock getter,
                ValueChecker<T> checker,
                ValueWatcher<T> watcher) {
            string key = GetSubKey(fragment);

            Property<T> prop = SetupProperty<Property<T>>(type, fragment, pass);

            if (prop != null) {
                T val = getter();
                prop.Setup(pass, val);
                SavePropertySyncer<T>(key, prop, getter);
                if (checker != null && !prop.AddValueChecker(checker)) {
                    Error("Add Checker Failed: {0} -> {1}, {2}", this, type, fragment);
                }
                if (watcher != null && !prop.AddValueWatcher(watcher)) {
                    Error("Add Watcher Failed: {0} -> {1}, {2}", this, type, fragment);
                }
            }
            return prop;
        }

        //SILP: EXTENSION_SETUP_PROPERTY(Bool, bool)
        public BoolProperty SetupBoolProperty(string fragment,                //__SILP__
                Pass pass, BoolProperty.GetterBlock getter,                   //__SILP__
                BoolBlockValueChecker.CheckerBlock checker,                   //__SILP__
                BoolBlockValueWatcher.WatcherBlock watcher) {                 //__SILP__
            return SetupProperty<bool>(PropertiesConsts.TypeBoolProperty,     //__SILP__
                fragment, pass, getter,                                       //__SILP__
                checker == null ? null : new BoolBlockValueChecker(checker),  //__SILP__
                watcher == null ? null : new BoolBlockValueWatcher(watcher)   //__SILP__
            ) as BoolProperty;                                                //__SILP__
        }                                                                     //__SILP__
                                                                              //__SILP__
        public BoolProperty SetupBoolProperty(string fragment, Pass pass,     //__SILP__
                BoolProperty.GetterBlock getter) {                            //__SILP__
            return SetupBoolProperty(fragment, pass, getter, null, null);     //__SILP__
        }                                                                     //__SILP__
                                                                              //__SILP__
        public BoolProperty SetupBoolProperty(string fragment, Pass pass,     //__SILP__
                BoolProperty.GetterBlock getter,                              //__SILP__
                BoolBlockValueWatcher.WatcherBlock watcher) {                 //__SILP__
            return SetupBoolProperty(fragment, pass, getter, null, watcher);  //__SILP__
        }                                                                     //__SILP__
                                                                              //__SILP__
        //SILP: EXTENSION_SETUP_PROPERTY(Int, int)
        public IntProperty SetupIntProperty(string fragment,                 //__SILP__
                Pass pass, IntProperty.GetterBlock getter,                   //__SILP__
                IntBlockValueChecker.CheckerBlock checker,                   //__SILP__
                IntBlockValueWatcher.WatcherBlock watcher) {                 //__SILP__
            return SetupProperty<int>(PropertiesConsts.TypeIntProperty,      //__SILP__
                fragment, pass, getter,                                      //__SILP__
                checker == null ? null : new IntBlockValueChecker(checker),  //__SILP__
                watcher == null ? null : new IntBlockValueWatcher(watcher)   //__SILP__
            ) as IntProperty;                                                //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        public IntProperty SetupIntProperty(string fragment, Pass pass,      //__SILP__
                IntProperty.GetterBlock getter) {                            //__SILP__
            return SetupIntProperty(fragment, pass, getter, null, null);     //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        public IntProperty SetupIntProperty(string fragment, Pass pass,      //__SILP__
                IntProperty.GetterBlock getter,                              //__SILP__
                IntBlockValueWatcher.WatcherBlock watcher) {                 //__SILP__
            return SetupIntProperty(fragment, pass, getter, null, watcher);  //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        //SILP: EXTENSION_SETUP_PROPERTY(Long, long)
        public LongProperty SetupLongProperty(string fragment,                //__SILP__
                Pass pass, LongProperty.GetterBlock getter,                   //__SILP__
                LongBlockValueChecker.CheckerBlock checker,                   //__SILP__
                LongBlockValueWatcher.WatcherBlock watcher) {                 //__SILP__
            return SetupProperty<long>(PropertiesConsts.TypeLongProperty,     //__SILP__
                fragment, pass, getter,                                       //__SILP__
                checker == null ? null : new LongBlockValueChecker(checker),  //__SILP__
                watcher == null ? null : new LongBlockValueWatcher(watcher)   //__SILP__
            ) as LongProperty;                                                //__SILP__
        }                                                                     //__SILP__
                                                                              //__SILP__
        public LongProperty SetupLongProperty(string fragment, Pass pass,     //__SILP__
                LongProperty.GetterBlock getter) {                            //__SILP__
            return SetupLongProperty(fragment, pass, getter, null, null);     //__SILP__
        }                                                                     //__SILP__
                                                                              //__SILP__
        public LongProperty SetupLongProperty(string fragment, Pass pass,     //__SILP__
                LongProperty.GetterBlock getter,                              //__SILP__
                LongBlockValueWatcher.WatcherBlock watcher) {                 //__SILP__
            return SetupLongProperty(fragment, pass, getter, null, watcher);  //__SILP__
        }                                                                     //__SILP__
                                                                              //__SILP__
        //SILP: EXTENSION_SETUP_PROPERTY(Float, float)
        public FloatProperty SetupFloatProperty(string fragment,               //__SILP__
                Pass pass, FloatProperty.GetterBlock getter,                   //__SILP__
                FloatBlockValueChecker.CheckerBlock checker,                   //__SILP__
                FloatBlockValueWatcher.WatcherBlock watcher) {                 //__SILP__
            return SetupProperty<float>(PropertiesConsts.TypeFloatProperty,    //__SILP__
                fragment, pass, getter,                                        //__SILP__
                checker == null ? null : new FloatBlockValueChecker(checker),  //__SILP__
                watcher == null ? null : new FloatBlockValueWatcher(watcher)   //__SILP__
            ) as FloatProperty;                                                //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public FloatProperty SetupFloatProperty(string fragment, Pass pass,    //__SILP__
                FloatProperty.GetterBlock getter) {                            //__SILP__
            return SetupFloatProperty(fragment, pass, getter, null, null);     //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public FloatProperty SetupFloatProperty(string fragment, Pass pass,    //__SILP__
                FloatProperty.GetterBlock getter,                              //__SILP__
                FloatBlockValueWatcher.WatcherBlock watcher) {                 //__SILP__
            return SetupFloatProperty(fragment, pass, getter, null, watcher);  //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        //SILP: EXTENSION_SETUP_PROPERTY(Double, double)
        public DoubleProperty SetupDoubleProperty(string fragment,              //__SILP__
                Pass pass, DoubleProperty.GetterBlock getter,                   //__SILP__
                DoubleBlockValueChecker.CheckerBlock checker,                   //__SILP__
                DoubleBlockValueWatcher.WatcherBlock watcher) {                 //__SILP__
            return SetupProperty<double>(PropertiesConsts.TypeDoubleProperty,   //__SILP__
                fragment, pass, getter,                                         //__SILP__
                checker == null ? null : new DoubleBlockValueChecker(checker),  //__SILP__
                watcher == null ? null : new DoubleBlockValueWatcher(watcher)   //__SILP__
            ) as DoubleProperty;                                                //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        public DoubleProperty SetupDoubleProperty(string fragment, Pass pass,   //__SILP__
                DoubleProperty.GetterBlock getter) {                            //__SILP__
            return SetupDoubleProperty(fragment, pass, getter, null, null);     //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        public DoubleProperty SetupDoubleProperty(string fragment, Pass pass,   //__SILP__
                DoubleProperty.GetterBlock getter,                              //__SILP__
                DoubleBlockValueWatcher.WatcherBlock watcher) {                 //__SILP__
            return SetupDoubleProperty(fragment, pass, getter, null, watcher);  //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        //SILP: EXTENSION_SETUP_PROPERTY(String, string)
        public StringProperty SetupStringProperty(string fragment,              //__SILP__
                Pass pass, StringProperty.GetterBlock getter,                   //__SILP__
                StringBlockValueChecker.CheckerBlock checker,                   //__SILP__
                StringBlockValueWatcher.WatcherBlock watcher) {                 //__SILP__
            return SetupProperty<string>(PropertiesConsts.TypeStringProperty,   //__SILP__
                fragment, pass, getter,                                         //__SILP__
                checker == null ? null : new StringBlockValueChecker(checker),  //__SILP__
                watcher == null ? null : new StringBlockValueWatcher(watcher)   //__SILP__
            ) as StringProperty;                                                //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        public StringProperty SetupStringProperty(string fragment, Pass pass,   //__SILP__
                StringProperty.GetterBlock getter) {                            //__SILP__
            return SetupStringProperty(fragment, pass, getter, null, null);     //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        public StringProperty SetupStringProperty(string fragment, Pass pass,   //__SILP__
                StringProperty.GetterBlock getter,                              //__SILP__
                StringBlockValueWatcher.WatcherBlock watcher) {                 //__SILP__
            return SetupStringProperty(fragment, pass, getter, null, watcher);  //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        //SILP: EXTENSION_SETUP_PROPERTY(Data, Data)
        public DataProperty SetupDataProperty(string fragment,                //__SILP__
                Pass pass, DataProperty.GetterBlock getter,                   //__SILP__
                DataBlockValueChecker.CheckerBlock checker,                   //__SILP__
                DataBlockValueWatcher.WatcherBlock watcher) {                 //__SILP__
            return SetupProperty<Data>(PropertiesConsts.TypeDataProperty,     //__SILP__
                fragment, pass, getter,                                       //__SILP__
                checker == null ? null : new DataBlockValueChecker(checker),  //__SILP__
                watcher == null ? null : new DataBlockValueWatcher(watcher)   //__SILP__
            ) as DataProperty;                                                //__SILP__
        }                                                                     //__SILP__
                                                                              //__SILP__
        public DataProperty SetupDataProperty(string fragment, Pass pass,     //__SILP__
                DataProperty.GetterBlock getter) {                            //__SILP__
            return SetupDataProperty(fragment, pass, getter, null, null);     //__SILP__
        }                                                                     //__SILP__
                                                                              //__SILP__
        public DataProperty SetupDataProperty(string fragment, Pass pass,     //__SILP__
                DataProperty.GetterBlock getter,                              //__SILP__
                DataBlockValueWatcher.WatcherBlock watcher) {                 //__SILP__
            return SetupDataProperty(fragment, pass, getter, null, watcher);  //__SILP__
        }                                                                     //__SILP__
                                                                              //__SILP__
    }
}
