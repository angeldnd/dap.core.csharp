using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Table<T> {
        private WeakList<ITableWatcher<T>> _GenericWatchers = null;
        private Dictionary<ITableWatcher, ITableWatcher<T>> _GenericWatcherMapping = new Dictionary<ITableWatcher, ITableWatcher<T>>();

        private WeakList<ITableWatcher<T>> _Watchers = null;

        private ITableWatcher<T> GetWrapperWatcher(ITableWatcher watcher) {
            ITableWatcher<T> wrapper = null;
            if (_GenericWatcherMapping.TryGetValue(watcher, out wrapper)) {
                return wrapper;
            }
            return null;
        }

        private ITableWatcher<T> CreateWrapperWatcher<T1>(ITableWatcher<T1> watcher)
                                    where T1 : class, IInTableElement {
            BlockTableWatcher<T> wrapper = new BlockTableWatcher<T>(this,
                (T _element) => {
                    T1 element = _element as T1;
                    if (element != null) {
                        watcher.OnElementAdded(element);
                    }
                },
                (T _element) => {
                    T1 element = _element as T1;
                    if (element != null) {
                        watcher.OnElementAdded(element);
                    }
                },
                (T _element) => {
                    T1 element = _element as T1;
                    if (element != null) {
                        watcher.OnElementIndexChanged(element);
                    }
                }
            );
            return wrapper;
        }

        public bool AddTableWatcher<T1>(ITableWatcher<T1> watcher)
                                    where T1 : class, IInTableElement {
            ITableWatcher<T> wrapper = GetWrapperWatcher(watcher);;
            if (wrapper == null) {
                wrapper = CreateWrapperWatcher<T1>(watcher);
                return WeakListHelper.Add(ref _GenericWatchers, wrapper);
            }
            return false;
        }

        public bool RemoveTableWatcher<T1>(ITableWatcher<T1> watcher)
                                    where T1 : class, IInTableElement {
            ITableWatcher<T> wrapper = GetWrapperWatcher(watcher);;
            if (wrapper != null) {
                return WeakListHelper.Remove(_GenericWatchers, wrapper);
            }
            return false;
        }

        public bool AddTableWatcher(ITableWatcher<T> watcher) {
            return WeakListHelper.Add(ref _Watchers, watcher);
        }

        public bool RemoveTableWatcher(ITableWatcher<T> watcher) {
            return WeakListHelper.Remove(_Watchers, watcher);
        }
    }
}
