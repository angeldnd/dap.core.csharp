using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class ItemConsts {
        public const string TypeItem = "Item";
    }

    public class Item : Context, Aspect {
        public override string Type {
            get { return ItemConsts.TypeItem; }
        }

        private Registry _Registry = null;
        public Registry Registry {
            get { return _Registry; }
        }

        //SILP: ASPECT_LOG_MIXIN(override)
        public override string GetLogPrefix() {                                                          //__SILP__
            if (_Entity != null) {                                                                       //__SILP__
                return string.Format("{0}[{1}] {2} ", _Entity.GetLogPrefix(), GetType().Name, RevPath);  //__SILP__
            } else {                                                                                     //__SILP__
                return string.Format("[] [{0}] {1} ", GetType().Name, RevPath);                          //__SILP__
            }                                                                                            //__SILP__
        }                                                                                                //__SILP__

        //SILP: ASPECT_MIXIN()
        private Entity _Entity = null;                                            //__SILP__
        public Entity Entity {                                                    //__SILP__
            get { return _Entity; }                                               //__SILP__
        }                                                                         //__SILP__
                                                                                  //__SILP__
        private string _Path = null;                                              //__SILP__
        public string Path {                                                      //__SILP__
            get { return _Path; }                                                 //__SILP__
        }                                                                         //__SILP__
                                                                                  //__SILP__
        public string RevPath {                                                   //__SILP__
            get {                                                                 //__SILP__
                return string.Format("{0} ({1})", _Path, Revision);               //__SILP__
            }                                                                     //__SILP__
        }                                                                         //__SILP__
                                                                                  //__SILP__
        public bool Inited {                                                      //__SILP__
            get { return _Entity != null; }                                       //__SILP__
        }                                                                         //__SILP__
                                                                                  //__SILP__
        public bool Init(Entity entity, string path) {                            //__SILP__
            if (_Entity != null) {                                                //__SILP__
                Error("Already Inited: {0} -> {1}, {2}", _Entity, entity, path);  //__SILP__
                return false;                                                     //__SILP__
            }                                                                     //__SILP__
            if (entity == null) {                                                 //__SILP__
                Error("Invalid Entity: {0}, {1}", entity, path);                  //__SILP__
                return false;                                                     //__SILP__
            }                                                                     //__SILP__
            if (!EntityConsts.IsValidAspectPath(path)) {                          //__SILP__
                Error("Invalid Path: {0}, {1}", entity, path);                    //__SILP__
                return false;                                                     //__SILP__
            }                                                                     //__SILP__
                                                                                  //__SILP__
            _Entity = entity;                                                     //__SILP__
            _Path = path;                                                         //__SILP__
            return true;                                                          //__SILP__
        }                                                                         //__SILP__
                                                                                  //__SILP__

        public void OnAdded() {
            _Registry = FindRegistry(_Entity);
            if (_Registry != null) {
                OnItemAdded();
            }
        }

        public void OnRemoved() {
            if (_Registry != null) {
                OnItemRemoved();
            }
            _Registry = null;
        }

        private Registry FindRegistry(Entity entity) {
            if (entity is Registry) {
                return (Registry)entity;
            } else if (entity is Aspect) {
                Aspect a = entity as Aspect;
                return FindRegistry(a.Entity);
            }
            return null;
        }

        public T GetParent<T>() where T : Item {
            if (_Registry != null) {
                return _Registry.GetParent<T>(Path);
            }
            return null;
        }

        public List<T> GetChildren<T>() where T : Item {
            if (_Registry != null) {
                return _Registry.GetChildren<T>(Path);
            }
            return null;
        }

        public void FilterChildren<T>(OnAspect<T> callback) where T : Item {
            if (_Registry != null) {
                _Registry.FilterChildren<T>(Path, callback);
            }
        }

        public string GetDescendantPath(string relativePath) {
            return RegistryHelper.GetDescendantPath(Path, relativePath);
        }

        public T GetAncestor<T>() where T : Item {
            if (_Registry != null) {
                return _Registry.GetAncestor<T>(Path);
            }
            return null;
        }

        public List<T> GetDescendants<T>() where T : Item {
            if (_Registry != null) {
                return _Registry.GetDescendants<T>(Path);
            }
            return null;
        }

        public void FilterDescendants<T>(OnAspect<T> callback) where T : Item {
            if (_Registry != null) {
                Registry.FilterDescendants<T>(Path, callback);
            }
        }

        public T GetDescendant<T>(string relativePath) where T : Item {
            if (_Registry != null) {
                return _Registry.GetDescendant<T>(Path, relativePath);
            }
            return null;
        }

        public Item GetDescendant(string relativePath) {
            return GetDescendant<Item>(relativePath);
        }

        public bool HasDescendant(string relativePath) {
            if (_Registry != null) {
                return _Registry.Has(GetDescendantPath(relativePath));
            }
            return false;
        }

        private T GetItemAspect<T>(string aspectPath, bool logError) where T : class, ItemAspect {
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

        public T GetItemAspect<T>(string aspectPath) where T : class, ItemAspect {
            return GetItemAspect<T>(aspectPath, true);
        }

        public bool TryGetItemAspect<T>(string aspectPath, out T aspect) where T : class, ItemAspect {
            aspect = GetItemAspect<T>(aspectPath, false);
            return aspect != null;
        }

        private OnAspect<Item> GetItemAspectCallback<T>(string aspectPath, OnAspect<T> callback) where T : class, ItemAspect {
            return (Item item) => {
                T aspect = item.GetItemAspect<T>(aspectPath, false);
                if (aspect != null) {
                    callback(aspect);
                }
            };
        }

        public void FilterChildrenWithAspect<T>(string aspectPath, OnAspect<T> callback) where T : class, ItemAspect {
            FilterChildren<Item>(GetItemAspectCallback<T>(aspectPath, callback));
        }

        public void FilterDescendantsWithAspect<T>(string aspectPath, OnAspect<T> callback) where T : class, ItemAspect {
            FilterDescendants<Item>(GetItemAspectCallback<T>(aspectPath, callback));
        }

        public T AddDescendant<T>(string relativePath) where T : Item {
            return Registry.Add<T>(RegistryHelper.GetAbsolutePath(Path, relativePath));
        }

        public Item AddDescendant(string relativePath) {
            return AddDescendant<Item>(relativePath);
        }

        public T RemoveDescendant<T>(string relativePath) where T : Item {
            if (_Registry != null) {
                return _Registry.Remove<T>(RegistryHelper.GetAbsolutePath(Path, relativePath));
            }
            return null;
        }

        public Item RemoveDescendant(string relativePath) {
            return Remove<Item>(relativePath);
        }

        public Item AddDescendant(string relativePath, string type) {
            if (_Registry != null) {
                return _Registry.AddItem(RegistryHelper.GetAbsolutePath(Path, relativePath), type);
            }
            return null;
        }

        public T GetOrAddItemAspect<T>(string aspectPath) where T : class, ItemAspect {
            T aspect = Get<T>(aspectPath);
            if (aspect != null) {
                return aspect;
            }
            return Add<T>(aspectPath);
        }

        protected virtual void OnItemAdded() {}
        protected virtual void OnItemRemoved() {}
    }
}
