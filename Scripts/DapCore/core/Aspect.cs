using System;

namespace ADD.Dap {
    public interface Aspect : DapObject, Logger {
        Entity Entity { get; }
        string Path { get; }

        bool Init(Entity entity, string path);
    }

    public abstract class BaseAspect : Aspect {
        //SILP: DAPOBJECT_MIXIN()
        public virtual string Type {                                  //__SILP__
            get { return null; }                                      //__SILP__
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
        private Entity _Entity = null;                                      //__SILP__
        public Entity Entity {                                              //__SILP__
            get { return _Entity; }                                         //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        private string _Path = null;                                        //__SILP__
        public string Path {                                                //__SILP__
            get { return _Path; }                                           //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        private bool _Inited = false;                                       //__SILP__
        public bool Inited {                                                //__SILP__
            get { return _Inited; }                                         //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        public bool Init(Entity entity, string path) {                      //__SILP__
            if (_Inited) return false;                                      //__SILP__
            if (entity == null || string.IsNullOrEmpty(path)) return false; //__SILP__
                                                                            //__SILP__
            _Entity = entity;                                               //__SILP__
            _Path = path;                                                   //__SILP__
            _Inited = true;                                                 //__SILP__
            OnInit();                                                       //__SILP__
            return true;                                                    //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        //SILP: ASPECT_ONINIT_MIXIN()
        protected virtual void OnInit() {}                            //__SILP__
        //SILP: ASPECT_ENCODE_DECODE_MIXIN()
        protected virtual bool DoEncode(Data data) {                  //__SILP__
            return true;                                              //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        protected virtual bool DoDecode(Data data) {                  //__SILP__
            return true;                                              //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        //SILP: ASPECT_LOG_MIXIN()
        private DebugLogger _DebugLogger = DebugLogger.Instance;                                          //__SILP__
                                                                                                          //__SILP__
        public bool DebugMode {                                                                           //__SILP__
            get { return Entity != null && Entity.DebugMode; }                                            //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public bool LogDebug {                                                                            //__SILP__
            get { return (Entity != null && Entity.LogDebug) || Log.LogDebug; }                           //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public virtual string GetLogPrefix() {                                                            //__SILP__
            if (_Entity != null) {                                                                        //__SILP__
                return string.Format("{0}[{1}] ", _Entity.GetLogPrefix(), GetType().Name);                //__SILP__
            } else {                                                                                      //__SILP__
                return string.Format("[] [] [{0}] ", GetType().Name);                                     //__SILP__
            }                                                                                             //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public void Critical(string format, params object[] values) {                                     //__SILP__
            if (DebugMode) {                                                                              //__SILP__
                _DebugLogger.Critical(GetLogPrefix() + string.Format(format, values));                    //__SILP__
            } else {                                                                                      //__SILP__
                Log.Critical(GetLogPrefix() + string.Format(format, values));                             //__SILP__
            }                                                                                             //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public void Error(string format, params object[] values) {                                        //__SILP__
            if (DebugMode) {                                                                              //__SILP__
                _DebugLogger.Error(GetLogPrefix() + string.Format(format, values));                       //__SILP__
            } else {                                                                                      //__SILP__
                Log.Error(GetLogPrefix() + string.Format(format, values));                                //__SILP__
            }                                                                                             //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public void Info(string format, params object[] values) {                                         //__SILP__
            if (DebugMode) {                                                                              //__SILP__
                _DebugLogger.LogWithPatterns(LoggerConsts.INFO, Entity.DebugPatterns,                     //__SILP__
                        GetLogPrefix() + _DebugLogger.GetMethodPrefix() + string.Format(format, values)); //__SILP__
            } else {                                                                                      //__SILP__
                Log.Info(GetLogPrefix() + string.Format(format, values));                                 //__SILP__
            }                                                                                             //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public void Debug(string format, params object[] values) {                                        //__SILP__
            if (DebugMode) {                                                                              //__SILP__
                _DebugLogger.LogWithPatterns(LoggerConsts.DEBUG, Entity.DebugPatterns,                    //__SILP__
                        GetLogPrefix() + _DebugLogger.GetMethodPrefix() + string.Format(format, values)); //__SILP__
            } else {                                                                                      //__SILP__
                Log.Debug(GetLogPrefix() + string.Format(format, values));                                //__SILP__
            }                                                                                             //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
    }

    public abstract class EntityAspect : Entity, Aspect {
        public override Aspect FactoryAspect(Entity entity, string path, string type) {
            if (Entity != null) {
                return Entity.FactoryAspect(entity, path, type);
            }
            return null;
        }
        //SILP: ENTITY_ASPECT_LOG_MIXIN()
        public override bool LogDebug {                                                              //__SILP__
            get { return base.LogDebug || (_Entity != null && _Entity.LogDebug); }                   //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        public override string GetLogPrefix() {                                                      //__SILP__
            if (Entity != null) {                                                                    //__SILP__
                return string.Format("{0}[{1}] [{2}]", Entity.GetLogPrefix(), GetType().Name, Name); //__SILP__
            } else {                                                                                 //__SILP__
                return string.Format("[] [] [{0}] [{1}] ", GetType().Name, Name);                    //__SILP__
            }                                                                                        //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        public override string[] DebugPatterns {                                                     //__SILP__
            get {                                                                                    //__SILP__
                string[] basePatterns = base.DebugPatterns;                                          //__SILP__
                string[] entityPatterns = null;                                                      //__SILP__
                if (_Entity != null) {                                                               //__SILP__
                    entityPatterns = _Entity.DebugPatterns;                                          //__SILP__
                }                                                                                    //__SILP__
                if (basePatterns == null || basePatterns.Length == 0) {                              //__SILP__
                    return entityPatterns;                                                           //__SILP__
                } else if (entityPatterns == null || entityPatterns.Length == 0) {                   //__SILP__
                    return basePatterns;                                                             //__SILP__
                }                                                                                    //__SILP__
                string[] result = new string[basePatterns.Length + entityPatterns.Length];           //__SILP__
                basePatterns.CopyTo(result, 0);                                                      //__SILP__
                entityPatterns.CopyTo(result, basePatterns.Length);                                  //__SILP__
                return result;                                                                       //__SILP__
            }                                                                                        //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        //SILP: ASPECT_MIXIN()
        private Entity _Entity = null;                                      //__SILP__
        public Entity Entity {                                              //__SILP__
            get { return _Entity; }                                         //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        private string _Path = null;                                        //__SILP__
        public string Path {                                                //__SILP__
            get { return _Path; }                                           //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        private bool _Inited = false;                                       //__SILP__
        public bool Inited {                                                //__SILP__
            get { return _Inited; }                                         //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        public bool Init(Entity entity, string path) {                      //__SILP__
            if (_Inited) return false;                                      //__SILP__
            if (entity == null || string.IsNullOrEmpty(path)) return false; //__SILP__
                                                                            //__SILP__
            _Entity = entity;                                               //__SILP__
            _Path = path;                                                   //__SILP__
            _Inited = true;                                                 //__SILP__
            OnInit();                                                       //__SILP__
            return true;                                                    //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        //SILP: ASPECT_ONINIT_MIXIN()
        protected virtual void OnInit() {}                            //__SILP__
    }
}
