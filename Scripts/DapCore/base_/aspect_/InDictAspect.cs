using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class InDictAspect<TO> : InDictElement<TO>, IInDictAspect
                                                where TO : class, IDict, IContextAccessor {
        //SILP:IN_DICT_MIXIN_CONSTRUCTOR(InDictAspect)
        protected InDictAspect(TO owner, string key) : base(owner, key) {  //__SILP__
        }                                                                  //__SILP__

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
