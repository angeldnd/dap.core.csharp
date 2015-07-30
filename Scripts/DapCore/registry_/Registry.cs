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

    public sealed class Registry : Context {
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

            AddChannel(RegistryConsts.ChannelTick, DepositChannelPass(RegistryConsts.ChannelTick, new Pass()));
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

        public static string GetAbsolutePath(ItemAspect ancestorAspect, string relativePath) {
            return GetAbsolutePath(ancestorAspect.Item.Path, relativePath);
        }

        public static string GetRelativePath(string ancestorPath, string descendantPath) {
            string prefix = ancestorPath + RegistryConsts.Separator;
            if (descendantPath.StartsWith(prefix)) {
                return descendantPath.Replace(prefix, "");
            } else {
                Log.Error("Is Not Desecendant: {0}, {1}", ancestorPath, descendantPath);
            }
            return null;
        }

        public static string GetRelativePath(ItemAspect ancestorAspect, string descendantPath) {
            return GetRelativePath(ancestorAspect.Item.Path, descendantPath);
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

        public T GetItemAspect<T>(ItemAspect a, string aspectPath) where T : class, Aspect {
            return GetItemAspect<T>(a.Item.Path, aspectPath);
        }

        public List<Item> GetChildren(string path) {
            return Filter<Item>(path + RegistryConsts.Separator + PatternMatcherConsts.WildcastSegment);
        }

        public List<Item> GetChildren(ItemAspect a) {
            return GetChildren(a.Item.Path);
        }

        public List<Item> GetDescendants(string path) {
            return Filter<Item>(path + RegistryConsts.Separator + PatternMatcherConsts.WildcastSegments);
        }

        public List<Item> GetDescendants(ItemAspect a) {
            return GetDescendants(a.Item.Path);
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

        public void FilterDescendantWithAspects<T>(ItemAspect a, string aspectPath,
                                                    OnAspect<T> callback) where T : class, Aspect {
            FilterDescendantWithAspects<T>(a.Item.Path, aspectPath, callback);
        }

        public List<T> GetDescendantWithAspects<T>(string path, string aspectPath) where T : class, Aspect {
            List<T> result = null;
            FilterDescendantWithAspects(path, aspectPath, (T aspect) => {
                if (result == null) result = new List<T>();
                result.Add(aspect);
            });
            return result;
        }

        public List<T> GetDescendantWithAspects<T>(ItemAspect a, string aspectPath) where T : class, Aspect {
            return GetDescendantWithAspects<T>(a.Item.Path, aspectPath);
        }

        public Item GetDescendant(string path, string relativePath) {
            string absPath = string.Format("{0}{1}{2}", path, RegistryConsts.Separator, relativePath);
            Item result =  Get<Item>(absPath);
            if (result == null) {
                Error("Descendant Not Found: {0}", absPath);
            }
            return result;
        }

        public Item GetDescendant(ItemAspect a, string relativePath) {
            return GetDescendant(a.Item.Path, relativePath);
        }

        public T GetDescendant<T>(string path, string relativePath) where T : ItemAspect {
            Item item = GetDescendant(path, relativePath);
            if (item != null) {
                ItemAspect typeAspect = item.TypeAspect;
                if (typeAspect != null && typeAspect is T) {
                    return typeAspect as T;
                } else {
                    Error("Descendant Not Matched: {0} -> {1}", typeof(T),
                            typeAspect == null ? "null" : typeAspect.GetType().ToString());
                }
            }
            return null;
        }

        public T GetDescendant<T>(ItemAspect a, string relativePath) where T : ItemAspect {
            return GetDescendant<T>(a.Item.Path, relativePath);
        }

        public T GetDescendantAspect<T>(string path, string relativePath, string aspectPath) where T : class, Aspect {
            string absPath = string.Format("{0}{1}{2}", path, RegistryConsts.Separator, relativePath);
            return GetItemAspect<T>(absPath, aspectPath);
        }

        public T GetDescendantAspect<T>(ItemAspect a, string relativePath, string aspectPath) where T : class, Aspect {
            return GetDescendantAspect<T>(a.Item.Path, relativePath, aspectPath);
        }

        public Item GetParent(string path) {
            string[] segments = path.Split(RegistryConsts.Separator);
            if (segments.Length <= 1) return null;

            StringBuilder parentPath = new StringBuilder();
            for (int i = 0; i < segments.Length - 1; i++) {
                parentPath.Append(segments[i]);
                if (i < segments.Length - 2) {
                    parentPath.Append(RegistryConsts.Separator);
                }
            }
            return Get<Item>(parentPath.ToString());
        }

        public Item GetParent(ItemAspect a) {
            return GetParent(a.Item.Path);
        }

        public T GetAncestor<T>(string path) where T : ItemAspect {
            Item parent = GetParent(path);
            if (parent == null) {
                return null;
            } else {
                T aspect = parent.TypeAspect as T;
                if (aspect != null) {
                    return aspect;
                } else {
                    return GetAncestor<T>(parent.Path);
                }
            }
        }

        public T GetAncestor<T>(ItemAspect a) where T : ItemAspect {
            return GetAncestor<T>(a.Item.Path);
        }

        public Item AddItem(string path, string itemType) {
            Item item = Add<Item>(path);
            if (item == null) {
                return null;
            }
            if (!item.Setup(itemType)) {
                Remove<Item>(path);
                return null;
            }
            return item;
        }

        public Item AddItem(string path) {
            return AddItem(path, ItemConsts.TypeItem);
        }

        public override Aspect FactoryAspect(Entity entity, string path, string type) {
            return Factory.FactoryAspect(entity, path, type);
        }
    }
}
