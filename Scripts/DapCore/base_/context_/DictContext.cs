using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class DictContext<TO, T> : DictInDict<TO, T>, IContext, IDictContext
                                                        where TO : class, IDictContext
                                                        where T : class, IContext {
        public DictContext(TO owner, string key) : base(owner, key) {
        //SILP: CONTEXT_MIXIN()
            _Path = Env.GetContextPath(this);                                 //__SILP__
                                                                              //__SILP__
            _Properties = new Properties(this, ContextConsts.KeyProperties);  //__SILP__
            _Channels = new Channels(this, ContextConsts.KeyChannels);        //__SILP__
            _Handlers = new Handlers(this, ContextConsts.KeyHandlers);        //__SILP__
            _Vars = new Vars(this, ContextConsts.KeyVars);                    //__SILP__
            _Manners = new Manners(this, ContextConsts.KeyManners);           //__SILP__
            _Bus = new Bus(this, ContextConsts.KeyBus);                       //__SILP__
        }                                                                     //__SILP__
                                                                              //__SILP__
        private readonly string _Path;                                        //__SILP__
        public string Path {                                                  //__SILP__
            get { return _Path; }                                             //__SILP__
        }                                                                     //__SILP__
                                                                              //__SILP__
        private readonly Properties _Properties;                              //__SILP__
        public Properties Properties {                                        //__SILP__
            get { return _Properties; }                                       //__SILP__
        }                                                                     //__SILP__
                                                                              //__SILP__
        private readonly Channels _Channels;                                  //__SILP__
        public Channels Channels {                                            //__SILP__
            get { return _Channels; }                                         //__SILP__
        }                                                                     //__SILP__
                                                                              //__SILP__
        private readonly Handlers _Handlers;                                  //__SILP__
        public Handlers Handlers {                                            //__SILP__
            get { return _Handlers; }                                         //__SILP__
        }                                                                     //__SILP__
                                                                              //__SILP__
        private readonly Vars _Vars;                                          //__SILP__
        public Vars Vars {                                                    //__SILP__
            get { return _Vars; }                                             //__SILP__
        }                                                                     //__SILP__
                                                                              //__SILP__
        private readonly Manners _Manners;                                    //__SILP__
        public Manners Manners {                                              //__SILP__
            get { return _Manners; }                                          //__SILP__
        }                                                                     //__SILP__
                                                                              //__SILP__
        private readonly Bus _Bus;                                            //__SILP__
        public Bus Bus {                                                      //__SILP__
            get { return _Bus; }                                              //__SILP__
        }                                                                     //__SILP__
                                                                              //__SILP__
        public IContext GetContext() {                                        //__SILP__
            return this;                                                      //__SILP__
        }                                                                     //__SILP__
                                                                              //__SILP__
        public override sealed string Uri {                                   //__SILP__
            get { return _Path; }                                             //__SILP__
        }                                                                     //__SILP__
                                                                              //__SILP__
        private bool _Debugging = false;                                      //__SILP__
        public bool Debugging {                                               //__SILP__
            get { return _Debugging; }                                        //__SILP__
            set { _Debugging = value; }                                       //__SILP__
        }                                                                     //__SILP__
                                                                              //__SILP__
        public override sealed bool DebugMode {                               //__SILP__
            get { return _Debugging; }                                        //__SILP__
        }                                                                     //__SILP__
                                                                              //__SILP__
        public override void OnAdded() {                                      //__SILP__
            Env.Instance.Hooks._OnContextAdded(this);                         //__SILP__
        }                                                                     //__SILP__
                                                                              //__SILP__
        public override void OnRemoved() {                                    //__SILP__
            Env.Instance.Hooks._OnContextRemoved(this);                       //__SILP__
        }                                                                     //__SILP__
    }
}
