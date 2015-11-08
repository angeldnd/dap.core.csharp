using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public struct TickableConsts {
        public const string TypeTickable = "Tickable";
    }

    public class Tickable : ItemType, EventListener {
        public override string Type {
            get { return TickableConsts.TypeTickable; }
        }

        public override void OnAdded() {
            base.OnAdded();
            if (Item.Registry != null) {
                Item.AddChannel(RegistryConsts.ChannelTick, Pass);
                Item.Registry.Channels.AddEventListener(RegistryConsts.ChannelTick, this);
            } else {
                Error("Invalid Tickable: Registry is null");
            }
        }

        public override void OnRemoved() {
            if (Item.Registry != null) {
                Item.Registry.Channels.RemoveEventListener(RegistryConsts.ChannelTick, this);
            }
            base.OnRemoved();
        }

        public void OnEvent(string channelPath, Data evt) {
            if (channelPath == RegistryConsts.ChannelTick) {
                Item.FireEvent(RegistryConsts.ChannelTick, Pass, evt);
            }
        }
    }
}
