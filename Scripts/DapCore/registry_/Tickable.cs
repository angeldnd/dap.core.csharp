using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class TickableConsts {
        public const string ChannelTick = "_tick";
    }

    public class Tickable : Item {
        private IEventListener _OnTick;

        public Tickable(Registry owner, string path, Pass pass) : base(owner, path, pass) {
        }

        public override void OnAdded() {
            Channel registryTickChannel = Owner.GetChannel(RegistryConsts.ChannelTick);

            if (registryTickChannel != null) {
                Channel contextTickChannel = this.AddChannel(TickableConsts.ChannelTick, Pass);
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
                Owner.Channels.RemoveEventListener(RegistryConsts.ChannelTick, _OnTick);
                _OnTick = null;
            }
        }
    }
}
