using System;

namespace angeldnd.dap {
    public abstract class Aspect<TO> : Element<TO>, IAspect<TO>
                                                where TO : ISection {
        //SILP:ELEMENT_MIXIN_CONSTRUCTOR(Aspect)
        protected Aspect(TO owner, Pass pass) : base(owner, pass) {   //__SILP__
        }                                                             //__SILP__

        //SILP:ASPECT_MIXIN()
        public IEntity GetEntity() {                                  //__SILP__
            return Owner.GetEntity();                                 //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public IEntity Entity {                                       //__SILP__
            get { return Owner.GetEntity(); }                         //__SILP__
        }                                                             //__SILP__
    }
}
