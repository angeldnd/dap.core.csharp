using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace angeldnd.dap {
    public interface IEnvWatcher {
        void OnRegistryAdded(Registry registry);
        void OnRegistryRemoved(Registry registry);
    }

    public static class EnvConsts {
        public const string DefaultLogDir = "dap";
        public const string DefaultLogName = "env";
        public const bool DefaultLogDebug = true;

        public const string ChannelTick = "_tick";
    }

    public sealed class Env : Tree<Registry> {
        static Env() {
            Bootstrapper bootstrapper = Bootstrapper.Bootstrap();
            if (bootstrapper != null) {
                if (Log.Init(bootstrapper.GetLogProvider())) {
                    _Bootstrapper = bootstrapper;
                    _Version = bootstrapper.GetVersion();
                    _SubVersion = bootstrapper.GetSubVersion();

                    Log.Info("DAP Environment Bootstrapped");
                    Log.Info("Bootstrapper: {0}", _Bootstrapper.GetType().AssemblyQualifiedName);
                    Log.Info("Log Provider: {0}", Log.Provider.GetType().FullName);

                    foreach (Plugin plugin in bootstrapper.GetPlugins()) {
                        bool ok = plugin.Init();
                        if (ok) {
                            Log.Info("Plugin Init Succeed: {0}", plugin.GetType().FullName);
                        } else {
                            Log.Error("Plugin Init Failed: {0}", plugin.GetType().FullName);
                        }
                    }
                }
            }
        }

        private static int _Version;
        public static int Version {
            get { return _Version; }
        }

        private static int _SubVersion;
        public static int SubVersion {
            get { return _SubVersion; }
        }

        private static Bootstrapper _Bootstrapper;
        public static Bootstrapper Bootstrapper {
            get { return _Bootstrapper; }
        }

        private static Pass _TickPass = new Pass();

        private static int _TickCount = 0;
        public static int TickCount {
            get { return _TickCount; }
        }

        public static float _TickDelta = 0f;
        public static float TickDelta {
            get { return _TickDelta; }
        }

        public static void Tick(int tickCount, float tickDelta) {
            //The tick channel will be triggered by some runtime, e.g. in Unity, will be from
            //FixedUpdate(), or other timer on other platform.
            if (tickCount <= _TickCount || tickDelta <= 0.0f) {
                _Instance.Error("Invalid Tick Param: tickCount = {0}, tickDelta = {1}", tickCount, tickDelta);
            }
            _TickCount = tickCount;
            _TickDelta = tickDelta;
            _Instance.All((Registry registry) => {
                registry.Channels.FireEvent(EnvConsts.ChannelTick, _TickPass);
            });
        }

        private static Env _Instance = new Env();

        private static WeakList<IEnvWatcher> _Watchers = null;

        public static bool AddWatcher(IEnvWatcher watcher) {
            return WeakListHelper.Add(ref _Watchers, watcher);
        }

        public static bool RemoveWatcher(IEnvWatcher watcher) {
            return WeakListHelper.Remove(_Watchers, watcher);
        }

        public static bool HasRegistry(string name) {
            return _Instance.Has(name);
        }

        public static Registry GetRegistry(string name) {
            return _Instance.Get(name);
        }

        public static void AllRegistries(Action<Registry> callback) {
            _Instance.All(callback);
        }

        public static Registry AddRegistry(string name, Pass pass, bool setupWithPlugins) {
            Registry registry = _Instance.Add(name, pass);
            if (registry != null && setupWithPlugins) {
                foreach (Plugin plugin in _Bootstrapper.GetPlugins()) {
                    bool ok = plugin.SetupRegistry(registry);
                    if (ok) {
                        registry.Info("Plugin SetupRegistry Succeed: {0}", plugin.GetType().FullName);
                    } else {
                        registry.Error("Plugin SetupRegistry Failed: {0}", plugin.GetType().FullName);
                    }
                }
            }
            if (registry != null) {
                registry.Channels.AddChannel(EnvConsts.ChannelTick, _TickPass);
                WeakListHelper.Notify(_Watchers, (IEnvWatcher watcher) => {
                    watcher.OnRegistryAdded(registry);
                });
            }
            return registry;
        }

        public static Registry AddRegistry(string name, bool setupWithPlugins) {
            return AddRegistry(name, new Pass(), setupWithPlugins);
        }

        public static Registry AddRegistry(string name, Pass pass) {
            return AddRegistry(name, pass, true);
        }

        public static Registry AddRegistry(string name) {
            return AddRegistry(name, new Pass(), true);
        }

        public static Registry RemoveRegistry(string name, Pass pass) {
            Registry registry = _Instance.Remove(name, pass);
            if (registry != null) {
                WeakListHelper.Notify(_Watchers, (IEnvWatcher watcher) => {
                    watcher.OnRegistryRemoved(registry);
                });
            }
            return registry;
        }

        public static Registry RemoveRegistry(string name) {
            return RemoveRegistry(name, null);
        }

        public static Registry GetOrAddRegistry(string name) {
            return _Instance.GetOrAdd(name);
        }

        private Env() : base(new Pass().Open) {
        }
    }
}
