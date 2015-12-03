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

        private Registry() {
            Factory = Factory.NewBuiltinFactory();

            //The tick channel will be triggered by some runtime, e.g. in Unity, will be from
            //FixedUpdate(), or other timer on other platform.
            AddChannel(RegistryConsts.ChannelTick, DepositChannelPass(RegistryConsts.ChannelTick, new Pass()));
        }

        public override Aspect FactoryAspect(Entity entity, string path, string type) {
            return Factory.FactoryAspect(entity, path, type);
        }

        public T GetParent<T>(string path) where T : Item {
	        return Get<T>(RegistryHelper.GetParentPath(path));
        }

        public List<T> GetChildren<T>(string path) where T : Item {
            return Filter<T>(RegistryHelper.GetChildrenPattern(path));
        }

        public void FilterChildren<T>(string path, OnAspect<T> callback) where T : Item {
            Filter<T>(RegistryHelper.GetChildrenPattern(path), callback);
        }

        public T GetAncestor<T>(string path) where T : Item {
            Item parent = GetParent<Item>(path);
            if (parent == null) {
                return null;
            } else {
                if (parent is T) {
                    return (T)parent;
                } else {
                    return GetAncestor<T>(parent.Path);
                }
            }
        }

        public List<T> GetDescendants<T>(string path) where T : Item {
            return Filter<T>(RegistryHelper.GetDescendantsPattern(path));
        }

        public void FilterDescendants<T>(string path, OnAspect<T> callback) where T : Item {
            Filter<T>(RegistryHelper.GetDescendantsPattern(path), callback);
        }

        public T GetDescendant<T>(string path, string relativePath) where T : Item {
            string absPath = RegistryHelper.GetDescendantPath(path, relativePath);
            Item result =  Get<Item>(absPath);
            if (result == null) {
                Error("GetDescendant: {0} Not Found", absPath);
            } else if (result is T) {
                return (T)result;
            } else {
                Error("GetDescendant: {0} Type Mismatched: {1} -> {2}",
                        absPath, typeof(T).FullName, result.GetType().FullName);
                Error("Descendant Not Found: {0}", absPath);
            }
            return null;
        }

        public T AddItem<T>(string path) where T : Item {
            return Add<T>(path);
        }

        public T GetItem<T>(string path) where T : Item {
            return Get<T>(path);
        }

        public Item AddItem(string path) {
            return Add<Item>(path);
        }

        public Item GetItem(string path) {
            return GetItem<Item>(path);
        }

        public Item AddItem(string path, string type) {
            Aspect aspect = Add(path, type);
            if (aspect is Item) {
                return (Item)aspect;
            } else if (aspect == null) {
                Error("AddItem: {0} Failed: {1}", path, type);
            } else {
                Error("AddItem: {0} Type Mismatched: {1} -> {2}", path, type, aspect.GetType().FullName);
                Remove<Aspect>(path);
            }
            return null;
        }
    }
}
