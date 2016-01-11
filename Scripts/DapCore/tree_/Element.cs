using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IOwner : IObject {
    }

    public interface IElement : IObject {
        IOwner GetOwner();

        string Path { get; }
        string RevPath { get; }

        bool AdminSecured { get; }
        bool WriteSecured { get; }
        bool CheckAdminPass(Pass pass);
        bool CheckWritePass(Pass pass);

        void OnAdded();
        void OnRemoved();
    }

    public interface IElement<TO> : IElement
                                        where TO : IOwner {
        TO Owner { get; }
    }

    public abstract class Element<TO> : Object, IElement<TO>
                                            where TO : IOwner {
        /*
         * Constructor(TO owner, string path, Pass pass)
         */
        private readonly TO _Owner;
        public TO Owner {
            get { return _Owner; }
        }
        public IOwner GetOwner() {
            return _Owner;
        }

        private readonly string _Path;
        public string Path {
            get { return _Path; }
        }

        private readonly Pass _Pass;
        public Pass Pass {
            get { return _Pass; }
        }

        protected Element(TO owner, string path, Pass pass) {
            _Owner = owner;
            _Path = path;
            _Pass = pass;
        }

        public string RevPath {
            get {
                return string.Format("{0} ({1})", Path, Revision);
            }
        }

        public override string LogPrefix {
            get {
                return string.Format("{0}{1}",
                        Owner.LogPrefix,
                        base.LogPrefix);
            }
        }

        public override bool DebugMode {
            get { return Owner.DebugMode; }
        }

        public override string[] DebugPatterns {
            get { return Owner.DebugPatterns; }
        }

        public bool AdminSecured {
            get {
                return Pass != null;
            }
        }

        public bool WriteSecured {
            get {
                if (Pass == null) return false;
                if (Pass.Writable) return false;
                return true;
            }
        }

        public bool CheckAdminPass(Pass pass, bool logError) {
            if (Pass == null) return true;
            if (Pass.CheckAdminPass(this, pass)) return true;

            if (logError) {
                Error("Invalid Admin Pass: Pass = {0}, pass = {1}", Pass, pass);
            }
            return false;
        }

        public bool CheckAdminPass(Pass pass) {
            return CheckAdminPass(pass, true);
        }

        public bool CheckWritePass(Pass pass, bool logError) {
            if (Pass == null) return true;
            if (Pass.CheckWritePass(this, pass)) return true;

            if (logError) {
                Error("Invalid Write Pass: Pass = {0}, pass = {1}", Pass, pass);
            }
            return false;
        }

        public bool CheckWritePass(Pass pass) {
            return CheckWritePass(pass, true);
        }

        public virtual void OnAdded() {}
        public virtual void OnRemoved() {}
    }
}
