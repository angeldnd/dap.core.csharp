using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class TickableConsts {
        public const string AspectTickable = "_tickable";
    }

    public class Tickable : ItemAspect<Item> {
        private Pass _Pass = new Pass();
        private EventListener _OnTick;

        public override void OnAdded() {
            if (Item.AddChannel(RegistryConsts.ChannelTick, _Pass)) {
                _OnTick = new BlockEventListener(
                    (string channelPath, Data evt) => {
                        Item.FireEvent(RegistryConsts.ChannelTick, _Pass, evt);
                });
                Registry.Channels.AddEventListener(RegistryConsts.ChannelTick, _OnTick);
            }
        }

        public override void OnRemoved() {
            if (_OnTick != null) {
                Item.Registry.Channels.RemoveEventListener(RegistryConsts.ChannelTick, _OnTick);
                _OnTick = null;
            }
        }
    }

    public static class TickableExtesnion {
        public static bool AddTickable(this Item item) {
            return item.Add<Tickable>(TickableConsts.AspectTickable);
        }
    }
}
