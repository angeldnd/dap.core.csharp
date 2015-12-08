using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class TickerConsts {
        public const string AspectTicker = "_ticker";
    }

    public class Ticker : ItemAspect, EventListener {
        public static bool AddTicker(this Item item) {
            return item.Add<Ticker>(TickerConsts.AspectTicker);
        }

        private Pass _Pass = new Pass();
        private EventListener _OnTick;

        protected override void OnAdded() {
            if (Item.AddChannel(RegistryConsts.ChannelTick, _Pass)) {
                _OnTick = new BlockEventListener(
                    (string channelPath, Data evt) => {
                        Item.FireEvent(RegistryConsts.ChannelTick, _Pass, evt);
                });
                Registry.Channels.AddEventListener(RegistryConsts.ChannelTick, _OnTick);
            }
        }

        protected override void OnRemoved() {
            if (_OnTick != null) {
                Item.Registry.Channels.RemoveEventListener(RegistryConsts.ChannelTick, _OnTick);
                _OnTick = null;
            }
        }
    }
}
