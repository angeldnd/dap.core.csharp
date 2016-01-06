using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class WeakListHelper {
        public static int Count<T>(WeakList<T> list) where T : class {
            if (list != null) {
                return list.Count;
            }
            return 0;
        }

        public static bool Add<T>(ref WeakList<T> list, T element) where T : class {
            if (element == null) return false;
            if (list == null) {
                list = new WeakList<T>();
            }
            return list.AddElement(element);
        }

        public static bool Remove<T>(WeakList<T> list, T element) where T : class {
            if (element == null) return false;
            if (list != null) {
                return list.Remove(element);
            }
            return false;
        }

        public static void Notify<T>(WeakList<T> list, Action<T> callback) where T : class {
            if (list != null) {
                list.ForEach(callback);
            }
        }

        public static bool IsValid<T>(WeakList<T> list, Func<T, bool> callback) where T : class {
            if (list != null) {
                return list.UntilFalse(callback);
            }
            return true;
        }
    }
}
