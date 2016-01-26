using System;

namespace angeldnd.dap {
    public abstract class Manner : InDictAspect<Manners> {
        public Manner(Manners owner, string key) : base(owner, key) {
        }

        public Properties Properties {
            get { return Context.Properties; }
        }

        public Channels Channels {
            get { return Context.Channels; }
        }

        public Handlers Handlers {
            get { return Context.Handlers; }
        }

        public Vars Vars {
            get { return Context.Vars; }
        }

        public Manners Manners {
            get { return Owner; }
        }
    }
}
