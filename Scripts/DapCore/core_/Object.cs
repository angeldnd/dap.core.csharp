using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public interface IObject : ILogger {
        string Type { get; }
        int Revision { get; }

        bool DebugMode { get; }
        string[] DebugPatterns { get; }

        string LogPrefix { get; }

        bool AdminSecured { get; }
        bool WriteSecured { get; }
        bool CheckAdminPass(Pass pass);
        bool CheckWritePass(Pass pass);
    }

    public static class ObjectConsts {
        public const string KeyType = "type";
    }

    public abstract class Object : Logger, IObject, IBlockOwner {
        public static T As<T>(object obj, bool logError) where T : class, IObject {
            if (obj == null) return null;

            if (!(obj is T)) {
                if (logError) {
                    Log.Error("Type Mismatched: <{0}> -> {1}: {2}",
                                typeof(T).FullName, obj.GetType().FullName, obj);
                }
                return null;
            }
            return (T)obj;
        }

        public static T As<T>(object obj) where T : class, IObject {
            return As<T>(obj, true);
        }

        public static bool Is<T>(object obj) where T : class, IObject {
            return As<T>(obj, false) != null;
        }

        public virtual string Type {
            get { return null; }
        }

        private readonly Pass _Pass;
        protected Pass Pass {
            get { return _Pass; }
        }

        protected Object(Pass pass) {
            _Pass = pass;
        }

        private int _Revision = 0;
        public int Revision {
            get { return _Revision; }
        }

        protected void AdvanceRevision() {
            _Revision += 1;
        }

        public override string LogPrefix {
            get {
                return string.Format("[{0}] ({1}) ",
                            Type != null ? Type : GetType().Name, Revision);
            }
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

        //SILP:BLOCK_OWNER()
        private List<WeakBlock> _Blocks = null;                       //__SILP__
                                                                      //__SILP__
        public void AddBlock(WeakBlock block) {                       //__SILP__
            if (_Blocks == null) {                                    //__SILP__
                _Blocks = new List<WeakBlock>();                      //__SILP__
            }                                                         //__SILP__
            if (!_Blocks.Contains(block)) {                           //__SILP__
                _Blocks.Add(block);                                   //__SILP__
            }                                                         //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public void RemoveBlock(WeakBlock block) {                    //__SILP__
            if (_Blocks == null) {                                    //__SILP__
                return;                                               //__SILP__
            }                                                         //__SILP__
            int index = _Blocks.IndexOf(block);                       //__SILP__
            if (index >= 0) {                                         //__SILP__
                _Blocks.RemoveAt(index);                              //__SILP__
            }                                                         //__SILP__
        }                                                             //__SILP__
    }
}
