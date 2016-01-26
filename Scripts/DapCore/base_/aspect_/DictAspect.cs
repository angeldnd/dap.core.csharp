using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class DictAspect<TO, T> : DictElement<TO, T>, IAspect
                                                        where TO : class, IOwner, IContextAccessor
                                                        where T : class, IInDictElement {
        //SILP:ELEMENT_MIXIN_CONSTRUCTOR(DictAspect)
        protected DictAspect(TO owner) : base(owner) {                //__SILP__
        }                                                             //__SILP__

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
