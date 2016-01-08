using System;

namespace angeldnd.dap {
    public interface ISectionWatcher {
    }

    public interface ISectionWatcher<TE> : ISectionWatcher where TE : IEntity {
        void OnAspectAdded(IAspect<TE> aspect);
        void OnAspectRemoved(IAspect<TE> aspect);
    }

    public interface ISection : ITree, IElement {
        IEntity GetEntity();
    }

    public interface ISection<TE> : IElement<TE>, ISection where TE : IEntity {
        TE Entity { get; }
    }

    public interface ISection<TE, TA> : ITree<TE, TA>, ISection<TE>
                                                    where TE : IEntity<TE>
                                                    where TA : IAspect<TE> {
    }

    public abstract class Section<TE, TA> : Tree<TE, TA>, ISection<TE, TA>
                                                    where TE : IEntity<TE>
                                                    where TA : IAspect<TE> {
        public Entity GetEntity() {
            return Owner;
        }

        public TE Entity {
            get { return Owner; }
        }

        protected Section(TE owner, string path, Pass pass) : base(owner, path, pass) {
        }

        protected override void OnElementAdded(TA element) {
            WeakListHelper.Notify(_Watchers, (ISectionWatcher<TE> watcher) => {
                watcher.OnAspectAdded(element);
            });
            Entity.OnAspectAdded(element);
        }

        protected override void OnElementRemoved(TA element) {
            WeakListHelper.Notify(_Watchers, (ISectionWatcher<TE> watcher) => {
                watcher.OnAspectRemoved(element);
            });
            Entity.OnAspectRemoved(element);
        }

        //SILP: DECLARE_LIST(Watcher, watcher, SectionWatcher<TE>, _Watchers)
        private WeakList<SectionWatcher<TE>> _Watchers = null;        //__SILP__
                                                                      //__SILP__
        public int WatcherCount {                                     //__SILP__
            get { return WeakListHelper.Count(_Watchers); }           //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool AddWatcher(SectionWatcher<TE> watcher) {          //__SILP__
            return WeakListHelper.Add(ref _Watchers, watcher);        //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool RemoveWatcher(SectionWatcher<TE> watcher) {       //__SILP__
            return WeakListHelper.Remove(_Watchers, watcher);         //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
    }
}
