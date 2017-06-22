using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public class DataCache {
        public const int Default_Capacity = 16;
        public const int Default_SubKind_Capacity = 4;

        private static Dictionary<string, DataCache> _Caches = new Dictionary<string, DataCache>();

        private static DataCache GetCache(string kind, int capacity, bool noProfile) {
            DataCache cache = null;
            if (!_Caches.TryGetValue(kind, out cache)) {
                cache = new DataCache(kind, capacity, noProfile);
                _Caches[kind] = cache;
            }
            return cache;
        }

        public static WeakData Take(string kind, bool noProfile = false) {
            return GetCache(kind, Default_Capacity, noProfile).Take(noProfile);
        }

        //When calling from threads other than main thread, need to specify noProfile = true
        public static WeakData Take(string kind, string subKind, bool noProfile = false) {
            return GetCache(kind + "." + subKind, Default_SubKind_Capacity, noProfile).Take(noProfile);
        }

        public static WeakData Take(ILogger caller, bool noProfile = false) {
            string kind = caller.GetType().FullName;
            return Take(kind, noProfile);
        }

        public static WeakData Take(ILogger caller, string subKind, bool noProfile = false) {
            string kind = caller.GetType().FullName;
            return Take(kind, subKind, noProfile);
        }

        public readonly string Kind = null;
        public readonly int Capacity;
        private List<WeakDataRef> _Instances = new List<WeakDataRef>();
        private List<WeakDataRef> _Temp = new List<WeakDataRef>();
        private WeakDataRefPool _RefPool;

        private RealDataPool _DataPool;

        public DataCache(string kind, int capacity, bool noProfile) {
            Kind = kind;
            Capacity = capacity;

            _RefPool = new WeakDataRefPool();
            _DataPool = new RealDataPool(Capacity);
            EnsureCapacity(capacity, noProfile);
        }

        private void EnsureCapacity(int capacity, bool noProfile) {
            if (capacity < Capacity) {
                capacity = Capacity;
            }
            if (Log.Profiler != null) Log.Profiler.BeginSample("DataCache.EnsureCapacity: " + Kind + " " + capacity.ToString());
            _DataPool.EnsureCapacity(capacity);
            if (Log.Profiler != null) Log.Profiler.EndSample();
        }

        private WeakData Take(bool noProfile) {
            if (Log.Profiler != null) Log.Profiler.BeginSample("DataCache.Take: " + Kind);
            if (_DataPool.Count <= 0) {
                DoCollect(noProfile);
                if (_DataPool.Count <= Capacity / 2) {
                    EnsureCapacity(_Instances.Count / 2, noProfile);
                }
            }
            RealData real = _DataPool.Take(true);
            WeakData weak = Register(real, noProfile);
            if (Log.Profiler != null) Log.Profiler.EndSample();
            return weak;
        }

        private WeakData Register(RealData real, bool noProfile) {
            if (Log.Profiler != null) Log.Profiler.BeginSample("Register: " + real.CapacityTip);
            WeakData weak = new WeakData(Kind, real);
            WeakDataRef r = _RefPool.Take();
            if (r == null) {
                r = new WeakDataRef(weak, real);
            } else {
                r._Reuse(weak, real);
            }
            _Instances.Add(r);
            if (Log.Profiler != null) Log.Profiler.EndSample();
            return weak;
        }

        private int DoCollect(bool noProfile = false) {
            if (Log.Profiler != null) Log.Profiler.BeginSample("DoCollect: " + _Instances.Count);
            List<WeakDataRef> tmp = _Temp;
            _Temp = _Instances;
            _Instances = tmp;

            int count = 0;
            foreach (WeakDataRef r in _Temp) {
                if (r.IsAlive) {
                    _Instances.Add(r);
                } else {
                    count++;
                    if (Log.Profiler != null) Log.Profiler.BeginSample("Collected");
                    _DataPool.Add(r.Real);
                    _RefPool.Add(r);
                    if (Log.Profiler != null) Log.Profiler.EndSample();
                }
            }
            _Temp.Clear();
            //Log.Error("DataCache.DoCollect {0} -> {1} -> {2}/{3}", Kind, count, _DataPool.Count, _Instances.Count);
            if (Log.Profiler != null) Log.Profiler.EndSample();
            return count;
        }
    }
}
