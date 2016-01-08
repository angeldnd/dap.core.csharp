using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IEntityWatcher {
    }

    public interface IEntityWatcher<TE> : IEntityWatcher where TE : IEntity {
        void OnAspectAdded(IAspect<TE> aspect);
        void OnAspectRemoved(IAspect<TE> aspect);
    }

    public interface IEntity : ITree {
    }

    public interface IEntity<TE> : IEntity
                                        where TE : IEntity {
    }

    public interface IEntity<TO, TE> : ITree<TO, ISection<TE>>, IEntity<TE>
                                        where TO : ITree
                                        where TE : IEntity {
        void OnAspectAdded(IAspect<TE> aspect);
        void OnAspectRemoved(IAspect<TE> aspect);
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

    public abstract class Entity<TO, TE> : Tree<TO, ISection<TE>>, IEntity<TO, TE>
                                                    where TO : ITree
                                                    where TE : IEntity {
        protected Entity(TO owner, string path, Pass pass) : base(owner, path, pass) {
        }

        protected virtual void OnAspectAdded(IAspect<TE> aspect) {
            WeakListHelper.Notify(_Watchers, (EntityWatcher<TE> watcher) => {
                watcher.OnAspectAdded(aspect);
            });
        }

        protected virtual void OnAspectRemoved(IAspect<TE> aspect) {
            WeakListHelper.Notify(_Watchers, (EntityWatcher<TE> watcher) => {
                watcher.OnAspectRemoved(aspect);
            });
        }

        //SILP: DECLARE_LIST(Watcher, watcher, EntityWatcher<TE>, _Watchers)
        private WeakList<EntityWatcher<TE>> _Watchers = null;         //__SILP__
                                                                      //__SILP__
        public int WatcherCount {                                     //__SILP__
            get { return WeakListHelper.Count(_Watchers); }           //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool AddWatcher(EntityWatcher<TE> watcher) {           //__SILP__
            return WeakListHelper.Add(ref _Watchers, watcher);        //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool RemoveWatcher(EntityWatcher<TE> watcher) {        //__SILP__
            return WeakListHelper.Remove(_Watchers, watcher);         //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
    }
}
