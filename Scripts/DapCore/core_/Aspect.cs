using System;

namespace angeldnd.dap {
    public interface Aspect : DapObject {
        Entity Entity { get; }
        string Path { get; }
        string RevPath { get; }

        bool Inited { get; }
        bool Init(Entity entity, string path);

        void OnAdded();
        void OnRemoved();
    }

    public abstract class BaseAspect : BaseDapObject, Aspect {
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
        public virtual bool Init(Entity entity, string path) {                    //__SILP__
            if (_Entity != null) {                                                //__SILP__
                Error("Already Inited: {0} -> {1}, {2}", _Entity, entity, path);  //__SILP__
                return false;                                                     //__SILP__
            }                                                                     //__SILP__
            if (entity == null) {                                                 //__SILP__
                Error("Invalid Entity: {0}, {1}", entity, path);                  //__SILP__
                return false;                                                     //__SILP__
            }                                                                     //__SILP__
            if (IsNullOrEmpty(path)) {                                            //__SILP__
                Error("Invalid Path: {0}, {1}", entity, path);                    //__SILP__
                return false;                                                     //__SILP__
            }                                                                     //__SILP__
                                                                                  //__SILP__
            _Entity = entity;                                                     //__SILP__
            _Path = path;                                                         //__SILP__
            return true;                                                          //__SILP__
        }                                                                         //__SILP__
                                                                                  //__SILP__
        //SILP: ASPECT_EVENTS_MIXIN()
        public virtual void OnAdded() {}                              //__SILP__
        public virtual void OnRemoved() {}                            //__SILP__

        //SILP: ASPECT_LOG_MIXIN(override)
        public override string GetLogPrefix() {                                                          //__SILP__
            if (_Entity != null) {                                                                       //__SILP__
                return string.Format("{0}[{1}] {2} ", _Entity.GetLogPrefix(), GetType().Name, RevPath);  //__SILP__
            } else {                                                                                     //__SILP__
                return string.Format("[] [{0}] {1} ", GetType().Name, RevPath);                          //__SILP__
            }                                                                                            //__SILP__
        }                                                                                                //__SILP__
        //SILP: ACCESSOR_LOG_MIXIN(this, _Entity, _Entity)
        public override bool DebugMode {                                    //__SILP__
            get { return _Entity != null && _Entity.DebugMode; }            //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        public override string[] DebugPatterns {                            //__SILP__
            get { return _Entity != null ? _Entity.DebugPatterns : null; }  //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
    }

    public abstract class EntityAspect : Entity, Aspect {
        public override bool DebugMode {
            get { return _Entity != null && _Entity.DebugMode; }
        }

        public override string[] DebugPatterns {
            get {
                string[] basePatterns = base.DebugPatterns;
                string[] entityPatterns = null;
                if (_Entity != null) {
                    entityPatterns = _Entity.DebugPatterns;
                }
                if (basePatterns == null || basePatterns.Length == 0) {
                    return entityPatterns;
                } else if (entityPatterns == null || entityPatterns.Length == 0) {
                    return basePatterns;
                }
                string[] result = new string[basePatterns.Length + entityPatterns.Length];
                basePatterns.CopyTo(result, 0);
                entityPatterns.CopyTo(result, basePatterns.Length);
                return result; 
            }
        }

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
        public virtual bool Init(Entity entity, string path) {                    //__SILP__
            if (_Entity != null) {                                                //__SILP__
                Error("Already Inited: {0} -> {1}, {2}", _Entity, entity, path);  //__SILP__
                return false;                                                     //__SILP__
            }                                                                     //__SILP__
            if (entity == null) {                                                 //__SILP__
                Error("Invalid Entity: {0}, {1}", entity, path);                  //__SILP__
                return false;                                                     //__SILP__
            }                                                                     //__SILP__
            if (IsNullOrEmpty(path)) {                                            //__SILP__
                Error("Invalid Path: {0}, {1}", entity, path);                    //__SILP__
                return false;                                                     //__SILP__
            }                                                                     //__SILP__
                                                                                  //__SILP__
            _Entity = entity;                                                     //__SILP__
            _Path = path;                                                         //__SILP__
            return true;                                                          //__SILP__
        }                                                                         //__SILP__
                                                                                  //__SILP__
        //SILP: ASPECT_LOG_MIXIN(override)
        public override string GetLogPrefix() {                                                          //__SILP__
            if (_Entity != null) {                                                                       //__SILP__
                return string.Format("{0}[{1}] {2} ", _Entity.GetLogPrefix(), GetType().Name, RevPath);  //__SILP__
            } else {                                                                                     //__SILP__
                return string.Format("[] [{0}] {1} ", GetType().Name, RevPath);                          //__SILP__
            }                                                                                            //__SILP__
        }                                                                                                //__SILP__
        //SILP: ASPECT_EVENTS_MIXIN()
        public virtual void OnAdded() {}                              //__SILP__
        public virtual void OnRemoved() {}                            //__SILP__
    }
}
