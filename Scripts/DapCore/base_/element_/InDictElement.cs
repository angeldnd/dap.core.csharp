using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class InDictElement<TO> : Element<TO>, IInDictElement
                                            where TO : class, IDict {
        //SILP: IN_DICT_MIXIN(InDictElement)
        protected InDictElement(TO owner, string key) : base(owner) {  //__SILP__
            _Key = key;                                                //__SILP__
        }                                                              //__SILP__
                                                                       //__SILP__
        public IDict OwnerAsDict {                                     //__SILP__
            get { return Owner; }                                      //__SILP__
        }                                                              //__SILP__
                                                                       //__SILP__
        private readonly string _Key;                                  //__SILP__
        public override string Key {                                   //__SILP__
            get { return _Key; }                                       //__SILP__
        }                                                              //__SILP__
    }
}
