using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;

namespace angeldnd.dap {
    public interface IRegistryWatcher {
        void OnItemAdded(IRegistry registry, IItem item);
        void OnItemRemoved(IRegistry registry, IItem item);
    }

    public interface IRegistry : IInTreeElement<Env>, IContext, ITree {
        int RegistryWatcherCount { get; }
        bool AddRegistryWatcher(IRegistryWatcher watcher);
        bool RemoveRegistryWatcher(IRegistryWatcher watcher);
    }

    public interface IRegistry<T> : ITree<T>, IRegistry
                                        where T : class, IItem {
    }

    public static class RegistryConsts {
        public const string TypeRegistry = "Registry";
    }

    public sealed class Registry : Registry<IItem> {
        public override string Type {
            get { return RegistryConsts.TypeRegistry; }
        }

        public Registry(Env owner, string path, Pass pass) : base(owner, path, pass) {
        }
    }

    public abstract class Registry<T> : TreeInTreeContext<Env, T>, IRegistry<T>
                                            where T : class, IItem {
        public Registry(Env owner, string path, Pass pass) : base(owner, path, pass) {
        }

        protected override void OnElementAdded(T item) {
            WeakListHelper.Notify(_RegistryWatchers, (IRegistryWatcher watcher) => {
                watcher.OnItemAdded(this, item);
            });
        }

        protected override void OnElementRemoved(T item) {
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
