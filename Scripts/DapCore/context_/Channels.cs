using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public sealed class Channels : TreeSection<IContext, Channel> {
        public Channels(IContext owner, Pass pass) : base(owner, pass) {
        }

        public Channel GetChannel(string channelPath) {
            return Get(channelPath);
        }

        public Channel AddChannel(string channelPath, Pass pass) {
            return Add(channelPath, pass);
        }

        public Channel AddChannel(string channelPath) {
            return AddChannel(channelPath, null);
        }

        public bool FireEvent(string channelPath, Pass pass, Data evt) {
            Channel channel = Get(channelPath);
            if (channel != null) {
                return channel.FireEvent(pass, evt);
            } else {
                Error("Invalid Channel Path: {0}", channelPath);
            }
            return false;
        }

        public bool FireEvent(string channelPath, Pass pass) {
            return FireEvent(channelPath, pass, null);
        }

        public bool FireEvent(string channelPath, Data evt) {
            return FireEvent(channelPath, null, evt);
        }

        public bool FireEvent(string channelPath) {
            return FireEvent(channelPath, null, null);
        }

        //SILP: ADD_REMOVE_HELPER(EventChecker, channelPath, channel, Channel, EventChecker, checker, IEventChecker)
        public bool AddEventChecker(string channelPath, IEventChecker checker) {     //__SILP__
            Channel channel = Get<Channel>(channelPath);                             //__SILP__
            if (channel != null) {                                                   //__SILP__
                return channel.AddEventChecker(checker);                             //__SILP__
            } else {                                                                 //__SILP__
                Error("Channel Not Found: {0}", channelPath);                        //__SILP__
            }                                                                        //__SILP__
            return false;                                                            //__SILP__
        }                                                                            //__SILP__
                                                                                     //__SILP__
        public bool RemoveEventChecker(string channelPath, IEventChecker checker) {  //__SILP__
            Channel channel = Get<Channel>(channelPath);                             //__SILP__
            if (channel != null) {                                                   //__SILP__
                return channel.RemoveEventChecker(checker);                          //__SILP__
            } else {                                                                 //__SILP__
                Error("Channel Not Found: {0}", channelPath);                        //__SILP__
            }                                                                        //__SILP__
            return false;                                                            //__SILP__
        }                                                                            //__SILP__
                                                                                     //__SILP__

        //SILP: ADD_REMOVE_HELPER(EventListener, channelPath, channel, Channel, EventListener, listener, IEventListener)
        public bool AddEventListener(string channelPath, IEventListener listener) {     //__SILP__
            Channel channel = Get<Channel>(channelPath);                                //__SILP__
            if (channel != null) {                                                      //__SILP__
                return channel.AddEventListener(listener);                              //__SILP__
            } else {                                                                    //__SILP__
                Error("Channel Not Found: {0}", channelPath);                           //__SILP__
            }                                                                           //__SILP__
            return false;                                                               //__SILP__
        }                                                                               //__SILP__
                                                                                        //__SILP__
        public bool RemoveEventListener(string channelPath, IEventListener listener) {  //__SILP__
            Channel channel = Get<Channel>(channelPath);                                //__SILP__
            if (channel != null) {                                                      //__SILP__
                return channel.RemoveEventListener(listener);                           //__SILP__
            } else {                                                                    //__SILP__
                Error("Channel Not Found: {0}", channelPath);                           //__SILP__
            }                                                                           //__SILP__
            return false;                                                               //__SILP__
        }                                                                               //__SILP__
                                                                                        //__SILP__
    }
}
