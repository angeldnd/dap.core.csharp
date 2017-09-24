using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class InDictElement<TO> : Element<TO>, IInDictElement
                                            where TO : class, IDict {
        //SILP: IN_DICT_MIXIN(InDictElement)
        protected InDictElement(TO owner, string key) : base(owner, key) {  //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        public IDict OwnerAsDict {                                          //__SILP__
            get { return Owner; }                                           //__SILP__
        }                                                                   //__SILP__
    }
}
