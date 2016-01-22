using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class TableInBoth<TO, T> : TableElement<TO, T>, IInTreeElement, IInTableElement
                                                        where TO : IOwner
                                                        where T : class, IInTableElement {
        //SILP:IN_BOTH_MIXIN(TableInBoth)
        protected TableInBoth(TO owner, string path, Pass pass) : base(owner, pass) {  //__SILP__
            if (owner is ITree) {                                                      //__SILP__
                _Path = path;                                                          //__SILP__
            }                                                                          //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        protected TableInBoth(TO owner, int index, Pass pass) : base(owner, pass) {    //__SILP__
            if (owner is ITable) {                                                     //__SILP__
                _Index = index;                                                        //__SILP__
            }                                                                          //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        public ITree OwnerAsTree {                                                     //__SILP__
            get { return Owner as ITree; }                                             //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        public ITable OwnerAsTable {                                                   //__SILP__
            get { return Owner as ITable; }                                            //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        private readonly string _Path;                                                 //__SILP__
        public string Path {                                                           //__SILP__
            get { return _Path; }                                                      //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        private int _Index = -1;                                                       //__SILP__
        public int Index {                                                             //__SILP__
            get { return _Index; }                                                     //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        public bool SetIndex(Pass pass, int index) {                                   //__SILP__
            if (!CheckAdminPass(pass)) return false;                                   //__SILP__
                                                                                       //__SILP__
            _Index = index;                                                            //__SILP__
            return true;                                                               //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        public override string RevInfo {                                               //__SILP__
            get {                                                                      //__SILP__
                if (_Path != null) {                                                   //__SILP__
                    return string.Format("[{0}] ({1}) ", _Path, Revision);             //__SILP__
                } else {                                                               //__SILP__
                    return string.Format("[{0}] ({1}) ", _Index, Revision);            //__SILP__
                }                                                                      //__SILP__
            }                                                                          //__SILP__
        }                                                                              //__SILP__
    }
}


