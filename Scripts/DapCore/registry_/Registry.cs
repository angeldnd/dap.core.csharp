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

        public static string GetItemPath(params string[] segments) {
            return string.Join("/", segments);
        }
    }

    public interface RegistryWatcher {
        void OnItemSetup(Registry registry, Item item);
    }

    public sealed class Registry : Context {
        public override char Separator {
            get { return RegistryConsts.Separator; }
        }

        private static Registry _Global = null;
        public static Registry Global {
            get {
                if (_Global == null) {
                    Bootstrap();
                }
                return _Global;
            }
        }

        private static bool _Bootstrapped = false;
        public static bool Bootstrapped {
            get { return _Bootstrapped; }
        }

        public static void Bootstrap() {
            if (!_Bootstrapped) {
                _Bootstrapped = true;
                _Global = new Registry();
                SetupLogging();
                BootstrapAutoBootstrappers();
            }
        }

        private static void SetupLogging() {
            int maxPriority = -1;
            Type logType = null;
            Type LogProviderType = typeof(LogProvider);

            Assembly a = Assembly.GetAssembly(typeof(Bootstrapper));

            Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly asm in asms) {
                Type[] types = asm.GetTypes();

                foreach (Type type in types) {
                    if (!type.IsSubclassOf(LogProviderType)) continue;

                    System.Object[] attribs = type.GetCustomAttributes(false);
                    foreach (System.Object attr in attribs) {
                        DapPriority priority = attr as DapPriority;
                        if (priority != null && priority.Priority > maxPriority) {
                            maxPriority = priority.Priority;
                            logType = type;
                        }
                    }
                }
            }

            if (logType != null) {
                Object result = logType.GetMethod("SetupLogging", BindingFlags.Public | BindingFlags.Static).Invoke(null, null);
                Log.Info("SetupLogging: {0} [{1}] -> {2}", logType.Name, maxPriority, result);
            }
        }

        private static void BootstrapAutoBootstrappers() {
            Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly asm in asms) {
                BootstrapAutoBootstrappers(asm);
            }
        }

        private static void BootstrapAutoBootstrappers(Assembly asm) {
            Type bootstrapType = typeof(Bootstrapper);
            Type[] types = asm.GetTypes();

            foreach (Type type in types) {
                if (!type.IsSubclassOf(bootstrapType)) continue;

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

        public static int GetDepth(string path) {
            if (string.IsNullOrEmpty(path)) return 0;
            int depth = 1;
            foreach (char ch in path) {
                if (ch == RegistryConsts.Separator) {
                    depth++;
                }
            }
            return depth;
        }

        public static string GetName(string path) {
            if (string.IsNullOrEmpty(path)) return null;
            int pos = path.LastIndexOf(RegistryConsts.Separator);
            if (pos >= 0) {
                return path.Substring(pos + 1);
            }
            return path;
        }

        public static string GetParentPath(string path) {
            return AspectHelper.GetParentPath(path, RegistryConsts.Separator);
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

        public readonly Factory Factory;

        private List<RegistryWatcher> _Watchers = new List<RegistryWatcher>();

        public Registry() {
            Factory = Factory.NewBuiltinFactory();

            //The tick channel will be triggered by some runtime, e.g. in Unity, will be from
            //FixedUpdate(), or other timer on other platform.
            AddChannel(RegistryConsts.ChannelTick, DepositChannelPass(RegistryConsts.ChannelTick, new Pass()));
        }

        public bool AddRegistryWatcher(RegistryWatcher watcher) {
            if (!_Watchers.Contains(watcher)) {
                _Watchers.Add(watcher);
                return true;
            }
            return false;
        }

        public bool RemoveRegistryWatcher(RegistryWatcher watcher) {
            if (_Watchers.Contains(watcher)) {
                _Watchers.Remove(watcher);
                return true;
            }
            return false;
        }


        public string GetDescendantsPattern(string path) {
            if (string.IsNullOrEmpty(path)) {
                return PatternMatcherConsts.WildcastSegments;
            } else {
                return path + RegistryConsts.Separator + PatternMatcherConsts.WildcastSegments;
            }
        }

        public string GetChildrenPattern(string path) {
            if (string.IsNullOrEmpty(path)) {
                return PatternMatcherConsts.WildcastSegment;
            } else {
                return path + RegistryConsts.Separator + PatternMatcherConsts.WildcastSegment;
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

        public T GetItemAspect<T>(ItemAspect a, string aspectPath) where T : class, Aspect {
            return GetItemAspect<T>(a.Item.Path, aspectPath);
        }

        public List<Item> GetChildren(string path) {
            return Filter<Item>(GetChildrenPattern(path));
        }

        public List<Item> GetChildren(ItemAspect a) {
            return GetChildren(a.Item.Path);
        }

        public List<Item> GetDescendants(string path) {
            return Filter<Item>(GetDescendantsPattern(path));
        }

        public List<Item> GetDescendants(ItemAspect a) {
            return GetDescendants(a.Item.Path);
        }

        public void FilterDescendantsWithAspect<T>(string path, string aspectPath,
                                                    OnAspect<T> callback) where T : class, Aspect {
            Filter<Item>(GetDescendantsPattern(path), (Item item) => {
                Aspect aspect = item.Get<Aspect>(aspectPath);
                if (aspect != null && aspect is T) {
                    callback(aspect as T);
                }
            });
        }

        public void FilterDescendantsWithAspect<T>(ItemAspect a, string aspectPath,
                                                    OnAspect<T> callback) where T : class, Aspect {
            FilterDescendantsWithAspect<T>(a.Item.Path, aspectPath, callback);
        }

        public List<T> GetDescendantsWithAspect<T>(string path, string aspectPath) where T : class, Aspect {
            List<T> result = null;
            FilterDescendantsWithAspect(path, aspectPath, (T aspect) => {
                if (result == null) result = new List<T>();
                result.Add(aspect);
            });
            return result;
        }

        public List<T> GetDescendantsWithAspect<T>(ItemAspect a, string aspectPath) where T : class, Aspect {
            return GetDescendantsWithAspect<T>(a.Item.Path, aspectPath);
        }

        public void FilterDescendants<T>(string path, OnAspect<T> callback) where T : ItemAspect {
            FilterDescendantsWithAspect<T>(path, ItemConsts.AspectType, callback);
        }

        public void FilterDescendants<T>(ItemAspect a, OnAspect<T> callback) where T : ItemAspect {
            FilterDescendantsWithAspect<T>(a.Item.Path, ItemConsts.AspectType, callback);
        }

        public List<T> GetDescendants<T>(string path) where T : ItemAspect {
            return GetDescendantsWithAspect<T>(path, ItemConsts.AspectType);
        }

        public List<T> GetDescendants<T>(ItemAspect a) where T : ItemAspect {
            return GetDescendantsWithAspect<T>(a.Item.Path, ItemConsts.AspectType);
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
            return Get<Item>(GetParentPath(path));
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
            if (item.Setup(itemType)) {
                for (int i = 0; i < _Watchers.Count; i++) {
                    _Watchers[i].OnItemSetup(this, item);
                }
            } else {
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
