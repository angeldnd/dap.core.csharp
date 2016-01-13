using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public class Context : Entity, IContext {
        public override string Type {
            get { return ContextConsts.TypeContext; }
        }

        public Context(Pass owner) : base(owner) {
        //SILP: CONTEXT_MIXIN()
            Pass sectionPass = Pass == null ? null : Pass.Open;       //__SILP__
                                                                      //__SILP__
            _Properties = new Properties(this, sectionPass);          //__SILP__
            _Channels = new Channels(this, sectionPass);              //__SILP__
            _Handlers = new Handlers(this, sectionPass);              //__SILP__
            _Vars = new Vars(this, sectionPass);                      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        private readonly Properties _Properties;                      //__SILP__
        public Properties Properties {                                //__SILP__
            get { return _Properties; }                               //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        private readonly Channels _Channels;                          //__SILP__
        public Channels Channels {                                    //__SILP__
            get { return _Channels; }                                 //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        private readonly Handlers _Handlers;                          //__SILP__
        public Handlers Handlers {                                    //__SILP__
            get { return _Handlers; }                                 //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        private readonly Vars _Vars;                                  //__SILP__
        public Vars Vars {                                            //__SILP__
            get { return _Vars; }                                     //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        private bool _DebugMode = false;                              //__SILP__
        public override bool DebugMode {                              //__SILP__
            get { return _DebugMode; }                                //__SILP__
        }                                                             //__SILP__
        public void SetDebugMode(bool debugMode) {                    //__SILP__
            _DebugMode= debugMode;                                    //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        private string[] _DebugPatterns = {""};                       //__SILP__
        public override string[] DebugPatterns {                      //__SILP__
            get { return _DebugPatterns; }                            //__SILP__
        }                                                             //__SILP__
        public void SetDebugPatterns(string[] patterns) {             //__SILP__
            _DebugPatterns = patterns;                                //__SILP__
        }                                                             //__SILP__
    }
}
