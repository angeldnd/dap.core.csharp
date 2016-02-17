using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class TickableConsts {
        public const string MannerTickable = "tickable";
    }

    public static class TickableExtension {
        public static Tickable AddTickable(this Manners manners) {
            return manners.Add<Tickable>(TickableConsts.MannerTickable);
        }

        public static Tickable GetTickable(this Manners manners) {
            return manners.Get<Tickable>(TickableConsts.MannerTickable);
        }
    }

    public class Tickable : Manner {
        public Tickable(Manners owner, string key) : base(owner, key) {
            IContext contextOwner = Context.GetOwner() as IContext;
            if (contextOwner == null) {
                Error("Invalid Context Owner: {0}", Context.GetOwner());
            }

            Channel ownerTickChannel = contextOwner.Channels.Get(EnvConsts.ChannelTick);
            if (ownerTickChannel == null) {
                Error("Context Owner Has No Tick Channel: {0}", contextOwner, EnvConsts.ChannelTick);
                return;
            }

            Channel contextTickChannel = Context.Channels.Add(EnvConsts.ChannelTick);
            if (contextTickChannel != null) {
                ownerTickChannel.AddEventWatcher(this,
                    (Channel channel, Data evt) => {
                        contextTickChannel.FireEvent(evt);
                });
            }
        }
    }
}
