using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public struct TickableConsts {
        public const string TypeTickable = "Tickable";
    }

    public class Tickable : Item, EventListener {
        public override string Type {
            get { return TickableConsts.TypeTickable; }
        }

        protected override void OnItemAdded() {
            AddChannel(RegistryConsts.ChannelTick, Pass);
            Registry.Channels.AddEventListener(RegistryConsts.ChannelTick, this);
        }

        protected override void OnItemRemoved() {
            Registry.Channels.RemoveEventListener(RegistryConsts.ChannelTick, this);
        }

        public void OnEvent(string channelPath, Data evt) {
            if (channelPath == RegistryConsts.ChannelTick) {
                FireEvent(RegistryConsts.ChannelTick, Pass, evt);
            }
        }
    }
}
