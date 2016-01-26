using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class InBothAspect<TO> : InBothElement<TO>, IAspect
                                                where TO : class, IOwner, IContextAccessor {
        //SILP:IN_BOTH_MIXIN_CONSTRUCTOR(InBothAspect)
        protected InBothAspect(TO owner, string key) : base(owner, key) {   //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        protected InBothAspect(TO owner, int index) : base(owner, index) {  //__SILP__
        }                                                                   //__SILP__

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
