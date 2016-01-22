using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class TableInBothAspect<TO, T> : TableInBoth<TO, T>, IAspect
                                                        where TO : IOwner, IContextAccessor
                                                        where T : class, IInTableElement {
        //SILP:IN_BOTH_MIXIN_CONSTRUCTOR(TableInBothAspect)
        protected TableInBothAspect(TO owner, string path, Pass pass) : base(owner, path, pass) {  //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        protected TableInBothAspect(TO owner, int index, Pass pass) : base(owner, index, pass) {   //__SILP__
        }                                                                                          //__SILP__

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
