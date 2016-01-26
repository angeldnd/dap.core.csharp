using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class InBothElement<TO> : Element<TO>, IInDictElement, IInTableElement
                                            where TO : class, IOwner {

        //SILP:IN_BOTH_MIXIN(InBothElement)
        protected InBothElement(TO owner, string key) : base(owner) {  //__SILP__
            if (owner is IDict) {                                      //__SILP__
                _Key = key;                                            //__SILP__
            }                                                          //__SILP__
        }                                                              //__SILP__
                                                                       //__SILP__
        protected InBothElement(TO owner, int index) : base(owner) {   //__SILP__
            if (owner is ITable) {                                     //__SILP__
                _Index = index;                                        //__SILP__
            }                                                          //__SILP__
        }                                                              //__SILP__
                                                                       //__SILP__
        public IDict OwnerAsDict {                                     //__SILP__
            get { return Owner as IDict; }                             //__SILP__
        }                                                              //__SILP__
                                                                       //__SILP__
        public ITable OwnerAsTable {                                   //__SILP__
            get { return Owner as ITable; }                            //__SILP__
        }                                                              //__SILP__
                                                                       //__SILP__
        private int _Index = -1;                                       //__SILP__
        public int Index {                                             //__SILP__
            get { return _Index; }                                     //__SILP__
        }                                                              //__SILP__
                                                                       //__SILP__
        public bool SetIndex(IOwner owner, int index) {                //__SILP__
            if (Owner != owner) return false;                          //__SILP__
                                                                       //__SILP__
            _Index = index;                                            //__SILP__
            return true;                                               //__SILP__
        }                                                              //__SILP__
                                                                       //__SILP__
        private readonly string _Key;                                  //__SILP__
        public override string Key {                                   //__SILP__
            get {                                                      //__SILP__
                if (_Key != null) {                                    //__SILP__
                    return _Key;                                       //__SILP__
                } else {                                               //__SILP__
                    return _Index.ToString();                          //__SILP__
                }                                                      //__SILP__
            }                                                          //__SILP__
        }                                                              //__SILP__
    }
}
