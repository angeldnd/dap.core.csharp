using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface BlockOwner {
        void AddBlock(WeakBlock block);
        void RemoveBlock(WeakBlock block);
    }

    public abstract class WeakBlock {
        private readonly WeakReference _OwnerReference = null;

        public bool IsOwnerAlive {
            get {
                return _OwnerReference != null && _OwnerReference.IsAlive;
            }
        }

        protected WeakBlock(BlockOwner owner) {
            if (owner != null) {
                _OwnerReference = new WeakReference(owner);
            }
        }

        public void OnAdded() {
            if (IsOwnerAlive) {
                ((BlockOwner)_OwnerReference.Target).AddBlock(this);
            }
        }

        public void OnRemoved() {
            if (IsOwnerAlive) {
                ((BlockOwner)_OwnerReference.Target).RemoveBlock(this);
            }
        }
    }
}
