using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IObject : ILogger {
        string Type { get; }
        int Revision { get; }

        string LogPrefix { get; }

        bool DebugMode { get; }
        string[] DebugPatterns { get; }
    }

    public static class ObjectConsts {
        public const string KeyType = "type";
    }

    public abstract class Object : Logger, IObject, IBlockOwner {
        public virtual string Type {
            get { return null; }
        }

        private int _Revision = 0;
        public int Revision {
            get { return _Revision; }
        }

        protected void AdvanceRevision() {
            _Revision += 1;
        }

        public override string LogPrefix {
            get {
                return string.Format("[{0}] ({1}) ",
                            Type != null ? Type : GetType().Name, Revision);
            }
        }

        //SILP:BLOCK_OWNER()
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
