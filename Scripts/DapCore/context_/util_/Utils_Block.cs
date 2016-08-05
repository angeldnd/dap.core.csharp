using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public partial class Utils {
        private List<BlockOwner> _BlockOwners = null;

        public BlockOwner RetainBlockOwner() {
            if (_BlockOwners == null) {
                _BlockOwners = new List<BlockOwner>();
            }

            BlockOwner owner = new BlockOwner();
            _BlockOwners.Add(owner);
            return owner;
        }

        /*
         * A bit hacky here, since the weak reference collection is
         * not under control, for cases that only want to be triggered
         * once, need to check the result here.
         * Use WaitHandler() as an example, in the watcher might add
         * another handler, which will trigger the watcher again.
         */
        public bool ReleaseBlockOwner(ref BlockOwner owner) {
            if (owner == null) {
                return false;
            }
            if (_BlockOwners != null) {
                int index = _BlockOwners.IndexOf(owner);
                if (index >= 0) {
                    _BlockOwners.RemoveAt(index);
                    owner = null;
                    return true;
                }
            }
            Error("Invalid BlockOwner: {0}", owner);
            owner = null;
            return false;
        }

        public bool WaitElement<T>(IDict<T> dict, string key, Action<T, bool> callback) where T : class, IInDictElement {
            T existElement = dict.Get(key, true);
            if (existElement != null) {
                callback(existElement, false);
                return false;
            }
            BlockOwner owner = RetainBlockOwner();
            dict.AddDictWatcher(new BlockDictElementAddedWatcher<T>(owner, (T element) => {
                if (owner == null) return;
                if (element.Key == key) {
                    if (ReleaseBlockOwner(ref owner)) {
                        callback(element, true);
                    }
                }
            }));
            return true;
        }
    }
}
