using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class TickableConsts {
        public const string TypeTickable = "Tickable";

        public const string MannerTickable = "tickable";

        public const string ChannelOnTick = "on_tick";

        [DapParam(typeof(float))]
        public const string KeyTime = "time";
        [DapParam(typeof(int))]
        public const string KeyTickCount = "tick_count";
        [DapParam(typeof(float))]
        public const string KeyTickTime = "tick_time";
    }

    public static class TickableExtension {
        public static Tickable AddTickable(this Manners manners) {
            return manners.Add<Tickable>(TickableConsts.MannerTickable);
        }

        public static Tickable GetTickable(this Manners manners) {
            return manners.Get<Tickable>(TickableConsts.MannerTickable);
        }

        public static Tickable GetOrAddTickable(this Manners manners) {
            return manners.GetOrAdd<Tickable>(TickableConsts.MannerTickable);
        }
    }

    [DapType(TickableConsts.TypeTickable)]
    [DapOrder(DapOrders.Manner)]
    public class Tickable : Manner {
        private Channel _ChannelOnTick = null;
        public Channel ChannelOnTick {
            get { return _ChannelOnTick; }
        }

        public Tickable(Manners owner, string key) : base(owner, key) {
            IContext contextOwner = Context.GetOwner() as IContext;
            if (contextOwner == null) {
                Error("Invalid Context Owner: {0}", Context.GetOwner());
            }

            Channel ownerTickChannel = contextOwner.Channels.Get(TickableConsts.ChannelOnTick);
            if (ownerTickChannel == null) {
                Error("Context Owner Has No Tick Channel: {0}", contextOwner, TickableConsts.ChannelOnTick);
                return;
            }

            _ChannelOnTick = Context.Channels.Add(TickableConsts.ChannelOnTick);
            if (_ChannelOnTick != null) {
                ownerTickChannel.AddEventWatcher(this, OnTick);
            }
        }

        private void OnTick(Channel channel, Data evt) {
            if (!Context.Removed) {
                _ChannelOnTick.FireEvent(evt);
            }
        }
    }
}
