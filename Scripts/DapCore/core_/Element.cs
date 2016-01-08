using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IElement : IObject {
        ITree GetOwner();

        string Path { get; }
        string RevPath { get; }

        void OnAdded();
        void OnRemoved();
    }

    public interface IElement<TO> : IElement where TO : ITree {
        TO Owner { get; }
    }

    public abstract class Element<TO> : Object, IElement<TO> where TO : Tree {
        /*
         * Constructor(TO owner, string path, Pass pass)
         */
        public readonly TO Owner;
        public readonly string Path;

        protected Element(TO owner, string path, Pass pass) : base(pass) {
            Owner = owner;
            Path = path;
        }

        public string RevPath {
            get {
                return string.Format("{0} ({1})", Path, Revision);
            }
        }

        public override string GetLogPrefix() {
            return string.Format("{0}[{1}] {2} ",
                    Owner.GetLogPrefix(),
                    Type != null ? Type : GetType().Name,
                    RevPath);
        }

        public override bool DebugMode {
            get { return Owner.DebugMode; }
        }

        public override string[] DebugPatterns {
            get { return Owner.DebugPatterns; }
        }

        public virtual void OnAdded() {}
        public virtual void OnRemoved() {}
    }
}
