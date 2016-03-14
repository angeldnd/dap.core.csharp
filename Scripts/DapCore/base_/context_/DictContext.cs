using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class DictContext<TO, T> : DictInDict<TO, T>, IContext, IDictContext
                                                        where TO : class, IDictContext
                                                        where T : class, IContext {
        public DictContext(TO owner, string key) : base(owner, key) {
        //SILP: CONTEXT_MIXIN()
            _Path = Env.GetContextPath(this);                                                     //__SILP__
                                                                                                  //__SILP__
            _Properties = AddTopAspect<Properties>(ContextConsts.KeyProperties);                  //__SILP__
            _Channels = AddTopAspect<Channels>(ContextConsts.KeyChannels);                        //__SILP__
            _Handlers = AddTopAspect<Handlers>(ContextConsts.KeyHandlers);                        //__SILP__
            _Bus = AddTopAspect<Bus>(ContextConsts.KeyBus);                                       //__SILP__
            _Vars = AddTopAspect<Vars>(ContextConsts.KeyVars);                                    //__SILP__
            _Manners = AddTopAspect<Manners>(ContextConsts.KeyManners);                           //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        private readonly string _Path;                                                            //__SILP__
        public string Path {                                                                      //__SILP__
            get { return _Path; }                                                                 //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        private readonly Properties _Properties;                                                  //__SILP__
        public Properties Properties {                                                            //__SILP__
            get { return _Properties; }                                                           //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        private readonly Channels _Channels;                                                      //__SILP__
        public Channels Channels {                                                                //__SILP__
            get { return _Channels; }                                                             //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        private readonly Handlers _Handlers;                                                      //__SILP__
        public Handlers Handlers {                                                                //__SILP__
            get { return _Handlers; }                                                             //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        private readonly Bus _Bus;                                                                //__SILP__
        public Bus Bus {                                                                          //__SILP__
            get { return _Bus; }                                                                  //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        private readonly Vars _Vars;                                                              //__SILP__
        public Vars Vars {                                                                        //__SILP__
            get { return _Vars; }                                                                 //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        private readonly Manners _Manners;                                                        //__SILP__
        public Manners Manners {                                                                  //__SILP__
            get { return _Manners; }                                                              //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        public IContext GetContext() {                                                            //__SILP__
            return this;                                                                          //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        public override sealed string Uri {                                                       //__SILP__
            get { return _Path; }                                                                 //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        private bool _Debugging = false;                                                          //__SILP__
        public bool Debugging {                                                                   //__SILP__
            get { return _Debugging; }                                                            //__SILP__
            set { _Debugging = value; }                                                           //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        public override sealed bool DebugMode {                                                   //__SILP__
            get { return _Debugging; }                                                            //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        public override void OnAdded() {                                                          //__SILP__
            Env.Instance.Hooks._OnContextAdded(this);                                             //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        public override void OnRemoved() {                                                        //__SILP__
            Env.Instance.Hooks._OnContextRemoved(this);                                           //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        protected override void AddSummaryFields(Data data) {                                     //__SILP__
            base.AddSummaryFields(data);                                                          //__SILP__
            data.S(ContextConsts.SummaryPath, _Path)                                              //__SILP__
                .B(ContextConsts.SummaryDebugging, _Debugging);                                   //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        private Dictionary<string, IAspect> _TopAspectsDict = new Dictionary<string, IAspect>();  //__SILP__
        private List<IAspect> _TopAspectsList = new List<IAspect>();                              //__SILP__
                                                                                                  //__SILP__
        protected T AddTopAspect<T>(string key) where T : class, IAspect {                        //__SILP__
            IAspect oldAspect = null;                                                             //__SILP__
            if (_TopAspectsDict.TryGetValue(key, out oldAspect)) {                                //__SILP__
                Critical("Top Aspect Key Conflicted: <{0}>: {1} -> {2}",                          //__SILP__
                            typeof(T).FullName, key, oldAspect);                                  //__SILP__
                return null;                                                                      //__SILP__
            }                                                                                     //__SILP__
            T topAspect = Factory.Create<T>(this, key);                                           //__SILP__
            if (topAspect != null) {                                                              //__SILP__
                _TopAspectsDict[topAspect.Key] = topAspect;                                       //__SILP__
                _TopAspectsList.Add(topAspect);                                                   //__SILP__
            }                                                                                     //__SILP__
            return topAspect;                                                                     //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        public T GetAspect<T>(string aspectPath, bool logError) where T : class, IAspect {        //__SILP__
            string[] keys = aspectPath.Split(PathConsts.PathSeparator);                           //__SILP__
            if (keys.Length < 1) {                                                                //__SILP__
                if (logError) {                                                                   //__SILP__
                    Error("Invalid aspectPath: {0}", aspectPath);                                 //__SILP__
                    return null;                                                                  //__SILP__
                }                                                                                 //__SILP__
            }                                                                                     //__SILP__
            IAspect topAspect;                                                                    //__SILP__
            if (!_TopAspectsDict.TryGetValue(keys[0], out topAspect)) {                           //__SILP__
                Error("Not Found: {0}", aspectPath);                                              //__SILP__
                return null;                                                                      //__SILP__
            }                                                                                     //__SILP__
            if (keys.Length == 1) {                                                               //__SILP__
                return As<T>(topAspect, logError);                                                //__SILP__
            } else {                                                                              //__SILP__
                IOwner asOwner = As<IOwner>(topAspect, logError);                                 //__SILP__
                if (asOwner != null) {                                                            //__SILP__
                    return TreeHelper.GetDescendant<T>(asOwner, keys, 1, logError);               //__SILP__
                }                                                                                 //__SILP__
            }                                                                                     //__SILP__
            return null;                                                                          //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        public IAspect GetAspect(string aspectPath, bool logError) {                              //__SILP__
            return GetAspect<IAspect>(aspectPath, logError);                                      //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        public void ForEachTopAspects(Action<IAspect> callback) {                                 //__SILP__
            var en = _TopAspectsList.GetEnumerator();                                             //__SILP__
            while (en.MoveNext()) {                                                               //__SILP__
                callback(en.Current);                                                             //__SILP__
            }                                                                                     //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
        public void ForEachAspects(Action<IAspect> callback) {                                    //__SILP__
            ForEachTopAspects((IAspect aspect) => {                                               //__SILP__
                AspectExtension.ForEachAspects(aspect, callback);                                 //__SILP__
            });                                                                                   //__SILP__
        }                                                                                         //__SILP__
                                                                                                  //__SILP__
    }
}
