using System;
using System.Collections.Generic;

using angeldnd.dap;
using angeldnd.dap.util;

namespace angeldnd.dap {
    public class DataEntry : Entry {
        private DataProperty _Data;
        public Data Data {
            get { return _Data.Value; }
        }

        public override void OnAdded() {
            _Data = Item.AddData(BundleConsts.PropEntryValue, Pass, null);
        }

        public override bool Setup(byte[] bytes) {
            if (Data != null) {
                Error("Already Setup: {0} -> {1}", Data, bytes.Length);
                return false;
            }
            _Data.SetValue(Pass, BundleLoader.ConvertToData(bytes));
            if (Data == null) return false;
            return OnSetup();
        }

        protected virtual bool OnSetup() {
            return true;
        }
    }
}

