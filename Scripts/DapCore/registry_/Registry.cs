using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;

namespace angeldnd.dap {
    public static class RegistryConsts {
        public const char Separator = '/';

        public const string TypeRegistry = "Registry";

        public const string ChannelTick = "_tick";

        public static string GetItemPath(params string[] segments) {
            return string.Join("/", segments);
        }
    }

    public sealed class Registry : Context {
        public override char Separator {
            get { return RegistryConsts.Separator; }
        }

        private string _Name = null;
        public string Name {
            get { return _Name; }
        }

        public Registry() {
            //The tick channel will be triggered by some runtime, e.g. in Unity, will be from
            //FixedUpdate(), or other timer on other platform.
            AddChannel(RegistryConsts.ChannelTick, DepositChannelPass(RegistryConsts.ChannelTick, new Pass()));
        }

        public override string GetLogPrefix() {
            return string.Format("[{0}] [{1}] ({2}) ", GetType().Name, _Name, Revision);
        }

        public bool Inited {
            get { return _Name != null; }
        }

        public bool Init(string name) {
            if (_Name == null) {
                _Name = name;
                return true;
            }
            Error("Already Inited: {0} -> {1}", _Name, name);
            return false;
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
