using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IBlock {
        string TypeName { get; }
    }

    public interface IBlockOwner : IBlock {
        void AddBlock(WeakBlock block);
        void RemoveBlock(WeakBlock block);
    }

    public abstract class WeakBlock : IBlock {
        private readonly WeakReference _OwnerReference = null;

        public bool IsOwnerAlive {
            get {
                return _OwnerReference != null && _OwnerReference.IsAlive;
            }
        }

        protected WeakBlock(IBlockOwner owner) {
            if (owner != null) {
                _OwnerReference = new WeakReference(owner);
            }
        }

        public override string ToString() {
            return TypeName;
        }

        public string TypeName {
            get {
                return IsOwnerAlive ? _OwnerReference.Target.GetType().Name : ("!" + GetType().Name);
            }
        }

        public void OnAdded() {
            if (IsOwnerAlive) {
                ((IBlockOwner)_OwnerReference.Target).AddBlock(this);
            }
        }

        public void OnRemoved() {
            if (IsOwnerAlive) {
                ((IBlockOwner)_OwnerReference.Target).RemoveBlock(this);
            }
        }
    }
}
