using System;

namespace angeldnd.dap {
    public abstract class Decorator : Manner {
        public Decorator(Manners owner, string key) : base(owner, key) {
            if (ShouldWatchProperties()) {
                Properties.ForEach((IProperty property) => {
                    OnPropertyAdded(property, false);
                });
                Properties.AddDictWatcher(new BlockDictWatcher<IProperty>(this, (IProperty property) => {
                    OnPropertyAdded(property, true);
                }, OnPropertyRemoved));
            }
            if (ShouldWatchChannels()) {
                Channels.ForEach((Channel channel) => {
                    OnChannelAdded(channel, false);
                });
                Channels.AddDictWatcher(new BlockDictWatcher<Channel>(this, (Channel channel) => {
                    OnChannelAdded(channel, true);
                }, OnChannelRemoved));
            }
            if (ShouldWatchHandlers()) {
                Handlers.ForEach((Handler handler) => {
                    OnHandlerAdded(handler, false);
                });
                Handlers.AddDictWatcher(new BlockDictWatcher<Handler>(this, (Handler handler) => {
                    OnHandlerAdded(handler, true);
                }, OnHandlerRemoved));
            }
            if (ShouldWatchBus()) {
                Bus.AddBusWatcher(this, OnBusMsg);
            }
            if (ShouldWatchVars()) {
                Vars.ForEach((IVar v) => {
                    OnVarAdded(v, false);
                });
                Vars.AddDictWatcher(new BlockDictWatcher<IVar>(this, (IVar v) => {
                    OnVarAdded(v, true);
                }, OnVarRemoved));
            }
        }

        protected virtual bool ShouldWatchProperties() {
            return false;
        }

        protected virtual bool ShouldWatchChannels() {
            return false;
        }

        protected virtual bool ShouldWatchHandlers() {
            return false;
        }

        protected virtual bool ShouldWatchBus() {
            return false;
        }

        protected virtual bool ShouldWatchVars() {
            return false;
        }

        protected virtual void OnPropertyAdded(IProperty property, bool isNew) {}
        protected virtual void OnPropertyRemoved(IProperty property) {}

        protected virtual void OnChannelAdded(Channel channel, bool isNew) {}
        protected virtual void OnChannelRemoved(Channel channel) {}

        protected virtual void OnHandlerAdded(Handler handler, bool isNew) {}
        protected virtual void OnHandlerRemoved(Handler handler) {}

        protected virtual void OnBusMsg(Bus bus, string msg) {}

        protected virtual void OnVarAdded(IVar v, bool isNew) {}
        protected virtual void OnVarRemoved(IVar v) {}
    }
}
