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
        private Dictionary<int, WeakList<TSub>> _InstanceSubscribers = null;
        private WeakList<TSub> _ClassSubscribers = null;

        public void AddSub(TSub sub) {
            WeakListHelper.Add<TSub>(ref _ClassSubscribers, sub);
        }

        public void AddSub(TPub pub, TSub sub) {
            if (_InstanceSubscribers == null) {
                _InstanceSubscribers = new Dictionary<int, WeakList<TSub>>();
            }
            int pubHash = pub.GetHashCode();
            WeakList<TSub> subs = null;
            if (!_InstanceSubscribers.TryGetValue(pubHash, out subs)) {
                subs = new WeakList<TSub>();
                _InstanceSubscribers[pubHash] = subs;
            }
            subs.Add(sub);
        }

        public void Publish(TPub pub, Action<TSub> callback) {
            if (_InstanceSubscribers != null) {
                int pubHash = pub.GetHashCode();
                WeakList<TSub> subs = null;
                if (_InstanceSubscribers.TryGetValue(pubHash, out subs)) {
                    subs.ForEach(callback);

                    if (subs.Count == 0) {
                        _InstanceSubscribers.Remove(pubHash);
                    }
                }
            }
            WeakListHelper.Notify(_ClassSubscribers, callback);
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
