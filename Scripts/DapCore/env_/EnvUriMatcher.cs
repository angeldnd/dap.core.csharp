using System;

namespace angeldnd.dap {
    public class EnvUriMatcher {
        public readonly PatternMatcher ContextPathPatternMatcher = null;
        public readonly PatternMatcher AspectPathPatternMatcher = null;

        public EnvUriMatcher(string contextPathPattern, string aspectPathPattern) {
            if (contextPathPattern != null) {
                ContextPathPatternMatcher = new PatternMatcher(PathConsts.SegmentSeparator, contextPathPattern);
                if (!string.IsNullOrEmpty(aspectPathPattern)) {
                    AspectPathPatternMatcher = new PatternMatcher(PathConsts.SegmentSeparator, aspectPathPattern);
                }
            }
        }

        public EnvUriMatcher(string contextPathPattern) : this(contextPathPattern, null) {
        }

        public override string ToString() {
            return string.Format("[{0}: {1} {2}]", GetType().Name, ContextPathPatternMatcher, AspectPathPatternMatcher);
        }

        public bool CanMatchContext() {
            return ContextPathPatternMatcher != null && AspectPathPatternMatcher == null;
        }

        public bool CanMatchAspect() {
            return ContextPathPatternMatcher != null && AspectPathPatternMatcher != null;
        }

        public bool IsMatched(string[] contextPathSegments) {
            if (!CanMatchContext()) return false;

            if (contextPathSegments != null) {
                return ContextPathPatternMatcher.IsMatched(contextPathSegments);
            }
            return false;
        }

        public bool IsMatched(string[] contextPathSegments, string[] aspectPathSegments) {
            if (!CanMatchAspect()) return false;

            if (contextPathSegments != null && aspectPathSegments != null) {
                return ContextPathPatternMatcher.IsMatched(contextPathSegments)
                        && AspectPathPatternMatcher.IsMatched(aspectPathSegments);
            }
            return false;
        }
    }
}
