using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class InTableElement<TO> : Element<TO>, IInTableElement<TO>
                                            where TO : ITable {
        //SILP: IN_TABLE_MIXIN(InTableElement, TO)
        private int _Index;                                                             //__SILP__
        public int Index {                                                              //__SILP__
            get { return _Index; }                                                      //__SILP__
        }                                                                               //__SILP__
                                                                                        //__SILP__
        protected InTableElement(TO owner, int index, Pass pass) : base(owner, pass) {  //__SILP__
            _Index = index;                                                             //__SILP__
        }                                                                               //__SILP__
                                                                                        //__SILP__
        public bool SetIndex(Pass pass, int index) {                                    //__SILP__
            if (!CheckAdminPass(pass)) return false;                                    //__SILP__
                                                                                        //__SILP__
            _Index = index;                                                             //__SILP__
            return true;                                                                //__SILP__
        }                                                                               //__SILP__
                                                                                        //__SILP__
        public string RevIndex {                                                        //__SILP__
            get {                                                                       //__SILP__
                return string.Format("[{0}] ({1})", _Index, Revision);                  //__SILP__
            }                                                                           //__SILP__
        }                                                                               //__SILP__
                                                                                        //__SILP__
        public override string LogPrefix {                                              //__SILP__
            get {                                                                       //__SILP__
                return string.Format("{0}[{1}] ({2}) ",                                 //__SILP__
                        base.LogPrefix, _Index, Revision);                              //__SILP__
            }                                                                           //__SILP__
        }                                                                               //__SILP__
    }
}
