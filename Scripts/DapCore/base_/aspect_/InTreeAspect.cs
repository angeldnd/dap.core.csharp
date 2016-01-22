using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class InTreeAspect<TO> : InTreeElement<TO>, IInTreeAspect
                                                where TO : ITree, IContextAccessor {
        //SILP:IN_TREE_MIXIN_CONSTRUCTOR(InTreeAspect)
        protected InTreeAspect(TO owner, string path, Pass pass) : base(owner, path, pass) {  //__SILP__
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
