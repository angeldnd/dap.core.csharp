using System;

namespace angeldnd.dap {
    public interface Aspect : DapObject, Logger {
        Entity Entity { get; }
        string Path { get; }
        string RevPath { get; }

        bool Inited { get; }
        bool Init(Entity entity, string path);

        void OnAdded();
        void OnRemoved();
    }

    public interface SecurableAspect : Aspect {
        bool Secured { get; }
        bool SetPass(Object pass);
    }

    public abstract class BaseAspect : Aspect {
        //SILP: DAPOBJECT_MIXIN()
        public virtual string Type {                                  //__SILP__
            get { return null; }                                      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        private int _Revision = 0;                                    //__SILP__
        public int Revision {                                         //__SILP__
            get { return _Revision; }                                 //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        protected virtual void AdvanceRevision() {                    //__SILP__
            _Revision += 1;                                           //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public Data Encode() {                                        //__SILP__
            if (!string.IsNullOrEmpty(Type)) {                        //__SILP__
                Data data = new Data();                               //__SILP__
                if (data.SetString(DapObjectConsts.KeyType, Type)) {  //__SILP__
                    if (DoEncode(data)) {                             //__SILP__
                        return data;                                  //__SILP__
                    }                                                 //__SILP__
                }                                                     //__SILP__
            }                                                         //__SILP__
            if (LogDebug) Debug("Not Encodable!");                    //__SILP__
            return null;                                              //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool Decode(Data data) {                               //__SILP__
            string type = data.GetString(DapObjectConsts.KeyType);    //__SILP__
            if (type == Type) {                                       //__SILP__
                return DoDecode(data);                                //__SILP__
            }                                                         //__SILP__
            return false;                                             //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        //SILP: ASPECT_MIXIN()
        private Entity _Entity = null;                                //__SILP__
        public Entity Entity {                                        //__SILP__
            get { return _Entity; }                                   //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        private string _Path = null;                                  //__SILP__
        public string Path {                                          //__SILP__
            get { return _Path; }                                     //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public string RevPath {                                       //__SILP__
            get {                                                     //__SILP__
                return string.Format("{0} ({1})", _Path, Revision);   //__SILP__
            }                                                         //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        private bool _Inited = false;                                 //__SILP__
        public bool Inited {                                          //__SILP__
            get { return _Inited; }                                   //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool Init(Entity entity, string path) {                //__SILP__
            if (_Inited) {                                            //__SILP__
                Error("Already Inited: {0}, {1}", entity, path);      //__SILP__
                return false;                                         //__SILP__
            }                                                         //__SILP__
            if (entity == null) {                                     //__SILP__
                Error("Invalid Entity: {0}, {1}", entity, path);      //__SILP__
                return false;                                         //__SILP__
            }                                                         //__SILP__
            if (string.IsNullOrEmpty(path)) {                         //__SILP__
                Error("Invalid Path: {0}, {1}", entity, path);        //__SILP__
                return false;                                         //__SILP__
            }                                                         //__SILP__
                                                                      //__SILP__
            _Entity = entity;                                         //__SILP__
            _Path = path;                                             //__SILP__
            _Inited = true;                                           //__SILP__
            return true;                                              //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        //SILP: ASPECT_EVENTS_MIXIN()
        public virtual void OnAdded() {}                              //__SILP__
        public virtual void OnRemoved() {}                            //__SILP__
        //SILP: ASPECT_ENCODE_DECODE_MIXIN()
        protected virtual bool DoEncode(Data data) {                  //__SILP__
            return true;                                              //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        protected virtual bool DoDecode(Data data) {                  //__SILP__
            return true;                                              //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__

        //SILP: ASPECT_LOG_MIXIN(virtual)
        public virtual string GetLogPrefix() {                                                             //__SILP__
            if (_Entity != null) {                                                                         //__SILP__
                return string.Format("{0}[{1}] [{2}] ", _Entity.GetLogPrefix(), GetType().Name, RevPath);  //__SILP__
            } else {                                                                                       //__SILP__
                return string.Format("[] [{0}] [{1}] ", GetType().Name, RevPath);                          //__SILP__
            }                                                                                              //__SILP__
        }                                                                                                  //__SILP__
        //SILP: ACCESSOR_LOG_MIXIN()
        private DebugLogger _DebugLogger = DebugLogger.Instance;                                      //__SILP__
                                                                                                      //__SILP__
        public bool DebugMode {                                                                       //__SILP__
            get { return Entity != null && Entity.DebugMode; }                                        //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public bool LogDebug {                                                                        //__SILP__
            get { return (Entity != null && Entity.LogDebug) || Log.LogDebug; }                       //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public void Critical(string format, params object[] values) {                                 //__SILP__
            if (DebugMode) {                                                                          //__SILP__
                _DebugLogger.Critical(                                                                //__SILP__
                        _DebugLogger.GetLogHint() + GetLogPrefix() + string.Format(format, values));  //__SILP__
            } else {                                                                                  //__SILP__
                Log.Critical(GetLogPrefix() + string.Format(format, values));                         //__SILP__
            }                                                                                         //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public void Error(string format, params object[] values) {                                    //__SILP__
            if (DebugMode) {                                                                          //__SILP__
                _DebugLogger.Error(                                                                   //__SILP__
                        _DebugLogger.GetLogHint() + GetLogPrefix() + string.Format(format, values));  //__SILP__
            } else {                                                                                  //__SILP__
                Log.Error(GetLogPrefix() + string.Format(format, values));                            //__SILP__
            }                                                                                         //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public void Info(string format, params object[] values) {                                     //__SILP__
            if (DebugMode) {                                                                          //__SILP__
                _DebugLogger.LogWithPatterns(LoggerConsts.INFO, Entity.DebugPatterns,                 //__SILP__
                        _DebugLogger.GetLogHint() + GetLogPrefix() + string.Format(format, values));  //__SILP__
            } else {                                                                                  //__SILP__
                Log.Info(GetLogPrefix() + string.Format(format, values));                             //__SILP__
            }                                                                                         //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public void Debug(string format, params object[] values) {                                    //__SILP__
            if (DebugMode) {                                                                          //__SILP__
                _DebugLogger.LogWithPatterns(LoggerConsts.DEBUG, Entity.DebugPatterns,                //__SILP__
                        _DebugLogger.GetLogHint() + GetLogPrefix() + string.Format(format, values));  //__SILP__
            } else {                                                                                  //__SILP__
                Log.Debug(GetLogPrefix() + string.Format(format, values));                            //__SILP__
            }                                                                                         //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
    }

    public abstract class BaseSecurableAspect : BaseAspect, SecurableAspect {
        private static readonly Guid OPEN_PASS = Guid.NewGuid();

        private Object _Pass = null;
        protected Object Pass {
            get { return _Pass; }
        }

        public bool Secured {
            get {
                if (_Pass == null) return false;
                if (OPEN_PASS.Equals(_Pass)) return false;
                return true;
            }
        }

        public bool SetPass(Object pass) {
            /*
             * The OPEN_PASS trick is to set the pass, so it can't
             * be set in the future, but it's "open", any pass can
             * pass the check.
             */
            if (_Pass == null) {
                if (pass == null) {
                    _Pass = OPEN_PASS;
                } else {
                    _Pass = pass;
                }
                return true;
            } else if (_Pass == pass) {
                return true;
            } else if (OPEN_PASS.Equals(_Pass) && (pass == null)) {
                return true;
            } else if (_Pass.Equals(pass)) {
                return true;
            }
            Error("SetPass Failed: {0} -> {1}", _Pass, pass);
            return false;
        }

        protected bool CheckPass(Object pass) {
            if (_Pass == null) return true;
            if (_Pass == pass) return true;
            if (OPEN_PASS.Equals(_Pass)) return true;
            if (_Pass.Equals(pass)) return true;

            Error("Invalid Pass: _Pass = {0}, pass = {1}", _Pass, pass);
            return false;
        }
    }

    public abstract class EntityAspect : Entity, Aspect {
        public override Aspect FactoryAspect(Entity entity, string path, string type) {
            if (Entity != null) {
                return Entity.FactoryAspect(entity, path, type);
            }
            return null;
        }

        public override bool LogDebug {
            get { return base.LogDebug || (_Entity != null && _Entity.LogDebug); }
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
        private Entity _Entity = null;                                //__SILP__
        public Entity Entity {                                        //__SILP__
            get { return _Entity; }                                   //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        private string _Path = null;                                  //__SILP__
        public string Path {                                          //__SILP__
            get { return _Path; }                                     //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public string RevPath {                                       //__SILP__
            get {                                                     //__SILP__
                return string.Format("{0} ({1})", _Path, Revision);   //__SILP__
            }                                                         //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        private bool _Inited = false;                                 //__SILP__
        public bool Inited {                                          //__SILP__
            get { return _Inited; }                                   //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool Init(Entity entity, string path) {                //__SILP__
            if (_Inited) {                                            //__SILP__
                Error("Already Inited: {0}, {1}", entity, path);      //__SILP__
                return false;                                         //__SILP__
            }                                                         //__SILP__
            if (entity == null) {                                     //__SILP__
                Error("Invalid Entity: {0}, {1}", entity, path);      //__SILP__
                return false;                                         //__SILP__
            }                                                         //__SILP__
            if (string.IsNullOrEmpty(path)) {                         //__SILP__
                Error("Invalid Path: {0}, {1}", entity, path);        //__SILP__
                return false;                                         //__SILP__
            }                                                         //__SILP__
                                                                      //__SILP__
            _Entity = entity;                                         //__SILP__
            _Path = path;                                             //__SILP__
            _Inited = true;                                           //__SILP__
            return true;                                              //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        //SILP: ASPECT_LOG_MIXIN(override)
        public override string GetLogPrefix() {                                                            //__SILP__
            if (_Entity != null) {                                                                         //__SILP__
                return string.Format("{0}[{1}] [{2}] ", _Entity.GetLogPrefix(), GetType().Name, RevPath);  //__SILP__
            } else {                                                                                       //__SILP__
                return string.Format("[] [{0}] [{1}] ", GetType().Name, RevPath);                          //__SILP__
            }                                                                                              //__SILP__
        }                                                                                                  //__SILP__
        //SILP: ASPECT_EVENTS_MIXIN()
        public virtual void OnAdded() {}                              //__SILP__
        public virtual void OnRemoved() {}                            //__SILP__
    }
}
