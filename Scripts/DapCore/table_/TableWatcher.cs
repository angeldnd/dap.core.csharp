using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface ITableWatcher {
    }

    public interface ITableWatcher<T> : ITableWatcher
                                        where T : class, IInTableElement {
        void OnElementAdded(T element);
        void OnElementRemoved(T element);
        void OnElementIndexChanged(T element);
    }

    public class BlockTableWatcher<T> : WeakBlock, ITableWatcher<T>
                                                where T : class, IInTableElement {
        private readonly Action<T> _AddedBlock;
        private readonly Action<T> _RemovedBlock;
        private readonly Action<T> _IndexChangedBlock;

        public BlockTableWatcher(IBlockOwner owner,
                                    Action<T> addedBlock,
                                    Action<T> removedBlock,
                                    Action<T> indexChangedBlock) : base(owner) {
            _AddedBlock = addedBlock;
            _RemovedBlock = removedBlock;
            _IndexChangedBlock = indexChangedBlock;
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

        public void OnElementIndexChanged(T element) {
            if (_IndexChangedBlock != null) {
                _IndexChangedBlock(element);
            }
        }
    }

    public sealed class BlockTableElementAddedWatcher<T> : BlockTableWatcher<T>
                                        where T : class, IInTableElement {
        public BlockTableElementAddedWatcher(IBlockOwner owner, Action<T> addedBlock) :
                                    base(owner, addedBlock, null, null) {
        }
    }

    public sealed class BlockTableElementRemovedWatcher<T> : BlockTableWatcher<T>
                                        where T : class, IInTableElement {
        public BlockTableElementRemovedWatcher(IBlockOwner owner, Action<T> removedBlock) :
                                    base(owner, null, removedBlock, null) {
        }
    }

    public sealed class BlockTableElementIndexChangedWatcher<T> : BlockTableWatcher<T>
                                        where T : class, IInTableElement {
        public BlockTableElementIndexChangedWatcher(IBlockOwner owner, Action<T> indexChangedBlock) :
                                    base(owner, null, null, indexChangedBlock) {
        }
    }
}
