using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class Entity : Object, IEntity {
        protected Entity(Pass pass) : base(pass) {
        }

        //SILP: ENTITY_MIXIN()
        private WeakList<IEntityWatcher> _EntityWatchers = null;                  //__SILP__
                                                                                  //__SILP__
        public int EntityWatcherCount {                                           //__SILP__
            get { return WeakListHelper.Count(_EntityWatchers); }                 //__SILP__
        }                                                                         //__SILP__
                                                                                  //__SILP__
        public bool AddEntityWatcher(IEntityWatcher watcher) {                    //__SILP__
            return WeakListHelper.Add(ref _EntityWatchers, watcher);              //__SILP__
        }                                                                         //__SILP__
                                                                                  //__SILP__
        public bool RemoveEntityWatcher(IEntityWatcher watcher) {                 //__SILP__
            return WeakListHelper.Remove(_EntityWatchers, watcher);               //__SILP__
        }                                                                         //__SILP__
                                                                                  //__SILP__
        public void OnAspectAdded(IAspect aspect) {                               //__SILP__
            WeakListHelper.Notify(_EntityWatchers, (IEntityWatcher watcher) => {  //__SILP__
                watcher.OnAspectAdded(this, aspect);                              //__SILP__
            });                                                                   //__SILP__
        }                                                                         //__SILP__
                                                                                  //__SILP__
        public void OnAspectRemoved(IAspect aspect) {                             //__SILP__
            WeakListHelper.Notify(_EntityWatchers, (IEntityWatcher watcher) => {  //__SILP__
                watcher.OnAspectRemoved(this, aspect);                            //__SILP__
            });                                                                   //__SILP__
        }                                                                         //__SILP__
    }
}
