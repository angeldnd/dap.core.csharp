using System;
using System.Diagnostics;
using System.IO;

using UnityEngine;

namespace ADD.Utils {
    public class Log {
        public static int MAX_STACK_TRACK_NUM = 6;
        public static float FLUSH_DURATION = 60.0f; // flush every minute

        private static string LOG_ROOT = Application.dataPath + "/../Logs";
        private static string TIMESTAMP_FORMAT = "HH:mm:ss.fff";    //http://msdn.microsoft.com/en-us/library/8kb3ddd4.aspx

        private static string _LogDir = "";
        private static string _LogName = "";
        private static int _RunID = -1;
        private static int _LogDayOfYear = -1;
        private static string _LogFilePath = "";
        private static bool _LogDebug = true;
        private static bool _LogAutoFlush = false;
        private static StreamWriter _LogWriter = null;
        private static float _LastFlushTime = 0;

        public static bool LogDebug {
            get {
                return _LogDebug;
            }
        }

        private static void SetupLogRoot() {
            if (Application.platform == RuntimePlatform.IPhonePlayer ||
                Application.platform == RuntimePlatform.Android) {
                LOG_ROOT = Application.temporaryCachePath;
            }
        }

        public static void SetupLogging(string logDir, string logName, int runID, bool logDebug) {
            SetupLogRoot();

            Debug("SetupLogging: {0} {1} {2} {3}", logDir, logName, runID, logDebug);
            _LogDir = logDir;
            _LogName = logName;
            if (runID < 0) {
                _RunID = GetNextRunID(logDir, logName, 0);
            } else {
                _RunID = runID;
            }
            _LogDebug = logDebug;
            SetupLogWriter();
        }

        public static int GetNextRunID(string logDir, string logName, int startRunID) {
            int runID = startRunID;

            while (true) {
                if (!IsLogFileExist(logDir, logName, runID)) {
                    break;
                }
                runID++;
            }
            Debug("GetNextRunID: {0} {1} {2} -> {3}", logDir, logName, startRunID, runID);
            return runID;
        }

        public static void SetRunID(int runID) {
            Debug("SetRunID: {0}", runID);
            _RunID = runID;
            SetupLogWriter();
        }

        public static void SetLogDebug(bool logDebug) {
            _LogDebug = logDebug;
        }

        public static void SetLogAutoFlush(bool autoFlush) {
            _LogAutoFlush = autoFlush;
            SetupLogWriter();
        }

        public static void FlushLogging() {
            if (!_LogAutoFlush && _LogWriter != null) {
                _LogWriter.Flush();
            }
        }

        public static void StopLogging() {
            if (_LogWriter != null) {
                _LogWriter.Flush();
                _LogWriter.Close();
                _LogWriter = null;
            }
        }

        private static bool IsLogFileExist(string logDir, string logName, int runID) {
            var now = System.DateTime.UtcNow;
            string month = now.ToString("yyyy-MM");
            string dir = string.Format("{0}/{1}/{2}", LOG_ROOT, month, logDir);

            string date = now.ToString("yyyy-MM-dd");
            string logFilePath = string.Format("{0}/{1}_{2}_{3}.log", dir, date, logName, runID);
 
            try {
                if (!Directory.Exists(dir)) {
                    return false;
                }
                return File.Exists(logFilePath);
            } catch (Exception e) {
                Log.Error("IsLogFileExist: Get Exception: {0}", e);
                return false;
            }
        }

        private static void SetupLogWriter() {
            StopLogging();
            _LastFlushTime = Time.time;

            var now = System.DateTime.UtcNow;
            string month = now.ToString("yyyy-MM");
            string dir = string.Format("{0}/{1}/{2}", LOG_ROOT, month, _LogDir);

            _LogDayOfYear = now.DayOfYear;
            string date = now.ToString("yyyy-MM-dd");
            _LogFilePath = string.Format("{0}/{1}_{2}_{3}.log", dir, date, _LogName, _RunID);
 
            try {
                if (!Directory.Exists(dir)) {
                    Debug("CreateDirectory: {0}", dir);
                    Directory.CreateDirectory(dir);
                }
                _LogWriter = File.AppendText(_LogFilePath);
                Info("Start Logging: {0}", _LogFilePath);

                //Probably can't use AutoFlush here due to performance issue
                //need to check whether the system level cache make this workable
                //or need to flush periodically.
                _LogWriter.AutoFlush = _LogAutoFlush;
            } catch (Exception e) {
                _LogWriter = null;
                Error("Failed to create log writer: {0} : {1}", _LogFilePath, e);
            }
        }

        public static string Format(string format, params object[] values) {
            return System.String.Format(format, values);
        }

        public static void AddLog(string type, StackTrace stackTrace, string format, params object[] values) {
            var now = System.DateTime.UtcNow;

            string msg = format;
            if (values != null && values.Length > 0) {
                msg = string.Format(format, values);
            }

            string log = null;
            if (stackTrace != null) {
                log = string.Format("{0} [{1}] {2}\n{3}", now.ToString(TIMESTAMP_FORMAT), type, msg,
                                    FormatStackTrace(stackTrace, "\t", MAX_STACK_TRACK_NUM));
                UnityEngine.Debug.LogError(log);
            } else {
                log = string.Format("{0} [{1}] {2}", now.ToString(TIMESTAMP_FORMAT), type, msg);
                UnityEngine.Debug.Log(log);
            }

            if (_LogWriter != null) {
                if (now.DayOfYear != _LogDayOfYear) {
                    SetupLogWriter();
                    if (_LogWriter == null) {
                        return;
                    }
                }
                try {
                    _LogWriter.WriteLine(log);
                    if (Time.time - _LastFlushTime > FLUSH_DURATION) {
                        _LogWriter.Flush();
                        _LastFlushTime = Time.time;
                    }
                } catch (Exception e) {
                    _LogWriter = null;
                    Error("Failed to write log: {0} : {1}", _LogFilePath, e);
                    //TODO: maybe try to resume logging to file later.
                }
            }
        }

        public static string FormatStackTrace(StackTrace stackTrace, string prefix, int max) {
            StringWriter writer = new StringWriter();
            for (int i = 0; i< stackTrace.FrameCount; i++) {
                if (i >= max) break;
                StackFrame stackFrame = stackTrace.GetFrame(i);
                var method = stackFrame.GetMethod();
                string line = string.Format("{0}{1}.{2}:{3}() ({4}:{5})", prefix,
                                            method.ReflectedType.Namespace,
                                            method.ReflectedType.Name,
                                            method.Name,
                                            stackFrame.GetFileName(),
                                            stackFrame.GetFileLineNumber());
                writer.WriteLine(line);
            }
            return writer.ToString();
        }

        public static void Critical(string format, params object[] values) {
            StackTrace stackTrace = new StackTrace(1, true);

            AddLog("CRITICAL", stackTrace, format, values);

            FlushLogging();

            //Not throwing exception for now
            //throw new System.ArgumentException(string.Format(format, values));
        }

        public static void Error(string format, params object[] values) {
            StackTrace stackTrace = new StackTrace(1, true);

            AddLog("ERROR", stackTrace, format, values);
        }

        public static void Info(string format, params object[] values) {
            AddLog("INFO", null, format, values);
        }

        public static void Debug(string format, params object[] values) {
            if (_LogDebug) {
                AddLog("DEBUG", null, format, values);
            }
        }

        public static void Custom(string type, string format, params object[] values) {
            AddLog(type, null, format, values);
        }
    }
}

