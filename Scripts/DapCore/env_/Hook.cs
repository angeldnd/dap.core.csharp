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

        public Hook(Hooks owner, int index) : base(owner, index) {
        }

        public bool Setup(string description,
                          Action<IContext> contextAddedBlock,
                          Action<IContext> contextRemovedBlock) {
            if (!_Setup) {
                _Setup = true;
                _Description = description;
                _ContextAddedBlock = contextAddedBlock;
                _ContextRemovedBlock = contextRemovedBlock;
                return true;
            }
            Error("Alread Setup: {0}, {1} -> {2}, {3}",
                        _ContextAddedBlock, _ContextRemovedBlock,
                        contextAddedBlock, contextRemovedBlock);
            return false;
        }

        public bool Setup(string description,
                          Action<IContext> contextAddedBlock) {
            return Setup(description, contextAddedBlock, null);
        }

        private List<PatternMatcher> _UriMatchers = new List<PatternMatcher>();

        public void AddUriPattern(string contextPathPattern) {
            _UriMatchers.Add(new PatternMatcher(PathConsts.SegmentSeparator, contextPathPattern));
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

        protected override void AddSummaryFields(Data summary) {
            base.AddSummaryFields(summary);
            summary.S(ContextConsts.SummaryDescription, _Description);
        }
    }
}
