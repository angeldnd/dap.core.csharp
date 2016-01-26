using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class TableInDict<TO, T> : TableElement<TO, T>, IInDictElement
                                                        where TO : class, IDict
                                                        where T : class, IInTableElement {
        //SILP: IN_DICT_MIXIN(TableInDict)
        protected TableInDict(TO owner, string key) : base(owner) {   //__SILP__
            _Key = key;                                               //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public IDict OwnerAsDict {                                    //__SILP__
            get { return Owner; }                                     //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        private readonly string _Key;                                 //__SILP__
        public override string Key {                                  //__SILP__
            get { return _Key; }                                      //__SILP__
        }                                                             //__SILP__
    }
}


