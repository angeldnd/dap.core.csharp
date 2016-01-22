using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IContextAccessor {
        IContext GetContext();
    }

    public interface IContext : IOwner, IElement, IContextAccessor {
        Properties Properties { get; }
        Channels Channels { get; }
        Handlers Handlers { get; }
        Vars Vars { get; }
        Manners Manners { get; }

        void SetDebugMode(bool debugMode);
        void SetDebugPatterns(string[] patterns);

        /*
        bool FireEvent(string channelPath, Pass pass, Data evt);
        bool FireEvent(string channelPath, Pass pass);
        bool FireEvent(string channelPath, Data evt);
        bool FireEvent(string channelPath);
        */
    }

    public interface ITreeContext : IContext, ITree {
    }

    public interface IInTreeContext : IContext, IInTreeElement {
    }

    public interface ITableContext : IContext, ITable {
    }

    public interface IInTableContext : IContext, IInTableElement {
    }

    public static class ContextConsts {
        public const char Separator = '/';

        public const string TypeContext = "Context";

        public const string VarsPropertyPasses = "_property_passes";
        public const string VarsChannelPasses = "_channel_passes";
        public const string VarsHandlerPasses = "_handler_passes";

        public const string SuffixHandlerAsync = "~";
        public const string SuffixChannelResponse = ">";

        public static string GetAsyncHandlerPath(string handlerPath) {
            return handlerPath + SuffixHandlerAsync;
        }

        public static string GetResponseChannelPath(string handlerPath) {
            return handlerPath + SuffixChannelResponse;
        }

        public static string GetVarPath(params string[] segments) {
            return string.Join(".", segments);
        }
    }
}
