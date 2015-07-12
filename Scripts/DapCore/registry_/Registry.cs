using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;

namespace angeldnd.dap {
    public struct RegistryConsts {
        public const char Separator = '/';

        public const string ChannelTick = "_tick";

        public const string DefaultLogDir = "dap";
        public const string DefaultLogName = "init";
        public const bool DefaultLogDebug = true;
    }

    public class Registry : Context {
        public override char Separator {
            get { return RegistryConsts.Separator; }
        }

        public static readonly Registry Global = new Registry();

        private static bool _Bootstrapped = false;
        public static bool Bootstrapped {
            get { return _Bootstrapped; }
        }

        public static void Bootstrap() {
            if (!_Bootstrapped) {
                _Bootstrapped = true;
                SetupLogging();
                BootstrapAutoBootstrappers();
            }
        }

        public readonly Factory Factory;

        public Registry() {
            Factory = Factory.NewBuiltinFactory();

            AddChannel(RegistryConsts.ChannelTick);
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

        public static string GetAbsolutePath(string ancestorPath, string relativePath) {
            return string.Format("{0}{1}{2}", ancestorPath, RegistryConsts.Separator, relativePath);
        }

        public static string GetRelativePath(string ancestorPath, string descendantPath) {
            string prefix = ancestorPath + RegistryConsts.Separator;
            if (descendantPath.StartsWith(prefix)) {
                return descendantPath.Replace(prefix, "");
            }
            return null;
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

        public void FilterDescendantWithAspects<T>(string path, string aspectPath,
                                                    OnAspect<T> callback) where T : class, Aspect {
            string pattern = path + RegistryConsts.Separator + PatternMatcherConsts.WildcastSegments;
            Filter<Item>(pattern, (Item item) => {
                T aspect = item.Get<T>(aspectPath);
                if (aspect != null) {
                    callback(aspect);
                }
            });
        }

        public List<T> GetDescendantWithAspects<T>(string path, string aspectPath) where T : class, Aspect {
            List<T> result = null;
            FilterDescendantWithAspects(path, aspectPath, (T aspect) => {
                if (result == null) result = new List<T>();
                result.Add(aspect);
            });
            return result;
        }

        public T GetDescendant<T>(string path, string relativePath) where T : Item {
            string absPath = string.Format("{0}{1}{2}", path, RegistryConsts.Separator, relativePath);
            return Get<T>(absPath);
        }

        public T GetDescendantAspect<T>(string path, string relativePath, string aspectPath) where T : class, Aspect {
            string absPath = string.Format("{0}{1}{2}", path, RegistryConsts.Separator, relativePath);
            return GetItemAspect<T>(absPath, aspectPath);
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

        public T GetAncestor<T>(string path) where T : Item {
            Item parent = GetParent<Item>(path);
            if (parent == null) {
                return null;
            } else if (parent is T) {
                return (T)parent;
            } else {
                return GetAncestor<T>(parent.Path);
            }
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
