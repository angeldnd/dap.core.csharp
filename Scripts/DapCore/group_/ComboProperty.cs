using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public class ComboProperty : TreeInBothSection<IProperties, IProperty>, ITreeProperties<IProperty>, IProperty {
        private bool DoEncode(Data data) {
            bool result = true;
            /*
            All((IProperty prop) => {
                Data subData = prop.Encode();
                if (subData != null) {
                    if (!data.SetData(prop.Path, subData)) {
                        result = false;
                    }
                } else {
                    result = false;
                }
            });
            */
            return result;
        }

        private bool DoDecode(Pass pass, Data data) {
            bool result = true;
            /*
            All((IProperty prop) => {
                Data subData = data.GetData(prop.Path, null);
                if (subData != null) {
                    if (!prop.Decode(pass, subData)) {
                        result = false;
                    }
                } else {
                    result = false;
                }
            });
            */
            return result;
        }

        //SILP: GROUP_PROPERTY_MIXIN(ComboProperty)
        public ComboProperty(IProperties owner, string path, Pass pass) : base(owner, path, pass) {  //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        public ComboProperty(IProperties owner, int index, Pass pass) : base(owner, index, pass) {   //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        //IProperty                                                                                  //__SILP__
        public Data Encode() {                                                                       //__SILP__
            if (!string.IsNullOrEmpty(Type)) {                                                       //__SILP__
                Data data = new Data();                                                              //__SILP__
                if (data.SetString(ObjectConsts.KeyType, Type)) {                                    //__SILP__
                    if (DoEncode(data)) {                                                            //__SILP__
                        return data;                                                                 //__SILP__
                    }                                                                                //__SILP__
                }                                                                                    //__SILP__
            }                                                                                        //__SILP__
            if (LogDebug) Debug("Not Encodable!");                                                   //__SILP__
            return null;                                                                             //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        public bool Decode(Pass pass, Data data) {                                                   //__SILP__
            if (!CheckWritePass(pass)) return false;                                                 //__SILP__
                                                                                                     //__SILP__
            string type = data.GetString(ObjectConsts.KeyType);                                      //__SILP__
            if (type == Type) {                                                                      //__SILP__
                return DoDecode(pass, data);                                                         //__SILP__
            } else {                                                                                 //__SILP__
                Error("Type Mismatched: {0}, {1}", Type, type);                                      //__SILP__
            }                                                                                        //__SILP__
            return false;                                                                            //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        public bool Decode(Data data) {                                                              //__SILP__
            return Decode(null, data);                                                               //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        private void FireOnChanged() {                                                               //__SILP__
            WeakListHelper.Notify(_VarWatchers, (IVarWatcher watcher) => {                           //__SILP__
                watcher.OnChanged(this);                                                             //__SILP__
            });                                                                                      //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        public BlockVarWatcher AddBlockVarWatcher(IBlockOwner owner,                                 //__SILP__
                                                            Action<IVar> _watcher) {                 //__SILP__
            BlockVarWatcher watcher = new BlockVarWatcher(owner, _watcher);                          //__SILP__
            if (AddVarWatcher(watcher)) {                                                            //__SILP__
                return watcher;                                                                      //__SILP__
            }                                                                                        //__SILP__
            return null;                                                                             //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        //IVar                                                                                       //__SILP__
        public object GetValue() {                                                                   //__SILP__
            return null;                                                                             //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        private WeakList<IVarWatcher> _VarWatchers = null;                                           //__SILP__
                                                                                                     //__SILP__
        public int VarWatcherCount {                                                                 //__SILP__
            get { return WeakListHelper.Count(_VarWatchers); }                                       //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        public bool AddVarWatcher(IVarWatcher watcher) {                                             //__SILP__
            if (WeakListHelper.Add(ref _VarWatchers, watcher)){                                      //__SILP__
                //_Properties.ResetAllVarWatchers(_PropertiesPass);                                  //__SILP__
                return true;                                                                         //__SILP__
            }                                                                                        //__SILP__
            return false;                                                                            //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        public bool RemoveVarWatcher(IVarWatcher watcher) {                                          //__SILP__
            if (WeakListHelper.Remove(_VarWatchers, watcher)) {                                      //__SILP__
                //_Properties.ResetAllVarWatchers(_PropertiesPass);                                  //__SILP__
                return true;                                                                         //__SILP__
            }                                                                                        //__SILP__
            return false;                                                                            //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        public int ValueCheckerCount {                                                               //__SILP__
            get { return 0; }                                                                        //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        public int ValueWatcherCount {                                                               //__SILP__
            get { return 0; }                                                                        //__SILP__
        }                                                                                            //__SILP__
    }
}
