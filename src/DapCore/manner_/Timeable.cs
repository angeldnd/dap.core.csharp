using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class TimeableConsts {
        public const string TypeTimeable = "Timeable";

        public const string MannerTimeable = "timeable";

        [DapParam(typeof(float))]
        public const string PropCurrentTime = "current_time";

        public const string ChannelOnTime = "on_time";
    }

    public static class TimeableExtension {
        public static Timeable AddTimeable(this Manners manners) {
            return manners.Add<Timeable>(TimeableConsts.MannerTimeable);
        }

        public static Timeable GetTimeable(this Manners manners) {
            return manners.Get<Timeable>(TimeableConsts.MannerTimeable);
        }

        public static Timeable GetOrAddTimeable(this Manners manners) {
            return manners.GetOrAdd<Timeable>(TimeableConsts.MannerTimeable);
        }
    }

    public interface ITimedContext : IContext {
        float CurrentTime { get; }
    }

    [DapType(TimeableConsts.TypeTimeable)]
    [DapOrder(DapOrders.Manner)]
    public class Timeable : Manner {
        private ITimedContext _TimeProvider = null;
        public float CurrentTime {
            get {
                return _TimeProvider == null ? -1f: _TimeProvider.CurrentTime;
            }
        }

        private Channel _ChannelOnTime = null;
        public Channel ChannelOnTime {
            get { return _ChannelOnTime; }
        }

        public Timeable(Manners owner, string key) : base(owner, key) {
            _TimeProvider = Context.GetAncestor<ITimedContext>();
            if (_TimeProvider == null) {
                Error("Invalid Timeable: TimeProvider Not Found");
                return;
            }

            IContext contextOwner = Context.GetOwner() as IContext;
            if (contextOwner == null) {
                Error("Invalid Context Owner: {0}", Context.GetOwner());
                return;
            }

            Channel ownerOnTimeChannel = contextOwner.Channels.Get(TimeableConsts.ChannelOnTime);
            if (ownerOnTimeChannel == null) {
                Error("Context Owner Has No OnTime Channel: {0}", contextOwner, TimeableConsts.ChannelOnTime);
                return;
            }

            _ChannelOnTime = Context.Channels.Add(TimeableConsts.ChannelOnTime);
            if (_ChannelOnTime != null) {
                ownerOnTimeChannel.AddEventWatcher(this, OnTime);
            }
        }

        private void OnTime(Channel channel, Data evt) {
            if (!Context.Removed) {
                _ChannelOnTime.FireEvent(evt);
            }
        }
    }
}
