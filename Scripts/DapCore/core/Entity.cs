using System;
using System.Collections.Generic;

namespace ADD.Dap {
    public interface EntityWatcher {
        void OnEntityAspectAdded(Entity entity, Aspect aspect);
        void OnEntityAspectRemoved(Entity entity, Aspect aspect);
    }

    public struct EntityConsts {
        public const string KeyAspects = "aspects";

        public const char Separator = '.';
    }

    public abstract class Entity : DapObject, Logger {
        public virtual char Separator {
            get { return EntityConsts.Separator; }
        }

        private string _Name = string.Empty;
        public string Name {
            get { return _Name; }
        }
        public void SetName(string name) {
            _Name = name;
        }

        private Dictionary<string, Aspect> _Aspects = new Dictionary<string, Aspect>();

        public Dictionary<string, Aspect>.KeyCollection AllPathes {
            get {
                return _Aspects.Keys;
            }
        }

        public Dictionary<string, Aspect>.ValueCollection AllAspects {
            get {
                return _Aspects.Values;
            }
        }

        private List<EntityWatcher> _Watchers = new List<EntityWatcher>();

        protected virtual bool DoEncode(Data data) {
            Data aspectsData = new Data();
            foreach (var pair in _Aspects) {
                Data aspectData = pair.Value.Encode();
                aspectsData.SetData(pair.Key, aspectData);
            }
            return data.SetData(EntityConsts.KeyAspects, aspectsData);
        }

        protected virtual bool DoDecode(Data data) {
            Data aspectsData = data.GetData(EntityConsts.KeyAspects);
            if (aspectsData != null) {
                return DecodeAspects(aspectsData) > 0;
            }
            return false;
        }

        public int DecodeAspects(Data aspectsData) {
            int succeedCount = 0;
            int failedCount = 0;
            foreach (var path in aspectsData.Keys) {
                bool succeed = false;
                Data aspectData = aspectsData.GetData(path);
                if (aspectData != null) {
                    Aspect aspect = GetAspect(path);
                    if (aspect != null) {
                        succeed = aspect.Decode(aspectData);
                    } else {
                        string type = aspectData.GetString(DapObjectConsts.KeyType);
                        if (!string.IsNullOrEmpty(type)) {
                            aspect = FactoryAspect(this, path, type);
                            if (aspect != null && aspect.Init(this, path) && aspect.Decode(aspectData)) {
                                SetAspect(aspect);
                                succeed = true;
                            }
                        }
                    }
                }
                if (succeed) {
                    succeedCount++;
                } else {
                    failedCount++;
                }
            }
            if (failedCount > 0) {
                Error("DecodeAspects: succeedCount = {0}, failedCount = {1}", succeedCount, failedCount);
            }
            return succeedCount;
        }

        public virtual Aspect FactoryAspect(Entity entity, string path, string type) {
            return null;
        }

        public bool AddWatcher(EntityWatcher watcher) {
            if (!_Watchers.Contains(watcher)) {
                _Watchers.Add(watcher);
                return true;
            }
            return false;
        }

        public bool RemoveWatcher(EntityWatcher watcher) {
            if (_Watchers.Contains(watcher)) {
                _Watchers.Remove(watcher);
                return true;
            }
            return false;
        }

        public bool Has(string path) {
            return _Aspects.ContainsKey(path);
        }

        public T Get<T>(string path) where T : class, Aspect {
            Aspect aspect = null;
            if (_Aspects.TryGetValue(path, out aspect)) {
                return aspect as T;
            }
            return null;
        }

        public List<T> Filter<T>(string pattern) where T : class, Aspect {
            List<T> result = null;
            var matcher = new PatternMatcher(Separator, pattern);
            foreach (var pair in _Aspects) {
                if (pair.Value is T && matcher.IsMatched(pair.Key)) {
                    if (result == null) result = new List<T>();
                    result.Add(pair.Value as T);
                }
            }
            return result;
        }

        public List<T> All<T>() where T : class, Aspect {
            return Filter<T>(PatternMatcherConsts.WildcastSegments);
        }

        public Aspect GetAspect(string path) {
            Aspect aspect = null;
            if (_Aspects.TryGetValue(path, out aspect)) {
                return aspect;
            }
            return null;
        }

        protected bool SetAspect(Aspect aspect) {
            Aspect oldAspect = GetAspect(aspect.Path);
            if (oldAspect != null) {
                Error("Aspect Exist: {0}, {1}, {2}", aspect.Path, oldAspect, aspect);
                return false;
            }
            if (aspect.Entity != this) {
                Error("Invalid Aspect: {0}", aspect);
                return false;
            }
            _Aspects[aspect.Path] = aspect;
            for (int i = 0; i < _Watchers.Count; i++) {
                _Watchers[i].OnEntityAspectAdded(this, aspect);
            }
            return true;
        }

        public T Add<T>(string path) where T : class, Aspect {
            if (!Has(path)) {
                T aspect = Activator.CreateInstance(typeof(T)) as T;
                if (aspect != null && aspect.Init(this, path)) {
                    SetAspect(aspect);
                }
                return aspect;
            }
            return null;
        }

        public T Remove<T>(string path) where T : class, Aspect {
            T aspect = Get<T>(path);
            if (aspect != null) {
                _Aspects.Remove(path);
                for (int i = 0; i < _Watchers.Count; i++) {
                    _Watchers[i].OnEntityAspectRemoved(this, aspect);
                }
                return aspect;
            }
            return null;
        }

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

        //SILP: ENTITY_LOG_MIXIN()
        private DebugLogger _DebugLogger = DebugLogger.Instance;                                          //__SILP__
                                                                                                          //__SILP__
        private bool _DebugMode = false;                                                                  //__SILP__
        public bool DebugMode {                                                                           //__SILP__
            get { return _DebugMode; }                                                                    //__SILP__
            set {                                                                                         //__SILP__
                _DebugMode = true;                                                                        //__SILP__
            }                                                                                             //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        private string[] _DebugPatterns = {""};                                                           //__SILP__
        public virtual string[] DebugPatterns {                                                           //__SILP__
            get { return _DebugPatterns; }                                                                //__SILP__
            set {                                                                                         //__SILP__
                _DebugPatterns = value;                                                                   //__SILP__
            }                                                                                             //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public virtual bool LogDebug {                                                                    //__SILP__
            get { return _DebugMode || Log.LogDebug; }                                                    //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public virtual string GetLogPrefix() {                                                            //__SILP__
            return string.Format("[{0}] [{1}] ", GetType().Name, Name);                                   //__SILP__
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
                _DebugLogger.LogWithPatterns(LoggerConsts.INFO, DebugPatterns,                            //__SILP__
                        GetLogPrefix() + _DebugLogger.GetMethodPrefix() + string.Format(format, values)); //__SILP__
            } else {                                                                                      //__SILP__
                Log.Info(GetLogPrefix() + string.Format(format, values));                                 //__SILP__
            }                                                                                             //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public void Debug(string format, params object[] values) {                                        //__SILP__
            if (DebugMode) {                                                                              //__SILP__
                _DebugLogger.LogWithPatterns(LoggerConsts.DEBUG, DebugPatterns,                           //__SILP__
                        GetLogPrefix() + _DebugLogger.GetMethodPrefix() + string.Format(format, values)); //__SILP__
            } else {                                                                                      //__SILP__
                Log.Debug(GetLogPrefix() + string.Format(format, values));                                //__SILP__
            }                                                                                             //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
    }
}
