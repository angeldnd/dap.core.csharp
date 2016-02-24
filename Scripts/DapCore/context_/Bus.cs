using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IBusSub {
        void OnMsg(Bus bus, string msg);
    }

    public sealed class BlockBusSub : WeakBlock, IBusSub {
        private readonly Action<Bus, string> _Block;

        public BlockBusSub(IBlockOwner owner, Action<Bus, string> block) : base(owner) {
            _Block = block;
        }

        public void OnMsg(Bus bus, string msg) {
            _Block(bus, msg);
        }
    }

    public sealed class Bus : Aspect<IContext> {
        private Dictionary<string, object> _MsgTokens = null;
        private WeakPubSub<string, IBusSub> _MsgSubs = null;

        public Bus(IContext owner, string key) : base(owner, key) {
        }

        public void AddSub(string msg, IBusSub sub) {
            if (_MsgSubs == null) {
                _MsgSubs = new WeakPubSub<string, IBusSub>();
            }
            _MsgSubs.AddSub(msg, sub);
        }

        public BlockBusSub AddSub(string msg, IBlockOwner owner, Action<Bus, string> block) {
            BlockBusSub result = new BlockBusSub(owner, block);
            AddSub(msg, result);
            return result;
        }

        public void Publish(string msg, object token) {
            if (_MsgTokens == null) {
                _MsgTokens = new Dictionary<string, object>();
            }
            object oldToken;
            if (_MsgTokens.TryGetValue(msg, out oldToken)) {
                if (oldToken != token) {
                    Error("Invalid Token: {0}: {1} -> {2}", msg, oldToken, token);
                    return;
                }
            } else {
                _MsgTokens[msg] = token;
            }

            if (_MsgSubs != null) {
                _MsgSubs.Publish(msg, (IBusSub sub) => {
                    sub.OnMsg(this, msg);
                });
            }
        }
    }
}
