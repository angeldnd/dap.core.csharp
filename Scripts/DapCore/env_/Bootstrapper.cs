using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace angeldnd.dap {
    public abstract class Bootstrapper {
        /*
         * Note: if more than one valid env assembly are provided, the actual one
         * used is not determined.
         */
        public const string DAP_ENV_ASSEMBLY = "DapEnv.dll";
        public const string DAP_UNITY_ASSEMBLY = "Assembly-CSharp.dll";

        public const string DAP_BOOTSTRAPPER = "DapBootstrapper";

        private static bool IsDapEnvAssembly(Assembly asm) {
            string fileName = System.IO.Path.GetFileName(asm.Location);
            return fileName == DAP_ENV_ASSEMBLY || fileName == DAP_UNITY_ASSEMBLY;
        }

        public static Bootstrapper Bootstrap() {
            Type BootstrapperType = typeof(Bootstrapper);
            Bootstrapper bootstrapper = null;

            Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly asm in asms) {
                if (IsDapEnvAssembly(asm)) {
                    Type type = asm.GetType(DAP_BOOTSTRAPPER);
                    if (type != null && type.IsSubclassOf(BootstrapperType)) {
                        bootstrapper = (Bootstrapper)Activator.CreateInstance(type);
                        if (bootstrapper != null) {
                            break;
                        }
                    }
                }
            }

            if (bootstrapper == null) {
                bootstrapper = new AssemblyBootstrapper();
            }
            return bootstrapper;
        }

        public abstract LogProvider GetLogProvider();
        public abstract int GetVersion();
        public abstract int GetSubVersion();
        public abstract Dictionary<string, Type> GetDapTypes();
        public abstract List<Plugin> GetPlugins();
    }

    public class AssemblyBootstrapper : Bootstrapper {
        public override int GetVersion() {
            return -1;
        }

        public override int GetSubVersion() {
            return -1;
        }

        public override LogProvider GetLogProvider() {
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

        public override Dictionary<string, Type> GetDapTypes() {
            Dictionary<string, Type> dapTypes = new Dictionary<string, Type>();
            Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly asm in asms) {
                AddDapTypes(dapTypes, asm);
            }
            return dapTypes;
        }

        private static void AddDapTypes(Dictionary<string, Type> dapTypes, Assembly asm) {
            Type objectType = typeof(IObject);
            Type[] types = asm.GetTypes();

            foreach (Type type in types) {
                if (!objectType.IsAssignableFrom(type)) continue;
                if (type.IsAbstract) continue;

                string dapType = DapType.GetDapType(type);
                if (dapType != null) {
                    if (dapTypes.ContainsKey(dapType)) {
                        Log.Critical("DapType Conflict: [{0}] {1} -> {2}",
                                        dapType, dapTypes[dapType].FullName, type.FullName);
                    } else {
                        dapTypes[dapType] = type;
                    }
                }
            }
        }

        public override List<Plugin> GetPlugins() {
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
