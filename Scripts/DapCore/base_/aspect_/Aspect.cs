using System;

namespace angeldnd.dap {
    public abstract class Aspect<TO> : Element<TO>, IAspect
                                            where TO : IOwner, IContextAccessor {
        //SILP:ELEMENT_MIXIN_CONSTRUCTOR(Aspect)
        protected Aspect(TO owner, Pass pass) : base(owner, pass) {   //__SILP__
        }                                                             //__SILP__

        //SILP:ASPECT_MIXIN()
        public IContext GetContext() {                                //__SILP__
            return Owner.GetContext();                                //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public IContext Context {                                     //__SILP__
            get { return Owner.GetContext(); }                        //__SILP__
        }                                                             //__SILP__
    }
}