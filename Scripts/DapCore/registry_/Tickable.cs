using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class TickableConsts {
        public const string AspectTickable = "_tickable";
    }

    public class Tickable : ItemAspect<Item> {
        public static bool AddToItem(Item item) {
            Tickable tickable = item.Add<Tickable>(TickableConsts.AspectTickable);
            return tickable != null && tickable.IsValid;
        }

        private Pass _Pass = new Pass();
        private EventListener _OnTick;

        public bool IsValid {
            get { return _OnTick != null; }
        }

        public override void OnAdded() {
            if (Item.AddChannel(RegistryConsts.ChannelTick, _Pass) != null) {
                _OnTick = new BlockEventListener(this,
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
}
