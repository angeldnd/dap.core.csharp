using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public sealed class Hook : InTableAspect<Hooks> {
        private bool _Setup = false;

        private string _Description = null;
        public string Description {
            get { return _Description; }
        }
        private Action<IContext> _ContextAddedBlock;
        private Action<IContext> _ContextRemovedBlock;
        private Action<IAspect> _AspectAddedBlock;
        private Action<IAspect> _AspectRemovedBlock;

        public Hook(Hooks owner, int index) : base(owner, index) {
        }

        public bool Setup(string description,
                          Action<IContext> contextAddedBlock,
                          Action<IContext> contextRemovedBlock,
                          Action<IAspect> aspectAddedBlock,
                          Action<IAspect> aspectRemovedBlock) {
            if (!_Setup) {
                _Setup = true;
                _Description = description;
                _ContextAddedBlock = contextAddedBlock;
                _ContextRemovedBlock = contextRemovedBlock;
                _AspectAddedBlock = aspectAddedBlock;
                _AspectRemovedBlock = aspectRemovedBlock;
                return true;
            }
            Error("Alread Setup: {0}, {1}; {2}, {3} -> {4}, {5}, {6}, {7}",
                        _ContextAddedBlock, _ContextRemovedBlock,
                        _AspectAddedBlock, _AspectRemovedBlock,
                        contextAddedBlock, contextRemovedBlock,
                        aspectAddedBlock, aspectRemovedBlock);
            return false;
        }

        public bool Setup(string description,
                          Action<IContext> contextAddedBlock,
                          Action<IContext> contextRemovedBlock) {
            return Setup(description, contextAddedBlock, contextRemovedBlock, null, null);
        }

        public bool Setup(string description,
                          Action<IContext> contextAddedBlock) {
            return Setup(description, contextAddedBlock, null, null, null);
        }

        public bool Setup(string description,
                          Action<IAspect> aspectAddedBlock,
                          Action<IAspect> aspectRemovedBlock) {
            return Setup(description, null, null, aspectAddedBlock, aspectRemovedBlock);
        }

        public bool Setup(string description,
                          Action<IAspect> aspectAddedBlock) {
            return Setup(description, null, null, aspectAddedBlock, null);
        }

        public bool Setup(string description,
                          Action<IContext> contextAddedBlock,
                          Action<IAspect> aspectAddedBlock) {
            return Setup(description, contextAddedBlock, null, aspectAddedBlock, null);
        }

        private List<EnvUriMatcher> _UriMatchers = new List<EnvUriMatcher>();

        public void AddUriPattern(string contextPathPattern, string aspectPathPattern) {
            _UriMatchers.Add(new EnvUriMatcher(contextPathPattern, aspectPathPattern));
        }

        public void AddUriPattern(string contextPathPattern) {
            _UriMatchers.Add(new EnvUriMatcher(contextPathPattern, null));
        }

        // Should only be called from IContext.OnAdded();
        public void _OnContextAdded(IContext context, string[] contextPathSegments) {
            if (_ContextAddedBlock != null) {
                for (int i = 0; i < _UriMatchers.Count; i++) {
                    if (_UriMatchers[i].IsMatched(contextPathSegments)) {
                        _ContextAddedBlock(context);
                        return;
                    }
                }
            }
        }

        // Should only be called from IContext.OnRemoved();
        public void _OnContextRemoved(IContext context, string[] contextPathSegments) {
            if (_ContextRemovedBlock != null) {
                for (int i = 0; i < _UriMatchers.Count; i++) {
                    if (_UriMatchers[i].IsMatched(contextPathSegments)) {
                        _ContextRemovedBlock(context);
                        return;
                    }
                }
            }
        }

        // Should only be called from IAspect.OnAdded();
        public void _OnAspectAdded(IAspect aspect, string[] contextPathSegments, string[] aspectPathSegments) {
            if (_AspectAddedBlock != null) {
                for (int i = 0; i < _UriMatchers.Count; i++) {
                    if (_UriMatchers[i].IsMatched(contextPathSegments, aspectPathSegments)) {
                        _AspectAddedBlock(aspect);
                        return;
                    }
                }
            }
        }

        // Should only be called from IAspect.OnRemoved();
        public void _OnAspectRemoved(IAspect aspect, string[] contextPathSegments, string[] aspectPathSegments) {
            if (_AspectRemovedBlock != null) {
                for (int i = 0; i < _UriMatchers.Count; i++) {
                    if (_UriMatchers[i].IsMatched(contextPathSegments, aspectPathSegments)) {
                        _AspectRemovedBlock(aspect);
                        return;
                    }
                }
            }
        }

        protected override void AddSummaryFields(Data summary) {
            base.AddSummaryFields(summary);
            summary.S(ContextConsts.SummaryDescription, _Description);
        }
    }
}
