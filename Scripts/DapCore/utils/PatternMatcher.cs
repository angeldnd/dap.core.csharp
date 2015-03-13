using System;

namespace ADD.Dap {
    public class PatternMatcherConsts {
        public const string WildcastSegment = "*";
        public const string WildcastSegments = "**";
    }

    public class PatternMatcher {
        public readonly char Separator;
        public readonly string Pattern;

        private string[] _Segments;

        public PatternMatcher(char separator, string pattern) {
            Separator = separator;
            Pattern = pattern;
            _Segments = pattern.Split(Separator);
        }

        public bool IsMatched(string path) {
            string[] pathSegments = path.Split(Separator);
            //TODO
            return false;
        }
    }
}
