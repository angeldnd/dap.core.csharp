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

        public static WeakData _Register(string kind, string subKind, RealData real) {
            return GetCache(kind + "." + subKind, Default_SubKind_Capacity).Register(real);
        }

        public readonly string Kind = null;
        public readonly int Capacity;
        private Queue<WeakDataRef> _Instances = new Queue<WeakDataRef>();
        private Queue<WeakDataRef> _Temp = new Queue<WeakDataRef>();
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
            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.BeginSample("DataCache.EnsureCapacity: " + Kind + " " + capacity.ToString());
            #endif
            _DataPool.EnsureCapacity(capacity);
            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.EndSample();
            #endif
        }

        public WeakData Take() {
            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.BeginSample("DataCache.Take: " + Kind);
            #endif
            if (_DataPool.Count <= 0) {
                DoCollect();
                if (_DataPool.Count <= Capacity / 2) {
                    EnsureCapacity(_Instances.Count / 2);
                }
            }
            RealData real = _DataPool.Take(true);
            WeakData weak = Register(real);
            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.EndSample();
            #endif
            return weak;
        }

        private WeakData Register(RealData real) {
            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.BeginSample("Register: " + real.CapacityTip);
            #endif
            WeakData weak = new WeakData(Kind, real);
            WeakDataRef r = _RefPool.Take();
            if (r == null) {
                r = new WeakDataRef(weak, real);
            } else {
                r._Reuse(weak, real);
            }
            _Instances.Enqueue(r);
            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.EndSample();
            #endif
            return weak;
        }

        public int DoCollect() {
            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.BeginSample("DoCollect: " + _Instances.Count);
            #endif
            Queue<WeakDataRef> tmp = _Temp;
            _Temp = _Instances;
            _Instances = tmp;

            int count = 0;
            while (_Temp.Count > 0) {
                WeakDataRef r = _Temp.Dequeue();
                if (r.IsAlive) {
                    _Instances.Enqueue(r);
                } else {
                    count++;
                    #if UNITY_EDITOR
                    UnityEngine.Profiling.Profiler.BeginSample("Collected");
                    #endif
                    _DataPool.Add(r.Real);
                    _RefPool.Add(r);
                    #if UNITY_EDITOR
                    UnityEngine.Profiling.Profiler.EndSample();
                    #endif
                }
            }
            //Log.Error("DataCache.DoCollect {0} -> {1} -> {2}/{3}", Kind, count, _DataPool.Count, _Instances.Count);
            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.EndSample();
            #endif
            return count;
        }
    }
}
