using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public class Channels : EntityAspect {
        public Channel GetChannel(string channelPath) {
            return Get<Channel>(channelPath);
        }

        public Channel AddChannel(string channelPath) {
            return Add<Channel>(channelPath);
        }

        //SILP: ADD_REMOVE_HELPER(EventChecker, channelPath, channel, Channel, EventChecker, checker, EventChecker)
        public bool AddEventChecker(string channelPath, EventChecker checker) {    //__SILP__
            Channel channel = Get<Channel>(channelPath);                           //__SILP__
            if (channel != null) {                                                 //__SILP__
                return channel.AddEventChecker(checker);                           //__SILP__
            }                                                                      //__SILP__
            return false;                                                          //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__
        public bool RemoveEventChecker(string channelPath, EventChecker checker) { //__SILP__
            Channel channel = Get<Channel>(channelPath);                           //__SILP__
            if (channel != null) {                                                 //__SILP__
                return channel.RemoveEventChecker(checker);                        //__SILP__
            }                                                                      //__SILP__
            return false;                                                          //__SILP__
        }                                                                          //__SILP__
                                                                                   //__SILP__

        //SILP: ADD_REMOVE_HELPER(EventListener, channelPath, channel, Channel, EventListener, listener, EventListener)
        public bool AddEventListener(string channelPath, EventListener listener) {    //__SILP__
            Channel channel = Get<Channel>(channelPath);                              //__SILP__
            if (channel != null) {                                                    //__SILP__
                return channel.AddEventListener(listener);                            //__SILP__
            }                                                                         //__SILP__
            return false;                                                             //__SILP__
        }                                                                             //__SILP__
                                                                                      //__SILP__
        public bool RemoveEventListener(string channelPath, EventListener listener) { //__SILP__
            Channel channel = Get<Channel>(channelPath);                              //__SILP__
            if (channel != null) {                                                    //__SILP__
                return channel.RemoveEventListener(listener);                         //__SILP__
            }                                                                         //__SILP__
            return false;                                                             //__SILP__
        }                                                                             //__SILP__
                                                                                      //__SILP__

        public bool FireEvent(string channelPath, Data evt) {
            Channel channel = Get<Channel>(channelPath);
            if (channel != null) {
                return channel.FireEvent(evt);
            }
            return false;
        }
    }
}
