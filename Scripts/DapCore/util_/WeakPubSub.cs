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
    public class WeakPubSub<TPub, TSub> where TSub : class {
        /*
         * Using List here since the list is mostly very short, also it's faster and more stable for Publish
         * Initialize in lazy way.
         */
        private Dictionary<int, List<WeakReference>> _InstanceSubscribers = null;
        private List<WeakReference> _ClassSubscribers = null;

        private void AddSub(List<WeakReference> subs, TSub sub) {
            for (int i = 0; i < subs.Count; i++) {
                if (subs[i].IsAlive && subs[i].Target == sub) {
                    return;
                }
            }

            subs.Add(new WeakReference(sub));
        }

        public void AddSub(TSub sub) {
            if (_ClassSubscribers == null) {
                _ClassSubscribers = new List<WeakReference>();
            }
            AddSub(_ClassSubscribers, sub);
        }

        public void AddSub(TPub pub, TSub sub) {
            if (_InstanceSubscribers == null) {
                _InstanceSubscribers = new Dictionary<int, List<WeakReference>>();
            }
            int pubHash = pub.GetHashCode();
            List<WeakReference> subs = null;
            if (!_InstanceSubscribers.TryGetValue(pubHash, out subs)) {
                subs = new List<WeakReference>();
                _InstanceSubscribers[pubHash] = subs;
            }
            AddSub(subs, sub);
        }

        private void NotifySubs(List<WeakReference> subs, Action<TSub> callback) {
            //The trick here is to reclaim at most one garbage in each publish, so don't need
            //to maintain any List, for better performance and cleaness.
            WeakReference garbage = null;

            for (int i = 0; i < subs.Count; i++) {
                if (subs[i].IsAlive) {
                    callback((TSub)subs[i].Target);
                } else if (garbage == null) {
                    garbage = subs[i];
                }
            }

            if (garbage != null) {
                subs.Remove(garbage);
            }
        }

        public void Publish(TPub pub, Action<TSub> callback) {
            if (_InstanceSubscribers != null) {
                int pubHash = pub.GetHashCode();
                List<WeakReference> subs = null;
                if (_InstanceSubscribers.TryGetValue(pubHash, out subs)) {
                    NotifySubs(subs, callback);

                    if (subs.Count == 0) {
                        _InstanceSubscribers.Remove(pubHash);
                    }
                }
            }
            if (_ClassSubscribers != null) {
                NotifySubs(_ClassSubscribers, callback);
            }
        }

        public void RemovePub(TPub pub) {
            if (_InstanceSubscribers != null) {
                int pubHash = pub.GetHashCode();
                if (_InstanceSubscribers.ContainsKey(pubHash)) {
                    _InstanceSubscribers.Remove(pubHash);
                }
            }
        }
    }
}
