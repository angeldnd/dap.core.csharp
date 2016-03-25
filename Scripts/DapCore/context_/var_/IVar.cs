using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IVar : IAspect, IInDictElement, IInTableElement {
        Type ValueType { get; }
        object GetValue();
        bool SetValue(object newValue);

        int VarWatcherCount { get; }
        bool AddVarWatcher(IVarWatcher watcher);
        bool RemoveVarWatcher(IVarWatcher watcher);
        BlockVarWatcher AddVarWatcher(IBlockOwner owner, Action<IVar> block);

        int ValueCheckerCount { get; }
        void AllValueCheckers<T1>(Action<T1> callback) where T1 : IValueChecker;

        int ValueWatcherCount { get; }
    }

    public interface IVar<T> : IVar {
        T Value { get; }
        bool SetValue(T newValue);

        bool Setup(T defaultValue);

        bool AddValueChecker(IValueChecker<T> checker);
        bool RemoveValueChecker(IValueChecker<T> checker);
        BlockValueChecker<T> AddValueChecker(IBlockOwner owner, Func<IVar<T>, T, bool> block);

        bool AddValueWatcher(IValueWatcher<T> watcher);
        bool RemoveValueWatcher(IValueWatcher<T> watcher);
        BlockValueWatcher<T> AddValueWatcher(IBlockOwner owner, Action<IVar<T>, T> block);
    }
}
