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
        private List<string> _Msgs = null;
        private WeakPubSub<string, IBusSub> _MsgSubs = null;
        private Dictionary<string, object> _MsgTokens = null;

        public Bus(IContext owner, string key) : base(owner, key) {
        }

        public void AddSub(string msg, IBusSub sub) {
            if (_Msgs == null) {
                _Msgs = new List<string>();
            }
            if (!_Msgs.Contains(msg)) {
                _Msgs.Add(msg);
            }
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

        public bool Publish(string msg, object token) {
            if (_MsgTokens == null) {
                _MsgTokens = new Dictionary<string, object>();
            }
            object oldToken;
            if (_MsgTokens.TryGetValue(msg, out oldToken)) {
                if (oldToken != token) {
                    Error("Invalid Token: {0}: {1} -> {2}", msg, oldToken, token);
                    return false;
                }
            } else {
                _MsgTokens[msg] = token;
            }

            if (_MsgSubs != null) {
                _MsgSubs.Publish(msg, (IBusSub sub) => {
                    sub.OnMsg(this, msg);
                });
            }
            return true;
        }

        protected override void AddSummaryFields(Data summary) {
            base.AddSummaryFields(summary);
            Data data = new Data();
            if (_Msgs != null) {
                for (int i = 0; i < _Msgs.Count; i++) {
                    string msg = _Msgs[i];
                    data.I(msg, _MsgSubs.GetSubCount(msg));
                }
            }
            summary.A(ContextConsts.SummaryData, data);
        }
    }
}
