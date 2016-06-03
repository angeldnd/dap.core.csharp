using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Dict<T> {
        private WeakList<IDictWatcher<T>> _GenericWatchers = null;
        private Dictionary<IDictWatcher, IDictWatcher<T>> _GenericWatcherMapping = new Dictionary<IDictWatcher, IDictWatcher<T>>();

        private WeakList<IDictWatcher<T>> _Watchers = null;

        public int GenericDictWatcherCount {
            get { return WeakListHelper.Count(_GenericWatchers); }
        }

        public int DictWatcherCount {
            get { return WeakListHelper.Count(_Watchers); }
        }

        private IDictWatcher<T> GetWrapperWatcher(IDictWatcher watcher) {
            IDictWatcher<T> wrapper = null;
            if (_GenericWatcherMapping.TryGetValue(watcher, out wrapper)) {
                return wrapper;
            }
            return null;
        }

        private IDictWatcher<T> CreateWrapperWatcher<T1>(IDictWatcher<T1> watcher)
                                    where T1 : class, IInDictElement {
            BlockDictWatcher<T> wrapper = new BlockDictWatcher<T>(this,
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
                }
            );
            return wrapper;
        }

        public bool AddDictWatcher<T1>(IDictWatcher<T1> watcher)
                                    where T1 : class, IInDictElement {
            IDictWatcher<T> wrapper = GetWrapperWatcher(watcher);;
            if (wrapper == null) {
                wrapper = CreateWrapperWatcher<T1>(watcher);
                return WeakListHelper.Add(ref _GenericWatchers, wrapper);
            }
            return false;
        }

        public bool RemoveDictWatcher<T1>(IDictWatcher<T1> watcher)
                                    where T1 : class, IInDictElement {
            IDictWatcher<T> wrapper = GetWrapperWatcher(watcher);;
            if (wrapper != null) {
                return WeakListHelper.Remove(_GenericWatchers, wrapper);
            }
            return false;
        }

        public bool AddDictWatcher(IDictWatcher<T> watcher) {
            return WeakListHelper.Add(ref _Watchers, watcher);
        }

        public bool RemoveDictWatcher(IDictWatcher<T> watcher) {
            return WeakListHelper.Remove(_Watchers, watcher);
        }
    }
}
