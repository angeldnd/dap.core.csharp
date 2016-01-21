using System;

namespace angeldnd.dap {
    public interface ISectionWatcher {
        void OnAspectAdded(ISection section, IAspect aspect);
        void OnAspectRemoved(ISection section, IAspect aspect);
    }

    public interface ISection : IOwner, IElement, IEntityAccessor {
        int WatcherCount { get; }
        bool AddWatcher(ISectionWatcher watcher);
        bool RemoveWatcher(ISectionWatcher watcher);
    }

    public interface ITreeSection : ITree, ISection {
    }

    public interface ITableSection : ITable, ISection {
    }
}
