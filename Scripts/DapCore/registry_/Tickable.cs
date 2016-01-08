using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class TickableConsts {
        public const string AspectTickable = "_tickable";
    }

    public class Tickable : BaseAspect<Item, Section<Item>> {
        public static bool AddToItem(Item item) {
            Tickable tickable = item.Add<Tickable>(TickableConsts.AspectTickable);
            return tickable != null && tickable.IsValid;
        }

        private EventListener _OnTick;

        public bool IsValid {
            get { return _OnTick != null; }
        }

        public Tickable(Item item, string path, Pass pass) : base(item, path, pass) {
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
