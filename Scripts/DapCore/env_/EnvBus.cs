using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class EnvBusConsts {
        public const string MsgOnInit = "on_init";
        public const string MsgOnBoot = "on_boot";
        public const string MsgOnHalt = "on_halt";
    }

    public interface IEnvBusSub {
        void OnMsg(Env env, string msg);
    }

    public sealed class BlockEnvBusSub : WeakBlock, IEnvBusSub {
        private readonly Action<Env, string> _Block;

        public BlockEnvBusSub(IBlockOwner owner, Action<Env, string> block) : base(owner) {
            _Block = block;
        }

        public void OnMsg(Env env, string msg) {
            _Block(env, msg);
        }
    }

    public static class EnvBus {
        private static WeakPubSub<string, IEnvBusSub> _MsgSubs = new WeakPubSub<string, IEnvBusSub>();

        public static void AddSub(string msg, IEnvBusSub sub) {
            _MsgSubs.AddSub(msg, sub);
        }

        public static BlockEnvBusSub AddSub(string msg, IBlockOwner owner, Action<Env, string> block) {
            BlockEnvBusSub result = new BlockEnvBusSub(owner, block);
            AddSub(msg, result);
            return result;
        }

        /*
         * should only be called by Env.cs
         */
        public static void _PublishByEnv(Env env, string msg) {
            if (_MsgSubs != null) {
                _MsgSubs.Publish(msg, (IEnvBusSub sub) => {
                    sub.OnMsg(env, msg);
                });
            }
        }
    }
}
