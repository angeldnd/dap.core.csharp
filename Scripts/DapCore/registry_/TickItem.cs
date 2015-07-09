using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public struct TickItemConsts {
        public const string TypeTickItem = "TickItem";
    }

    public class TickItem : Item, EventListener {
        public override string Type {
            get { return TickItemConsts.TypeTickItem; }
        }

        public TickItem() {
            AddChannel(RegistryConsts.ChannelTick);
        }

        public override void OnAdded() {
            base.OnAdded();
            if (Registry != null) {
                Registry.Channels.AddEventListener(RegistryConsts.ChannelTick, this);
            }
        }

        public override void OnRemoved() {
            if (Registry != null) {
                Registry.Channels.RemoveEventListener(RegistryConsts.ChannelTick, this);
            }
            base.OnRemoved();
        }

        public void OnEvent(string channelPath, Data evt) {
            if (channelPath == RegistryConsts.ChannelTick) {
                FireEvent(RegistryConsts.ChannelTick, evt);
            }
        }
    }
}
