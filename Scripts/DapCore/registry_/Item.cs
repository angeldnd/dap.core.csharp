using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public struct ItemConsts {
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

        //SILP: ASPECT_MIXIN()
        private Entity _Entity = null;                                       //__SILP__
        public Entity Entity {                                               //__SILP__
            get { return _Entity; }                                          //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        private string _Path = null;                                         //__SILP__
        public string Path {                                                 //__SILP__
            get { return _Path; }                                            //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        public string RevPath {                                              //__SILP__
            get {                                                            //__SILP__
                return string.Format("{0}|{1}", _Path, Revision);            //__SILP__
            }                                                                //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        private bool _Inited = false;                                        //__SILP__
        public bool Inited {                                                 //__SILP__
            get { return _Inited; }                                          //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        public bool Init(Entity entity, string path) {                       //__SILP__
            if (_Inited) return false;                                       //__SILP__
            if (entity == null || string.IsNullOrEmpty(path)) return false;  //__SILP__
                                                                             //__SILP__
            _Entity = entity;                                                //__SILP__
            _Path = path;                                                    //__SILP__
            _Inited = true;                                                  //__SILP__
            return true;                                                     //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__

        public virtual void OnAdded() {
            _Registry = FindRegistry(_Entity);
        }

        public virtual void OnRemoved() {
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

        public override Aspect FactoryAspect(Entity entity, string path, string type) {
            if (_Registry != null) {
                return _Registry.FactoryAspect(entity, path, type);
            }
            return null;
        }
    }
}
