using System;
using System.Collections.Generic;

using angeldnd.dap;
using angeldnd.dap.util;

namespace angeldnd.dap {
    public class TextEntry : Entry {
        private StringProperty _Text;
        public string Text {
            get { return _Text.Value; }
        }

        public override void OnAdded() {
            _Text = Item.AddString(BundleConsts.PropEntryValue, Pass, null);
        }

        public override bool Setup(byte[] bytes) {
            if (Text != null) {
                Error("Already Setup: {0} -> {1}", Text, bytes.Length);
                return false;
            }
            _Text.SetValue(Pass, BundleLoader.ConvertToText(bytes));
            if (Text == null) return false;
            return OnSetup();
        }

        protected virtual bool OnSetup() {
            return true;
        }
    }
}

