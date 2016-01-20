using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class TableInBothEntity<TO, T> : TableInBoth<TO, T>, IEntity
                                                        where TO : IOwner
                                                        where T : class, IInTableElement {
        //SILP:IN_BOTH_MIXIN_CONSTRUCTOR(TableInBothEntity)
        protected TableInBothEntity(TO owner, string path, Pass pass) : base(owner, path, pass) {  //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        protected TableInBothEntity(TO owner, int index, Pass pass) : base(owner, index, pass) {   //__SILP__
        }                                                                                          //__SILP__

        //SILP: ENTITY_MIXIN()
        public IEntity GetEntity() {                                              //__SILP__
            return this;                                                          //__SILP__
        }                                                                         //__SILP__
                                                                                  //__SILP__
        private bool _DebugMode = false;                                          //__SILP__
        public override bool DebugMode {                                          //__SILP__
            get { return _DebugMode; }                                            //__SILP__
        }                                                                         //__SILP__
        public void SetDebugMode(bool debugMode) {                                //__SILP__
            _DebugMode= debugMode;                                                //__SILP__
        }                                                                         //__SILP__
                                                                                  //__SILP__
        private string[] _DebugPatterns = null;                                   //__SILP__
        public override string[] DebugPatterns {                                  //__SILP__
            get { return _DebugPatterns; }                                        //__SILP__
        }                                                                         //__SILP__
        public void SetDebugPatterns(string[] patterns) {                         //__SILP__
            _DebugPatterns = patterns;                                            //__SILP__
        }                                                                         //__SILP__
                                                                                  //__SILP__
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
        public virtual void OnAspectAdded(IAspect aspect) {                       //__SILP__
            WeakListHelper.Notify(_EntityWatchers, (IEntityWatcher watcher) => {  //__SILP__
                watcher.OnAspectAdded(this, aspect);                              //__SILP__
            });                                                                   //__SILP__
        }                                                                         //__SILP__
                                                                                  //__SILP__
        public virtual void OnAspectRemoved(IAspect aspect) {                     //__SILP__
            WeakListHelper.Notify(_EntityWatchers, (IEntityWatcher watcher) => {  //__SILP__
                watcher.OnAspectRemoved(this, aspect);                            //__SILP__
            });                                                                   //__SILP__
        }                                                                         //__SILP__
    }
}
