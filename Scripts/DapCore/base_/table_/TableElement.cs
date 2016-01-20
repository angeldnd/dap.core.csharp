using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class TableElement<TO, T> : Table<T>, IElement<TO>
                                                        where TO : IOwner
                                                        where T : class, IInTableElement {
        //SILP: ELEMENT_MIXIN(TableElement)
        protected TableElement(TO owner, Pass pass) : base(pass) {    //__SILP__
            _Owner = owner;                                           //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        private readonly TO _Owner;                                   //__SILP__
        public TO Owner {                                             //__SILP__
            get { return _Owner; }                                    //__SILP__
        }                                                             //__SILP__
        public IOwner GetOwner() {                                    //__SILP__
            return _Owner;                                            //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public override string LogPrefix {                            //__SILP__
            get {                                                     //__SILP__
                return string.Format("{0}{1}",                        //__SILP__
                        Owner.LogPrefix, base.LogPrefix);             //__SILP__
            }                                                         //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public override bool DebugMode {                              //__SILP__
            get { return Owner.DebugMode; }                           //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public override string[] DebugPatterns {                      //__SILP__
            get { return Owner.DebugPatterns; }                       //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public virtual void OnAdded() {}                              //__SILP__
        public virtual void OnRemoved() {}                            //__SILP__
    }
}


