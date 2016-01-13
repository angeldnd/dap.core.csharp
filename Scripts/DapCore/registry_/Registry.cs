using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;

namespace angeldnd.dap {
    public interface IRegistryWatcher {
        void OnItemAdded(Registry entity, IItem item);
        void OnItemRemoved(Registry entity, IItem item);
    }

    public static class RegistryConsts {
        public const string TypeRegistry = "Registry";

        public const string ChannelTick = "_tick";
    }

    public sealed class Registry : TreeInTreeContext<Env, IItem> {
        public override string Type {
            get { return RegistryConsts.TypeRegistry; }
        }

        public Registry(Env owner, string path, Pass pass) : base(owner, path, pass) {
        }

        public override void OnAdded() {
            //The tick channel will be triggered by some runtime, e.g. in Unity, will be from
            //FixedUpdate(), or other timer on other platform.
            this.AddChannel(RegistryConsts.ChannelTick,
                            this.DepositChannelPass(RegistryConsts.ChannelTick, new Pass()));
        }

        public override void OnRemoved() {
            Channels.Remove(Pass, RegistryConsts.ChannelTick);
        }

        protected override void OnElementAdded(IItem item) {
            WeakListHelper.Notify(_RegistryWatchers, (IRegistryWatcher watcher) => {
                watcher.OnItemAdded(this, item);
            });
        }

        protected override void OnElementRemoved(IItem item) {
            WeakListHelper.Notify(_RegistryWatchers, (IRegistryWatcher watcher) => {
                watcher.OnItemRemoved(this, item);
            });
        }

        //SILP: DECLARE_LIST(RegistryWatcher, watcher, IRegistryWatcher, _RegistryWatchers)
        private WeakList<IRegistryWatcher> _RegistryWatchers = null;    //__SILP__
                                                                        //__SILP__
        public int RegistryWatcherCount {                               //__SILP__
            get { return WeakListHelper.Count(_RegistryWatchers); }     //__SILP__
        }                                                               //__SILP__
                                                                        //__SILP__
        public bool AddRegistryWatcher(IRegistryWatcher watcher) {      //__SILP__
            return WeakListHelper.Add(ref _RegistryWatchers, watcher);  //__SILP__
        }                                                               //__SILP__
                                                                        //__SILP__
        public bool RemoveRegistryWatcher(IRegistryWatcher watcher) {   //__SILP__
            return WeakListHelper.Remove(_RegistryWatchers, watcher);   //__SILP__
        }                                                               //__SILP__
    }
}
