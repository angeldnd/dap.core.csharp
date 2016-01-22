using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class InTreeElement<TO> : Element<TO>, IInTreeElement
                                            where TO : ITree {
        //SILP: IN_TREE_MIXIN(InTreeElement)
        protected InTreeElement(TO owner, string path, Pass pass) : base(owner, pass) {  //__SILP__
            _Path = path;                                                                //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        public ITree OwnerAsTree {                                                       //__SILP__
            get { return Owner; }                                                        //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        private readonly string _Path;                                                   //__SILP__
        public string Path {                                                             //__SILP__
            get { return _Path; }                                                        //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        public override string RevInfo {                                                 //__SILP__
            get {                                                                        //__SILP__
                return string.Format("[{0}] ({1}) ", _Path, Revision);                   //__SILP__
            }                                                                            //__SILP__
        }                                                                                //__SILP__
    }
}
