using System;
using System.Collections.Generic;


namespace angeldnd.dap {
    public class Mappings : Mapping {
        public override int Count {
            get { return base.Count + (_Mappings == null ? 0 : _Mappings.Count); }
        }

        private List<IMapping> _Mappings = null;

        public Mappings(string title) : base(title) {
        }

        public override bool TryMapKey(string key, out string mappedKey) {
            if (base.TryMapKey(key, out mappedKey)) {
                return true;
            }
            if (_Mappings != null) {
                foreach (IMapping mapping in _Mappings) {
                    if (mapping.TryMapKey(key, out mappedKey)) {
                        return true;
                    }
                }
            }
            mappedKey = key;
            return false;
        }

        public void ForEachMapping(Action<IMapping> callback) {
            if (_Mappings != null) {
                foreach (var mapping in _Mappings) {
                    callback(mapping);
                }
            }
        }

        public bool AddMapping(IMapping mapping) {
            if (_Mappings == null) {
                _Mappings = new List<IMapping>();
            }
            if (_Mappings.Contains(mapping)) {
                return false;
            }
            _Mappings.Add(mapping);
            return true;
        }
    }
}
