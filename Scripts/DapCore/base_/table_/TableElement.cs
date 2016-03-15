using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class TableElement<TO, T> : Table<T>, IElement<TO>
                                                        where TO : class, IOwner
                                                        where T : class, IInTableElement {
        //SILP: ELEMENT_MIXIN(TableElement)
        protected TableElement(TO owner, string key) {                  //__SILP__
            _Owner = owner;                                             //__SILP__
            _Key = key;                                                 //__SILP__
        }                                                               //__SILP__
                                                                        //__SILP__
        protected TableElement(TO owner) {                              //__SILP__
            _Owner = owner;                                             //__SILP__
            _Key = string.Format("{0}", Guid.NewGuid().GetHashCode());  //__SILP__
        }                                                               //__SILP__
                                                                        //__SILP__
        private readonly TO _Owner;                                     //__SILP__
        public TO Owner {                                               //__SILP__
            get { return _Owner; }                                      //__SILP__
        }                                                               //__SILP__
        public IOwner GetOwner() {                                      //__SILP__
            return _Owner;                                              //__SILP__
        }                                                               //__SILP__
                                                                        //__SILP__
        private readonly string _Key;                                   //__SILP__
        public string Key {                                             //__SILP__
            get { return _Key; }                                        //__SILP__
        }                                                               //__SILP__
                                                                        //__SILP__
        public override bool DebugMode {                                //__SILP__
            get { return _Owner == null ? false : _Owner.DebugMode; }   //__SILP__
        }                                                               //__SILP__
                                                                        //__SILP__
        public virtual void OnAdded() {}                                //__SILP__
        public virtual void OnRemoved() {}                              //__SILP__
                                                                        //__SILP__
        protected override void AddSummaryFields(Data summary) {        //__SILP__
            base.AddSummaryFields(summary);                             //__SILP__
            summary.S(ElementConsts.SummaryKey, _Key);                  //__SILP__
        }                                                               //__SILP__
    }
}
