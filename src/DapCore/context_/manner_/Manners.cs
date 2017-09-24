using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public sealed class Manners : DictAspect<IContext, IManner> {
        public Manners(IContext owner, string key) : base(owner, key) {
        }

        public bool WaitManner(string mannerKey, Action<IManner, bool> callback) {
            return Owner.Utils.WaitElement(this, mannerKey, callback);
        }
    }
}
