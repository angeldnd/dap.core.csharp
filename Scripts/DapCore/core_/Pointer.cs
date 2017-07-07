using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IPointer : ILogger {
        IObject GetObj();
        void ClearObj();
    }

    public interface IPointer<T> : IPointer
                                        where T : IObject {
        T Obj { get; }
        bool SetObj(T obj);
    }

    public abstract class Pointer<T> : Logger, IPointer<T>, IBlockOwner
                                            where T : class, IObject {
        private T _Obj = null;
        public T Obj {
            get { return _Obj; }
        }

        public IObject GetObj() {
            return _Obj;
        }

        public bool SetObj(T obj) {
            if (_Obj == null) {
                _Obj = obj;
                bool result = OnSetup();
                if (!result) {
                    _Obj = null;
                }
                return result;
            } else if (_Obj == obj) {
                return true;
            } else {
                Error("Already Setup: {0} -> {1}", _Obj, obj);
            }
            return false;
        }

        public void ClearObj() {
            OnClear();
            _Obj = null;
        }

        protected virtual bool OnSetup() {
            return true;
        }

        protected virtual void OnClear() {}

        public override bool DebugMode {
            get { return _Obj == null ? false : _Obj.DebugMode; }
        }

        public override string LogPrefix {
            get {
                return string.Format("[{0}] {1}", GetType().Name,
                            _Obj == null ? "null" : _Obj.LogPrefix);
            }
        }

        //SILP:BLOCK_OWNER()
        public string TypeName {                                      //__SILP__
            get {                                                     //__SILP__
                return GetType().Name;                                //__SILP__
            }                                                         //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        private List<WeakBlock> _Blocks = null;                       //__SILP__
                                                                      //__SILP__
        public void AddBlock(WeakBlock block) {                       //__SILP__
            if (_Blocks == null) {                                    //__SILP__
                _Blocks = new List<WeakBlock>();                      //__SILP__
            }                                                         //__SILP__
            if (!_Blocks.Contains(block)) {                           //__SILP__
                _Blocks.Add(block);                                   //__SILP__
            }                                                         //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public void RemoveBlock(WeakBlock block) {                    //__SILP__
            if (_Blocks == null) {                                    //__SILP__
                return;                                               //__SILP__
            }                                                         //__SILP__
            int index = _Blocks.IndexOf(block);                       //__SILP__
            if (index >= 0) {                                         //__SILP__
                _Blocks.RemoveAt(index);                              //__SILP__
            }                                                         //__SILP__
        }                                                             //__SILP__
    }
}
