using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public class Channels : SecurableEntityAspect {
        public Channel GetChannel(string channelPath) {
            return Get<Channel>(channelPath);
        }

        public Channel AddChannel(string channelPath, Pass pass) {
            Channel channel = Add<Channel>(channelPath);
            if (channel != null) {
                channel.SetPass(pass);
            }
            return channel;
        }

        public Channel AddChannel(string channelPath) {
            return AddChannel(channelPath, null);
        }

        public bool FireEvent(string channelPath, Pass pass, Data evt) {
            Channel channel = Get<Channel>(channelPath);
            if (channel != null) {
                return channel.FireEvent(pass, evt);
            }
            return false;
        }

        public bool FireEvent(string channelPath, Data evt) {
            return FireEvent(channelPath, null, evt);
        }

        //SILP: ADD_REMOVE_HELPER(EventChecker, channelPath, channel, Channel, EventChecker, checker, EventChecker)
        public bool AddEventChecker(string channelPath, EventChecker checker) {     //__SILP__
            Channel channel = Get<Channel>(channelPath);                            //__SILP__
            if (channel != null) {                                                  //__SILP__
                return channel.AddEventChecker(checker);                            //__SILP__
            } else {                                                                //__SILP__
                Error("Channel Not Found: {0}", channelPath);                       //__SILP__
            }                                                                       //__SILP__
            return false;                                                           //__SILP__
        }                                                                           //__SILP__
                                                                                    //__SILP__
        public bool RemoveEventChecker(string channelPath, EventChecker checker) {  //__SILP__
            Channel channel = Get<Channel>(channelPath);                            //__SILP__
            if (channel != null) {                                                  //__SILP__
                return channel.RemoveEventChecker(checker);                         //__SILP__
            } else {                                                                //__SILP__
                Error("Channel Not Found: {0}", channelPath);                       //__SILP__
            }                                                                       //__SILP__
            return false;                                                           //__SILP__
        }                                                                           //__SILP__
                                                                                    //__SILP__

        //SILP: ADD_REMOVE_HELPER(EventListener, channelPath, channel, Channel, EventListener, listener, EventListener)
        public bool AddEventListener(string channelPath, EventListener listener) {     //__SILP__
            Channel channel = Get<Channel>(channelPath);                               //__SILP__
            if (channel != null) {                                                     //__SILP__
                return channel.AddEventListener(listener);                             //__SILP__
            } else {                                                                   //__SILP__
                Error("Channel Not Found: {0}", channelPath);                          //__SILP__
            }                                                                          //__SILP__
            return false;                                                              //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
        public bool RemoveEventListener(string channelPath, EventListener listener) {  //__SILP__
            Channel channel = Get<Channel>(channelPath);                               //__SILP__
            if (channel != null) {                                                     //__SILP__
                return channel.RemoveEventListener(listener);                          //__SILP__
            } else {                                                                   //__SILP__
                Error("Channel Not Found: {0}", channelPath);                          //__SILP__
            }                                                                          //__SILP__
            return false;                                                              //__SILP__
        }                                                                              //__SILP__
                                                                                       //__SILP__
    }
}
