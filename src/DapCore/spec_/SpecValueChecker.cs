using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface ISpecValueChecker : IValueChecker {
        bool DoEncode(Data spec);
    }

    public abstract class SpecValueChecker<T> : ISpecValueChecker, IValueChecker<T> {
        public string BlockName {
            get {
                return GetType().Name;
            }
        }

        public bool IsValid(IVar<T> v, T newVal) {
            return IsValid(newVal);
        }

        protected abstract bool IsValid(T val);
        public abstract bool DoEncode(Data spec);
    }
}
