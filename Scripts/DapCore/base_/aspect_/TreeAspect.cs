using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class TreeAspect<TO, T> : TreeElement<TO, T>, IAspect
                                                        where TO : IOwner, IContextAccessor
                                                        where T : class, IInTreeElement {
        //SILP:ELEMENT_MIXIN_CONSTRUCTOR(TreeAspect)
        protected TreeAspect(TO owner, Pass pass) : base(owner, pass) {  //__SILP__
        }                                                                //__SILP__

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
