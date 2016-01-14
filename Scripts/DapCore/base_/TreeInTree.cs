using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class TreeInTree<TO, T> : TreeElement<TO, T>, IInTreeElement<TO>
                                                        where TO : ITree
                                                        where T : class, IInTreeElement {
        //SILP: IN_TREE_MIXIN(TreeInTree, TO)
        private readonly string _Path;                                                //__SILP__
        public string Path {                                                          //__SILP__
            get { return _Path; }                                                     //__SILP__
        }                                                                             //__SILP__
                                                                                      //__SILP__
        protected TreeInTree(TO owner, string path, Pass pass) : base(owner, pass) {  //__SILP__
            _Path = path;                                                             //__SILP__
        }                                                                             //__SILP__
                                                                                      //__SILP__
        public string RevPath {                                                       //__SILP__
            get {                                                                     //__SILP__
                return string.Format("{0} ({1})", Path, Revision);                    //__SILP__
            }                                                                         //__SILP__
        }                                                                             //__SILP__
                                                                                      //__SILP__
        public override string LogPrefix {                                            //__SILP__
            get {                                                                     //__SILP__
                return string.Format("{0}{1} ({2}) ",                                 //__SILP__
                        base.LogPrefix, Path, Revision);                              //__SILP__
            }                                                                         //__SILP__
        }                                                                             //__SILP__
    }
}


