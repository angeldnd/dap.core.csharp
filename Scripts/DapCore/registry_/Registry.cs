using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;

namespace angeldnd.dap {
    public interface IRegistry : IContext {
    }

    public static class RegistryConsts {
        public const char Separator = '/';

        public const string TypeRegistry = "Registry";

        public const string SectionItems = "_items";

        public const string ChannelTick = "_tick";

        public static string GetItemPath(params string[] segments) {
            return string.Join("/", segments);
        }
    }

    public sealed class Registry : Context<Env, Registry>, IRegistry {
        public readonly RegistryItems Items;

        public Registry(Env owner, string path, Pass pass) : base(owner, path, pass) {
            Items = new RegistryItems(this, RegistryConsts.SectionItems, Pass);

            //The tick channel will be triggered by some runtime, e.g. in Unity, will be from
            //FixedUpdate(), or other timer on other platform.
            AddChannel(RegistryConsts.ChannelTick, DepositChannelPass(RegistryConsts.ChannelTick, new Pass()));
        }

        public T GetParent<T>(string path) where T : Item {
	        return Items.Get<T>(RegistryHelper.GetParentPath(path));
        }

        public List<T> GetChildren<T>(string path) where T : Item {
            return Items.Filter<T>(RegistryHelper.GetChildrenPattern(path));
        }

        public void FilterChildren<T>(string path, Action<T> callback) where T : Item {
            Items.Filter<T>(RegistryHelper.GetChildrenPattern(path), callback);
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
            return Items.Filter<T>(RegistryHelper.GetDescendantsPattern(path));
        }

        public void FilterDescendants<T>(string path, Action<T> callback) where T : Item {
            Items.Filter<T>(RegistryHelper.GetDescendantsPattern(path), callback);
        }

        public T GetDescendant<T>(string path, string relativePath) where T : Item {
            string absPath = RegistryHelper.GetDescendantPath(path, relativePath);
            Item result =  Items.Get<Item>(absPath);
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
            return Items.Add<T>(path);
        }

        public T GetItem<T>(string path) where T : Item {
            return Items.Get<T>(path);
        }

        public Item AddItem(string path) {
            return Items.Add<Item>(path);
        }

        public Item GetItem(string path) {
            return GetItem<Item>(path);
        }

        public Item AddItem(string path, string type) {
            Aspect aspect = Items.Add(path, type);
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
