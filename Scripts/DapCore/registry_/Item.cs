using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IItem : IContext {
    }

    public interface IItem<TE> : IContext<TE>, IItem
                                    where TE : IItem {
    }

    public interface IItem<TO, TE> : IContext<TO, TE>, IItem<TE>
                                        where TO : ITree
                                        where TE : IItem {
    }

    public static class ItemConsts {
        public const string TypeItem = "Item";
    }

    public sealed class Item : Item<Registry, Item> {
        public override string Type {
            get { return ItemConsts.TypeItem; }
        }

        public Item(Registry registry, string path, Pass pass) : base(registry, path, pass) {
        }
    }

    public class Item<TO, TE> : Context<TO, TE>, IItem<TO, TE>
                                    where TO : IRegistry
                                    where TE : IItem {
        public TO Registry {
            get { return Owner; }
        }

        public Item(TO registry, string path, Pass pass) : base(registry, path, pass) {
        }

        private string _Name = null;
        public string Name {
            get {
                if (_Name == null) {
                    _Name = Path;
                    string parentPath = RegistryHelper.GetParentPath(Path);
                    if (parentPath != null) {
                        _Name = _Name.Substring(parentPath.Length + 1);
                    }
                }
                return _Name;
            }
        }

        public T GetParent<T>() where T : Item {
            return Registry.GetParent<T>(Path);
        }

        public List<T> GetChildren<T>() where T : Item {
            return Registry.GetChildren<T>(Path);
        }

        public void FilterChildren<T>(Action<T> callback) where T : Item {
            Registry.FilterChildren<T>(Path, callback);
        }

        public string GetDescendantPath(string relativePath) {
            return RegistryHelper.GetDescendantPath(Path, relativePath);
        }

        public T GetAncestor<T>() where T : Item {
            return Registry.GetAncestor<T>(Path);
        }

        public List<T> GetDescendants<T>() where T : Item {
            return Registry.GetDescendants<T>(Path);
        }

        public void FilterDescendants<T>(Action<T> callback) where T : Item {
            Registry.FilterDescendants<T>(Path, callback);
        }

        public T GetDescendant<T>(string relativePath) where T : Item {
            return Registry.GetDescendant<T>(Path, relativePath);
        }

        public Item GetDescendant(string relativePath) {
            return GetDescendant<Item>(relativePath);
        }

        public bool HasDescendant(string relativePath) {
            return Registry.Has(GetDescendantPath(relativePath));
        }

        private T GetItemAspect<T>(string aspectPath, bool logError) where T : Aspect<Item> {
            Aspect aspect = Get<Aspect>(aspectPath);
            if (aspect != null && aspect is T) {
                return (T)aspect;
            } else if (logError) {
                if (aspect == null) {
                    Error("GetItemAspect: {0} Not Found: {1}", typeof(T).FullName, aspectPath);
                } else {
                    Error("GetItemAspect: {0} Type Mismatched: {1} -> {2}", typeof(T).FullName, aspect.GetType().FullName);
                }
            }
            return null;
        }

        public T GetItemAspect<T>(string aspectPath) where T : Aspect<Item> {
            return GetItemAspect<T>(aspectPath, true);
        }

        public bool TryGetItemAspect<T>(string aspectPath, out T aspect) where T : Aspect<Item> {
            aspect = GetItemAspect<T>(aspectPath, false);
            return aspect != null;
        }

        private Action<Item> GetItemAspectCallback<T>(string aspectPath, Action<T> callback) where T : Aspect<Item> {
            return (Item item) => {
                T aspect = item.GetItemAspect<T>(aspectPath, false);
                if (aspect != null) {
                    callback(aspect);
                }
            };
        }

        public void FilterChildrenWithAspect<T>(string aspectPath, Action<T> callback) where T : Aspect<Item> {
            FilterChildren<Item>(GetItemAspectCallback<T>(aspectPath, callback));
        }

        public void FilterDescendantsWithAspect<T>(string aspectPath, Action<T> callback) where T : Aspect<Item> {
            FilterDescendants<Item>(GetItemAspectCallback<T>(aspectPath, callback));
        }

        public T AddDescendant<T>(string relativePath) where T : Item {
            return Registry.Add<T>(RegistryHelper.GetAbsolutePath(Path, relativePath));
        }

        public Item AddDescendant(string relativePath) {
            return AddDescendant<Item>(relativePath);
        }

        public T RemoveDescendant<T>(string relativePath) where T : Item {
            return Registry.Remove<T>(GetDescendantPath(relativePath));
        }

        public Item RemoveDescendant(string relativePath) {
            return Remove<Item>(relativePath);
        }

        public Item AddDescendant(string relativePath, string type) {
            return Registry.AddItem(GetDescendantPath(relativePath), type);
        }

        public T GetOrAddItemAspect<T>(string aspectPath) where T : Aspect<Item> {
            return GetOrAddContextAspect<T>(aspectPath);
        }
    }
}
