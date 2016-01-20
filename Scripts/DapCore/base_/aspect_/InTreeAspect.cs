using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class InTreeAspect<TO> : InTreeElement<TO>, IInTreeAspect<TO>
                                                where TO : ITreeSection {
        //SILP:IN_TREE_MIXIN_CONSTRUCTOR(InTreeAspect)
        protected InTreeAspect(TO owner, string path, Pass pass) : base(owner, path, pass) {  //__SILP__
        }                                                                                     //__SILP__

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
