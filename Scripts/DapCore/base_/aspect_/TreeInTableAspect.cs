using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class TreeInTableAspect<TO, T> : TreeInTable<TO, T>, IInTableAspect<TO>
                                                        where TO : ITableSection
                                                        where T : class, IInTreeElement {
        //SILP:IN_TABLE_MIXIN_CONSTRUCTOR(TreeInTableAspect)
        protected TreeInTableAspect(TO owner, int index, Pass pass) : base(owner, index, pass) {  //__SILP__
        }                                                                                         //__SILP__

        //SILP: ASPECT_MIXIN()
        public IEntity GetEntity() {                                  //__SILP__
            return Owner.GetEntity();                                 //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public IEntity Entity {                                       //__SILP__
            get { return Owner.GetEntity(); }                         //__SILP__
        }                                                             //__SILP__
    }
}
