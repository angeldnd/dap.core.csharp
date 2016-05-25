using System;

namespace angeldnd.dap {
    public class EnvUriMatcher {
        public static bool CaseSensitive = false;

        public readonly PatternMatcher ContextPathPatternMatcher = null;
        public readonly PatternMatcher AspectPathPatternMatcher = null;

        public EnvUriMatcher(string contextPathPattern, string aspectPathPattern) {
            if (contextPathPattern == null) {
                throw new NullReferenceException("Invalid contextPathPattern");
            }
            ContextPathPatternMatcher = new PatternMatcher(PathConsts.PathSeparator, contextPathPattern, CaseSensitive);
            if (!string.IsNullOrEmpty(aspectPathPattern)) {
                AspectPathPatternMatcher = new PatternMatcher(PathConsts.PathSeparator, aspectPathPattern, CaseSensitive);
            }
        }

        public override string ToString() {
            return string.Format("[{0}: {1} {2}]", GetType().Name, ContextPathPatternMatcher, AspectPathPatternMatcher);
        }

        public bool IsMatched(IContext context) {
            if (context != null && AspectPathPatternMatcher == null) {
                return ContextPathPatternMatcher.IsMatched(context.Path);
            }
            return false;
        }

        public bool IsMatched(IAspect aspect) {
            if (aspect != null && AspectPathPatternMatcher != null) {
                IContext context = aspect.Context;
                return context != null
                        && ContextPathPatternMatcher.IsMatched(context.Path)
                        && AspectPathPatternMatcher.IsMatched(aspect.Path);
            }
            return false;
        }
    }
}
