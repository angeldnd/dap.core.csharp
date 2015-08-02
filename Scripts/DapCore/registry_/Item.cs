using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public struct ItemConsts {
        public const string TypeItem = "Item";
        public const string AspectType = "_type";

        public const string PropType = "_type";
    }

    public sealed class Item : Context, Aspect {
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
            if (string.IsNullOrEmpty(path)) {                                     //__SILP__
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
        }

        public void OnRemoved() {
            _Registry = null;
        }

        public ItemAspect TypeAspect {
            get { return Get<ItemAspect>(ItemConsts.AspectType); }
        }

        //Only Registry supposed to call this method.
        internal bool Setup(string itemType) {
            if (IsString(ItemConsts.PropType)) {
                Error("Already Setup: {0} -> {1}, {2}", GetString(ItemConsts.PropType), itemType);
                return false;
            }
            if (itemType == ItemConsts.TypeItem || string.IsNullOrEmpty(itemType)) {
                AddString(ItemConsts.PropType, Pass, ItemConsts.TypeItem);
                return true;
            }

            Aspect aspect = Add(ItemConsts.AspectType, itemType, Pass);
            if (aspect != null) {
                if (aspect is ItemAspect) {
                    AddString(ItemConsts.PropType, Pass, itemType);
                    return true;
                } else {
                    Error("Invalid Type: {0} -> {1}", itemType, aspect.GetType());
                }
            }
            AddString(ItemConsts.PropType, Pass, ItemConsts.TypeItem);
            return false;
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

        public override Aspect FactoryAspect(Entity entity, string path, string type) {
            if (_Registry != null) {
                return _Registry.FactoryAspect(entity, path, type);
            }
            return null;
        }

        public string GetDescendantPath(string relativePath) {
            return Registry.GetAbsolutePath(Path, relativePath);
        }
    }
}
