using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IEntityWatcher {
        void OnAspectAdded(IAspect aspect);
        void OnAspectRemoved(IAspect aspect);
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

    public abstract class Entity : Object, IEntity {
        public virtual void OnAspectAdded(IAspect aspect) {
            WeakListHelper.Notify(_Watchers, (IEntityWatcher watcher) => {
                watcher.OnAspectAdded(aspect);
            });
        }

        public virtual void OnAspectRemoved(IAspect aspect) {
            WeakListHelper.Notify(_Watchers, (IEntityWatcher watcher) => {
                watcher.OnAspectRemoved(aspect);
            });
        }

        //SILP: DECLARE_LIST(Watcher, watcher, IEntityWatcher, _Watchers)
        private WeakList<IEntityWatcher> _Watchers = null;            //__SILP__
                                                                      //__SILP__
        public int WatcherCount {                                     //__SILP__
            get { return WeakListHelper.Count(_Watchers); }           //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool AddWatcher(IEntityWatcher watcher) {              //__SILP__
            return WeakListHelper.Add(ref _Watchers, watcher);        //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool RemoveWatcher(IEntityWatcher watcher) {           //__SILP__
            return WeakListHelper.Remove(_Watchers, watcher);         //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
    }
}
