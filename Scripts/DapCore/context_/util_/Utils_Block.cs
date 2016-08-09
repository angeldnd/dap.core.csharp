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

        public bool WaitElement<T>(IDict dict, string key, Action<T, bool> callback) where T : class, IInDictElement {
            T existElement = dict.Get<T>(key, true);
            if (existElement != null) {
                callback(existElement, false);
                return false;
            }
            BlockOwner owner = RetainBlockOwner();
            dict.AddDictWatcher<T>(new BlockDictElementAddedWatcher<T>(owner, (T element) => {
                if (owner == null) return;
                if (element.Key == key) {
                    if (ReleaseBlockOwner(ref owner)) {
                        callback(element, true);
                    }
                }
            }));
            return true;
        }

        public bool WaitSetupAspect<T>(IDict dict, string key, Action<T, bool> callback, bool waitSetup = true)
                                                                                    where T : class, IInDictElement, ISetupAspect {
            bool waitingForSetup = false;
            bool waitingForAspect = WaitElement(dict, key, (T aspect, bool isNew) => {
                if (!aspect.NeedSetup() || !waitSetup) {
                    callback(aspect, isNew);
                    return;
                }
                BlockOwner setupOwner = RetainBlockOwner();
                aspect.AddSetupWatcher(new BlockSetupWatcher(setupOwner, (ISetupAspect _aspect) => {
                    if (ReleaseBlockOwner(ref setupOwner)) {
                        callback(aspect, isNew);
                    }
                }));
                waitingForSetup = true;
            });
            return waitingForAspect || waitingForSetup;
        }
    }
}
