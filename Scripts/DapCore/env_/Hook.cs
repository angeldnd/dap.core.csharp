using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public sealed class Hook : InDictAspect<Hooks> {
        private bool _Setup = false;
        private Action<IContext> _ContextAddedBlock;
        private Action<IContext> _ContextRemovedBlock;
        private Action<IAspect> _AspectAddedBlock;
        private Action<IAspect> _AspectRemovedBlock;

        public Hook(Hooks owner, string key) : base(owner, key) {
        }

        public bool Setup(Action<IContext> contextAddedBlock,
                          Action<IContext> contextRemovedBlock,
                          Action<IAspect> aspectAddedBlock,
                          Action<IAspect> aspectRemovedBlock) {
            if (!_Setup) {
                _Setup = true;
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

        public bool Setup(Action<IContext> contextAddedBlock,
                          Action<IContext> contextRemovedBlock) {
            return Setup(contextAddedBlock, contextRemovedBlock, null, null);
        }

        public bool Setup(Action<IAspect> aspectAddedBlock,
                          Action<IAspect> aspectRemovedBlock) {
            return Setup(null, null, aspectAddedBlock, aspectRemovedBlock);
        }

        public bool Setup(Action<IContext> contextAddedBlock,
                          Action<IAspect> aspectAddedBlock) {
            return Setup(contextAddedBlock, null, aspectAddedBlock, null);
        }

        private List<EnvUriMatcher> _UriMatchers = new List<EnvUriMatcher>();

        public void AddUriPattern(string contextPathPattern, string aspectPathPattern) {
            _UriMatchers.Add(new EnvUriMatcher(contextPathPattern, aspectPathPattern));
        }

        public void AddUriPattern(string contextPathPattern) {
            _UriMatchers.Add(new EnvUriMatcher(contextPathPattern, null));
        }

        // Should only be called from IContext.OnAdded();
        public void _OnContextAdded(IContext context) {
            if (_ContextAddedBlock != null) {
                for (int i = 0; i < _UriMatchers.Count; i++) {
                    if (_UriMatchers[i].IsMatched(context)) {
                        _ContextAddedBlock(context);
                        return;
                    }
                }
            }
        }

        // Should only be called from IContext.OnRemoved();
        public void _OnContextRemoved(IContext context) {
            if (_ContextRemovedBlock != null) {
                for (int i = 0; i < _UriMatchers.Count; i++) {
                    if (_UriMatchers[i].IsMatched(context)) {
                        _ContextRemovedBlock(context);
                        return;
                    }
                }
            }
        }

        // Should only be called from IAspect.OnAdded();
        public void _OnAspectAdded(IAspect aspect) {
            if (_AspectAddedBlock != null) {
                for (int i = 0; i < _UriMatchers.Count; i++) {
                    if (_UriMatchers[i].IsMatched(aspect)) {
                        _AspectAddedBlock(aspect);
                        return;
                    }
                }
            }
        }

        // Should only be called from IAspect.OnRemoved();
        public void _OnAspectRemoved(IAspect aspect) {
            if (_AspectRemovedBlock != null) {
                for (int i = 0; i < _UriMatchers.Count; i++) {
                    if (_UriMatchers[i].IsMatched(aspect)) {
                        _AspectRemovedBlock(aspect);
                        return;
                    }
                }
            }
        }

        public override void OnAdded() {
            //Do Nothing.
        }

        public override void OnRemoved() {
            //Do Nothing.
        }
    }
}
