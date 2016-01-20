using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class TreeInTableContext<TO, T> : TreeInTable<TO, T>, IContext
                                                        where TO : ITable
                                                        where T : class, IInTreeElement {
        public TreeInTableContext(TO owner, int index, Pass pass) : base(owner, index, pass) {
        //SILP: CONTEXT_MIXIN()
            Pass sectionPass = Pass.ToOpen(Pass);                     //__SILP__
                                                                      //__SILP__
            _Properties = new Properties(this, sectionPass);          //__SILP__
            _Channels = new Channels(this, sectionPass);              //__SILP__
            _Handlers = new Handlers(this, sectionPass);              //__SILP__
            _Vars = new Vars(this, sectionPass);                      //__SILP__
            _Others = new Others(this, sectionPass);                  //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        private readonly Properties _Properties;                      //__SILP__
        public Properties Properties {                                //__SILP__
            get { return _Properties; }                               //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        private readonly Channels _Channels;                          //__SILP__
        public Channels Channels {                                    //__SILP__
            get { return _Channels; }                                 //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        private readonly Handlers _Handlers;                          //__SILP__
        public Handlers Handlers {                                    //__SILP__
            get { return _Handlers; }                                 //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        private readonly Vars _Vars;                                  //__SILP__
        public Vars Vars {                                            //__SILP__
            get { return _Vars; }                                     //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        private readonly Others _Others;                              //__SILP__
        public Others Others {                                        //__SILP__
            get { return _Others; }                                   //__SILP__
        }                                                             //__SILP__

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
