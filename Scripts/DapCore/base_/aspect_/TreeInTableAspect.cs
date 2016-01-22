using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class TreeInTableAspect<TO, T> : TreeInTable<TO, T>, IInTableAspect
                                                        where TO : ITable, IContextAccessor
                                                        where T : class, IInTreeElement {
        //SILP:IN_TABLE_MIXIN_CONSTRUCTOR(TreeInTableAspect)
        protected TreeInTableAspect(TO owner, int index, Pass pass) : base(owner, index, pass) {  //__SILP__
        }                                                                                         //__SILP__

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
