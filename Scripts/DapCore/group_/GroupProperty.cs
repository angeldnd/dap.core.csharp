using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface GroupValueWatcher : ValueWatcher {
        void OnChanged(string path);
    }

    public abstract class GroupProperty : Properties, Property, EntityWatcher, VarWatcher {
        //Not support Var's API here.
        public Object GetValue() {
            return null;
        }
        public int VarWatcherCount {
            get { return 0; }
        }
        public bool AddVarWatcher(VarWatcher watcher) {
            return false;
        }
        public bool RemoveVarWatcher(VarWatcher watcher) {
            return false;
        }

        //Property
        public Data Encode() {
            if (!string.IsNullOrEmpty(Type)) {
                Data data = new Data();
                if (data.SetString(DapObjectConsts.KeyType, Type)) {
                    if (DoEncode(data)) {
                        return data;
                    }
                }
            }
            if (LogDebug) Debug("Not Encodable!");
            return null;
        }

        public bool Decode(Data data) {
            return Decode(null, data);
        }

        public bool Decode(Pass pass, Data data) {
            if (!CheckWritePass(pass)) return false;

            string type = data.GetString(DapObjectConsts.KeyType);
            if (type == Type) {
                return DoDecode(pass, data);
            } else {
                Error("Mismatched Type: {0}, {1}", Type, type);
            }
            return false;
        }

        protected abstract bool DoEncode(Data data);
        protected abstract bool DoDecode(Pass pass, Data data);

        public override void OnAdded() {
            AddWatcher(this);
        }

        public override void OnRemoved() {
            RemoveWatcher(this);
        }

        public virtual void OnAspectAdded(Entity entity, Aspect aspect) {
            /*
             * Do not need to add var watcher in case there is no group watcher
             */
            if (_Watchers != null) {
                if (entity == this && aspect is Property) {
                    ((Property)aspect).AddVarWatcher(this);
                }
            }
        }

        public virtual void OnAspectRemoved(Entity entity, Aspect aspect) {
            if (entity == this && aspect is Property) {
                ((Property)aspect).RemoveVarWatcher(this);
            }
        }

        protected void FireOnChanged() {
            if (_Watchers != null) {
                for (int i = 0; i < _Watchers.Count; i++) {
                    _Watchers[i].OnChanged(Path);
                }
            }
        }


        public void OnVarChanged(Var v) {
            FireOnChanged();
        }

        private void ResetAllVarWatchers() {
            All<Property>((Property prop) => {
                if (_Watchers != null) {
                    prop.AddVarWatcher(this);
                } else {
                    prop.RemoveVarWatcher(this);
                }
            });
        }

        /* GroupProperty does not support value checkers, since its values are
         * composited by multiple values, the set value was not done at once,
         * so it's not possible for a checker to check, individual checkers can
         * be added to it's sub properties though.
         */
        public int ValueCheckerCount {
            get { return 0; }
        }
        public void AllValueCheckers<T1>(OnValueChecker<T1> callback) where T1 : ValueChecker {}

        //SILP: DECLARE_LIST(ValueWatcher, watcher, GroupValueWatcher, _Watchers)
        protected List<GroupValueWatcher> _Watchers = null;                  //__SILP__
                                                                             //__SILP__
        public int ValueWatcherCount {                                       //__SILP__
            get {                                                            //__SILP__
                if (_Watchers == null) {                                     //__SILP__
                    return 0;                                                //__SILP__
                }                                                            //__SILP__
                return _Watchers.Count;                                      //__SILP__
            }                                                                //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        public virtual bool AddValueWatcher(GroupValueWatcher watcher) {     //__SILP__
            if (_Watchers == null) {                                         //__SILP__
                _Watchers = new List<GroupValueWatcher>();                   //__SILP__
                ResetAllVarWatchers();                                       //__SILP__
            }                                                                //__SILP__
            if (!_Watchers.Contains(watcher)) {                              //__SILP__
                _Watchers.Add(watcher);                                      //__SILP__
                return true;                                                 //__SILP__
            }                                                                //__SILP__
            return false;                                                    //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        public virtual bool RemoveValueWatcher(GroupValueWatcher watcher) {  //__SILP__
            if (_Watchers != null && _Watchers.Contains(watcher)) {          //__SILP__
                _Watchers.Remove(watcher);                                   //__SILP__
                if (_Watchers.Count == 0) {                                  //__SILP__
                    _Watchers = null;                                        //__SILP__
                    ResetAllVarWatchers();                                   //__SILP__
                }                                                            //__SILP__
                return true;                                                 //__SILP__
            }                                                                //__SILP__
            return false;                                                    //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
    }
}
