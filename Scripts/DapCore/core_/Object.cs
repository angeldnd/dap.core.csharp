using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IObject : ILogger, IBlockOwner {
        string DapType { get; }

        int Revision { get; }
        string RevInfo { get; }

        string Uri { get; }
        bool DebugMode { get; }
        string LogPrefix { get; }

        Data Summary { get; }
    }

    public static class ObjectConsts {
        public const string KeyDapType = "dap_type";

        [DapParam(typeof(string))]
        public const string SummaryType = "type";
        [DapParam(typeof(string))]
        public const string SummaryDapType = "dap_type";
        [DapParam(typeof(int))]
        public const string SummaryRevision = "rev";
        [DapParam(typeof(string))]
        public const string SummaryUri = "uri";
        [DapParam(typeof(bool))]
        public const string SummaryDebugMode = "debug_mode";
        [DapParam(typeof(int))]
        public const string SummaryBlockCount = "block_count";
    }

    public abstract class Object : Logger, IObject {
        private string _DapType = null;
        public string DapType {
            get {
                if (_DapType == null) {
                    _DapType = angeldnd.dap.DapType.GetDapType(GetType());
                }
                return _DapType;
            }
        }

        private int _Revision = 0;
        public int Revision {
            get { return _Revision; }
        }

        public virtual string RevInfo {
            get { return string.Format("({0})", _Revision); }
        }

        protected void AdvanceRevision() {
            _Revision += 1;
        }

        public virtual string Uri {
            get { return "_"; }
        }

        public override string LogPrefix {
            get {
                string dapType = DapType;
                if (dapType != null) {
                    return string.Format("[{0}] [{1}] {2} ", dapType, Uri, RevInfo);
                }
                return string.Format("_[{0}] [{1}] {2} ", GetType().FullName, Uri, RevInfo);
            }
        }

        public override string ToString() {
            return Summary.ToFullString(null);
        }

        protected virtual void AddSummaryFields(Data summary) {
            summary.I(ObjectConsts.SummaryBlockCount, _Blocks == null ? 0 : _Blocks.Count);
        }

        public Data Summary {
            get {
                Data summary = DataCache.Take("_summary")
                        .S(ObjectConsts.SummaryType, GetType().FullName)
                        .S(ObjectConsts.SummaryDapType, DapType)
                        .I(ObjectConsts.SummaryRevision, _Revision)
                        .S(ObjectConsts.SummaryUri, Uri)
                        .B(ObjectConsts.SummaryDebugMode, DebugMode);

                AddSummaryFields(summary);
                return summary;
            }
        }

        //SILP:BLOCK_OWNER()
        private List<WeakBlock> _Blocks = null;                       //__SILP__
                                                                      //__SILP__
        public void AddBlock(WeakBlock block) {                       //__SILP__
            if (_Blocks == null) {                                    //__SILP__
                _Blocks = new List<WeakBlock>();                      //__SILP__
            }                                                         //__SILP__
            if (!_Blocks.Contains(block)) {                           //__SILP__
                _Blocks.Add(block);                                   //__SILP__
            }                                                         //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public void RemoveBlock(WeakBlock block) {                    //__SILP__
            if (_Blocks == null) {                                    //__SILP__
                return;                                               //__SILP__
            }                                                         //__SILP__
            int index = _Blocks.IndexOf(block);                       //__SILP__
            if (index >= 0) {                                         //__SILP__
                _Blocks.RemoveAt(index);                              //__SILP__
            }                                                         //__SILP__
        }                                                             //__SILP__
    }
}
