using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace angeldnd.dap {
    public abstract class Bootstrapper {
        public const string DAP_ENV_ASSEMBLY = "DapEnv.dll";
        public const string DAP_UNITY_ASSEMBLY = "Assembly-CSharp.dll";

        public const string DAP_BOOTSTRAPPER = "DapBootstrapper";

        [DllImport("DapEnv.dll")]
        public static extern string GetBootstrapper();

        private static bool IsDapEnvAssembly(Assembly asm) {
            string fileName = System.IO.Path.GetFileName(asm.Location);
            return fileName == DAP_ENV_ASSEMBLY || fileName == DAP_UNITY_ASSEMBLY;
        }

        public static Environment Bootstrap() {
            Type BootstrapperType = typeof(Bootstrapper);
            Bootstrapper bootstrapper = null;

            Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly asm in asms) {
                if (IsDapEnvAssembly(asm)) {
                    Type type = asm.GetType(DAP_BOOTSTRAPPER);
                    if (type != null && type.IsSubclassOf(BootstrapperType)) {
                        bootstrapper = (Bootstrapper)Activator.CreateInstance(type);
                    }
                }
            }

            if (bootstrapper == null) {
                bootstrapper = new AssemblyBootstrapper();
            }

            Environment env = null;
            if (bootstrapper != null) {
                env = bootstrapper.Init();
            }

            if (env != null) {
                env.Bootstrapper = bootstrapper.GetType().AssemblyQualifiedName;
                Log.Provider = env.LogProvider;
            }
            return env;
        }

        protected abstract Environment Init();
    }

    public class AssemblyBootstrapper : Bootstrapper {
        public static Environment GetAssemblyEnv() {
            return new AssemblyBootstrapper().Init();
        }

        protected override Environment Init() {
            LogProvider logProvider = GetLogProvider();
            List<Plugin> plugins = GetPlugins();
            if (logProvider != null) {
                return new Environment(logProvider, plugins.ToArray());
            }
            return null;
        }

        private LogProvider GetLogProvider() {
            int maxPriority = -1;
            Type logType = null;
            Type LogProviderType = typeof(LogProvider);

            Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly asm in asms) {
                Type[] types = asm.GetTypes();

                foreach (Type type in types) {
                    if (!type.IsSubclassOf(LogProviderType)) continue;
                    if (type.IsAbstract) continue;

                    int priority = DapPriority.GetPriority(type);
                    if (priority > maxPriority) {
                        maxPriority = priority;
                        logType = type;
                    }
                }
            }

            if (logType != null) {
                return (LogProvider)Activator.CreateInstance(logType);
            }
            return null;
        }

        private List<Plugin> GetPlugins() {
            List<Plugin> plugins = new List<Plugin>();
            Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly asm in asms) {
                AddPlugins(plugins, asm);
            }

            Dictionary<Plugin, int> orders = new Dictionary<Plugin, int>();

            foreach (Plugin plugin in plugins) {
                orders[plugin] = DapOrder.GetOrder(plugin.GetType());
            }

            plugins.Sort((Plugin a, Plugin b) => {
                int orderA = orders[a];
                int orderB = orders[b];
                if (orderA == orderB) {
                    return a.GetType().FullName.CompareTo(b.GetType().FullName);
                } else {
                    return orderA.CompareTo(orderB);
                }
            });
            return plugins;
        }

        private static void AddPlugins(List<Plugin> plugins, Assembly asm) {
            Type pluginType = typeof(Plugin);
            Type[] types = asm.GetTypes();

            foreach (Type type in types) {
                if (!type.IsSubclassOf(pluginType)) continue;
                if (type.IsAbstract) continue;

                Plugin plugin = (Plugin)Activator.CreateInstance(type);
                if (plugin != null) {
                    plugins.Add(plugin);
                }
            }
        }
    }
}
