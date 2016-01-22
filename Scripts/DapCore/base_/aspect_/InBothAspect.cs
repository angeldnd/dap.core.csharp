using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class InBothAspect<TO> : InBothElement<TO>, IAspect
                                                where TO : IOwner, IContextAccessor {
        //SILP:IN_BOTH_MIXIN_CONSTRUCTOR(InBothAspect)
        protected InBothAspect(TO owner, string path, Pass pass) : base(owner, path, pass) {  //__SILP__
        }                                                                                     //__SILP__
                                                                                              //__SILP__
        protected InBothAspect(TO owner, int index, Pass pass) : base(owner, index, pass) {   //__SILP__
        }                                                                                     //__SILP__

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
