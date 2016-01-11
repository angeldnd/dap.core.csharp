using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IContext : IEntity {
        void SetDebugMode(bool debugMode);
        void SetDebugPatterns(string[] patterns);

        Properties Properties { get; }
        Channels Channels { get; }
        Handlers Handlers { get; }
        Vars Vars { get; }
    }

    public static class ContextConsts {
        public const string TypeContext = "Context";

        public const string SectionProperties = "_properties";
        public const string SectionChannels = "_channels";
        public const string SectionHandlers = "_handlers";
        public const string SectionVars = "_vars";

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

    public class Context : Entity, IContext {
        public override string Type {
            get { return ContextConsts.TypeContext; }
        }

        private readonly Properties _Properties;
        public Properties Properties {
            get { return _Properties; }
        }

        private readonly Channels _Channels;
        public Channels Channels {
            get { return _Channels; }
        }

        private readonly Handlers _Handlers;
        public Handlers Handlers {
            get { return _Handlers; }
        }

        private readonly Vars _Vars;
        public Vars Vars {
            get { return _Vars; }
        }

        public Context() : base() {
            Pass sectionPass = new Pass().Open;

            _Properties = new Properties(this, ContextConsts.SectionProperties, sectionPass);
            _Channels = new Channels(this, ContextConsts.SectionChannels, sectionPass);
            _Handlers = new Handlers(this, ContextConsts.SectionHandlers, sectionPass);
            _Vars = new Vars(this, ContextConsts.SectionVars, sectionPass);
        }

        private bool _DebugMode = false;
        public override bool DebugMode {
            get { return _DebugMode; }
        }
        public void SetDebugMode(bool debugMode) {
            _DebugMode= debugMode;
        }

        private string[] _DebugPatterns = {""};
        public override string[] DebugPatterns {
            get { return _DebugPatterns; }
        }
        public void SetDebugPatterns(string[] patterns) {
            _DebugPatterns = patterns;
        }
    }
}
