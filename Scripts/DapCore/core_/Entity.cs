using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface EntityWatcher {
        void OnAspectAdded(Entity entity, Aspect aspect);
        void OnAspectRemoved(Entity entity, Aspect aspect);
    }

    public struct EntityConsts {
        public const string KeyAspects = "aspects";

        public const char Separator = '.';
    }

    public abstract class Entity : DapObject, Logger {
        public virtual char Separator {
            get { return EntityConsts.Separator; }
        }

        private Dictionary<string, Aspect> _Aspects = new Dictionary<string, Aspect>();

        public int Count {
            get { return _Aspects.Count; }
        }

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

        public delegate void OnAspect<T>(T aspect) where T : class, Aspect;

        public delegate bool CheckAspect<T>(T aspect) where T : class, Aspect;

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
                if (aspect is T) {
                    return (T)aspect;
                } else {
                    Error("Get<{0}>({1}): Type Mismatch: {2}", typeof(T).Name, path, aspect.GetType().Name);
                }
            } else {
                Debug("Get<{0}>({1}): Not Found", typeof(T).Name, path);
            }
            return null;
        }

        public void Filter<T>(string pattern, OnAspect<T> callback) where T : class, Aspect {
            var matcher = new PatternMatcher(Separator, pattern);
            foreach (var pair in _Aspects) {
                /*
                if (LogDebug) {
                    Debug("Check: {0}, {1} -> {2}, {3} -> {4}, {5}",
                        pattern, typeof(T),
                        pair.Key, pair.Value.GetType(),
                        (pair.Value is T), matcher.IsMatched(pair.Key));
                }
                */
                if (pair.Value is T && matcher.IsMatched(pair.Key)) {
                    callback((T)pair.Value);
                }
            }
        }

        public void All<T>(OnAspect<T> callback) where T : class, Aspect {
            Filter<T>(PatternMatcherConsts.WildcastSegments, callback);
        }

        public List<T> Filter<T>(string pattern) where T : class, Aspect {
            List<T> result = null;
            Filter<T>(pattern, (T aspect) => {
                if (result == null) result = new List<T>();
                result.Add(aspect);
            });
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

        protected bool AddAspect(Aspect aspect) {
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
            aspect.OnAdded();

            for (int i = 0; i < _Watchers.Count; i++) {
                _Watchers[i].OnAspectAdded(this, aspect);
            }
            AdvanceRevision();
            return true;
        }

        public Aspect Add(string path, string type) {
            return Add(path, type, null);
        }

        public Aspect Add(string path, string type, Pass pass) {
            if (!Has(path)) {
                Aspect aspect = FactoryAspect(this, path, type);
                if (aspect != null) {
                    if (InitAddAspect(aspect, path, pass)) {
                        return aspect;
                    }
                } else {
                    Error("Failed to Factory Aspect: {0}, {1}", path, type);
                }
            } else {
                Error("Aspect Already Exist: {0}, {1}", path, type);
            }
            return null;
        }

        private bool InitAddAspect(Aspect aspect, string path, Pass pass) {
            if (aspect != null) {
                if (aspect is SecurableAspect) {
                    var sa = aspect as SecurableAspect;
                    return sa.Init(this, path, pass) && AddAspect(sa);
                } else if (pass == null) {
                    return aspect.Init(this, path) && AddAspect(aspect);
                } else {
                    Error("Aspect Is Not Securable: {0}, {1} -> {2}", path, pass, aspect);
                }
            }
            return false;
        }

        public T Add<T>(string path) where T : class, Aspect {
            return Add<T>(path, null);
        }

        public T Add<T>(string path, Pass pass) where T : class, Aspect {
            if (!Has(path)) {
                T aspect = Activator.CreateInstance(typeof(T)) as T;
                if (aspect != null) {
                    if (InitAddAspect(aspect, path, pass)) {
                        return aspect;
                    }
                } else {
                    Error("Failed to Create Aspect: {0}, {1}", path, typeof(T));
                }
            } else {
                Error("Aspect Already Exist: {0}, {1}", path, typeof(T));
            }
            return null;
        }

        public T Remove<T>(string path) where T : class, Aspect {
            return Remove<T>(path, null);
        }

        public T Remove<T>(string path, Pass pass) where T : class, Aspect {
            T aspect = Get<T>(path);
            if (aspect != null) {
                if (aspect is SecurableAspect) {
                    var sa = aspect as SecurableAspect;
                    if (!sa.CheckAdminPass(pass)) {
                        return null;
                    }
                }

                aspect.OnRemoved();
                _Aspects.Remove(path);
                AdvanceRevision();

                for (int i = 0; i < _Watchers.Count; i++) {
                    _Watchers[i].OnAspectRemoved(this, aspect);
                }
                return aspect;
            } else {
                Error("Aspect Not Exist: {0}, {1}", path, typeof(T));
            }
            return null;
        }

        public List<T> RemoveByChecker<T>(CheckAspect<T> checker) where T : class, Aspect {
            return RemoveByChecker<T>(null, checker);
        }

        public List<T> RemoveByChecker<T>(Pass pass, CheckAspect<T> checker) where T : class, Aspect {
            List<T> removed = null;
            List<T> matched = All<T>();
            if (matched != null) {
                foreach (T aspect in matched) {
                    if (checker(aspect)) {
                        T _aspect = Remove<T>(aspect.Path, pass);
                        if (_aspect != null) {
                            if (removed == null) {
                                removed = new List<T>();
                            }
                            removed.Add(_aspect);
                        }
                    }
                }
            }
            return removed;
        }

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

        //SILP: ENTITY_LOG_MIXIN()
        private DebugLogger _DebugLogger = DebugLogger.Instance;                                      //__SILP__
                                                                                                      //__SILP__
        private bool _DebugMode = false;                                                              //__SILP__
        public bool DebugMode {                                                                       //__SILP__
            get { return _DebugMode; }                                                                //__SILP__
            set {                                                                                     //__SILP__
                _DebugMode = true;                                                                    //__SILP__
            }                                                                                         //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        private string[] _DebugPatterns = {""};                                                       //__SILP__
        public virtual string[] DebugPatterns {                                                       //__SILP__
            get { return _DebugPatterns; }                                                            //__SILP__
            set {                                                                                     //__SILP__
                _DebugPatterns = value;                                                               //__SILP__
            }                                                                                         //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public virtual bool LogDebug {                                                                //__SILP__
            get { return _DebugMode || Log.LogDebug; }                                                //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public virtual string GetLogPrefix() {                                                        //__SILP__
            return string.Format("[{0}] ({1}) ", GetType().Name, Revision);                           //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public void Critical(string format, params object[] values) {                                 //__SILP__
            Log.Source = this;                                                                        //__SILP__
            if (DebugMode) {                                                                          //__SILP__
                _DebugLogger.Critical(                                                                //__SILP__
                    _DebugLogger.GetLogHint() + GetLogPrefix() + string.Format(format, values));      //__SILP__
            } else {                                                                                  //__SILP__
                Log.Critical(GetLogPrefix() + string.Format(format, values));                         //__SILP__
            }                                                                                         //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public void Error(string format, params object[] values) {                                    //__SILP__
            Log.Source = this;                                                                        //__SILP__
            if (DebugMode) {                                                                          //__SILP__
                _DebugLogger.Error(                                                                   //__SILP__
                    _DebugLogger.GetLogHint() + GetLogPrefix() + string.Format(format, values));      //__SILP__
            } else {                                                                                  //__SILP__
                Log.Error(GetLogPrefix() + string.Format(format, values));                            //__SILP__
            }                                                                                         //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public void Info(string format, params object[] values) {                                     //__SILP__
            Log.Source = this;                                                                        //__SILP__
            if (DebugMode) {                                                                          //__SILP__
                _DebugLogger.LogWithPatterns(LoggerConsts.INFO, DebugPatterns,                        //__SILP__
                        _DebugLogger.GetLogHint() + GetLogPrefix() + string.Format(format, values));  //__SILP__
            } else {                                                                                  //__SILP__
                Log.Info(GetLogPrefix() + string.Format(format, values));                             //__SILP__
            }                                                                                         //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public void Debug(string format, params object[] values) {                                    //__SILP__
            Log.Source = this;                                                                        //__SILP__
            if (DebugMode) {                                                                          //__SILP__
                _DebugLogger.LogWithPatterns(LoggerConsts.DEBUG, DebugPatterns,                       //__SILP__
                        _DebugLogger.GetLogHint() + GetLogPrefix() + string.Format(format, values));  //__SILP__
            } else {                                                                                  //__SILP__
                Log.Debug(GetLogPrefix() + string.Format(format, values));                            //__SILP__
            }                                                                                         //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
    }
}
