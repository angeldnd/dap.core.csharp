using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class TreeInBothAspect<TO, T> : TreeInBoth<TO, T>, IAspect<TO>
                                                        where TO : ISection
                                                        where T : class, IInTreeElement {
        //SILP:IN_BOTH_MIXIN_CONSTRUCTOR(TreeInBothAspect)
        protected TreeInBothAspect(TO owner, string path, Pass pass) : base(owner, path, pass) {  //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        protected TreeInBothAspect(TO owner, int index, Pass pass) : base(owner, index, pass) {   //__SILP__
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
