using System;

namespace angeldnd.dap {
    public abstract class TreeSection<TO, T> : TreeElement<TO, T>, ITreeSection
                                                    where TO : IOwner, IEntityAccessor
                                                    where T : class, IInTreeAspect {
        protected TreeSection(TO owner, Pass pass) : base(owner, pass) {
        }

        //SILP:SECTION_MIXIN(T)
        public IEntity GetEntity() {                                         //__SILP__
            return Owner.GetEntity();                                        //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        public IEntity Entity {                                              //__SILP__
            get { return Owner.GetEntity(); }                                //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        protected override void OnElementAdded(T element) {                  //__SILP__
            WeakListHelper.Notify(_Watchers, (ISectionWatcher watcher) => {  //__SILP__
                watcher.OnAspectAdded(this, element);                        //__SILP__
            });                                                              //__SILP__
            Entity.OnAspectAdded(element);                                   //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        protected override void OnElementRemoved(T element) {                //__SILP__
            WeakListHelper.Notify(_Watchers, (ISectionWatcher watcher) => {  //__SILP__
                watcher.OnAspectRemoved(this, element);                      //__SILP__
            });                                                              //__SILP__
            Entity.OnAspectRemoved(element);                                 //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        private WeakList<ISectionWatcher> _Watchers = null;                  //__SILP__
                                                                             //__SILP__
        public int WatcherCount {                                            //__SILP__
            get { return WeakListHelper.Count(_Watchers); }                  //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        public bool AddWatcher(ISectionWatcher watcher) {                    //__SILP__
            return WeakListHelper.Add(ref _Watchers, watcher);               //__SILP__
        }                                                                    //__SILP__
                                                                             //__SILP__
        public bool RemoveWatcher(ISectionWatcher watcher) {                 //__SILP__
            return WeakListHelper.Remove(_Watchers, watcher);                //__SILP__
        }                                                                    //__SILP__
    }
}
