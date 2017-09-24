using System;
using System.Collections;
using System.Collections.Generic;

using System.IO;
using System.Diagnostics;

namespace angeldnd.dap {
    [DapPriority(-1)]
    public class ConsoleLogProvider : LogProvider {
        public ConsoleLogProvider(bool logDebug) : base(logDebug) {
        }

        public override string ToString() {
            return string.Format("[<{0}>{1}]", GetType().FullName, LogDebug ? "(Debug)" : "");
        }

        protected override void OnAddLog(System.DateTime now, object source, string kind, string log, StackTrace stackTrace) {
            Console.WriteLine(log);
        }
    }
}

