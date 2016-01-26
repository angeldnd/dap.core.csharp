using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IDictWatcher {
    }

    public interface IDictWatcher<T> : IDictWatcher
                                        where T : class, IInDictElement {
        void OnElementAdded(T element);
        void OnElementRemoved(T element);
    }

    public class BlockDictWatcher<T> : WeakBlock, IDictWatcher<T>
                                                where T : class, IInDictElement {
        private readonly Action<T> _AddedBlock;
        private readonly Action<T> _RemovedBlock;

        public BlockDictWatcher(IBlockOwner owner,
                                    Action<T> addedBlock,
                                    Action<T> removedBlock) : base(owner) {
            _AddedBlock = addedBlock;
            _RemovedBlock = removedBlock;
        }

        public void OnElementAdded(T element) {
            if (_AddedBlock != null) {
                _AddedBlock(element);
            }
        }

        public void OnElementRemoved(T element) {
            if (_RemovedBlock != null) {
                _RemovedBlock(element);
            }
        }
    }

    public sealed class BlockDictElementAddedWatcher<T> : BlockDictWatcher<T>
                                                where T : class, IInDictElement {
        public BlockDictElementAddedWatcher(IBlockOwner owner, Action<T> addedBlock) :
                                    base(owner, addedBlock, null) {
        }
    }

    public sealed class BlockDictElementRemovedWatcher<T> : BlockDictWatcher<T>
                                                where T : class, IInDictElement {
        public BlockDictElementRemovedWatcher(IBlockOwner owner, Action<T> removedBlock) :
                                    base(owner, null, removedBlock) {
        }
    }
}
