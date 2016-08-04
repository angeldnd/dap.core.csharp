using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IBusWatcher {
        void OnBusMsg(Bus bus, string msg);
    }

    public sealed class BlockBusWatcher : WeakBlock, IBusWatcher {
        private readonly Action<Bus, string> _Block;

        public BlockBusWatcher(IBlockOwner owner, Action<Bus, string> block) : base(owner) {
            _Block = block;
        }

        public void OnBusMsg(Bus bus, string msg) {
            _Block(bus, msg);
        }
    }

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

        public BlockBusWatcher AddBusWatcher(IBlockOwner owner, Action<Bus, string> block) {
            BlockBusWatcher result = new BlockBusWatcher(owner, block);
            if (AddBusWatcher(result)) {
                return result;
            }
            return null;
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

        private bool CheckToken(string msg, object token) {
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
            }
            return true;
        }

        public bool Publish(string msg, object token) {
            TryAddMsg(msg);
            if (!CheckToken(msg, token)) {
                return false;
            }
            _MsgCounts[msg] = GetMsgCount(msg) + 1;

            if (_MsgSubs != null) {
                _MsgSubs.Publish(msg, (IBusSub sub) => {
                    sub.OnMsg(this, msg);
                });
            }

            WeakListHelper.Notify(_BusWatchers, (IBusWatcher watcher) => {
                watcher.OnBusMsg(this, msg);
            });

            if (LogDebug) {
                Debug("Publish {0}: sub_count = {1}, msg_count = {2}",
                         msg, GetSubCount(msg), GetMsgCount(msg));
            }
            return true;
        }

        public bool Clear(string msg, object token) {
            if (!CheckToken(msg, token)) {
                return false;
            }
            _MsgCounts[msg] = 0;
            return true;
        }

        public int GetSubCount(string msg) {
            if (_MsgSubs != null) {
                return _MsgSubs.GetSubCount(msg);
            }
            return 0;
        }

        public object GetMsgToken(string msg) {
            if (_MsgTokens == null) return null;

            object token;
            if (_MsgTokens.TryGetValue(msg, out token)) {
                return token;
            }
            return null;
        }

        public int GetMsgCount(string msg) {
            if (_MsgCounts == null) return 0;

            int count;
            if (_MsgCounts.TryGetValue(msg, out count)) {
                return count;
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

        //SILP: DECLARE_LIST(BusWatcher, listener, IBusWatcher, _BusWatchers)
        private WeakList<IBusWatcher> _BusWatchers = null;            //__SILP__
                                                                      //__SILP__
        public int BusWatcherCount {                                  //__SILP__
            get { return WeakListHelper.Count(_BusWatchers); }        //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool AddBusWatcher(IBusWatcher listener) {             //__SILP__
            return WeakListHelper.Add(ref _BusWatchers, listener);    //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool RemoveBusWatcher(IBusWatcher listener) {          //__SILP__
            return WeakListHelper.Remove(_BusWatchers, listener);     //__SILP__
        }                                                             //__SILP__
    }
}
