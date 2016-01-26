using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace angeldnd.dap {
    public static class EnvConsts {
        public const string DefaultLogDir = "dap";
        public const string DefaultLogName = "env";
        public const bool DefaultLogDebug = true;

        public const string ChannelTick = "_tick";
    }

    public sealed class Env : DictContext<Env, Registry>, IDictWatcher<Registry> {
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
            _Instance.Tick();
        }

        private static readonly Env _Instance = new Env();
        public static Env Instance {
            get { return _Instance; }
        }

        private Env() : base(null, null) {
            AddDictWatcher(this);
        }

        public override string LogPrefix {
            get {
                return string.Format("[Env] {0} ", RevInfo);
            }
        }

        public void OnElementAdded(Registry registry) {
            registry.Channels.Add(EnvConsts.ChannelTick);
        }

        public void OnElementRemoved(Registry registry) {
        }

        private void Tick() {
            ForEach((Registry registry) => {
                registry.Channels.FireEvent(EnvConsts.ChannelTick, null);
            });
        }
    }
}
