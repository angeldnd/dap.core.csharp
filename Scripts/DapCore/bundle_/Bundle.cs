using System;
using System.Collections.Generic;

using angeldnd.dap;
using angeldnd.dap.util;

namespace angeldnd.dap {
    public struct BundleConsts {
        public const string TypeBundle = "Bundle";
        public const string TypeDataEntry = "DataEntry";
        public const string TypeTextEntry = "TextEntry";
        public const string TypeBinEntry = "BinEntry";

        public const string PropEntryValue = "_v";
        public const string VarEntryValue = "_v";

        public const string AspectLoader = "_loader";

        public const string BundleIndexPath = "index";

        [DapParam(typeof(string))]
        public const string PropBundlePath = "_bundle_path";
    }

    public sealed class Bundle : ItemType {
        private StringProperty _BundlePath = null;
        public string BundlePath {
            get { return _BundlePath.Value; }
        }
        private string _Password = null;

        public override void OnAdded() {
            _BundlePath = Item.AddString(BundleConsts.PropBundlePath, Pass, "");
        }

        public bool Setup(string bundlePath, string password) {
            if (_Password != null) {
                Error("Already Setup: {0} -> {1}", _Password, password);
                return false;
            }

            _BundlePath.SetValue(Pass, bundlePath);

            //Not put it into property, since it's private information
            _Password = password;

            BundleLoader loader = Item.Get<BundleLoader>(BundleConsts.AspectLoader);
            if (loader == null) {
                Error("BundleLoader Not Found");
                return false;
            }
            bool result = LoadIndex(loader);
            if (loader != null) {
                loader.OnDone();
            }
            return result;
        }

        private bool LoadIndex(BundleLoader loader) {
            Data indexData = loader.LoadIndex(_Password);
            if (indexData == null) {
                Error("Load Index Data Failed");
                return false;
            }
            foreach (string key in indexData.Keys) {
                string entryType = indexData.GetString(key, null);
                if (entryType == null) {
                    Error("Invalid Entry Data: {0} -> {1}", key, indexData.GetValue(key));
                    return false;
                }
                byte[] entryBytes = loader.LoadBytes(_Password, key);
                if (entryBytes == null) {
                    Error("Load Entry Bytes Failed: {0}", key);
                    return false;
                }
                Item item = Registry.AddItem(GetDescendantPath(key), entryType);
                if (item == null) {
                    return false;
                }
                Entry entry = item.ItemType as Entry;
                if (entry == null) {
                    Error("Invalid Entry Item: {0} -> {1}", key, item.ItemType.GetType());
                    return false;
                }
                if (!entry.Setup(entryBytes)) {
                    Error("Setup Entry Failed: {0} -> {1}}", key, entryBytes.Length);
                    return false;
                }
            }
            return true;
        }

        public T GetEntry<T>(string relativePath) where T : Entry {
            return Registry.GetDescendant<T>(Item.Path, relativePath);
        }
    }
}

