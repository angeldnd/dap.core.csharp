using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class DictInDictAspect<TO, T> : DictInDict<TO, T>, IInDictAspect
                                                        where TO : class, IDict, IContextAccessor
                                                        where T : class, IInDictElement {
        //SILP:IN_DICT_MIXIN_CONSTRUCTOR(DictInDictAspect)
        protected DictInDictAspect(TO owner, string key) : base(owner, key) {  //__SILP__
        }                                                                      //__SILP__

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
