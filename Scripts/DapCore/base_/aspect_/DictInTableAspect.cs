using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class DictInTableAspect<TO, T> : DictInTable<TO, T>, IAspect
                                                        where TO : class, ITable, IContextElement
                                                        where T : class, IInDictElement {
        //SILP:IN_TABLE_ASPECT_MIXIN_CONSTRUCTOR(DictInTableAspect)
        protected DictInTableAspect(TO owner, int index) : base(owner, index) {  //__SILP__
            _Context = owner == null ? null : owner.GetContext();                //__SILP__
            _Path = Env.GetAspectPath(this);                                     //__SILP__
            _Uri = Env.GetAspectUri(this);                                       //__SILP__
        }                                                                        //__SILP__

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
        public override void OnAdded() {                              //__SILP__
            if (_Context != null) {                                   //__SILP__
                Env.Instance.Hooks._OnAspectAdded(this);              //__SILP__
            }                                                         //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public override void OnRemoved() {                            //__SILP__
            if (_Context != null) {                                   //__SILP__
                Env.Instance.Hooks._OnAspectRemoved(this);            //__SILP__
            }                                                         //__SILP__
        }                                                             //__SILP__
    }
}
