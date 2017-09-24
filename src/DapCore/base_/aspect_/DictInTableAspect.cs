using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class DictInTableAspect<TO, T> : DictInTable<TO, T>, IDictAspect
                                                        where TO : class, ITable, IContextElement
                                                        where T : class, IInDictElement {
        //SILP:IN_TABLE_ASPECT_MIXIN_CONSTRUCTOR(DictInTableAspect)
        protected DictInTableAspect(TO owner, int index) : base(owner, index) {  //__SILP__
            _Context = owner == null ? null : owner.GetContext();                //__SILP__
            _Path = Root.GetAspectPath(this);                                    //__SILP__
            _Uri = Root.GetAspectUri(this);                                      //__SILP__
        }                                                                        //__SILP__
                                                                                 //__SILP__
        public override bool _SetIndex(IOwner owner, int index) {                //__SILP__
            if (!base._SetIndex(owner, index)) return false;                     //__SILP__
                                                                                 //__SILP__
            _Path = Root.GetAspectPath(this);                                    //__SILP__
            _Uri = Root.GetAspectUri(this);                                      //__SILP__
            return true;                                                         //__SILP__
        }                                                                        //__SILP__

        //SILP: ASPECT_MIXIN()
        private readonly IContext _Context;                                            //__SILP__
        public IContext GetContext() {                                                 //__SILP__
            return _Context;                                                           //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        public IContext Context {                                                      //__SILP__
            get { return _Context; }                                                   //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        private string _Path;                                                          //__SILP__
        public string Path {                                                           //__SILP__
            get { return _Path; }                                                      //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        private string _Uri;                                                           //__SILP__
        public override sealed string Uri {                                            //__SILP__
            get {                                                                      //__SILP__
                return _Uri;                                                           //__SILP__
            }                                                                          //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        public override string BlockName {                                             //__SILP__
            get {                                                                      //__SILP__
                return string.Format("{0}<{1}>", GetType().Name, _Context.BlockName);  //__SILP__
            }                                                                          //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        private bool _Debugging = false;                                               //__SILP__
        public bool Debugging {                                                        //__SILP__
            get { return _Debugging; }                                                 //__SILP__
            set { _Debugging = value; }                                                //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        public override sealed bool DebugMode {                                        //__SILP__
            get { return _Debugging || base.DebugMode; }                               //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        protected override sealed void OnAdded() {                                     //__SILP__
            if (_Context != null) {                                                    //__SILP__
                _Context.Root.Hooks._OnAspectAdded(this);                              //__SILP__
            }                                                                          //__SILP__
            OnAspectAdded();                                                           //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        protected override sealed void OnRemoved() {                                   //__SILP__
            if (_Context != null) {                                                    //__SILP__
                //TODO: Add AspectWatcher to Context                                   //__SILP__
            }                                                                          //__SILP__
            OnAspectRemoved();                                                         //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        protected virtual void OnAspectAdded() {}                                      //__SILP__
        protected virtual void OnAspectRemoved() {}                                    //__SILP__
                                                                                       //__SILP__
    }
}
