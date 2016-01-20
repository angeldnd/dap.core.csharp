using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IValueChecker {
    }

    public interface IValueChecker<T> : IValueChecker {
        bool IsValid(IVar<T> v, T newValue);
    }

    public sealed class BlockValueChecker<T> : WeakBlock, IValueChecker<T> {
        private readonly Func<IVar<T>, T, bool> Checker;

        public BlockValueChecker(IBlockOwner owner, Func<IVar<T>, T, bool> checker) : base(owner) {
            Checker = checker;
        }

        public bool IsValid(IVar<T> v, T newVal) {
            return Checker(v, newVal);
        }
    }
}
