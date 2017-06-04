using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public sealed class DebugHook : Hook {
        private bool _Setup = false;

        private string _Description = null;
        public string Description {
            get { return _Description; }
        }

        public DebugHook(Hooks owner, int index) : base(owner, index) {
            Setup(
                "DebugHook",
                (IContext context) => {
                    context.Debugging = true;
            });
        }

        private List<EnvUriMatcher> _AspectPathMatchers = new List<EnvUriMatcher>();
        public int AspectMatchersCount {
            get { return _AspectPathMatchers.Count; }
        }

        public void AddAspectPattern(string contextPathPattern, string aspectPathPattern) {
            EnvUriMatcher matcher = new EnvUriMatcher(contextPathPattern, aspectPathPattern);
            if (matcher.CanMatchAspect()) {
                _AspectPathMatchers.Add(matcher);
            } else {
                Error("Invalid AspectPattern: {0}, {1}", contextPathPattern, aspectPathPattern);
            }
        }

        // Should only be called from IAspect.OnAdded();
        public void _OnAspectAdded(IAspect aspect, string[] contextPathSegments, string[] aspectPathSegments) {
            for (int i = 0; i < _AspectPathMatchers.Count; i++) {
                if (_AspectPathMatchers[i].IsMatched(contextPathSegments, aspectPathSegments)) {
                    aspect.Debugging = true;
                    return;
                }
            }
        }

        protected override void AddSummaryExtraPatterns(Data patterns) {
            for (int i = 0; i < _AspectPathMatchers.Count; i++) {
                patterns.S("aspect_" + i.ToString(),
                        string.Format("{0}:{1}",
                            _AspectPathMatchers[i].ContextPathPatternMatcher.Pattern,
                            _AspectPathMatchers[i].AspectPathPatternMatcher.Pattern));
            }
        }
    }
}
