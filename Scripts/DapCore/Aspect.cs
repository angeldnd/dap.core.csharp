using System;
using System.Diagnostics;
using System.IO;

using UnityEngine;

using ADD.Utils;

namespace ADD.Dap {
    public interface Aspect : Logger {
        Entity Entity { get; }
    }

    public abstract class BaseAspect : Aspect {
        //SILP: ASPECT_LOG_MIXIN()
        private DebugLogger _DebugLogger = DebugLogger.Instance;                                          //__SILP__
                                                                                                          //__SILP__
        public abstract Entity Entity { get; }                                                            //__SILP__
                                                                                                          //__SILP__
        public bool LogDebug {                                                                            //__SILP__
            get { return Entity != null && Entity.DebugMode; }                                            //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public virtual string GetLogPrefix() {                                                            //__SILP__
            if (Entity != null) {                                                                         //__SILP__
                return string.Format("{0}[{1}] ", Entity.GetLogPrefix(), GetType().Name);                 //__SILP__
            } else {                                                                                      //__SILP__
                return string.Format("[] [] [{0}] ", GetType().Name);                                     //__SILP__
            }                                                                                             //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public void Critical(string format, params object[] values) {                                     //__SILP__
            if (LogDebug) {                                                                               //__SILP__
                _DebugLogger.Critical(GetLogPrefix() + string.Format(format, values));                    //__SILP__
            } else {                                                                                      //__SILP__
                Log.Critical(GetLogPrefix() + string.Format(format, values));                             //__SILP__
            }                                                                                             //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public void Error(string format, params object[] values) {                                        //__SILP__
            if (LogDebug) {                                                                               //__SILP__
                _DebugLogger.Error(GetLogPrefix() + string.Format(format, values));                       //__SILP__
            } else {                                                                                      //__SILP__
                Log.Error(GetLogPrefix() + string.Format(format, values));                                //__SILP__
            }                                                                                             //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public void Info(string format, params object[] values) {                                         //__SILP__
            if (LogDebug) {                                                                               //__SILP__
                _DebugLogger.LogWithPatterns("INFO", Entity.DebugPatterns,                                //__SILP__
                        GetLogPrefix() + _DebugLogger.GetMethodPrefix() + string.Format(format, values)); //__SILP__
            } else {                                                                                      //__SILP__
                Log.Info(GetLogPrefix() + string.Format(format, values));                                 //__SILP__
            }                                                                                             //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public void Debug(string format, params object[] values) {                                        //__SILP__
            if (LogDebug) {                                                                               //__SILP__
                _DebugLogger.LogWithPatterns("DEBUG", Entity.DebugPatterns,                               //__SILP__
                        GetLogPrefix() + _DebugLogger.GetMethodPrefix() + string.Format(format, values)); //__SILP__
            } else {                                                                                      //__SILP__
                Log.Debug(GetLogPrefix() + string.Format(format, values));                                //__SILP__
            }                                                                                             //__SILP__
        }                                                                                                 //__SILP__
    }

    public abstract class MonoAspect : MonoBehaviour, Aspect {
        //SILP: ASPECT_LOG_MIXIN()
        private DebugLogger _DebugLogger = DebugLogger.Instance;                                          //__SILP__
                                                                                                          //__SILP__
        public abstract Entity Entity { get; }                                                            //__SILP__
                                                                                                          //__SILP__
        public bool LogDebug {                                                                            //__SILP__
            get { return Entity != null && Entity.DebugMode; }                                            //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public virtual string GetLogPrefix() {                                                            //__SILP__
            if (Entity != null) {                                                                         //__SILP__
                return string.Format("{0}[{1}] ", Entity.GetLogPrefix(), GetType().Name);                 //__SILP__
            } else {                                                                                      //__SILP__
                return string.Format("[] [] [{0}] ", GetType().Name);                                     //__SILP__
            }                                                                                             //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public void Critical(string format, params object[] values) {                                     //__SILP__
            if (LogDebug) {                                                                               //__SILP__
                _DebugLogger.Critical(GetLogPrefix() + string.Format(format, values));                    //__SILP__
            } else {                                                                                      //__SILP__
                Log.Critical(GetLogPrefix() + string.Format(format, values));                             //__SILP__
            }                                                                                             //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public void Error(string format, params object[] values) {                                        //__SILP__
            if (LogDebug) {                                                                               //__SILP__
                _DebugLogger.Error(GetLogPrefix() + string.Format(format, values));                       //__SILP__
            } else {                                                                                      //__SILP__
                Log.Error(GetLogPrefix() + string.Format(format, values));                                //__SILP__
            }                                                                                             //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public void Info(string format, params object[] values) {                                         //__SILP__
            if (LogDebug) {                                                                               //__SILP__
                _DebugLogger.LogWithPatterns("INFO", Entity.DebugPatterns,                                //__SILP__
                        GetLogPrefix() + _DebugLogger.GetMethodPrefix() + string.Format(format, values)); //__SILP__
            } else {                                                                                      //__SILP__
                Log.Info(GetLogPrefix() + string.Format(format, values));                                 //__SILP__
            }                                                                                             //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public void Debug(string format, params object[] values) {                                        //__SILP__
            if (LogDebug) {                                                                               //__SILP__
                _DebugLogger.LogWithPatterns("DEBUG", Entity.DebugPatterns,                               //__SILP__
                        GetLogPrefix() + _DebugLogger.GetMethodPrefix() + string.Format(format, values)); //__SILP__
            } else {                                                                                      //__SILP__
                Log.Debug(GetLogPrefix() + string.Format(format, values));                                //__SILP__
            }                                                                                             //__SILP__
        }                                                                                                 //__SILP__
    }

    public abstract class EntityAspect : Entity, Aspect {
        //SILP: ASPECT_LOG_MIXIN()
        private DebugLogger _DebugLogger = DebugLogger.Instance;                                          //__SILP__
                                                                                                          //__SILP__
        public abstract Entity Entity { get; }                                                            //__SILP__
                                                                                                          //__SILP__
        public bool LogDebug {                                                                            //__SILP__
            get { return Entity != null && Entity.DebugMode; }                                            //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public virtual string GetLogPrefix() {                                                            //__SILP__
            if (Entity != null) {                                                                         //__SILP__
                return string.Format("{0}[{1}] ", Entity.GetLogPrefix(), GetType().Name);                 //__SILP__
            } else {                                                                                      //__SILP__
                return string.Format("[] [] [{0}] ", GetType().Name);                                     //__SILP__
            }                                                                                             //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public void Critical(string format, params object[] values) {                                     //__SILP__
            if (LogDebug) {                                                                               //__SILP__
                _DebugLogger.Critical(GetLogPrefix() + string.Format(format, values));                    //__SILP__
            } else {                                                                                      //__SILP__
                Log.Critical(GetLogPrefix() + string.Format(format, values));                             //__SILP__
            }                                                                                             //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public void Error(string format, params object[] values) {                                        //__SILP__
            if (LogDebug) {                                                                               //__SILP__
                _DebugLogger.Error(GetLogPrefix() + string.Format(format, values));                       //__SILP__
            } else {                                                                                      //__SILP__
                Log.Error(GetLogPrefix() + string.Format(format, values));                                //__SILP__
            }                                                                                             //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public void Info(string format, params object[] values) {                                         //__SILP__
            if (LogDebug) {                                                                               //__SILP__
                _DebugLogger.LogWithPatterns("INFO", Entity.DebugPatterns,                                //__SILP__
                        GetLogPrefix() + _DebugLogger.GetMethodPrefix() + string.Format(format, values)); //__SILP__
            } else {                                                                                      //__SILP__
                Log.Info(GetLogPrefix() + string.Format(format, values));                                 //__SILP__
            }                                                                                             //__SILP__
        }                                                                                                 //__SILP__
                                                                                                          //__SILP__
        public void Debug(string format, params object[] values) {                                        //__SILP__
            if (LogDebug) {                                                                               //__SILP__
                _DebugLogger.LogWithPatterns("DEBUG", Entity.DebugPatterns,                               //__SILP__
                        GetLogPrefix() + _DebugLogger.GetMethodPrefix() + string.Format(format, values)); //__SILP__
            } else {                                                                                      //__SILP__
                Log.Debug(GetLogPrefix() + string.Format(format, values));                                //__SILP__
            }                                                                                             //__SILP__
        }                                                                                                 //__SILP__
    }
}
