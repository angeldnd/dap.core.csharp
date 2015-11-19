using System;
using System.Collections.Generic;

using angeldnd.dap;
using angeldnd.dap.util;

namespace angeldnd.dap {
    public class BinEntry : Entry {
        private Var<byte[]> _Bytes;
        public byte[] Bytes {
            get { return _Bytes.Value; }
        }

        public override void OnAdded() {
            _Bytes = Item.Vars.AddVar<byte[]>(BundleConsts.VarEntryValue, Pass, null);
        }

        public override bool Setup(byte[] bytes) {
            if (Bytes != null) {
                Error("Already Setup: {0} -> {1}", Bytes.Length, bytes.Length);
                return false;
            }
            _Bytes.SetValue(Pass, bytes);
            if (Bytes == null) return false;
            return OnSetup();
        }

        protected virtual bool OnSetup() {
            return true;
        }
    }
}

