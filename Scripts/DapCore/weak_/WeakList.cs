using System;
using System.Collections.Generic;

/* After Unity using a newer DotNet version, should switch to the
 *
 * System.Runtime.CompilerServices.ConditionalWeakTable
 * WeakReference<T>
 *
 *
 * which use reference instead of hashcode.
 */

namespace angeldnd.dap {
    /*
     * There are some special logic for WeakBlock, since don't want to force all items
     * to implement OnAdded() and OnRemoved().
     */
    public sealed class WeakList<T> where T : class {
        private readonly List<WeakReference> _Elements = new List<WeakReference>();

        private int _LockCount = 0;
        private bool _NeedGc = false;
        private List<KeyValuePair<bool, T>> _Ops = null;

        public int Count {
            get {
                return _Elements.Count;
            }
        }

        public bool Add(T element) {
            if (_LockCount > 0) {
                if (!Contains(element)) {
                    if (_Ops == null) {
                        _Ops = new List<KeyValuePair<bool, T>>();
                    }
                    _Ops.Add(new KeyValuePair<bool, T>(true, element));
                    return true;
                } else {
                    return false;
                }
            } else {
                return DoAddElement(element);
            }
        }

        public bool Remove(T element) {
            if (_LockCount > 0) {
                if (Contains(element)) {
                    if (_Ops == null) {
                        _Ops = new List<KeyValuePair<bool, T>>();
                    }
                    _Ops.Add(new KeyValuePair<bool, T>(false, element));
                    return true;
                } else {
                    return false;
                }
            } else {
                return DoRemoveElement(element);
            }
        }

        public void Clear() {
            _Elements.Clear();
        }

        public int IndexOf(T element) {
            for (int i = 0; i < _Elements.Count; i++) {
                if (_Elements[i].IsAlive && _Elements[i].Target == element) {
                    return i;
                }
            }
            return -1;
        }

        public bool Contains(T element) {
            return IndexOf(element) >= 0;
        }

        private bool DoAddElement(T element) {
            if (!Contains(element)) {
                _Elements.Add(new WeakReference(element));
                WeakBlock block = element as WeakBlock;
                if (block != null) {
                    block.OnAdded();
                }
                return true;
            }
            return false;
        }

        private bool DoRemoveElement(T element) {
            int index = IndexOf(element);
            if (index >= 0) {
                WeakBlock block = element as WeakBlock;
                if (block != null) {
                    block.OnRemoved();
                }
                _Elements.RemoveAt(index);
                return true;
            }
            return false;
        }
        public void ForEach(Action<T> callback) {
            //Duplicated with Until() to prevent GC.Alloc

            //The trick here is to reclaim at most one garbage in each publish, so don't need
            //to maintain any List, for better performance and cleaness.
            int garbageIndex = -1;

            for (int i = 0; i < _Elements.Count; i++) {
                WeakReference element = _Elements[i];
                if (element.IsAlive) {
                    callback((T)element.Target);
                } else if (garbageIndex < 0) {
                    if (Log.LogDebug) {
                        Log.Debug("Garbage Item In WeakList Found: {0}, {1}", this, element.Target);
                    }
                    garbageIndex = i;
                }
            }

            if (garbageIndex >= 0) {
                _Elements.RemoveAt(garbageIndex);
            }
        }

        public int CollectAllGarbage() {
            IProfiler profiler = Log.BeginSample("CollectAll");
            int count = 0;
            int startIndex = 0;
            while (true) {
                if (profiler != null) profiler.BeginSample("CollectOne");
                startIndex = CollectOneGarbage(startIndex);
                if (profiler != null) profiler.EndSample();
                if (startIndex < 0) {
                    break;
                }
                count++;
            }
            if (profiler != null) profiler.EndSample();
            return count;
        }

        private int CollectOneGarbage(int startIndex) {
            int garbageIndex = -1;

            for (int i = startIndex; i < _Elements.Count; i++) {
                WeakReference element = _Elements[i];
                if (!element.IsAlive) {
                    if (Log.LogDebug) {
                        Log.Debug("Garbage Item In WeakList Found: {0}, {1}", this, element.Target);
                    }
                    garbageIndex = i;
                    break;
                }
            }

            if (garbageIndex >= 0) {
                _Elements.RemoveAt(garbageIndex);
                return garbageIndex;
            }
            return -1;
        }

        private bool Until(Func<T, bool> callback, bool breakCondition) {
            bool result = !breakCondition;

            //The trick here is to reclaim at most one garbage in each publish, so don't need
            //to maintain any List, for better performance and cleaness.
            int garbageIndex = -1;

            for (int i = 0; i < _Elements.Count; i++) {
                WeakReference element = _Elements[i];
                if (element.IsAlive) {
                    if (callback((T)element.Target) == breakCondition) {
                        result = breakCondition;
                        break;
                    }
                } else if (garbageIndex < 0) {
                    if (Log.LogDebug) {
                        Log.Debug("Garbage Item In WeakList Found: {0}, {1}", this, element.Target);
                    }
                    garbageIndex = i;
                }
            }

            if (garbageIndex >= 0) {
                _Elements.RemoveAt(garbageIndex);
            }

            return result;
        }

        public bool UntilTrue(Func<T, bool> callback) {
            return Until(callback, true);
        }

        public bool UntilFalse(Func<T, bool> callback) {
            return Until(callback, false);
        }

        // This is for the cases that need better performance, need
        // more trivial codes though, check Channel.cs for example
        public List<WeakReference> RetainLock() {
            IProfiler profiler = Log.BeginSample("WeakList.RetainLock");
            _LockCount++;
            var result = _Elements;
            if (profiler != null) profiler.EndSample();
            return result;
        }

        public void ReleaseLock(bool needGc) {
            if (needGc) {
                _NeedGc = true;
            }
            _LockCount--;
            if (_LockCount == 0) {
                IProfiler profiler = Log.BeginSample("WeakList.ReleaseLock");
                if (_NeedGc) {
                    CollectAllGarbage();
                    _NeedGc = false;
                }
                if (_Ops != null) {
                    foreach (var op in _Ops) {
                        if (op.Key == true) {
                            if (profiler != null) profiler.BeginSample("DoAdd");
                            DoAddElement(op.Value);
                            if (profiler != null) profiler.EndSample();
                        } else {
                            if (profiler != null) profiler.BeginSample("DoRemove");
                            DoRemoveElement(op.Value);
                            if (profiler != null) profiler.EndSample();
                        }
                    }
                    _Ops.Clear();
                }
                if (profiler != null) profiler.EndSample();
            }
        }

        public T GetTarget(WeakReference element) {
            if (element.IsAlive) {
                return (T)element.Target;
            }
            return null;
        }
    }
}
