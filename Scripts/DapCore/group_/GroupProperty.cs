using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IGroupValueWatcher : IValueWatcher {
        void OnChanged(GroupProperty property);
    }

    public abstract class GroupProperty : IProperty, ISectionWatcher, IVarWatcher {
        //Not support Var's API here.
        public Object GetValue() {
            return null;
        }
        public int VarWatcherCount {
            get { return 0; }
        }
        public bool AddVarWatcher(IVarWatcher watcher) {
            return false;
        }
        public bool RemoveVarWatcher(IVarWatcher watcher) {
            return false;
        }

        //Property
        public Data Encode() {
            if (!string.IsNullOrEmpty(Type)) {
                Data data = new Data();
                if (data.SetString(ObjectConsts.KeyType, Type)) {
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

            string type = data.GetString(ObjectConsts.KeyType);
            if (type == Type) {
                return DoDecode(pass, data);
            } else {
                Error("Type Mismatched: {0}, {1}", Type, type);
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
            if (_ValueWatchers != null) {
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
            WeakListHelper.Notify(_ValueWatchers, (GroupValueWatcher watcher) => {
                watcher.OnChanged(Path);
            });
        }

        public void OnVarChanged(IVar v) {
            FireOnChanged();
        }

        private void ResetAllVarWatchers() {
            All<Property>((Property prop) => {
                if (_ValueWatchers != null) {
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
        public void AllValueCheckers<T1>(OnValueChecker<T1> callback) where T1 : IValueChecker {}

        //SILP: DECLARE_LIST(ValueWatcher, watcher, IGroupValueWatcher, _ValueWatchers)
        private WeakList<IGroupValueWatcher> _ValueWatchers = null;   //__SILP__
                                                                      //__SILP__
        public int ValueWatcherCount {                                //__SILP__
            get { return WeakListHelper.Count(_ValueWatchers); }      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool AddValueWatcher(IGroupValueWatcher watcher) {     //__SILP__
            return WeakListHelper.Add(ref _ValueWatchers, watcher);   //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool RemoveValueWatcher(IGroupValueWatcher watcher) {  //__SILP__
            return WeakListHelper.Remove(_ValueWatchers, watcher);    //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
    }
}
