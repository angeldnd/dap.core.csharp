using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class DictInBothAspect<TO, T> : DictInBoth<TO, T>, IAspect
                                                        where TO : class, IOwner, IContextAccessor
                                                        where T : class, IInDictElement {
        //SILP:IN_BOTH_MIXIN_CONSTRUCTOR(DictInBothAspect)
        protected DictInBothAspect(TO owner, string key) : base(owner, key) {   //__SILP__
        }                                                                       //__SILP__
                                                                                //__SILP__
        protected DictInBothAspect(TO owner, int index) : base(owner, index) {  //__SILP__
        }                                                                       //__SILP__

        //SILP: ASPECT_MIXIN()
        public IContext GetContext() {                                //__SILP__
            return Owner.GetContext();                                //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public IContext Context {                                     //__SILP__
            get { return Owner.GetContext(); }                        //__SILP__
        }                                                             //__SILP__
    }
}
