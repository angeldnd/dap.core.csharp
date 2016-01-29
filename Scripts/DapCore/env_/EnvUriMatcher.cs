using System;

namespace angeldnd.dap {
    public class EnvUriMatcher {
        public readonly PatternMatcher ContextPathPatternMatcher = null;
        public readonly PatternMatcher AspectPathPatternMatcher = null;

        public EnvUriMatcher(string contextPathPattern, string aspectPathPattern) {
            if (string.IsNullOrEmpty(contextPathPattern)) {
                throw new NullReferenceException("Invalid contextPathPattern");
            }
            ContextPathPatternMatcher = new PatternMatcher(PathConsts.PathSeparator, contextPathPattern);
            if (!string.IsNullOrEmpty(aspectPathPattern)) {
                AspectPathPatternMatcher = new PatternMatcher(PathConsts.PathSeparator, aspectPathPattern);
            }
        }

        public bool IsMatched(IContext context) {
            if (context != null && AspectPathPatternMatcher == null) {
                return ContextPathPatternMatcher.IsMatched(context.Path);
            }
            return false;
        }

        public bool IsMatched(IAspect aspect) {
            if (aspect != null && AspectPathPatternMatcher != null) {
                return IsMatched(aspect.Context)
                        && AspectPathPatternMatcher.IsMatched(aspect.Path);
            }
            return false;
        }
    }
}
