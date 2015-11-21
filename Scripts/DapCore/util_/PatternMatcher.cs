using System;

namespace angeldnd.dap {
    public struct PatternMatcherConsts {
        public const string WildcastSegment = "*";
        public const string WildcastSegments = "**";
    }

    public class PatternMatcher {
        public readonly char Separator;
        public readonly string Pattern;

        public string[] Segments;

        public PatternMatcher(char separator, string pattern) {
            Separator = separator;
            Pattern = pattern;
            if (Pattern == PatternMatcherConsts.WildcastSegments) {
                Segments = null;
            } else {
                Segments = Pattern.Split(Separator);
            }
        }

        public override string ToString() {
            return string.Format("[{0}: {1} {2}]", GetType().Name, Separator, Pattern);
        }

        private bool IsMatchedSegment(string patternSegment, string pathSegment) {
            if (patternSegment == PatternMatcherConsts.WildcastSegments) {
                return true;
            } else if (patternSegment == PatternMatcherConsts.WildcastSegment) {
                return true;
            } else if (patternSegment == pathSegment) {
                return true;
            }
            return false;
        }

        private bool IsMatched(int patternIndex, int pathIndex, string[] pathSegments) {
            if (patternIndex >= Segments.Length || pathIndex >= pathSegments.Length) {
                return false;
            }
            bool result = false;

            if (IsMatchedSegment(Segments[patternIndex], pathSegments[pathIndex])) {
                bool isWildcastSegments = Segments[patternIndex] == PatternMatcherConsts.WildcastSegments;

                if (pathIndex == pathSegments.Length - 1) {
                    if (patternIndex == Segments.Length - 1) {
                        result = true;
                    } else if (isWildcastSegments) {
                        result = IsMatched(patternIndex + 1, pathIndex, pathSegments);
                    }
                } else if (patternIndex == Segments.Length - 1) {
                    result = isWildcastSegments;
                } else if (isWildcastSegments) {
                    // If ** is not the last segment, then need to try matching the next one for a better match
                    if (IsMatchedSegment(Segments[patternIndex + 1], pathSegments[pathIndex])) {
                        result = IsMatched(patternIndex + 2, pathIndex + 1, pathSegments);
                    } else {
                        result = IsMatched(patternIndex, pathIndex + 1, pathSegments);
                    }
                } else {
                    result = IsMatched(patternIndex + 1, pathIndex + 1, pathSegments);
                }
            }
            return result;
        }

        public bool IsMatched(string path) {
            if (Pattern == PatternMatcherConsts.WildcastSegments) {
                return true;
            }

            string[] pathSegments = path.Split(Separator);
            bool result = IsMatched(0, 0, pathSegments);
            return result;
        }
    }
}
