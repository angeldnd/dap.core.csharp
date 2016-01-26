using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class Context<TO> : InDictElement<TO>, IContext
                                            where TO : class, IDictContext {
        public Context(TO owner, string key) : base(owner, key) {
        //SILP: CONTEXT_MIXIN()
            _Path = ContextConsts.GetContextPath(ContextExtension.GetKeys(this));  //__SILP__
                                                                                   //__SILP__
            _Properties = new Properties(this);                                    //__SILP__
            _Channels = new Channels(this);                                        //__SILP__
            _Handlers = new Handlers(this);                                        //__SILP__
            _Vars = new Vars(this);                                                //__SILP__
            _Manners = new Manners(this);                                          //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        private string _Path;                                                      //__SILP__
        public string Path {                                                       //__SILP__
            get { return _Path; }                                                  //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        private readonly Properties _Properties;                                   //__SILP__
        public Properties Properties {                                             //__SILP__
            get { return _Properties; }                                            //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        private readonly Channels _Channels;                                       //__SILP__
        public Channels Channels {                                                 //__SILP__
            get { return _Channels; }                                              //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        private readonly Handlers _Handlers;                                       //__SILP__
        public Handlers Handlers {                                                 //__SILP__
            get { return _Handlers; }                                              //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        private readonly Vars _Vars;                                               //__SILP__
        public Vars Vars {                                                         //__SILP__
            get { return _Vars; }                                                  //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        private readonly Manners _Manners;                                         //__SILP__
        public Manners Manners {                                                   //__SILP__
            get { return _Manners; }                                               //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public IContext GetContext() {                                             //__SILP__
            return this;                                                           //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        private bool _DebugMode = false;                                           //__SILP__
        public override bool DebugMode {                                           //__SILP__
            get { return _DebugMode; }                                             //__SILP__
        }                                                                          //__SILP__
        public void SetDebugMode(bool debugMode) {                                 //__SILP__
            _DebugMode= debugMode;                                                 //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        private string[] _DebugPatterns = null;                                    //__SILP__
        public override string[] DebugPatterns {                                   //__SILP__
            get { return _DebugPatterns; }                                         //__SILP__
        }                                                                          //__SILP__
        public void SetDebugPatterns(string[] patterns) {                          //__SILP__
            _DebugPatterns = patterns;                                             //__SILP__
        }                                                                          //__SILP__
    }
}
