using System;
using System.Collections.Generic;

using System.Linq;

namespace angeldnd.dap {
    public interface IMapping {
        int MappingCount { get; }
        bool HasMapKey(string key);
        string MapKey(string key);
        bool TryMapKey(string key, out string mappedKey);
    }

    public class Mapping : IMapping {
        public readonly string Title;
        public virtual int MappingCount {
            get { return _MappedKeys == null ? 0 : _MappedKeys.Count; }
        }

        private Dictionary<string, string> _MappedKeys = null;

        public Mapping(string title) {
            Title = title;
        }

        public string MapKey(string key) {
            string mappedKey;
            if (TryMapKey(key, out mappedKey)) {
                return mappedKey;
            }
            return key;
        }

        public virtual bool TryMapKey(string key, out string mappedKey) {
            if (_MappedKeys != null && _MappedKeys.TryGetValue(key, out mappedKey)) {
                return true;
            }
            mappedKey = key;
            return false;
        }

        public virtual bool HasMapKey(string key) {
            if (_MappedKeys == null) return false;
            return _MappedKeys.ContainsKey(key);
        }

        public void ForEachMappedKey(Action<string, string> callback) {
            if (_MappedKeys != null) {
                foreach (var kv in _MappedKeys) {
                    callback(kv.Key, kv.Value);
                }
            }
        }

        public bool AddMappedKey(ILogger logger, string key, string mappedKey) {
            if (_MappedKeys == null) {
                _MappedKeys = new Dictionary<string, string>();
            }

            if (HasMapKey(key)) {
                logger.Error("<{0}>.AddMappedKey Failed: Already Exist: : {1} -> {2} -> {3}",
                                GetType().Name, key, MapKey(key), mappedKey);
                return false;
            }
            _MappedKeys[key] = mappedKey;
            return true;
        }

        public bool AddMappedKey(string key, string mappedKey) {
            return AddMappedKey(Env.Instance, key, mappedKey);
        }
    }
}
