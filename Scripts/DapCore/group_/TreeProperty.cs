using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public class TreeProperty<T> : ComboProperty
                                                where T : class, IProperty {
        public const string KeySelf = "Self";
        public const string KeyChildren = "Children";

        private T _Self = null;
        public T Self {
            get { return _Self; }
        }

        private DictProperty<TreeProperty<T>> _Children = null;
        public DictProperty<TreeProperty<T>> Children {
            get { return _Children; }
        }

        public TreeProperty(IDictProperties owner, string key) : base(owner, key) {
            OnInit();
        }

        public TreeProperty(ITableProperties owner, int index) : base(owner, index) {
            OnInit();
        }

        private void OnInit() {
            _Self = Add<T>(KeySelf);
            _Children = Add<DictProperty<TreeProperty<T>>>(KeyChildren);
        }
    }
}
