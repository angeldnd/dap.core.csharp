using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract class DictContext<TO, T> : DictInDict<TO, T>, IContext, IDictContext
                                                        where TO : class, IDictContext
                                                        where T : class, IContext {
        public DictContext(TO owner, string key) : base(owner, key) {
        //SILP: CONTEXT_MIXIN()
            _Root = Root.GetRoot(this);                                                               //__SILP__
            _Path = Root.GetContextPath(this);                                                        //__SILP__
                                                                                                      //__SILP__
            _Mapping = AddTopAspect<Mapping>(ContextConsts.KeyMapping);                               //__SILP__
            _Properties = AddTopAspect<Properties>(ContextConsts.KeyProperties);                      //__SILP__
            _Channels = AddTopAspect<Channels>(ContextConsts.KeyChannels);                            //__SILP__
            _Handlers = AddTopAspect<Handlers>(ContextConsts.KeyHandlers);                            //__SILP__
            _Bus = AddTopAspect<Bus>(ContextConsts.KeyBus);                                           //__SILP__
            _Vars = AddTopAspect<Vars>(ContextConsts.KeyVars);                                        //__SILP__
            _Utils = AddTopAspect<Utils>(ContextConsts.KeyUtils);                                     //__SILP__
            _Manners = AddTopAspect<Manners>(ContextConsts.KeyManners);                               //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        private readonly Root _Root = null;                                                           //__SILP__
        public Root Root {                                                                            //__SILP__
            get { return _Root; }                                                                     //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public IDictContext OwnerAsDictContext {                                                      //__SILP__
            get { return Owner; }                                                                     //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public override string BlockName {                                                            //__SILP__
            get {                                                                                     //__SILP__
                return GetType().Name;                                                                //__SILP__
            }                                                                                         //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        private readonly string _Path;                                                                //__SILP__
        public string Path {                                                                          //__SILP__
            get { return _Path; }                                                                     //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        private readonly Mapping _Mapping;                                                            //__SILP__
        public Mapping Mapping {                                                                      //__SILP__
            get { return _Mapping; }                                                                  //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        private readonly Properties _Properties;                                                      //__SILP__
        public Properties Properties {                                                                //__SILP__
            get { return _Properties; }                                                               //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        private readonly Channels _Channels;                                                          //__SILP__
        public Channels Channels {                                                                    //__SILP__
            get { return _Channels; }                                                                 //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        private readonly Handlers _Handlers;                                                          //__SILP__
        public Handlers Handlers {                                                                    //__SILP__
            get { return _Handlers; }                                                                 //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        private readonly Bus _Bus;                                                                    //__SILP__
        public Bus Bus {                                                                              //__SILP__
            get { return _Bus; }                                                                      //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        private readonly Vars _Vars;                                                                  //__SILP__
        public Vars Vars {                                                                            //__SILP__
            get { return _Vars; }                                                                     //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        private readonly Utils _Utils;                                                                //__SILP__
        public Utils Utils {                                                                          //__SILP__
            get { return _Utils; }                                                                    //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        private readonly Manners _Manners;                                                            //__SILP__
        public Manners Manners {                                                                      //__SILP__
            get { return _Manners; }                                                                  //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public IContext GetContext() {                                                                //__SILP__
            return this;                                                                              //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public override sealed string Uri {                                                           //__SILP__
            get { return _Path; }                                                                     //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        private bool _Debugging = false;                                                              //__SILP__
        public bool Debugging {                                                                       //__SILP__
            get { return _Debugging; }                                                                //__SILP__
            set { _Debugging = value; }                                                               //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public override sealed bool DebugMode {                                                       //__SILP__
            get { return _Debugging; }                                                                //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        private bool _Removed = false;                                                                //__SILP__
        public bool Removed {                                                                         //__SILP__
            get { return _Removed; }                                                                  //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        protected override sealed void OnAdded() {                                                    //__SILP__
            Root.Hooks._OnContextAdded(this);                                                         //__SILP__
            OnContextAdded();                                                                         //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        protected override sealed void OnRemoved() {                                                  //__SILP__
            if (_Removed) {                                                                           //__SILP__
                Error("Already Removed");                                                             //__SILP__
                return;                                                                               //__SILP__
            }                                                                                         //__SILP__
            _Removed = true;                                                                          //__SILP__
            Root.Hooks._OnContextRemoved(this);                                                       //__SILP__
            OnContextRemoved();                                                                       //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        protected override void AddSummaryFields(Data summary) {                                      //__SILP__
            base.AddSummaryFields(summary);                                                           //__SILP__
            summary.S(ContextConsts.SummaryPath, _Path)                                               //__SILP__
                   .B(ContextConsts.SummaryDebugging, _Debugging);                                    //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        private Dictionary<string, IAspect> _TopAspectsDict = new Dictionary<string, IAspect>();      //__SILP__
        private List<IAspect> _TopAspectsList = new List<IAspect>();                                  //__SILP__
                                                                                                      //__SILP__
        protected TA AddTopAspect<TA>(string key) where TA : class, IAspect {                         //__SILP__
            IAspect oldAspect = null;                                                                 //__SILP__
            if (_TopAspectsDict.TryGetValue(key, out oldAspect)) {                                    //__SILP__
                Critical("Top Aspect Key Conflicted: <{0}>: {1} -> {2}",                              //__SILP__
                            typeof(TA).FullName, key, oldAspect);                                     //__SILP__
                return null;                                                                          //__SILP__
            }                                                                                         //__SILP__
            TA topAspect = Factory.Create<TA>(this, key);                                             //__SILP__
            if (topAspect != null) {                                                                  //__SILP__
                _TopAspectsDict[topAspect.Key] = topAspect;                                           //__SILP__
                _TopAspectsList.Add(topAspect);                                                       //__SILP__
            }                                                                                         //__SILP__
            return topAspect;                                                                         //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public TA GetAspect<TA>(string aspectPath, bool isDebug = false) where TA : class, IAspect {  //__SILP__
            if (aspectPath == null) return null;                                                      //__SILP__
            List<string> segments = PathConsts.Split(aspectPath);                                     //__SILP__
            if (segments.Count < 1) {                                                                 //__SILP__
                ErrorOrDebug(isDebug, "Invalid aspectPath: {0}", aspectPath);                         //__SILP__
                return null;                                                                          //__SILP__
            }                                                                                         //__SILP__
            IAspect topAspect;                                                                        //__SILP__
            if (!_TopAspectsDict.TryGetValue(segments[0], out topAspect)) {                           //__SILP__
                ErrorOrDebug(isDebug, "Not Found: {0}", aspectPath);                                  //__SILP__
                return null;                                                                          //__SILP__
            }                                                                                         //__SILP__
            if (segments.Count == 1) {                                                                //__SILP__
                return topAspect.As<TA>(isDebug);                                                     //__SILP__
            } else {                                                                                  //__SILP__
                IOwner asOwner = topAspect.As<IOwner>(isDebug);                                       //__SILP__
                if (asOwner != null) {                                                                //__SILP__
                    return TreeHelper.GetDescendant<TA>(asOwner, segments, 1, isDebug);               //__SILP__
                }                                                                                     //__SILP__
            }                                                                                         //__SILP__
            return null;                                                                              //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public IAspect GetAspect(string aspectPath, bool isDebug = false) {                           //__SILP__
            return GetAspect<IAspect>(aspectPath, isDebug);                                           //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public void ForEachTopAspects(Action<IAspect> callback) {                                     //__SILP__
            var en = _TopAspectsList.GetEnumerator();                                                 //__SILP__
            while (en.MoveNext()) {                                                                   //__SILP__
                callback(en.Current);                                                                 //__SILP__
            }                                                                                         //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public void ForEachAspects(Action<IAspect> callback) {                                        //__SILP__
            ForEachTopAspects((IAspect aspect) => {                                                   //__SILP__
                AspectExtension.ForEachAspects(aspect, callback);                                     //__SILP__
            });                                                                                       //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public virtual void AddDetailFields(Data summary) {                                           //__SILP__
            Data aspects = DataCache.Take("_summary.aspects");                                        //__SILP__
            ForEachAspects((IAspect aspect) => {                                                      //__SILP__
                aspects.A(aspect.Path, aspect.Summary);                                               //__SILP__
            });                                                                                       //__SILP__
            summary.A(ContextConsts.SummaryAspects, aspects);                                         //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        protected virtual void OnContextAdded() {}                                                    //__SILP__
        protected virtual void OnContextRemoved() {}                                                  //__SILP__
                                                                                                      //__SILP__

        public void AddDescendants(Data summary, bool treeMode) {
            Data contexts = DataCache.Take("_summary.contexts");
            if (treeMode) {
                this.ForEachContexts((IContext child) => {
                    contexts.A(child.Path, child.Summary);
                });
            } else {
                ForEach((IContext child) => {
                    contexts.A(child.Path, child.Summary);
                });
            }
            summary.A(ContextConsts.SummaryContexts, contexts);
        }
    }
}
