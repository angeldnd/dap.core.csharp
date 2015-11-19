using System;
using System.Collections.Generic;

using angeldnd.dap;
using angeldnd.dap.util;

namespace angeldnd.dap {
    public abstract class BundleLoader : ItemAspect {
        public static Data ConvertToData(byte[] bytes) {
            if (bytes != null) {
                return DataSerializer.ReadData(bytes);
            }
            return null;
        }

        public static string ConvertToText(byte[] bytes) {
            if (bytes != null) {
                return StringHelper.DecodeUtf8FromBytes(bytes);
            }
            return null;
        }

        public Data LoadIndex(string password) {
            return ConvertToData(LoadBytes(password, BundleConsts.BundleIndexPath));
        }

        public abstract byte[] LoadBytes(string password, string path);

        public virtual void OnDone() {}
    }
}

