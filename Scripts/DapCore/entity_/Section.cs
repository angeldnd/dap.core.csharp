using System;

namespace angeldnd.dap {
    public interface ISectionWatcher {
        void OnAspectAdded(IAspect aspect);
        void OnAspectRemoved(IAspect aspect);
    }

    public interface ISection : ITree {
        IEntity GetEntity();
    }

    public interface ISection<TE> : ISection
                                        where TE : IEntity {
        TE Entity { get; }
    }

    public interface ISection<TE, TA> : ITree<TE, TA>, ISection<TE>
                                                    where TE : class, IEntity
                                                    where TA : class, IAspect<TE> {
    }

    public abstract class Section<TE, TA> : Tree<TE, TA>, ISection<TE, TA>
                                                    where TE : class, IEntity
                                                    where TA : class, IAspect<TE> {
        public IEntity GetEntity() {
            return Owner;
        }

        public TE Entity {
            get { return Owner; }
        }

        protected Section(TE owner, string path, Pass pass) : base(owner, path, pass) {
        }

        protected override void OnElementAdded(TA element) {
            WeakListHelper.Notify(_Watchers, (ISectionWatcher watcher) => {
                watcher.OnAspectAdded(element);
            });
            Entity.OnAspectAdded(element);
        }

        protected override void OnElementRemoved(TA element) {
            WeakListHelper.Notify(_Watchers, (ISectionWatcher watcher) => {
                watcher.OnAspectRemoved(element);
            });
            Entity.OnAspectRemoved(element);
        }

        //SILP: DECLARE_LIST(Watcher, watcher, ISectionWatcher, _Watchers)
        private WeakList<ISectionWatcher> _Watchers = null;           //__SILP__
                                                                      //__SILP__
        public int WatcherCount {                                     //__SILP__
            get { return WeakListHelper.Count(_Watchers); }           //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool AddWatcher(ISectionWatcher watcher) {             //__SILP__
            return WeakListHelper.Add(ref _Watchers, watcher);        //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool RemoveWatcher(ISectionWatcher watcher) {          //__SILP__
            return WeakListHelper.Remove(_Watchers, watcher);         //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
    }
}
