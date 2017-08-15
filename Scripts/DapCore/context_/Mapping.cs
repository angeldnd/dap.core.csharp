using System;
using System.Collections.Generic;

using System.Linq;

namespace angeldnd.dap {
    public sealed class Mapping : Aspect<IContext> {
        public int Count {
            get { return _Mappings == null ? 0 : _Mappings.Count; }
        }

        private Data _Mappings = null;
        public Data Mappings {
            get { return _Mappings; }
        }

        private Data _AspectsSummary = null;
        public Data AspectsSummary {
            get { return _AspectsSummary; }
        }

        public Mapping(IContext obj, string key) : base(obj, key) {
        }

        public void _AddAspectSummary(string path, Data aspect) {
            if (_AspectsSummary == null) {
                _AspectsSummary = new RealData();
            }
            _AspectsSummary.A(path, aspect);
        }

        protected override void AddSummaryFields(Data summary) {
            base.AddSummaryFields(summary);
            if (_Mappings != null) {
                summary.A(ContextConsts.SummaryMappings, _Mappings);
            }
            if (_AspectsSummary != null) {
                summary.A(ContextConsts.SummaryAspects, _AspectsSummary);
            }
        }

        public string Map(string key) {
            if (_Mappings == null) return key;
            return _Mappings.GetString(key, key);
        }

        public bool AddMap(string from, string to) {
            if (_Mappings == null) {
                _Mappings = new RealData();
            }

            if (_Mappings.HasKey(from)) {
                Error("AddMap Failed: Already Exist: : {0}: {1} -> {2}",
                                from, Map(from), to);
                return false;
            }
            return _Mappings.SetString(from, to);
        }

        public bool RemoveMap(string from, string to) {
            if (_Mappings == null) return false;

            string old = Map(from);
            if (old != to) {
                Error("RemoveMap Failed: Not Matched: {0}: {1} -> {2}", from, old, to);
                return false;
            }

            RealData newMappings = new RealData();
            foreach (var k in _Mappings.Keys) {
                if (k != from) {
                    newMappings.S(k, _Mappings.GetString(k, k));
                }
            }
            _Mappings = newMappings;
            return true;
        }
    }
}
