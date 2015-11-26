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

        private static Registry _Global = new Registry();
        public static Registry Global {
            get {
                return _Global;
            }
        }

        static Registry() {
            Environment env = Bootstrapper.Bootstrap();
            if (env != null) {
                if (Log.Provider == null) {
                    Log.SetProvider(env.LogProvider);
                }
                _Global.Info("DAP Environment Bootstrapped");
                _Global.Info("Bootstrapper: {0}", env.Bootstrapper);
                _Global.Info("Log Provider: {0}", env.LogProvider.GetType().FullName);

                SpecHelper.RegistrySpecValueCheckers();

                foreach (Plugin plugin in env.Plugins) {
                    bool ok = plugin.Init();
                    if (ok) {
                        _Global.Info("Plugin Init Succeed: {0}", plugin.GetType().FullName);
                    } else {
                        _Global.Error("Plugin Init Failed: {0}", plugin.GetType().FullName);
                    }
                }
            }
        }

        public readonly Factory Factory;

        private List<RegistryWatcher> _Watchers = new List<RegistryWatcher>();

        private Registry() {
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

        public T GetItem<T>(string path) where T : ItemType {
            Item item = GetItem(path);
            if (item != null) {
                return item.GetItemType<T>();
            }
            return null;
        }

        public T GetItemAspect<T>(string path, string aspectPath) where T : ItemAspect {
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

        public T GetItemAspect<T>(ItemAspect a, string aspectPath) where T : ItemAspect {
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
                                                    OnAspect<T> callback) where T : ItemAspect {
            Filter<Item>(GetDescendantsPattern(path), (Item item) => {
                if (item.Has(aspectPath)) {
                    Aspect aspect = item.Get<Aspect>(aspectPath);
                    if (aspect != null && aspect is T) {
                        callback(aspect as T);
                    }
                }
            });
        }

        public void FilterDescendantsWithAspect<T>(ItemAspect a, string aspectPath,
                                                    OnAspect<T> callback) where T : ItemAspect {
            FilterDescendantsWithAspect<T>(a.Item.Path, aspectPath, callback);
        }

        public List<T> GetDescendantsWithAspect<T>(string path, string aspectPath) where T : ItemAspect {
            List<T> result = null;
            FilterDescendantsWithAspect(path, aspectPath, (T aspect) => {
                if (result == null) result = new List<T>();
                result.Add(aspect);
            });
            return result;
        }

        public List<T> GetDescendantsWithAspect<T>(ItemAspect a, string aspectPath) where T : ItemAspect {
            return GetDescendantsWithAspect<T>(a.Item.Path, aspectPath);
        }

        public void FilterDescendants<T>(string path, OnAspect<T> callback) where T : ItemType {
            FilterDescendantsWithAspect<T>(path, ItemConsts.AspectType, callback);
        }

        public void FilterDescendants<T>(ItemAspect a, OnAspect<T> callback) where T : ItemType {
            FilterDescendantsWithAspect<T>(a.Item.Path, ItemConsts.AspectType, callback);
        }

        public List<T> GetDescendants<T>(string path) where T : ItemType {
            return GetDescendantsWithAspect<T>(path, ItemConsts.AspectType);
        }

        public List<T> GetDescendants<T>(ItemAspect a) where T : ItemType {
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

        public T GetDescendant<T>(string path, string relativePath) where T : ItemType {
            Item item = GetDescendant(path, relativePath);
            if (item != null) {
                ItemType typeAspect = item.ItemType;
                if (typeAspect != null && typeAspect is T) {
                    return typeAspect as T;
                } else {
                    Error("Descendant Not Matched: {0} -> {1}", typeof(T),
                            typeAspect == null ? "null" : typeAspect.GetType().ToString());
                }
            }
            return null;
        }

        public T GetDescendant<T>(ItemAspect a, string relativePath) where T : ItemType {
            return GetDescendant<T>(a.Item.Path, relativePath);
        }

        public T GetDescendantAspect<T>(string path, string relativePath, string aspectPath) where T : ItemAspect {
            string absPath = string.Format("{0}{1}{2}", path, RegistryConsts.Separator, relativePath);
            return GetItemAspect<T>(absPath, aspectPath);
        }

        public T GetDescendantAspect<T>(ItemAspect a, string relativePath, string aspectPath) where T : ItemAspect {
            return GetDescendantAspect<T>(a.Item.Path, relativePath, aspectPath);
        }

        public Item GetParent(string path) {
	        return Get<Item>(RegistryHelper.GetParentPath(path));
        }

        public Item GetParent(ItemAspect a) {
            return GetParent(a.Item.Path);
        }

        public T GetAncestor<T>(string path) where T : ItemType {
            Item parent = GetParent(path);
            if (parent == null) {
                return null;
            } else {
                T aspect = parent.ItemType as T;
                if (aspect != null) {
                    return aspect;
                } else {
                    return GetAncestor<T>(parent.Path);
                }
            }
        }

        public T GetAncestor<T>(ItemAspect a) where T : ItemType {
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

        public T AddItem<T>(string path) where T : ItemType {
            Item item = Add<Item>(path);
            if (item == null) {
                return null;
            }
            if (item.Setup<T>()) {
                for (int i = 0; i < _Watchers.Count; i++) {
                    _Watchers[i].OnItemSetup(this, item);
                }
            } else {
                Remove<Item>(path);
                return null;
            }
            return item.ItemType as T;
        }

        public override Aspect FactoryAspect(Entity entity, string path, string type) {
            return Factory.FactoryAspect(entity, path, type);
        }
    }
}
