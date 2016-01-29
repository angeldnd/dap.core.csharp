using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class DictInBothAspect<TO, T> : DictInBoth<TO, T>, IAspect
                                                        where TO : class, IOwner, IContextAccessor
                                                        where T : class, IInDictElement {
        //SILP:IN_BOTH_ASPECT_MIXIN_CONSTRUCTOR(DictInBothAspect)
        protected DictInBothAspect(TO owner, string key) : base(owner, key) {   //__SILP__
            _Context = owner == null ? null : owner.GetContext();               //__SILP__
            _Path = Env.GetAspectPath(this);                                    //__SILP__
            _Uri = Env.GetAspectUri(this);                                      //__SILP__
            _DebugMode = Env.GetAspectDebugMode(this);                          //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        protected DictInBothAspect(TO owner, int index) : base(owner, index) {  //__SILP__
            _Context = owner == null ? null : owner.GetContext();               //__SILP__
            _Path = Env.GetAspectPath(this);                                    //__SILP__
            _Uri = Env.GetAspectUri(this);                                      //__SILP__
            _DebugMode = Env.GetAspectDebugMode(this);                          //__SILP__
        }                                                                       //__SILP__

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
        private readonly bool _DebugMode = false;                     //__SILP__
        public override sealed bool DebugMode {                       //__SILP__
            get { return _DebugMode; }                                //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public override void OnAdded() {                              //__SILP__
            Env.Instance.Hooks._OnAspectAdded(this);                  //__SILP__
        }                                                             //__SILP__
    }
}
