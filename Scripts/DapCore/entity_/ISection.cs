using System;

namespace angeldnd.dap {
    public interface ISectionWatcher {
        void OnAspectAdded(IAspect aspect);
        void OnAspectRemoved(IAspect aspect);
    }

    public interface ISection : IOwner, IElement, IEntityAccessor {
        int WatcherCount { get; }
        bool AddWatcher(ISectionWatcher watcher);
        bool RemoveWatcher(ISectionWatcher watcher);
    }
}
