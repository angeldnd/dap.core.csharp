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

        public virtual void OnAdded() {
            base.OnAdded();
            if (Registry != null) {
                Registry.Channels.AddEventListener(ContextConsts.ChannelTick, this);
            }
        }

        public virtual void OnRemoved() {
            if (Registry != null) {
                Registry.Channels.RemoveEventListener(ContextConsts.ChannelTick, this);
            }
            base.OnRemoved();
        }

        public void OnEvent(string channelPath, Data evt) {
            if (channelPath == ContextConsts.ChannelTick) {
                FireEvent(ContextConsts.ChannelTick, evt);
            }
        }
    }
}
