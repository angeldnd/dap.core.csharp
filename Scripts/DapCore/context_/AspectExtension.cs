using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class AspectExtension {
        public static void ForEachAspects<T>(this IAspect aspect, Action<T> callback)
                                                    where T : class, IAspect {
            T asT = aspect as T;
            if (asT != null) {
                callback(asT);
            }
            IDict asDict = aspect as IDict;
            if (asDict != null) {
                TreeHelper.ForEachDescendants<T>(asDict, callback);
            }
            ITable asTable = aspect as ITable;
            if (asTable != null) {
                TreeHelper.ForEachDescendants<T>(asTable, callback);
            }
        }
    }
}
