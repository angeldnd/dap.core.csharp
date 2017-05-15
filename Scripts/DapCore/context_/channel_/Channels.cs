using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public sealed class Channels : DictAspect<IContext, Channel> {
        public Channels(IContext owner, string key) : base(owner, key) {
        }

        public bool WaitChannel(string channelKey, Action<Channel, bool> callback) {
            return Owner.Utils.WaitElement(this, channelKey, callback);
        }

        public bool WaitAndWatchChannel(string channelKey, IEventWatcher watcher) {
            return Owner.Utils.WaitElement(this, channelKey, (Channel channel, bool isNew) => {
                channel.AddEventWatcher(watcher);
            });
        }

        public bool WaitAndWatchChannel(string channelKey, IBlockOwner owner, Action<Channel, Data> block) {
            return WaitAndWatchChannel(channelKey, new BlockEventWatcher(owner, block));
        }

        public bool FireEvent(string channelKey, Data evt) {
            Channel channel = Get(channelKey);
            if (channel != null) {
                return channel.FireEvent(evt);
            }
            return false;
        }

        public BlockEventChecker AddEventChecker(string channelKey, IBlockOwner owner, Func<Channel, Data, bool> block) {
            Channel channel = Get(channelKey);
            if (channel != null) {
                return channel.AddEventChecker(owner, block);
            }
            return null;
        }

        public BlockEventWatcher AddEventWatcher(string channelKey, IBlockOwner owner, Action<Channel, Data> block) {
            Channel channel = Get(channelKey);
            if (channel != null) {
                return channel.AddEventWatcher(owner, block);
            }
            return null;
        }

        //SILP: ADD_REMOVE_HELPER(EventChecker, channelKey, channel, Channel, EventChecker, checker, IEventChecker)
        public bool AddEventChecker(string channelKey, IEventChecker checker) {     //__SILP__
            Channel channel = Get(channelKey);                                      //__SILP__
            if (channel != null) {                                                  //__SILP__
                return channel.AddEventChecker(checker);                            //__SILP__
            }                                                                       //__SILP__
            return false;                                                           //__SILP__
        }                                                                           //__SILP__
                                                                                    //__SILP__
        public bool RemoveEventChecker(string channelKey, IEventChecker checker) {  //__SILP__
            Channel channel = Get(channelKey);                                      //__SILP__
            if (channel != null) {                                                  //__SILP__
                return channel.RemoveEventChecker(checker);                         //__SILP__
            }                                                                       //__SILP__
            return false;                                                           //__SILP__
        }                                                                           //__SILP__

        //SILP: ADD_REMOVE_HELPER(EventWatcher, channelKey, channel, Channel, EventWatcher, listener, IEventWatcher)
        public bool AddEventWatcher(string channelKey, IEventWatcher listener) {     //__SILP__
            Channel channel = Get(channelKey);                                       //__SILP__
            if (channel != null) {                                                   //__SILP__
                return channel.AddEventWatcher(listener);                            //__SILP__
            }                                                                        //__SILP__
            return false;                                                            //__SILP__
        }                                                                            //__SILP__
                                                                                     //__SILP__
        public bool RemoveEventWatcher(string channelKey, IEventWatcher listener) {  //__SILP__
            Channel channel = Get(channelKey);                                       //__SILP__
            if (channel != null) {                                                   //__SILP__
                return channel.RemoveEventWatcher(listener);                         //__SILP__
            }                                                                        //__SILP__
            return false;                                                            //__SILP__
        }                                                                            //__SILP__
    }
}
