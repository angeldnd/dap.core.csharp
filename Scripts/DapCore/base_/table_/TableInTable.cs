using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class TableInTable<TO, T> : TableElement<TO, T>, IInTableElement
                                                        where TO : ITable
                                                        where T : class, IInTableElement {
        //SILP: IN_TABLE_MIXIN(TableInTable)
        protected TableInTable(TO owner, int index, Pass pass) : base(owner, pass) {  //__SILP__
            _Index = index;                                                           //__SILP__
        }                                                                             //__SILP__
                                                                                      //__SILP__
        public ITable OwnerAsTable {                                                  //__SILP__
            get { return Owner; }                                                     //__SILP__
        }                                                                             //__SILP__
                                                                                      //__SILP__
        private int _Index = -1;                                                      //__SILP__
        public int Index {                                                            //__SILP__
            get { return _Index; }                                                    //__SILP__
        }                                                                             //__SILP__
                                                                                      //__SILP__
        public bool SetIndex(Pass pass, int index) {                                  //__SILP__
            if (!CheckAdminPass(pass)) return false;                                  //__SILP__
                                                                                      //__SILP__
            _Index = index;                                                           //__SILP__
            return true;                                                              //__SILP__
        }                                                                             //__SILP__
                                                                                      //__SILP__
        public override string RevInfo {                                              //__SILP__
            get {                                                                     //__SILP__
                return string.Format("[{0}] ({1}) ", _Index, Revision);               //__SILP__
            }                                                                         //__SILP__
        }                                                                             //__SILP__
    }
}


