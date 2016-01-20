using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public class TickItem : Item {
        private IEventListener _OnTick;

        public TickItem(Registry owner, string path, Pass pass) : base(owner, path, pass) {
        }

        public override void OnAdded() {
            Channel registryTickChannel = Owner.Channels.GetChannel(EnvConsts.ChannelTick);

            if (registryTickChannel != null) {
                Channel contextTickChannel = Channels.AddChannel(EnvConsts.ChannelTick, Pass);
                if (contextTickChannel != null) {
                    _OnTick = new BlockEventListener(this,
                        (Channel channel, Data evt) => {
                            contextTickChannel.FireEvent(Pass, evt);
                    });
                    registryTickChannel.AddEventListener(_OnTick);
                }
            }
        }

        public override void OnRemoved() {
            if (_OnTick != null) {
                Owner.Channels.RemoveEventListener(EnvConsts.ChannelTick, _OnTick);
                _OnTick = null;
            }
        }
    }
}
