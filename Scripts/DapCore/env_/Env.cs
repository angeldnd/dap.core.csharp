using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace angeldnd.dap {
    public static class EnvConsts {
        public const string DefaultLogDir = "dap";
        public const string DefaultLogName = "env";
        public const bool DefaultLogDebug = false;

        public const string KeyHooks = "Hooks";

        public const string MsgOnInit = "on_init";
        public const string MsgOnBoot = "on_boot";
        public const string MsgOnHalt = "on_halt";

        [DapParam(typeof(string))]
        public const string SummaryAppId = "app_id";
        [DapParam(typeof(int))]
        public const string SummaryVersion = "version";
        [DapParam(typeof(int))]
        public const string SummarySubVersion = "sub_version";
        [DapParam(typeof(int))]
        public const string SummaryRound = "round";
        [DapParam(typeof(int))]
        public const string SummaryTickCount = "tick_count";
        [DapParam(typeof(float))]
        public const string SummaryTickTime = "tick_time";
        [DapParam(typeof(string))]
        public const string SummaryBootstrapper = "bootstrapper";
        [DapParam(typeof(Data))]
        public const string SummaryPlugins = "plugins";
        public const string SummaryOk = "ok";
    }

    public sealed class Env : DictContext<Env, IContext> {
        static Env() {
            Bootstrapper bootstrapper = Bootstrapper.Bootstrap();
            if (bootstrapper != null) {
                if (Log.Init(bootstrapper.GetLogProvider())) {
                    _Bootstrapper = bootstrapper;

                    _AppId = bootstrapper.GetAppId();
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
            Log.Flush();
        }

        private static Bootstrapper _Bootstrapper;
        public static Bootstrapper Bootstrapper {
            get { return _Bootstrapper; }
        }
        private static List<Plugin> _Plugins;

        private static string _AppId;
        public static string AppId {
            get { return _AppId; }
        }

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

        public static Data _TickData = null;
        public static Data TickData {
            get { return _TickData; }
        }

        public static float _Time = 0;
        public static float Time {
            get { return _Time; }
        }

        public static Data NewTickEvt() {
            return _TickData == null ? new Data() : _TickData.Clone();
        }

        public static void Tick(float time, float tickDelta) {
            //The tick channel will be triggered by some runtime, e.g. in Unity, will be from
            //FixedUpdate(), or other timer on other platform.
            if (tickDelta <= 0.0f) {
                _Instance.Error("Invalid Tick Param: tickDelta = {0}", tickDelta);
            }
            SetTime(time);

            _TickCount++;
            _TickDelta = tickDelta;
            _TickTime = _TickTime + tickDelta;
            _TickData = new Data()
                .F(TickableConsts.KeyTime, _Time)
                .I(TickableConsts.KeyTickCount, _TickCount)
                .F(TickableConsts.KeyTickTime, _TickTime);
            _Instance.Tick(_TickData);
        }

        public static void SetTime(float time) {
            if (time < _Time) {
                _Instance.Error("SetTime Failed: {0} -> {1}", _Time, time);
            }
            _Time = time;
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
            //Can NOT create any aspects other than Hooks here.
            //Also can't use normal add here..
            Hooks = new Hooks(this, EnvConsts.KeyHooks);
        }

        private Channel _ChannelOnTick = null;

        public readonly Hooks Hooks;

        public override string LogPrefix {
            get {
                return string.Format("[Env] {0} ", RevInfo);
            }
        }

        private List<int> _FailedPluginIndexes = new List<int>();

        private void Tick(Data evt) {
            _ChannelOnTick.FireEvent(evt);
        }

        private void PublishOnBusAndEnvBus(string msg) {
            Bus.Publish(msg, this);
            EnvBus._PublishByEnv(this, msg);
        }

        private void Init() {
            Log.Info("Dap Environment Init: Version = {0}, Sub Version = {1}, Round = {2}",
                        _Version, _SubVersion, _Round);
            Hooks.Setup();
            _ChannelOnTick = Channels.Add(TickableConsts.ChannelOnTick);

            PublishOnBusAndEnvBus(EnvConsts.MsgOnInit);

            Boot();
        }

        private void Halt() {
            Log.Info("Dap Environment Halt: Version = {0}, Sub Version = {1}, Round = {2}",
                        _Version, _SubVersion, _Round);
            PublishOnBusAndEnvBus(EnvConsts.MsgOnHalt);
        }

        private void Boot() {
            for (int i = 0; i < _Plugins.Count; i++) {
                var plugin = _Plugins[i];
                bool ok = plugin.Init();
                if (ok) {
                    Info("Plugin Init Succeed: {0}", plugin.GetType().FullName);
                } else {
                    _FailedPluginIndexes.Add(i);
                    Error("Plugin Init Failed: {0}", plugin.GetType().FullName);
                }
            }
            Log.Info("Dap Environment Boot Finished: Version = {0}, Sub Version = {1}, Round = {2}",
                        _Version, _SubVersion, _Round);
            PublishOnBusAndEnvBus(EnvConsts.MsgOnBoot);
        }

        /*
         * Note that this call is not working in constructors (since elements are added
         * to owner after)
         */
        public bool TryGetByUri(string uri, out IContext context, out IAspect aspect) {
            context = null;
            aspect = null;
            if (uri == null) return false;

            string contextPath, aspectPath;
            if (!UriConsts.Decode(uri, out contextPath, out aspectPath)) {
                return false;
            }

            if (string.IsNullOrEmpty(contextPath)) {
                context = this;
            } else {
                context = ContextExtension.GetContext(this, contextPath, true);
            }
            if (context == null) return false;

            if (string.IsNullOrEmpty(aspectPath)) {
                return true;
            } else {
                aspect = context.GetAspect(aspectPath, true);
                return aspect != null;
            }
        }

        public T GetByUri<T>(string uri, bool isDebug = false) where T : class, IContextElement {
            T result = null;
            IContext context;
            IAspect aspect;
            if (TryGetByUri(uri, out context, out aspect)) {
                if (aspect != null) {
                    result = aspect.As<T>(true);
                    if (result == null) {
                        ErrorOrDebug(isDebug, "GetByUri<{0}>({1}): Aspect Type MisMatched: {2}",
                                        typeof(T).FullName, uri, aspect);
                    }
                } else {
                    result = context.As<T>(true);
                    if (result == null) {
                        ErrorOrDebug(isDebug, "GetByUri<{0}>({1}): Context Type MisMatched: {2}",
                                        typeof(T).FullName, uri, context);
                    }
                }
            } else {
                ErrorOrDebug(isDebug, "GetByUri<{0}>({1}): Not Found", typeof(T).FullName, uri);
            }
            return result;
        }

        public bool DelayTicks(int ticks, Action<Channel, Data> callback) {
            if (ticks <= 0 || callback == null) return false;

            var owner = Utils.RetainBlockOwner();
            int startTickCount = TickCount;
            _ChannelOnTick.AddEventWatcher(owner, (Channel channel, Data evt) => {
                if (owner == null) return;
                if (TickCount - startTickCount > ticks) {
                    if (Utils.ReleaseBlockOwner(ref owner)) {
                        callback(channel, evt);
                    }
                }
            });
            return true;
        }

        public bool DelaySeconds(float delay, Action<Channel, Data> callback) {
            if (delay <= 0 || callback == null) return false;

            var owner = Utils.RetainBlockOwner();
            float startTime = Time;
            _ChannelOnTick.AddEventWatcher(owner, (Channel channel, Data evt) => {
                if (owner == null) return;
                if (Time - startTime > delay) {
                    if (Utils.ReleaseBlockOwner(ref owner)) {
                        callback(channel, evt);
                    }
                }
            });
            return true;
        }

        protected override void AddSummaryFields(Data summary) {
            base.AddSummaryFields(summary);
            Data plugins = new Data();
            for (int i = 0; i < _Plugins.Count; i++) {
                var plugin = _Plugins[i];
                plugins.A(i.ToString(), new Data()
                            .S(ObjectConsts.SummaryType, plugin.GetType().FullName)
                            .B(EnvConsts.SummaryOk, !_FailedPluginIndexes.Contains(i)));
            }
            summary.S(EnvConsts.SummaryAppId, _AppId)
                   .I(EnvConsts.SummaryVersion, _Version)
                   .I(EnvConsts.SummarySubVersion, _SubVersion)
                   .I(EnvConsts.SummaryRound, _Round)
                   .I(EnvConsts.SummaryTickCount, _TickCount)
                   .F(EnvConsts.SummaryTickTime, _TickTime)
                   .S(EnvConsts.SummaryBootstrapper, _Bootstrapper.GetType().FullName)
                   .A(EnvConsts.SummaryPlugins, plugins);
        }
    }
}
