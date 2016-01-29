using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class TableInDict<TO, T> : TableElement<TO, T>, IInDictElement
                                                        where TO : class, IDict
                                                        where T : class, IInTableElement {
        //SILP: IN_DICT_MIXIN(TableInDict)
        protected TableInDict(TO owner, string key) : base(owner, key) {  //__SILP__
        }                                                                 //__SILP__
                                                                          //__SILP__
        public IDict OwnerAsDict {                                        //__SILP__
            get { return Owner; }                                         //__SILP__
        }                                                                 //__SILP__
    }
}


