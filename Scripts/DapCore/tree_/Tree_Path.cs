using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Tree<T> {
        public int GetDepth(string path) {
            return TreeHelper.GetDepth(Separator, path);
        }

        public string GetName(string path) {
            return TreeHelper.GetName(Separator, path);
        }

        public string GetParentPath(string path) {
            return TreeHelper.GetParentPath(Separator, path);
        }
    }
}
