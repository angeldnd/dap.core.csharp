using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class TreeInTreeAspect<TO, T> : TreeInTree<TO, T>, IInTreeAspect
                                                        where TO : ITree, IContextAccessor
                                                        where T : class, IInTreeElement {
        //SILP:IN_TREE_MIXIN_CONSTRUCTOR(TreeInTreeAspect)
        protected TreeInTreeAspect(TO owner, string path, Pass pass) : base(owner, path, pass) {  //__SILP__
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
