using System;

namespace angeldnd.dap {
    public interface DapObject : Logger {
        string Type { get; }
        int Revision { get; } //Mainly For Debugging

        bool DebugMode { get; }
        string[] DebugPatterns { get; }
        string GetLogPrefix();
    }

    public struct DapObjectConsts {
        public const string KeyType = "type";
    }

    public abstract class BaseDapObject : DapLogger, DapObject {
        public virtual string Type {
            get { return null; }
        }

        private int _Revision = 0;
        public int Revision {
            get { return _Revision; }
        }

        protected void AdvanceRevision() {
            _Revision += 1;
        }

        public override string GetLogPrefix() {
            return string.Format("[{0}] ({1}) ", GetType().Name, Revision);
        }
    }
}
