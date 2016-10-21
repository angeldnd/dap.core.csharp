using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace angeldnd.dap {
    public abstract class Bootstrapper {
#if DOTNET_CORE
        private static Bootstrapper _Bootstrapper = null;
        public static bool SetBootstrapper(Bootstrapper bootstrapper) {
            if (_Bootstrapper == null) {
                _Bootstrapper = bootstrapper;
                return true;
            }
            return false;
        }

        public static Bootstrapper Bootstrap() {
            return _Bootstrapper;
        }
#else
        /*
         * Note: if more than one valid env assembly are provided, the actual one
         * used is not determined.
         */
        public const string DAP_ENV_ASSEMBLY = "DapEnv";
        public const string DAP_UNITY_ASSEMBLY = "Assembly-CSharp";

        public const string DAP_BOOTSTRAPPER = "DapBootstrapper";

        private static bool IsDapEnvAssembly(Assembly asm) {
            /*
             * For Unity3D's IL2CPP to work, need to check CodeBase instead of Location
             */
            try {
                string fileName = System.IO.Path.GetFileName(asm.CodeBase);
                fileName = fileName.Replace(".dll", "");
                return fileName == DAP_ENV_ASSEMBLY || fileName == DAP_UNITY_ASSEMBLY;
            } catch (Exception e) {
            }
            return false;
        }

        public static Bootstrapper Bootstrap() {
            Type BootstrapperType = typeof(Bootstrapper);
            Bootstrapper bootstrapper = null;

            Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly asm in asms) {
                if (IsDapEnvAssembly(asm)) {
                    Type type = asm.GetType(DAP_BOOTSTRAPPER);
                    if (type != null && type._IsSubclassOf(BootstrapperType)) {
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
#endif

        public abstract LogProvider GetLogProvider();
        public abstract string GetAppId();
        public abstract int GetVersion();
        public abstract int GetSubVersion();
        public abstract Dictionary<string, Type> GetDapTypes();
        public abstract List<Plugin> GetPlugins();
    }

    public class AssemblyBootstrapper : Bootstrapper {
        public override string GetAppId() {
            return "N/A";
        }

        public override int GetVersion() {
            return 0;
        }

        public override int GetSubVersion() {
            return 0;
        }

        public override LogProvider GetLogProvider() {
            int maxPriority = -1;
            Type logType = null;

            AssemblyHelper.ForEachSubClass<LogProvider>((Type type) => {
                int priority = DapPriority.GetPriority(type);
                if (priority > maxPriority) {
                    maxPriority = priority;
                    logType = type;
                }
            });

            if (logType != null) {
                return (LogProvider)Activator.CreateInstance(logType);
            }
            return null;
        }

        public override Dictionary<string, Type> GetDapTypes() {
            Dictionary<string, Type> dapTypes = new Dictionary<string, Type>();

            AssemblyHelper.ForEachInterface<IObject>((Type type) => {
                AddDapType(dapTypes, type);
            });
            return dapTypes;
        }

        private static void AddDapType(Dictionary<string, Type> dapTypes, Type type) {
            string dapType = DapType.GetDapType(type);
            if (dapType != null) {
                if (dapTypes.ContainsKey(dapType)) {
                    Log.Critical("DapType Conflict: [{0}] {1} -> {2}",
                                    dapType, dapTypes[dapType], type);
                } else {
                    dapTypes[dapType] = type;
                }
            }
        }

        public override List<Plugin> GetPlugins() {
            List<Plugin> plugins = AssemblyHelper.CreateInstancesOfSubClass<Plugin>();

            DapOrder.SortByOrder(plugins);
            return plugins;
        }
    }
}
