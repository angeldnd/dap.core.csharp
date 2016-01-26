using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class InTableAspect<TO> : InTableElement<TO>, IInTableAspect
                                                    where TO : class, ITable, IContextAccessor {
        //SILP:IN_TABLE_MIXIN_CONSTRUCTOR(InTableAspect)
        protected InTableAspect(TO owner, int index) : base(owner, index) {  //__SILP__
        }                                                                    //__SILP__

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
