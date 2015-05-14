using System;

namespace angeldnd.dap {
    public struct PatternMatcherConsts {
        public const string WildcastSegment = "*";
        public const string WildcastSegments = "**";
    }

    public class PatternMatcher {
        public readonly char Separator;
        public readonly string Pattern;

        //private string[] _Segments;

        public PatternMatcher(char separator, string pattern) {
            Separator = separator;
            Pattern = pattern;
            //_Segments = pattern.Split(Separator);
        }

        public bool IsMatched(string path) {
            if (Pattern == PatternMatcherConsts.WildcastSegments) {
                return true;
            }
            //string[] pathSegments = path.Split(Separator);
            //TODO
            return false;
        }
    }
}
