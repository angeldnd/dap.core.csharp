using System;

namespace angeldnd.dap {
    public static class PatternMatcherConsts {
        public const char WildcastChar = '*';
        public const string WildcastSegment = "*";
        public const string WildcastSegments = "**";
    }

    public class PatternMatcher {
        public readonly char Separator;
        public readonly string Pattern;
        public readonly bool CaseSensitive;

        public string[] Segments;

        public PatternMatcher(char separator, string pattern, bool caseSensitive=false) {
            Separator = separator;
            Pattern = pattern;
            CaseSensitive = caseSensitive;

            if (Pattern == null || Pattern == PatternMatcherConsts.WildcastSegments) {
                Segments = null;
            } else {
                Segments = (CaseSensitive ? Pattern : Pattern.ToLower()).Split(Separator);
            }
        }

        public override string ToString() {
            return string.Format("[{0}: {1} {2}]", GetType().Name, Separator, Pattern);
        }

        private bool IsMatchedChar(char patternChar, char wordChar) {
            if (patternChar == PatternMatcherConsts.WildcastChar) {
                return true;
            } else if (patternChar == wordChar) {
                return true;
            }
            return false;
        }

        private bool IsMatchedWord(string pattern, string word, int patternIndex, int wordIndex) {
            if (patternIndex >= pattern.Length || wordIndex >= word.Length) {
                return false;
            }
            bool result = false;

            if (IsMatchedChar(pattern[patternIndex], word[wordIndex])) {
                bool isWildcast = pattern[patternIndex] == PatternMatcherConsts.WildcastChar;

                if (wordIndex == word.Length - 1) {
                    if (patternIndex == pattern.Length - 1) {
                        result = true;
                    } else if (isWildcast) {
                        result = IsMatchedWord(pattern, word, patternIndex + 1, wordIndex);
                    }
                } else if (patternIndex == pattern.Length - 1) {
                    result = isWildcast;
                } else if (isWildcast) {
                    // If * is not the last segment, then need to try matching the next one for a better match
                    if (IsMatchedChar(pattern[patternIndex + 1], word[wordIndex])) {
                        result = IsMatchedWord(pattern, word, patternIndex + 2, wordIndex + 1);
                    } else {
                        result = IsMatchedWord(pattern, word, patternIndex, wordIndex + 1);
                    }
                } else {
                    result = IsMatchedWord(pattern, word, patternIndex + 1, wordIndex + 1);
                }
            }
            return result;
        }

        private bool IsMatchedSegment(string patternSegment, string pathSegment) {
            if (patternSegment == PatternMatcherConsts.WildcastSegments) {
                return true;
            } else if (patternSegment == PatternMatcherConsts.WildcastSegment) {
                return true;
            } else if (patternSegment == pathSegment) {
                return true;
            } else {
                return IsMatchedWord(patternSegment, pathSegment, 0, 0);
            }
        }

        private bool IsMatched(string[] pathSegments, int patternIndex, int pathIndex) {
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
                        result = IsMatched(pathSegments, patternIndex + 1, pathIndex);
                    }
                } else if (patternIndex == Segments.Length - 1) {
                    result = isWildcastSegments;
                } else if (isWildcastSegments) {
                    // If ** is not the last segment, then need to try matching the next one for a better match
                    if (IsMatchedSegment(Segments[patternIndex + 1], pathSegments[pathIndex])) {
                        result = IsMatched(pathSegments, patternIndex + 2, pathIndex + 1);
                    } else {
                        result = IsMatched(pathSegments, patternIndex, pathIndex + 1);
                    }
                } else {
                    result = IsMatched(pathSegments, patternIndex + 1, pathIndex + 1);
                }
            }
            return result;
        }

        public bool IsMatched(string path) {
            if (Pattern == null || path == null) {
                return false;
            }
            if (Pattern == PatternMatcherConsts.WildcastSegments) {
                return true;
            }

            string[] pathSegments = (CaseSensitive ? path : path.ToLower()).Split(Separator);
            bool result = IsMatched(pathSegments, 0, 0);
            return result;
        }
    }
}
