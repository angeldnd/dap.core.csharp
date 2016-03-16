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

        public const string MsgOnInit = "on_init";
        public const string MsgOnBoot = "on_boot";
        public const string MsgOnHalt = "on_halt";

        public const string SummaryRound = "round";
        public const string SummaryTickCount = "tick_count";
        public const string SummaryTickTime = "tick_time";
    }

    public sealed class Env : DictContext<Env, Items> {
        static Env() {
            Bootstrapper bootstrapper = Bootstrapper.Bootstrap();
            if (bootstrapper != null) {
                if (Log.Init(bootstrapper.GetLogProvider())) {
                    _Bootstrapper = bootstrapper;

                    _Version = bootstrapper.GetVersion();
                    _SubVersion = bootstrapper.GetSubVersion();
                    foreach (var kv in bootstrapper.GetDapTypes()) {
                        Factory.Register(kv.Key, kv.Value);
                    }
                    _Plugins = bootstrapper.GetPlugins();

                    Log.Info("Dap Environment Bootstrapped: Version = {0}, Sub Version = {1}, Round = {2}",
                                _Version, _SubVersion, _Round);
                    Log.Info("Bootstrapper: {0}", _Bootstrapper.GetType().AssemblyQualifiedName);
                    Log.Info("Log Provider: {0}", Log.Provider.GetType().FullName);

                    Reboot();
                }
            }
        }

        /*
         * The logic is very simple here, though to make the actually Reset work properly,
         * Extra work need to be done in all logics that caching values in the old Env.
         */
        public static void Reboot() {
            if (_Instance != null) {
                _Instance.Halt();
                _Instance = null;
            }

            _Round++;
            _TickCount = 0;
            _TickTime = 0f;
            _TickDelta = 0f;

            _Instance = new Env();
            _Instance.Init();
        }

        private static Bootstrapper _Bootstrapper;
        private static List<Plugin> _Plugins;

        private static int _Version;
        public static int Version {
            get { return _Version; }
        }

        private static int _SubVersion;
        public static int SubVersion {
            get { return _SubVersion; }
        }

        private static int _Round = 0;
        public static int Round {
            get { return _Round; }
        }

        private static int _TickCount = 0;
        public static int TickCount {
            get { return _TickCount; }
        }

        public static float _TickTime = 0f;
        public static float TickTime {
            get { return _TickTime; }
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
            _TickTime = _TickTime + tickDelta;
            _Instance.Tick();
        }

        public static string GetContextPath(IContext context) {
            return TreeHelper.GetPath<IContext>(_Instance, context);
        }

        public static string GetAspectPath(IAspect aspect) {
            return TreeHelper.GetPath<IAspect>(aspect.Context, aspect);
        }

        public static string GetAspectUri(IAspect aspect) {
            IContext context = aspect.Context;
            return UriConsts.Encode(context == null ? "" : context.Path, aspect.Path);
        }

        private static Env _Instance;
        public static Env Instance {
            get { return _Instance; }
        }

        private Env() : base(null, null) {
            //Can NOT create any aspects other than Hooks/Hook here.
            Hooks = AddTopAspect<Hooks>(EnvConsts.KeyHooks);
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

        private void PublishOnBusAndEnvBus(string msg) {
            Bus.Publish(msg, this);
            EnvBus._PublishByEnv(this, msg);
        }

        private void Init() {
            Log.Info("Dap Environment Init: Version = {0}, Sub Version = {1}, Round = {2}",
                        _Version, _SubVersion, _Round);
            _TickChannel = Channels.Add(EnvConsts.ChannelTick);

            PublishOnBusAndEnvBus(EnvConsts.MsgOnInit);

            Boot();
        }

        private void Halt() {
            Log.Info("Dap Environment Halt: Version = {0}, Sub Version = {1}, Round = {2}",
                        _Version, _SubVersion, _Round);
            PublishOnBusAndEnvBus(EnvConsts.MsgOnHalt);
        }

        private void Boot() {
            foreach (Plugin plugin in _Plugins) {
                bool ok = plugin.Init();
                if (ok) {
                    Info("Plugin Init Succeed: {0}", plugin.GetType().FullName);
                } else {
                    Error("Plugin Init Failed: {0}", plugin.GetType().FullName);
                }
            }
            Log.Info("Dap Environment Boot Finished: Version = {0}, Sub Version = {1}, Round = {2}",
                        _Version, _SubVersion, _Round);
            PublishOnBusAndEnvBus(EnvConsts.MsgOnBoot);
        }

        public bool TryGetByUri(string uri, out IContext context, out IAspect aspect) {
            context = null;
            aspect = null;
            if (uri == null) return false;

            string[] segments = uri.Split(UriConsts.PathSeparator);
            if (segments.Length < 1 || segments.Length > 2) {
                Error("Invalid Uri: {0} -> {1}", uri, segments.Length);
                return false;
            }

            if (string.IsNullOrEmpty(segments[0])) {
                context = this;
            } else {
                context = ContextExtension.GetContext(this, segments[0], false);
            }
            if (context == null) return false;

            if (segments.Length == 1) {
                return true;
            } else {
                aspect = context.GetAspect(segments[1], false);
                return aspect != null;
            }
        }

        protected override void AddSummaryFields(Data summary) {
            base.AddSummaryFields(summary);
            summary.I(EnvConsts.SummaryRound, _Round)
                   .I(EnvConsts.SummaryTickCount, _TickCount)
                   .F(EnvConsts.SummaryTickTime, _TickTime);
        }
    }
}
