using System;
using System.Collections.Generic;
using System.Text;

namespace angeldnd.dap {
    public class DataCache {
        public const int Default_Capacity = 16;

        private static Dictionary<string, DataCache> _Caches = new Dictionary<string, DataCache>();

        public static DataCache GetCache(string kind) {
            DataCache cache = null;
            if (!_Caches.TryGetValue(kind, out cache)) {
                cache = new DataCache(kind);
                _Caches[kind] = cache;
            }
            return cache;
        }

        public static WeakData Take(string kind) {
            return GetCache(kind).Take();
        }

        public static WeakData Take(string kind, string subKind) {
            return Take(kind + "." + subKind);
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
        private Queue<WeakDataRef> _Instances = new Queue<WeakDataRef>();
        private Queue<WeakDataRef> _Temp = new Queue<WeakDataRef>();
        private RealDataPool _Pool = new RealDataPool(Default_Capacity);

        public DataCache(string kind) {
            Kind = kind;
        }

        public WeakData Take() {
            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.BeginSample("DataCache.Take: " + Kind);
            #endif
            if (_Pool.Count <= 0) {
                DoCollect();
                if (_Pool.Count <= Default_Capacity / 2) {
                    _Pool.EnsureCapacity(_Instances.Count / 2);
                }
            }
            RealData real = _Pool.Take(true);
            WeakData weak = new WeakData(Kind, real);
            WeakDataRef r = new WeakDataRef(weak, real);
            _Instances.Enqueue(r);
            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.EndSample();
            #endif
            return weak;
        }

        public int DoCollect() {
            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.BeginSample("DataCache.DoCollect: " + Kind);
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
                    _Pool.Add(r.Real);
                }
            }
            //Log.Error("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA {0} -> {1} -> {2}/{3}", Kind, count, _Pool.Count, _Instances.Count);
            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.EndSample();
            #endif
            return count;
        }
    }
}
