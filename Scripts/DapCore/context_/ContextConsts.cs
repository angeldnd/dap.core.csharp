using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class ContextConsts {
        public const string TypeContext = "Context";

        public const string KeyProperties = "Properties";
        public const string KeyChannels = "Channels";
        public const string KeyHandlers = "Handlers";
        public const string KeyVars = "Vars";
        public const string KeyManners = "Manners";
        public const string KeyBus = "Bus";

        public const string SuffixHandlerAsync = "~";
        public const string SuffixChannelResponse = ">";

        public static string GetAspectKey(params string[] segments) {
            return string.Join(".", segments);
        }

        public static string GetAsyncHandlerKey(string handlerKey) {
            return handlerKey + SuffixHandlerAsync;
        }

        public static string GetResponseChannelKey(string handlerKey) {
            return handlerKey + SuffixChannelResponse;
        }
    }
}
