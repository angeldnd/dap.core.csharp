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

        public const string KeyHooks = "Hooks";
        public const string KeyDebugHook = "debug";

        public const string ChannelTick = "tick";
    }

    public sealed class Env : DictContext<Env, Items> {
        static Env() {
            Bootstrapper bootstrapper = Bootstrapper.Bootstrap();
            if (bootstrapper != null) {
                if (Log.Init(bootstrapper.GetLogProvider())) {
                    _Bootstrapper = bootstrapper;

                    _Version = bootstrapper.GetVersion();
                    _SubVersion = bootstrapper.GetSubVersion();
                    _Instance = new Env();
                    _Instance.Setup();

                    Log.Info("Dap Environment Bootstrapped: Version = {0}, Sub Version = {1}",
                                _Version, _SubVersion);
                    Log.Info("Bootstrapper: {0}", _Bootstrapper.GetType().AssemblyQualifiedName);
                    Log.Info("Log Provider: {0}", Log.Provider.GetType().FullName);

                    foreach (var kv in bootstrapper.GetDapTypes()) {
                        Factory.Register(kv.Key, kv.Value);
                    }

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

        private static Bootstrapper _Bootstrapper;
        public static Bootstrapper Bootstrapper {
            get { return _Bootstrapper; }
        }

        private static int _Version;
        public static int Version {
            get { return _Version; }
        }

        private static int _SubVersion;
        public static int SubVersion {
            get { return _SubVersion; }
        }

        public static string GetContextPath(IContext context) {
            return TreeHelper.GetPath<IContext>(_Instance, context);
        }

        public static string GetAspectPath(IAspect aspect) {
            return TreeHelper.GetPath<IAspect>(aspect.Context, aspect);
        }

        public static string GetAspectUri(IAspect aspect) {
            IContext context = aspect.Context;
            return string.Format("{0}{1}{2}",
                        context == null ? "" : context.Path,
                        UriConsts.UriSeparator,
                        aspect.Path);
        }

        private static int _TickCount = 0;
        public static int TickCount {
            get { return _TickCount; }
        }

        public static float _TickDelta = 0f;
        public static float TickDelta {
            get { return _TickDelta; }
        }

        public static void Tick(float tickDelta) {
            //The tick channel will be triggered by some runtime, e.g. in Unity, will be from
            //FixedUpdate(), or other timer on other platform.
            if (tickDelta <= 0.0f) {
                _Instance.Error("Invalid Tick Param: tickDelta = {0}", tickDelta);
            }
            _TickCount++;
            _TickDelta = tickDelta;
            _Instance.Tick();
        }

        private static Env _Instance;
        public static Env Instance {
            get { return _Instance; }
        }

        private Env() : base(null, null) {
            //Can NOT create any aspects other than Hooks/Hook here.

            Hooks = new Hooks(this, EnvConsts.KeyHooks);
            Hook debugHook = Hooks.Add(EnvConsts.KeyDebugHook);
            debugHook.Setup(
                (IContext context) => {
                    context.Debugging = true;
                },
                (IAspect aspect) => {
                    aspect.Debugging = true;
                }
            );
        }

        private Channel _TickChannel = null;

        private void Setup() {
            _TickChannel = Channels.Add(EnvConsts.ChannelTick);
        }

        public readonly Hooks Hooks;

        public override string LogPrefix {
            get {
                return string.Format("[Env] {0} ", RevInfo);
            }
        }

        public override void OnAdded() {
            //Do Nothing.
        }

        public override void OnRemoved() {
            //Do Nothing.
        }

        private void Tick() {
            _TickChannel.FireEvent(null);
        }
    }
}
