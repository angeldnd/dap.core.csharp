using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class TreeInTree<TO, T> : TreeElement<TO, T>, IInTreeElement
                                                        where TO : ITree
                                                        where T : class, IInTreeElement {
        //SILP: IN_TREE_MIXIN(TreeInTree)
        protected TreeInTree(TO owner, string path, Pass pass) : base(owner, pass) {  //__SILP__
            _Path = path;                                                             //__SILP__
        }                                                                             //__SILP__
                                                                                      //__SILP__
        public ITree OwnerAsTree {                                                    //__SILP__
            get { return Owner; }                                                     //__SILP__
        }                                                                             //__SILP__
                                                                                      //__SILP__
        private readonly string _Path;                                                //__SILP__
        public string Path {                                                          //__SILP__
            get { return _Path; }                                                     //__SILP__
        }                                                                             //__SILP__
                                                                                      //__SILP__
        public override string RevInfo {                                              //__SILP__
            get {                                                                     //__SILP__
                return string.Format("[{0}] ({1}) ", _Path, Revision);                //__SILP__
            }                                                                         //__SILP__
        }                                                                             //__SILP__
    }
}


