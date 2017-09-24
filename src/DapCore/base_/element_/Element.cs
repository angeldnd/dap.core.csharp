using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class Element<TO> : Object, IElement<TO>
                                            where TO : class, IOwner {
        //SILP: ELEMENT_MIXIN(Element)
        protected Element(TO owner, string key) {                                        //__SILP__
            _Owner = owner;                                                              //__SILP__
            _Key = key;                                                                  //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        protected Element(TO owner) {                                                    //__SILP__
            _Owner = owner;                                                              //__SILP__
            _Key = string.Format("{0}", Guid.NewGuid().ToString());                      //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        private readonly TO _Owner;                                                      //__SILP__
        public TO Owner {                                                                //__SILP__
            get { return _Owner; }                                                       //__SILP__
        }                                                                                //__SILP__
        public IOwner GetOwner() {                                                       //__SILP__
            return _Owner;                                                               //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        private readonly string _Key;                                                    //__SILP__
        public string Key {                                                              //__SILP__
            get { return _Key; }                                                         //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        public override string BlockName {                                               //__SILP__
            get {                                                                        //__SILP__
                return string.Format("{0}<{1}>", GetType().Name,                         //__SILP__
                            _Owner == null ? "null" : _Owner.BlockName);                 //__SILP__
            }                                                                            //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        public override bool DebugMode {                                                 //__SILP__
            get { return _Owner == null ? false : _Owner.DebugMode; }                    //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        private bool _IsOrphan = true;                                                   //__SILP__
        public bool IsOrphan {                                                           //__SILP__
            get { return _IsOrphan; }                                                    //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        public void _OnAdded(IOwner owner) {                                             //__SILP__
            if (_Owner != owner) {                                                       //__SILP__
                throw new DapException("{0}: _OnAdded: Wrong Owner: {1} -> {2}",         //__SILP__
                                        LogPrefix, _Owner, owner);                       //__SILP__
            }                                                                            //__SILP__
            if (!_IsOrphan) {                                                            //__SILP__
                throw new DapException("{0}: _OnAdded: IsOrphan == false", LogPrefix);   //__SILP__
            }                                                                            //__SILP__
            _IsOrphan = false;                                                           //__SILP__
            OnAdded();                                                                   //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        public void _OnRemoved(IOwner owner) {                                           //__SILP__
            if (_Owner != owner) {                                                       //__SILP__
                throw new DapException("{0}: _OnRemoved: Wrong Owner: {1} -> {2}",       //__SILP__
                                        LogPrefix, _Owner, owner);                       //__SILP__
            }                                                                            //__SILP__
            if (_IsOrphan) {                                                             //__SILP__
                throw new DapException("{0}: _OnRemoved: IsOrphan == true", LogPrefix);  //__SILP__
            }                                                                            //__SILP__
            _IsOrphan = true;                                                            //__SILP__
            OnRemoved();                                                                 //__SILP__
        }                                                                                //__SILP__
                                                                                         //__SILP__
        protected virtual void OnAdded() {}                                              //__SILP__
        protected virtual void OnRemoved() {}                                            //__SILP__
                                                                                         //__SILP__
        protected override void AddSummaryFields(Data summary) {                         //__SILP__
            base.AddSummaryFields(summary);                                              //__SILP__
            summary.S(ElementConsts.SummaryKey, _Key);                                   //__SILP__
        }                                                                                //__SILP__
    }
}
