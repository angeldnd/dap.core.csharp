using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IEntityWatcher {
        void OnAspectAdded(IEntity entity, IAspect aspect);
        void OnAspectRemoved(IEntity entity, IAspect aspect);
    }

    public interface IEntity : IOwner {
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
