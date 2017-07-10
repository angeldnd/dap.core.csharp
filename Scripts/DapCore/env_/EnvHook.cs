using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public class EnvHook : InTableAspect<EnvHooks> {
        private bool _Setup = false;

        private string _Description = null;
        public string Description {
            get { return _Description; }
        }
        private Action<IContext> _ContextAddedBlock;
        private Action<IContext> _ContextRemovedBlock;

        public EnvHook(EnvHooks owner, int index) : base(owner, index) {
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

        private List<EnvUriMatcher> _ContextPathMatchers = new List<EnvUriMatcher>();
        public int ContextMatchersCount {
            get { return _ContextPathMatchers.Count; }
        }

        public void AddContextPattern(string contextPathPattern) {
            EnvUriMatcher matcher = new EnvUriMatcher(contextPathPattern);
            if (matcher.CanMatchContext()) {
                _ContextPathMatchers.Add(matcher);
            } else {
                Error("Invalid ContextPattern: {0}", contextPathPattern);
            }
        }

        // Should only be called from IContext.OnAdded();
        public void _OnContextAdded(IContext context, string[] contextPathSegments) {
            if (_ContextAddedBlock != null) {
                for (int i = 0; i < _ContextPathMatchers.Count; i++) {
                    if (_ContextPathMatchers[i].IsMatched(contextPathSegments)) {
                        _ContextAddedBlock(context);
                        return;
                    }
                }
            }
        }

        // Should only be called from IContext.OnRemoved();
        public void _OnContextRemoved(IContext context, string[] contextPathSegments) {
            if (_ContextRemovedBlock != null) {
                for (int i = 0; i < _ContextPathMatchers.Count; i++) {
                    if (_ContextPathMatchers[i].IsMatched(contextPathSegments)) {
                        _ContextRemovedBlock(context);
                        return;
                    }
                }
            }
        }

        protected override void AddSummaryFields(Data summary) {
            base.AddSummaryFields(summary);
            summary.S(ContextConsts.SummaryDescription, _Description);
            Data patterns = DataCache.Take(this, EnvConsts.SummaryPatterns);
            for (int i = 0; i < _ContextPathMatchers.Count; i++) {
                patterns.S("context_" + i.ToString(), _ContextPathMatchers[i].ContextPathPatternMatcher.Pattern);
            }
            AddSummaryExtraPatterns(patterns);
            summary.A(EnvConsts.SummaryPatterns, patterns);
        }

        protected virtual void AddSummaryExtraPatterns(Data patterns) {}
    }
}
