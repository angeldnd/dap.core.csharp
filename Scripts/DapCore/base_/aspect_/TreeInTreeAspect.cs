using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class TreeInTreeAspect<TO, T> : TreeInTree<TO, T>, IAspect<TO>
                                                        where TO : ISection, ITree
                                                        where T : class, IInTreeElement {
        //SILP:IN_TREE_MIXIN_CONSTRUCTOR(TreeInTreeAspect)
        protected TreeInTreeAspect(TO owner, string path, Pass pass) : base(owner, path, pass) {  //__SILP__
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
