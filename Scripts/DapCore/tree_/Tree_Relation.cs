using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public abstract partial class Tree<T> {
        public T1 GetParent<T1>(string path) where T1 : class, T {
            return Get<T1>(TreeHelper.GetParentPath(Separator, path));
        }

        public void FilterChildren<T1>(string path, Action<T1> callback) where T1 : class, T {
            Filter<T1>(TreeHelper.GetChildrenPattern(Separator, path), callback);
        }

        public List<T1> GetChildren<T1>(string path) where T1 : class, T {
            return Filter<T1>(TreeHelper.GetChildrenPattern(Separator, path));
        }

        public T1 GetAncestor<T1>(string path) where T1 : class, T {
            T parent = GetParent<T>(path);
            if (parent == null) {
                return null;
            } else {
                if (parent is T1) {
                    return (T1)parent;
                } else {
                    return GetAncestor<T1>(parent.Path);
                }
            }
        }

        public T1 GetDescendant<T1>(string path, string relativePath) where T1 : class, T {
            return Get<T1>(TreeHelper.GetDescendantPath(Separator, path, relativePath));
        }

        public void FilterDescendants<T1>(string path, Action<T1> callback) where T1 : class, T {
            Filter<T1>(TreeHelper.GetDescendantsPattern(Separator, path), callback);
        }

        public List<T1> GetDescendants<T1>(string path) where T1 : class, T {
            return Filter<T1>(TreeHelper.GetDescendantsPattern(Separator, path));
        }

        public T GetParent(string path) {
            return GetParent<T>(path);
        }

        public void FilterChildren(string path, Action<T> callback) {
            FilterChildren<T>(path, callback);
        }

        public List<T> GetChildren(string path) {
            return GetChildren<T>(path);
        }

        public T GetAncestor(string path) {
            return GetAncestor<T>(path);
        }

        public T GetDescendant(string path, string relativePath) {
            return GetDescendant<T>(path, relativePath);
        }

        public void FilterDescendants(string path, Action<T> callback) {
            FilterDescendants<T>(path, callback);
        }

        public List<T> GetDescendants(string path) {
            return GetDescendants<T>(path);
        }
    }
}
