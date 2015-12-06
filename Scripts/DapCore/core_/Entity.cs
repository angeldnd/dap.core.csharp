using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface EntityWatcher {
        void OnAspectAdded(Entity entity, Aspect aspect);
        void OnAspectRemoved(Entity entity, Aspect aspect);
    }

    public static class EntityConsts {
        public const string KeyAspects = "aspects";

        public const char Separator = '.';
    }

    public abstract class Entity : BaseDapObject {
        public virtual char Separator {
            get { return EntityConsts.Separator; }
        }

        private Dictionary<string, Aspect> _Aspects = new Dictionary<string, Aspect>();

        public int AspectsCount {
            get { return _Aspects.Count; }
        }

        public ICollection<string> AspectsKeys {
            get {
                return _Aspects.Keys;
            }
        }

        public delegate void OnAspect<T>(T aspect) where T : class, Aspect;

        public delegate bool CheckAspect<T>(T aspect) where T : class, Aspect;

        //SILP: DECLARE_LIST(Watcher, watcher, EntityWatcher, _Watchers)
        protected List<EntityWatcher> _Watchers = null;                    //__SILP__
                                                                           //__SILP__
        public bool AddWatcher(EntityWatcher watcher) {                    //__SILP__
            if (_Watchers == null) _Watchers = new List<EntityWatcher>();  //__SILP__
            if (!_Watchers.Contains(watcher)) {                            //__SILP__
                _Watchers.Add(watcher);                                    //__SILP__
                return true;                                               //__SILP__
            }                                                              //__SILP__
            return false;                                                  //__SILP__
        }                                                                  //__SILP__
                                                                           //__SILP__
        public bool RemoveWatcher(EntityWatcher watcher) {                 //__SILP__
            if (_Watchers != null && _Watchers.Contains(watcher)) {        //__SILP__
                _Watchers.Remove(watcher);                                 //__SILP__
                return true;                                               //__SILP__
            }                                                              //__SILP__
            return false;                                                  //__SILP__
        }                                                                  //__SILP__
                                                                           //__SILP__

        public bool Has(string path) {
            return _Aspects.ContainsKey(path);
        }

        public T Get<T>(string path) where T : class, Aspect {
            Aspect aspect = null;
            if (_Aspects.TryGetValue(path, out aspect)) {
                if (aspect is T) {
                    return (T)aspect;
                } else {
                    Error("Get<{0}>({1}): Type Mismatched: {2}", typeof(T).Name, path, aspect.GetType().Name);
                }
            } else {
                Debug("Get<{0}>({1}): Not Found", typeof(T).Name, path);
            }
            return null;
        }

        public void Filter<T>(string pattern, OnAspect<T> callback) where T : class, Aspect {
            var matcher = new PatternMatcher(Separator, pattern);
            var en = _Aspects.GetEnumerator();
            while (en.MoveNext()) {
                /*
                if (LogDebug) {
                    Debug("Check: {0}, {1} -> {2}, {3} -> {4}, {5}",
                        pattern, typeof(T),
                        pair.Key, pair.Value.GetType(),
                        (pair.Value is T), matcher.IsMatched(pair.Key));
                }
                */
                if (en.Current.Value is T && matcher.IsMatched(en.Current.Key)) {
                    callback((T)en.Current.Value);
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

            if (_Watchers != null) {
                for (int i = 0; i < _Watchers.Count; i++) {
                    _Watchers[i].OnAspectAdded(this, aspect);
                }
            }
            AdvanceRevision();
            return true;
        }

        public Aspect Add(string path, string type) {
            return Add(path, type, null);
        }

        public Aspect Add(string path, string type, Pass pass) {
            if (!Has(path)) {
                Aspect aspect = Factory.NewAspect(type);
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

                if (_Watchers != null) {
                    for (int i = 0; i < _Watchers.Count; i++) {
                        _Watchers[i].OnAspectRemoved(this, aspect);
                    }
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
    }
}
