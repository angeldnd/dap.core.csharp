using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public class DataCache {
        public const int Default_Capacity = 16;
        public const int Default_SubKind_Capacity = 4;

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
        private List<WeakDataRef> _Temp = new List<WeakDataRef>();
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
            if (_DataPool.Count <= 0) {
                DoCollect();
                if (_DataPool.Count <= Capacity / 2) {
                    EnsureCapacity(_Instances.Count / 2);
                }
            }
            RealData real = _DataPool.Take(true);
            WeakData weak = Register(real);
            if (profiler != null) profiler.EndSample();
            return weak;
        }

        private WeakData Register(RealData real) {
            IProfiler profiler = Log.BeginSample("Register: " + real.CapacityTip);
            WeakData weak = new WeakData(Kind, real);
            WeakDataRef r = _RefPool.Take();
            if (r == null) {
                r = new WeakDataRef(weak, real);
            } else {
                r._Reuse(weak, real);
            }
            _Instances.Add(r);
            if (profiler != null) profiler.EndSample();
            return weak;
        }

        private int DoCollect() {
            IProfiler profiler = Log.BeginSample("DoCollect: " + _Instances.Count);
            List<WeakDataRef> tmp = _Temp;
            _Temp = _Instances;
            _Instances = tmp;

            int count = 0;
            foreach (WeakDataRef r in _Temp) {
                if (r.IsAlive) {
                    _Instances.Add(r);
                } else {
                    count++;
                    if (profiler != null) profiler.BeginSample("Collected");
                    _DataPool.Add(r.Real);
                    _RefPool.Add(r);
                    if (profiler != null) profiler.EndSample();
                }
            }
            _Temp.Clear();
            //Log.Error("DataCache.DoCollect {0} -> {1} -> {2}/{3}", Kind, count, _DataPool.Count, _Instances.Count);
            if (profiler != null) profiler.EndSample();
            return count;
        }
    }
}
