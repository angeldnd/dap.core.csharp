using System;

namespace angeldnd.dap {
    public abstract class DictManner<T> : DictInDictAspect<Manners, T>, IManner
                                                where T : class, IInDictElement {
        public DictManner(Manners owner, string key) : base(owner, key) {
        //SILP: MANNER_MIXIN()
        }                                                             //__SILP__
                                                                      //__SILP__
        public Properties Properties {                                //__SILP__
            get { return Context.Properties; }                        //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public Channels Channels {                                    //__SILP__
            get { return Context.Channels; }                          //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public Handlers Handlers {                                    //__SILP__
            get { return Context.Handlers; }                          //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public Bus Bus {                                              //__SILP__
            get { return Context.Bus; }                               //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public Vars Vars {                                            //__SILP__
            get { return Context.Vars; }                              //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public Manners Manners {                                      //__SILP__
            get { return Owner; }                                     //__SILP__
        }                                                             //__SILP__
    }
}
