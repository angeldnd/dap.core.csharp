using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class ContextConsts {
        public const char ContextSeparator = '/';
        public const char AspectSeparator = '.';

        public const string TypeContext = "Context";

        public const string SuffixHandlerAsync = "~";
        public const string SuffixChannelResponse = ">";

        public static string GetContextPath(params string[] keys) {
            return string.Join("/", keys);
        }

        public static string GetParentPath(string path) {
            return DictHelper.GetParentKey(ContextSeparator, path);
        }

        public static string GetContextKey(string path) {
            return DictHelper.GetName(ContextSeparator, path);
        }

        public static string GetAspectKey(params string[] segments) {
            return string.Join(".", segments);
        }

        public static string GetAsyncHandlerKey(string handlerKey) {
            return handlerKey + SuffixHandlerAsync;
        }

        public static string GetResponseChannelKey(string handlerKey) {
            return handlerKey + SuffixChannelResponse;
        }

        public static PatternMatcher GetContextMatcher(string pattern) {
            return new PatternMatcher(ContextSeparator, pattern);
        }

        public static PatternMatcher GetAspectMatcher(string pattern) {
            return new PatternMatcher(AspectSeparator, pattern);
        }
    }
}
