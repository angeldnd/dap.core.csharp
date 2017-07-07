using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public class BlockOwner : IBlockOwner {
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
