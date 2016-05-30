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
        private Dictionary<string, int> _MsgCounts = null;

        public Bus(IContext owner, string key) : base(owner, key) {
        }

        private void TryAddMsg(string msg) {
            if (_Msgs == null) {
                _Msgs = new List<string>();
            }
            if (!_Msgs.Contains(msg)) {
                _Msgs.Add(msg);
            }
        }

        public void AddSub(string msg, IBusSub sub) {
            TryAddMsg(msg);
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
            TryAddMsg(msg);
            if (_MsgTokens == null) {
                _MsgTokens = new Dictionary<string, object>();
                _MsgCounts = new Dictionary<string, int>();
            }
            object oldToken;
            if (_MsgTokens.TryGetValue(msg, out oldToken)) {
                if (oldToken != token) {
                    Error("Invalid Token: {0}: {1} -> {2}", msg, oldToken, token);
                    return false;
                }
            } else {
                _MsgTokens[msg] = token;
                _MsgCounts[msg] = GetMsgCount(msg) + 1;
            }

            if (_MsgSubs != null) {
                _MsgSubs.Publish(msg, (IBusSub sub) => {
                    sub.OnMsg(this, msg);
                });
            }
            if (LogDebug) {
                Debug("Publish {0}: sub_count = {1}, msg_count = {2}",
                         msg, GetSubCount(msg), GetMsgCount(msg));
            }
            return true;
        }

        public int GetSubCount(string msg) {
            if (_MsgSubs != null) {
                return _MsgSubs.GetSubCount(msg);
            }
            return 0;
        }

        public object GetMsgToken(string msg) {
            if (_MsgTokens != null) {
                object token;
                if (_MsgTokens.TryGetValue(msg, out token)) {
                    return token;
                }
            }
            return null;
        }

        public int GetMsgCount(string msg) {
            if (_MsgCounts != null) {
                int count;
                if (_MsgCounts.TryGetValue(msg, out count)) {
                    return count;
                }
            }
            return 0;
        }

        protected override void AddSummaryFields(Data summary) {
            base.AddSummaryFields(summary);
            Data data = new Data();
            if (_Msgs != null) {
                for (int i = 0; i < _Msgs.Count; i++) {
                    string msg = _Msgs[i];
                    Data msgData = new Data();
                    msgData.I(ContextConsts.SummarySubCount, GetSubCount(msg));
                    msgData.I(ContextConsts.SummaryMsgCount, GetMsgCount(msg));
                    data.A(msg, msgData);
                }
            }
            summary.A(ContextConsts.SummaryData, data);
        }
    }
}
