using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;

namespace angeldnd.dap {
    public struct RegistryConsts {
        public const char Separator = '/';

        public const string DefaultLogDir = "dap";
        public const string DefaultLogName = "init";
        public const bool DefaultLogDebug = true;
    }

    public class Registry : Context {
        public override char Separator {
            get { return RegistryConsts.Separator; }
        }

        public static readonly Registry Global;
        static Registry() {
            SetupLogging();
            Global = new Registry();
            BootstrapAutoBootstrappers();
        }

        public readonly Factory Factory;

        public Registry() {
            Factory = Factory.NewBuiltinFactory();
        }

        private static void SetupLogging() {
            Assembly asm = Assembly.GetAssembly(typeof(LogProvider));
            Type[] types = asm.GetTypes();

            int maxPriority = -1;
            Type logType = null;

            foreach (Type type in types) {
                System.Object[] attribs = type.GetCustomAttributes(false);
                foreach (System.Object attr in attribs) {
                    DapPriority priority = attr as DapPriority;
                    if (priority != null && priority.Priority > maxPriority) {
                        maxPriority = priority.Priority;
                        logType = type;
                    }
                }
            }
            if (logType != null) {
                Object result = logType.GetMethod("SetupLogging", BindingFlags.Public | BindingFlags.Static).Invoke(null, null);
                Log.Info("SetupLogging: {0} [{1}] -> {2}", logType.Name, maxPriority, result);
            }
        }

        private static void BootstrapAutoBootstrappers() {
            Assembly asm = Assembly.GetAssembly(typeof(Bootstrapper));
            Type[] types = asm.GetTypes();

            foreach (Type type in types) {
                System.Object[] attribs = type.GetCustomAttributes(false);
                foreach (System.Object attr in attribs) {
                    AutoBootstrap autoBootstrap = attr as AutoBootstrap;
                    if (autoBootstrap != null) {
                        Object result = type.GetMethod("Bootstrap", BindingFlags.Public | BindingFlags.Static).Invoke(null, null);
                        Log.Info("Bootstrap: {0} -> {1}", type.Name, result);
                    }
                }
            }
        }

        public Item GetItem(string path) {
            return Get<Item>(path);
        }

        public T GetItemAspect<T>(string path, string aspectPath) where T : class, Aspect {
            Item item = GetItem(path);
            if (item == null) {
                Error("GetItemAspect: Item Not Found: {0}", path);
                return null;
            }
            T aspect = item.Get<T>(aspectPath);
            if (aspect == null) {
                Error("GetItemAspect: {0} Not Found: {1} -> {2}", typeof(T).Name, aspectPath, item);
                return null;
            }
            return aspect;
        }

        public List<T> GetChildren<T>(string path) where T : Item {
            return Filter<T>(path + RegistryConsts.Separator + PatternMatcherConsts.WildcastSegment);
        }

        public List<T> GetDescendants<T>(string path) where T : Item {
            return Filter<T>(path + RegistryConsts.Separator + PatternMatcherConsts.WildcastSegments);
        }

        public T GetParent<T>(string path) where T : Item {
            string[] segments = path.Split(RegistryConsts.Separator);
            if (segments.Length <= 1) return null;

            StringBuilder parentPath = new StringBuilder();
            for (int i = 0; i < segments.Length - 1; i++) {
                parentPath.Append(segments[i]);
                if (i < segments.Length - 2) {
                    parentPath.Append(RegistryConsts.Separator);
                }
            }
            return Get<T>(parentPath.ToString());
        }

        public Item AddItem(string path, string type) {
            if (!Has(path)) {
                Aspect aspect = Factory.FactoryAspect(this, path, type);
                if (aspect != null && aspect is Item && AddAspect(aspect)) {
                    return aspect as Item;
                }
            }
            return null;
        }

        public override Aspect FactoryAspect(Entity entity, string path, string type) {
            return Factory.FactoryAspect(entity, path, type);
        }
    }
}
