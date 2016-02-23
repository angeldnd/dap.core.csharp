using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class PathConsts {
        public const char PathSeparator = '/';

        public const string PathSeparatorAsString = "/";

        public static string PathToKey(string path) {
            return path.Replace(PathSeparator, DictConsts.KeySeparator);
        }

        public static string KeyToPath(string key) {
            return key.Replace(DictConsts.KeySeparator, PathSeparator);
        }
    }
}
