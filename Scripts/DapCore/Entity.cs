using System;
using System.Diagnostics;
using System.IO;

using UnityEngine;

using ADD.Utils;

namespace ADD.Dap {
   public abstract class Entity : MonoBehaviour, Logger {
        //Keep them public so default editor inspector can be shown
        //TODO: Create custom editor so they can be private again
        public bool _DebugMode = false;
        public string[] _DebugPatterns = {""};

        public bool DebugMode {
            get { return _DebugMode; }
        }
        public string[] DebugPatterns {
            get { return _DebugPatterns; }
        }

        private DebugLogger _DebugLogger = DebugLogger.Instance;

        public bool LogDebug {
            get { return DebugMode; }
        }

        public string FullName {
            get {
                Transform t = this.transform;
                string fullName = "";
                while (t != null) {
                    if (string.IsNullOrEmpty(fullName)) {
                        fullName = t.name;
                    } else {
                        fullName = t + "/" + fullName;
                    }
                    t = t.parent;
                }
                return fullName;
            }
        }

        public virtual string GetLogPrefix() {
            return string.Format("[{0}] [{1}] ", GetType().Name, name);
        }

        public void Critical(string format, params object[] values) {
            if (_DebugMode) {
                _DebugLogger.Critical(GetLogPrefix() + string.Format(format, values));
            } else {
                Log.Critical(GetLogPrefix() + string.Format(format, values));
            }
        }

        public void Error(string format, params object[] values) {
            if (_DebugMode) {
                _DebugLogger.Error(GetLogPrefix() + string.Format(format, values));
            } else {
                Log.Error(GetLogPrefix() + string.Format(format, values));
            }
        }

        public void Info(string format, params object[] values) {
            if (_DebugMode) {
                _DebugLogger.LogWithPatterns("INFO", DebugPatterns,
                        GetLogPrefix() + _DebugLogger.GetMethodPrefix() + string.Format(format, values));
            } else {
                Log.Info(GetLogPrefix() + string.Format(format, values));
            }
        }

        public void Debug(string format, params object[] values) {
            if (_DebugMode) {
                _DebugLogger.LogWithPatterns("DEBUG", DebugPatterns,
                        GetLogPrefix() + _DebugLogger.GetMethodPrefix() + string.Format(format, values));
            } else {
                Log.Debug(GetLogPrefix() + string.Format(format, values));
            }
        }
    }
}
