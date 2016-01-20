using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class InTableAspect<TO> : InTableElement<TO>, IInTableAspect<TO>
                                                where TO : ITableSection {
        //SILP:IN_TABLE_MIXIN_CONSTRUCTOR(InTableAspect)
        protected InTableAspect(TO owner, int index, Pass pass) : base(owner, index, pass) {  //__SILP__
        }                                                                                     //__SILP__

        //SILP: ASPECT_MIXIN()
        public IEntity GetEntity() {                                  //__SILP__
            return Owner.GetEntity();                                 //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public IEntity Entity {                                       //__SILP__
            get { return Owner.GetEntity(); }                         //__SILP__
        }                                                             //__SILP__
    }
}
