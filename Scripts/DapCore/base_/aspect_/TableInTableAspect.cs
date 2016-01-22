using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class TableInTableAspect<TO, T> : TableInTable<TO, T>, IInTableAspect
                                                        where TO : ITable, IContextAccessor
                                                        where T : class, IInTableElement {
        //SILP:IN_TABLE_MIXIN_CONSTRUCTOR(TableInTableAspect)
        protected TableInTableAspect(TO owner, int index, Pass pass) : base(owner, index, pass) {  //__SILP__
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
