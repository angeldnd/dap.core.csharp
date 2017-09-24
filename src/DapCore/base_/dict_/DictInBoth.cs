using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class DictInBoth<TO, T> : DictElement<TO, T>, IInDictElement, IInTableElement
                                                        where TO : class, IOwner
                                                        where T : class, IInDictElement {
        //SILP:IN_BOTH_MIXIN(DictInBoth)
        protected DictInBoth(TO owner, string key) : base(owner, key) {     //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        protected DictInBoth(TO owner, int index) : base(owner) {           //__SILP__
            if (owner is ITable) {                                          //__SILP__
                _Index = index;                                             //__SILP__
            }                                                               //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        public IDict OwnerAsDict {                                          //__SILP__
            get { return Owner as IDict; }                                  //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        public ITable OwnerAsTable {                                        //__SILP__
            get { return Owner as ITable; }                                 //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        private int _Index = -1;                                            //__SILP__
        public int Index {                                                  //__SILP__
            get { return _Index; }                                          //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        public virtual bool _SetIndex(IOwner owner, int index) {            //__SILP__
            if (Owner != owner) return false;                               //__SILP__
            if (_Index == index) return false;                              //__SILP__
                                                                            //__SILP__
            _Index = index;                                                 //__SILP__
            return true;                                                    //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        protected override void AddSummaryFields(Data summary) {            //__SILP__
            base.AddSummaryFields(summary);                                 //__SILP__
            summary.I(ElementConsts.SummaryIndex, _Index);                  //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        public override string RevInfo {                                    //__SILP__
            get {                                                           //__SILP__
                if (_Index >= 0) {                                          //__SILP__
                    return string.Format("[{0}] ({1})", _Index, Revision);  //__SILP__
                } else {                                                    //__SILP__
                    return string.Format("({0})", Revision);                //__SILP__
                }                                                           //__SILP__
            }                                                               //__SILP__
        }                                                                   //__SILP__
    }
}


