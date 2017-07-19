using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public class DataCache {
        public const int Max_Collect_Tries = 1024;
        public const int Max_Single_Alloc = 128;

        public const int Default_Capacity = 32;
        public const int Default_SubKind_Capacity = 16;

        private static Dictionary<string, DataCache> _Caches = new Dictionary<string, DataCache>();

        private static DataCache GetCache(string kind, int capacity) {
            DataCache cache = null;
            if (!_Caches.TryGetValue(kind, out cache)) {
                cache = new DataCache(kind, capacity);
                _Caches[kind] = cache;
            }
            return cache;
        }

        public static WeakData Take(string kind) {
            return GetCache(kind, Default_Capacity).Take();
        }

        public static WeakData Take(string kind, string subKind) {
            return GetCache(kind + "." + subKind, Default_SubKind_Capacity).Take();
        }

        public static WeakData Take(ILogger caller) {
            string kind = caller.GetType().FullName;
            return Take(kind);
        }

        public static WeakData Take(ILogger caller, string subKind) {
            string kind = caller.GetType().FullName;
            return Take(kind, subKind);
        }

        public readonly string Kind = null;
        public readonly int Capacity;
        private List<WeakDataRef> _Instances = new List<WeakDataRef>();

        private List<WeakDataRef> _Collecting = new List<WeakDataRef>();
        private int _CollectingIndex = 0;

        private WeakDataRefPool _RefPool;
        private RealDataPool _DataPool;

        public DataCache(string kind, int capacity) {
            Kind = kind;
            Capacity = capacity;

            _RefPool = new WeakDataRefPool();
            _DataPool = new RealDataPool(Capacity);
            EnsureCapacity(capacity);
        }

        private void EnsureCapacity(int capacity) {
            if (capacity < Capacity) {
                capacity = Capacity;
            }
            IProfiler profiler = Log.BeginSample("DataCache.EnsureCapacity: " + Kind + " " + capacity.ToString());
            _DataPool.EnsureCapacity(capacity);
            if (profiler != null) profiler.EndSample();
        }

        private WeakData Take() {
            IProfiler profiler = Log.BeginSample("DataCache.Take: " + Kind);
            int capacity = Capacity / 2;
            if (_DataPool.Count <= capacity) {
                DoCollect(capacity);
            }
            if (_DataPool.Count <= capacity) {
                capacity = Capacity;
                if (_Instances.Count / 2 > capacity) {
                    capacity = _Instances.Count / 2;
                    if (capacity > _DataPool.Count + Max_Single_Alloc) {
                        capacity = _DataPool.Count + Max_Single_Alloc;
                    }
                }
                EnsureCapacity(capacity);
            }
            RealData real = _DataPool.Take(true);
            WeakData weak = Register(real);
            if (profiler != null) profiler.EndSample();
            return weak;
        }

        private WeakData Register(RealData real) {
            IProfiler profiler = Log.BeginSample("Register");
            if (profiler != null) profiler.BeginSample("New WeakData");
            WeakData weak = new WeakData(Kind, real);
            if (profiler != null) profiler.EndSample();
            WeakDataRef r = _RefPool.Take();
            if (r == null) {
                if (profiler != null) profiler.BeginSample("New WeakDataRef");
                r = new WeakDataRef(weak, real);
                if (profiler != null) profiler.EndSample();
            } else {
                if (profiler != null) profiler.BeginSample("_Reuse");
                r._Reuse(weak, real);
                if (profiler != null) profiler.EndSample();
            }
            _Instances.Add(r);
            if (profiler != null) profiler.EndSample();
            return weak;
        }

        private int DoCollect(int capacity) {
            IProfiler profiler = Log.BeginSample("DoCollect");

            if (_CollectingIndex >= _Collecting.Count) {
                _Collecting.Clear();
                _CollectingIndex = 0;

                if (_Instances.Count > 0) {
                    List<WeakDataRef> tmp = _Collecting;
                    _Collecting = _Instances;
                    _Instances = tmp;
                }
            }

            int checkedCount = 0;
            int collectedCount = 0;
            while (_CollectingIndex < _Collecting.Count) {
                WeakDataRef r = _Collecting[_CollectingIndex++];
                if (r.IsAlive) {
                    _Instances.Add(r);
                } else {
                    _DataPool.Add(r.Real);
                    _RefPool.Add(r);
                    collectedCount++;
                    if (collectedCount >= capacity) {
                        break;
                    }
                }
                checkedCount ++;
                if (checkedCount >= Max_Collect_Tries) {
                    break;
                }
            }
            if (profiler != null) profiler.BeginSample(string.Format("{0}, {1} -> {2} -> {3}",
                                            _Instances.Count, _Collecting.Count, checkedCount, collectedCount));
            if (profiler != null) profiler.EndSample();
            //Log.Error("DataCache.DoCollect {0} -> {1} -> {2}/{3}", Kind, count, _DataPool.Count, _Instances.Count);
            if (profiler != null) profiler.EndSample();
            return collectedCount;
        }
    }
}
