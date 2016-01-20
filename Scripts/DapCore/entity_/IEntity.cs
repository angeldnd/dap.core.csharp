using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IEntityAccessor {
        IEntity GetEntity();
    }

    public interface IEntityWatcher {
        void OnAspectAdded(IEntity entity, IAspect aspect);
        void OnAspectRemoved(IEntity entity, IAspect aspect);
    }

    public interface IEntity : IOwner, IEntityAccessor {
        void SetDebugMode(bool debugMode);
        void SetDebugPatterns(string[] patterns);

        int EntityWatcherCount { get; }
        bool AddEntityWatcher(IEntityWatcher watcher);
        bool RemoveEntityWatcher(IEntityWatcher watcher);

        void OnAspectAdded(IAspect aspect);
        void OnAspectRemoved(IAspect aspect);
    }

    public static class EntityConsts {
        public const string KeyAspects = "aspects";

        public const char Separator = '.';

        public const char EntitySeparator = ':';

        public static bool IsValidAspectPath(string path) {
            if (string.IsNullOrEmpty(path)) {
                return false;
            }
            if (path.IndexOf(EntitySeparator) >= 0) {
                return false;
            }
            return true;
        }
    }
}
