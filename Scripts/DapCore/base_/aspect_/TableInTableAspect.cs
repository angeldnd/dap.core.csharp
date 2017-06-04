using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class TableInTableAspect<TO, T> : TableInTable<TO, T>, IAspect
                                                        where TO : class, ITable, IContextElement
                                                        where T : class, IInTableElement {
        //SILP:IN_TABLE_ASPECT_MIXIN_CONSTRUCTOR(TableInTableAspect)
        protected TableInTableAspect(TO owner, int index) : base(owner, index) {  //__SILP__
            _Context = owner == null ? null : owner.GetContext();                 //__SILP__
            _Path = Env.GetAspectPath(this);                                      //__SILP__
            _Uri = Env.GetAspectUri(this);                                        //__SILP__
        }                                                                         //__SILP__

        //SILP: ASPECT_MIXIN()
        private readonly IContext _Context;                           //__SILP__
        public IContext GetContext() {                                //__SILP__
            return _Context;                                          //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public IContext Context {                                     //__SILP__
            get { return _Context; }                                  //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        private readonly string _Path;                                //__SILP__
        public string Path {                                          //__SILP__
            get { return _Path; }                                     //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        private readonly string _Uri;                                 //__SILP__
        public override sealed string Uri {                           //__SILP__
            get {                                                     //__SILP__
                return _Uri;                                          //__SILP__
            }                                                         //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        private bool _Debugging = false;                              //__SILP__
        public bool Debugging {                                       //__SILP__
            get { return _Debugging; }                                //__SILP__
            set { _Debugging = value; }                               //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public override sealed bool DebugMode {                       //__SILP__
            get { return _Debugging || base.DebugMode; }              //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        protected override sealed void OnAdded() {                    //__SILP__
            if (_Context != null) {                                   //__SILP__
                Env.Instance.Hooks._OnAspectAdded(this);              //__SILP__
            }                                                         //__SILP__
            OnAspectAdded();                                          //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        protected override sealed void OnRemoved() {                  //__SILP__
            if (_Context != null) {                                   //__SILP__
                //TODO: Add AspectWatcher to Context                  //__SILP__
            }                                                         //__SILP__
            OnAspectRemoved();                                        //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        protected virtual void OnAspectAdded() {}                     //__SILP__
        protected virtual void OnAspectRemoved() {}                   //__SILP__
                                                                      //__SILP__
        //SILP: TABLE_ASPECT_MIXIN()
        protected override void AddSummaryFields(Data summary) {                        //__SILP__
            base.AddSummaryFields(summary);                                             //__SILP__
            summary.S(ContextConsts.SummaryPath, _Path)                                 //__SILP__
                   .B(ContextConsts.SummaryDebugging, _Debugging)                       //__SILP__
                   .I(ContextConsts.SummaryWatcherCount, TableWatcherCount)             //__SILP__
                   .I(ContextConsts.Summary2ndWatcherCount, GenericTableWatcherCount);  //__SILP__
        }                                                                               //__SILP__
    }
}
