using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace angeldnd.dap {
    public static class Env {
        static Env() {
            Context context = new Context();
            _Registries = context.Add<Vars>("registries");

            Bootstrapper bootstrapper = Bootstrapper.Bootstrap();
            if (bootstrapper != null) {
                if (Log.Init(bootstrapper.GetLogDebug(), bootstrapper.GetLogProvider())) {
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

        private static Vars _Registries = null;

        public static bool HasRegistry(string name) {
            return _Registries.Has(name);
        }

        public static Registry GetRegistry(string name) {
            return _Registries.GetValue<Registry>(name, null);
        }

        public static void AllRegistries(Action<Registry> callback) {
            _Registries.All<Var<Registry>>((Var<Registry> v) => {
                callback(v.Value);
            });
        }

        public static Registry AddRegistry(string name, Pass pass, bool setupWithPlugins) {
            if (_Registries.Has(name)) {
                Log.Error("Registry Already Exist: {0}", name);
                return null;
            }

            Registry registry = Factory.New<Registry>(RegistryConsts.TypeRegistry);
            if (registry == null || !registry.Init(name)) {
                return null;
            }

            registry = _Registries.DepositValue<Registry>(name, pass, registry, null);
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
            return registry;
        }

        public static Registry RemoveRegistry(string name, Pass pass) {
            return _Registries.WithdrawValue<Registry>(name, pass, null);
        }
    }
}
